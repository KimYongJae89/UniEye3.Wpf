#include "Defines.h"

__global__ void init_dpl(int L[], int N);

__global__ void kerneldpl(int I, BYTE D[], int L[], bool* m, int N, int W);

__global__ void kernel8dpl(int I, BYTE D[], int L[], bool* m, int N, int W);
