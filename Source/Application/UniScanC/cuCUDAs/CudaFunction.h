#include "Defines.h"
#include "CudaImage.h"
#include <cmath>

#define EXPORT_DLL extern "C" __declspec(dllexport)

CudaImage* GetCudaImage(UINT imageId);

EXPORT_DLL bool CUDA_INITIALIZE(int& gpuNo);
EXPORT_DLL void CUDA_RELEASE();

EXPORT_DLL void CUDA_THREAD_NUM(int threadNum);

EXPORT_DLL void CUDA_WAIT();

// 이미지를 생성하고 이미지에 대한 고유 식별 번호를 리턴한다.
EXPORT_DLL UINT CUDA_CREATE_IMAGE(int width, int height, int depth);
EXPORT_DLL bool CUDA_CLIP_IMAGE(UINT srcImage, UINT dstImage, int x, int y, int width, int height);
EXPORT_DLL bool CUDA_SET_IMAGE(UINT image, void* pImageBuffer);

EXPORT_DLL bool CUDA_CLEAR_IMAGE(UINT image);
EXPORT_DLL void CUDA_FREE_IMAGE(UINT image);
EXPORT_DLL bool CUDA_GET_IMAGE(UINT image, void* pDstBuffer);

EXPORT_DLL void CUDA_SET_ROI(UINT image, double x, double y, double width, double height);
EXPORT_DLL void CUDA_RESET_ROI(UINT image);

// 생성된 이미지의 프로파일을 계산한다.
EXPORT_DLL bool CUDA_CREATE_PROFILE(UINT srcImage);

// 이미지의 영역을 분할하여 영역별 평균 밝기를 계산한다.
EXPORT_DLL bool CUDA_AREA_AVERAGE(UINT srcImage, int areaCount, float* hostAverage);

// 라벨 버퍼 미리 할당
EXPORT_DLL bool CUDA_CREATE_LABEL_BUFFER(UINT srcImage);
EXPORT_DLL bool CUDA_CREATE_FFT_BUFFER(UINT srcImage);

EXPORT_DLL bool CUDA_BINARIZE(UINT srcImage, UINT dstImage, float lower, float upper, bool inverse = false);
EXPORT_DLL bool CUDA_BINARIZE_LOWER(UINT srcImage, UINT dstImage, float lower, bool inverse = false);
EXPORT_DLL bool CUDA_BINARIZE_UPPER(UINT srcImage, UINT dstImage, float upper, bool inverse = false);

EXPORT_DLL bool CUDA_ADAPTIVE_BINARIZE(UINT srcImage, UINT dstImage, float lower, float upper, bool inverse = false);
EXPORT_DLL bool CUDA_ADAPTIVE_BINARIZE_LOWER(UINT srcImage, UINT dstImage, float lower, bool inverse = false);
EXPORT_DLL bool CUDA_ADAPTIVE_BINARIZE_UPPER(UINT srcImage, UINT dstImage, float upper, bool inverse = false);


// 이미지 영역을 분할하여 각 영역별 평균(hostAverage)과 비교값을 이진화한다.
EXPORT_DLL bool CUDA_AREA_BINARIZE(UINT srcImage, UINT dstImage, int areaCount, float* hostAverage, int lowThreshold, int highThreshold, bool inverse = false);

EXPORT_DLL bool CUDA_EDGE_DETECT(UINT srcImage, EdgeSearchDirection dir, int threshold, int* startPos, int* endPos);

// Blob
EXPORT_DLL int CUDA_LABELING(UINT binImage);

EXPORT_DLL bool CUDA_BLOBING(UINT binImage, UINT srcImage, int count,
	UINT* areaArray, UINT* xMinArray, UINT* xMaxArray, UINT* yMinArray, UINT* yMaxArray, 
	UINT* vMinArray, UINT* vMaxArray, float* vMeanArray);

// MORPHOLOGY
EXPORT_DLL bool CUDA_MORPHOLOGY_ERODE(UINT srcImage, UINT dstImage, int maskSize);
EXPORT_DLL bool CUDA_MORPHOLOGY_DILATE(UINT srcImage, UINT dstImage, int maskSize);
EXPORT_DLL bool CUDA_MORPHOLOGY_OPEN(UINT srcImage, UINT dstImage, int maskSize);
EXPORT_DLL bool CUDA_MORPHOLOGY_CLOSE(UINT srcImage, UINT dstImage, int maskSize);
EXPORT_DLL bool CUDA_MORPHOLOGY_THINNING(UINT srcImage);

EXPORT_DLL bool CUDA_SOBEL(UINT srcImage, UINT dstImage);

EXPORT_DLL bool CUDA_CANNY(UINT srcImage, UINT dstImage, int lowThreshold, int highThreshold);
EXPORT_DLL bool Save(UINT srcImage, char* fileName);

// Filter
EXPORT_DLL bool CUDA_MEAN_FILTER(UINT srcImage, UINT dstImage, int filterSize);

// Transformation
EXPORT_DLL bool CUDA_RESIZE(UINT srcImage, UINT dstImage, int interpolation);

// FFT
EXPORT_DLL bool NoiseRemoval(UINT srcImage, double radius, double threshold);

EXPORT_DLL bool CUDA_EDGE_FINDER(UINT srcImage, UINT dstImage, float threshold, int arraySize, bool inverse);

// MATH
EXPORT_DLL bool CUDA_MATH_AND(UINT srcImage1, UINT srcImage2, UINT dstImage);
EXPORT_DLL bool CUDA_MATH_OR(UINT srcImage1, UINT srcImage2, UINT dstImage);
EXPORT_DLL bool CUDA_MATH_XOR(UINT srcImage1, UINT srcImage2, UINT dstImage);
EXPORT_DLL bool CUDA_MATH_MUL(UINT srcImage, float* profile);

// Ransac
EXPORT_DLL bool CUDA_RANSAC(int width, int height, double* xArray, double* yArray, int size, double* cost, double* gradient, double* centerX, double* centerY, double threshold);

EXPORT_DLL bool CUDA_CALIBRATION(UINT srcImage);

EXPORT_DLL bool CUDA_CREATE_CALIBRATION_BUFFER(UINT srcImage, float calValue, UINT length);

