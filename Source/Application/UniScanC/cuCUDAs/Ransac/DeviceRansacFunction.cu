#include "DeviceRansacFunction.cuh"
#include <curand_kernel.h>

__device__ int lock;

__device__ int kernel_GetCost(double* xArray, double* yArray, unsigned int size, double gradient, double centerX, double centerY, double distThreshold)
{
	double cost = 0;

	if (gradient != 0)
	{
		double lowerSide = sqrt(pow(gradient, 2) + 1);

		for (int i = 0; i < size; i++)
		{
			if (abs(((xArray[i] - centerX) * gradient) + centerY - yArray[i]) / lowerSide <= distThreshold)
				cost++;
		}
	}

	return cost;
}

__global__ void kernel_Compute(double* xArray, double* yArray, double* gradientArray, double* centerXArray, double* centerYArray, unsigned int* costArray, unsigned int size, int iter, double distThreshold, unsigned long seed)
{
	int id = blockIdx.x * blockDim.x + threadIdx.x;
	if (id >= iter)
		return;

	curandState state;
	curand_init(seed, id, 0, &state);

	int randNum1 = (int)(curand_uniform(&state) * (float)(size - 1));
	int randNum2 = (int)(curand_uniform(&state) * (float)(size - 1));

	double x1 = xArray[randNum1];
	double y1 = yArray[randNum1];

	double x2 = xArray[randNum2];
	double y2 = yArray[randNum2];

	centerXArray[id] = (x1 + x2) / 2.0;
	centerYArray[id] = (y1 + y2) / 2.0;

	if (x1 == x2)
		gradientArray[id] = DBL_MAX;
	else
		gradientArray[id] = (y2 - y1) / (x2 - x1);

	int cost = kernel_GetCost(xArray, yArray, size, gradientArray[id], centerXArray[id], centerYArray[id], distThreshold);
	
	costArray[id] = cost;
}