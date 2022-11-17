using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unieye.WPF.Base.Models
{
    public class Ransac
    {
        public bool FindInSamples(Point[] samples, int sampleCnt, Point data)
        {
            for (int index = 0; index < sampleCnt; index++)
            {
                if (samples[index].X == data.X && samples[index].Y == data.Y)
                {
                    return true;
                }
            }

            return false;
        }

        public void GetSamples(Point[] samples, int sampleCnt, Point[] srcData, int dataCnt)
        {
            for (int index = 0; index < sampleCnt; index++)
            {
                var r = new Random();
                int choosedIndex = r.Next(0, dataCnt);

                if (FindInSamples(samples, index, srcData[choosedIndex]) == false)
                {
                    samples[index] = srcData[choosedIndex];
                }
                else
                {
                    index--;
                }
            }
        }

        //public double RansacEllipseFitting(Point[] data, int dataCnt, EllipseModel ellipseModel, double distanceThreshold, int cntSamples)
        //{
        //    if (dataCnt < cntSamples)
        //        return 0;

        //    double maxCost = 0;
        //    int outLiers = 0;
        //    List<Point> inLiers = new List<Point>();
        //    inLiers.Clear();

        //    EllipseModel estimatedModel = new EllipseModel();

        //    int maxiter = (int)(1 + Math.Log(1 - 0.999) / Math.Log(1 - Math.Pow(0.8, cntSamples)));

        //    for (int index = 0; index < maxiter; index++)
        //    {
        //        Point[] samplePts = new Point[cntSamples];
        //        GetSamples(samplePts, cntSamples, data, data.Length);

        //        estimatedModel.ComputeModelParameter(samplePts, cntSamples);

        //        if (estimatedModel.convertStdForm() == false)
        //        {
        //            index--;
        //            continue;
        //        }

        //        double curCost = estimatedModel.ModelVerification(data, dataCnt, distanceThreshold, inLiers);

        //        if (maxCost < curCost)
        //        {
        //            maxCost = curCost;
        //            ellipseModel.ComputeModelParameter(inLiers.ToArray(), inLiers.ToArray().Length);
        //            ellipseModel.convertStdForm();

        //            if (maxCost / dataCnt > 0.79)
        //                index = maxiter;
        //        }
        //    }

        //    return maxCost;
        //}

        public double RansacLineFitting(Point[] data, LineModel lineModel, double distance_threshold)
        {
            const int no_samples = 2;
            if (data.Length < no_samples)
            {
                return 0.0;
            }

            lineModel.compute_model_parameter(data);

            var vInliersPoint = new List<Point>();
            double max_cost = lineModel.model_verification(data, distance_threshold, vInliersPoint);

            var samplePoint = new Point[no_samples];
            var estimated_model = new LineModel();

            int max_iteration = (int)(1 + Math.Log(1 - 0.9999) / Math.Log(1 - Math.Pow(max_cost / data.Length, no_samples)));

            for (int i = 0; i < max_iteration; i++)
            {
                vInliersPoint.Clear();
                // 1. hypothesis // 원본 데이터에서 임의로 N개의 셈플 데이터를 고른다.
                GetSamples(samplePoint, no_samples, data, data.Length);

                // 이 데이터를 정상적인 데이터로 보고 모델 파라메터를 예측한다.
                estimated_model.compute_model_parameter(samplePoint);

                // 2. Verification // 원본 데이터가 예측된 모델에 잘 맞는지 검사한다.
                double cost = estimated_model.model_verification(data, distance_threshold, vInliersPoint);
                // 만일 예측된 모델이 잘 맞는다면, 이 모델에 대한 유효한 데이터로 새로운 모델을 구한다.
                if (max_cost < cost)
                {
                    max_cost = cost;
                    lineModel.compute_model_parameter(vInliersPoint.ToArray());
                }
            }
            return max_cost;
        }
    }
}
