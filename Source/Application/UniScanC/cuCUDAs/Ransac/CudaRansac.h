#ifndef _CUDA_Ransac_H_
#define _CUDA_Ransac_H_

#include "Defines.h"
#include "CudaImage.h"

class CudaRansac
{
public:
	bool Compute(int width, int height, double* xArray, double* yArray, int size, double* cost, double* gradient, double* centerX, double* centerY, double threshold);
private:
	void LineCompute(double* xArray, double* yArray, int size, double* gradient, double* centerX, double* centerY);
	int CalcDistance(double* xArray, double* yArray, int size, double gradient, double centerX, double centerY, double distThreshold);
};

#endif
#pragma once
