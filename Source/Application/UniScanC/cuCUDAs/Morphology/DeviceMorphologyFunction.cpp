#include "DeviceMorphologyFunction.h"

__global__ void kernel_Erode(BYTE * pSrcBuffer, BYTE * pDstBuffer, int W, int H, int mask)
{
	int x = blockIdx.x * blockDim.x + threadIdx.x;
	int y = blockIdx.y * blockDim.y + threadIdx.y;

	if (x + 1 >= W || y + 1 >= H ||
		x - 1 < 0 || y - 1 < 0)
		return;

	int id = y * W + x;

	for (int rangeY = y - mask; rangeY <= y + mask; rangeY++)
	{
		if (rangeY < 0 || rangeY >= H)
			continue;

		for (int rangeX = x - mask; rangeX <= x + mask; rangeX++)
		{
			if (rangeX < 0 || rangeX >= W)
				continue;

			int index = rangeY * W + rangeX;

			if (pSrcBuffer[index] == 0)
			{
				pDstBuffer[id] = 0;
				return;
			}
		}
	}

	pDstBuffer[id] = 255;
}

__global__ void kernel_Dilate(BYTE * pSrcBuffer, BYTE * pDstBuffer, int W, int H, int mask)
{
	int x = blockIdx.x * blockDim.x + threadIdx.x;
	int y = blockIdx.y * blockDim.y + threadIdx.y;

	if (x + 1 >= W || y + 1 >= H ||
		x - 1 < 0 || y - 1 < 0)
		return;

	int id = y * W + x;

	bool isFind = false;
	for (int rangeY = y - mask; rangeY <= y + mask; rangeY++)
	{
		if (rangeY < 0 || rangeY >= H)
			continue;

		for (int rangeX = x - mask; rangeX <= x + mask; rangeX++)
		{
			if (rangeX < 0 || rangeX >= W)
				continue;

			int index = rangeY * W + rangeX;

			if (pSrcBuffer[index] == 255)
			{
				pDstBuffer[id] = 255;
				return;
			}
		}
	}

	pDstBuffer[id] = 0;
}

__device__ int thinningFilter[2][3][3] =
{
	{
		{ 0,  0,  0},
		{-1,  1, -1},
		{ 1,  1,  1}
	},
	{
		{-1,  0,  0},
		{ 1,  1,  0},
		{-1,  1, -1}
	}
};

__global__ void kernel_Thinning(BYTE * pSrcBuffer, BYTE * pDstBuffer, int W, int H, bool* pResult)
{
	int x = blockIdx.x * blockDim.x + threadIdx.x;
	int y = blockIdx.y * blockDim.y + threadIdx.y;

	int id = y * W + x;

	//int id = blockIdx.x * blockDim.x + blockIdx.y * blockDim.x * gridDim.x + threadIdx.x;

	//if (id >= W * H)
	//	return;

	//int x = id % W;
	//int y = id / W;

	if (x + 1 >= W || y + 1 >= H ||
		x - 1 < 0 || y - 1 < 0)
		return;

	if (pSrcBuffer[id] == 0)
		return;

	// 주변 픽셀이 2개 미만일 경우 무시한다.
	int nNearCount = 0;

	for (int rangeY = -1; rangeY <= 1; rangeY++)
	{
		for (int rangeX = -1; rangeX <= 1; rangeX++)
		{
			if (pSrcBuffer[(y + rangeY) * W + (x + rangeX)] == 255)
				nNearCount++;

			if (nNearCount >= 3)
				break;
		}

		if (nNearCount >= 3)
			break;
	}

	if (nNearCount < 3)
		return;

	for (int rot = 0; rot < 4; rot++)
	{
		for (int i = 0; i < 2; i++)
		{
			bool isFind = true;
			int nForePixelCount = 0;

			for (int rangeY = 0; rangeY < 3; rangeY++)
			{
				for (int rangeX = 0; rangeX < 3; rangeX++)
				{
					int filterX = rangeX;
					int filterY = rangeY;

					int nFilterValue = 0;

					switch (rot)
					{
					case 1:
						nFilterValue = thinningFilter[i][filterX][2 - filterY];
						break;
					case 2:
						nFilterValue = thinningFilter[i][2 - filterY][2 - filterX];
						break;
					case 3:
						nFilterValue = thinningFilter[i][2 - filterX][filterY];
						break;
					case 0:
					default:
						nFilterValue = thinningFilter[i][filterY][filterX];
						break;
					}

					if (nFilterValue < 0)
						continue;

					nFilterValue *= 255;

					int maskY = y + rangeY - 1;
					int maskX = x + rangeX - 1;

					int srcPixelValue = pSrcBuffer[maskY * W + maskX];

					if (srcPixelValue != nFilterValue)
					{
						isFind = false;
						break;
					}
				}

				if (!isFind)
					break;
			}

			if (isFind)
			{
				pDstBuffer[id] = 0;
				*pResult = true;
				return;
			}
		}
	}
}

__device__ 	int edgeTrimFilter[2][3][3] =
{
	{
		{ 0,  0,  0},
		{ 0,  1,  0},
		{ 0, -1, -1}
	},
	{
		{ 0,  0,  0},
		{ 0,  1,  0},
		{-1, -1,  0}
	}
};

__global__ void kernel_EdgeTrim(BYTE * pSrcBuffer, BYTE * pDstBuffer, int W, int H, bool* pResult)
{
	int x = blockIdx.x * blockDim.x + threadIdx.x;
	int y = blockIdx.y * blockDim.y + threadIdx.y;

	if (x + 1 >= W || y + 1 >= H ||
		x - 1 < 0 || y - 1 < 0)
		return;

	int id = y * W + x;

	if (pSrcBuffer[id] == 0)
		return;

	for (int rot = 0; rot < 4; rot++)
	{
		for (int i = 0; i < 2; i++)
		{
			bool isFind = true;

			for (int rangeY = 0; rangeY < 3; rangeY++)
			{
				for (int rangeX = 0; rangeX < 3; rangeX++)
				{
					int filterX = rangeX;
					int filterY = rangeY;

					int nFilterValue = 0;

					switch (rot)
					{
					case 1:
						nFilterValue = edgeTrimFilter[i][filterX][2 - filterY] * 255;
						break;
					case 2:
						nFilterValue = edgeTrimFilter[i][2 - filterY][2 - filterX] * 255;
						break;
					case 3:
						nFilterValue = edgeTrimFilter[i][2 - filterX][filterY] * 255;
						break;
					case 0:
					default:
						nFilterValue = edgeTrimFilter[i][filterY][filterX] * 255;
						break;
					}

					if (nFilterValue < 0)
						continue;

					int maskY = y + rangeY - 1;
					int maskX = x + rangeX - 1;

					if (pSrcBuffer[maskY * W + maskX] != nFilterValue)
					{
						isFind = false;
						break;
					}
				}

				if (!isFind)
					break;
			}

			if (isFind)
			{
				pDstBuffer[id] = 0;
				*pResult = true;
				return;
			}
		}
	}
}

__device__ int sobelFilter[2][3][3] =
{
	{
		{  1,  2,  1},
		{  0,  0,  0},
		{ -1, -2, -1}
	},
	{
		{ -1, 0, 1},
		{ -2, 0, 2},
		{ -1, 0, 1}
	},
};

__global__ void kernel_Sobel(BYTE* pSrcBuffer, BYTE * pDstBuffer, int W, int H)
{
	int x = blockIdx.x * blockDim.x + threadIdx.x;
	int y = blockIdx.y * blockDim.y + threadIdx.y;

	if (x + 1 >= W || y + 1 >= H ||
		x - 1 < 0 || y - 1 < 0)
		return;

	int id = y * W + x;

	int sumValue[2];

	for (int i = 0; i < 2; i++)
	{
		sumValue[i] = 0;

		for (int rangeY = 0; rangeY < 3; rangeY++)
		{
			for (int rangeX = 0; rangeX < 3; rangeX++)
			{
				sumValue[i] += (sobelFilter[i][rangeY][rangeX] * pSrcBuffer[(y + rangeY - 1) * W + (x + rangeX - 1)]);
			}
		}
	}

	//pDstBuffer[id] = sqrtf((sumValue[0] * sumValue[0]) + (sumValue[1] * sumValue[1]));
	//pDstBuffer[id] = abs(sumValue[0]) + abs(sumValue[1]);
	pDstBuffer[id] = (sumValue[0] > 0 ? sumValue[0] : -sumValue[0]) + (sumValue[1] > 0 ? sumValue[1] : -sumValue[1]);
}
