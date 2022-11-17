using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

// Dummy
namespace DynMvp.Base
{
    public enum ColorSpace
    {
        RGB, HSI
    }

    public struct ColorValue
    {
        public ColorSpace ColorSpace { get; set; }
        public float Value1 { get; set; }
        public float Value2 { get; set; }
        public float Value3 { get; set; }

        public ColorValue(ColorSpace colorSpace)
        {
            ColorSpace = colorSpace;
            Value1 = 0;
            Value2 = 0;
            Value3 = 0;
        }

        public ColorValue(float value1, float value2, float value3, ColorSpace colorSpace = ColorSpace.RGB)
        {
            ColorSpace = colorSpace;
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
        }

        public ColorValue(Color color)
        {
            ColorSpace = ColorSpace.RGB;
            Value1 = color.R;
            Value2 = color.G;
            Value3 = color.B;
        }

        public ColorValue GetColor(ColorSpace colorSpace)
        {
            if (ColorSpace == ColorSpace.RGB && colorSpace == ColorSpace.HSI)
            {
                return RgbToHsi();
            }
            else if (ColorSpace == ColorSpace.HSI && colorSpace == ColorSpace.RGB)
            {
                return HsiToRgb();
            }

            return this;
        }

        public ColorValue HsiToRgb()
        {
            var rgbColor = new ColorValue(ColorSpace.RGB);

            float hue = Value1;
            float saturation = Value2;
            float luminance = Value3;

            float red, green, blue;
            if (saturation == 0)
            {
                red = green = blue = luminance * 255;
            }
            else
            {
                double t1, t2;
                double th = hue / 6.0d;

                if (luminance < 0.5d)
                {
                    t2 = luminance * (1d + saturation);
                }
                else
                {
                    t2 = (luminance + saturation) - (luminance * saturation);
                }
                t1 = 2d * luminance - t2;

                double tr, tg, tb;
                tr = th + (1.0d / 3.0d);
                tg = th;
                tb = th - (1.0d / 3.0d);

                tr = ColorCalc(tr, t1, t2);
                tg = ColorCalc(tg, t1, t2);
                tb = ColorCalc(tb, t1, t2);

                red = (float)(tr * 255f);
                green = (float)(tg * 255f);
                blue = (float)(tb * 255f);
            }

            rgbColor.Value1 = red;
            rgbColor.Value2 = green;
            rgbColor.Value3 = blue;

            return rgbColor;
        }

        private static double ColorCalc(double c, double t1, double t2)
        {
            if (c < 0)
            {
                c += 1d;
            }

            if (c > 1)
            {
                c -= 1d;
            }

            if (6.0d * c < 1.0d)
            {
                return t1 + (t2 - t1) * 6.0d * c;
            }

            if (2.0d * c < 1.0d)
            {
                return t2;
            }

            if (3.0d * c < 2.0d)
            {
                return t1 + (t2 - t1) * (2.0d / 3.0d - c) * 6.0d;
            }

            return t1;
        }

        public ColorValue RgbToHsi()
        {
            var hlsColor = new ColorValue(ColorSpace.HSI);

            float red = (Value1 / 255f);
            float green = (Value2 / 255f);
            float blue = (Value3 / 255f);

            float min = Math.Min(Math.Min(red, green), blue);
            float max = Math.Max(Math.Max(red, green), blue);
            float delta = max - min;

            float hue = 0;
            float saturation = 0;
            float luminance = (max + min) / 2.0f;

            if (delta != 0)
            {
                if (luminance < 0.5f)
                {
                    saturation = delta / (max + min);
                }
                else
                {
                    saturation = delta / (2.0f - max - min);
                }

                float deltaRed = ((max - red) / 6.0f + (delta / 2.0f)) / delta;
                float deltaGreen = ((max - green) / 6.0f + (delta / 2.0f)) / delta;
                float deltaBlue = ((max - blue) / 6.0f + (delta / 2.0f)) / delta;

                if (red == max)
                {
                    hue = deltaBlue - deltaGreen;
                }
                else if (green == max)
                {
                    hue = (1.0f / 3.0f) + deltaRed - deltaBlue;
                }
                else if (blue == max)
                {
                    hue = (2.0f / 3.0f) + deltaGreen - deltaRed;
                }

                if (hue < 0)
                {
                    hue += 1.0f;
                }

                if (hue > 1)
                {
                    hue -= 1.0f;
                }
            }

            hlsColor.Value1 = hue;
            hlsColor.Value2 = saturation;
            hlsColor.Value3 = luminance;

            return hlsColor;
        }

        public Color ToColor()
        {
            ColorValue rgbColor = GetColor(ColorSpace.RGB);
            return Color.FromArgb((int)rgbColor.Value1, (int)rgbColor.Value2, (int)rgbColor.Value3);
        }

        public void Load(XmlElement colorElement)
        {
            ColorSpace = (ColorSpace)Enum.Parse(typeof(ColorSpace), XmlHelper.GetValue(colorElement, "ColorSpace", "RGB"));
            Value1 = Convert.ToSingle(XmlHelper.GetValue(colorElement, "Value1", "0"));
            Value2 = Convert.ToSingle(XmlHelper.GetValue(colorElement, "Value2", "0"));
            Value3 = Convert.ToSingle(XmlHelper.GetValue(colorElement, "Value3", "0"));
        }

        public void Save(XmlElement colorElement)
        {
            XmlHelper.SetValue(colorElement, "ColorSpace", ColorSpace.ToString());
            XmlHelper.SetValue(colorElement, "Value1", Value1.ToString());
            XmlHelper.SetValue(colorElement, "Value2", Value2.ToString());
            XmlHelper.SetValue(colorElement, "Value3", Value3.ToString());
        }

        public void SetValue(int valueNo, float value)
        {
            switch (valueNo)
            {
                default:
                case 0: Value1 = value; break;
                case 1: Value2 = value; break;
                case 2: Value3 = value; break;
            }
        }
    }
}
