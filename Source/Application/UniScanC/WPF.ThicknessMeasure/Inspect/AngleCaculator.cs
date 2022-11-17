using DynMvp.Devices.Spectrometer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Config;

namespace WPF.ThicknessMeasure.Inspect
{
    public class AngleCaculator
    {
        #region 속성
        private Override.DeviceManager DeviceManager => Override.DeviceManager.Instance() as Override.DeviceManager;

        private List<double> CoeffList { get; set; }
        #endregion


        #region 생성자
        public AngleCaculator()
        {
            ReadAngleCoef();
        }
        #endregion


        #region 메서드
        public void CalculateAngle(Dictionary<string, Layer> layerList)
        {
            double[,] angleVotage = DeviceManager.GetAngleData();
            if (angleVotage.Length > 0)
            {
                double avg1 = 0.0;
                double avg2 = 0.0;
                for (int i = 0; i < angleVotage.Length / 2; i++)
                {
                    avg1 += angleVotage[0, i];
                    avg2 += angleVotage[1, i];
                }

                avg1 = avg1 / (angleVotage.Length / 2);
                avg2 = avg2 / (angleVotage.Length / 2);

                double lightPosition = Cal_LightPosition(avg1, avg2);
                double angle = GetAngle(lightPosition);

                foreach (Layer layer in layerList.Values)
                {
                    layer.Angle = angle;
                }
            }
        }

        public double Cal_LightPosition(double V1, double V2)
        {
            // S3932 PSD 센서에 들어오는 빛의 위치 계산
            // 데이터시트 공식 참조
            // V1 : Voltage 1
            // V2 : Voltage 2
            // L : 센서영역 길이 (12mm)
            // return : 빛의 위치
            double L = 12.0;

            return L * (V2 - V1) / (2.0 * (V1 + V2));
        }

        public double GetAngle(double LightPos)
        {
            double Angle = 0.0;
            for (int i = 0; i < CoeffList.Count; i++)
            {
                Angle += Math.Pow(LightPos, i) * CoeffList[i];
            }

            return Angle;
        }

        private void ReadAngleCoef()
        {
            CoeffList = new List<double>();
            string fileName = string.Format(@"{0}\..\Config\AngleCurveFittingData.txt", PathConfig.Instance().Model);
            using (var file = new System.IO.StreamReader(fileName))
            {
                string[] tokens = file.ReadLine().Split(',');
                foreach (string token in tokens)
                {
                    CoeffList.Add(Convert.ToDouble(token));
                }
            }
        }

        internal void AngleScanStartStop(bool start, bool isContinuous = true)
        {
            DeviceManager.AngleScanStartStop(start, isContinuous);
        }
        #endregion
    }
}
