using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Gerber
{
    public class Constant
    {
        public const int APERTURE_PARAMETERS_MAX = 102;
        public const int APERTURE_MIN = 10;
        public const int APERTURE_MAX = 9999;
    }

    public enum LayerType
    {
        RS_274x, Drill, PickNPlace
    }

    public enum Unit
    {
        Inch, /*!< inches */
        mm, /*!< mm */
        Unspecified/*!< use default units */
    }

    public enum ApertureType
    {
        None, /*!< no aperture used */
        Clrcle, /*!< a round aperture */
        Rectangle, /*!< a rectangular aperture */
        Oval, /*!< an ovular (obround) aperture */
        Polygon, /*!< a polygon aperture */
        Macro, /*!< a RS274X macro */
        MacroCircle, /*!< a RS274X circle macro */
        MacroOutline, /*!< a RS274X outline macro */
        MacroPolygon, /*!< a RS274X polygon macro */
        MacroMoire, /*!< a RS274X moire macro */
        MacroThermal, /*!< a RS274X thermal macro */
        MacroLine20, /*!< a RS274X line (code 20) macro */
        MacroLine21, /*!< a RS274X line (code 21) macro */
        MacroLine22 /*!< a RS274X line (code 22) macro */
    }

    public enum OpCode
    {
        NOP, /*!< no operation */
        Push, /*!< push the instruction onto the stack */
        PushParam, /*!< push parameter onto stack */
        PopParam, /*!< pop parameter from stack */
        Add, /*!< mathmatical add operation */
        Sub, /*!< mathmatical subtract operation */
        Mul, /*!< mathmatical multiply operation */
        Div, /*!< mathmatical divide operation */
        Primative /*!< draw macro primative */
    }

    public enum KnockOutType
    {
        NoKnockOut, FixedKnock, Border
    }

    public enum Polarity
    {
        Positive, /*!< draw "positive", using the current layer's polarity */
        Negative, /*!< draw "negative", reversing the current layer's polarity */
        Dark, /*!< add to the current rendering */
        Clear /*!< subtract from the current rendering */
    }

    public enum Interpolation
    {
        LinearX1,     /*!< draw a line */
        X10,          /*!< draw a line */
        LinearX01,    /*!< draw a line */
        LinearX001,   /*!< draw a line */
        CircularCW,  /*!< draw an arc in the clockwise direction */
        CircularCCW, /*!< draw an arc in the counter-clockwise direction */
        PolygonStart,  /*!< start a polygon draw */
        PolygonEnd,    /*!< end a polygon draw */
        Deleted       /*!< the net has been deleted by the user, and will not be drawn */
    }

    public enum ApertureState
    {
        Off,  /*!< tool drawing is off, and nothing will be drawn */
        On,   /*!< tool drawing is on, and something will be drawn */
        Flash /*!< tool is flashing, and will draw a single aperture */
    }

    public enum MirrorState
    {
        NoMirror, FlipA, FlipB, FlipAB
    }

    public enum AxisSelect
    {
        NoSelect, SwapAB
    }

    public enum OmitZeros
    {
        Leading,    /*!< omit extra zeros before the decimal point */
        Trailing,   /*!< omit extra zeros after the decimal point */
        Explicit,   /*!< explicitly specify how many decimal places are used */
        Unspecified /*!< use the default parsing style */
    }

    public enum Coordinate
    {
        Absolute,   /*!< all coordinates are absolute from a common origin */
        Incremental /*!< all coordinates are relative to the previous coordinate */
    }

    public enum Encoding
    {
        None, Ascii, EBCDIC, BCD, IsoAscii, EIA
    }

    public enum ImageJustify
    {
        NoJustify, LowerLeft, CenterJustify
    }

    public enum HIDType
    {
        Label, Integer, Real, String, Boolean, Enum, Mixed, Path
    }

    public enum MessageType
    {
        Fatal, /*!< processing cannot continue */
        Error, /*!< something went wrong, but processing can still continue */
        Warning, /*!< something was encountered that may provide the wrong output */
        Note /*!< an irregularity was encountered, but needs no intervention */
    }


    public enum MacroCircleIndex
    {
        EXPOSURE,
        DIAMETER,
        CENTER_X,
        CENTER_Y,
    }

    public enum MacroOutlineIndex
    {
        EXPOSURE,
        NUMBER_OF_POINTS,
        FIRST_X,
        FIRST_Y,
        ROTATION
    }

    public enum MacroPolygonIndex
    {
        EXPOSURE,
        NUMBER_OF_POINTS,
        CENTER_X,
        CENTER_Y,
        DIAMETER,
        ROTATION
    }

    public enum MacroMoireIndex
    {
        CENTER_X,
        CENTER_Y,
        OUTSIDE_DIAMETER,
        CIRCLE_THICKNESS,
        GAP_WIDTH,
        NUMBER_OF_CIRCLES,
        CROSSHAIR_THICKNESS,
        CROSSHAIR_LENGTH,
        ROTATION
    }

    public enum MacroThermalIndex
    {
        CENTER_X,
        CENTER_Y,
        OUTSIDE_DIAMETER,
        INSIDE_DIAMETER,
        CROSSHAIR_THICKNESS,
        ROTATION
    }

    public enum MacroLine20Index
    {
        EXPOSURE,
        LINE_WIDTH,
        START_X,
        START_Y,
        END_X,
        END_Y,
        ROTATION
    }

    public enum MacroLine21Index
    {
        EXPOSURE,
        WIDTH,
        HEIGHT,
        CENTER_X,
        CENTER_Y,
        ROTATION
    }

    public enum MacroLine22Index
    {
        EXPOSURE,
        WIDTH,
        HEIGHT,
        LOWER_LEFT_X,
        LOWER_LEFT_Y,
        ROTATION
    }
}
