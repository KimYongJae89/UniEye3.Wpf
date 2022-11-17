#include "Defines.h"

class CudaCCL
{
private:
	BYTE* Dd;
	int* Ld;
	int* Rd;

	int m_Width;
	int m_Height;

public:
	void ccl_dpl(BYTE* srcImage, int* dstImage, int W, int H, int degree_of_connectivity);
	void ccl_np(BYTE* srcImage, int* dstImage, int W, int H, int degree_of_connectivity);
	void ccl_le(BYTE* srcImage, int* dstImage, int W, int H, int degree_of_connectivity);
	int ccl_hm(BYTE * binImage, int * labelImage, BYTE * maskImage, int W, int H);
	bool blob_hm(BYTE * sourceImage, int * labelImage, int W, int H, int count, unsigned int* areaArray, unsigned int* xMinArray, unsigned int* xMaxArray, unsigned int* yMinArray, unsigned int* yMaxArray, unsigned int* vMinArray, unsigned int* vMaxArray, float* vMeanArray);
};
