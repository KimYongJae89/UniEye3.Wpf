using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Devices.MotionController
{
    public class AxisPosition
    {
        public string Name { get; set; }
        public string AxisHandlerName { get; set; }
        public float[] Position { get; set; } = new float[3];

        public AxisPosition()
        {
        }

        public AxisPosition Clone()
        {
            var position = new AxisPosition(GetPosition());
            position.Name = Name;

            return position;
        }

        public AxisPosition(int numAxis)
        {
            Position = new float[numAxis];
        }

        public AxisPosition(params float[] positions)
        {
            SetPosition(positions);
        }

        public AxisPosition(string axisHandlerName, params float[] positions)
        {
            AxisHandlerName = axisHandlerName;
            SetPosition(positions);
        }

        public int NumAxis => Position.Count();

        public float this[int key]
        {
            get => Position[key];
            set => Position[key] = value;
        }

        public bool IsEmpty => Position == null;

        public PointF ToPointF()
        {
            return new PointF(Position[0], Position[1]);
        }

        public void SetPosition(params float[] positions)
        {
            var posList = new List<float>();

            foreach (float pos in positions)
            {
                posList.Add(pos);
            }

            Position = posList.ToArray();
        }

        public float[] GetPosition()
        {
            return Position;
        }

        public void ResetPosition()
        {
            for (int i = 0; i < Position.Count(); i++)
            {
                Position[i] = 0;
            }
        }

        public void GetValue(XmlElement xmlElement, string keyName)
        {
            string posStr = XmlHelper.GetValue(xmlElement, keyName, "");
            if (posStr != "")
            {
                if (posStr.IndexOf(':') > -1)
                {
                    string[] strArray = posStr.Split(':');
                    Name = strArray[0].Trim();
                    AxisHandlerName = strArray[1].Trim();
                    posStr = strArray[2];
                }

                string[] posStrArray = posStr.Split(',');
                Position = new float[posStrArray.Count()];

                for (int i = 0; i < posStrArray.Count(); i++)
                {
                    if (posStrArray.Length - 1 >= i)
                    {
                        Position[i] = Convert.ToSingle(posStrArray[i]);
                    }
                    else
                    {
                        Position[i] = 0;
                    }
                }
            }
        }

        public void SetValue(XmlElement xmlElement, string keyName)
        {
            XmlHelper.SetValue(xmlElement, keyName, ToString());
        }

        public static AxisPosition ToAxisPosition(params float[] positions)
        {
            var axisPosition = new AxisPosition();
            axisPosition.SetPosition(positions);

            return axisPosition;
        }

        public override string ToString()
        {
            string posStr = Name + " : " + AxisHandlerName + " : ";
            for (int i = 0; i < NumAxis; i++)
            {
                posStr += Position[i].ToString() + ",";
            }

            posStr = posStr.TrimEnd(',');

            return posStr;
        }
    }
}
