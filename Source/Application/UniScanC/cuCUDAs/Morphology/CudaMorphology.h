#ifndef _CUDA_MORPHOLOGY_H_
#define _CUDA_MORPHOLOGY_H_

#include "Defines.h"
#include "CudaImage.h"

class CudaMorphology
{
public:
	static void Erode(CudaImage* pSrcImage, CudaImage* pDstImage, int maskSize = 1);
	static void Dilate(CudaImage* pSrcImage, CudaImage* pDstImage, int maskSize = 1);

	static void Open(CudaImage* pSrcImage, CudaImage* pDstImage, int maskSize = 1);
	static void Close(CudaImage* pSrcImage, CudaImage* pDstImage, int maskSize = 1);
	static void Thinning(CudaImage * pSrcImage);

	static void Sobel(CudaImage * pSrcImage, CudaImage * pDstImage);
};

#endif
