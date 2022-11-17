#include "CudaMath.h"
#include "DeviceMathFunction.h"

void CudaMath::AND(CudaImage* pSrcImage1, CudaImage* pSrcImage2, CudaImage* pDstImage)
{
	int nWidth = pSrcImage1->Width;
	int nHeight = pSrcImage1->Height;

	dim3 grid, threads;
	GetThreadNum(grid, threads, nWidth, nHeight);

	kernel_AND << < grid, threads >> > ((BYTE*)pSrcImage1->pImageBuffer, (BYTE*)pSrcImage2->pImageBuffer, (BYTE*)pDstImage->pImageBuffer, nWidth, nHeight);
}

void CudaMath::OR(CudaImage* pSrcImage1, CudaImage* pSrcImage2, CudaImage* pDstImage)
{
	int nWidth = pSrcImage1->Width;
	int nHeight = pSrcImage1->Height;

	dim3 grid, threads;
	GetThreadNum(grid, threads, nWidth, nHeight);

	kernel_OR << < grid, threads >> > ((BYTE*)pSrcImage1->pImageBuffer, (BYTE*)pSrcImage2->pImageBuffer, (BYTE*)pDstImage->pImageBuffer, nWidth, nHeight);
}

void CudaMath::XOR(CudaImage* pSrcImage1, CudaImage* pSrcImage2, CudaImage* pDstImage)
{
	int nWidth = pSrcImage1->Width;
	int nHeight = pSrcImage1->Height;

	dim3 grid, threads;
	GetThreadNum(grid, threads, nWidth, nHeight);

	kernel_XOR << < grid, threads >> > ((BYTE*)pSrcImage1->pImageBuffer, (BYTE*)pSrcImage2->pImageBuffer, (BYTE*)pDstImage->pImageBuffer, nWidth, nHeight);
}

void CudaMath::MUL(CudaImage* pSrcImage, float* profile)
{
	int nWidth = pSrcImage->Width;
	int nHeight = pSrcImage->Height;

	int blockNum = nWidth % MAX_GPU_THREAD_NUM == 0 ? nWidth / MAX_GPU_THREAD_NUM : nWidth / MAX_GPU_THREAD_NUM + 1;

	cudaMemcpy(pSrcImage->pProfile, profile, nWidth * sizeof(float), cudaMemcpyHostToDevice);

	kernel_MUL << < blockNum, MAX_GPU_THREAD_NUM >> > ((BYTE*)pSrcImage->pImageBuffer, pSrcImage->pProfile, nWidth, nHeight);
}