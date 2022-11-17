#include "Defines.h"

__global__ void kernel_mask_hm(BYTE D[], BYTE M[], int W, int H);
__global__ void kernel_labeling_hm(BYTE D[], BYTE M[], int I[], int W, int H);
__global__ void init_label_map(BYTE M[], int L[], int N);
__global__ void find_horizental_hm(BYTE M[], int L[], int W, int H, int block_dist, bool* changed);
__global__ void find_vertical_hm(BYTE M[], int L[], int W, int H, int block_dist, bool* changed);
__global__ void indexing_hm(BYTE M[], int L[], int N);
__global__ void counting_root_hm(int L[], int N, int count[]);
__global__ void labeling_root_hm(int L[], int N, int count[]);
__global__ void labeling_child_hm(int L[], int N);
__global__ void kernel_blob_hm(BYTE S[], int L[], int N, int W, unsigned int* areaArray, unsigned int* xMinArray, unsigned int* xMaxArray, unsigned int* yMinArray, unsigned int* yMaxArray, unsigned int* vMinArray, unsigned int* vMaxArray, float* vMeanArray);
