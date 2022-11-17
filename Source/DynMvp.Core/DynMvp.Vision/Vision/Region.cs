using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DynMvp.Vision
{
    public interface Region
    {
    }

    public class RectangleRegion : Region
    {
        private RectangleF rectangle;
        public float X
        {
            get => rectangle.X;
            set => rectangle.X = value;
        }
        public float Y
        {
            get => rectangle.Y;
            set => rectangle.Y = value;
        }
        public float Width
        {
            get => rectangle.Width;
            set => rectangle.Width = value;
        }
        public float Height
        {
            get => rectangle.Height;
            set => rectangle.Height = value;
        }
        public float Angle { get; set; }
    }
}
