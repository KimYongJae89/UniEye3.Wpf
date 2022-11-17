#include "cuda_runtime.h"
#include "device_launch_parameters.h"
#include "CudaImage.h"

#define MEM_RELEASE(x) if(x) delete x; x = NULL;
#define CUDA_RELEASE(x) if(x) cudaFree(x); x = NULL;

CudaImage::CudaImage(int id, int _gpuNo)
{
	imageId = id;
	gpuNo = _gpuNo;

	Width = 0;
	Height = 0;

	pImageBuffer = NULL;
	pProfile = NULL;
	pRoiRect = NULL;

	plabelBuffer = NULL;

	forwardPlan = NULL;
	inversePlan = NULL;
	pComplexBuffer = NULL;
}

CudaImage::~CudaImage()
{
	Free();
}

bool CudaImage::Alloc(int width, int height, int depth)
{
	Free();

	Depth = depth;

	int size = width * height;
	if (!CheckCudaError(cudaMalloc(&pImageBuffer, size * depth)))
		goto $CudaFailed;

	if (!CheckCudaError(cudaMalloc(&pProfile, width * sizeof(float))))
		goto $CudaFailed;

	CheckCudaError(cudaMemset(pImageBuffer, 0, size * depth));
	CheckCudaError(cudaMemset(pProfile, 0, Width * sizeof(float)));

	Width = width;
	Height = height;

	return true;

$CudaFailed:
	Free();

	return false;
}

void CudaImage::Free()
{
	CUDA_RELEASE(pImageBuffer);
	CUDA_RELEASE(pProfile);
	CUDA_RELEASE(pRoiRect);

	if (plabelBuffer != NULL)
	{
		CUDA_RELEASE(plabelBuffer->pLabel);
		CUDA_RELEASE(plabelBuffer->pMask);

		delete plabelBuffer;
		plabelBuffer = NULL;
	}

	if (forwardPlan != NULL && inversePlan != NULL && pComplexBuffer != NULL)
	{
		cufftDestroy(forwardPlan);
		cufftDestroy(inversePlan);
		CUDA_RELEASE(pComplexBuffer);
	}
}

bool CudaImage::SetImage(void* pDstImage)
{
	if (!pImageBuffer)
		return false;

	return CheckCudaError(cudaMemcpy(pImageBuffer, pDstImage, Width * Height * Depth, cudaMemcpyHostToDevice));
}

bool CudaImage::GetImage(void* pDstBuffer)
{
	if (pImageBuffer)
		return CheckCudaError(cudaMemcpy(pDstBuffer, pImageBuffer, Width * Height * Depth, cudaMemcpyDeviceToHost));

	return false;
}

bool CudaImage::ClearImage()
{
	return CheckCudaError(cudaMemset(pImageBuffer, 0, Width * Height * Depth));
}

bool CudaImage::GetProfile(float * pDstBuffer)
{
	if (pProfile)
		return CheckCudaError(cudaMemcpy((void*)pDstBuffer, pProfile, Width * sizeof(float), cudaMemcpyDeviceToHost));

	return false;
}

void CudaImage::SetImageROI(double x, double y, double _width, double _height)
{
	RECT rect;
	rect.left = x;
	rect.right = x + _width;
	rect.top = y;
	rect.bottom = y + _height;

	if (!CheckCudaError(cudaMalloc(&pRoiRect, sizeof(RECT))))
		return;

	CheckCudaError(cudaMemcpy(pRoiRect, &rect, sizeof(RECT), cudaMemcpyHostToDevice));
}

void CudaImage::ResetImageROI()
{
	MEM_RELEASE(pRoiRect);
}

bool CudaImage::CheckCudaError(cudaError_t cudaResult)
{
	m_lastError = cudaResult;
	return cudaResult == cudaSuccess;
}
