#include "Defines.h"

__global__ void init_le(int L[], int R[], int N);

__device__ int diff_le(int d1, int d2);

__global__ void scanning(BYTE D[], int L[], int R[], bool * m, int N, int W);

__global__ void scanning8(BYTE D[], int L[], int R[], bool * m, int N, int W);

__global__ void analysis(BYTE D[], int L[], int R[], int N);

__global__ void labeling(BYTE D[], int L[], int R[], int N);
