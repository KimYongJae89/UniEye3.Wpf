using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Gerber
{
    public class GerberRender
    {
        //gerbv_image_t* image,
        //                    gdouble pixelWidth,
        //                    gchar drawMode, gerbv_selection_info_t* selectionInfo,
        //                    gerbv_render_info_t* renderInfo, gboolean allowOptimization,
        //                     gerbv_user_transformation_t transform, gboolean pixelOutput


        //struct gerbv_net *net, *polygonStartNet=NULL;

        //double p1, p2, p3, p4, p5, dx, dy, lineWidth;
        //gerbv_netstate_t* oldState;
        //gerbv_layer_t* oldLayer;
        //int repeat_X = 1, repeat_Y = 1;
        //double repeat_dist_X = 0, repeat_dist_Y = 0;
        //int repeat_i, repeat_j;
        //oddWidth = FALSE;
        //double minX = 0, minY = 0, maxX = 0, maxY = 0;
        //double criticalRadius;
        //bool displayPixel = TRUE;

        private GerberData gerber;
        private Matrix transformMatrix;
        private bool invertPolarity = false;
        private Stack<Matrix> matrixStack = new Stack<Matrix>();
        private double x1, y1, x2, y2, cp_x = 0, cp_y = 0;
        private CombineMode curCombineMode;
        private CombineMode combineModeClear;
        private CombineMode combineModeDark;
        private FigureProperty figProp;

        private void ApplyNetState(NetEntryState netEntryState)
        {
            /* apply scale factor */
            transformMatrix.Scale((float)netEntryState.ScaleA, (float)netEntryState.ScaleB);
            /* apply offset */
            transformMatrix.Translate((float)netEntryState.OffsetA, (float)netEntryState.OffsetB);

            /* apply mirror */
            switch (netEntryState.MirrorState)
            {
                case MirrorState.FlipA:
                    transformMatrix.Scale(-1, 1);
                    break;
                case MirrorState.FlipB:
                    transformMatrix.Scale(1, -1);
                    break;
                case MirrorState.FlipAB:
                    transformMatrix.Scale(-1, -1);
                    break;
                default:
                    break;
            }

            /* finally, apply axis select */
            if (netEntryState.AxisSelect == AxisSelect.SwapAB)
            {
                /* we do this by rotating 270 (counterclockwise, then mirroring
                   the Y axis */
                transformMatrix.Rotate((float)(3 * Math.PI / 2));
                transformMatrix.Scale(1, -1);
            }
        }

        public FigureGroup BuildFigure(GerberData gerber)
        {
            this.gerber = gerber;

            var figureGroup = new FigureGroup();

            transformMatrix = new Matrix();

            figProp = new FigureProperty();
            figProp.Pen = new Pen(Color.Green);
            figProp.Brush = new SolidBrush(Color.Green);

            GerberInfo info = gerber.Info;
            /* do initial justify */
            transformMatrix.Translate((float)info.ImageJustifyOffsetActualA,
                    (float)info.ImageJustifyOffsetActualB);

            transformMatrix.Translate((float)info.OffsetA,
                    (float)info.OffsetB);
            transformMatrix.Rotate((float)info.ImageRotation);

            /* load in polarity operators depending on the image polarity */
            invertPolarity = false;
            if (info.Polarity == Polarity.Negative)
            {
                invertPolarity = !invertPolarity;
            }

            if (invertPolarity)
            {
                combineModeClear = CombineMode.Replace;
                combineModeDark = CombineMode.Exclude;
            }
            else
            {
                combineModeClear = CombineMode.Exclude;
                combineModeDark = CombineMode.Replace;
            }

            matrixStack.Push(transformMatrix.Clone());
            matrixStack.Push(transformMatrix.Clone());

            int repeat_X = 1, repeat_Y = 1;
            double repeat_dist_X = 0, repeat_dist_Y = 0;

            bool curInvertPolarity = invertPolarity;
            Layer curLayer = null;
            NetEntryState curNetEntryState = null;
            List<NetEntry> netEntryList = gerber.GetRenderableNetEntryList();
            foreach (NetEntry netEntry in netEntryList)
            {
                Trace.Write(netEntry.ApertureNo + " : ");

                /* check if this is a new layer */
                if (netEntry.Layer != curLayer)
                {
                    transformMatrix = matrixStack.Pop();
                    transformMatrix = matrixStack.Pop();

                    matrixStack.Push(transformMatrix.Clone());

                    curLayer = netEntry.Layer;

                    /* do any rotations */
                    transformMatrix.Rotate((float)curLayer.Rotation);
                    /* handle the layer polarity */
                    if ((curLayer.Polarity == Polarity.Clear) ^ invertPolarity)
                    {
                        combineModeClear = CombineMode.Replace;
                        combineModeDark = CombineMode.Exclude;
                    }
                    else
                    {
                        combineModeClear = CombineMode.Exclude;
                        combineModeDark = CombineMode.Replace;
                    }

                    /* check for changes to step and repeat */
                    repeat_X = curLayer.StepNRepeat.X;
                    repeat_Y = curLayer.StepNRepeat.Y;
                    repeat_dist_X = curLayer.StepNRepeat.DistX;
                    repeat_dist_Y = curLayer.StepNRepeat.DistY;

                    /* draw any knockout areas */
                    if (curLayer.Knockout != null)
                    {
                        Knockout knockout = curLayer.Knockout;

                        double border = knockout.Border;
                        var rectangle = new RectangleF(
                            (float)(knockout.LowerLeftX - border), (float)(knockout.LowerLeftY - border),
                            (float)(knockout.Width + (border * 2)), (float)(knockout.Height + (border * 2)));

                        var rectangleFigure = new RectangleFigure(rectangle, "Figure");

                        if (knockout.Polarity == Polarity.Clear)
                        {
                            rectangleFigure.CombineMode = combineModeClear;
                        }
                        else
                        {
                            rectangleFigure.CombineMode = combineModeDark;
                        }
                    }
                    /* finally, reapply old netstate transformation */
                    matrixStack.Push(transformMatrix.Clone());

                    ApplyNetState(netEntry.State);
                }

                /* check if this is a new netstate */
                if (netEntry.State != curNetEntryState)
                {
                    /* pop the transformation matrix back to the "pre-state" state and resave it */
                    transformMatrix = matrixStack.Pop();
                    matrixStack.Push(transformMatrix.Clone());

                    /* it's a new state, so recalculate the new transformation matrix for it */
                    ApplyNetState(netEntry.State);
                    curNetEntryState = netEntry.State;
                }

                for (int repeat_i = 0; repeat_i < repeat_X; repeat_i++)
                {
                    for (int repeat_j = 0; repeat_j < repeat_Y; repeat_j++)
                    {
                        double sr_x = repeat_i * repeat_dist_X;
                        double sr_y = repeat_j * repeat_dist_Y;

                        x1 = netEntry.StartX + sr_x;
                        y1 = netEntry.StartY + sr_y;
                        x2 = netEntry.StopX + sr_x;
                        y2 = netEntry.StopY + sr_y;

                        /* translate circular x,y data as well */
                        CircleSegment circleSegment = netEntry.CircleSegment;
                        if (circleSegment != null)
                        {
                            cp_x = circleSegment.CenterPtX + sr_x;
                            cp_y = circleSegment.CenterPtY + sr_y;
                        }

                        /*
                        * Polygon Area Fill routines
                        */
                        switch (netEntry.Interpolation)
                        {
                            case Interpolation.PolygonStart:
                                BuildPolygonFigure(netEntry, sr_x, sr_y);
                                continue;
                            case Interpolation.Deleted:
                                continue;
                            default:
                                break;
                        }

                        Aperture aperture = gerber.GetAperture(netEntry.ApertureNo);
                        /*
                        * If aperture state is off we allow use of undefined apertures.
                        * This happens when gerber files starts, but hasn't decided on 
                        * which aperture to use.
                        */
                        if (aperture == null)
                        {
                            /* Commenting this out since it gets emitted every time you click on the screen 
                            if (net->aperture_state != GERBV_APERTURE_STATE_OFF)
                            GERB_MESSAGE("Aperture D%d is not defined\n", net->aperture);
                            */
                            continue;
                        }

                        switch (netEntry.ApertureState)
                        {
                            case ApertureState.On:
                                DrawLineAperture(netEntry, aperture, figureGroup);
                                break;
                            case ApertureState.Off:
                                break;
                            case ApertureState.Flash:
                                DrawFlashAperture(netEntry, aperture, figureGroup);
                                break;
                            default:
                                LogHelper.Debug(LoggerType.Operation, "Unknown aperture state");
                                return null;
                        }
                    }
                }
            }

            return figureGroup;
        }

        private void BuildPolygonFigure(NetEntry startNetEntry, double sr_x, double sr_y)
        {
            double cp_x = 0, cp_y = 0;
            bool firstPoint = true;

            var polygonFigure = new PolygonFigure();
            bool polygonFound = false;
            foreach (NetEntry netEntry in gerber.NetEntryList)
            {
                if (netEntry == startNetEntry)
                {
                    polygonFound = true;
                }

                if (polygonFound)
                {
                    double x2 = netEntry.StopX + sr_x;
                    double y2 = netEntry.StopY + sr_y;

                    /* translate circular x,y data as well */
                    CircleSegment circleSeg = netEntry.CircleSegment;
                    if (circleSeg != null)
                    {
                        cp_x = circleSeg.CenterPtX + sr_x;
                        cp_y = circleSeg.CenterPtY + sr_y;
                    }

                    if (firstPoint)
                    {

                        polygonFigure.AddLIne(TransformPoint(x2, y2));
                        firstPoint = false;
                        continue;
                    }

                    switch (netEntry.Interpolation)
                    {
                        case Interpolation.X10:
                        case Interpolation.LinearX01:
                        case Interpolation.LinearX001:
                        case Interpolation.LinearX1:

                            polygonFigure.AddLIne(TransformPoint(x2, y2));
                            break;
                        case Interpolation.CircularCW:
                        case Interpolation.CircularCCW:

                            double sweepAngle;
                            if (circleSeg.Angle2 > circleSeg.Angle1)
                            {
                                sweepAngle = circleSeg.Angle2 - circleSeg.Angle1;
                            }
                            else
                            {
                                sweepAngle = 360 - (circleSeg.Angle1 - circleSeg.Angle2);
                            }

                            float radius = TransformPoint((circleSeg.Width / 2), 0).X;
                            polygonFigure.AddArc(TransformPoint(cp_x, cp_y), radius, (float)circleSeg.Angle1, (float)sweepAngle);
                            break;
                        case Interpolation.PolygonEnd:
                            return;
                        default:
                            break;
                    }
                }
            }
        }

        private void DrawLineAperture(NetEntry netEntry, Aperture aperture, FigureGroup figureGroup)
        {
            /* if the aperture width is truly 0, then render as a 1 pixel width
                line.  0 diameter apertures are used by some programs to draw labels,
                etc, and they are rendered by other programs as 1 pixel wide */
            /* NOTE: also, make sure all lines are at least 1 pixel wide, so they
                always show up at low zoom levels */

            double criticalRadius = aperture.Parameter[0] / 2.0;
            double lineWidth = criticalRadius * 2.0;

            PointF startPt = TransformPoint(x1, y1);
            PointF endPt = TransformPoint(x2, y2);

            FigureProperty newFigureProp = figProp.Clone();
            newFigureProp.Pen.Width = (float)lineWidth;

            switch (netEntry.Interpolation)
            {
                case Interpolation.X10:
                case Interpolation.LinearX01:
                case Interpolation.LinearX001:
                case Interpolation.LinearX1:

                    newFigureProp.Pen.SetLineCap(LineCap.Round, LineCap.Round, DashCap.Flat);

                    switch (aperture.Type)
                    {
                        case ApertureType.Clrcle:
                            figureGroup.AddFigure(new LineFigure(startPt, endPt, newFigureProp));
                            break;
                        case ApertureType.Rectangle:
                            double dx = (aperture.Parameter[0] / 2);
                            double dy = (aperture.Parameter[1] / 2);
                            if (x1 > x2)
                            {
                                dx = -dx;
                            }

                            if (y1 > y2)
                            {
                                dy = -dy;
                            }

                            var polygonFigure = new PolygonFigure(newFigureProp);
                            polygonFigure.AddLIne(TransformPoint(x1 - dx, y1 - dy));
                            polygonFigure.AddLIne(TransformPoint(x1 - dx, y1 + dy));
                            polygonFigure.AddLIne(TransformPoint(x2 - dx, y2 + dy));
                            polygonFigure.AddLIne(TransformPoint(x2 + dx, y2 + dy));
                            polygonFigure.AddLIne(TransformPoint(x2 + dx, y2 - dy));
                            polygonFigure.AddLIne(TransformPoint(x1 + dx, y2 - dy));

                            figureGroup.AddFigure(polygonFigure);
                            break;
                        /* for now, just render ovals or polygons like a circle */
                        case ApertureType.Oval:
                        case ApertureType.Polygon:
                            figureGroup.AddFigure(new LineFigure(startPt, endPt, "Figure"));
                            break;
                        /* macros can only be flashed, so ignore any that might be here */
                        default:
                            break;
                    }
                    break;
                case Interpolation.CircularCW:
                case Interpolation.CircularCCW:
                    /* cairo doesn't have a function to draw oval arcs, so we must
                        * draw an arc and stretch it by scaling different x and y values
                        */
                    if (aperture.Type == ApertureType.Rectangle)
                    {
                        newFigureProp.Pen.SetLineCap(LineCap.Square, LineCap.Square, DashCap.Flat);
                    }
                    else
                    {
                        newFigureProp.Pen.SetLineCap(LineCap.Round, LineCap.Round, DashCap.Flat);
                    }

                    matrixStack.Push(transformMatrix.Clone());

                    transformMatrix.Translate((float)cp_x, (float)cp_y);

                    CircleSegment circleSeg = netEntry.CircleSegment;
                    transformMatrix.Scale((float)circleSeg.Width, (float)circleSeg.Height);

                    double sweepAngle;
                    if (circleSeg.Angle2 > circleSeg.Angle1)
                    {
                        sweepAngle = circleSeg.Angle2 - circleSeg.Angle1;
                    }
                    else
                    {
                        sweepAngle = 360 - (circleSeg.Angle1 - circleSeg.Angle2);
                    }

                    RectangleF rectangleF = DrawingHelper.FromCenterSize(TransformPoint(cp_x, cp_y), new SizeF(TransformPoint(circleSeg.Width, circleSeg.Width)));
                    var figure = new EllipseFigure(rectangleF, newFigureProp);
                    figure.SetArcAngle((float)circleSeg.Angle1, (float)sweepAngle);

                    figureGroup.AddFigure(figure);

                    transformMatrix = matrixStack.Pop();
                    break;
                default:
                    break;
            }
        }

        private void DrawFlashAperture(NetEntry netEntry, Aperture aperture, FigureGroup figureGroup)
        {
            double p1 = aperture.Parameter[0];
            double p2 = aperture.Parameter[1];
            double p3 = aperture.Parameter[2];
            double p4 = aperture.Parameter[3];
            double p5 = aperture.Parameter[4];

            matrixStack.Push(transformMatrix.Clone());

            transformMatrix.Translate((float)x2, (float)y2);

            FigureProperty newFigureProp = figProp.Clone();

            RectangleF circleRect = DrawingHelper.FromCenterSize(TransformPoint(), new SizeF(TransformPoint(p1, p1)));
            RectangleF rectangle = DrawingHelper.FromCenterSize(TransformPoint(), new SizeF(TransformPoint(p1, p2)));

            switch (aperture.Type)
            {
                case ApertureType.Clrcle:
                    figureGroup.AddFigure(new EllipseFigure(circleRect, newFigureProp));
                    AddApertureHole(p2, p3, figureGroup);
                    break;
                case ApertureType.Rectangle:
                    figureGroup.AddFigure(new RectangleFigure(rectangle, newFigureProp));
                    AddApertureHole(p3, p4, figureGroup);
                    break;
                case ApertureType.Oval:
                    figureGroup.AddFigure(new OblongFigure(rectangle, newFigureProp));
                    AddApertureHole(p3, p4, figureGroup);
                    break;
                case ApertureType.Polygon:
                    AddPolygon(p1, p2, p3, figureGroup, figProp);
                    AddApertureHole(p4, p5, figureGroup);
                    break;
                case ApertureType.Macro:
                    AddApertureMacro(aperture, figureGroup);
                    break;
                default:
                    LogHelper.Debug(LoggerType.Operation, "Unknown aperture type");
                    break;
            }

            transformMatrix = matrixStack.Pop();
        }

        private void AddApertureHole(double dimX, double dimY, FigureGroup figureGroup)
        {
            if (dimX > 0)
            {
                RectangleF rectangle;
                if (dimY > 0)
                {
                    rectangle = DrawingHelper.FromCenterSize(TransformPoint(), new SizeF(TransformPoint(dimX, dimY)));
                }
                else
                {
                    rectangle = DrawingHelper.FromCenterSize(TransformPoint(), new SizeF(TransformPoint(dimX, dimX)));
                }

                figureGroup.AddFigure(new RectangleFigure(rectangle, figProp.Clone()));
            }
        }

        private void AddPolygon(double outsideDiameter, double numberOfSides, double degreesOfRotation, FigureGroup figureGourp, FigureProperty figureProperty)
        {
            int i, numberOfSidesInteger = (int)numberOfSides;

            var polygonFigure = new PolygonFigure(figureProperty);
            polygonFigure.Angle = (float)degreesOfRotation;

            polygonFigure.AddLIne(new PointF((float)(outsideDiameter / 2.0), 0));
            for (i = 1; i <= numberOfSidesInteger; i++)
            {
                double angle = (double)i / numberOfSidesInteger * Math.PI * 2.0;

                double ptX = Math.Cos(angle) * outsideDiameter / 2.0;
                double ptY = Math.Sin(angle) * outsideDiameter / 2.0;
                polygonFigure.AddLIne(TransformPoint(ptX, ptY));
            }

            figureGourp.AddFigure(polygonFigure);
        }

        private void UpdateCombineMode(double exposureSetting)
        {
            if (exposureSetting == 0.0)
            {
                curCombineMode = combineModeClear;
            }
            else if (exposureSetting == 1.0)
            {
                curCombineMode = combineModeDark;
            }
            else if (exposureSetting == 2.0)
            {
                /* reverse current exposure setting */
                if (curCombineMode == combineModeClear)
                {
                    curCombineMode = combineModeDark;
                }
                else
                {
                    curCombineMode = combineModeClear;
                }
            }
        }

        private PointF TransformPoint()
        {
            return TransformPoint(new PointF(0, 0));
        }

        private PointF TransformPoint(PointF srcPoint)
        {
            var ptArr = new PointF[] { srcPoint };
            transformMatrix.TransformPoints(ptArr);

            return ptArr[0];
        }

        private PointF TransformPoint(double x, double y)
        {
            var ptArr = new PointF[] { new PointF((float)x, (float)y) };
            transformMatrix.TransformPoints(ptArr);

            Trace.WriteLine(ptArr[0]);
            return ptArr[0];
        }

        private int AddApertureMacro(Aperture aperture, FigureGroup figureGroup)
        {
            int handled = 1;
            List<SimplifiedApertureMacro> samList = aperture.SamList;
            bool usesClearPrimative = Convert.ToBoolean(aperture.Parameter[0]);

            LogHelper.Debug(LoggerType.Operation, "Drawing simplified aperture macros");

            foreach (SimplifiedApertureMacro sam in samList)
            {
                /* 
                    * This handles the exposure thing in the aperture macro
                    * The exposure is always the first element on stack independent
                    * of aperture macro.
                    */
                matrixStack.Push(transformMatrix.Clone());
                CombineMode preCombineMode = curCombineMode;

                FigureProperty newFigProp = figProp.Clone();

                double[] param = sam.Parameter;

                if (sam.Type == ApertureType.MacroCircle)
                {
                    UpdateCombineMode(param[(int)MacroCircleIndex.EXPOSURE]);

                    transformMatrix.Translate((float)param[(int)MacroCircleIndex.CENTER_X], (float)param[(int)MacroCircleIndex.CENTER_Y]);

                    float diameter = (float)param[(int)MacroCircleIndex.DIAMETER];
                    RectangleF rectangle = DrawingHelper.FromCenterSize(TransformPoint(), new SizeF(TransformPoint(diameter, diameter)));
                    var figure = new EllipseFigure(rectangle, newFigProp);
                    figure.CombineMode = curCombineMode;

                    figureGroup.AddFigure(figure);
                }
                else if (sam.Type == ApertureType.MacroOutline)
                {
                    int pointCounter, numberOfPoints;
                    numberOfPoints = (int)param[(int)MacroOutlineIndex.NUMBER_OF_POINTS] + 1;

                    UpdateCombineMode(param[(int)MacroOutlineIndex.EXPOSURE]);

                    var figure = new PolygonFigure(newFigProp);
                    figure.CombineMode = curCombineMode;
                    figure.Angle = (float)param[(int)((numberOfPoints - 1) * 2 + MacroOutlineIndex.ROTATION)];
                    figure.AddLIne(TransformPoint(param[(int)MacroOutlineIndex.FIRST_X], param[(int)MacroOutlineIndex.FIRST_Y]));

                    for (pointCounter = 0; pointCounter < numberOfPoints; pointCounter++)
                    {
                        figure.AddLIne(TransformPoint(param[(int)(pointCounter * 2 + MacroOutlineIndex.FIRST_X)],
                                                        param[(int)(pointCounter * 2 + MacroOutlineIndex.FIRST_Y)]));
                    }

                    figureGroup.AddFigure(figure);
                }
                else if (sam.Type == ApertureType.MacroPolygon)
                {
                    UpdateCombineMode(param[(int)MacroPolygonIndex.EXPOSURE]);

                    transformMatrix.Translate((float)param[(int)MacroPolygonIndex.CENTER_X], (float)param[(int)MacroPolygonIndex.CENTER_Y]);

                    AddPolygon(param[(int)MacroPolygonIndex.DIAMETER],
                                param[(int)MacroPolygonIndex.NUMBER_OF_POINTS], param[(int)MacroPolygonIndex.ROTATION], figureGroup, newFigProp);
                }
                else if (sam.Type == ApertureType.MacroMoire)
                {
                    double diameter, gap;
                    int circleIndex;

                    transformMatrix.Translate((float)param[(int)MacroMoireIndex.CENTER_X], (float)param[(int)MacroMoireIndex.CENTER_Y]);
                    transformMatrix.Rotate((float)param[(int)MacroMoireIndex.ROTATION]);

                    diameter = param[(int)MacroMoireIndex.OUTSIDE_DIAMETER] - param[(int)MacroMoireIndex.CIRCLE_THICKNESS];
                    gap = param[(int)MacroMoireIndex.GAP_WIDTH] + param[(int)MacroMoireIndex.CIRCLE_THICKNESS];

                    newFigProp.Pen.Width = (float)param[(int)MacroMoireIndex.CIRCLE_THICKNESS];

                    RectangleF rectangle;
                    for (circleIndex = 0; circleIndex < (int)param[(int)MacroMoireIndex.NUMBER_OF_CIRCLES]; circleIndex++)
                    {
                        float curDia = (float)(diameter - gap * circleIndex);

                        rectangle = new RectangleF(TransformPoint(), new SizeF(TransformPoint(curDia, curDia)));
                        figureGroup.AddFigure(new EllipseFigure(rectangle, newFigProp));
                    }

                    newFigProp = newFigProp.Clone();

                    float crosshairRadius = (float)(param[(int)MacroMoireIndex.CROSSHAIR_LENGTH] / 2.0);

                    newFigProp.Pen.Width = (float)param[(int)MacroMoireIndex.CROSSHAIR_THICKNESS];

                    rectangle = DrawingHelper.FromCenterSize(TransformPoint(), new SizeF(TransformPoint(crosshairRadius * 2, crosshairRadius * 2)));

                    figureGroup.AddFigure(new CrossFigure(rectangle, newFigProp));
                }
                else if (sam.Type == ApertureType.MacroThermal)
                {
                    // [Curtis]향후 필요시 구현 예정. 아래 참조 코드 지우지 말 것... 
                    //transformMatrix.Translate((float)param[(int)MacroThermalIndex.CENTER_X], (float)param[(int)MacroThermalIndex.CENTER_Y]);
                    //transformMatrix.Rotate((float)param[(int)MacroThermalIndex.ROTATION]);

                    //double startAngle1 = Math.Atan(param[(int)MacroThermalIndex.CROSSHAIR_THICKNESS] / param[(int)MacroThermalIndex.INSIDE_DIAMETER]);
                    //double endAngle1 = Math.PI / 2 - startAngle1;
                    //double endAngle2 = Math.Atan(param[(int)MacroThermalIndex.CROSSHAIR_THICKNESS] / param[(int)MacroThermalIndex.OUTSIDE_DIAMETER]);
                    //double startAngle2 = Math.PI / 2 - endAngle2;

                    //for (int i = 0; i < 4; i++)
                    //{
                    //    cairo_arc(cairoTarget, 0, 0, param[(int)MacroThermalIndex.INSIDE_DIAMETER] / 2.0, startAngle1, endAngle1);

                    //    cairo_rel_line_to(cairoTarget, 0, param[(int)MacroThermalIndex.CROSSHAIR_THICKNESS]);

                    //    cairo_arc_negative(cairoTarget, 0, 0, param[(int)MacroThermalIndex.OUTSIDE_DIAMETER] / 2.0,
                    //    startAngle2, endAngle2);

                    //    cairo_rel_line_to(cairoTarget, -param[(int)MacroThermalIndex.CROSSHAIR_THICKNESS], 0);

                    //    transformMatrix.Rotate((float)90);
                    //}
                }
                else if (sam.Type == ApertureType.MacroLine20)
                {
                    UpdateCombineMode(param[(int)MacroLine20Index.EXPOSURE]);

                    double cParameter = param[(int)MacroLine20Index.LINE_WIDTH];

                    newFigProp.Pen.Width = (float)cParameter;
                    newFigProp.Pen.SetLineCap(LineCap.Flat, LineCap.Flat, DashCap.Flat);

                    transformMatrix.Rotate((float)(param[(int)MacroLine20Index.ROTATION]));

                    PointF startPt = TransformPoint(param[(int)MacroLine20Index.START_X], param[(int)MacroLine20Index.START_Y]);
                    PointF endPt = TransformPoint(param[(int)MacroLine20Index.END_X], param[(int)MacroLine20Index.END_X]);

                    figureGroup.AddFigure(new LineFigure(startPt, endPt, newFigProp));
                }
                else if (sam.Type == ApertureType.MacroLine21)
                {
                    UpdateCombineMode(param[(int)MacroLine21Index.EXPOSURE]);

                    transformMatrix.Translate((float)param[(int)MacroLine21Index.CENTER_X], (float)param[(int)MacroLine21Index.CENTER_Y]);
                    transformMatrix.Rotate((float)param[(int)MacroLine21Index.ROTATION]);

                    RectangleF rectangle = DrawingHelper.FromCenterSize(TransformPoint(),
                        new SizeF(TransformPoint(param[(int)MacroLine21Index.WIDTH], param[(int)MacroLine21Index.WIDTH])));
                    figureGroup.AddFigure(new RectangleFigure(rectangle, newFigProp));
                }
                else if (sam.Type == ApertureType.MacroLine22)
                {
                    UpdateCombineMode(param[(int)MacroLine22Index.EXPOSURE]);

                    transformMatrix.Translate((float)param[(int)MacroLine22Index.LOWER_LEFT_X], (float)param[(int)MacroLine22Index.LOWER_LEFT_Y]);
                    transformMatrix.Rotate((float)param[(int)MacroLine22Index.ROTATION]);

                    var rectangle = new RectangleF(TransformPoint(),
                        new SizeF(TransformPoint(param[(int)MacroLine22Index.WIDTH], param[(int)MacroLine22Index.HEIGHT])));
                    figureGroup.AddFigure(new RectangleFigure(rectangle, newFigProp));
                }
                else
                {
                    handled = 0;
                }

                curCombineMode = preCombineMode;

                transformMatrix = matrixStack.Pop();
            }

            return handled;
        } /* gerbv_draw_amacro */
    }
}
