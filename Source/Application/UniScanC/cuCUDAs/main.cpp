#include <GL/freeglut.h>
#include <chrono>

#include "helper_image.h"
#include "CudaFunction.h"
#include "NPP/NppProcessing.h"


void initializeData(char *file, unsigned char **pixels, unsigned int *width, unsigned int *height)
{
	GLint bsize;
	size_t file_length = strlen(file);
	unsigned int g_Bpp;

	if (!strcmp(&file[file_length - 3], "pgm"))
	{
		if (sdkLoadPGM<unsigned char>(file, pixels, width, height) != true)
		{
			printf("Failed to load PGM image file: %s\n", file);
			exit(EXIT_FAILURE);
		}

		g_Bpp = 1;
	}
}

bool SaveData(char *file, BYTE* pixels, unsigned int width, unsigned int height)
{
	return sdkSavePGM<BYTE>(file, pixels, width, height);
}

chrono::time_point<chrono::steady_clock> startTime;
void FunctionStart()
{
	startTime = std::chrono::high_resolution_clock::now();//::GetTickCount();
}

void FunctionStop(string s)
{
	cudaDeviceSynchronize();
	cout << std::chrono::duration_cast<std::chrono::nanoseconds>(std::chrono::high_resolution_clock::now() - startTime).count() / 1000 << " us - " << s <<"\n";
}

int main()
{
	unsigned char *pixels = NULL;
	unsigned int width = 12280;
	unsigned int height = 1250;

	int gpu = 0;
	CUDA_INITIALIZE(gpu);

	initializeData("D:\\CUDA_TEST\\16.pgm", &pixels, &width, &height);

	int image = CUDA_CREATE_IMAGE(width, height, sizeof(BYTE));
	int dstImage = CUDA_CREATE_IMAGE(width, height, sizeof(BYTE));

	CUDA_SET_IMAGE(image, pixels);
	//CUDA_SET_IMAGE(dstImage, pixels);

	CUDA_MEAN_FILTER(image, dstImage, 9);

	//CUDA_CREATE_PROFILE(image);
	//CUDA_ADAPTIVE_BINARIZE_LOWER(image, dstImage, 30);
	//CUDA_CANNY(dstImage, image, 30, 150);

	//float* profile = new float[10];&
	//CUDA_AREA_AVERAGE(image, 10, profile);
	//CUDA_AREA_BINARIZE(image, dstImage, 10, profile, 50, 255);

	BYTE* pBuffer = new BYTE[width * height];
	CUDA_GET_IMAGE(dstImage, pBuffer);

	SaveData("D:\\CUDA_TEST\\16_RESULT.pgm", pBuffer, width, height);

	delete[] pBuffer;
	//delete[] profile;

	CUDA_FREE_IMAGE(image);
	CUDA_FREE_IMAGE(dstImage);

	CUDA_RELEASE();
}
