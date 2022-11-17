#include <cmath>
#include "ccl_hm.h"

__device__ int set_label_hm(int* count, int* lock)
{
	int temp = 0;
	bool blocked = true;
	while (blocked) {
		if (atomicCAS(lock, 0, 1) == 0)
		{
			temp = (*count)++;
			atomicExch(lock, 0);
			blocked = false;
		}
	}

	return temp;
}

__global__ void kernel_mask_hm(BYTE D[], BYTE M[], int W, int H)
{
	int x = blockIdx.x * blockDim.x + threadIdx.x;

	if (x >= W)
		return;

	int y = (blockIdx.y * blockDim.y) + threadIdx.y;

	if (y >= H)
		return;

	int id = y * W + x;

	if (D[id] == 0)
		return;

	BYTE temp = 0b00000000;

	if (x > 0 && y > 0 && D[id - W - 1] > 0)
		temp |= 0b10000000;

	if (y > 0 && D[id - W] > 0)
		temp |= 0b01000000;

	if (x < W - 1 && y > 0 && D[id - W + 1] > 0)
		temp |= 0b00100000;

	if (x > 0 && D[id - 1] > 0)
		temp |= 0b00010000;

	if (x < W - 1 && D[id + 1] > 0)
		temp |= 0b00001000;

	if (x > 0 && y < H - 1 && D[id + W - 1] > 0)
		temp |= 0b00000100;

	if (y < H - 1 && D[id + W] > 0)
		temp |= 0b00000010;

	if (x < W - 1 && y < H - 1 && D[id + W + 1] > 0)
		temp |= 0b00000001;

	M[id] = temp;
}

__global__ void init_label_map(BYTE M[], int L[], int N)
{
	int id = blockIdx.x * blockDim.x + threadIdx.x;

	if (id >= N)
		return;

	if (M[id] == 0b00000000)
		return;

	if (L[id] == id)
		L[id] = id;
}

__global__ void kernel_labeling_hm(BYTE D[], BYTE M[], int L[], int W, int H)
{
	__shared__ extern int sL[];
	__shared__ bool merged;

	int startX = blockIdx.x * blockDim.x;
	int globalX = startX + threadIdx.x;
	
	if (globalX >= W)
		return;
	 
	int startY = blockIdx.y * blockDim.y;
	int globalY = startY + threadIdx.y;
	
	if (globalY >= H)
		return;
	
	int id = globalY * W + globalX;
	
	if (M[id] == 0b00000000)
	{
		if (D[id] > 0)
			L[id] = id;

		return;
	}
	

	int local_id = threadIdx.y * blockDim.x + threadIdx.x;
	
	int merged_index = blockDim.x * blockDim.y;
	BYTE temp = M[id];

	if (temp & 0b10000000 && threadIdx.x > 0 && threadIdx.y > 0)
		sL[local_id] = local_id - blockDim.x - 1;
	else if (temp & 0b01000000 && threadIdx.y > 0)
		sL[local_id] = local_id - blockDim.x;
	else if (temp & 0b00100000 && threadIdx.x < blockDim.x - 1 && threadIdx.y > 0)
		sL[local_id] = local_id - blockDim.x + 1;
	else if (temp & 0b00010000 && threadIdx.x > 0)
		sL[local_id] = local_id - 1;
	else
		sL[local_id] = local_id;

	__syncthreads();

	while (sL[local_id] > sL[sL[local_id]])
		sL[local_id] = sL[sL[local_id]];

	do
	{
		__syncthreads();

		merged = false;

		int index = sL[local_id];
		int ref_index = index;
		
		if (temp & 0b10000000 && threadIdx.x > 0 && threadIdx.y > 0)
			index = min(index, sL[local_id - blockDim.x - 1]);
		if (temp & 0b01000000 && threadIdx.y > 0)
			index = min(index, sL[local_id - blockDim.x]);
		if (temp & 0b00100000 && threadIdx.x < blockDim.x - 1 && threadIdx.y > 0)
			index = min(index, sL[local_id - blockDim.x + 1]);
		if (temp & 0b00010000 && threadIdx.x > 0)
			index = min(index, sL[local_id - 1]);
		if (temp & 0b00001000 && threadIdx.x < blockDim.x - 1)
			index = min(index, sL[local_id + 1]);
		if (temp & 0b00000100 && threadIdx.x > 0 && threadIdx.y < blockDim.y - 1)
			index = min(index, sL[local_id + blockDim.x - 1]);
		if (temp & 0b00000010 && threadIdx.y < blockDim.y - 1)
			index = min(index, sL[local_id + blockDim.x]);
		if (temp & 0b00000001 && threadIdx.x < blockDim.x - 1 && threadIdx.y < blockDim.y - 1)
			index = min(index, sL[local_id + blockDim.x + 1]);

		__syncthreads();

		if (sL[ref_index] > index)
			atomicMin(&(sL[ref_index]), index);
			
		__syncthreads();

		index = sL[local_id];
		while (index > sL[index])
			index = sL[index];

		__syncthreads();

		if (index < ref_index)
		{
			sL[local_id] = index;
			merged = true;
		}
			
		__syncthreads();

	} while (merged == true);
	
	L[id] = (((sL[local_id] / blockDim.x) + startY) * W) + (sL[local_id] % blockDim.x) + startX;
}

__global__ void find_vertical_hm(BYTE M[], int L[], int W, int H, int block_dist, bool *changed)
{
	__shared__ extern int sL[];

	int x = (blockIdx.x * block_dist) + block_dist + threadIdx.x - 1;
	if (x >= W)
		return;

	int y = blockIdx.y * blockDim.y + threadIdx.y;

	if (y >= H)
		return;

	int id = y * W + x;

	if (threadIdx.x == 0 && (M[id] & 0b00101001) == 0b00000000)
		return;
	
	if (threadIdx.x == 1 && (M[id] & 0b10010100) == 0b00000000)
		return;
	
	int local_index = blockDim.x * threadIdx.y + threadIdx.x;
	sL[local_index] = L[id];

	int ref_index = sL[local_index];//I[id];
	int index = ref_index;

	BYTE temp = M[id];

	__syncthreads();

	if (threadIdx.x == 0)
	{
		if (temp & 0b00100000)
		{
			if (threadIdx.y > 0)
				index = min(index, sL[local_index - blockDim.x + 1]);
			else
				index = min(index, L[id - W + 1]);
		}
		if (temp & 0b00001000)
		{
			index = min(index, sL[local_index + 1]);
		}
		if (temp & 0b00000001)
		{
			if (threadIdx.y < blockDim.y - 1)
				index = min(index, sL[local_index + blockDim.x + 1]);
			else
				index = min(index, L[id + W + 1]);
		}
	}
	else
	{
		if (temp & 0b10000000)
		{
			if (threadIdx.y > 0)
				index = min(index, sL[local_index - blockDim.x - 1]);
			else
				index = min(index, L[id - W - 1]);
		}
		if (temp & 0b00010000)
		{
			index = min(index, sL[local_index - 1]);
		}
		if (temp & 0b00000100)
		{
			if (threadIdx.y < blockDim.y -1)
				index = min(index, sL[local_index + blockDim.x - 1]);
			else
				index = min(index, L[id + W - 1]);
		}
	}
	
	if (L[ref_index] > index)
	{
		atomicMin(&(L[ref_index]), index);
		*changed = true;
	}
}

__global__ void find_horizental_hm(BYTE M[], int L[], int W, int H, int block_dist, bool *changed)
{
	__shared__ extern int sL[];

	int x = blockIdx.x * blockDim.x + threadIdx.x;
	if (x >= W)
		return;

	int y = (blockIdx.y * block_dist) + block_dist + threadIdx.y - 1;

	if (y >= H)
		return;

	int id = y * W + x;

	if (threadIdx.y == 0 && (M[id] & 0b00000111) == 0b00000000)
		return;

	if (threadIdx.y == 1 && (M[id] & 0b11100000) == 0b00000000)
		return;

	int local_index = blockDim.x * threadIdx.y + threadIdx.x;
	sL[local_index] = L[id];

	int ref_index = sL[local_index];//I[id];
	int index = ref_index;

	BYTE temp = M[id];

	__syncthreads();

	if (threadIdx.y == 0)
	{
		if (temp & 0b00000100)
		{
			if (threadIdx.x > 0)
				index = min(index, sL[local_index + blockDim.x - 1]);
			else
				index = min(index, L[id + W - 1]);
		}
		if (temp & 0b00000010)
		{
			index = min(index, sL[local_index + blockDim.x]);
		}
		if (temp & 0b00000001)
		{
			if (threadIdx.x < blockDim.x - 1)
				index = min(index, sL[local_index + blockDim.x + 1]);
			else
				index = min(index, L[id + W + 1]);
		}
	}
	else
	{
		if (temp & 0b10000000)
		{
			if (threadIdx.x > 0)
				index = min(index, sL[local_index - blockDim.x - 1]);
			else
				index = min(index, L[id - W - 1]);
		}
		if (temp & 0b01000000)
		{
			index = min(index, sL[local_index - blockDim.x]);
		}
		if (temp & 0b00100000)
		{
			if (threadIdx.x < blockDim.x - 1)
				index = min(index, sL[local_index - blockDim.x + 1]);
			else
				index = min(index, L[id - W + 1]);
		}
	}

	if (L[ref_index] > index)
	{
		atomicMin(&(L[ref_index]), index);
		*changed = true;
	}
}

__global__ void indexing_hm(BYTE M[], int L[], int N)
{
	int id = blockIdx.x * blockDim.x + threadIdx.x;

	if (id >= N)
		return;

	if (M[id] == 0b00000000)
		return;

	int index = L[id];
	int refIndex = index;
	while (index > L[index])
		index = L[index];

	if (refIndex > index)
		L[id] = index;
}

__global__ void counting_root_hm(int L[], int N, int count[])
{
	__shared__ int s_count;

	if (threadIdx.x == 0)
		s_count = 0;
 
	int id = blockIdx.x * blockDim.x + threadIdx.x;
	if (id >= N)
		return;

	if (L[id] == id)
	{
		__syncthreads();

		atomicAdd(&s_count, 1);

		__syncthreads();
		count[blockIdx.x] = s_count;
	}
}

__global__ void labeling_root_hm(int L[],int N, int count[])
{
	__shared__ int s_count;
	__shared__ int s_lock;

	if (threadIdx.x == 0)
	{
 		s_count = count[(gridDim.x *  blockIdx.y) + blockIdx.x];
		s_lock = 0;
	}

	int id = blockIdx.x * blockDim.x + threadIdx.x;

	if (id >= N)
		return;

	__syncthreads();

	if (L[id] == id)
		L[id] = -set_label_hm(&s_count, &s_lock) - 2;
}

__global__ void labeling_child_hm(int L[], int N)
{
	int id = blockIdx.x * blockDim.x + threadIdx.x;

	if (id >= N)
		return;

	if (L[id] > 0)
		L[id] = L[L[id]];
}

__global__ void kernel_blob_hm(BYTE S[], int L[], int N, int W, unsigned int* areaArray, unsigned int* xMinArray, unsigned int* xMaxArray, unsigned int* yMinArray, unsigned int* yMaxArray, unsigned int* vMinArray, unsigned int* vMaxArray, float* vMeanArray)
{
	int id = blockIdx.x * blockDim.x + threadIdx.x;
	if (id >= N)
		return;

	if (L[id] >= -1)
		return;

	int x = id % W;
	int y = id / W;

	int arrayIndex = -(L[id] + 2);

	unsigned int value = (unsigned int)S[id];

	atomicAdd(&(areaArray[arrayIndex]), 1);

	if (xMinArray[arrayIndex] > x)
		atomicMin(&(xMinArray[arrayIndex]), x);

	if (xMaxArray[arrayIndex] < x)
		atomicMax(&(xMaxArray[arrayIndex]), x);

	if (yMinArray[arrayIndex] > y)
		atomicMin(&(yMinArray[arrayIndex]), y);

	if (yMaxArray[arrayIndex] < y)
		atomicMax(&(yMaxArray[arrayIndex]), y);
	
	if (vMinArray[arrayIndex] > value)
		atomicMin(&(vMinArray[arrayIndex]), value);

	if (vMaxArray[arrayIndex] < value)
		atomicMax(&(vMaxArray[arrayIndex]), value);

	atomicAdd(&(vMeanArray[arrayIndex]), S[id]);
}
