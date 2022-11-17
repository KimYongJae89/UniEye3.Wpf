using DynMvp.Vision;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using UniScanC.Struct;

namespace UniScanC.AlgoTask
{
    public class InspectBufferPool
    {
        public Size Size { get; private set; }
        public int Count { get; private set; }

        private ConcurrentQueue<AlgoImage> queue;
        private ConcurrentQueue<byte[]> bytesBufferQueue;

        public float Usage => (Count == 0) ? 0 : queue.Count * 1.0f / Count;

        public InspectBufferPool(Size size)
        {
            Size = size;
            queue = new ConcurrentQueue<AlgoImage>();
            bytesBufferQueue = new ConcurrentQueue<byte[]>();
        }

        public void Alloc(int count, int bytesCount, ImagingLibrary library)
        {
            Debug.WriteLine($"InspectBufferPool::Alloc - {count} + {bytesCount}");
            Count = count;
            for (int i = 0; i < count; i++)
            {
                AlgoImage algoImage = ImageBuilder.GetInstance(library).Build(ImageType.Grey, Size.Width, Size.Height);
                queue.Enqueue(algoImage);

            }
            for (int i = 0; i < bytesCount; i++)
            {
                byte[] bytes = new byte[Size.Width * Size.Height];
                bytesBufferQueue.Enqueue(bytes);
            }
        }

        public void Dispose()
        {
            Debug.WriteLine($"InspectBufferPool::Dispose - {queue.Count} + {bytesBufferQueue.Count}");
            while (queue.TryDequeue(out AlgoImage algoImage))
            {
                algoImage?.Dispose();
            }

            while (bytesBufferQueue.TryDequeue(out byte[] bytes))
            {
                ;
            }

            GC.Collect();
        }

        public byte[] RequestBytesBuffer()
        {
            if (bytesBufferQueue.TryDequeue(out byte[] bytes))
            {
                return bytes;
            }

            Debug.WriteLine($"InspectBufferPool::RequeseBytes Fail");
            return null;
        }

        public AlgoImage RequestBuffer()
        {
            if (queue.TryDequeue(out AlgoImage algoImage))
            {
                algoImage?.Clear();
                return algoImage;
            }

            Debug.WriteLine($"InspectBufferPool::RequestBuffer Fail");
            return null;
        }

        public bool RequestBuffers(AlgoImage[] array, int count)
        {
            //Debug.WriteLine($"InspectBufferPool::RequestBuffer - Remain: {queue.Count}");
            Debug.Assert(array.Length >= count);

            var arr = new AlgoImage[count];
            for (int i = 0; i < count; i++)
            {
                arr[i] = RequestBuffer();
            }

            if (Array.TrueForAll(arr, f => f != null))
            {
                Array.Copy(arr, array, count);
                return true;
            }

            Debug.WriteLine($"InspectBufferPool::RequestBuffers Fail");
            ReturnBuffers(arr);
            return false;
        }

        public void ReturnBytesBuffer(byte[] bytes)
        {
            if (bytes == null)
            {
                return;
            }

            bytesBufferQueue.Enqueue(bytes);
        }

        public void ReturnBuffer(AlgoImage algoImage)
        {
            if (algoImage == null)
            {
                return;
            }

            queue.Enqueue(algoImage);
            //Debug.WriteLine($"InspectBufferPool::ReturnBuffer - Remain: {queue.Count}");
        }

        public void ReturnBuffers(AlgoImage[] algoImages)
        {
            Array.ForEach(algoImages, f =>
            {
                if (f != null)
                {
                    ReturnBuffer(f);
                }
            });
        }

        //public bool RequestSet(InspectBufferSet inspectBufferSet)
        //{
        //    Debug.WriteLine($"InspectBufferPool::Request - FrameNo: {resultBuffer.FrameNo}");
        //    if (!SystemConfig.Instance.IsSaveDebugData)
        //        return true;

        //    inspectBufferSet.RoiBuffer = RequestBuffer();
        //    return inspectBufferSet.RoiBuffer != null;
        //}

        //public void ReturnSet(InspectBufferSet inspectBufferSet)
        //{
        //    Debug.WriteLine($"InspectBufferPool::Return - FrameNo: {resultBuffer.FrameNo}");

        //    ReturnBuffer(inspectBufferSet.RoiBuffer);
        //    inspectBufferSet.RoiBuffer = null;
        //}
    }
}
