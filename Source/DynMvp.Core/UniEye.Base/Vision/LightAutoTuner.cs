using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using System.Collections.Generic;
using System.Threading;

namespace UniEye.Base.Vision
{
    public class LightTuneData
    {
        public Dictionary<int, float> ValueMap { get; } = new Dictionary<int, float>();
        public int Count => ValueMap.Count;
        public KeyValuePair<int, float> LastTuneValue { get; set; }

        public LightTuneData()
        {
        }

        public void Clear()
        {
            ValueMap.Clear();
        }

        public void AddTuneValue(int lightValue, float std)
        {
            LastTuneValue = new KeyValuePair<int, float>(lightValue, std);
            ValueMap.Add(lightValue, std);
        }

        public KeyValuePair<int, float> GetMaxTuneValue()
        {
            var maxKeyValue = new KeyValuePair<int, float>(0, 0);

            foreach (KeyValuePair<int, float> keyValue in ValueMap)
            {
                if (keyValue.Value > maxKeyValue.Value)
                {
                    maxKeyValue = keyValue;
                }
            }

            return maxKeyValue;
        }
    }

    public delegate void OnTuningDelegate(ImageD[] tuningImageArray);
    public delegate void TuneDoneDelegate(ImageD[] tuneDoneImageArray);

    public class LightAutoTuner
    {
        private int lightStep = 5;
        private Thread tuneThread;
        public LightTuneData SumTuneData { get; } = new LightTuneData();
        public LightTuneData[] TuneDataArr { get; }
        public int CurLightValue { get; private set; }

        private LightTuneValueCalculator lightTuneValueCalculator;

        public OnTuningDelegate OnTuning;
        public TuneDoneDelegate TuneDone;
        private ImageD[] tuningImageArray;
        public LightAutoTuner(LightTuneValueCalculator lightTuneValueCalculator)
        {
            CameraHandler cameraHandler = DeviceManager.Instance().CameraHandler;
            tuningImageArray = new ImageD[cameraHandler.NumCamera];
            TuneDataArr = new LightTuneData[cameraHandler.NumCamera];

            this.lightTuneValueCalculator = lightTuneValueCalculator;
        }

        public float GetProgressPercent()
        {
            return CurLightValue / 255.0f * 100.0f;
        }

        public int GetLightTuneValue()
        {
            KeyValuePair<int, float> maxTuneValue = GetMaxTuneValue();

            int lightValue = maxTuneValue.Key;
            if (lightValue == 0)
            {
                lightValue = 255;
            }

            return lightValue;
        }

        public KeyValuePair<int, float> GetMaxTuneValue()
        {
            return SumTuneData.GetMaxTuneValue();
        }

        public void Start()
        {
            CurLightValue = 10;

            foreach (LightTuneData tuneData in TuneDataArr)
            {
                tuneData.Clear();
            }

            SumTuneData.Clear();

            for (int i = 0; i < tuningImageArray.Length; i++)
            {
                tuningImageArray[i] = null;
            }

            tuneThread = new Thread(TuneProcess);
            tuneThread.Start();
        }

        public void Stop()
        {
            if (tuneThread != null && tuneThread.IsAlive == true)
            {
                tuneThread.Abort();
            }
        }

        private void TuneProcess()
        {
            float maxStdValue = 0;

            LightCtrlHandler lightCtrlHandler = DeviceManager.Instance().LightCtrlHandler;
            CameraHandler cameraHandler = DeviceManager.Instance().CameraHandler;

            for (int lightValue = 0; lightValue < 255; lightValue += lightStep)
            {
                CurLightValue = lightValue;

                lightCtrlHandler.TurnOn(new LightValue(1, CurLightValue));
                cameraHandler.GrabOnce();
                cameraHandler.WaitGrabDone();

                float sumValue = 0;
                foreach (Camera camera in cameraHandler)
                {
                    ImageD image = camera.GetGrabbedImage();
                    float tuneValue = lightTuneValueCalculator.GetValue(image);
                    TuneDataArr[camera.Index].AddTuneValue(lightValue, tuneValue);

                    tuningImageArray[camera.Index] = image;

                    sumValue += tuneValue;
                }

                SumTuneData.AddTuneValue(lightValue, sumValue);

                OnTuning?.Invoke(tuningImageArray);

                if (maxStdValue < sumValue)
                {
                    maxStdValue = sumValue;
                }
                else if (maxStdValue > sumValue)
                {
                    break;
                }

                Thread.Sleep(100);
            }//while

            TuneDone?.Invoke(tuningImageArray);
        }
    }
}
