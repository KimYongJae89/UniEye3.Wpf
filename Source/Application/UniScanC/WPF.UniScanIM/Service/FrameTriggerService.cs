using DynMvp.Base;
using DynMvp.Devices.FrameGrabber;
using Emgu.CV;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using UniScanC.Data;
using WPF.UniScanIM.Override;

namespace WPF.UniScanIM.Service
{
    public static class FrameTriggerService
    {
        public delegate void OnInspectFrameGrabbedDelegate(ModuleInfo moduleInfo, Image2D image);
        public static event OnInspectFrameGrabbedDelegate OnInspectFrameGrabbed;

        #region 속성
        private static Dictionary<ModuleInfo, ulong> FrameId { get; set; } = new Dictionary<ModuleInfo, ulong>();
        private static Dictionary<ModuleInfo, AutoResetEvent> FlushEvent { get; set; } = new Dictionary<ModuleInfo, AutoResetEvent>();
        private static Dictionary<ModuleInfo, ConcurrentQueue<Image2D>> FragmentImageQueue { get; set; } = new Dictionary<ModuleInfo, ConcurrentQueue<Image2D>>();

        private static Task TriggerChangedTask;
        private static bool TaskStopFlag;
        #endregion

        #region 메서드
        // 이미지 크기 대비 버퍼 크기 - 이 이상 Count가 쌓이면 강제 Flush 수행.
        public static int GetMaxQueueSize(ModuleInfo moduleInfo)
        {
            return moduleInfo.BufferHeight / moduleInfo.Camera.ImageSize.Height;
        }

        public static void Start()
        {
            var deviceMonitor = DeviceMonitor.Instance() as DeviceMonitor;
            deviceMonitor.FrameTriggerChanged += OnFrameTriggerChangedProc;

            TaskStopFlag = false;
            TriggerChangedTask = Task.Run(new Action(TriggerChangedTaskProc));
        }

        public static void Stop()
        {
            TaskStopFlag = true;
            TriggerChangedTask.Wait(1000);
            TriggerChangedTask = null;

            ClearQueue();

            var deviceMonitor = DeviceMonitor.Instance() as DeviceMonitor;
            deviceMonitor.FrameTriggerChanged -= OnFrameTriggerChangedProc;
        }

        private static void ClearQueue()
        {
            FragmentImageQueue.Clear();
            FlushEvent.Clear();
            FrameId.Clear();
        }

        private static void TriggerChangedTaskProc()
        {
            while (!TaskStopFlag)
            {
                Thread.Sleep(10);
                ModuleInfo[] moduleInfos = FlushEvent.Keys.ToArray();
                foreach (ModuleInfo moduleInfo in moduleInfos)
                {
                    // io 받아서 Flush
                    if (FlushEvent[moduleInfo].WaitOne(0))
                    {
                        LogHelper.Debug(LoggerType.Inspection, $"FrameTriggerService::TriggerChangedTaskProc - Flush Queue with Event.");
                        FlushQueue(moduleInfo);
                    }

                    // 크기가 차서 Flush
                    int maxSize = GetMaxQueueSize(moduleInfo);
                    if (FragmentImageQueue[moduleInfo].Count > maxSize)
                    {
                        LogHelper.Debug(LoggerType.Inspection, $"FrameTriggerService::TriggerChangedTaskProc - Flush Queue with Size.");
                        FlushQueue(moduleInfo);
                    }
                }
            }
        }

        private static void FlushQueue(ModuleInfo moduleInfo)
        {
            Image2D frameImage = FrameTriggerService.GetFrameImage(moduleInfo);

            var cameraBufferTag = (CameraBufferTag)frameImage.Tag;
            int frameNo = (int)cameraBufferTag.FrameId;
            Size size = cameraBufferTag.FrameSize;
            LogHelper.Debug(LoggerType.Inspection, $"FrameTriggerService::FlushQueue - Topic: {moduleInfo.Topic}, FrameId: {frameNo}, Size: {size}");
            OnInspectFrameGrabbed?.Invoke(moduleInfo, frameImage);
        }

        private static void OnFrameTriggerChangedProc(bool value)
        {
            if (value)
            {
                foreach (AutoResetEvent autoResetEvent in FlushEvent.Values)
                {
                    autoResetEvent.Set();
                }
            }
        }

        /// <summary>
        /// 검사 모드에서 카메라 이미지 그랩 콜백
        /// </summary>
        /// <param name="camera"></param>
        internal static void InspectImageGrabbed(Camera camera)
        {
            var image = camera.GetGrabbedImage() as Image2D;
            ModuleInfo moduleInfo = SystemConfig.Instance.ModuleList.FirstOrDefault(f => f.Camera == camera);

            Enqueue(moduleInfo, image);
        }

        private static void Enqueue(ModuleInfo moduleInfo, Image2D image)
        {
            if (!FragmentImageQueue.ContainsKey(moduleInfo))
            {
                FragmentImageQueue.Add(moduleInfo, new ConcurrentQueue<Image2D>());
            }

            FragmentImageQueue[moduleInfo].Enqueue(image);

            if (!FlushEvent.ContainsKey(moduleInfo))
            {
                FlushEvent.Add(moduleInfo, new AutoResetEvent(false));
            }

            if (!FrameId.ContainsKey(moduleInfo))
            {
                FrameId.Add(moduleInfo, 0);
            }
        }

        public static Image2D GetFrameImage(ModuleInfo moduleInfo)
        {
            if (!FragmentImageQueue.ContainsKey(moduleInfo))
            {
                return null;
            }

            ConcurrentQueue<Image2D> imageQueue = FragmentImageQueue[moduleInfo];
            if (imageQueue.Count == 0)
            {
                return null;
            }

            // Flush Queue 개수: '버퍼에 담을 수 있는 최대 이미지 수'와 '현재 큐에 있는 이미지 수' 중 작은 수.
            // lock 하지 않음 - BlockCopy는 Service Task에서 수행. 다른 스레드의 Enqueue와는 독립적으로 작동.
            int allCount = imageQueue.Count;
            int maxCount = GetMaxQueueSize(moduleInfo);
            int count = Math.Min(maxCount, allCount);

            Size cameraSize = moduleInfo.Camera.ImageSize;
            int w = moduleInfo.BufferWidth;
            int h = moduleInfo.BufferHeight;
            byte[] data = new byte[w * h];
            for (int i = 0; i < count; i++)
            {
                imageQueue.TryDequeue(out Image2D image);
                image.ConvertFromDataPtr();
                Buffer.BlockCopy(image.Data, 0, data, i * image.Data.Length, image.Data.Length);
            }
            int tagHeight = cameraSize.Height * count;
            if (moduleInfo.Camera.CameraInfo.GrabberType == GrabberType.Virtual)
            {
                var virtualHeightCheck = new Mat(h, w, Emgu.CV.CvEnum.DepthType.Cv8U, 1);
                Marshal.Copy(data, 0, virtualHeightCheck.DataPointer, w * h);
                for (int i = h - 1; i > 0; --i)
                {
                    int sumData = (int)CvInvoke.Sum(virtualHeightCheck.Row(i)).V0;
                    if (sumData != 0)
                    {
                        tagHeight = i + 1;
                        break;
                    }
                }
            }
            var frameImage = new Image2D();
            frameImage.Initialize(w, h, moduleInfo.Camera.NumOfBand, w, data);
            var tagSize = new Size(frameImage.Width, tagHeight);
            frameImage.Tag = new CameraBufferTag(0, FrameId[moduleInfo]++, 0, tagSize);
            return frameImage;
        }
        #endregion
    }
}
