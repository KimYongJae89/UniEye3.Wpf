/* 
   Dummy Includes for cutil_inline.h
   Wei LI <kuantkid <at> gmail <dot> com>
 */
#include <helper_cuda.h>
#include "cuda_runtime.h"
#include "device_launch_parameters.h"

#define cutilCheckMsg(a) getLastCudaError(a)
#define cutGetMaxGflopsDeviceId() gpuGetMaxGflopsDeviceId()

#define MIN(a,b) (a) < (b) ? (a) : (b)
