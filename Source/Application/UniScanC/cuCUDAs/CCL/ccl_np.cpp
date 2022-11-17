// Marathon Match - CCL - Neighbour Propagation

#include <cmath>
#include "ccl_np.h"

#define NOMINMAX

__global__ void init_np(int L[], int N)
{
	int id = blockIdx.x * blockDim.x + blockIdx.y * blockDim.x * gridDim.x + threadIdx.x;
	if (id >= N) return;

	L[id] = id;
}

__global__ void kernel(BYTE D[], int L[], bool* m, int N, int W)
{
	int id = blockIdx.x * blockDim.x + blockIdx.y * blockDim.x * gridDim.x + threadIdx.x;
	if (id >= N)
		return;

	int label = 0;

	if (D[id] <= 0)
	{
		L[id] = 0;
		return;
	}

	if (id - W >= 0 && D[id - W] > 0)
		label = (int)L[id - W];
	else if (id - 1 >= 0 && D[id - 1] > 0)
		label = (int)L[id - 1];

	if (label != 0 && label < L[id]) {
		L[id] = label;
		*m = true;
	}
}

__global__ void kernel8(BYTE D[], int L[], bool* m, int N, int W)
{
	int id = blockIdx.x * blockDim.x + blockIdx.y * blockDim.x * gridDim.x + threadIdx.x;
	if (id >= N)
		return;

	int Did = D[id];
	int label = N;
	if (id - W >= 0 && D[id - W] > 0)
		label = L[id - W];
	if (id + W < N  && D[id + W] > 0)
		label = min(label, L[id + W]);

	int r = id % W;
	if (r)
	{
		if (D[id - 1] > 0)
			label = L[id - 1];

		if (id - W - 1 >= 0 && D[id - W - 1] > 0)
			label = L[id - W - 1];
		if (id + W - 1 < N  && D[id + W - 1] > 0) 
			label = min(label, L[id + W - 1]);
	}

	if (r + 1 != W)
	{
		if (D[id + 1] > 0)
			label = min(label, L[id + 1]);

		if (id - W + 1 >= 0 && D[id - W + 1] > 0) 
			label = min(label, L[id - W + 1]);
		if (id + W + 1 < N  && D[id + W + 1]> 0)
			label = min(label, L[id + W + 1]);
	}

	if (label < L[id]) {
		//atomicMin(&R[L[id]], label);
		L[id] = label;
		*m = true;
	}
}
