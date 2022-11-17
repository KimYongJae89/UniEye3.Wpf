using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanC.Algorithm.Simple
{
    internal class VerticalEdgeFinder
    {
        public static int[] Find(float[] data)
        {
            float mean = data.Average();
            float max = data.Max();
            float min = data.Min();

            //min과 max의 차이가 50보다 적다면 영역 내에 엣지가 없음으로 판단.
            if (max - min < 50)
            {
                return new int[0];
            }

            float threshold = (mean + max) / 2;

            int src = -1;
            var list = new List<int>();
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] > threshold)
                {
                    if (src < 0)
                    {
                        src = i;
                    }
                }
                else if (src >= 0)
                {
                    if (i - src > 5)
                    {
                        list.Add((src + i) / 2);
                    }

                    src = -1;
                }
            }

            if (src >= 0 && src < data.Length - 5)
            {
                list.Add((src + data.Length) / 2);
            }

            return list.ToArray();
        }
    }
}
