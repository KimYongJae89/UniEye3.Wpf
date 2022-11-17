using DynMvp.Data;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniEye.Base.UI
{
    public class ShampooBarcodeFigurePropertyPool
    {
        public virtual FigureProperty GetFigureProperty(object figureObject)
        {
            var figureProperty = new FigureProperty();

            if (figureObject is VisionProbe visionProbe)
            {
                if (visionProbe.Name.Contains("앞면"))
                {
                    figureProperty.Pen = new Pen(Color.LightBlue, 2.0f);
                }
                else if (visionProbe.Name.Contains("뒷면"))
                {
                    figureProperty.Pen = new Pen(Color.LightPink, 2.0f);
                }
            }
            else
            {
                figureProperty.Pen = new Pen(Color.Yellow, 1.0f);
            }

            return figureProperty;
        }
    }
}
