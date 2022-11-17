#include "CudaRansac.h"
#include "DeviceRansacFunction.cuh"

bool CudaRansac::Compute(int width, int height, double* xArray, double* yArray, int size, double* cost, double* gradient, double* centerX, double* centerY, double threshold)
{
	double offset;
	LineCompute(xArray, yArray, size, gradient, centerX, centerY);

	vector<POINT> inliers;

	*cost = CalcDistance(xArray, yArray, size, *gradient, *centerX, *centerY, threshold);

	double val1 = log(1 - pow((double)*cost / (double)size, 3));
	double val2 = 1 + log(1 - 0.9999);

	int numInteration = (int)round(val2 / val1);
	
	if (numInteration <= 1)
		return true;

	double* dXArray;
	double* dYArray;

	unsigned int* dCostArray;
	double* dGradientArray;
	double* dCenterXArray;
	double* dCenterYArray;

	cudaMalloc(&dXArray, size * sizeof(double));
	cudaMalloc(&dYArray, size * sizeof(double));

	cudaMalloc(&dCostArray, numInteration * sizeof(unsigned int));
	cudaMalloc(&dGradientArray, numInteration * sizeof(double));
	cudaMalloc(&dCenterXArray, numInteration * sizeof(double));
	cudaMalloc(&dCenterYArray, numInteration * sizeof(double));
	
	cudaError_t cudaStatus;
	CUDA_ERROR_CHECK(cudaStatus);

	cudaMemcpy(dXArray, xArray, size * sizeof(double), cudaMemcpyHostToDevice);
	cudaMemcpy(dYArray, yArray, size * sizeof(double), cudaMemcpyHostToDevice);

	unsigned long seed = time(0);
	int threadNum = 256;
	int blockNum = numInteration % threadNum == 0 ? numInteration / threadNum : numInteration / threadNum + 1;

	kernel_Compute << <blockNum, threadNum >> > (dXArray, dYArray, dGradientArray, dCenterXArray, dCenterYArray, dCostArray, size, numInteration, threshold, seed);

	CUDA_ERROR_CHECK(cudaStatus);

	unsigned int* costArray = new unsigned int[numInteration];
	double* gradientArray = new double[numInteration];
	double* centerXArray = new double[numInteration];
	double* centerYArray = new double[numInteration];
	cudaMemcpy(costArray, dCostArray, numInteration * sizeof(unsigned int), cudaMemcpyDeviceToHost);
	cudaMemcpy(gradientArray, dGradientArray, numInteration * sizeof(double), cudaMemcpyDeviceToHost);
	cudaMemcpy(centerXArray, dCenterXArray, numInteration * sizeof(double), cudaMemcpyDeviceToHost);
	cudaMemcpy(centerYArray, dCenterYArray, numInteration * sizeof(double), cudaMemcpyDeviceToHost);

	unsigned int maxCost = *cost;
	int maxIndex;
	for (int i = 0; i < numInteration; i++)
	{
		if (costArray[i] > maxCost)
		{
			maxCost = costArray[i];
			maxIndex = i;
		}
	}

	if (maxCost > *cost)
	{
		*cost = maxCost;
		*gradient = gradientArray[maxIndex];
		*centerX = centerXArray[maxIndex];
		*centerY = centerYArray[maxIndex];
	}

	cudaFree(dCostArray);

	cudaFree(dXArray);
	cudaFree(dYArray);
	cudaFree(dGradientArray);
	cudaFree(dCenterXArray);
	cudaFree(dCenterYArray);

	free(costArray);
	free(gradientArray);
	free(centerXArray);
	free(centerYArray);

	CUDA_ERROR_CHECK(cudaStatus);

	return cudaStatus == cudaSuccess;
}

void CudaRansac::LineCompute(double* xArray, double* yArray, int size, double* gradient, double* centerX, double* centerY)
{
	double avgX = 0;
	double avgY = 0;
	for (int i = 0; i < size; i++)
	{
		avgX += xArray[i];
		avgY += yArray[i];
	}

	avgX /= (double)size;
	avgY /= (double)size;

	double upper = 0;
	double lower = 0;

	for (int i = 0; i < size; i++)
	{
		upper += (yArray[i] - avgY) * (xArray[i] - avgX);
		lower += pow(xArray[i] - avgX, 2);
	}

	*gradient = upper / lower;

	//*offset = avgY - (*gradient) * avgX;
	*centerX = avgX;
	*centerY = avgY;
}

int CudaRansac::CalcDistance(double* xArray, double* yArray, int size, double gradient, double centerX, double centerY, double distThreshold)
{
	double cost = 0;

	if (gradient == NULL)
	{
		for (int i = 0; i < size; i++)
		{
			double dist = abs(xArray[i] - centerX);
			if (dist < distThreshold)
				cost++;
		}
	}
	else
	{
		double lowerSide = sqrt(pow(gradient, 2) + 1);

		for (int i = 0; i < size; i++)
		{
			double dist = abs((xArray[i] - centerX) * gradient + centerY - yArray[i]) / lowerSide;

			if (dist < distThreshold)
				cost++;
		}
	}

	return cost;
}
