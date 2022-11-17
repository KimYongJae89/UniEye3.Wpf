using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DynMvp.Component.DepthSystem.DepthViewer
{
    public enum TextureColorType
    {
        None, Rainbow, Hsv1, Hsv2, Gray
    }

    public class TextureColor
    {
        private int textureHandle;
        private int levelCount;
        private TextureColorType textureColorType;
        private byte[] textureColor;
        private float lowRatio;
        private float highRatio;
        private const int minWaveLength = 380;
        private const int maxWaveLength = 780;

        public bool Create(TextureColorType textureColorType, int levelCount, float lowRatio, float highRatio, Color? startColorQ = null, Color? endColorQ = null)
        {
            GL.DeleteTextures(1, ref textureHandle);

            this.levelCount = levelCount;
            this.textureColorType = textureColorType;
            this.lowRatio = lowRatio;
            this.highRatio = highRatio;

            int lowIndex = (int)(levelCount * lowRatio);
            int highIndex = (int)(levelCount * highRatio);

            int i;
            int idx;

            Color startColor = startColorQ ?? Color.Black;
            Color endColor = endColorQ ?? Color.Black;

            textureColor = new byte[levelCount * 3];

            for (i = 0; i < lowIndex; i++)
            {
                idx = i * 3;

                textureColor[idx + 0] = startColor.R;
                textureColor[idx + 1] = startColor.G;
                textureColor[idx + 2] = startColor.B;
            }
            float fNumColor = (float)(highIndex - lowIndex)/* - 1.0f*/;

            Color color;

            switch (textureColorType)
            {
                case TextureColorType.None:
                    {
                        for (i = lowIndex; i < highIndex; i++)
                        {
                            color = Color.FromArgb(0);
                            idx = i * 3;
                            textureColor[idx + 0] = color.R;
                            textureColor[idx + 1] = color.G;
                            textureColor[idx + 2] = color.B;
                        }
                    }
                    break;
                case TextureColorType.Rainbow:
                    {
                        for (i = lowIndex; i < highIndex; i++)
                        {
                            idx = i * 3;
                            color = GetRainbowColor((i - lowIndex) / fNumColor);

                            textureColor[idx + 0] = color.R;
                            textureColor[idx + 1] = color.G;
                            textureColor[idx + 2] = color.B;
                        }
                    }
                    break;
                case TextureColorType.Hsv1:
                case TextureColorType.Hsv2:
                    {
                        float h1 = startColor.GetHue();
                        float s1 = startColor.GetSaturation();
                        float v1 = startColor.GetBrightness();
                        float h2 = endColor.GetHue();
                        float s2 = endColor.GetSaturation();
                        float v2 = endColor.GetSaturation();

                        float fDist_h = (h2 - h1);
                        float fDist_s = (s2 - s1);
                        float fDist_v = (v2 - v1);

                        for (i = lowIndex; i < highIndex; i++)
                        {
                            idx = i * 3;
                            float fRate = (i - lowIndex) / fNumColor;
                            h2 = h1 + fDist_h * fRate;
                            s2 = s1 + fDist_s * fRate;
                            v2 = v1 + fDist_v * fRate;

                            color = ColorFromHSV(h2, s2, v2);

                            textureColor[idx + 0] = color.R;
                            textureColor[idx + 1] = color.G;
                            textureColor[idx + 2] = color.B;
                        }
                    }
                    break;
                case TextureColorType.Gray:
                    {
                        float startGray = startColor.R;
                        float endGray = endColor.R;

                        float fDist = (endGray - startGray);

                        for (i = lowIndex; i < highIndex; i++)
                        {
                            idx = i * 3;
                            float fRate = (i - lowIndex) / fNumColor;
                            byte gray = (byte)(startGray + fDist * fRate);

                            textureColor[idx + 0] = gray;
                            textureColor[idx + 1] = gray;
                            textureColor[idx + 2] = gray;
                        }
                    }
                    break;
            }

            for (i = highIndex; i < levelCount; i++)
            {
                idx = i * 3;
                textureColor[idx + 0] = endColor.R;
                textureColor[idx + 1] = endColor.G;
                textureColor[idx + 2] = endColor.B;
            }

            GL.GenTextures(1, out textureHandle);
            GL.BindTexture(TextureTarget.Texture1D, textureHandle);
            GL.TexImage1D(TextureTarget.Texture1D, 0, PixelInternalFormat.Three, levelCount, 0, PixelFormat.Rgb, PixelType.UnsignedByte, textureColor);
            GL.TexParameter(TextureTarget.Texture1D, TextureParameterName.TextureBorderColor, new float[] { 1, 1, 1, 1 });
            GL.TexParameter(TextureTarget.Texture1D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture1D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture1D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            return true;
        }

        public void Bind()
        {
            GL.Enable(EnableCap.Texture1D);
            GL.BindTexture(TextureTarget.Texture1D, textureHandle);
        }

        private static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
            {
                return Color.FromArgb(255, v, t, p);
            }
            else if (hi == 1)
            {
                return Color.FromArgb(255, q, v, p);
            }
            else if (hi == 2)
            {
                return Color.FromArgb(255, p, v, t);
            }
            else if (hi == 3)
            {
                return Color.FromArgb(255, p, q, v);
            }
            else if (hi == 4)
            {
                return Color.FromArgb(255, t, p, v);
            }
            else
            {
                return Color.FromArgb(255, v, p, q);
            }
        }

        private Color GetRainbowColor(double dFraction)
        {
            if (dFraction < 0.0 || dFraction > 1.0)
            {
                return Color.FromArgb(0);
            }

            return (WaveLength2RGB(minWaveLength + dFraction * ((maxWaveLength) - minWaveLength)));
        }

        private Color WaveLength2RGB(double dWaveLen)
        {
            int nWaveLen = (int)dWaveLen;

            double dRed = 0.0;
            double dGreen = 0.0;
            double dBlue = 0.0;

            if (minWaveLength <= nWaveLen && nWaveLen <= 439)
            {
                dRed = -(dWaveLen - 440) / (440 - 380);
                dGreen = 0.0;
                dBlue = 1.0;
            }
            else if (440 <= nWaveLen && nWaveLen <= 489)
            {
                dRed = 0.0;
                dGreen = (dWaveLen - 440) / (490 - 440);
                dBlue = 1.0;
            }
            else if (490 <= nWaveLen && nWaveLen <= 509)
            {
                dRed = 0.0;
                dGreen = 1.0;
                dBlue = -(dWaveLen - 510) / (510 - 490);
            }
            else if (510 <= nWaveLen && nWaveLen <= 579)
            {
                dRed = (dWaveLen - 510) / (580 - 510);
                dGreen = 1.0;
                dBlue = 0.0;
            }
            else if (580 <= nWaveLen && nWaveLen <= 644)
            {
                dRed = 1.0;
                dGreen = -(dWaveLen - 645) / (645 - 580);
                dBlue = 0.0;
            }
            else if (645 <= nWaveLen && nWaveLen <= maxWaveLength)
            {
                dRed = 1.0;
                dGreen = 0.0;
                dBlue = 0.0;
            }

            double dFactor = 0.0;
            if (minWaveLength <= nWaveLen && nWaveLen <= 419)
            {
                dFactor = 0.3 + 0.7 * (dWaveLen - 380) / (420 - 380);
            }

            if (420 <= nWaveLen && nWaveLen <= 700)
            {
                dFactor = 1.0;
            }

            if (701 <= nWaveLen && maxWaveLength <= 780)
            {
                dFactor = 0.3 + 0.7 * (780 - dWaveLen) / (780 - 700);
            }

            double dGamma = 0.80;
            double dMaxIntensity = 255.0;
            byte r, g, b;

            if (dRed == 0.0)
            {
                r = 0;
            }
            else
            {
                r = (byte)(dMaxIntensity * Math.Pow(dRed * dFactor, dGamma));
            }

            if (dGreen == 0.0)
            {
                g = 0;
            }
            else
            {
                g = (byte)(dMaxIntensity * Math.Pow(dGreen * dFactor, dGamma));
            }

            if (dBlue == 0.0)
            {
                b = 0;
            }
            else
            {
                b = (byte)(dMaxIntensity * Math.Pow(dBlue * dFactor, dGamma));
            }

            return Color.FromArgb(r, g, b);
        }
    }
}
