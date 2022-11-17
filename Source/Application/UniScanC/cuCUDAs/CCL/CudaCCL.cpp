#include "CudaCCL.h"
#include "ccl_dpl.h"
#include "ccl_np.h"
#include "ccl_le.h"
#include "ccl_hm.h"
#include <chrono>
#include <npp.h>

chrono::time_point<chrono::steady_clock> start_Time;
void Function_Start()
{
	start_Time = std::chrono::high_resolution_clock::now();//::GetTickCount();
}

void Function_Stop(string s)
{
	cout << std::chrono::duration_cast<std::chrono::nanoseconds>(std::chrono::high_resolution_clock::now() - start_Time).count() / 1000 << " us - " << s << "\n";
}


void CudaCCL::ccl_dpl(BYTE * srcImage, int * dstImage, int W, int H, int degree_of_connectivity)
{
	m_Width = W;
	m_Height = H;

	int N = W * H;

	Ld = dstImage;
	Dd = srcImage;

	bool* md;
	cudaMalloc((void**)&md, sizeof(bool));

	int width = static_cast<int>(sqrt(static_cast<double>(N) / MAX_GPU_THREAD_NUM)) + 1;
	dim3 grid(width, width, 1);
	dim3 threads(MAX_GPU_THREAD_NUM, 1, 1);

	init_dpl << <grid, threads >> > (Ld, N);

	for (;;) {
		bool m = false;
		cudaMemcpy(md, &m, sizeof(bool), cudaMemcpyHostToDevice);
		for (int i = 0; i < 4; i++)
		{
			kerneldpl << <grid, threads >> > (i, Dd, Ld, md, N, W);

			if (degree_of_connectivity == 8)
				kernel8dpl << <grid, threads >> > (i, Dd, Ld, md, N, W);

			cudaMemcpy(&m, md, sizeof(bool), cudaMemcpyDeviceToHost);
		}

		if (!m)
			break;
	}
}

void CudaCCL::ccl_np(BYTE* srcImage, int* dstImage, int W, int H, int degree_of_connectivity)
{
	m_Width = W;
	m_Height = H;

	int N = W * H;

	Ld = dstImage;
	Dd = srcImage;

	bool* md;
	cudaMalloc((void**)&md, sizeof(bool));

	int width = static_cast<int>(sqrt(static_cast<double>(N) / MAX_GPU_THREAD_NUM)) + 1;
	dim3 grid(width, width, 1);
	dim3 threads(MAX_GPU_THREAD_NUM, 1, 1);

	init_np << <grid, threads >> > (Ld, N);

	for (;;) {
		bool m = false;
		//cudaMemcpy(md, &m, sizeof(bool), cudaMemcpyHostToDevice);
		cudaMemset(md, 0, sizeof(bool));

		if (degree_of_connectivity == 4)
			kernel << <grid, threads >> > (Dd, Ld, md, N, W);
		else
			kernel8 << <grid, threads >> > (Dd, Ld, md, N, W);

		//if (cudaGetLastError() != cudaSuccess)
		//	assert(false);

		cudaMemcpy(&m, md, sizeof(bool), cudaMemcpyDeviceToHost);
		if (!m)
			break;
	}
}

void CudaCCL::ccl_le(BYTE * srcImage, int * dstImage, int W, int H, int degree_of_connectivity)
{
	m_Width = W;
	m_Height = H;

	int N = W * H;

	Ld = dstImage;
	Dd = srcImage;

	cudaMalloc((void**)&Rd, sizeof(int) * N);

	bool* md;
	cudaMalloc((void**)&md, sizeof(bool));

	int width = static_cast<int>(sqrt(static_cast<double>(N) / MAX_GPU_THREAD_NUM)) + 1;
	dim3 grid(width, width, 1);
	dim3 threads(MAX_GPU_THREAD_NUM, 1, 1);

	init_le << <grid, threads >> > (Ld, Rd, N);

	for (;;) {
		bool m = false;
		cudaMemset(md, 0, sizeof(bool));

		if (degree_of_connectivity == 4) 
			scanning << <grid, threads >> > (Dd, Ld, Rd, md, N, W);
		else 
			scanning8 << <grid, threads >> > (Dd, Ld, Rd, md, N, W);

		cudaMemcpy(&m, md, sizeof(bool), cudaMemcpyDeviceToHost);

		if (m)
		{
			analysis << <grid, threads >> > (Dd, Ld, Rd, N);
			//cudaThreadSynchronize();
			labeling << <grid, threads >> > (Dd, Ld, Rd, N);
		}
		else
			break;
	}

	cudaFree(Rd);
}

int CudaCCL::ccl_hm(BYTE* binImage, int* labelImage, BYTE* maskImage, int W, int H)
{
	m_Width = W;
	m_Height = H;

	int thread_num = MAX_GPU_THREAD_NUM;

	int N = W * H;
	cudaMemsetAsync(labelImage, 0xFF, N * sizeof(int));
	cudaMemsetAsync(maskImage, 0, N * sizeof(BYTE));

	int root = sqrt(thread_num);
	int labelH = (root / 16) * 16;
	int labelW = thread_num / labelH;

	dim3 labeling_threads(labelW, labelH);
	dim3 labeling_grid((W % labeling_threads.x == 0 ? W / labeling_threads.x : W / labeling_threads.x + 1), (H % labeling_threads.y == 0 ? H / labeling_threads.y : H / labeling_threads.y + 1));
	
	kernel_mask_hm << <labeling_grid, labeling_threads >> > (binImage, maskImage, W, H);
	if (cudaSuccess != cudaGetLastError())
		return -1;

	int count_block_num = N % thread_num == 0 ? N / thread_num : N / thread_num + 1;

	kernel_labeling_hm << <labeling_grid, labeling_threads, labeling_threads.x * labeling_threads.y * sizeof(int) >> > (binImage, maskImage, labelImage, W, H);
	if (cudaSuccess != cudaGetLastError())
		return -1;

	int half_thread_num = thread_num / 2;
	dim3 labeling_horizental_thread(half_thread_num, 2);
	dim3 labeling_horizental_grid(W % half_thread_num == 0 ? W / half_thread_num : W / half_thread_num + 1, H % labeling_threads.y == 0 ? H / labeling_threads.y : H / labeling_threads.y + 1);

	dim3 labeling_vertical_thread(2, half_thread_num);
	dim3 labeling_vertical_grid(W % labeling_threads.x == 0 ? W / labeling_threads.x : W / labeling_threads.x + 1, H % half_thread_num == 0 ? H / half_thread_num : H / half_thread_num + 1);

	init_label_map << < count_block_num, thread_num >> > (maskImage, labelImage, N);

	bool changed = false;
	bool* d_changed;
	cudaMalloc(&d_changed, 1);
	cudaStream_t streamV, streamH;
	cudaStreamCreate(&streamV); 
	cudaStreamCreate(&streamH);

	do
	{
		cudaMemset(d_changed, 0, 1);
		find_vertical_hm << <labeling_vertical_grid, labeling_vertical_thread, thread_num * sizeof(int) >> > (maskImage, labelImage, W, H, labelW, d_changed);
		find_horizental_hm << <labeling_horizental_grid, labeling_horizental_thread, thread_num * sizeof(int) >> > (maskImage, labelImage, W, H, labelH, d_changed);
		
		cudaMemcpy(&changed, d_changed, 1, cudaMemcpyDeviceToHost);

		if (changed == true)
			indexing_hm << <count_block_num, thread_num >> > (maskImage, labelImage, N);

		if (cudaSuccess != cudaGetLastError())
		{
			cudaFree(d_changed);
			return -1; 
		}
	} while (changed == true);

	cudaFree(d_changed); 

	cudaStream_t streamI, streamC;
	cudaStreamCreate(&streamI);
	cudaStreamCreate(&streamC);

	indexing_hm << <count_block_num, thread_num >> > (maskImage, labelImage, N);

	int* dCount;
	int* i_dCount;

	cudaMalloc(&dCount, count_block_num * sizeof(int));
	cudaMalloc(&i_dCount, count_block_num * sizeof(int));

	cudaMemset(dCount, 0, count_block_num * sizeof(int));

	counting_root_hm << <count_block_num, thread_num, 0, streamC >> > (labelImage, N, dCount);
	if (cudaSuccess != cudaGetLastError())
	{
		cudaFree(dCount);
		cudaFree(i_dCount);
		return -1;
	}
		
	int* count = new int[count_block_num];
	int* i_count = new int[count_block_num];
	cudaMemcpy(count, dCount, count_block_num * sizeof(int), cudaMemcpyDeviceToHost);;

	int total = count[0];
	i_count[0] = 0;
	for (int i = 1; i < count_block_num; i++)
	{
		total += count[i];
		i_count[i] = count[i - 1] + i_count[i - 1];
	}

	delete[] count;
	cudaFree(dCount);
	cudaMemcpy(i_dCount, i_count, count_block_num * sizeof(int), cudaMemcpyHostToDevice);;
	delete[] i_count;

	labeling_root_hm << <count_block_num, thread_num >> > (labelImage, N, i_dCount);
	if (cudaSuccess != cudaGetLastError())
	{
		cudaFree(i_dCount);
		return -1;
	}

	cudaFree(i_dCount);

	labeling_child_hm << <count_block_num, thread_num >> > (labelImage, N);
	if (cudaSuccess != cudaGetLastError())
		return -1;

	return total;
}

bool CudaCCL::blob_hm(BYTE * sourceImage, int * labelImage, int W, int H, int count, unsigned int* areaArray, unsigned int* xMinArray, unsigned int* xMaxArray, unsigned int* yMinArray, unsigned int* yMaxArray, unsigned int* vMinArray, unsigned int* vMaxArray, float* vMeanArray)
{
	m_Width = W;
	m_Height = H;
	int N = W * H;

	unsigned int* dAreaArray;

	unsigned int* dXMinArray;
	unsigned int* dXMaxArray;

	unsigned int* dYMaxArray;
	unsigned int* dYMinArray;

	unsigned int* dVMinArray;
	unsigned int* dVMaxArray;
	float* dVMeanArray;

	cudaMalloc(&dAreaArray, count * sizeof(unsigned int));

	cudaMalloc(&dXMinArray, count * sizeof(unsigned int));
	cudaMalloc(&dXMaxArray, count * sizeof(unsigned int));

	cudaMalloc(&dYMaxArray, count * sizeof(unsigned int));
	cudaMalloc(&dYMinArray, count * sizeof(unsigned int));

	cudaMalloc(&dVMinArray, count * sizeof(unsigned int));
	cudaMalloc(&dVMaxArray, count * sizeof(unsigned int));
	cudaMalloc(&dVMeanArray, count * sizeof(float));

	cudaMemsetAsync(dAreaArray, 0, count * sizeof(unsigned int));

	cudaMemsetAsync(dXMinArray, 0xFF, count * sizeof(unsigned int));
	cudaMemsetAsync(dXMaxArray, 0, count * sizeof(unsigned int));

	cudaMemsetAsync(dYMinArray, 0xFF, count * sizeof(unsigned int));
	cudaMemsetAsync(dYMaxArray, 0, count * sizeof(unsigned int));

	cudaMemsetAsync(dVMinArray, 0xFF, count * sizeof(BYTE));
	cudaMemsetAsync(dVMaxArray, 0, count * sizeof(BYTE));
	cudaMemsetAsync(dVMeanArray, 0, count * sizeof(float));

	kernel_blob_hm << <N % MAX_GPU_THREAD_NUM == 0 ? N / MAX_GPU_THREAD_NUM : N / MAX_GPU_THREAD_NUM + 1, MAX_GPU_THREAD_NUM >> > (sourceImage, labelImage, N, W, dAreaArray, dXMinArray, dXMaxArray, dYMinArray, dYMaxArray, dVMinArray, dVMaxArray, dVMeanArray);

	cudaMemcpyAsync(areaArray, dAreaArray, count * sizeof(unsigned int), cudaMemcpyDeviceToHost);

	cudaMemcpyAsync(xMinArray, dXMinArray, count * sizeof(unsigned int), cudaMemcpyDeviceToHost);
	cudaMemcpyAsync(xMaxArray, dXMaxArray, count * sizeof(unsigned int), cudaMemcpyDeviceToHost);

	cudaMemcpyAsync(yMinArray, dYMinArray, count * sizeof(unsigned int), cudaMemcpyDeviceToHost);
	cudaMemcpyAsync(yMaxArray, dYMaxArray, count * sizeof(unsigned int), cudaMemcpyDeviceToHost);

	cudaMemcpyAsync(vMinArray, dVMinArray, count * sizeof(unsigned int), cudaMemcpyDeviceToHost);
	cudaMemcpyAsync(vMaxArray, dVMaxArray, count * sizeof(unsigned int), cudaMemcpyDeviceToHost);
	cudaMemcpy(vMeanArray, dVMeanArray, count * sizeof(float), cudaMemcpyDeviceToHost);

	for (int i = 0; i < count; i++)
		vMeanArray[i] /= areaArray[i];

	cudaFree(dAreaArray);
	cudaFree(dXMinArray);
	cudaFree(dXMaxArray);
	cudaFree(dYMaxArray);
	cudaFree(dYMinArray);
	cudaFree(dVMinArray);
	cudaFree(dVMaxArray);
	cudaFree(dVMeanArray);

	if (cudaSuccess != cudaGetLastError())
		return false;

	return true;
}
