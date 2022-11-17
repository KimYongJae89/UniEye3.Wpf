#include "DeviceFunction.h"

__device__ void lock(int *mutex) {
	while (atomicCAS(mutex, 0, 1) != 0);
}
__device__ void unlock(int *mutex) {
	atomicExch(mutex, 0);
}

__global__ void devClip(BYTE* pSrcImageBuffer, BYTE* pDstImageBuffer,
	const int srcWidth, 
	const int srcHeight,
	const int dstX,
	const int dstY, 
	const int dstWidth,
	const int dstHeight)
{
	int idx = blockIdx.x * blockDim.x + blockIdx.y * blockDim.x * gridDim.x + threadIdx.x;
	if (idx >= srcWidth * srcHeight)
		return;

	int x = idx % srcWidth;
	int y = idx / srcWidth;

	if (x < dstX || x > dstX + dstWidth ||
		y < dstY || y > dstY + dstHeight)
		return;

	int dstIndex = (y - dstY) * dstWidth + (x - dstX);
	pDstImageBuffer[dstIndex] = pSrcImageBuffer[idx];
}

__global__ void devProfile(BYTE* pImageBuffer, float* pProfile, const int width, const int height)
{
	float dev_Profile = 0;
	int x = blockIdx.x * blockDim.x + threadIdx.x;

	if (x >= width)
		return;

	int i = x;

	for (int y = 0; y < height; y++)
	{
		dev_Profile += (float)pImageBuffer[i];
		i += width;
	}

	pProfile[x] = dev_Profile / (float)height;
}

__global__ void devAreaSum(BYTE* pImageBuffer, const int width, const int height, const int areaCount, float* average, int* mutex)
{
	int areaWidth = (width / (float)areaCount) + 0.5;
	
	int idx = blockDim.x * blockIdx.x + threadIdx.x;

	if (idx >= width)
		return;

	int areaIdx = idx / areaWidth;

	float avg = 0;

	for (int y = 0; y < height; y++)
	{
		lock(mutex);
		average[areaIdx] += pImageBuffer[y * width + idx];
		unlock(mutex);
	}
}

__global__ void devEdgeFinderX(
	BYTE* pImageBuffer, 
	float* pProfile, 
	double threshold, 
	int range, 
	int* startPos, 
	int* endPos, 
	CudaLock lock)
{
	int x = blockIdx.x * blockDim.x + threadIdx.x;

	if (x >= range)
		return;

	if (pProfile[x] > threshold)
	{
		lock.lock();

		if (*startPos > x)
			*startPos = x;

		if (*endPos < x)
			*endPos = x;

		lock.unlock();
	}
}

__global__ void devEdgeFinder(
	BYTE* pSrcBuffer,
	BYTE* pDstBuffer,
	const int width,
	const int height,
	const float threshold,
	const int arraySize,
	bool inverse,
	LPRECT lpRoiRect)
{
	int idx = blockIdx.x * blockDim.x + blockIdx.y * blockDim.x * gridDim.x + threadIdx.x;

	if (idx >= width * height)
		return;

	int x = idx % width;
	int y = idx / width;

	int target = 255;
	int background = 0;

	if (inverse)
	{
		target = 0;
		background = 255;
	}

	if (lpRoiRect && (x < lpRoiRect->left || x > lpRoiRect->right || y < lpRoiRect->top || y > lpRoiRect->bottom))
	{
		pDstBuffer[idx] = background;
		return;
	}

	int count = 0;
	float intensity = 0;

	int halfRange = arraySize / 2;
	for (int rangeY = 0; rangeY < arraySize; rangeY++)
	{
		int yIndex = y - rangeY - halfRange;
		if (yIndex < 0 || yIndex >= height)
			continue;

		//for (int rangeX = 0; rangeX < arraySize; rangeX++)
		//{
		//	int xIndex = x - rangeX - halfRange;
		//	if (xIndex < 0 || xIndex >= width)
		//		continue;

		//	int yIndex = y - rangeY - halfRange;
		//	if (yIndex < 0 || yIndex >= height)
		//		continue;

		//	if (idx == yIndex * width + xIndex)
		//		continue;

		//	intensity += pSrcBuffer[yIndex * width + xIndex];
		//	count++;
		//}

			
		if (idx == yIndex * width + x)
			continue;

		intensity += pSrcBuffer[yIndex * width + x];
		count++;
	}

	if (intensity != 0)
	{
		intensity /= (float)count;
		if (abs(pSrcBuffer[idx] - intensity) >= threshold)
		{
			pDstBuffer[idx] = target;
			return;
		}
	}
}

__global__ void devLowerBinarize(
	BYTE* pSrcBuffer,
	BYTE* pDstBuffer,
	const int width,
	const int height,
	const float threshold,
	bool inverse,
	LPRECT lpRoiRect)
{
	int idx = blockIdx.x * blockDim.x + blockIdx.y * blockDim.x * gridDim.x + threadIdx.x;

	if (idx >= width * height)
		return;

	int x = idx % width;
	int y = idx / width;

	int target = 255;
	int background = 0;

	if (inverse)
	{
		target = 0;
		background = 255;
	}

	if (lpRoiRect && (x < lpRoiRect->left || x > lpRoiRect->right || y < lpRoiRect->top || y > lpRoiRect->bottom))
	{
		pDstBuffer[idx] = background;
		return;
	}

	if (pSrcBuffer[idx] < threshold)
		pDstBuffer[idx] = target;
	else
		pDstBuffer[idx] = background;
}

__global__ void devUpperBinarize(
	BYTE* pSrcBuffer,
	BYTE* pDstBuffer,
	const int width,
	const int height,
	const float threshold,
	bool inverse,
	LPRECT lpRoiRect)
{
	int idx = blockIdx.x * blockDim.x + blockIdx.y * blockDim.x * gridDim.x + threadIdx.x;

	if (idx >= width * height)
		return;

	int x = idx % width;
	int y = idx / width;

	int target = 255;
	int background = 0;

	if (inverse)
	{
		target = 0;
		background = 255;
	}

	if (lpRoiRect && (x < lpRoiRect->left || x > lpRoiRect->right || y < lpRoiRect->top || y > lpRoiRect->bottom))
	{
		pDstBuffer[idx] = background;
		return;
	}

	if (pSrcBuffer[idx] > threshold)
		pDstBuffer[idx] = target;
	else
		pDstBuffer[idx] = background;
}

__global__ void devBinarize(
	BYTE* pSrcBuffer,
	BYTE* pDstBuffer,
	const int width,
	const int height,
	const float lower,
	const float upper,
	bool inverse,
	LPRECT lpRoiRect)
{
	int idx = blockIdx.x * blockDim.x + blockIdx.y * blockDim.x * gridDim.x + threadIdx.x;

	if (idx >= width * height)
		return;

	int x = idx % width;
	int y = idx / width;

	int target = 255;
	int background = 0;

	if (inverse)
	{
		target = 0;
		background = 255;
	}

	if (lpRoiRect && (x < lpRoiRect->left || x > lpRoiRect->right || y < lpRoiRect->top || y > lpRoiRect->bottom))
	{
		pDstBuffer[idx] = background;
		return;
	}

	if (pSrcBuffer[idx] < lower ||
		pSrcBuffer[idx] > upper)
	{
		pDstBuffer[idx] = target;
	}
	else
	{
		pDstBuffer[idx] = background;
	}
}

__global__ void devLowerAdaptiveBinarize(
	BYTE* pSrcBuffer, 
	BYTE* pDstBuffer, 
	float* pProfile, 
	const int width, 
	const int height, 
	const float threshold,
	bool inverse,
	LPRECT lpRoiRect)
{
	int idx = blockIdx.x * blockDim.x + blockIdx.y * blockDim.x * gridDim.x + threadIdx.x;

	if (idx >= width * height)
		return;

	int x = idx % width;
	int y = idx / width;

	int target = 255;
	int background = 0;

	if (inverse)
	{
		target = 0;
		background = 255;
	}

	if (lpRoiRect && (x < lpRoiRect->left || x > lpRoiRect->right || y < lpRoiRect->top || y > lpRoiRect->bottom))
	{
		pDstBuffer[idx] = background;
		return;
	}

	float line_threshold = pProfile[x] - threshold;

	if (pSrcBuffer[idx] < line_threshold)
		pDstBuffer[idx] = target;
	else
		pDstBuffer[idx] = background;
}

__global__ void devUpperAdaptiveBinarize(
	BYTE* pSrcBuffer,
	BYTE* pDstBuffer,
	float* pProfile,
	const int width,
	const int height,
	const float threshold,
	bool inverse,
	LPRECT lpRoiRect)
{
	int idx = blockIdx.x * blockDim.x + blockIdx.y * blockDim.x * gridDim.x + threadIdx.x;

	if (idx >= width * height)
		return;

	int x = idx % width;
	int y = idx / width;

	int target = 255;
	int background = 0;

	if (inverse)
	{
		target = 0;
		background = 255;
	}

	if (lpRoiRect && (x < lpRoiRect->left || x > lpRoiRect->right || y < lpRoiRect->top || y > lpRoiRect->bottom))
	{
		pDstBuffer[idx] = background;
		return;
	}

	float line_threshold = pProfile[x] + threshold;

	if (pSrcBuffer[idx] > line_threshold)
		pDstBuffer[idx] = target;
	else
		pDstBuffer[idx] = background;
}

__global__ void devAdaptiveBinarize(
	BYTE* pSrcBuffer,
	BYTE* pDstBuffer,
	float* pProfile,
	const int width,
	const int height,
	const float lower,
	const float upper,
	bool inverse,
	LPRECT lpRoiRect)
{
	int idx = blockIdx.x * blockDim.x + blockIdx.y * blockDim.x * gridDim.x + threadIdx.x;

	if (idx >= width * height)
		return;

	int x = idx % width;
	int y = idx / width;

	int target = 255;
	int background = 0;

	if (inverse)
	{
		target = 0;
		background = 255;
	}

	if (lpRoiRect && (x < lpRoiRect->left || x > lpRoiRect->right || y < lpRoiRect->top || y > lpRoiRect->bottom))
	{
		pDstBuffer[idx] = background;
		return;
	}

	float line_Lower = pProfile[x] - lower;
	float line_Upper = pProfile[x] + upper;

	if (pSrcBuffer[idx] < line_Lower ||
		pSrcBuffer[idx] > line_Upper)
	{
		pDstBuffer[idx] = target;
	}
	else
	{
		pDstBuffer[idx] = background;
	}
}

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
	LPRECT lpRoiRect)
{
	int idx = blockIdx.x * blockDim.x + blockIdx.y * blockDim.x * gridDim.x + threadIdx.x;

	if (idx >= width * height)
		return;

	int x = idx % width;
	int y = idx / width;

	int areaWidth = width / (float)areaCount + 0.5;
	int areaIndex = x / areaWidth;

	int target = 255;
	int background = 0;

	if (inverse)
	{
		target = 0;
		background = 255;
	}

	if (lpRoiRect && (x < lpRoiRect->left || x > lpRoiRect->right || y < lpRoiRect->top || y > lpRoiRect->bottom))
	{
		pDstBuffer[idx] = background;
		return;
	}

	float line_Lower = 0;
	float line_Upper = 0;
	if (lower != 0)
		line_Lower = pProfile[areaIndex] - lower;

	if (upper != 0)
		line_Upper = pProfile[areaIndex] + upper;

	if ((line_Lower != 0 && pSrcBuffer[idx] < line_Lower) ||
		(line_Upper != 0 && pSrcBuffer[idx] > line_Upper))
	{
		pDstBuffer[idx] = target;
	}
	else
	{
		pDstBuffer[idx] = background;
	}
}

__global__ void devNoiseRemoval(
	cuComplex* pSrcBuffer,
	const int width,
	const int height,
	const double radius,
	const double threshold)
{
	int idx = blockIdx.x * blockDim.x + threadIdx.x;
 
	if (idx >= width * height)
		return;

	int x = idx % width;
	int y = idx / width;

	double widthRadius = width * radius;
	double heightRadius = height * radius;

	double distTh = sqrt(widthRadius * widthRadius + heightRadius * heightRadius);

	double minX = min(x, width - x - 1);
	double minY = min(y, height - y - 1);

	if (sqrt(minX * minX + minY * minY) >= distTh)
	{
		pSrcBuffer[idx].x = 0.0f;
		pSrcBuffer[idx].y = 0.0f;
	}
}

__device__ void CopyShared(
	BYTE* pSrcBuffer,
	int* shared,
	int idx,
	int s_idx,
	int x,
	int y,
	int width,
	int s_width,
	int height,
	int filterHalfSize)
{
	shared[s_idx] = pSrcBuffer[idx];

	if (x == 0)
	{
		for (int i = -filterHalfSize; i < 0; i++)
			shared[s_idx + i] = -1;
	}
	else if (threadIdx.x == 0)
	{
		for (int i = -filterHalfSize; i < 0; i++)
			shared[s_idx + i] = pSrcBuffer[idx + i];
	}

	if (y == 0)
	{
		for (int i = -filterHalfSize; i < 0; i++)
			shared[s_idx + (i * s_width)] = -1;
	}
	else if (threadIdx.y == 0)
	{
		for (int i = -filterHalfSize; i < 0; i++)
			shared[s_idx + (i * s_width)] = pSrcBuffer[idx + (i * width)];
	}

	if (threadIdx.x == blockDim.x - 1 || x == width - 1)
	{
		for (int i = 1; i <= filterHalfSize; i++)
		{
			if (x + i > width - 1)
			{
				shared[s_idx + i] = -1;
				continue;
			}

			shared[s_idx + i] = pSrcBuffer[idx + i];
		}
	}

	if (threadIdx.y == blockDim.y - 1 || y == height - 1)
	{
		for (int i = 1; i <= filterHalfSize; i++)
		{
			if (y + i > height - 1)
			{
				shared[s_idx + (i * s_width)] = -1;
				continue;
			}

			shared[s_idx + (i * s_width)] = pSrcBuffer[idx + (i * width)];
		}
	}

	if (threadIdx.x == 0 && threadIdx.y == 0)
	{
		for (int y_idx = -filterHalfSize; y_idx < 0; y_idx++)
		{
			int g_index = idx + (y_idx * width);
			int s_index = s_idx + (y_idx * s_width);

			for (int x_idx = -filterHalfSize; x_idx < 0; x_idx++)
			{
				if (y + y_idx < 0 || x + x_idx < 0)
				{
					shared[s_index + x_idx] = -1;
					continue;
				}

				shared[s_index + x_idx] = pSrcBuffer[g_index + x_idx];
			}
		}
	}

	if ((threadIdx.x == blockDim.x - 1 || x == width - 1) && threadIdx.y == 0)
	{
		for (int y_idx = -filterHalfSize; y_idx < 0; y_idx++)
		{
			int g_index = idx + (y_idx * width);
			int s_index = s_idx + (y_idx * s_width);

			for (int x_idx = 1; x_idx <= filterHalfSize; x_idx++)
			{
				if (y + y_idx < 0 || x + x_idx > width - 1)
				{
					shared[s_index + x_idx] = -1;
					continue;
				}

				shared[s_index + x_idx] = pSrcBuffer[g_index + x_idx];
			}
		}
	}

	if (threadIdx.x == 0 && (threadIdx.y == blockDim.y - 1 || y == height - 1))
	{
		for (int y_idx = 1; y_idx <= filterHalfSize; y_idx++)
		{
			int g_index = idx + (y_idx * width);
			int s_index = s_idx + (y_idx * s_width);

			for (int x_idx = -filterHalfSize; x_idx < 0; x_idx++)
			{
				if (y + y_idx > height - 1 || x + x_idx < 0)
				{
					shared[s_index + x_idx] = -1;
					continue;
				}

				shared[s_index + x_idx] = pSrcBuffer[g_index + x_idx];
			}
		}
	}

	if ((threadIdx.x == blockDim.x - 1 || x == width - 1) && (threadIdx.y == blockDim.y - 1 || y == height - 1))
	{
		for (int y_idx = 1; y_idx <= filterHalfSize; y_idx++)
		{
			int g_index = idx + (y_idx * width);
			int s_index = s_idx + (y_idx * s_width);

			for (int x_idx = 1; x_idx <= filterHalfSize; x_idx++)
			{
				if (y + y_idx > height - 1 || x + x_idx > width - 1)
				{
					shared[s_index + x_idx] = -1;
					continue;
				}

				shared[s_index + x_idx] = pSrcBuffer[g_index + x_idx];
			}
		}
	}
}

__device__ void MeanFilter(
	BYTE* pDstBuffer,
	int* shared,
	int idx,
	int s_idx,
	int s_width,
	int filterHalfSize)
{
	float count = 0;
	float average = 0;

	int start_y = s_idx + (-filterHalfSize * s_width) - filterHalfSize;
	int end_y = s_idx + (filterHalfSize * s_width) - filterHalfSize;

	int size = min(filterHalfSize * 2 + 1, s_width - (start_y % s_width) - 1);

	for (int y_idx = start_y; y_idx <= end_y; y_idx += s_width)
	{
		for (int x_idx = y_idx; x_idx < y_idx + size; x_idx++)
		{
			if (shared[x_idx] == -1)
				continue;

			average += shared[x_idx];
			count++;
		}
	}

	pDstBuffer[idx] = average / count;
}

__device__ void SobelFilter(
	BYTE* shared,
	int s_idx,
	int s_width, 
	int x,
	int y,
	int width, 
	int height)
{
	float value = 0;

	if (x > 0 && y > 0 && x < width - 1 && y < height - 1)
	{
		float filter1[] =
		{
			-1, 0, 1,
			-2, 0, 2,
			-1, 0, 1
		};

		float filter2[] =
		{
			-1, -2, -1,
			0, 0, 0,
			1, 2, 1
		};

		int start_y = s_idx - s_width;
		int end_y = s_idx + s_width;

		for (int y_idx = start_y, filter_idx = 0; y_idx <= end_y; y_idx += s_width)
		{
			int start_x = y_idx - 1;
			int end_x = y_idx + 1;

			for (int x_idx = start_x; x_idx <= end_x; x_idx++, filter_idx++)
			{
				value += shared[x_idx] * filter1[filter_idx];
				value += shared[x_idx] * filter2[filter_idx];
			}
		}
	}

	__syncthreads();

	shared[s_idx] = value;
}

__global__ void devMeanFilter(
	BYTE* pSrcBuffer,
	BYTE* pDstBuffer,
	const int width,
	const int height,
	const int filterSize)
{
	extern __shared__ int shared[];

	int x = blockIdx.x * blockDim.x + threadIdx.x;

	if (x > width - 1)
		return;

	int y = blockIdx.y * blockDim.y + threadIdx.y;

	if (y > height - 1)
		return;

	int idx = y * width + x;

	int filterHalfSize = filterSize / 2;

	int s_width = blockDim.x + filterSize - 1;
	int s_x = threadIdx.x + filterHalfSize;
	int s_y = threadIdx.y + filterHalfSize;
	int s_idx = s_y * s_width + s_x;

	CopyShared(pSrcBuffer, shared, idx, s_idx, x, y, width, s_width, height, filterHalfSize);

	__syncthreads();

	MeanFilter(pDstBuffer, shared, idx, s_idx, s_width, filterHalfSize);
}

///

__device__ __inline__ void getLinearInterpCoefs(float val, float* c0, float* c1)
{
	*c1 = val - floorf(val); *c0 = 1.0 - (*c1);
}

__global__ void remapCombined_kernel(cudaTextureObject_t tex_inArg, BYTE *d_out,
	int xSizeIn, int ySizeIn, int xSizeOut, int ySizeOut, int pitchInDATA_TYPE, int pitchOutDATA_TYPE,
	float yPreScale, float xPreScale, float yPreOffset, float xPreOffset, float cosTheta, float sinTheta)
{ // each thread processes one output sample
	int iFx = blockIdx.x*blockDim.x + threadIdx.x;
	int iFy = blockIdx.y*blockDim.y + threadIdx.y;

	int indexOut = iFy * pitchOutDATA_TYPE + iFx;
	float Fx, Fy; BYTE V;
	if ((iFx < xSizeOut) && (iFy < ySizeOut))
	{
		Fx = (float(iFx) + xPreOffset)*xPreScale + 0.5f;
		Fy = (float(iFy) + yPreOffset)*yPreScale + 0.5f;
		float ffX = Fx - xSizeIn / 2;
		float ffY = Fy - ySizeIn / 2;
		Fx = cosTheta * ffX + sinTheta * ffY + xSizeIn / 2;
		Fy = -sinTheta * ffX + cosTheta * ffY + ySizeIn / 2;
		int xCoordInt = (int)Fx;
		int yCoordInt = (int)Fy;
		int indexIn = xCoordInt + pitchInDATA_TYPE * (yCoordInt);
		
		{ 
			// bilinear interpolation case
			BYTE V00, V01, V10, V11;
			V00 = tex1Dfetch<BYTE>(tex_inArg, indexIn);
			V10 = tex1Dfetch<BYTE>(tex_inArg, indexIn + 1);
			V01 = tex1Dfetch<BYTE>(tex_inArg, indexIn + pitchInDATA_TYPE);
			V11 = tex1Dfetch<BYTE>(tex_inArg, indexIn + pitchInDATA_TYPE + 1);
			float c0, c1, d0, d1;
			getLinearInterpCoefs(Fx, &c0, &c1);
			getLinearInterpCoefs(Fy, &d0, &d1);
			V = c0 * d0*V00 + c0 * d1*V01 + c1 * d0*V10 + c1 * d1*V11;
		}
	}
}
