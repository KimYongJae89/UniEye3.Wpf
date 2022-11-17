#include "Defines.h"

__global__ void init_np(int L[], int N);

__global__ void kernel(BYTE D[], int L[], bool * m, int N, int W);

__global__ void kernel8(BYTE D[], int L[], bool * m, int N, int W);