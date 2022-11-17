
#include <iostream> // cout, cerr
#include <assert.h> // assert
#include <string.h> // memcpy
#include <chrono> // time/clocks
#include <math.h>

#define M_PI       3.14159265358979323846   // pi

#include "NppProcessing.h"
#include "ImagesNPP.h"
#include <npp.h>

void NppProcessing::Resize(CudaImage* pSrcImage, CudaImage* pDstImage, int interpolation)
{
	NppiSize srcSize = { pSrcImage->Width, pSrcImage->Height };
	NppiRect srcRect = { srcSize.width , srcSize.height };
	
	NppiSize dstSize = { pDstImage->Width, pDstImage->Height };
	NppiRect dstRect = { dstSize.width , dstSize.height };

	BYTE* pSrcBuffer = new BYTE[srcSize.width * srcSize.height];
	BYTE* pDstBuffer = new BYTE[dstSize.width * dstSize.height];

	pSrcImage->GetImage(pSrcBuffer);
	pDstImage->GetImage(pDstBuffer);

	nppiResize_8u_C1R(
		(BYTE*)pSrcBuffer, srcSize.width, srcSize, srcRect,
		(BYTE*)pDstBuffer, dstSize.width, dstSize, dstRect, interpolation /*NppiInterpolationMode*/);

	pDstImage->SetImage(pDstBuffer);

	delete[] pSrcBuffer;
	delete[] pDstBuffer;
}

void NppProcessing::Canny(CudaImage * pSrcImage, CudaImage * pDstImage, int lowThreshold, int highThreshold)
{
	NppiSize oSizeImage = { pSrcImage->Width, pSrcImage->Height };

	RECT* roiRect = new RECT;
	cudaMemcpy(roiRect, pSrcImage->pRoiRect, sizeof(RECT), cudaMemcpyKind::cudaMemcpyDeviceToHost);

	//NppiSize oSizeROI = 
	//{ 
	//	roiRect->right - roiRect->left,
	//	roiRect->bottom - roiRect->top
	//};

	//NppiPoint oSrcOffset = { roiRect->left, roiRect->top };
	NppiPoint oSrcOffset = { 0, 0 };

	npp::ImageNPP_8u_C1 oDeviceSrc(oSizeImage.width, oSizeImage.height);
	oDeviceSrc.copyFrom((BYTE*)pSrcImage->pImageBuffer, pSrcImage->Width);

	npp::ImageNPP_8u_C1 oDeviceDst(oSizeImage.width, oSizeImage.height);

	int nBufferSize = 0;
	Npp8u * pScratchBufferNPP = 0;

	// get necessary scratch buffer size and allocate that much device memory

	NppStatus eStatusNPP = nppiFilterCannyBorderGetBufferSize(oSizeImage, &nBufferSize);
	if (eStatusNPP != NPP_SUCCESS)
		return;

	cudaMalloc((void **)&pScratchBufferNPP, nBufferSize);

	if ((nBufferSize > 0) && (pScratchBufferNPP != NULL))
	{
		nppiFilterCannyBorder_8u_C1R(
			oDeviceSrc.data(), oDeviceSrc.pitch(), oSizeImage, oSrcOffset,
			oDeviceDst.data(), oDeviceDst.pitch(), oSizeImage,
			NPP_FILTER_SOBEL, NPP_MASK_SIZE_3_X_3, lowThreshold, highThreshold,
			nppiNormL2, NPP_BORDER_REPLICATE, pScratchBufferNPP);
	}

	// free scratch buffer memory
	cudaFree(pScratchBufferNPP);

	oDeviceDst.copyTo((BYTE*)pDstImage->pImageBuffer, pDstImage->Width);

	delete roiRect;

	nppiFree(oDeviceSrc.data());
	nppiFree(oDeviceDst.data());
}
