#include "Defines.h"

__global__ void kernel_Erode(BYTE* pSrcBuffer, BYTE* pDstBuffer, int W, int H, int mask);
__global__ void kernel_Dilate(BYTE* pSrcBuffer, BYTE* pDstBuffer, int W, int H, int mask);
__global__ void kernel_Thinning(BYTE * pSrcBuffer, BYTE * pDstBuffer, int W, int H, bool* pResult);
__global__ void kernel_EdgeTrim(BYTE * pSrcBuffer, BYTE * pDstBuffer, int W, int H, bool* pResult);
__global__ void kernel_Sobel(BYTE* pSrcBuffer, BYTE * pDstBuffer, int W, int H);
