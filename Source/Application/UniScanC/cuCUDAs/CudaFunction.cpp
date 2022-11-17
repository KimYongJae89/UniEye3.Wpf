#include "CudaFunction.h"
#include "DeviceFunction.h"
#include "HostFunction.h"
#include "CCL/CudaCCL.h"
#include "Morphology/CudaMorphology.h"
#include "NPP/NppProcessing.h"
#include "helper_image.h"
#include "Ransac/CudaRansac.h"
#include "Math/CudaMath.h"

#include <set>
#include <map>
#include <vector>

using namespace std;

static map<int, CudaImage*> cudaImageMap;
static map<int, CudaBlobList*> cudaBlobMap;

cudaError_t cudaLastResult;

CudaImage* GetCudaImage(UINT imageId)
{
	auto itr = cudaImageMap.find(imageId);
	if (itr == cudaImageMap.end())
		return NULL;

	return itr->second;
}

////////////////////////////////////////////////////////////////////////////////////

bool CUDA_INITIALIZE(int& gpuNo)
{
	cudaDeviceReset();

	// Choose which GPU to run on, change this on a multi-GPU system.
	int count = 0;
	cudaLastResult = cudaGetDeviceCount(&count);
	if (cudaLastResult != cudaSuccess)
		return false;

	if (gpuNo >= count)
		return false;

	cudaLastResult = cudaSetDevice(gpuNo);
	if (cudaLastResult != cudaSuccess)
		return false;

	cudaError_t cudaStatus;
	CUDA_ERROR_CHECK(cudaStatus);

	return true;
}

void CUDA_RELEASE()
{
	for (auto itr = cudaImageMap.begin();
		itr != cudaImageMap.end();
		itr++)
	{
		delete itr->second;
	}

	cudaImageMap.clear();

	cudaDeviceReset();

	UNIQUE_IMAGE_NO = 1;
}

void CUDA_THREAD_NUM(int threadNum)
{
	MAX_GPU_THREAD_NUM = threadNum;
}

void CUDA_WAIT()
{
	cudaDeviceSynchronize();
}

UINT CUDA_CREATE_IMAGE(int width, int height, int depth)
{
	int id = UNIQUE_IMAGE_NO++;

	CudaImage* cudaImage = new CudaImage(id, 0);
	if (!cudaImage->Alloc(width, height, depth))
	{
		delete cudaImage;
		return 0;
	}

	cudaError_t cudaStatus;
	CUDA_ERROR_CHECK(cudaStatus);

	cudaImageMap[id] = cudaImage;
	return id;
}

bool CUDA_CLIP_IMAGE(UINT srcImage, UINT dstImage, int x, int y, int width, int height)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	if (!cudaSrcImage)
		return false;

	auto cudaDstImage = GetCudaImage(dstImage);
	if (!cudaDstImage)
		return false;

	int blockSize = static_cast<int>(sqrt(static_cast<double>(cudaSrcImage->Width * cudaSrcImage->Height) / MAX_GPU_THREAD_NUM)) + 1;
	dim3 grid(blockSize, blockSize, 1);
	dim3 threads(MAX_GPU_THREAD_NUM, 1, 1);

	devClip << <grid, threads >> > ((BYTE*)cudaSrcImage->pImageBuffer, (BYTE*)cudaDstImage->pImageBuffer, 
		cudaSrcImage->Width, cudaSrcImage->Height, 
		x, y, width, height);

	cudaError_t cudaStatus;
	CUDA_ERROR_CHECK(cudaStatus);

	return cudaStatus != cudaSuccess;
}

bool CUDA_SET_IMAGE(UINT image, void* pImageBuffer)
{
	auto cudaImage = GetCudaImage(image);
	if (!cudaImage)
		return false;

	return cudaImage->SetImage(pImageBuffer);
}

bool CUDA_CLEAR_IMAGE(UINT image)
{
	auto cudaImage = GetCudaImage(image);
	if (!cudaImage)
		return false;

	return cudaImage->ClearImage();
}

void CUDA_FREE_IMAGE(UINT image)
{
	auto cudaImage = GetCudaImage(image);
	if (!cudaImage)
		return;

	delete cudaImage;
	cudaImageMap.erase(image);
}

bool CUDA_GET_IMAGE(UINT image, void* pDstBuffer)
{
	auto cudaSrcImage = GetCudaImage(image);
	if (!cudaSrcImage)
		return false;

	return cudaSrcImage->GetImage(pDstBuffer);
}


void CUDA_SET_ROI(UINT image, double x, double y, double width, double height)
{
	auto cudaSrcImage = GetCudaImage(image);
	if (!cudaSrcImage)
		return;

	cudaSrcImage->SetImageROI(x, y, width, height);
}

void CUDA_RESET_ROI(UINT image)
{
	auto cudaSrcImage = GetCudaImage(image);
	if (!cudaSrcImage)
		return;

	cudaSrcImage->ResetImageROI();
}

#include <cassert>

bool CUDA_CREATE_PROFILE(UINT srcImage)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	if (!cudaSrcImage)
		return false;

	int blockNum = (cudaSrcImage->Width + MAX_GPU_THREAD_NUM - 1) / MAX_GPU_THREAD_NUM;
	devProfile <<<blockNum, MAX_GPU_THREAD_NUM>>> ((BYTE*)cudaSrcImage->pImageBuffer, cudaSrcImage->pProfile, cudaSrcImage->Width, cudaSrcImage->Height);
	
	cudaError_t cudaStatus;
	CUDA_ERROR_CHECK(cudaStatus);

	return cudaStatus == cudaSuccess;
}

bool CUDA_AREA_AVERAGE(UINT srcImage, int areaCount, float* hostAverage)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	if (!cudaSrcImage)
		return false;

	float* devAverage;
	cudaMalloc(&devAverage, sizeof(float) * areaCount);
	cudaMemset(devAverage, 0, sizeof(float) * areaCount);

	//int width = (cudaSrcImage->Width / (float)areaCount) + 0.5;
	dim3 grid = GetThreadGrid(cudaSrcImage->Width, cudaSrcImage->Height, MAX_GPU_THREAD_NUM, 1);
	dim3 thread(MAX_GPU_THREAD_NUM, 1);

	int* mutex;
	cudaMalloc(&mutex, sizeof(int));
	cudaMemset(mutex, 0, sizeof(int));

	devAreaSum<< <grid, thread >> > ((BYTE*)cudaSrcImage->pImageBuffer, cudaSrcImage->Width, cudaSrcImage->Height, areaCount, devAverage, mutex);

	cudaMemcpy(hostAverage, devAverage, areaCount * sizeof(float), cudaMemcpyDeviceToHost);
	cudaFree(devAverage);

	cudaError_t cudaStatus;
	CUDA_ERROR_CHECK(cudaStatus);

	return cudaStatus == cudaSuccess;
}

bool CUDA_CREATE_LABEL_BUFFER(UINT srcImage)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	if (!cudaSrcImage)
		return false;

	int size = cudaSrcImage->Width * cudaSrcImage->Height;

	if (cudaSrcImage->plabelBuffer == NULL)
	{
		cudaSrcImage->plabelBuffer = new LabelBuffer();

		if (cudaSuccess != cudaMalloc(&cudaSrcImage->plabelBuffer->pLabel, size * sizeof(int)))
			return false;

		if (cudaSuccess != cudaMalloc(&cudaSrcImage->plabelBuffer->pMask, size * sizeof(BYTE)))
			return false;
	}

	return true;
}

bool CUDA_CREATE_FFT_BUFFER(UINT srcImage)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	if (!cudaSrcImage)
		return false;

	cudaError_t cudaStatus;
	CUDA_ERROR_CHECK(cudaStatus);

	//auto result = cudaMalloc(&(cudaSrcImage->pComplexBuffer), 16384 * 16384 * sizeof(cufftComplex));
	if (cudaSuccess != cudaMalloc((void**)(&cudaSrcImage->pComplexBuffer), cudaSrcImage->Height * (cudaSrcImage->Width + 1) * sizeof(cufftComplex)))
		return false;

	CUDA_ERROR_CHECK(cudaStatus);

	if (CUFFT_SUCCESS != cufftPlan2d(&cudaSrcImage->forwardPlan, cudaSrcImage->Height, cudaSrcImage->Width, CUFFT_R2C))
		return false;
	
	CUDA_ERROR_CHECK(cudaStatus);

	if (CUFFT_SUCCESS != cufftPlan2d(&cudaSrcImage->inversePlan, cudaSrcImage->Height, cudaSrcImage->Width, CUFFT_C2R))
		return false;

	CUDA_ERROR_CHECK(cudaStatus);

	return true;
}

bool CUDA_BINARIZE(UINT srcImage, UINT dstImage, float lower, float upper, bool inverse)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	auto cudaDstImage = GetCudaImage(dstImage);

	if (!cudaSrcImage || !cudaDstImage)
		return false;

	// 이미지 사이즈 확인
	if ((cudaSrcImage->Width != cudaDstImage->Width)
		|| (cudaSrcImage->Height != cudaDstImage->Height))
		return false;

	int width = static_cast<int>(sqrt(static_cast<double>(cudaSrcImage->Width * cudaSrcImage->Height) / MAX_GPU_THREAD_NUM)) + 1;
	dim3 grid(width, width, 1);
	dim3 threads(MAX_GPU_THREAD_NUM, 1, 1);

	BYTE* pSrcBuffer = (BYTE*)cudaSrcImage->pImageBuffer;
	BYTE* pDstBuffer = (BYTE*)cudaDstImage->pImageBuffer;

	devBinarize << <grid, threads>> > (pSrcBuffer, pDstBuffer, cudaSrcImage->Width, cudaSrcImage->Height, lower, upper, inverse, cudaSrcImage->pRoiRect);
	
	//cudaError_t cudaStatus = cudaDeviceSynchronize();
	//assert(cudaStatus == cudaSuccess);

	cudaError_t cudaStatus;
	CUDA_ERROR_CHECK(cudaStatus);

	return cudaStatus == cudaSuccess;
}

bool CUDA_BINARIZE_LOWER(UINT srcImage, UINT dstImage, float lower, bool inverse)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	auto cudaDstImage = GetCudaImage(dstImage);

	if (!cudaSrcImage || !cudaDstImage)
		return false;

	// 이미지 사이즈 확인
	if ((cudaSrcImage->Width != cudaDstImage->Width)
		|| (cudaSrcImage->Height != cudaDstImage->Height))
		return false;

	int blockSize = sqrt(MAX_GPU_THREAD_NUM);

	int gridWidth = cudaSrcImage->Width %  blockSize == 0 ? cudaSrcImage->Width / blockSize : cudaSrcImage->Width / blockSize + 1;
	int gridHeight = cudaSrcImage->Height %  blockSize == 0 ? cudaSrcImage->Height / blockSize : cudaSrcImage->Height / blockSize + 1;

	dim3 grid(gridWidth, gridHeight, 1);
	dim3 block(blockSize, blockSize, 1);

	BYTE* pSrcBuffer = (BYTE*)cudaSrcImage->pImageBuffer;
	BYTE* pDstBuffer = (BYTE*)cudaDstImage->pImageBuffer;

	devLowerBinarize << <grid, block >> > (pSrcBuffer, pDstBuffer, cudaSrcImage->Width, cudaSrcImage->Height, lower, inverse, cudaSrcImage->pRoiRect);

	cudaError_t cudaStatus;
	CUDA_ERROR_CHECK(cudaStatus);

	return cudaStatus == cudaSuccess;
}

bool CUDA_BINARIZE_UPPER(UINT srcImage, UINT dstImage, float upper, bool inverse)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	auto cudaDstImage = GetCudaImage(dstImage);

	if (!cudaSrcImage || !cudaDstImage)
		return false;

	// 이미지 사이즈 확인
	if ((cudaSrcImage->Width != cudaDstImage->Width)
		|| (cudaSrcImage->Height != cudaDstImage->Height))
		return false;

	int width = static_cast<int>(sqrt(static_cast<double>(cudaSrcImage->Width * cudaSrcImage->Height) / MAX_GPU_THREAD_NUM)) + 1;
	dim3 grid(width, width, 1);
	dim3 threads(MAX_GPU_THREAD_NUM, 1, 1);

	BYTE* pSrcBuffer = (BYTE*)cudaSrcImage->pImageBuffer;
	BYTE* pDstBuffer = (BYTE*)cudaDstImage->pImageBuffer;

	devUpperBinarize << <grid, threads >> > (pSrcBuffer, pDstBuffer, cudaSrcImage->Width, cudaSrcImage->Height, upper, inverse, cudaSrcImage->pRoiRect);
	
	cudaError_t cudaStatus;
	CUDA_ERROR_CHECK(cudaStatus);

	return cudaStatus == cudaSuccess;
}

bool CUDA_ADAPTIVE_BINARIZE(UINT srcImage, UINT dstImage, float lower, float upper, bool inverse)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	auto cudaDstImage = GetCudaImage(dstImage);

	if (!cudaSrcImage || !cudaDstImage)
		return false;

	// 이미지 사이즈 확인
	if ((cudaSrcImage->Width != cudaDstImage->Width)
		|| (cudaSrcImage->Height != cudaDstImage->Height))
		return false;
	
	int size = cudaSrcImage->Width * cudaSrcImage->Height;

	int width = static_cast<int>(sqrt(static_cast<double>(size) / MAX_GPU_THREAD_NUM)) + 1;
	dim3 grid(width, width, 1);
	dim3 threads(MAX_GPU_THREAD_NUM, 1, 1);

	BYTE* pSrcBuffer = (BYTE*)cudaSrcImage->pImageBuffer;
	BYTE* pDstBuffer = (BYTE*)cudaDstImage->pImageBuffer;

	devAdaptiveBinarize <<<grid, threads >>> (pSrcBuffer, pDstBuffer, cudaSrcImage->pProfile, cudaSrcImage->Width, cudaSrcImage->Height, lower, upper, inverse, cudaSrcImage->pRoiRect);
	
	cudaError_t cudaStatus;
	CUDA_ERROR_CHECK(cudaStatus);

	return cudaStatus == cudaSuccess;
}

bool CUDA_ADAPTIVE_BINARIZE_LOWER(UINT srcImage, UINT dstImage, float lower, bool inverse)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	auto cudaDstImage = GetCudaImage(dstImage);

	if (!cudaSrcImage || !cudaDstImage)
		return false;

	// 이미지 사이즈 확인
	if ((cudaSrcImage->Width != cudaDstImage->Width)
		|| (cudaSrcImage->Height != cudaDstImage->Height))
		return false;

	int size = cudaSrcImage->Width * cudaSrcImage->Height;

	int width = static_cast<int>(sqrt(static_cast<double>(size) / MAX_GPU_THREAD_NUM)) + 1;
	dim3 grid(width, width, 1);
	dim3 threads(MAX_GPU_THREAD_NUM, 1, 1);

	BYTE* pSrcBuffer = (BYTE*)cudaSrcImage->pImageBuffer;
	BYTE* pDstBuffer = (BYTE*)cudaDstImage->pImageBuffer;

	devLowerAdaptiveBinarize <<<grid, threads>>> (pSrcBuffer, pDstBuffer, cudaSrcImage->pProfile, cudaSrcImage->Width, cudaSrcImage->Height, lower, inverse, cudaSrcImage->pRoiRect);
	
	cudaError_t cudaStatus;
	CUDA_ERROR_CHECK(cudaStatus);

	cudaDeviceSynchronize();

	return cudaStatus == cudaSuccess;
}

bool CUDA_ADAPTIVE_BINARIZE_UPPER(UINT srcImage, UINT dstImage, float upper, bool inverse)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	auto cudaDstImage = GetCudaImage(dstImage);

	if (!cudaSrcImage || !cudaDstImage)
		return false;

	// 이미지 사이즈 확인
	if ((cudaSrcImage->Width != cudaDstImage->Width)
		|| (cudaSrcImage->Height != cudaDstImage->Height))
		return false;

	int size = cudaSrcImage->Width * cudaSrcImage->Height;

	int width = static_cast<int>(sqrt(static_cast<double>(size) / MAX_GPU_THREAD_NUM)) + 1;
	dim3 grid(width, width, 1);
	dim3 threads(MAX_GPU_THREAD_NUM, 1, 1);

	BYTE* pSrcBuffer = (BYTE*)cudaSrcImage->pImageBuffer;
	BYTE* pDstBuffer = (BYTE*)cudaDstImage->pImageBuffer;

	devUpperAdaptiveBinarize << <grid, threads >> > (pSrcBuffer, pDstBuffer, cudaSrcImage->pProfile, cudaSrcImage->Width, cudaSrcImage->Height, upper, inverse, cudaSrcImage->pRoiRect);

	cudaError_t cudaStatus;
	CUDA_ERROR_CHECK(cudaStatus);

	cudaDeviceSynchronize();

	return cudaStatus == cudaSuccess;
}

bool CUDA_AREA_BINARIZE(UINT srcImage, UINT dstImage, 
	int areaCount, 
	float* hostAverage, 
	int lowThreshold, int highThreshold, 
	bool inverse)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	auto cudaDstImage = GetCudaImage(dstImage);

	if (!cudaSrcImage || !cudaDstImage)
		return false;

	// 이미지 사이즈 확인
	if ((cudaSrcImage->Width != cudaDstImage->Width)
		|| (cudaSrcImage->Height != cudaDstImage->Height))
		return false;

	float* devAverage;
	cudaMalloc(&devAverage, sizeof(float) * areaCount);
	cudaMemcpy(devAverage, hostAverage, areaCount * sizeof(float), cudaMemcpyHostToDevice);

	int size = cudaSrcImage->Width * cudaSrcImage->Height;

	int width = static_cast<int>(sqrt(static_cast<double>(size) / MAX_GPU_THREAD_NUM)) + 1;
	dim3 grid(width, width, 1);
	dim3 threads(MAX_GPU_THREAD_NUM, 1, 1);

	 devAreaBinarize << <grid, threads >> > 
		 ((BYTE*)cudaSrcImage->pImageBuffer, (BYTE*)cudaDstImage->pImageBuffer, 
			 areaCount, devAverage,
			 cudaSrcImage->Width, cudaSrcImage->Height, 
			 lowThreshold, highThreshold, inverse, cudaSrcImage->pRoiRect);

	cudaFree(devAverage);

	cudaError_t cudaStatus;
	CUDA_ERROR_CHECK(cudaStatus);

	return cudaStatus == cudaSuccess;
}


bool CUDA_EDGE_DETECT(UINT srcImage, EdgeSearchDirection dir, int threshold, int* startPos, int* endPos)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	if (!cudaSrcImage)
		return false;

	*startPos = cudaSrcImage->Width - 1;
	*endPos = 0;

	int range = cudaSrcImage->Width;
	float* pProfile = new float[cudaSrcImage->Width];
	cudaSrcImage->GetProfile(pProfile);

	return hostEdgeDetectX(pProfile, 0, range, range, threshold, startPos, endPos, 1);
}

#include <thrust/unique.h>

int CUDA_LABELING(UINT binImage)
{
	auto cudaBinImage = GetCudaImage(binImage);
	
	if (!cudaBinImage)
		return -1;

	int width = cudaBinImage->Width;
	int height = cudaBinImage->Height;

	if (cudaBinImage->plabelBuffer == NULL)
		return -1;

	LabelBuffer* labelBuffer = cudaBinImage->plabelBuffer;

	CudaCCL ccl;
	return ccl.ccl_hm((BYTE*)cudaBinImage->pImageBuffer, (int*)labelBuffer->pLabel, (BYTE*)labelBuffer->pMask, width, height);

	//// Device array -> Device vector
	//thrust::device_vector<int> imageMap((int*)cudaLabelImage->pImageBuffer, (int*)cudaLabelImage->pImageBuffer + (width * height));

	//// Remove duplicate pairs
	//auto newEnd = thrust::unique(imageMap.begin(), imageMap.end());

	//// Trim the vectors
	//imageMap.erase(newEnd, imageMap.end());
	/////

	//int* imageData = imageMap.data().get();
	//int count = imageMap.size();

}

bool CUDA_BLOBING(UINT binImage, UINT srcImage, int count,
	UINT* areaArray, UINT* xMinArray, UINT* xMaxArray, UINT* yMinArray, UINT* yMaxArray,
	UINT* vMinArray, UINT* vMaxArray, float* vMeanArray)
{
	auto cudaBinmage = GetCudaImage(binImage);
	auto cudaSrcImage = GetCudaImage(srcImage);

	if (!cudaBinmage)
		return false;

	int width = cudaBinmage->Width;
	int height = cudaBinmage->Height;

	if (cudaBinmage->plabelBuffer == NULL)
		return false;

	CudaCCL ccl;
	return ccl.blob_hm((BYTE*)cudaSrcImage->pImageBuffer, (int*)cudaBinmage->plabelBuffer->pLabel, width, height, count, areaArray, xMinArray, xMaxArray, yMinArray, yMaxArray, vMinArray, vMaxArray, vMeanArray);
}

bool CUDA_MORPHOLOGY_ERODE(UINT srcImage, UINT dstImage, int maskSize)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	auto cudaDstImage = GetCudaImage(dstImage);

	if (!cudaSrcImage || !cudaDstImage)
		return false;

	// 이미지 사이즈 확인
	if ((cudaSrcImage->Width != cudaDstImage->Width)
		|| (cudaSrcImage->Height != cudaDstImage->Height))
		return false;

	CudaMorphology::Erode(cudaSrcImage, cudaDstImage, maskSize);

	return true;
}

bool CUDA_MORPHOLOGY_DILATE(UINT srcImage, UINT dstImage, int maskSize)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	auto cudaDstImage = GetCudaImage(dstImage);

	if (!cudaSrcImage || !cudaDstImage)
		return false;

	// 이미지 사이즈 확인
	if ((cudaSrcImage->Width != cudaDstImage->Width)
		|| (cudaSrcImage->Height != cudaDstImage->Height))
		return false;

	CudaMorphology::Dilate(cudaSrcImage, cudaDstImage, maskSize);

	return true;
}

bool CUDA_MORPHOLOGY_OPEN(UINT srcImage, UINT dstImage, int maskSize)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	auto cudaDstImage = GetCudaImage(dstImage);

	if (!cudaSrcImage || !cudaDstImage)
		return false;

	// 이미지 사이즈 확인
	if ((cudaSrcImage->Width != cudaDstImage->Width)
		|| (cudaSrcImage->Height != cudaDstImage->Height))
		return false;

	CudaMorphology::Open(cudaSrcImage, cudaDstImage, maskSize);

	return true;
}

bool CUDA_MORPHOLOGY_CLOSE(UINT srcImage, UINT dstImage, int maskSize)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	auto cudaDstImage = GetCudaImage(dstImage);

	if (!cudaSrcImage || !cudaDstImage)
		return false;

	// 이미지 사이즈 확인
	if ((cudaSrcImage->Width != cudaDstImage->Width)
		|| (cudaSrcImage->Height != cudaDstImage->Height))
		return false;

	CudaMorphology::Close(cudaSrcImage, cudaDstImage, maskSize);

	return true;
}

bool CUDA_MORPHOLOGY_THINNING(UINT srcImage)
{
	auto cudaSrcImage = GetCudaImage(srcImage);

	if (!cudaSrcImage)
		return false;

	CudaMorphology::Thinning(cudaSrcImage);

	return true;
}

bool CUDA_SOBEL(UINT srcImage, UINT dstImage)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	auto cudaDstImage = GetCudaImage(dstImage);

	if (!cudaSrcImage || !cudaDstImage)
		return false;

	// 이미지 사이즈 확인
	if ((cudaSrcImage->Width != cudaDstImage->Width)
		|| (cudaSrcImage->Height != cudaDstImage->Height))
		return false;

	CudaMorphology::Sobel(cudaSrcImage, cudaDstImage);

	return true;
}

bool CUDA_CANNY(UINT srcImage, UINT dstImage, int lowThreshold, int highThreshold)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	auto cudaDstImage = GetCudaImage(dstImage);

	if (!cudaSrcImage || !cudaDstImage)
		return false;

	// 이미지 사이즈 확인
	if ((cudaSrcImage->Width != cudaDstImage->Width)
		|| (cudaSrcImage->Height != cudaDstImage->Height))
		return false;

	NppProcessing::Canny(cudaSrcImage, cudaDstImage, lowThreshold, highThreshold);

	return true;
}

bool Save(UINT srcImage, char* fileName)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	if (!cudaSrcImage)
		return false;

	UINT w = (UINT)cudaSrcImage->Width;
	UINT h = (UINT)cudaSrcImage->Height;

	BYTE* pBuffer = new BYTE[w * h];

	if (!cudaSrcImage->GetImage(pBuffer))
		return false;

	return sdkSavePGM<BYTE>(fileName, pBuffer, w, h);
}

bool NoiseRemoval(UINT srcImage, double radius, double threshold)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	if (!cudaSrcImage)
		return false;
	
	cudaError_t cudaStatus;

	CUDA_ERROR_CHECK(cudaStatus);

	if (CUFFT_SUCCESS != cufftExecR2C(cudaSrcImage->forwardPlan, (float*)cudaSrcImage->pImageBuffer, cudaSrcImage->pComplexBuffer))
		return false;

	cudaDeviceSynchronize();
	CUDA_ERROR_CHECK(cudaStatus);

	int width = cudaSrcImage->Width / 2 + 1;
	int height = cudaSrcImage->Height;

	devNoiseRemoval<<<width * height / MAX_GPU_THREAD_NUM, MAX_GPU_THREAD_NUM>>>(cudaSrcImage->pComplexBuffer, width, height, radius, threshold);
	cudaDeviceSynchronize();

	CUDA_ERROR_CHECK(cudaStatus);
	auto result = cufftExecC2R(cudaSrcImage->inversePlan, cudaSrcImage->pComplexBuffer, (float*)cudaSrcImage->pImageBuffer);
	if (CUFFT_SUCCESS != result)
		return false;

	cudaDeviceSynchronize();
	CUDA_ERROR_CHECK(cudaStatus);

	return true;
}

bool CUDA_EDGE_FINDER(UINT srcImage, UINT dstImage, float threshold, int arraySize, bool inverse)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	auto cudaDstImage = GetCudaImage(dstImage);

	if (!cudaSrcImage || !cudaDstImage)
		return false;

	// 이미지 사이즈 확인
	if ((cudaSrcImage->Width != cudaDstImage->Width)
		|| (cudaSrcImage->Height != cudaDstImage->Height))
		return false;

	int width = static_cast<int>(sqrt(static_cast<double>(cudaSrcImage->Width * cudaSrcImage->Height) / MAX_GPU_THREAD_NUM)) + 1;
	dim3 grid(width, width, 1);
	dim3 threads(MAX_GPU_THREAD_NUM, 1, 1);

	BYTE* pSrcBuffer = (BYTE*)cudaSrcImage->pImageBuffer;
	BYTE* pDstBuffer = (BYTE*)cudaDstImage->pImageBuffer;

	bool isSame = cudaSrcImage == cudaDstImage;
	if (isSame)
	{
		cudaMalloc(&pSrcBuffer, cudaSrcImage->Width * cudaSrcImage->Height * sizeof(BYTE));
		cudaMemcpy(pSrcBuffer, pDstBuffer, cudaSrcImage->Width * cudaSrcImage->Height * sizeof(BYTE), cudaMemcpyHostToHost);
	}

	devEdgeFinder << <grid, threads >> > (pSrcBuffer, pDstBuffer, cudaSrcImage->Width, cudaSrcImage->Height, threshold, arraySize, inverse, cudaSrcImage->pRoiRect);

	cudaError_t cudaStatus;
	CUDA_ERROR_CHECK(cudaStatus);

	if (isSame)
		cudaFree(pSrcBuffer);

	return cudaStatus == cudaSuccess;
}

bool CUDA_MEAN_FILTER(UINT srcImage, UINT dstImage, int filterSize)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	auto cudaDstImage = GetCudaImage(dstImage);

	if (!cudaSrcImage || !cudaDstImage)
		return false;

	// 이미지 사이즈 확인
	if ((cudaSrcImage->Width != cudaDstImage->Width)
		|| (cudaSrcImage->Height != cudaDstImage->Height))
		return false;

	int blockSize = sqrt(MAX_GPU_THREAD_NUM);

	int gridWidth = cudaSrcImage->Width %  blockSize == 0 ? cudaSrcImage->Width / blockSize : cudaSrcImage->Width / blockSize + 1;
	int gridHeight = cudaSrcImage->Height %  blockSize == 0 ? cudaSrcImage->Height / blockSize : cudaSrcImage->Height / blockSize + 1;

	dim3 grid(gridWidth, gridHeight, 1);
	dim3 block(blockSize, blockSize, 1);

	BYTE* pSrcBuffer = (BYTE*)cudaSrcImage->pImageBuffer;
	BYTE* pDstBuffer = (BYTE*)cudaDstImage->pImageBuffer;

	bool isSame = cudaSrcImage == cudaDstImage;
	if (isSame)
	{
		cudaMalloc(&pSrcBuffer, cudaSrcImage->Width * cudaSrcImage->Height * sizeof(BYTE));
		cudaMemcpy(pSrcBuffer, pDstBuffer, cudaSrcImage->Width * cudaSrcImage->Height * sizeof(BYTE), cudaMemcpyHostToHost);
	}

	devMeanFilter <<< grid, block, sizeof(int) * (blockSize + filterSize - 1) * (blockSize + filterSize - 1) >>> (pSrcBuffer, pDstBuffer, cudaSrcImage->Width, cudaSrcImage->Height, filterSize);

	cudaError_t cudaStatus;
	CUDA_ERROR_CHECK(cudaStatus);

	if (isSame)
		cudaFree(pSrcBuffer);

	cudaDeviceSynchronize();

	return cudaStatus == cudaSuccess;
}

bool CUDA_RESIZE(UINT srcImage, UINT dstImage, int interpolation)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	auto cudaDstImage = GetCudaImage(dstImage);

	if (!cudaSrcImage || !cudaDstImage)
		return false;

	NppProcessing::Resize(cudaSrcImage, cudaDstImage, interpolation);

	return true;
}


bool CUDA_MATH_XOR(UINT srcImage1, UINT srcImage2, UINT dstImage)
{
	auto cudaSrcImage1 = GetCudaImage(srcImage1);
	auto cudaSrcImage2 = GetCudaImage(srcImage2);
	auto cudaDstImage = GetCudaImage(dstImage);

	if ((cudaSrcImage1->Width != cudaSrcImage2->Width)
		|| (cudaSrcImage1->Height != cudaSrcImage2->Height))
		return false;

	if ((cudaSrcImage1->Width != cudaDstImage->Width)
		|| (cudaSrcImage1->Height != cudaDstImage->Height))
		return false;

	if ((cudaSrcImage2->Width != cudaDstImage->Width)
		|| (cudaSrcImage2->Height != cudaDstImage->Height))
		return false;

	CudaMath::XOR(cudaSrcImage1, cudaSrcImage2, cudaDstImage);

	return true;
}

bool CUDA_MATH_AND(UINT srcImage1, UINT srcImage2, UINT dstImage)
{
	auto cudaSrcImage1 = GetCudaImage(srcImage1);
	auto cudaSrcImage2 = GetCudaImage(srcImage2);
	auto cudaDstImage = GetCudaImage(dstImage);

	if ((cudaSrcImage1->Width != cudaSrcImage2->Width)
		|| (cudaSrcImage1->Height != cudaSrcImage2->Height))
		return false;

	if ((cudaSrcImage1->Width != cudaDstImage->Width)
		|| (cudaSrcImage1->Height != cudaDstImage->Height))
		return false;

	if ((cudaSrcImage2->Width != cudaDstImage->Width)
		|| (cudaSrcImage2->Height != cudaDstImage->Height))
		return false;

	CudaMath::AND(cudaSrcImage1, cudaSrcImage2, cudaDstImage);

	return true;
}

bool CUDA_MATH_OR(UINT srcImage1, UINT srcImage2, UINT dstImage)
{
	auto cudaSrcImage1 = GetCudaImage(srcImage1);
	auto cudaSrcImage2 = GetCudaImage(srcImage2);
	auto cudaDstImage = GetCudaImage(dstImage);

	if ((cudaSrcImage1->Width != cudaSrcImage2->Width)
		|| (cudaSrcImage1->Height != cudaSrcImage2->Height))
		return false;

	if ((cudaSrcImage1->Width != cudaDstImage->Width)
		|| (cudaSrcImage1->Height != cudaDstImage->Height))
		return false;

	if ((cudaSrcImage2->Width != cudaDstImage->Width)
		|| (cudaSrcImage2->Height != cudaDstImage->Height))
		return false;

	CudaMath::OR(cudaSrcImage1, cudaSrcImage2, cudaDstImage);

	return true;
}

bool CUDA_MATH_MUL(UINT srcImage, float* profile)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	CudaMath::MUL(cudaSrcImage, profile);

	return true;
}

bool CUDA_RANSAC(int width, int height, 
	double* xArray, double* yArray, 
	int size, double* cost, double* gradient, 
	double* centerX, double* centerY, double threshold)
{
	CudaRansac ransac;
	return ransac.Compute(width, height, xArray, yArray, size, cost, gradient, centerX, centerY, threshold);
}

bool CUDA_CALIBRATION(UINT srcImage)
{
	return true;
}

bool CUDA_CREATE_CALIBRATION_BUFFER(UINT srcImage, float calValue, UINT length)
{
	return true;
}
