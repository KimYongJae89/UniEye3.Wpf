#include "CudaMorphology.h"
#include "DeviceMorphologyFunction.h"

void CudaMorphology::Erode(CudaImage* pSrcImage, CudaImage* pDstImage, int maskSize)
{
	int nWidth = pSrcImage->Width;
	int nHeight = pSrcImage->Height;

	dim3 grid, threads;
	GetThreadNum(grid, threads, nWidth, nHeight);

	kernel_Erode << <grid, threads >> > ((BYTE*)pSrcImage->pImageBuffer, (BYTE*)pDstImage->pImageBuffer, nWidth, nHeight, maskSize);
}

void CudaMorphology::Dilate(CudaImage* pSrcImage, CudaImage* pDstImage, int maskSize)
{
	int nWidth = pSrcImage->Width;
	int nHeight = pSrcImage->Height;

	dim3 grid, threads;
	GetThreadNum(grid, threads, nWidth, nHeight);

	kernel_Dilate << <grid, threads >> > ((BYTE*)pSrcImage->pImageBuffer, (BYTE*)pDstImage->pImageBuffer, nWidth, nHeight, maskSize);
}

void CudaMorphology::Open(CudaImage * pSrcImage, CudaImage * pDstImage, int maskSize)
{
	BYTE* pTempBuffer;
	cudaMalloc(&pTempBuffer, pSrcImage->Width * pSrcImage->Height * sizeof(BYTE));

	int nWidth = pSrcImage->Width;
	int nHeight = pSrcImage->Height;

	dim3 grid, threads;
	GetThreadNum(grid, threads, nWidth, nHeight);

	kernel_Erode << <grid, threads >> > ((BYTE*)pSrcImage->pImageBuffer, (BYTE*)pTempBuffer, nWidth, nHeight, maskSize);
	kernel_Dilate << <grid, threads >> > ((BYTE*)pTempBuffer, (BYTE*)pDstImage->pImageBuffer, nWidth, nHeight, maskSize);

	cudaFree(pTempBuffer);
}

void CudaMorphology::Close(CudaImage * pSrcImage, CudaImage * pDstImage, int maskSize)
{
	BYTE* pTempBuffer;
	cudaMalloc(&pTempBuffer, pSrcImage->Width * pSrcImage->Height * sizeof(BYTE));

	int nWidth = pSrcImage->Width;
	int nHeight = pSrcImage->Height;

	dim3 grid, threads;
	GetThreadNum(grid, threads, nWidth, nHeight);

	kernel_Dilate << <grid, threads >> > ((BYTE*)pSrcImage->pImageBuffer, (BYTE*)pTempBuffer, nWidth, nHeight, maskSize);
	kernel_Erode << <grid, threads >> > ((BYTE*)pTempBuffer, (BYTE*)pDstImage->pImageBuffer, nWidth, nHeight, maskSize);

	cudaFree(pTempBuffer);
}

void CudaMorphology::Thinning(CudaImage * pSrcImage)
{
	int nWidth = pSrcImage->Width;
	int nHeight = pSrcImage->Height;

	dim3 grid, threads;
	GetThreadNum(grid, threads, nWidth, nHeight);

	bool* pCudaResult;
	cudaMalloc(&pCudaResult, sizeof(bool));

	BYTE* pDstBuffer;
	cudaMalloc(&pDstBuffer, sizeof(BYTE) * nWidth * nHeight);
	cudaMemcpy(pDstBuffer, pSrcImage->pImageBuffer, sizeof(BYTE) * nWidth * nHeight, cudaMemcpyDeviceToDevice);

	bool* pHostResult = new bool;
	*pHostResult = true;

	while (*pHostResult)
	{
		cudaMemset(pCudaResult, 0x00, sizeof(bool));

		kernel_Thinning << <grid, threads >> > (pDstBuffer, pDstBuffer, nWidth, nHeight, pCudaResult);

		cudaMemcpy(pHostResult, pCudaResult, sizeof(bool), cudaMemcpyDeviceToHost);
	}

	while (*pHostResult)
	{
		cudaMemset(pCudaResult, 0x00, sizeof(bool));

		kernel_EdgeTrim << <grid, threads >> > (pDstBuffer, pDstBuffer, nWidth, nHeight, pCudaResult);

		cudaMemcpy(pHostResult, pCudaResult, sizeof(bool), cudaMemcpyDeviceToHost);
	}

	cudaMemcpy(pSrcImage->pImageBuffer, pDstBuffer, sizeof(BYTE) * nWidth * nHeight, cudaMemcpyDeviceToDevice);

	cudaFree(pDstBuffer);
	cudaFree(pCudaResult);
	delete[] pHostResult;
}

void CudaMorphology::Sobel(CudaImage * pSrcImage, CudaImage * pDstImage)
{
	int nWidth = pSrcImage->Width;
	int nHeight = pSrcImage->Height;

	dim3 grid, threads;
	GetThreadNum(grid, threads, nWidth, nHeight);

	kernel_Sobel << <grid, threads >> > ((BYTE*)pSrcImage->pImageBuffer, (BYTE*)pDstImage->pImageBuffer, nWidth, nHeight);
}
