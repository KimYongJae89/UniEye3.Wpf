#include "DeviceMathFunction.h"

__global__ void kernel_AND(BYTE* pSrcBuffer1, BYTE* pSrcBuffer2, BYTE* pDstBuffer, int W, int H)
{
	int id = (blockIdx.x * blockDim.x) + (blockIdx.y * blockDim.x * gridDim.x) + threadIdx.x;

	if (id >= W * H)
		return;

	if (pSrcBuffer1[id] && pSrcBuffer2[id])
		pDstBuffer[id] = 255;
	else
		pDstBuffer[id] = 0;
}

__global__ void kernel_OR(BYTE* pSrcBuffer1, BYTE* pSrcBuffer2, BYTE* pDstBuffer, int W, int H)
{
	int id = (blockIdx.x * blockDim.x) + (blockIdx.y * blockDim.x * gridDim.x) + threadIdx.x;

	if (id >= W * H)
		return;

	if (pSrcBuffer1[id] || pSrcBuffer2[id])
		pDstBuffer[id] = 255;
	else
		pDstBuffer[id] = 0;
}


__global__ void kernel_XOR(BYTE* pSrcBuffer1, BYTE* pSrcBuffer2, BYTE* pDstBuffer, int W, int H)
{
	int id = (blockIdx.x * blockDim.x) + (blockIdx.y * blockDim.x * gridDim.x) + threadIdx.x;

	if (id >= W * H)
		return;

	if (pSrcBuffer1[id] ^ pSrcBuffer2[id])
		pDstBuffer[id] = 255;
	else
		pDstBuffer[id] = 0;
}

__global__ void kernel_MUL(BYTE* pSrcBuffer, float* profile, int W, int H)
{
	int id = blockIdx.x * blockDim.x + threadIdx.x;

	if (id >= W)
		return;

	for (int y = 0, index = id; y < H; y++, index += W)
	{
		float value = pSrcBuffer[index] * profile[id];
		value = value > 255 ? 255 : value;
		value = value < 0 ? 0 : value;

		pSrcBuffer[index] = value;
	}
}