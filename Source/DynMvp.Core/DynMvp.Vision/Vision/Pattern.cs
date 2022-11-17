using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace DynMvp.Vision
{
    public enum PatternType
    {
        Good, Ng
    }

    public abstract class Pattern
    {
        public PatternType PatternType { get; set; }

        protected FigureGroup maskFigures = new FigureGroup();
        public FigureGroup MaskFigures
        {
            get => maskFigures;
            set => maskFigures = value;
        }
        public bool UseEdgeFilter { get; set; }

        protected Image2D patternImage = null;
        public Image2D PatternImage
        {
            get => patternImage;
            set => patternImage = value;
        }
        public PointF Origin { get; set; }

        public virtual void Dispose()
        {
            if (patternImage != null)
            {
                patternImage.Dispose();
            }
        }

        public abstract Pattern Clone();

        public virtual void Copy(Pattern pattern)
        {
            PatternType = pattern.PatternType;
            maskFigures = (FigureGroup)pattern.maskFigures.Clone();

            Dispose();

            //Train(pattern.PatternImage, Patter);
        }

        public string GetPatternImageString()
        {
            if (patternImage == null)
            {
                return "";
            }

            var patternBitmap = patternImage.ToBitmap();
            string patternImageString = ImageHelper.BitmapToBase64String(patternBitmap);
            patternBitmap.Dispose();

            return patternImageString;
        }

        public void SetPatternImageString(string patternImageString)
        {
            Bitmap patternBitmap = ImageHelper.Base64StringToBitmap(patternImageString);
            if (patternBitmap != null)
            {
                Dispose();

                var patternImage = Image2D.ToImage2D(patternBitmap);
                patternImage.SaveImage("D:\\Pattern.bmp", ImageFormat.Bmp);
                //patternImage.ConvertFromDataPtr();

                //patternBitmap.Dispose();
                this.patternImage = patternImage;
                //Train(patternImage, null);
                //patternImage.Dispose();
            }
        }

        public abstract void Train(Image2D image, PatternMatchingParam patternMatchingParam);

        public virtual Image2D GetMaskedImage()
        {
            return patternImage;
        }

        public abstract void UpdateMaskImage();

        public abstract PatternResult Inspect(AlgoImage targetClipImage, PatternMatchingParam patternMatchingParam, DebugContext debugContext);

    }
}
