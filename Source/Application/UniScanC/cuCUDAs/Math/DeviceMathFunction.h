#include "Defines.h"

__global__ void kernel_AND(BYTE* pSrcBuffer1, BYTE* pSrcBuffer2, BYTE* pDstBuffer, int W, int H);
__global__ void kernel_OR(BYTE* pSrcBuffer1, BYTE* pSrcBuffer2, BYTE* pDstBuffer, int W, int H);
__global__ void kernel_XOR(BYTE* pSrcBuffer1, BYTE* pSrcBuffer2, BYTE* pDstBuffer, int W, int H);
__global__ void kernel_MUL(BYTE* pSrcBuffer, float* profile, int W, int H);