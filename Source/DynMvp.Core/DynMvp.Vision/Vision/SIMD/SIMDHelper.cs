using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Vision.Vision.SIMD
{
    public static class SIMDHelper
    {
        private const string dllName = "UniScanG.Vision.dll";

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CalibrateLine(ImageData src, ImageData dst, ImageData mul100, [MarshalAs(UnmanagedType.I4)] int width, int heigth);

        [StructLayout(LayoutKind.Sequential)]
        private struct ImageData
        {
            private IntPtr ptr;

            [MarshalAs(UnmanagedType.I4)]
            private int pitch;

            public ImageData(IntPtr ptr, int pitch)
            {
                this.ptr = ptr;
                this.pitch = pitch;
            }
        }

        public static void IterateProduct(IEnumerable<byte> datas, IEnumerable<byte> coffs, IEnumerable<byte> results, Size size)
        {
            IterateProduct(datas, coffs, results, size.Width, size.Height);
        }

        public static void IterateProduct(IEnumerable<byte> datas, IEnumerable<byte> coffs, IEnumerable<byte> results, int width, int heigth)
        {
            Debug.Assert(datas.Count() == results.Count());

            var gcHandleSrc = GCHandle.Alloc(datas, GCHandleType.Pinned);
            var gcHandleDst = GCHandle.Alloc(results, GCHandleType.Pinned);
            var gcHandleMul = GCHandle.Alloc(coffs, GCHandleType.Pinned);

            IntPtr srcPtr = gcHandleSrc.AddrOfPinnedObject();
            IntPtr dstPtr = gcHandleDst.AddrOfPinnedObject();
            IntPtr mulPtr = gcHandleMul.AddrOfPinnedObject();

            IterateProduct(srcPtr, mulPtr, dstPtr, width, heigth);

            gcHandleMul.Free();
            gcHandleDst.Free();
            gcHandleSrc.Free();
        }

        public static void IterateProduct(IntPtr datas08, IntPtr coffs08, IntPtr results08, int pitch, int heigth)
        {
            var src = new ImageData(datas08, pitch);
            var mul = new ImageData(coffs08, pitch);
            var dst = new ImageData(results08, pitch);

            CalibrateLine(src, dst, mul, pitch, heigth);
        }
    }
}
