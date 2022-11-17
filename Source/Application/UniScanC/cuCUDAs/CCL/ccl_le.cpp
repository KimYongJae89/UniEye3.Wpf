// Marathon Match - CCL - Label Equivalence

#include <iostream>
#include <iomanip>
#include <fstream>
#include <sstream>
#include <string>
#include <vector>
#include <map>
#include <queue>
#include <list>
#include <algorithm>
#include <utility>
#include <cmath>
#include <functional>
#include <cstring>
#include <cmath>
#include <limits>

#include "device_launch_parameters.h"
#include "cuda_runtime.h"
#include "ccl_le.h"

#define NOMINMAX

using namespace std;

__global__ void init_le(int L[], int R[], int N)
{
	int id = blockIdx.x * blockDim.x + blockIdx.y * blockDim.x * gridDim.x + threadIdx.x;
	if (id >= N)
		return;

	L[id] = R[id] = id;
}

__global__ void scanning(BYTE D[], int L[], int R[], bool* m, int N, int W)
{
	int id = blockIdx.x * blockDim.x + blockIdx.y * blockDim.x * gridDim.x + threadIdx.x;
	if (id >= N)
		return;

	int Did = D[id];

	int label = N;
	if (id - W >= 0 && D[id - W] > 0)
		label = L[id - W];

	int r = id % W;
	if (r && D[id - 1] > 0)
		label = L[id - 1];

	if (label < L[id])
	{
		//atomicMin(&R[L[id]], label);
		R[L[id]] = label;
		*m = true;
	}
}

__global__ void scanning8(BYTE D[], int L[], int R[], bool* m, int N, int W)
{
	int id = blockIdx.x * blockDim.x + blockIdx.y * blockDim.x * gridDim.x + threadIdx.x;
	if (id >= N) return;

	int Did = D[id];
	int label = N;
	if (id - W >= 0 && D[id - W] > 0)
		label = L[id - W];

	int r = id % W;
	if (r)
	{
		if (D[id - 1] > 0)
			label = min(label, L[id - 1]);
		if (id - W - 1 >= 0 && D[id - W - 1] > 0)
			label = min(label, L[id - W - 1]);
		if (id + W - 1 < N  && D[id + W - 1] > 0)
			label = min(label, L[id + W - 1]);
	}
	if (r + 1 != W)
	{
		if (D[id + 1] > 0)
			label = min(label, L[id + 1]);
		if (id - W + 1 >= 0 && D[id - W + 1] > 0)
			label = min(label, L[id - W + 1]);
		if (id + W + 1 < N  && D[id + W + 1] > 0)
			label = min(label, L[id + W + 1]);
	}

	if (label < L[id])
	{
		//atomicMin(&R[L[id]], label);
		R[L[id]] = label;
		*m = true;
	}
}

__global__ void analysis(BYTE D[], int L[], int R[], int N)
{
	int id = blockIdx.x * blockDim.x + blockIdx.y * blockDim.x * gridDim.x + threadIdx.x;
	if (id >= N) return;

	int label = L[id];
	int ref;
	if (label == id) {
		do { label = R[ref = label]; } while (ref ^ label);
		R[id] = label;
	}
}

__global__ void labeling(BYTE D[], int L[], int R[], int N)
{
	int id = blockIdx.x * blockDim.x + blockIdx.y * blockDim.x * gridDim.x + threadIdx.x;
	if (id >= N) return;

	L[id] = R[R[L[id]]];
}
