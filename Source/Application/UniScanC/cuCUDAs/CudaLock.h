#include "Defines.h"

/***************/
/* LOCK STRUCT */
/***************/
struct CudaLock
{
	int *d_state;

	// --- Constructor
	CudaLock(void) {
		int h_state = 0;													// --- Host side lock state initializer
		cudaMalloc((void **)&d_state, sizeof(int));							// --- Allocate device side lock state
		cudaMemcpy(d_state, &h_state, sizeof(int), cudaMemcpyHostToDevice);	// --- Initialize device side lock state
	}

	// --- Destructor
	__host__ __device__ ~CudaLock(void) {
#if !defined(__CUDACC__)
		cudaFree(d_state);
#else

#endif  
	}

	// --- Lock function
	__device__ void lock(void) { while (atomicCAS(d_state, 0, 1) != 0); }

	// --- Unlock function
	__device__ void unlock(void) { atomicExch(d_state, 0); }
};