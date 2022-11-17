// Marathon Match - CCL - Directional Propagation Labelling

#include <cmath>
#include "ccl_dpl.h"

__global__ void init_dpl(int L[], int N)
{
	int id = blockIdx.x * blockDim.x + blockIdx.y * blockDim.x * gridDim.x + threadIdx.x;
	if (id >= N) return;

	L[id] = id;
}

__global__ void kerneldpl(int I, BYTE D[], int L[], bool* m, int N, int W)
{
	int id = blockIdx.x * blockDim.x + blockIdx.y * blockDim.x * gridDim.x + threadIdx.x;
	int H = N / W;
	int S, E, step;
	switch (I) {
	case 0:
		if (id >= W) return;
		S = id;
		E = W * (H - 1) + id;
		step = W;
		break;
	case 1:
		if (id >= H) return;
		S = id * W;
		E = S + W - 1;
		step = 1;
		break;
	case 2:
		if (id >= W) return;
		S = W * (H - 1) + id;
		E = id;
		step = -W;
		break;
	case 3:
		if (id >= H) return;
		S = (id + 1) * W - 1;
		E = id * W;
		step = -1;
		break;
	}

	int label = L[S];
	for (int n = S + step; n != E + step; n += step)
	{
		if (D[n - step] > 0 && label < L[n])
		{
			L[n] = label;
			*m = true;
		}
		else
		{
			label = L[n];
		}
	}
}

__global__ void kernel8dpl(int I, BYTE D[], int L[], bool* m, int N, int W)
{
	int id = blockIdx.x * blockDim.x + blockIdx.y * blockDim.x * gridDim.x + threadIdx.x;
	int H = N / W;
	int S, E1, E2, step;
	switch (I) {
	case 0:
		if (id >= W + H - 1)
			return;
		if (id < W)
			S = id;
		else
			S = (id - W + 1) * W;
		E1 = W - 1; // % W
		E2 = H - 1; // / W
		step = W + 1;
		break;
	case 1:
		if (id >= W + H - 1)
			return;
		if (id < W)
			S = W * (H - 1) + id;
		else
			S = (id - W + 1) * W;
		E1 = W - 1; // % W
		E2 = 0; // / W
		step = -W + 1;
		break;
	case 2:
		if (id >= W + H - 1)
			return;
		if (id < W)
			S = W * (H - 1) + id;
		else
			S = (id - W) * W + W - 1;
		E1 = 0; // % W
		E2 = 0; // / W
		step = -(W + 1);
		break;
	case 3:
		if (id >= W + H - 1)
			return;
		if (id < W)
			S = id;
		else
			S = (id - W + 1) * W + W - 1;
		E1 = 0; // % W
		E2 = H - 1; // / W
		step = W - 1;
		break;
	}

	if (E1 == S % W || E2 == S / W)
		return;

	int label = L[S];
	for (int n = S + step;; n += step)
	{
		if (D[n - step] > 0 && label < L[n])
		{
			L[n] = label;
			*m = true;
		}
		else
			label = L[n];

		if (E1 == n % W || E2 == n / W)
			break;
	}
}
