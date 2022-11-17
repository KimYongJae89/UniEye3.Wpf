#ifndef _CUDA_DEFINES_H_
#define _CUDA_DEFINES_H_

#include <Windows.h>
#include <vector>
#include <map>
#include <thrust/device_vector.h>
#include <cufft.h>
#include <device_functions.h>
#include "device_launch_parameters.h"
#include "cuda_runtime.h"
#include "host_defines.h"
#include "device_functions.h"

static UINT UNIQUE_IMAGE_NO = 1;
static UINT UNIQUE_BLOB_NO = 1;

// 128 - 512 범위의 스레드 블록 크기 선택은 전술한 문제에 부딪힐 가능성이 낮다.
// 일반적으로 블록당 128개의 스레드 선택과 블록당 256개의 스레드 선택 간에 코드의 성능 차이는 크지 않다.
// CUDA 내부 호환성 때문에 항상 32의 배수로 크기를 선택하는 것을 권장한다.
static UINT MAX_GPU_THREAD_NUM = 256;

#ifdef _MSC_VER
#include <ctime>

inline double get_time()
{
	return static_cast<double>(std::clock()) / CLOCKS_PER_SEC;
}
#else
#include <sys/time.h>
#include "ccl_np.h"
inline double get_time()
{
	timeval tv;
	gettimeofday(&tv, 0);
	return tv.tv_sec + 1e-6 * tv.tv_usec;
}
#endif

//CUDA 인덱싱
//1차원 데이터(x only)	blockDim.x * blockIdx.x + threadIdx.x
//2차원 데이터(x, y)	blockDim.x * blockIdx.x + threadIdx.x, blockDim.y * blockIdx.y + threadIdx.y
//2차원 데이터(x only)	N.x * blockDim.y * blockIdx.y + N.x * threadIdx.y + blockDim.x * blockIdx.x + blockIdx.x

// CUDA Block 계산법
// kernel<<grid, block>>

inline void GetThreadNum(dim3& grid, dim3& threads, int width, int height)
{
	int root = static_cast<int>(sqrt(MAX_GPU_THREAD_NUM));

	grid.x = width % root == 0 ?  width / root : width / root + 1;
	grid.y = height % root == 0 ? height / root : height / root + 1;
	grid.z = 1;

	threads.x = root;
	threads.y = root;
	threads.z = 1;

	//int block = static_cast<int>(sqrt(static_cast<double>(width * height) / MAX_GPU_THREAD_NUM)) + 1;

	//grid.x = block;
	//grid.y = block;
	//grid.z = 1;

	//threads.x = MAX_GPU_THREAD_NUM;
	//threads.y = 1;
	//threads.z = 1;
}

inline dim3 GetThreadGrid(int dataWidth, int dataHeight, int threadX, int threadY)
{
	dim3 grid_dimension;
	grid_dimension.x = (dataWidth + threadX - 1) / threadX;
	grid_dimension.y = (dataHeight + threadY - 1) / threadY;

	return grid_dimension;
}

using namespace std;
using namespace thrust;

enum EdgeSearchDirection
{
	Horizontal = 0,
	Vertical
};

typedef thrust::host_vector<int> IndexList;

struct CudaBlob
{
	int label;
	IndexList xList;
	IndexList yList;
};

typedef map<int, CudaBlob*> CudaBlobList;
//typedef vector<CudaBlob*> CudaBlobList;

#define CUDA_ERROR_CHECK(x) x = cudaGetLastError(); assert(x == cudaSuccess)
							
#endif