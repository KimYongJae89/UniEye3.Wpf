using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Gerber
{
    public class GerberLoader
    {
        private GerberData gerberData;
        private GerberStat gerberStat;
        private GerberFormat format = new GerberFormat(); // Dummy

        private int currX;
        private int currY;
        private int prevX;
        private int prevY;
        private int curDeltaCpX;
        private int curDeltaCpY;
        private int curApertureNo;
        private int changed;
        private ApertureState curApertureState;
        private Interpolation curInterpolation;
        private Interpolation prevInterpolation;
        private Layer curLayer = null;
        private NetEntry curNetEntry = null;
        private NetEntryState curNetEntryState;
        private RectangleF boundingBox;
        private int polygonPoints;
        private bool inPolygonfill;
        private NetEntry polygonStartEntry;
        private bool useMultiQuad;
        private bool knockoutMeasure = false;
        private double knockoutLimitXmin;
        private double knockoutLimitYmin;
        private double knockoutLimitXmax;
        private double knockoutLimitYmax;
        private Layer knockoutLayer = null;
        private Matrix currentMatrix;

        public GerberLoader()
        {
            ResetBoundingBox();
        }

        private void ResetBoundingBox()
        {
            boundingBox = RectangleF.FromLTRB(float.MaxValue, float.MaxValue, float.MinValue, float.MinValue);
        }

        private void UpdateBoundingBox(Matrix matrix, double x, double y,
                double apertureSizeX1, double apertureSizeX2, double apertureSizeY1, double apertureSizeY2)
        {
            double ourX1 = x - apertureSizeX1, ourY1 = y - apertureSizeY1;
            double ourX2 = x + apertureSizeX2, ourY2 = y + apertureSizeY2;

            var ptArr = new PointF[] { new PointF((float)ourX1, (float)ourY1), new PointF((float)ourX2, (float)ourY2) };

            /* transform the point to the final rendered position, accounting
               for any scaling, offsets, mirroring, etc */
            /* NOTE: we need to already add/subtract in the aperture size since
               the final rendering may be scaled */

            matrix.TransformPoints(ptArr);

            /* check both points against the min/max, since depending on the rotation,
               mirroring, etc, either point could possibly be a min or max */

            boundingBox = RectangleF.FromLTRB(
                Math.Min(boundingBox.Left, Math.Min(ptArr[0].X, ptArr[1].X)), // Left
                Math.Max(boundingBox.Top, Math.Max(ptArr[0].Y, ptArr[1].Y)), // Top
                Math.Max(boundingBox.Right, Math.Max(ptArr[0].X, ptArr[1].X)), // Right
                Math.Min(boundingBox.Bottom, Math.Min(ptArr[0].Y, ptArr[1].Y))); // Bottom
        }

        private void UpdateBoundingBox(double repeatOffX, double repeatOffY)
        {
            GerberInfo info = gerberData.Info;

            if (boundingBox.Left < info.MinX)
            {
                info.MinX = boundingBox.Left;
            }

            if (boundingBox.Right + repeatOffX > info.MaxX)
            {
                info.MaxX = boundingBox.Right + repeatOffX;
            }

            if (boundingBox.Bottom < info.MinY)
            {
                info.MinY = boundingBox.Bottom;
            }

            if (boundingBox.Top + repeatOffY > info.MaxY)
            {
                info.MaxY = boundingBox.Top + repeatOffY;
            }
        }

        public void DebugLog(string message)
        {
            LogHelper.Debug(LoggerType.Operation, message);
        }

        public GerberData Load(string fileName)
        {
            bool eofFound = false;

            try
            {
                using (var reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
                {
                    gerberData = new GerberData();
                    gerberStat = gerberData.GerberStat;
                    curLayer = gerberData.GetLastLayer();
                    curNetEntryState = gerberData.GetLastNetEntryState();

                    while (eofFound == false)
                    {
                        char ch = (char)reader.ReadByte();
                        switch (ch)
                        {
                            case 'G':
                                ParseGCode(reader);
                                break;
                            case 'D':
                                ParseDCode(reader);
                                break;
                            case 'M':
                                eofFound = ParseMCode(reader) > 0;
                                break;
                            case 'X':
                                DebugLog("... Found X code\n");
                                gerberStat.X++;
                                GetPos(reader, format.XNumInteger, format.XNumDecimal, ref currX);
                                changed = 1;
                                break;
                            case 'Y':
                                DebugLog("... Found Y code\n");
                                gerberStat.Y++;
                                GetPos(reader, format.YNumInteger, format.YNumDecimal, ref currY);
                                changed = 1;
                                break;
                            case 'I':
                                DebugLog("... Found I code\n");
                                gerberStat.I++;
                                GetIncremental(reader, format.XNumInteger, format.XNumDecimal, ref curDeltaCpX);
                                changed = 1;
                                break;
                            case 'J':
                                DebugLog("... Found J code\n");
                                gerberStat.I++;
                                GetIncremental(reader, format.YNumInteger, format.YNumDecimal, ref curDeltaCpY);
                                changed = 1;
                                break;
                            case '%':
                                DebugLog("... Found %% code\n");
                                while (true)
                                {
                                    ParseRs274x(reader);
                                    /* advance past any whitespace here */
                                    ch = (char)reader.ReadByte();
                                    while ((ch == '\n') || (ch == '\r') || (ch == ' ') || (ch == '\t') || (ch == 0))
                                    {
                                        ch = (char)reader.ReadByte();
                                    }

                                    if (ch == '%')
                                    {
                                        break;
                                    }
                                    // loop again to catch multiple blocks on the same line (separated by * char)
                                    reader.BaseStream.Position--;
                                }
                                break;
                            case '*':
                                FinalizeLine();
                                break;
                            case (char)10:   /* White space */
                            case (char)13:
                            case ' ':
                            case '\t':
                            case (char)0:
                                break;
                            default:
                                gerberStat.Unknown++;
                                gerberStat.AddError(string.Format("Found unknown character (whitespace?) [%d]%c", ch, ch));
                                break;
                        }
                    }
                }
            }
            catch (EndOfStreamException)
            {
                if (eofFound == false)
                {
                    gerberStat.AddError(string.Format("Unexpected EOF found in file. {0} ", fileName));
                }
            }

            return gerberData;
        }

        private void GetPos(BinaryReader reader, int numInteger, int numDecimal, ref int posTarget)
        {
            int len = 0;
            int pos = GerberLoaderHelper.ReadInteger(reader, ref len);
            if (format.OmitZeros == OmitZeros.Trailing)
            {
                int pow = ((numInteger + numDecimal) - len);
                if (pow > 0)
                {
                    pos *= (int)Math.Pow(10, pow);
                }
            }

            if (format.Coordinate == Coordinate.Incremental)
            {
                posTarget += pos;
            }
            else
            {
                posTarget = pos;
            }
        }

        private void GetIncremental(BinaryReader reader, int numInteger, int numDecimal, ref int incTarget)
        {
            int len = 0;
            int pos = GerberLoaderHelper.ReadInteger(reader, ref len);
            if (format.OmitZeros == OmitZeros.Trailing)
            {
                int pow = ((numInteger + numDecimal) - len);
                if (pow > 0)
                {
                    pos *= (int)Math.Pow(10, pow);
                }
            }

            incTarget = pos;
        }

        private Layer CreateNewLayer()
        {
            Layer newLayer;
            if (curLayer == null)
            {
                newLayer = new Layer();
            }
            else
            {
                newLayer = curLayer.Clone();
            }

            gerberData.AddLayer(newLayer);

            return newLayer;
        }

        private NetEntry CreateNetEntry(Layer layer = null, NetEntryState netEntryState = null)
        {
            var netEntry = new NetEntry();

            if (layer != null)
            {
                netEntry.Layer = layer;
            }
            else if (curNetEntry != null)
            {
                netEntry.Layer = curNetEntry.Layer;
            }

            if (netEntryState != null)
            {
                netEntry.State = netEntryState;
            }
            else if (curNetEntry != null)
            {
                netEntry.State = curNetEntry.State;
            }

            gerberData.AddNetEntry(netEntry);

            return netEntry;
        }

        private NetEntryState CreateNetEntryState()
        {
            var netEntryState = new NetEntryState();

            gerberData.AddNetEntryState(netEntryState);

            return netEntryState;
        }

        private void UpdateKnockoutLayer()
        {
            if (knockoutMeasure)
            {
                knockoutLayer.Knockout.LowerLeftX = knockoutLimitXmin;
                knockoutLayer.Knockout.LowerLeftY = knockoutLimitYmin;
                knockoutLayer.Knockout.Width = knockoutLimitXmax - knockoutLimitXmin;
                knockoutLayer.Knockout.Height = knockoutLimitYmax - knockoutLimitYmin;
                knockoutMeasure = false;
            }
        }

        private void ParseGCode(BinaryReader reader)
        {
            char ch;

            int op_int = GerberLoaderHelper.ReadInteger(reader);

            DebugLog(string.Format("parse_G_code, D{0} ... \n", op_int));

            switch (op_int)
            {
                case 0:  /* Move */
                    /* Is this doing anything really? */
                    gerberStat.G0++;
                    break;
                case 1:  /* Linear Interpolation (1X scale) */
                    curInterpolation = Interpolation.LinearX1;
                    gerberStat.G1++;
                    break;
                case 2:  /* Clockwise Linear Interpolation */
                    curInterpolation = Interpolation.CircularCW;
                    gerberStat.G2++;
                    break;
                case 3:  /* Counter Clockwise Linear Interpolation */
                    curInterpolation = Interpolation.CircularCCW;
                    gerberStat.G3++;
                    break;
                case 4:  /* Ignore Data Block */
                    /* Don't do anything, just read 'til * */
                    /* SDB asks:  Should we look for other codes while reading G04 in case
                 * user forgot to put * at end of comment block? */
                    do
                    {
                        ch = (char)reader.ReadByte();
                    }
                    while (ch != '*');
                    gerberStat.G4++;
                    break;
                case 10: /* Linear Interpolation (10X scale) */
                    curInterpolation = Interpolation.X10;
                    gerberStat.G10++;
                    break;
                case 11: /* Linear Interpolation (0.1X scale) */
                    curInterpolation = Interpolation.LinearX01;
                    gerberStat.G11++;
                    break;
                case 12: /* Linear Interpolation (0.01X scale) */
                    curInterpolation = Interpolation.LinearX001;
                    gerberStat.G12++;
                    break;
                case 36: /* Turn on Polygon Area Fill */
                    prevInterpolation = curInterpolation;
                    curInterpolation = Interpolation.PolygonStart;
                    changed = 1;
                    gerberStat.G36++;
                    break;
                case 37: /* Turn off Polygon Area Fill */
                    curInterpolation = Interpolation.PolygonEnd;
                    changed = 1;
                    gerberStat.G37++;
                    break;
                case 54: /* Tool prepare */
                    /* XXX Maybe uneccesary??? */
                    ch = (char)reader.ReadByte();
                    if (ch == 'D')
                    {
                        int a = GerberLoaderHelper.ReadInteger(reader);
                        if ((a >= 0) && (a <= Constant.APERTURE_MAX))
                        {
                            curApertureNo = a;
                        }
                        else
                        {
                            gerberStat.AddError(string.Format("Found aperture D{0} out of bounds while parsing G code in file.", a));
                        }
                    }
                    else
                    {
                        gerberStat.AddError(string.Format("Found unexpected code '{0}' after G54 in file.", ch));
                        /* Must insert error count here */
                    }
                    gerberStat.G54++;
                    break;
                case 55: /* Prepare for flash */
                    gerberStat.G55++;
                    break;
                case 70: /* Specify inches */
                    curNetEntryState = CreateNetEntryState();
                    curNetEntryState.Unit = Unit.Inch;
                    gerberStat.G70++;
                    break;
                case 71: /* Specify millimeters */
                    curNetEntryState = CreateNetEntryState();
                    curNetEntryState.Unit = Unit.mm;
                    gerberStat.G71++;
                    break;
                case 74: /* Disable 360 circular interpolation */
                    useMultiQuad = false;
                    gerberStat.G74++;
                    break;
                case 75: /* Enable 360 circular interpolation */
                    useMultiQuad = true;
                    gerberStat.G75++;
                    break;
                case 90: /* Specify absolut format */
                    format.Coordinate = Coordinate.Absolute;
                    gerberStat.G90++;
                    break;
                case 91: /* Specify incremental format */
                    format.Coordinate = Coordinate.Incremental;
                    gerberStat.G91++;
                    break;
                default:
                    gerberStat.AddError(string.Format("Encountered unknown G code G'{0}' after G54 in file.", op_int));
                    gerberStat.GUnknown++;
                    /* Enter error count here */
                    break;
            }

            return;
        }

        private void ParseDCode(BinaryReader reader)
        {
            int op_int = GerberLoaderHelper.ReadInteger(reader);

            DebugLog(string.Format("parse_D_code, D{0} ... \n", op_int));
            switch (op_int)
            {
                case 0: /* Invalid code */
                    gerberStat.AddError("Found invalid D00 code.");
                    gerberStat.DError++;
                    break;
                case 1: /* Exposure on */
                    curApertureState = ApertureState.On;
                    changed = 1;
                    gerberStat.D1++;
                    break;
                case 2: /* Exposure off */
                    curApertureState = ApertureState.Off;
                    changed = 1;
                    gerberStat.D2++;
                    break;
                case 3: /* Flash aperture */
                    curApertureState = ApertureState.Flash;
                    changed = 1;
                    gerberStat.D3++;
                    break;
                default: /* Aperture in use */
                    if ((op_int >= 0) && (op_int <= Constant.APERTURE_MAX))
                    {
                        curApertureNo = op_int;
                    }
                    else
                    {
                        gerberStat.AddError(string.Format("Found out of bounds aperture D{0}.", op_int));
                        gerberStat.DError++;
                    }
                    changed = 0;
                    break;
            }

            return;
        }

        private int ParseMCode(BinaryReader reader)
        {
            int op_int = GerberLoaderHelper.ReadInteger(reader);

            DebugLog(string.Format("parse_M_code, D{0} ... \n", op_int));

            switch (op_int)
            {
                case 0:  /* Program stop */
                    gerberStat.M0++;
                    return 1;
                case 1:  /* Optional stop */
                    gerberStat.M1++;
                    return 2;
                case 2:  /* End of program */
                    gerberStat.M2++;
                    return 3;
                default:
                    gerberStat.AddError(string.Format("Encountered unknown M code M'{0}'.", op_int));
                    gerberStat.MUnknown++;
                    break;
            }
            return 0;
        }


        /* ------------------------------------------------------------------ */
        private Aperture ParseAperture(BinaryReader reader, double scale)
        {
            char ch = (char)reader.ReadByte();
            if (ch != 'D')
            {
                return null;
            }

            int apertureNo = GerberLoaderHelper.ReadInteger(reader);

            /*
             * Read in the whole aperture defintion and tokenize it
             */
            string valueStr = GerberLoaderHelper.ReadString(reader, '*');

            if (string.IsNullOrEmpty(valueStr))
            {
                gerberStat.AddError("Invalid aperture definition.");
                return null;
            }

            var aperture = new Aperture();
            aperture.ApertureNo = apertureNo;

            string[] valueToken = valueStr.Split(',');
            if (valueToken[0].Length == 1)
            {
                switch (valueToken[0][0])
                {
                    case 'C':
                        aperture.Type = ApertureType.Clrcle;
                        break;
                    case 'R':
                        aperture.Type = ApertureType.Rectangle;
                        break;
                    case 'O':
                        aperture.Type = ApertureType.Oval;
                        break;
                    case 'P':
                        aperture.Type = ApertureType.Polygon;
                        break;
                }
            }
            else
            {
                aperture.Type = ApertureType.Macro;
                /*
                 * In aperture definition, point to the aperture macro 
                 * used in the defintion
                 */
                aperture.Macro = gerberData.GetApertureMacro(valueStr);
            }

            if (valueToken.Length > 1)
            {
                string[] paramToken = valueToken[1].Split('X');

                for (int i = 0; i < paramToken.Length; i++)
                {
                    if (i == Constant.APERTURE_PARAMETERS_MAX)
                    {
                        gerberStat.AddError("Maximum number of allowed parameters exceeded in aperture.");
                        break;
                    }

                    try
                    {
                        double value = Convert.ToDouble(paramToken[i]);
                        /* convert any MM values to inches */
                        /* don't scale polygon angles or side numbers, or macro parmaeters */
                        if (!(((aperture.Type == ApertureType.Polygon) && ((i == 1) || (i == 2))) ||
                            (aperture.Type == ApertureType.Macro)))
                        {
                            value /= scale;
                        }

                        aperture.Parameter[i] = value;
                    }
                    catch (FormatException)
                    {
                        gerberStat.AddError("Failed to read all parameters exceeded in aperture.");
                        aperture.Parameter[i] = 0;
                    }
                }
            }

            if (aperture.Type == ApertureType.Macro)
            {
                if (aperture.Macro == null)
                {
                    gerberStat.AddError("Aperture Macro NULL in simplify aperture macro\n");
                    return null;
                }

                DebugLog(string.Format("Simplifying aperture {0} using aperture macro {1}", aperture.ApertureNo, aperture.Macro.Name));

                if (aperture.AddSimplifiedApertureMacro(scale) == false)
                {
                    gerberStat.AddError("Some error on adding simplified aperture macro.");
                }

                DebugLog("Done simplifying");
            }

            return aperture;
        } /* parse_aperture_definition */

        /* ------------------------------------------------------------------ */
        private void ParseRs274x(BinaryReader reader)
        {
            double scale = 1.0;
            if (curNetEntryState.Unit == Unit.mm)
            {
                scale = 25.4;
            }

            int value;
            char ch;
            string contextStr = System.Text.Encoding.Default.GetString(reader.ReadBytes(2));
            string valueStr;

            switch (contextStr.ToString())
            {
                /* 
                 * Directive parameters 
                 */
                case "AS": /* Axis Select */
                    valueStr = System.Text.Encoding.Default.GetString(reader.ReadBytes(2));

                    curNetEntryState = gerberData.AddNewEntryState();

                    if (valueStr == "AY" || valueStr == "BX")
                    {
                        curNetEntryState.AxisSelect = AxisSelect.SwapAB;
                    }
                    else
                    {
                        curNetEntryState.AxisSelect = AxisSelect.NoSelect;
                    }
                    break;

                case "FS": /* Format Statement */
                    format.Clear();

                    ch = (char)reader.ReadByte();
                    switch (ch)
                    {
                        case 'L':
                            format.OmitZeros = OmitZeros.Leading;
                            break;
                        case 'T':
                            format.OmitZeros = OmitZeros.Trailing;
                            break;
                        case 'D':
                            format.OmitZeros = OmitZeros.Explicit;
                            break;
                        default:
                            gerberStat.AddError(string.Format("Undefined handling of zeros in format code '{0}'.", ch));
                            reader.BaseStream.Position--;
                            format.OmitZeros = OmitZeros.Leading;
                            break;
                    }

                    ch = (char)reader.ReadByte();
                    switch (ch)
                    {
                        case 'A':
                            format.Coordinate = Coordinate.Absolute;
                            break;
                        case 'I':
                            format.Coordinate = Coordinate.Incremental;
                            break;
                        default:
                            gerberStat.AddError(string.Format("Invalid coordinate type in format code '{0}'.", ch));
                            format.Coordinate = Coordinate.Absolute;
                            break;
                    }

                    ch = (char)reader.ReadByte();
                    while (ch != '*')
                    {
                        value = (char)reader.ReadByte() - '0';
                        switch (ch)
                        {
                            case 'N':
                                format.LimitSeqNo = value;
                                break;
                            case 'G':
                                format.LimitGeneralFunc = value;
                                break;
                            case 'D':
                                format.LimitPlotFunc = value;
                                break;
                            case 'M':
                                format.LimitMiscFunc = value;
                                break;
                            case 'X':
                                if ((value < 0) || (value > 6))
                                {
                                    gerberStat.AddError(string.Format("Illegal format of X Num integer : {0}.", value));
                                }
                                else
                                {
                                    format.XNumInteger = value;
                                }

                                value = (char)reader.ReadByte() - '0';
                                if ((value < 0) || (value > 6))
                                {
                                    gerberStat.AddError(string.Format("Illegal format of X Num Decimal : {0}.", value));
                                }
                                else
                                {
                                    format.XNumDecimal = value;
                                }

                                break;
                            case 'Y':
                                if ((value < 0) || (value > 6))
                                {
                                    gerberStat.AddError(string.Format("Illegal format of Y Num integer : {0}.", value));
                                }
                                else
                                {
                                    format.YNumInteger = value;
                                }

                                value = (char)reader.ReadByte() - '0';
                                if ((value < 0) || (value > 6))
                                {
                                    gerberStat.AddError(string.Format("Illegal format of Y Num Decimal : {0}.", value));
                                }
                                else
                                {
                                    format.YNumDecimal = value;
                                }

                                break;
                            default:
                                reader.BaseStream.Position--;
                                gerberStat.AddError(string.Format("Illegal format statement in format code '{0}'.", ch));
                                break;
                        }
                        ch = (char)reader.ReadByte();
                    }
                    break;
                case "MI": /* Mirror Image */
                    ch = (char)reader.ReadByte();
                    curNetEntryState = gerberData.AddNewEntryState();

                    while (ch != '*')
                    {
                        switch (ch)
                        {
                            case 'A':
                                value = GerberLoaderHelper.ReadInteger(reader);
                                if (value == 1)
                                {
                                    if (curNetEntryState.MirrorState == MirrorState.FlipB)
                                    {
                                        curNetEntryState.MirrorState = MirrorState.FlipAB;
                                    }
                                    else
                                    {
                                        curNetEntryState.MirrorState = MirrorState.FlipA;
                                    }
                                }
                                break;
                            case 'B':
                                value = GerberLoaderHelper.ReadInteger(reader);
                                if (value == 1)
                                {
                                    if (curNetEntryState.MirrorState == MirrorState.FlipA)
                                    {
                                        curNetEntryState.MirrorState = MirrorState.FlipAB;
                                    }
                                    else
                                    {
                                        curNetEntryState.MirrorState = MirrorState.FlipB;
                                    }
                                }
                                break;
                            default:
                                gerberStat.AddError(string.Format("Illegal format statement in format code '{0}'.", ch));
                                break;
                        }
                        ch = (char)reader.ReadByte();
                    }
                    break;
                case "MO": /* Mode of Units */

                    valueStr = System.Text.Encoding.Default.GetString(reader.ReadBytes(2));
                    curNetEntryState = gerberData.AddNewEntryState();

                    switch (valueStr)
                    {
                        case "IN":
                            curNetEntryState.Unit = Unit.Inch;
                            break;
                        case "MM":
                            curNetEntryState.Unit = Unit.mm;
                            break;
                        default:
                            gerberStat.AddError(string.Format("Illegal uni '{0}'.", valueStr));
                            break;
                    }
                    break;
                case "OF": /* Offset */
                    ch = (char)reader.ReadByte();

                    while (ch != '*')
                    {
                        switch (ch)
                        {
                            case 'A':
                                curNetEntryState.OffsetA = GerberLoaderHelper.ReadDouble(reader) / scale;
                                break;
                            case 'B':
                                curNetEntryState.OffsetB = GerberLoaderHelper.ReadDouble(reader) / scale;
                                break;
                            default:
                                gerberStat.AddError(string.Format("Wrong character in offset '{0}'.", ch));
                                break;
                        }
                        ch = (char)reader.ReadByte();
                    }
                    break;
                //case "IF": /* Include file */
                //    {
                //        string includeFilename = ReadString(reader, '*');

                //        if (String.IsNullOrEmpty(includeFilename) == false)
                //        {
                //            gchar* fullPath;
                //            if (!g_path_is_absolute(includeFilename))
                //            {
                //                fullPath = g_build_filename(directoryPath, includeFilename, NULL);
                //            }
                //            else
                //            {
                //                fullPath = g_strdup(includeFilename);
                //            }

                //            if (levelOfRecursion < 10)
                //            {
                //                gerb_file_t* includefd = NULL;

                //                includefd = gerb_fopen(fullPath);
                //                if (includefd)
                //                {
                //                    gerber_parse_file_segment(levelOfRecursion + 1, image, state, curr_net, stats, includefd, directoryPath);
                //                    gerb_fclose(includefd);
                //                }
                //                else
                //                {
                //                    string = g_strdup_printf("In file %s,\nIncluded file %s cannot be found\n",
                //                                 fd->filename, fullPath);
                //                    gerbv_stats_add_error(stats->error_list,
                //                                  -1,
                //                                  string,
                //                                  GERBV_MESSAGE_ERROR);
                //                    g_free(string);
                //                }
                //                g_free(fullPath);
                //            }
                //            else
                //            {
                //                string = g_strdup_printf("Parser encountered more than 10 levels of include file recursion which is not allowed by the RS-274X spec\n");
                //                gerbv_stats_add_error(stats->error_list,
                //                          -1,
                //                          string,
                //                          GERBV_MESSAGE_ERROR);
                //                g_free(string);
                //            }

                //        }
                //    }
                //    break;
                case "IO": /* Image offset */
                    ch = (char)reader.ReadByte();

                    while (ch != '*')
                    {
                        switch (ch)
                        {
                            case 'A':
                                gerberData.Info.OffsetA = GerberLoaderHelper.ReadDouble(reader) / scale;
                                break;
                            case 'B':
                                gerberData.Info.OffsetB = GerberLoaderHelper.ReadDouble(reader) / scale;
                                break;
                            default:
                                gerberStat.AddError(string.Format("Wrong character in image offset '{0}'.", ch));
                                break;
                        }
                        ch = (char)reader.ReadByte();
                    }
                    break;
                case "SF": /* Scale Factor */
                    ch = (char)reader.ReadByte();
                    curNetEntryState = gerberData.AddNewEntryState();

                    while (ch != '*')
                    {
                        switch (ch)
                        {
                            case 'A':
                                curNetEntryState.ScaleA = GerberLoaderHelper.ReadDouble(reader);
                                break;
                            case 'B':
                                curNetEntryState.ScaleB = GerberLoaderHelper.ReadDouble(reader);
                                break;
                            default:
                                gerberStat.AddError(string.Format("Wrong character in image offset '{0}'.", ch));
                                break;
                        }
                        ch = (char)reader.ReadByte();
                    }
                    break;
                case "IC": /* Input Code */
                    /* Thanks to Stephen Adam for providing this information. As he writes:
                     *      btw, here's a logic puzzle for you.  If you need to
                     * read the gerber file to see how it's encoded, then
                     * how can you read it?
                     */

                    valueStr = System.Text.Encoding.Default.GetString(reader.ReadBytes(2));

                    switch (valueStr)
                    {
                        case "AS":
                            gerberData.Info.Encoding = Encoding.Ascii;
                            break;
                        case "EB":
                            gerberData.Info.Encoding = Encoding.EBCDIC;
                            break;
                        case "BC":
                            gerberData.Info.Encoding = Encoding.BCD;
                            break;
                        case "IS":
                            gerberData.Info.Encoding = Encoding.IsoAscii;
                            break;
                        case "EI":
                            gerberData.Info.Encoding = Encoding.EIA;
                            break;
                        default:
                            gerberStat.AddError(string.Format("Wrong nunknown input code '{0}'.", valueStr));
                            break;
                    }
                    break;

                /* Image parameters */
                case "IJ": /* Image Justify */
                    ch = (char)reader.ReadByte();
                    gerberData.Info.ImageJustifyTypeA = ImageJustify.LowerLeft;
                    gerberData.Info.ImageJustifyTypeB = ImageJustify.LowerLeft;
                    gerberData.Info.ImageJustifyOffsetA = 0.0;
                    gerberData.Info.ImageJustifyOffsetB = 0.0;

                    while (ch != '*')
                    {
                        switch (ch)
                        {
                            case 'A':
                                ch = (char)reader.ReadByte();
                                if (ch == 'C')
                                {
                                    gerberData.Info.ImageJustifyTypeA = ImageJustify.CenterJustify;
                                }
                                else if (ch == 'L')
                                {
                                    gerberData.Info.ImageJustifyTypeA = ImageJustify.LowerLeft;
                                }
                                else
                                {
                                    reader.BaseStream.Position--;
                                    gerberData.Info.ImageJustifyOffsetA = GerberLoaderHelper.ReadDouble(reader) / scale;
                                }
                                break;
                            case 'B':
                                ch = (char)reader.ReadByte();
                                if (ch == 'C')
                                {
                                    gerberData.Info.ImageJustifyTypeB = ImageJustify.CenterJustify;
                                }
                                else if (ch == 'L')
                                {
                                    gerberData.Info.ImageJustifyTypeB = ImageJustify.LowerLeft;
                                }
                                else
                                {
                                    reader.BaseStream.Position--;
                                    gerberData.Info.ImageJustifyOffsetB = GerberLoaderHelper.ReadDouble(reader) / scale;
                                }
                                break;
                            default:
                                gerberStat.AddError(string.Format("nwrong character in image justify '{0}'.", ch));
                                break;
                        }
                        ch = (char)reader.ReadByte();
                    }
                    break;
                case "IN": /* Image Name */
                    gerberData.Info.Name = GerberLoaderHelper.ReadString(reader, '*');
                    break;
                case "IP": /* Image Polarity */

                    valueStr = System.Text.Encoding.Default.GetString(reader.ReadBytes(3));

                    if (valueStr == "POS")
                    {
                        gerberData.Info.Polarity = Polarity.Positive;
                    }
                    else if (valueStr == "NEG")
                    {
                        gerberData.Info.Polarity = Polarity.Negative;
                    }
                    else
                    {
                        gerberStat.AddError(string.Format("Unknown polarity '{0}'.", valueStr));
                    }

                    break;
                case "IR": /* Image Rotation */
                    value = GerberLoaderHelper.ReadInteger(reader) % 360;
                    if (value == 0)
                    {
                        gerberData.Info.ImageRotation = 0.0;
                    }
                    else if (value == 90)
                    {
                        gerberData.Info.ImageRotation = Math.PI / 2.0;
                    }
                    else if (value == 180)
                    {
                        gerberData.Info.ImageRotation = Math.PI;
                    }
                    else if (value == 270)
                    {
                        gerberData.Info.ImageRotation = 3.0 * Math.PI / 2.0;
                    }
                    else
                    {
                        gerberStat.AddError(string.Format("Image rotation must be 0, 90, 180 or 270 : '{0}'.", value));
                    }

                    break;
                case "PF": /* Plotter Film */
                    gerberData.Info.PlotterFilm = GerberLoaderHelper.ReadString(reader, '*');
                    break;
                /* Aperture parameters */
                case "AD": /* Aperture Description */
                    Aperture aperture = ParseAperture(reader, scale);
                    if (aperture == null)
                    {
                        break;
                    }

                    int apertureNo = aperture.ApertureNo;
                    if ((apertureNo >= 0) && (apertureNo <= Constant.APERTURE_MAX))
                    {
                        aperture.Unit = curNetEntryState.Unit;

                        gerberData.AddAperture(aperture);

                        // gerbv_stats_add_to_D_list(stats->D_code_list, ano);
                        if (apertureNo < Constant.APERTURE_MIN)
                        {
                            gerberStat.AddError(string.Format("naperture number out of bounds : '{0}'.", apertureNo));
                        }
                    }
                    else
                    {
                        gerberStat.AddError(string.Format("naperture number out of bounds : '{0}'.", apertureNo));
                    }
                    /* Add aperture info to stats->aperture_list here */
                    break;
                case "AM": /* Aperture Macro */
                    var apertureMacro = ApertureMacro.Parse(reader);
                    if (apertureMacro != null)
                    {
                        gerberData.AddApertureMacro(apertureMacro);
#if DEBUG
                        apertureMacro.Print();
#endif
                    }
                    else
                    {
                        gerberStat.AddError("failed to parse aperture macro");
                    }
                    // return, since we want to skip the later back-up loop
                    return;
                /* Layer */
                case "LN": /* Layer Name */
                    curLayer = CreateNewLayer();
                    curLayer.Name = GerberLoaderHelper.ReadString(reader, '*');
                    break;
                case "LP": /* Layer Polarity */
                    curLayer = CreateNewLayer();

                    ch = (char)reader.ReadByte();
                    switch (ch)
                    {
                        case 'D': /* Dark Polarity (default) */
                            curLayer.Polarity = Polarity.Dark;
                            break;
                        case 'C': /* Clear Polarity */
                            curLayer.Polarity = Polarity.Clear;
                            break;
                        default:
                            gerberStat.AddError(string.Format("nunknown Layer Polarity {0}", ch));
                            break;
                    }
                    break;
                case "KO": /* Knock Out */
                    curLayer = CreateNewLayer();
                    curLayer.Knockout = new Knockout();

                    UpdateKnockoutLayer();
                    /* reset any previous knockout measurements */
                    knockoutMeasure = false;
                    ch = (char)reader.ReadByte();
                    if (ch == '*')
                    { /* Disable previous KO parameters */
                        curLayer.Knockout.Type = KnockOutType.NoKnockOut;
                        break;
                    }
                    else if (ch == 'C')
                    {
                        curLayer.Knockout.Polarity = Polarity.Clear;
                    }
                    else if (ch == 'D')
                    {
                        curLayer.Knockout.Polarity = Polarity.Dark;
                    }
                    else
                    {
                        gerberStat.AddError(string.Format("Invalid knockout polarity {0}", ch));
                    }
                    curLayer.Knockout.Clear();

                    ch = (char)reader.ReadByte();
                    while (ch != '*')
                    {
                        switch (ch)
                        {
                            case 'X':
                                curLayer.Knockout.Type = KnockOutType.FixedKnock;
                                curLayer.Knockout.LowerLeftX = GerberLoaderHelper.ReadDouble(reader) / scale;
                                break;
                            case 'Y':
                                curLayer.Knockout.Type = KnockOutType.FixedKnock;
                                curLayer.Knockout.LowerLeftY = GerberLoaderHelper.ReadDouble(reader) / scale;
                                break;
                            case 'I':
                                curLayer.Knockout.Type = KnockOutType.FixedKnock;
                                curLayer.Knockout.Width = GerberLoaderHelper.ReadDouble(reader) / scale;
                                break;
                            case 'J':
                                curLayer.Knockout.Type = KnockOutType.FixedKnock;
                                curLayer.Knockout.Height = GerberLoaderHelper.ReadDouble(reader) / scale;
                                break;
                            case 'K':
                                curLayer.Knockout.Type = KnockOutType.Border;
                                curLayer.Knockout.Border = GerberLoaderHelper.ReadDouble(reader) / scale;
                                /* this is a bordered knockout, so we need to start measuring the
                                   size of a square bordering all future components */
                                knockoutMeasure = true;
                                knockoutLimitXmin = double.MaxValue;
                                knockoutLimitYmin = double.MaxValue;
                                knockoutLimitXmax = double.MinValue;
                                knockoutLimitYmax = double.MinValue;
                                knockoutLayer = curLayer;
                                break;
                            default:
                                gerberStat.AddError(string.Format("Invalid knockout polarity {0}", ch));
                                break;
                        }
                        ch = (char)reader.ReadByte();
                    }
                    break;
                case "SR": /* Step and Repeat */
                    /* start by generating a new layer (duplicating previous layer settings */
                    curLayer = CreateNewLayer();
                    curLayer.StepNRepeat = new StepNRepeat();

                    ch = (char)reader.ReadByte();
                    if (ch == '*')
                    { /* Disable previous SR parameters */
                        curLayer.StepNRepeat.Reset();
                        break;
                    }

                    while (ch != '*')
                    {
                        switch (ch)
                        {
                            case 'X':
                                curLayer.StepNRepeat.X = GerberLoaderHelper.ReadInteger(reader);
                                break;
                            case 'Y':
                                curLayer.StepNRepeat.Y = GerberLoaderHelper.ReadInteger(reader);
                                break;
                            case 'I':
                                curLayer.StepNRepeat.DistX = GerberLoaderHelper.ReadDouble(reader) / scale;
                                break;
                            case 'J':
                                curLayer.StepNRepeat.DistY = GerberLoaderHelper.ReadDouble(reader) / scale;
                                break;
                            default:
                                gerberStat.AddError(string.Format("Invalid nstep-and-repeat parameter {0}", ch));
                                break;
                        }

                        /*
                         * Repeating 0 times in any direction would disable the whole plot, and
                         * is probably not intended. At least one other tool (viewmate) seems
                         * to interpret 0-time repeating as repeating just once too.
                         */
                        if (curLayer.StepNRepeat.X == 0)
                        {
                            curLayer.StepNRepeat.X = 1;
                        }

                        if (curLayer.StepNRepeat.Y == 0)
                        {
                            curLayer.StepNRepeat.Y = 1;
                        }

                        ch = (char)reader.ReadByte();
                    }
                    break;
                /* is this an actual RS274X command??  It isn't explainined in the spec... */
                case "RO":
                    curLayer = CreateNewLayer();

                    curLayer.Rotation = GerberLoaderHelper.ReadDouble(reader) * Math.PI / 180;

                    ch = (char)reader.ReadByte();
                    if (ch != '*')
                    {
                        gerberStat.AddError(string.Format("Invalid Rotation End {0}", ch));
                    }
                    break;
                default:
                    gerberStat.AddError(string.Format("Unknown RS-274X extension {0}", contextStr));
                    break;
            }
            // make sure we read until the trailing * character
            // first, backspace once in case we already read the trailing *
            reader.BaseStream.Position--;

            ch = (char)reader.ReadByte();
            while (ch != '*')
            {
                ch = (char)reader.ReadByte();
            }

            return;
        } /* parse_rs274x */

        private void FinalizeLine()
        {
            DebugLog("... Found * code\n");
            gerberStat.Star++;
            if (changed == 0)
            {
                return;
            }

            changed = 0;

            /* don't even bother saving the net if the aperture state is GERBV_APERTURE_STATE_OFF and we
               aren't starting a polygon fill (where we need it to get to the start point) */
            if ((curApertureState == ApertureState.Off) && (!inPolygonfill) &&
                    (curInterpolation != Interpolation.PolygonStart))
            {
                /* save the coordinate so the next net can use it for a start point */
                prevX = currX;
                prevY = currY;
                return;
            }

            curNetEntry = CreateNetEntry(curLayer, curNetEntryState);

            /*
             * Scale to given coordinate format
             * XXX only "omit leading zeros".
             */

            double scale = 1.0;
            if (curNetEntryState.Unit == Unit.mm)
            {
                scale = 25.4;
            }

            double scaleX = 0.0, scaleY = 0.0;
            double deltaCpX = 0.0, deltaCpY = 0.0;
            if (format != null)
            {
                scaleX = Math.Pow(10.0, format.XNumDecimal);
                scaleY = Math.Pow(10.0, format.YNumDecimal);
            }
            scaleX *= scale;
            scaleY *= scale;
            curNetEntry.StartX = prevX / scaleX;
            curNetEntry.StartY = prevY / scaleY;
            curNetEntry.StopX = currX / scaleX;
            curNetEntry.StopY = currY / scaleY;
            deltaCpX = curDeltaCpX / scaleX;
            deltaCpY = curDeltaCpY / scaleY;
            switch (curInterpolation)
            {
                case Interpolation.CircularCW:
                    curNetEntry.CircleSegment = new CircleSegment();
                    curNetEntry.CircleSegment.Initialize(curNetEntry, true, useMultiQuad, deltaCpX, deltaCpY);
                    break;
                case Interpolation.CircularCCW:
                    curNetEntry.CircleSegment = new CircleSegment();
                    curNetEntry.CircleSegment.Initialize(curNetEntry, false, useMultiQuad, deltaCpX, deltaCpY);
                    break;
                case Interpolation.PolygonStart:
                    /* 
                     * To be able to get back and fill in number of polygon corners
                     */
                    polygonStartEntry = curNetEntry;
                    inPolygonfill = true;
                    polygonPoints = 0;
                    /* reset the bounding box */
                    ResetBoundingBox();
                    break;
                case Interpolation.PolygonEnd:
                    /* save the calculated bounding box to the master node */
                    polygonStartEntry.BoundingBox = boundingBox;
                    /* close out the polygon */
                    polygonStartEntry = null;
                    inPolygonfill = false;
                    polygonPoints = 0;
                    break;
                default:
                    break;
            }  /* switch(state->interpolation) */

            /* 
             * Count number of points in Polygon Area 
             */
            if (inPolygonfill && polygonStartEntry != null)
            {
                /* 
                 * "...all lines drawn with D01 are considered edges of the
                 * polygon. D02 closes and fills the polygon."
                 * p.49 rs274xrevd_e.pdf
                 * D02 -> state->aperture_state == GERBV_APERTURE_STATE_OFF
                 */

                /* UPDATE: only end the polygon during a D02 call if we've already
                   drawn a polygon edge (with D01) */

                if ((curApertureState == ApertureState.Off &&
                        curInterpolation != Interpolation.PolygonStart) && (polygonPoints > 0))
                {
                    curNetEntry.Interpolation = Interpolation.PolygonEnd;

                    curNetEntry = CreateNetEntry(curLayer, curNetEntryState);
                    curNetEntry.Interpolation = Interpolation.PolygonStart;

                    polygonStartEntry.BoundingBox = boundingBox;
                    polygonStartEntry = curNetEntry;
                    polygonPoints = 0;

                    curNetEntry = CreateNetEntry(curLayer, curNetEntryState);
                    curNetEntry.StartX = prevX / scaleX;
                    curNetEntry.StartY = prevY / scaleY;
                    curNetEntry.StopX = currX / scaleX;
                    curNetEntry.StopY = currY / scaleY;

                    ResetBoundingBox();
                }
                else if (curInterpolation != Interpolation.PolygonStart)
                {
                    polygonPoints++;
                }
            }  /* if (state->in_parea_fill && state->parea_start_node) */

            curNetEntry.Interpolation = curInterpolation;

            /* 
             * Override circular interpolation if no center was given.
             * This should be a safe hack, since a good file should always 
             * include I or J.  And even if the radius is zero, the endpoint 
             * should be the same as the start point, creating no line 
             */
            if (((curInterpolation == Interpolation.CircularCW) ||
             (curInterpolation == Interpolation.CircularCCW)) &&
            ((deltaCpX == 0.0) && (deltaCpY == 0.0)))
            {
                curNetEntry.Interpolation = Interpolation.LinearX1;
            }

            /*
             * If we detected the end of Polygon Area Fill we go back to
             * the interpolation we had before that.
             * Also if we detected any of the quadrant flags, since some
             * gerbers don't reset the interpolation (EagleCad again).
             */
            if ((curInterpolation == Interpolation.PolygonStart) ||
            (curInterpolation == Interpolation.PolygonEnd))
            {
                curInterpolation = prevInterpolation;
            }

            /*
             * Save layer polarity and unit
             */
            curNetEntry.Layer = curLayer;

            deltaCpX = 0.0;
            deltaCpY = 0.0;
            curNetEntry.ApertureNo = curApertureNo;
            curNetEntry.ApertureState = curApertureState;

            /*
             * For next round we save the current position as
             * the previous position
             */
            prevX = currX;
            prevY = currY;

            /*
             * If we have an aperture defined at the moment we find 
             * min and max of image with compensation for mm.
             */
            if ((curNetEntry.ApertureNo == 0) && !inPolygonfill)
            {
                return;
            }

            /* only update the min/max values and aperture stats if we are drawing */
            if ((curNetEntry.ApertureState != ApertureState.Off) &&
                    (curNetEntry.Interpolation != Interpolation.PolygonStart))
            {
                double repeat_off_X = 0.0, repeat_off_Y = 0.0;

                /* Update stats with current aperture number if not in polygon */
                if (!inPolygonfill)
                {
                    DebugLog("     In parse_D_code, adding 1 to D_list ...\n");
                    if (gerberStat.IncrementDCodeItem(curNetEntry.ApertureNo) == false)
                    {
                        gerberStat.AddError(string.Format("Found undefined D code {0}", curNetEntry.ApertureNo));
                        gerberStat.DUnknown++;
                    }
                }

                /*
                 * If step_and_repeat (%SR%) is used, check min_x,max_y etc for
                 * the ends of the step_and_repeat lattice. This goes wrong in 
                 * the case of negative dist_X or dist_Y, in which case we 
                 * should compare against the startpoints of the lines, not 
                 * the stoppoints, but that seems an uncommon case (and the 
                 * error isn't very big any way).
                 */
                repeat_off_X = (curLayer.StepNRepeat.X - 1) * curLayer.StepNRepeat.DistX;
                repeat_off_Y = (curLayer.StepNRepeat.Y - 1) * curLayer.StepNRepeat.DistY;

                currentMatrix = new Matrix();
                /* offset image */
                currentMatrix.Translate((float)gerberData.Info.OffsetA, (float)gerberData.Info.OffsetB);
                /* do image rotation */
                currentMatrix.Rotate((float)gerberData.Info.ImageRotation);
                /* it's a new layer, so recalculate the new transformation matrix for it do any rotations */
                currentMatrix.Rotate((float)curLayer.Rotation);

                /* calculate current layer and state transformation matrices apply scale factor */
                currentMatrix.Scale((float)curNetEntryState.ScaleA, (float)curNetEntryState.ScaleB);
                /* apply offset */
                currentMatrix.Translate((float)curNetEntryState.OffsetA, (float)curNetEntryState.OffsetB);

                /* apply mirror */
                switch (curNetEntryState.MirrorState)
                {
                    case MirrorState.FlipA:
                        currentMatrix.Scale(-1, 1);
                        break;
                    case MirrorState.FlipB:
                        currentMatrix.Scale(1, -1);
                        break;
                    case MirrorState.FlipAB:
                        currentMatrix.Scale(-1, -1);
                        break;
                    default:
                        break;
                }
                /* finally, apply axis select */
                if (curNetEntryState.AxisSelect == AxisSelect.SwapAB)
                {
                    /* we do this by rotating 270 (counterclockwise, then 
                     *  mirroring the Y axis 
                     */
                    currentMatrix.Rotate((float)(3 * Math.PI / 2));
                    currentMatrix.Scale(1, -1);
                }
                /* if it's a macro, step through all the primitive components
                   and calculate the true bounding box */
                Aperture aperture = gerberData.GetAperture(curNetEntry.ApertureNo);
                if ((aperture != null) && (aperture.Type == ApertureType.Macro))
                {
                    List<SimplifiedApertureMacro> samList = aperture.SamList;

                    foreach (SimplifiedApertureMacro sam in samList)
                    {
                        double offsetx = 0, offsety = 0, widthx = 0, widthy = 0;
                        bool calculatedAlready = false;
                        double[] param = sam.Parameter;

                        if (sam.Type == ApertureType.MacroCircle)
                        {
                            offsetx = param[(int)MacroCircleIndex.CENTER_X];
                            offsety = param[(int)MacroCircleIndex.CENTER_Y];
                            widthx = widthy = param[(int)MacroCircleIndex.DIAMETER];
                        }
                        else if (sam.Type == ApertureType.MacroOutline)
                        {
                            int pointCounter, numberOfPoints;
                            numberOfPoints = (int)param[(int)MacroOutlineIndex.NUMBER_OF_POINTS];

                            for (pointCounter = 0; pointCounter <= numberOfPoints; pointCounter++)
                            {
                                UpdateBoundingBox(currentMatrix,
                                        curNetEntry.StopX + param[pointCounter * 2 + (int)MacroOutlineIndex.FIRST_X],
                                        curNetEntry.StopY + param[pointCounter * 2 + (int)MacroOutlineIndex.FIRST_Y],
                                        0, 0, 0, 0);
                            }
                            calculatedAlready = true;
                        }
                        else if (sam.Type == ApertureType.MacroPolygon)
                        {
                            offsetx = param[(int)MacroPolygonIndex.CENTER_X];
                            offsety = param[(int)MacroPolygonIndex.CENTER_Y];
                            widthx = widthy = param[(int)MacroPolygonIndex.DIAMETER];
                        }
                        else if (sam.Type == ApertureType.MacroMoire)
                        {
                            offsetx = param[(int)MacroMoireIndex.CENTER_X];
                            offsety = param[(int)MacroMoireIndex.CENTER_Y];
                            widthx = widthy = param[(int)MacroMoireIndex.OUTSIDE_DIAMETER];
                        }
                        else if (sam.Type == ApertureType.MacroThermal)
                        {
                            offsetx = param[(int)MacroThermalIndex.CENTER_X];
                            offsety = param[(int)MacroThermalIndex.CENTER_Y];
                            widthx = widthy = param[(int)MacroThermalIndex.OUTSIDE_DIAMETER];
                        }
                        else if (sam.Type == ApertureType.MacroLine20)
                        {
                            widthx = widthy = param[(int)MacroLine20Index.LINE_WIDTH];
                            UpdateBoundingBox(currentMatrix,
                                curNetEntry.StopX + param[(int)MacroLine20Index.START_X],
                                curNetEntry.StopY + param[(int)MacroLine20Index.START_Y],
                                widthx / 2, widthx / 2, widthy / 2, widthy / 2);
                            UpdateBoundingBox(currentMatrix,
                                curNetEntry.StopX + param[(int)MacroLine20Index.END_X],
                                curNetEntry.StopY + param[(int)MacroLine20Index.END_Y],
                                widthx / 2, widthx / 2, widthy / 2, widthy / 2);
                            calculatedAlready = true;
                        }
                        else if (sam.Type == ApertureType.MacroLine21)
                        {
                            double largestDimension = Math.Sqrt(param[(int)MacroLine21Index.WIDTH] / 2 *
                                             param[(int)MacroLine21Index.WIDTH] / 2 + param[(int)MacroLine21Index.HEIGHT / 2] *
                                             param[(int)MacroLine21Index.HEIGHT] / 2);

                            offsetx = param[(int)MacroLine21Index.CENTER_X];
                            offsety = param[(int)MacroLine21Index.CENTER_Y];
                            widthx = widthy = largestDimension;
                        }
                        else if (sam.Type == ApertureType.MacroLine22)
                        {
                            double largestDimension = Math.Sqrt(param[(int)MacroLine22Index.WIDTH] / 2 *
                                             param[(int)MacroLine22Index.WIDTH] / 2 + param[(int)MacroLine22Index.HEIGHT / 2] *
                                             param[(int)MacroLine22Index.HEIGHT] / 2);

                            offsetx = param[(int)MacroLine22Index.LOWER_LEFT_X] +
                                  param[(int)MacroLine22Index.WIDTH] / 2;
                            offsety = param[(int)MacroLine22Index.LOWER_LEFT_Y] +
                                  param[(int)MacroLine22Index.HEIGHT] / 2;
                            widthx = widthy = largestDimension;
                        }

                        if (!calculatedAlready)
                        {
                            UpdateBoundingBox(currentMatrix,
                                           curNetEntry.StopX + offsetx,
                                           curNetEntry.StopY + offsety,
                                           widthx / 2, widthx / 2, widthy / 2, widthy / 2);
                        }
                    }
                }
                else
                {
                    double apertureSizeX, apertureSizeY;

                    if (aperture != null)
                    {
                        apertureSizeX = aperture.Parameter[0];
                        if ((aperture.Type == ApertureType.Rectangle) || (aperture.Type == ApertureType.Oval))
                        {
                            apertureSizeY = aperture.Parameter[1];
                        }
                        else
                        {
                            apertureSizeY = apertureSizeX;
                        }
                    }
                    else
                    {
                        /* this is usually for polygon fills, where the aperture width
                           is "zero" */
                        apertureSizeX = apertureSizeY = 0;
                    }

                    /* if it's an arc path, use a special calc */
                    if ((curNetEntry.Interpolation == Interpolation.CircularCW) ||
                         (curNetEntry.Interpolation == Interpolation.CircularCCW))
                    {
                        CircleSegment circleSegment = curNetEntry.CircleSegment;
                        /* to calculate the arc bounding box, we chop it into 1 degree steps, calculate
                           the point at each step, and use it to figure out the bounding box */
                        double angleDiff = circleSegment.Angle2 - circleSegment.Angle1;
                        int i, steps = (int)Math.Abs(angleDiff);
                        for (i = 0; i <= steps; i++)
                        {
                            double tempX = circleSegment.CenterPtX + circleSegment.Width / 2.0 *
                                     Math.Cos((circleSegment.Angle1 +
                                     (angleDiff * i) / steps) * Math.PI / 180);
                            double tempY = circleSegment.CenterPtY + circleSegment.Width / 2.0 *
                                     Math.Sin((circleSegment.Angle1 +
                                     (angleDiff * i) / steps) * Math.PI / 180);
                            UpdateBoundingBox(currentMatrix,
                                       tempX, tempY,
                                       apertureSizeX / 2, apertureSizeX / 2,
                                       apertureSizeY / 2, apertureSizeY / 2);
                        }

                    }
                    else
                    {
                        /* check both the start and stop of the aperture points against
                           a running min/max counter */
                        /* Note: only check start coordinate if this isn't a flash, 
                           since the start point may be bogus if it is a flash */
                        if (curNetEntry.ApertureState != ApertureState.Flash)
                        {
                            UpdateBoundingBox(currentMatrix,
                                           curNetEntry.StartX, curNetEntry.StartY,
                                           apertureSizeX / 2, apertureSizeX / 2,
                                           apertureSizeY / 2, apertureSizeY / 2);
                        }

                        UpdateBoundingBox(currentMatrix,
                                       curNetEntry.StopX, curNetEntry.StopY,
                                       apertureSizeX / 2, apertureSizeX / 2,
                                       apertureSizeY / 2, apertureSizeY / 2);
                    }

                }
                /* update the info bounding box with this latest bounding box */
                UpdateBoundingBox(repeat_off_X, repeat_off_Y);

                /* optionally update the knockout measurement box */
                if (knockoutMeasure)
                {
                    if (boundingBox.Left < knockoutLimitXmin)
                    {
                        knockoutLimitXmin = boundingBox.Left;
                    }

                    if (boundingBox.Right + repeat_off_X > knockoutLimitXmax)
                    {
                        knockoutLimitXmax = boundingBox.Right + repeat_off_X;
                    }

                    if (boundingBox.Bottom < knockoutLimitYmin)
                    {
                        knockoutLimitYmin = boundingBox.Bottom;
                    }

                    if (boundingBox.Top + repeat_off_Y > knockoutLimitYmax)
                    {
                        knockoutLimitYmax = boundingBox.Top + repeat_off_Y;
                    }
                }
                /* if we're not in a polygon fill, then update the object bounding box */
                if (!inPolygonfill)
                {
                    curNetEntry.BoundingBox = boundingBox;
                    ResetBoundingBox();
                }
            }
        }
    }
}
