using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Vision.Planbss
{
    internal class VisionDef
    {
    }

    public enum YesNo
    {
        Yes, No
    }

    public enum EUSER
    {
        OPERATOR, ADMIN, DEVELOPER
    }

    public enum SearchSide
    {
        Left, Top, Right, Bottom
    }

    public enum CardinalPoint
    {
        East, West, South, North, NorthEast, NorthWest, SouthEast, SouthWest
    }

    public enum ConvexShape
    {
        None, Left, Top, Right, Bottom
    }

    public enum EIMAGEMONO
    {
        MONO_NONE, MONO_GRAY, MONO_CH1, MONO_CH2, MONO_CH3, MONO_BAYER
    }

    public enum EIMAGECOLOR
    {
        COLOR_NONE, COLOR_RGB, COLOR_HLS, COLOR_HSV, COLOR_YCrCb, COLOR_Lab,
        COLOR_Luv, COLOR_XYZ, COLOR_YUV, COLOR_HSLFULL, COLOR_HSVFULL
    }

    public enum EIMAGEUNION
    {
        UNION_NONE, UNION_MIN, UNION_MAX, UNION_PLUS, UNION_MINUS, UNION_MINEWSN,
        UNION_DIFFERENCE, UNION_MINUSMIN, UNION_MINUSMAX,
        UNION_BITWISEAND, UNION_BITWISEOR, UNION_BITWISEXOR, UNION_BITWISENOT
    }

    public struct PointD
    {
        public double X { get; set; }
        public double Y { get; set; }

        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }
        public void Set(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    public class IF_IMAGE
    {
        public int nIndex;			///< Frame Index
        public EIMAGECOLOR eColor;			///< Frame Channel
        public EIMAGEMONO eMono;			///< Image Union Operation Type	
    }

    public class IF_FRAME
    {
        public IF_IMAGE Source;
        public IF_IMAGE Target;
        public EIMAGEUNION eUnion;

        public void Set(int _nIndexSrc, EIMAGECOLOR _eColorSrc, EIMAGEMONO _eMonoSrc, EIMAGEUNION _eUnion = EIMAGEUNION.UNION_NONE,
                        int _nIndexTgt = 0, EIMAGECOLOR _eColorTgt = EIMAGECOLOR.COLOR_RGB, EIMAGEMONO _eMonoTgt = EIMAGEMONO.MONO_GRAY)
        {
            Source.nIndex = _nIndexSrc;
            Source.eColor = _eColorSrc;
            Source.eMono = _eMonoSrc;
            eUnion = _eUnion;
            Target.nIndex = _nIndexTgt;
            Target.eColor = _eColorTgt;
            Target.eMono = _eMonoTgt;
        }
    }
}
