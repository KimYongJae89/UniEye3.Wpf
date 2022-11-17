using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Gerber
{
    public class Instruction
    {
        private int iValue;
        private double dValue;

        public void SetValue(double value) { dValue = value; }
        public void SetValue(int value) { iValue = value; }
        public double GetDouble() { return dValue; }
        public int GetInt() { return iValue; }
        public OpCode OpCode { get; set; }
    }

    public class SimplifiedApertureMacro
    {
        public ApertureType Type { get; set; }
        public double[] Parameter { get; } = new double[Constant.APERTURE_PARAMETERS_MAX];

        public SimplifiedApertureMacro()
        {
        }

        public void CopyParameter(double[] paramArr)
        {
            paramArr.CopyTo(Parameter, 0);
        }
    }

    public class StepNRepeat
    {
        public int X { get; set; }
        public int Y { get; set; }
        public double DistX { get; set; }
        public double DistY { get; set; }

        public StepNRepeat()
        {
            Reset();
        }

        public void Reset()
        {
            X = Y = 1;
            DistX = DistY = 0.0;
        }
    }

    public class Knockout
    {
        public KnockOutType Type { get; set; }
        public Polarity Polarity { get; set; }
        public double LowerLeftX { get; set; }
        public double LowerLeftY { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Border { get; set; }

        public void Clear()
        {
            Type = KnockOutType.NoKnockOut;
            Polarity = Polarity.Clear;
            LowerLeftX = 0;
            LowerLeftY = 0;
            Width = 0;
            Height = 0;
            Border = 0;
        }
    }

    public class Layer
    {
        public string Name { get; set; }
        public StepNRepeat StepNRepeat { get; set; }
        public Knockout Knockout { get; set; }
        public double Rotation { get; set; }
        public Polarity Polarity { get; set; }

        public Layer()
        {
            StepNRepeat = new StepNRepeat();
        }

        public Layer Clone()
        {
            var layer = new Layer();
            layer.Name = Name;
            layer.StepNRepeat = StepNRepeat;
            layer.Knockout = Knockout;
            layer.Rotation = Rotation;
            layer.Polarity = Polarity;

            return layer;
        }
    }

    public class NetEntryState // NetState
    {
        public AxisSelect AxisSelect { get; set; }
        public MirrorState MirrorState { get; set; }
        public Unit Unit { get; set; }
        public double OffsetA { get; set; }
        public double OffsetB { get; set; }
        public double ScaleA { get; set; }
        public double ScaleB { get; set; }

        public NetEntryState()
        {
            OffsetA = OffsetB = 0;
            ScaleA = ScaleB = 1;
        }
    }

    public class NetEntry // gerbv_net
    {
        public double StartX { get; set; }
        public double StartY { get; set; }
        public double StopX { get; set; }
        public double StopY { get; set; }
        public RectangleF BoundingBox { get; set; }
        public int ApertureNo { get; set; }
        public ApertureState ApertureState { get; set; }
        public Interpolation Interpolation { get; set; }
        public CircleSegment CircleSegment { get; set; }
        public string Label { get; set; }
        public Layer Layer { get; set; }
        public NetEntryState State { get; set; }

        public NetEntry()
        {
            CircleSegment = new CircleSegment();
        }

        internal void Clear()
        {
            CircleSegment.Clear();
        }
    }

    public class GerberFormat
    {
        public OmitZeros OmitZeros { get; set; }
        public Coordinate Coordinate { get; set; }
        public int XNumInteger { get; set; }
        public int XNumDecimal { get; set; }
        public int YNumInteger { get; set; }
        public int YNumDecimal { get; set; }
        public int LimitSeqNo { get; set; }
        public int LimitGeneralFunc { get; set; }
        public int LimitPlotFunc { get; set; }
        public int LimitMiscFunc { get; set; }

        public void Clear()
        {
            OmitZeros = OmitZeros.Unspecified;
            Coordinate = Coordinate.Absolute;
            XNumInteger = 0;
            XNumDecimal = 0;
            YNumInteger = 0;
            YNumDecimal = 0;
            LimitSeqNo = 0;
            LimitGeneralFunc = 0;
            LimitPlotFunc = 0;
            LimitMiscFunc = 0;
        }
    }

    public class HID_AttributeValue
    {
        //int int_value;
        //string str_value;
        //double real_value;
    }

    public class HID_Attribute
    {
        //string name;
        //string help_text;
        //HIDType type;
        //int min_val;
        //int max_val;           /* for integer and real */
        //HID_AttributeValue default_val; /* Also actual value for global attributes.  */
        //string[] enumerations;
        ///* If set, this is used for global attributes (i.e. those set
        //   statically with REGISTER_ATTRIBUTES below) instead of changing
        //   the default_val.  Note that a HID_Mixed attribute must specify a
        //   pointer to gerbv_HID_Attr_Val here, and HID_Boolean assumes this is
        //   "char *" so the value should be initialized to zero, and may be
        //   set to non-zero (not always one).  */
        //object value;
        //int hash; /* for detecting changes. */
    }

    public class GerberInfo
    {
        public string Name { get; set; }
        public Polarity Polarity { get; set; }
        public double MinX { get; set; }
        public double MinY { get; set; }
        public double MaxX { get; set; }
        public double MaxY { get; set; }
        public double OffsetA { get; set; }
        public double OffsetB { get; set; }
        public Encoding Encoding { get; set; }
        public double ImageRotation { get; set; }
        public ImageJustify ImageJustifyTypeA { get; set; }
        public ImageJustify ImageJustifyTypeB { get; set; }
        public double ImageJustifyOffsetA { get; set; }
        public double ImageJustifyOffsetB { get; set; }
        public double ImageJustifyOffsetActualA { get; set; }
        public double ImageJustifyOffsetActualB { get; set; }
        public string PlotterFilm { get; set; }
        public string TypeStr { get; set; }
        public List<HID_Attribute> AttributeList { get; }

        public GerberInfo()
        {
            AttributeList = new List<HID_Attribute>();
        }

        public void Clear()
        {
            Name = "";
            TypeStr = "";
            AttributeList.Clear();
        }
    }

    /*! errors found in the files */
    public class ErrorItem
    {
        public int LayerIndex { get; set; } = -1;
        public MessageType Type { get; set; }
        public string Message { get; set; }
    }

    public class UserTransform
    {
        public float TranslateX { get; set; }
        public float TranslateY { get; set; }
        public float ScaleX { get; set; }
        public float ScaleY { get; set; }
        public float Rotation { get; set; }
        public bool MirrorAroundX { get; set; }
        public bool MirrorAroundY { get; set; }
        public bool Inverted { get; set; }
    }
}
