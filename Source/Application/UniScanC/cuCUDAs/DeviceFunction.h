//#ifndef _CUDA_FUNCTION_H_
//#define _CUDA_FUNCTION_H_
//#endif

#include "Defines.h"
#include "device_launch_parameters.h"
#include "cuda_runtime.h"
#include "CudaLock.h"

__global__ void devClip(BYTE* pSrcImageBuffer, BYTE* pDstImageBuffer,
	const int srcWidth,
	const int srcHeight,
	const int dstX,
	const int dstY,
	const int dstWidth,
	const int dstHeight);

__global__ void devProfile(BYTE* pImageBuffer, float* pProfile, const int width, const int height);
__global__ void devAreaSum(BYTE* pImageBuffer, const int width, const int height, const int areaCount, float* average, int* mutex = NULL);

__global__ void devEdgeFinderX(
	BYTE* pImageBuffer, 
	float* pProfile, 
	double threshold, 
	int range, 
	int* startPos, 
	int* endPos, 
	CudaLock lock);

__global__ void devEdgeFinder(
	BYTE* pSrcBuffer,
	BYTE* pDstBuffer,
	const int width,
	const int height,
	const float threshold,
	const int arraySize,
	bool inverse,
	LPRECT lpRoiRect);

__global__ void devLowerBinarize(
	BYTE* pSrcBuffer,
	BYTE* pDstBuffer,
	const int width,
	const int height,
	const float threshold,
	bool inverse = false,
	LPRECT lpRoiRect = NULL);

__global__ void devUpperBinarize(
	BYTE* pSrcBuffer,
	BYTE* pDstBuffer,
	const int width,
	const int height,
	const float threshold,
	bool inverse = false,
	LPRECT lpRoiRect = NULL);

__global__ void devBinarize(
	BYTE* pSrcBuffer,
	BYTE* pDstBuffer,
	const int width,
	const int height,
	const float lower,
	const float upper,
	bool inverse = false,
	LPRECT lpRoiRect = NULL);

__global__ void devLowerAdaptiveBinarize(
	BYTE* pSrcBuffer,
	BYTE* pDstBuffer,
	float* pProfile,
	const int width,
	const int height,
	const float threshold,
	bool inverse = false,
	LPRECT lpRoiRect = NULL);

__global__ void devUpperAdaptiveBinarize(
	BYTE* pSrcBuffer,
	BYTE* pDstBuffer,
	float* pProfile,
	const int width,
	const int height,
	const float threshold,
	bool inverse = false,
	LPRECT lpRoiRect = NULL);

__global__ void devAdaptiveBinarize(
	BYTE* pSrcBuffer,
	BYTE* pDstBuffer,
	float* pProfile,
	const int width,
	const int height,
	const float lower,
	const float upper,
	bool inverse = false,
	LPRECT lpRoiRect = NULL);

__global__ void devLabelMarker(
	BYTE* pImageBuffer,
	const int width,
	const int height,
	int** profileLabel,
	int** profilePosX,
	int** profilePosY,
	LPRECT lpRoiRect = NULL);

__global__ void devAreaBinarize(
	BYTE* pSrcBuffer,
	BYTE* pDstBuffer,
	int areaCount,
	float* pProfile,
	const int width,
	const int height,
	const float lower,
	const float upper,
	bool inverse,
	LPRECT lpRoiRect);

__global__ void devNoiseRemoval(
	cuComplex* pSrcBuffer,
	const int width,
	const int height,
	const double radius,
	const double threshold);

__global__ void devMeanFilter(
	BYTE* pSrcBuffer,
	BYTE* pDstBuffer,
	const int width,
	const int height,
	const int filterSize);