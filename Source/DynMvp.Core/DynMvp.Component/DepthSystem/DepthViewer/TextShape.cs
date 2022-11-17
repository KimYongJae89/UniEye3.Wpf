using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace DynMvp.Component.DepthSystem.DepthViewer
{
    internal enum AlignFormat
    {
        Left, Right, Center
    }

    public class FontImageInfo
    {
        public FontImageInfo()
        {
            storedX = storedY = width = height = 0;
        }

        /** Character's width */
        public int width;

        /** Character's height */
        public int height;

        /** Character's stored x position */
        public int storedX;

        /** Character's stored y position */
        public int storedY;
    }

    internal class FontStorage
    {
        private static FontStorage instance;
        public static FontStorage Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FontStorage();
                }

                return instance;
            }
        }
        public Font Font { get; set; }
        public bool AntiAlias { get; set; }
        public int FontSize { get; set; } = 0;
        public int FontHeight { get; set; } = 0;
        public int TextureID { get; private set; }
        public int TextureWidth { get; private set; } = 512;
        public int TextureHeight { get; } = 512;

        private FontImageInfo[] fontImageInfoArray = new FontImageInfo[256];
        private Dictionary<char, FontImageInfo> customFontImageInfoArray = new Dictionary<char, FontImageInfo>();

        private FontStorage()
        {

        }

        public void Initialize(Font font, bool antiAlias, char[] additionalChars)
        {
            Font = font;
            FontSize = (int)font.Size + 3;
            AntiAlias = antiAlias;

            CreateSet(additionalChars);

            FontHeight -= 1;
            if (FontHeight <= 0)
            {
                FontHeight = 1;
            }
        }

        private Bitmap GetFontImage(char ch)
        {
            // Create a temporary image to extract the character's size
            var tempfontImage = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var g = Graphics.FromImage(tempfontImage);
            if (AntiAlias == true)
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
            }
            SizeF dims = g.MeasureString(new string(new char[] { ch }), Font);

            if (ch == ' ')
            {
                dims = g.MeasureString("l", Font);
                //SizeF dims2 = g.MeasureString("aa", font);
                //dims.Width -= dims2.Width;
            }

            int charwidth = (int)dims.Width + 2;

            if (charwidth <= 0)
            {
                charwidth = 7;
            }
            int charheight = (int)dims.Height + 3;
            if (charheight <= 0)
            {
                charheight = FontSize;
            }

            // Create another image holding the character we are creating
            var fontImage = new Bitmap(charwidth, charheight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var gt = Graphics.FromImage(fontImage);
            if (AntiAlias == true)
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
            }

            int charx = 3;
            int chary = 1;
            gt.DrawString(new string(new char[] { ch }), Font, new SolidBrush(Color.White), new PointF(charx, chary));

            return fontImage;
        }

        private void CreateSet(char[] customCharsArray)
        {
            // If there are custom chars then I expand the font texture twice
            if (customCharsArray != null && customCharsArray.Length > 0)
            {
                TextureWidth *= 2;
            }

            // In any case this should be done in other way. Texture with size 512x512
            // can maintain only 256 characters with resolution of 32x32. The texture
            // size should be calculated dynamicaly by looking at character sizes.

            try
            {

                var imgTemp = new Bitmap(TextureWidth, TextureHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                var g = Graphics.FromImage(imgTemp);

                //g.FillRectangle(new SolidBrush(Color.Yellow), 0, 0, textureWidth, textureHeight);

                int rowHeight = 0;
                int positionX = 0;
                int positionY = 0;

                int customCharsLength = (customCharsArray != null) ? customCharsArray.Length : 0;

                for (int i = 0; i < 256 + customCharsLength; i++)
                {

                    // get 0-255 characters and then custom characters
                    char ch = (i < 256) ? (char)i : customCharsArray[i - 256];

                    Bitmap fontImage = GetFontImage(ch);

                    var fontImageInfo = new FontImageInfo();

                    fontImageInfo.width = fontImage.Width;
                    fontImageInfo.height = fontImage.Height;

                    if (positionX + fontImageInfo.width >= TextureWidth)
                    {
                        positionX = 0;
                        positionY += rowHeight;
                        rowHeight = 0;
                    }

                    fontImageInfo.storedX = positionX;
                    fontImageInfo.storedY = positionY;

                    if (fontImageInfo.height > FontHeight)
                    {
                        FontHeight = fontImageInfo.height;
                    }

                    if (fontImageInfo.height > rowHeight)
                    {
                        rowHeight = fontImageInfo.height;
                    }

                    // Draw it here
                    g.DrawImage(fontImage, positionX, positionY);

                    positionX += fontImageInfo.width;

                    if (i < 256)
                    { // standard characters
                        fontImageInfoArray[i] = fontImageInfo;
                    }
                    else
                    { // custom characters
                        customFontImageInfoArray[ch] = fontImageInfo;
                    }

                    fontImage = null;
                }

                TextureID = LoadImage(imgTemp);

            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to create font.");
                Console.WriteLine(e.StackTrace);
            }
        }

        public static int LoadImage(Bitmap bufferedImage)
        {
            try
            {
                short width = (short)bufferedImage.Width;
                short height = (short)bufferedImage.Height;
                //textureLoader.bpp = bufferedImage.getColorModel().hasAlpha() ? (byte)32 : (byte)24;
                int bpp = bufferedImage.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppArgb ? 32 : 24;

                BitmapData bData = bufferedImage.LockBits(new Rectangle(new Point(), bufferedImage.Size),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                int byteCount = bData.Stride * bufferedImage.Height;
                byte[] byteI = new byte[byteCount];
                Marshal.Copy(bData.Scan0, byteI, 0, byteCount);
                bufferedImage.UnlockBits(bData);

                int result = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, result);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);

                GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (int)TextureEnvMode.Modulate);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.GenerateMipmap, 1);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte, byteI);
                return result;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Environment.Exit(-1);
            }

            return -1;
        }

        public FontImageInfo GetFontImageInfo(char ch)
        {
            FontImageInfo fontImageInfo = null;
            if (ch < 256)
            {
                fontImageInfo = fontImageInfoArray[ch];
            }
            else
            {
                fontImageInfo = customFontImageInfoArray[ch];
            }

            return fontImageInfo;
        }

        public int GetWidth(string text)
        {
            int totalwidth = 0;

            for (int i = 0; i < text.Length; i++)
            {
                char currentChar = text[i];
                FontImageInfo fontImageInfo = GetFontImageInfo(currentChar);

                if (fontImageInfo != null)
                {
                    totalwidth += fontImageInfo.width;
                }
            }
            return totalwidth;
        }

    }

    internal class TextShape : Shape
    {
        private int correctL = 9, correctR = 8;
        private float x;
        private float y;
        private string text;
        private float scaleX;
        private float scaleY;
        private AlignFormat alignFormat;
        private Vector3 color;

        public TextShape()
        {

        }

        public TextShape(string text, float x, float y, float scaleX, float scaleY, AlignFormat alignFormat = AlignFormat.Left)
        {
            Initialize(text, x, y, scaleX, scaleY, alignFormat);
        }

        public void Initialize(string text, float x, float y, float scaleX, float scaleY, AlignFormat alignFormat = AlignFormat.Left)
        {
            this.x = x;
            this.y = y;
            this.text = text;
            this.scaleX = scaleX;
            this.scaleY = scaleY;
            this.alignFormat = alignFormat;
            color = new Vector3(1.0f, 1.0f, 1.0f);
        }

        public void SetColor(Color color)
        {
            this.color = new Vector3(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f);
        }

        public void setCorrection(bool on)
        {
            if (on)
            {
                correctL = 2;
                correctR = 1;
            }
            else
            {
                correctL = 0;
                correctR = 0;
            }
        }

        private void DrawQuad(float drawX, float drawY, float drawX2, float drawY2,
                float srcX, float srcY, float srcX2, float srcY2)
        {
            float DrawWidth = drawX2 - drawX;
            float DrawHeight = drawY2 - drawY;
            float TextureSrcX = srcX / FontStorage.Instance.TextureWidth;
            float TextureSrcY = srcY / FontStorage.Instance.TextureHeight;
            float SrcWidth = srcX2 - srcX;
            float SrcHeight = srcY2 - srcY;
            float RenderWidth = (SrcWidth / FontStorage.Instance.TextureWidth);
            float RenderHeight = (SrcHeight / FontStorage.Instance.TextureHeight);

            GL.Color3(color);
            GL.TexCoord2(TextureSrcX, TextureSrcY);
            GL.Vertex2(drawX, drawY);
            GL.TexCoord2(TextureSrcX, TextureSrcY + RenderHeight);
            GL.Vertex2(drawX, drawY + DrawHeight);
            GL.TexCoord2(TextureSrcX + RenderWidth, TextureSrcY + RenderHeight);
            GL.Vertex2(drawX + DrawWidth, drawY + DrawHeight);
            GL.TexCoord2(TextureSrcX + RenderWidth, TextureSrcY);
            GL.Vertex2(drawX + DrawWidth, drawY);
        }

        public override void Draw()
        {
            char charCurrent;

            int totalwidth = 0;
            int i = 0, d = 1, c = correctL;
            float startY = 0;
            int endIndex = text.Length - 1;

            switch (alignFormat)
            {
                case AlignFormat.Right:
                    d = -1;
                    c = correctR;

                    while (i < endIndex)
                    {
                        if (text[i] == '\n')
                        {
                            startY += FontStorage.Instance.FontHeight;
                        }

                        i++;
                    }
                    break;
                case AlignFormat.Center:
                    for (int l = 0; l <= endIndex; l++)
                    {
                        charCurrent = text[l];
                        if (charCurrent == '\n')
                        {
                            break;
                        }

                        FontImageInfo fontImageInfo = FontStorage.Instance.GetFontImageInfo(charCurrent);
                        totalwidth += fontImageInfo.width - correctL;
                    }
                    totalwidth /= -2;
                    break;
                case AlignFormat.Left:
                default:
                    d = 1;
                    c = correctL;
                    break;
            }

            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, FontStorage.Instance.TextureID);
#pragma warning disable CS0618 // Type or member is obsolete
            GL.Begin(BeginMode.Quads);
#pragma warning restore CS0618 // Type or member is obsolete

            while (i >= 0 && i <= endIndex)
            {
                charCurrent = text[i];

                FontImageInfo fontImageInfo = FontStorage.Instance.GetFontImageInfo(charCurrent);
                if (fontImageInfo != null)
                {
                    if (d < 0)
                    {
                        totalwidth += (fontImageInfo.width - c) * d;
                    }

                    if (charCurrent == '\n')
                    {
                        startY += FontStorage.Instance.FontHeight * d;
                        totalwidth = 0;
                        if (alignFormat == AlignFormat.Center)
                        {
                            for (int l = i + 1; l <= endIndex; l++)
                            {
                                charCurrent = text[l];
                                if (charCurrent == '\n')
                                {
                                    break;
                                }

                                fontImageInfo = FontStorage.Instance.GetFontImageInfo(charCurrent);
                                totalwidth += fontImageInfo.width - correctL;
                            }
                            totalwidth /= -2;
                        }
                    }
                    else
                    {
                        DrawQuad((totalwidth + fontImageInfo.width) * scaleX + x, startY * scaleY + y,
                            totalwidth * scaleX + x,
                            (startY + fontImageInfo.height) * scaleY + y, fontImageInfo.storedX + fontImageInfo.width,
                            fontImageInfo.storedY, fontImageInfo.storedX,
                            fontImageInfo.storedY + fontImageInfo.height);

                        if (d > 0)
                        {
                            totalwidth += (fontImageInfo.width - c) * d;
                        }
                    }
                    i += d;

                }
            }
            GL.End();
            GL.Disable(EnableCap.Texture2D);
        }

        //public static bool IsSupported(string fontname)
        //{
        //    FontFamily[] font = GetFonts();
        //    for (int i = font.Length - 1; i >= 0; i--)
        //    {
        //        if (string.Equals(font[i].Name, fontname, StringComparison.CurrentCultureIgnoreCase))
        //            return true;
        //    }
        //    return false;
        //}

        //public static FontFamily[] GetFonts()
        //{
        //    return (new InstalledFontCollection()).Families;
        //}

        //public static byte[] intToByteArray(int value)
        //{
        //    return new byte[] {
        //            (byte)(value >> 24),
        //            (byte)(value >> 16),
        //            (byte)(value >> 8),
        //            (byte)value};
        //}
    }
}
