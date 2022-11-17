using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Devices.Light
{
    public class LightCtrlHandler
    {
        protected List<LightCtrl> lightCtrlList = new List<LightCtrl>();

        public IEnumerator<LightCtrl> GetEnumerator()
        {
            return lightCtrlList.GetEnumerator();
        }

        public int Count => lightCtrlList.Count;

        public int NumLightCtrl => lightCtrlList.Count;

        public int NumLight
        {
            get
            {
                int sumLight = 0;
                foreach (LightCtrl lightCtrl in lightCtrlList)
                {
                    sumLight += lightCtrl.NumChannel;
                }

                return sumLight;
            }
        }

        private LightParam lastLightParam = null;

        public void AddLightCtrl(LightCtrl lightCtrl)
        {
            if (lightCtrlList.Count > 0)
            {
                lightCtrl.StartChannelIndex = lightCtrlList.Last().StartChannelIndex + lightCtrlList.Last().NumChannel;
            }

            lightCtrlList.Add(lightCtrl);
        }

        public LightCtrl GetLightCtrl(uint lightCtrlIndex)
        {
            if (lightCtrlList.Count <= lightCtrlIndex)
            {
                return null;
            }

            return lightCtrlList[(int)lightCtrlIndex];
        }

        public LightCtrl GetLightCtrlByIndex(int lightCtrlIndex)
        {
            foreach (LightCtrl lightCtrl in lightCtrlList)
            {
                if (lightCtrl.StartChannelIndex <= lightCtrlIndex && lightCtrl.EndChannelIndex > lightCtrlIndex)
                {
                    return lightCtrl;
                }
            }

            return null;
        }

        public void Release()
        {
            foreach (LightCtrl lightCtrl in lightCtrlList)
            {
                lightCtrl.Release();
            }
        }

        public void TurnOn()
        {
            foreach (LightCtrl lightCtrl in lightCtrlList)
            {
                lightCtrl.TurnOn();
            }
        }

        public void TurnOn(LightValue lightValue)
        {
            foreach (LightCtrl lightCtrl in lightCtrlList)
            {
                lightCtrl.TurnOn(lightValue);
            }
        }

        public void TurnOn(LightParam lightParam)
        {
            if (lastLightParam != null && lastLightParam.IsSame(lightParam))
            {
                return;
            }

            lastLightParam = (LightParam)lightParam.Clone();

            foreach (LightCtrl lightCtrl in lightCtrlList)
            {
                lightCtrl.TurnOn(lightParam.LightValue);
            }
        }

        public void TurnOn(LightParam lightParam, int deviceIndex)
        {
            foreach (LightCtrl lightCtrl in lightCtrlList)
            {
                lightCtrl.TurnOn(lightParam.LightValue, deviceIndex);
            }
        }

        public void TurnOff()
        {
            lastLightParam = null;

            foreach (LightCtrl lightCtrl in lightCtrlList)
            {
                lightCtrl.TurnOff();
            }
        }

        public void TurnOff(int deviceIndex)
        {
            foreach (LightCtrl lightCtrl in lightCtrlList)
            {
                lightCtrl.TurnOff(deviceIndex);
            }
        }
    }
}
