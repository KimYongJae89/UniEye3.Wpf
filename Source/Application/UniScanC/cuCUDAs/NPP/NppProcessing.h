#ifndef _NPP_PROCESSING_H_
#define _NPP_PROCESSING_H_

#include "Defines.h"
#include "CudaImage.h"

class NppProcessing
{
public:
	static void Resize(CudaImage* pSrcImage, CudaImage* pDstImage, int interpolation);
	static void Canny(CudaImage * pSrcImage, CudaImage * pDstImage, int lowThreshold, int highThreshold);

};

#endif