using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Vision.Cuda
{
    public static class CudaMethods
    {
        public enum EdgeSearchDirection
        {
            Horizontal = 0,
            Vertical
        };

        public enum InterpolationMode
        {
            UNDEFINED = 0,
            NN = 1,                         //  Nearest neighbor filtering. 
            LINEAR = 2,                     //  Linear interpolation. 
            CUBIC = 4,                      //  Cubic interpolation. 
            CUBIC2P_BSPLINE,                //  Two-parameter cubic filter (B=1, C=0) 
            CUBIC2P_CATMULLROM,             //  Two-parameter cubic filter (B=0, C=1/2) 
            CUBIC2P_B05C03,                 //  Two-parameter cubic filter (B=1/2, C=3/10) 
            SUPER = 8,                      //  Super sampling. 
            LANCZOS = 16,                   //  Lanczos filtering. 
            LANCZOS3_ADVANCED = 17,         //  Generic Lanczos filtering with order 3. 
            SMOOTH_EDGE = 0x8000000    //<  Smooth edge filtering.
        }

        [DllImport("cuCUDAs.dll")]
        public static extern bool CUDA_INITIALIZE(ref int gpuNo);
        [DllImport("cuCUDAs.dll")]
        public static extern void CUDA_RELEASE();
        [DllImport("cuCUDAs.dll")]
        public static extern void CUDA_THREAD_NUM(int threadNum);

        [DllImport("cuCUDAs.dll")]
        public static extern void CUDA_WAIT();

        [DllImport("cuCUDAs.dll")]
        public static extern uint CUDA_CREATE_IMAGE(int width, int height, int depth);
        [DllImport("cuCUDAs.dll")]
        public static extern bool CUDA_CLIP_IMAGE(uint srcImage, uint dstImage, int x, int y, int width, int height);
        [DllImport("cuCUDAs.dll")]
        public static extern bool CUDA_SET_IMAGE(uint image, IntPtr pImageBuffer);
        [DllImport("cuCUDAs.dll")]
        public static extern void CUDA_FREE_IMAGE(uint image);
        [DllImport("cuCUDAs.dll")]
        public static extern bool CUDA_GET_IMAGE(uint image, IntPtr pDstBuffer);
        [DllImport("cuCUDAs.dll")]
        public static extern void CUDA_SET_ROI(uint image, double x, double y, double width, double height);
        [DllImport("cuCUDAs.dll")]
        public static extern bool CUDA_CLEAR_IMAGE(uint image);
        [DllImport("cuCUDAs.dll")]
        public static extern void CUDA_RESET_ROI(uint image);
        [DllImport("cuCUDAs.dll")]
        public static extern bool CUDA_CREATE_PROFILE(uint srcImage);

        [DllImport("cuCUDAs.dll")]
        public static extern bool CUDA_AREA_AVERAGE(uint srcImage, int areaCount, float[] hostAverage);
        [DllImport("cuCUDAs.dll")]
        public static extern bool CUDA_CREATE_LABEL_BUFFER(uint srcImage);

        // Edge Detect
        [DllImport("cuCUDAs.dll")]
        public static extern bool CUDA_EDGE_DETECT(uint srcImage, EdgeSearchDirection dir, int threshold, ref int startPos, ref int endPos);

        // Binarize
        [DllImport("cuCUDAs.dll")]
        public static extern bool CUDA_BINARIZE(uint srcImage, uint dstImage, float lower, float upper, bool inverse = false);
        [DllImport("cuCUDAs.dll")]
        public static extern bool CUDA_BINARIZE_LOWER(uint srcImage, uint dstImage, float lower, bool inverse = false);
        [DllImport("cuCUDAs.dll")]
        public static extern bool CUDA_BINARIZE_UPPER(uint srcImage, uint dstImage, float upper, bool inverse = false);

        // Vertical adaptive
        [DllImport("cuCUDAs.dll")]
        public static extern bool CUDA_ADAPTIVE_BINARIZE(uint srcImage, uint dstImage, float lower, float upper, bool inverse = false);
        [DllImport("cuCUDAs.dll")]
        public static extern bool CUDA_ADAPTIVE_BINARIZE_LOWER(uint srcImage, uint dstImage, float lower, bool inverse = false);
        [DllImport("cuCUDAs.dll")]
        public static extern bool CUDA_ADAPTIVE_BINARIZE_UPPER(uint srcImage, uint dstImage, float upper, bool inverse = false);

        [DllImport("cuCUDAs.dll")]
        public static extern bool CUDA_AREA_BINARIZE(uint srcImage, uint dstImage, int areaCount, float[] profile, float lower, float upper, bool inverse = false);

        // Blob
        [DllImport("cuCUDAs.dll")]
        public static extern int CUDA_LABELING(uint binImage);
        [DllImport("cuCUDAs.dll")]
        public static extern bool CUDA_BLOBING(uint binImage, uint srcImage, int count,
            uint[] areaArray, uint[] xMinArray, uint[] xMaxArray, uint[] yMinArray, uint[] yMaxArray,
            uint[] vMinArray, uint[] vMaxArray, float[] vMeanArray);

        // Morphology
        [DllImport("cuCUDAs.dll")]
        public static extern bool CUDA_MORPHOLOGY_ERODE(uint srcImage, uint dstImage, int maskSize);
        [DllImport("cuCUDAs.dll")]
        public static extern bool CUDA_MORPHOLOGY_DILATE(uint srcImage, uint dstImage, int maskSize);
        [DllImport("cuCUDAs.dll")]
        public static extern bool CUDA_MORPHOLOGY_OPEN(uint srcImage, uint dstImage, int maskSize);
        [DllImport("cuCUDAs.dll")]
        public static extern bool CUDA_MORPHOLOGY_CLOSE(uint srcImage, uint dstImage, int maskSize);

        [DllImport("cuCUDAs.dll")]
        public static extern bool CUDA_MORPHOLOGY_THINNING(uint srcImage);

        [DllImport("cuCUDAs.dll")]
        public static extern bool CUDA_CANNY(uint srcImage, uint dstImage, int lowThreshold, int highThreshold);

        [DllImport("cuCUDAs.dll")]
        public static extern bool Save(uint srcImage, char[] fileName);

        [DllImport("cuCUDAs.dll")]
        public static extern bool CUDA_EDGE_FINDER(uint srcImage, uint dstImage, float threshold, int arraySize, bool inverse = false);

        [DllImport("cuCUDAs.dll")]
        public static extern bool CUDA_MEAN_FILTER(uint srcImage, uint dstImage, int filterSize);

        [DllImport("cuCUDAs.dll")]
        public static extern bool CUDA_RESIZE(uint srcImage, uint dstImage, double scaleFactor, int interpolation);

        [DllImport("cuCUDAs.dll")]
        public static extern bool CUDA_MATH_MUL(uint srcImage, float[] profile);
    }

}
