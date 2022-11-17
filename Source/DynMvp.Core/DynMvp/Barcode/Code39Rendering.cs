using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace DynMvp.Barcode
{
    /// <summary>
    /// Summary description for Code128Rendering.
    /// </summary>
    public class Code39Renderer : BarcodeRenderer
    {
        private Dictionary<string, byte[]> chars = new Dictionary<string, byte[]>();
        private Color backColor = Color.FromArgb(255, 255, 255, 255);
        private Color foreColor = Color.FromArgb(255, 0, 0, 0);

        public Code39Renderer()
        {
            if (chars.Count == 0)
            {
                StringToCode39();
            }
        }


        public override Image GetBarcodeImage(string inputData)
        {
            //if (chars.Count == 0)
            //    StringToCode39();
            ////Size = new Size(422, 173);
            ////Bitmap bitmap = new Bitmap(Size.Width, Size.Height);


            //Graphics insGraphics = Graphics.FromImage(bitmap);
            //string text = inputData.ToUpper();
            //if (!text.StartsWith("*"))
            //{
            //    text = "*" + text;
            //}
            //if (!text.EndsWith("*"))
            //{
            //    text = text + "*";
            //}
            //insGraphics.FillRectangle(new SolidBrush(this.backColor), 0, 0, Size.Width, Size.Height);
            //int narrowCount = 0;
            //int wideCount = 0;
            //int blankCount = 0;

            //for (int i = 0; i < text.Length; i++)
            //{
            //    byte[] insByteArray = chars[text.Substring(i, 1)];
            //    foreach (byte insByte in insByteArray)
            //    {
            //        if (insByte == 1)
            //        {
            //            wideCount++;
            //        }
            //        else
            //        {
            //            narrowCount++;
            //        }
            //    }
            //    if (i + 1 != text.Length)
            //    {
            //        blankCount++;
            //    }
            //}
            //decimal widthPerUnit = Math.Round(Convert.ToDecimal(Size.Width) / Convert.ToDecimal(((wideCount * _WideNarrowRatio) + blankCount + narrowCount)), 2);
            //decimal currentLeft = 0;
            //for (int i = 0; i < text.Length; i++)
            //{
            //    byte[] insByteArray = chars[text.Substring(i, 1)];
            //    int index = 0;
            //    foreach (byte insByte in insByteArray)
            //    {
            //        if (index % 2 == 0)
            //        {
            //            insGraphics.FillRectangle(new SolidBrush(this.foreColor), (float)currentLeft, 0, (float)(insByte == 1 ? widthPerUnit * _WideNarrowRatio : widthPerUnit), Size.Height);

            //        }
            //        currentLeft += (insByte == 1 ? widthPerUnit * _WideNarrowRatio : widthPerUnit);

            //        index++;
            //    }
            //    currentLeft += widthPerUnit;
            //}


            //Image image = (Image)bitmap;
            ////image.Save(@"D:\39.bmp");
            return null;
        }

        private void StringToCode39()
        {
            chars.Add("1", new byte[] { 1, 0, 0, 1, 0, 0, 0, 0, 1 });
            chars.Add("2", new byte[] { 0, 0, 1, 1, 0, 0, 0, 0, 1 });
            chars.Add("3", new byte[] { 1, 0, 1, 1, 0, 0, 0, 0, 0 });
            chars.Add("4", new byte[] { 0, 0, 0, 1, 1, 0, 0, 0, 1 });
            chars.Add("5", new byte[] { 1, 0, 0, 1, 1, 0, 0, 0, 0 });
            chars.Add("6", new byte[] { 0, 0, 1, 1, 1, 0, 0, 0, 0 });
            chars.Add("7", new byte[] { 0, 0, 0, 1, 0, 0, 1, 0, 1 });
            chars.Add("8", new byte[] { 1, 0, 0, 1, 0, 0, 1, 0, 0 });
            chars.Add("9", new byte[] { 0, 0, 1, 1, 0, 0, 1, 0, 0 });
            chars.Add("0", new byte[] { 0, 0, 0, 1, 1, 0, 1, 0, 0 });
            chars.Add("A", new byte[] { 1, 0, 0, 0, 0, 1, 0, 0, 1 });
            chars.Add("B", new byte[] { 0, 0, 1, 0, 0, 1, 0, 0, 1 });
            chars.Add("C", new byte[] { 1, 0, 1, 0, 0, 1, 0, 0, 0 });
            chars.Add("D", new byte[] { 0, 0, 0, 0, 1, 1, 0, 0, 1 });
            chars.Add("E", new byte[] { 1, 0, 0, 0, 1, 1, 0, 0, 0 });
            chars.Add("F", new byte[] { 0, 0, 1, 0, 1, 1, 0, 0, 0 });
            chars.Add("G", new byte[] { 0, 0, 0, 0, 0, 1, 1, 0, 1 });
            chars.Add("H", new byte[] { 1, 0, 0, 0, 0, 1, 1, 0, 0 });
            chars.Add("I", new byte[] { 0, 0, 1, 0, 0, 1, 1, 0, 0 });
            chars.Add("J", new byte[] { 0, 0, 0, 0, 1, 1, 1, 0, 0 });
            chars.Add("K", new byte[] { 1, 0, 0, 0, 0, 0, 0, 1, 1 });
            chars.Add("L", new byte[] { 0, 0, 1, 0, 0, 0, 0, 1, 1 });
            chars.Add("M", new byte[] { 1, 0, 1, 0, 0, 0, 0, 1, 0 });
            chars.Add("N", new byte[] { 0, 0, 0, 0, 1, 0, 0, 1, 1 });
            chars.Add("O", new byte[] { 1, 0, 0, 0, 1, 0, 0, 1, 0 });
            chars.Add("P", new byte[] { 0, 0, 1, 0, 1, 0, 0, 1, 0 });
            chars.Add("Q", new byte[] { 0, 0, 0, 0, 0, 0, 1, 1, 1 });
            chars.Add("R", new byte[] { 1, 0, 0, 0, 0, 0, 1, 1, 0 });
            chars.Add("S", new byte[] { 0, 0, 1, 0, 0, 0, 1, 1, 0 });
            chars.Add("T", new byte[] { 0, 0, 0, 0, 1, 0, 1, 1, 0 });
            chars.Add("U", new byte[] { 1, 1, 0, 0, 0, 0, 0, 0, 1 });
            chars.Add("V", new byte[] { 0, 1, 1, 0, 0, 0, 0, 0, 1 });
            chars.Add("W", new byte[] { 1, 1, 1, 0, 0, 0, 0, 0, 0 });
            chars.Add("X", new byte[] { 0, 1, 0, 0, 1, 0, 0, 0, 1 });
            chars.Add("Y", new byte[] { 1, 1, 0, 0, 1, 0, 0, 0, 0 });
            chars.Add("Z", new byte[] { 0, 1, 1, 0, 1, 0, 0, 0, 0 });
            chars.Add("-", new byte[] { 0, 1, 0, 0, 0, 0, 1, 0, 1 });
            chars.Add(".", new byte[] { 1, 1, 0, 0, 0, 0, 1, 0, 0 });
            chars.Add(" ", new byte[] { 0, 1, 1, 0, 0, 0, 1, 0, 0 });
            chars.Add("*", new byte[] { 0, 1, 0, 0, 1, 0, 1, 0, 0 });
            chars.Add("$", new byte[] { 0, 1, 0, 1, 0, 1, 0, 0, 0 });
            chars.Add("/", new byte[] { 0, 1, 0, 1, 0, 0, 0, 1, 0 });
            chars.Add("+", new byte[] { 0, 1, 0, 0, 0, 1, 0, 1, 0 });
            chars.Add("%", new byte[] { 0, 0, 0, 1, 0, 1, 0, 1, 0 });
        }
    }
}
