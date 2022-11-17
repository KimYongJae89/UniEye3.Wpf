#include "Defines.h"

__global__ void kernel_Compute(double* xArray, double* yArray, double* gradientArray, double* centerXArray, double* centerYArray, unsigned int* sharedCost, unsigned int size, int iter, double distThreshold, unsigned long seed);