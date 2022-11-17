using Matrox.MatroxImagingLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Devices.FrameGrabber
{
    public interface ICustomBuffer
    {
        IntPtr Ptr { get; }
        void Update(long width, long height);
        void Dispose();
    }

    public enum CustomBufferType
    {
        None, Mil
    }

    public class CustomBufferFactory
    {
        public static ICustomBuffer Create(CustomBufferType customBufferType)
        {
            switch (customBufferType)
            {
                case CustomBufferType.Mil:
                    return new CustomBufferMil();
            }

            //Debug.Assert(false, "Invalid NativeBufferType");

            return null;
        }
    }

    internal class CustomBufferMil : ICustomBuffer
    {
        private MIL_ID milId;

        public IntPtr Ptr
        {
            get
            {
                if (milId == MIL.M_NULL)
                {
                    return IntPtr.Zero;
                }
                else
                {
                    return (IntPtr)MIL.MbufInquire(milId, MIL.M_HOST_ADDRESS);
                }
            }
        }

        public void Dispose()
        {
            if (milId != MIL.M_NULL)
            {
                MIL.MbufFree(milId);
            }

            milId = MIL.M_NULL;
        }

        public void Update(long width, long height)
        {
            long attribute = MIL.M_ARRAY;

            //MIL_INT nonPageMemorySize = MIL.MappInquire(MIL.M_NON_PAGED_MEMORY_SIZE);
            //if (nonPageMemorySize > 0)
            //    attribute += MIL.M_NON_PAGED;

            milId = MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, width, height, 8 + MIL.M_UNSIGNED, attribute, MIL.M_NULL);
            MIL.MbufClear(milId, 0);
        }
    }
}
