using DynMvp.Base;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using UniScanC.Data;
using UniScanC.Enums;
using UniScanC.Helpers;
using UniScanC.Models;
using WPF.UniScanIM.Override;

namespace WPF.UniScanIM.Service
{
    public static class IMLightCalibrationService
    {
        #region 속성
        public static Dictionary<ModuleInfo, Buffer> BufferDic { get; private set; }
        #endregion


        #region 메서드
        // 시퀀스 시작. 카메라 초기화 등.
        public static void LightCalibrationStart(double lineSpeed)
        {
            var moduleInfoList = SystemConfig.Instance.ModuleList.ToList();
            BufferDic = new Dictionary<ModuleInfo, Buffer>();

            try
            {
                moduleInfoList.ForEach(f =>
                {
                    float grabHz = UnitConvertor.GetGrabHz(lineSpeed, f.ResolutionHeight);
                    f.Camera.SetAcquisitionLineRate(grabHz);

                    var buffer = new Buffer();
                    buffer.AlgoImage = ImageBuilder.MilImageBuilder.Build(ImageType.Grey, f.Camera.ImageSize.Width, f.Camera.ImageSize.Height);
                    buffer.LightValueList = new List<int>();
                    buffer.ProfileList = new List<float[]>();
                    buffer.EdgeProfileList = new List<float[]>();
                    buffer.ImageAverageGreyValueList = new List<float>();
                    buffer.ManualResetEventDone = new ManualResetEvent(false);

                    BufferDic.Add(f, buffer);
                });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        // 한 장 그랩
        public static void LightCalibrationGrab(int lightValue)
        {
            var moduleInfoList = SystemConfig.Instance.ModuleList.ToList();
            moduleInfoList.ForEach(f =>
            {
                _ = BufferDic[f].ManualResetEventDone.Reset();
                BufferDic[f].LightValueList.Add(lightValue);

                f.Camera.ImageGrabbed += LightCalibrationImageGrabbed;
                f.Camera.GrabOnceAsync();
            });

            EventWaitHandle[] handles = BufferDic.Values.Select(f => f.ManualResetEventDone).ToArray();
            if (WaitHandle.WaitAll(handles))
            {

            }
            else
            {
                moduleInfoList.ForEach(f => f.Camera.Stop());
                LogHelper.Debug(LoggerType.Grab, $"IMLightCalibrationService::LightCalibrationGrab - autoResetEvent Timeout {DateTime.Now:HH:mm:ss.fff}");
            }
        }

        public static void LightCalibrationImageGrabbed(Camera camera)
        {
            camera.ImageGrabbed -= LightCalibrationImageGrabbed;
            if (!(camera.GetGrabbedImage() is Image2D image))
            {
                LogHelper.Debug(LoggerType.Grab, $"IMLightCalibrationService::LightCalibrationImageGrabbed - image.DataPtr == IntPtr.Zero {DateTime.Now:HH:mm:ss.fff}");
                return;
            }

            if (!(ModelManager.Instance().CurrentModel is Model currentModel))
            {
                return;
            }

            var tag = (CameraBufferTag)image.Tag;
            ModuleInfo moduleInfo = SystemConfig.Instance.ModuleList.First(f => f.Camera == camera);
            VisionModel visionModel = currentModel.VisionModels[moduleInfo.ModuleNo];
            Buffer buffer = BufferDic[moduleInfo];
            byte[] data;
            if (image.DataPtr == IntPtr.Zero)
            {
                data = image.Data;
            }
            else
            {
                data = new byte[image.Width * image.Height];
                for (int h = 0; h < image.Height; ++h)
                {
                    Marshal.Copy(IntPtr.Add(image.DataPtr, image.Pitch * h), data, image.Width * h, image.Width);
                }
            }
            image.Dispose();

            buffer.AlgoImage.SetByte(data);
            if (SystemConfig.Instance.IsSaveDebugData)
            {
                buffer.AlgoImage.Save(string.Format("C:\\Image\\lightCal_{0}.JPG", buffer.ProfileList.Count));
            }

            // 프레임 마진으로 이미지 자르기
            var roiRectangle = Rectangle.Round(RectangleF.FromLTRB(
                UnitConvertor.Um2Px(visionModel.FrameMarginL, moduleInfo.ResolutionWidth),
                UnitConvertor.Um2Px(visionModel.FrameMarginT, moduleInfo.ResolutionHeight),
                tag.FrameSize.Width - UnitConvertor.Um2Px(visionModel.FrameMarginR, moduleInfo.ResolutionWidth),
                tag.FrameSize.Height - UnitConvertor.Um2Px(visionModel.FrameMarginB, moduleInfo.ResolutionHeight)
                ));

            // 잘려진 이미지로 프로파일 획득
            using (AlgoImage algoImage = buffer.AlgoImage.GetChildImage(roiRectangle))
            {
                // 프로젝션하여 프로파일 만들기
                ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(algoImage);
                float[] profile = imageProcessing.Projection(algoImage, TwoWayDirection.Horizontal, ProjectionType.Mean);
                buffer.ProfileList.Add(profile);

                // 프로파일에서 엣지 획득
                int threshold = (int)Math.Round((visionModel.TargetIntensity + visionModel.OutTargetIntensity) / 2.0f);
                (int leftEdge, int rightEdge) = UniScanC.Algorithm.Simple.HorizentalEdgeFinder.Find(profile, moduleInfo.CamPos, threshold);
                if (moduleInfo.CamPos == ECamPosition.Mid || moduleInfo.CamPos == ECamPosition.Right)
                {
                    leftEdge = 0;
                }
                if (moduleInfo.CamPos == ECamPosition.Mid || moduleInfo.CamPos == ECamPosition.Left)
                {
                    rightEdge = profile.Length;
                }

                // 엣지를 제외한 부분 추출
                float[] edgeProfile = new float[rightEdge - leftEdge];
                Array.Copy(profile, leftEdge, edgeProfile, 0, rightEdge - leftEdge);
                buffer.EdgeProfileList.Add(edgeProfile);

                // 프로파일의 평균 값을 리스트에 추가
                buffer.ImageAverageGreyValueList.Add(edgeProfile.Average());
            }
            // 작업 완료
            _ = buffer.ManualResetEventDone.Set();
        }

        public static void LightCalibrationFinish()
        {
            foreach (Buffer buffer in BufferDic.Values)
            {
                buffer.AlgoImage.Dispose();
                buffer.LightValueList.Clear();
                buffer.ProfileList.Clear();
                buffer.EdgeProfileList.Clear();
                buffer.ImageAverageGreyValueList.Clear();
                buffer.ManualResetEventDone.Dispose();
            }
        }
        #endregion


        #region 구조체
        public struct Buffer
        {
            public AlgoImage AlgoImage;
            public List<int> LightValueList;
            public List<float[]> ProfileList;
            public List<float[]> EdgeProfileList;
            public List<float> ImageAverageGreyValueList;
            public ManualResetEvent ManualResetEventDone;
        }
        #endregion
    }
}
