#ifndef _CUDA_MATH_H_
#define _CUDA_MATH_H_

#include "Defines.h"
#include "CudaImage.h"

class CudaMath
{
public:
	static void AND(CudaImage* pSrcImage1, CudaImage* pSrcImage2, CudaImage* pDstImage);
	static void OR(CudaImage* pSrcImage1, CudaImage* pSrcImage2, CudaImage* pDstImage);
	static void XOR(CudaImage* pSrcImage1, CudaImage* pSrcImage2, CudaImage* pDstImage);
	static void MUL(CudaImage* pSrcImage, float* profile);
};

#endif
