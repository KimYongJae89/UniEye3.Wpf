using DynMvp.Base;
using DynMvp.Data;
using DynMvp.TSP;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml;

namespace DynMvp.GerberCvt
{
    public enum Coornidate
    {
        LL, LR, UL, UR
    }

    public enum Unit
    {
        mm, inch, cm, mil, micron, tn, uinch
    }

    public enum MirrorType
    {
        NoMirror, MirrorX, MirroY
    }

    public class ModuleGroup
    {
        public int GroupNo { get; set; }
        public RectangleF Region { get; set; }
        public List<Module> ModuleList { get; } = new List<Module>();

        public Module GetModule(int moduleNo)
        {
            return ModuleList.Find(x => x.No == moduleNo);
        }

        public void AddModule(Module module)
        {
            ModuleList.Add(module);
        }

        internal void Offset(float offsetX, float offsetY)
        {
            foreach (Module module in ModuleList)
            {
                module.Offset(offsetX, offsetY);
            }
        }

        internal void FlipX(float centerY)
        {
            foreach (Module module in ModuleList)
            {
                module.FlipX(centerY);
            }
        }

        internal void FlipY(float centerX)
        {
            foreach (Module module in ModuleList)
            {
                module.FlipY(centerX);
            }
        }
    }

    public class Module
    {
        public int No { get; }
        public string Name { get; set; }
        public PointF Pos { get; set; }
        public float Angle { get; set; }
        public bool Skip { get; set; }

        public Module(int no, float posX, float posY, float angle)
        {
            No = no;
            Pos = new PointF(posX, posY);
            Angle = angle;
        }

        public void Offset(float offsetX, float offsetY)
        {
            var offset = new SizeF(offsetX, offsetY);
            Pos = PointF.Add(Pos, offset);
        }

        internal void FlipX(float centerY)
        {
            throw new NotImplementedException();
        }

        internal void FlipY(float centerX)
        {
            throw new NotImplementedException();
        }
    }

    public enum FiducialType { Global, Module, Local }
    public enum FigureShape { Rectangle, Circle, Undifined, Oblong, Sloped }

    public class Fiducial
    {
        public int No { get; }
        public string PartName { get; }
        public FiducialType Type { get; }
        public FigureShape Shape { get; }
        public PointF Pos { get; private set; }
        public SizeF Size { get; set; }
        public SizeF Offset { get; }
        public string RefCode { get; }
        public int ModuleNo { get; }
        public float Angle { get; }

        public Fiducial(int no, FiducialType fiducialType, FigureShape fiducialShape, float posX, float posY, float width, float height, float offsetX, float offsetY, string refCode, int moduleNo)
        {
            No = no;
            Type = fiducialType;
            Shape = fiducialShape;
            Pos = new PointF(posX, posY);
            Size = new SizeF(width, height);
            Offset = new SizeF(offsetX, offsetY);
            RefCode = refCode;
            ModuleNo = moduleNo;
        }

        public Fiducial(int no, string partName, FiducialType fiducialType, float posX, float posY, float angle, int moduleNo)
        {
            No = no;
            PartName = partName;
            Type = fiducialType;
            Pos = new PointF(posX, posY);
            Angle = angle;
            ModuleNo = moduleNo;
        }

        public Figure CreateFigure()
        {
            RectangleF figureRect = DrawingHelper.FromCenterSize(new PointF(0, 0), Size);

            Figure figure = null;
            switch (Shape)
            {
                case FigureShape.Rectangle:
                case FigureShape.Undifined:
                case FigureShape.Oblong:
                case FigureShape.Sloped:
                    figure = new RectangleFigure(figureRect, new Pen(Color.Yellow, 1));
                    break;
                case FigureShape.Circle:
                    figure = new EllipseFigure(figureRect, new Pen(Color.Yellow, 1));
                    break;
            }

            figure.Offset(Pos.X, Pos.Y);
            return figure;
        }

        public void OffsetPos(float offsetX, float offsetY)
        {
            var offset = new SizeF(offsetX, offsetY);
            Pos = PointF.Add(Pos, offset);
        }

        internal void FlipX(float centerY)
        {
            throw new NotImplementedException();
        }

        internal void FlipY(float centerX)
        {
            throw new NotImplementedException();
        }
    }

    public class BadMark
    {
        public int No { get; }
        public FigureShape Shape { get; }
        public PointF Pos { get; private set; }
        public SizeF Size { get; set; }
        public int ModuleNo { get; }

        public BadMark(int no, FigureShape fiducialShape, float posX, float posY, float width, float height, int moduleNo)
        {
            No = no;
            Shape = fiducialShape;
            Pos = new PointF(posX, posY);
            Size = new SizeF(width, height);
            ModuleNo = moduleNo;
        }

        public Figure CreateFigure()
        {
            RectangleF figureRect = DrawingHelper.FromCenterSize(new PointF(0, 0), Size);

            Figure figure = null;
            switch (Shape)
            {
                case FigureShape.Rectangle:
                case FigureShape.Undifined:
                case FigureShape.Oblong:
                case FigureShape.Sloped:
                    figure = new RectangleFigure(figureRect, new Pen(Color.Yellow, 1));
                    break;
                case FigureShape.Circle:
                    figure = new EllipseFigure(figureRect, new Pen(Color.Yellow, 1));
                    break;
            }

            figure.Offset(Pos.X, Pos.Y);
            return figure;
        }

        public void OffsetPos(float offsetX, float offsetY)
        {
            var offset = new SizeF(offsetX, offsetY);
            Pos = PointF.Add(Pos, offset);
        }

        internal void FlipX(float centerY)
        {
            throw new NotImplementedException();
        }

        internal void FlipY(float centerX)
        {
            throw new NotImplementedException();
        }
    }

    public class Pattern
    {
        public int No { get; }
        public FigureShape Shape { get; }
        public SizeF Size { get; }
        public PointF Centroid { get; }
        public float Area { get; }
        public float Angle { get; }
        public float Boundary { get; set; }

        public Pattern(int patternNo, FigureShape patternShape, float width, float height, float centroidX, float centroidY, float area, float angle)
        {
            No = patternNo;
            Shape = patternShape;
            Size = new SizeF(width, height);
            Centroid = new PointF(centroidX, centroidY);
            Area = area;
            Angle = angle;
            Boundary = CalculateBoundary();
        }

        private float CalculateBoundary()
        {
            float calculationBoundary = 0.0f;

            switch (Shape)
            {
                case FigureShape.Rectangle:
                    calculationBoundary = Size.Width * 2 + Size.Height * 2;
                    break;
                case FigureShape.Circle:
                    calculationBoundary = Convert.ToSingle(Math.PI) * Size.Width;
                    break;
                case FigureShape.Oblong:
                    {
                        float r = Math.Min(Size.Width, Size.Height);
                        float b1 = Convert.ToSingle(Math.PI) * r;
                        float w = Math.Max(Size.Width, Size.Height) - r;
                        float b2 = w * 2;
                        calculationBoundary = b1 + b2;
                    }
                    break;
                case FigureShape.Sloped:
                    calculationBoundary = Size.Width * 2 + Size.Height * 2;
                    break;
            }

            return calculationBoundary;
        }

        public RotatedRect GetRectangle()
        {
            return new RotatedRect(DrawingHelper.FromCenterSize(new PointF(0, 0), Size), Angle);
        }

        public Figure CreateFigure(bool good = true)
        {
            RectangleF figureRect = DrawingHelper.FromCenterSize(new PointF(0, 0), Size);
            var figureRotatedRect = new RotatedRect(figureRect, Angle);

            Color color = Color.Yellow;
            int width = 1;
            if (good == false)
            {
                color = Color.Red;
                width = 2;
            }

            Figure figure = null;
            switch (Shape)
            {
                case FigureShape.Rectangle:
                    figure = new RectangleFigure(figureRotatedRect, new Pen(color, width));
                    break;
                case FigureShape.Circle:
                    figure = new EllipseFigure(figureRect, new Pen(color, width));
                    break;
                case FigureShape.Oblong:
                    figure = new OblongFigure(figureRotatedRect, new Pen(color, width));
                    break;
                case FigureShape.Sloped:
                    figure = new RectangleFigure(figureRotatedRect, new Pen(color, width));
                    break;
            }
            return figure;
        }


        public Image2D CreateImage(float pixSize, float rotateDeg)
        {
            return CreateImage(pixSize, pixSize, rotateDeg);
        }

        public Image2D CreateImage(float pixSizeW, float pixSizeH, float rotateDeg)
        {
            Figure figure = CreateFigure();
            var figureRect = figure.GetRectangle().ToRectangleF();
            var imageRect = new RectangleF(0, 0, figureRect.Width / pixSizeW, figureRect.Height / pixSizeH);

            var transformer = new CoordTransformer();
            transformer.SetSrcRect(figureRect);
            transformer.SetDisplayRect(imageRect);

            var img = new Bitmap((int)imageRect.Width, (int)imageRect.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            var g = Graphics.FromImage(img);

            figure.TempBrush = new SolidBrush(Color.White);
            figure.Draw(g, transformer, false);
            figure.ResetTempProperty();

            Bitmap grayImg = ImageHelper.ConvertGrayImage(img);
            var img2D = Image2D.ToImage2D(grayImg);

            grayImg.Dispose();
            img.Dispose();

            return img2D;
        }

        public bool InspectAreaRatio(int maskThicknessUm)
        {
            bool result = true;
            if (Area / (Boundary * maskThicknessUm) < 0.66)
            {
                result = false;
            }

            return result;
        }
    }

    public class Pad
    {
        public int No { get; }
        public int PatternNo { get; }
        public PointF Pos { get; private set; }
        public PointF Center { get; }
        public RotatedRect Rectangle { get; private set; }
        public string RefCode { get; }
        public int ModuleNo { get; }
        public int PinNo { get; }
        public int FovNo { get; set; }
        public float Angle { get; private set; }

        public Pad(int padNo, int patternNo, float posX, float posY, float centerX, float centerY, RotatedRect rectangle, string refCode, int pinNo, int moduleNo, int fovNo)
        {
            No = padNo;
            PatternNo = patternNo;
            Pos = new PointF(posX, posY);
            Center = new PointF(centerX, centerY);
            Rectangle = rectangle;
            RefCode = refCode;
            ModuleNo = moduleNo;
            FovNo = fovNo;
            PinNo = pinNo;
        }

        public RectangleF GetBoundRect()
        {
            return Rectangle.GetBoundRect();
        }

        public void Offset(float offsetX, float offsetY)
        {
            var offset = new SizeF(offsetX, offsetY);
            Pos = PointF.Add(Pos, offset);
            Rectangle.Offset(offset);
        }

        public void Rotate(float angle)
        {
            float deltaAngle = (angle - Angle);
            Angle += deltaAngle;
            Angle %= 360;

            Rectangle = new RotatedRect(Rectangle.ToRectangleF(), (Rectangle.Angle + deltaAngle) % 360);
        }

        internal void FlipX(float centerY)
        {
            throw new NotImplementedException();
        }

        internal void FlipY(float centerX)
        {
            throw new NotImplementedException();
        }
    }

    public class Component
    {
        public int No { get; }
        public PointF Pos { get; private set; }
        public float OffsetX { get; set; }
        public float OffsetY { get; set; }
        public string RefCode { get; }
        public string PartCode { get; }
        public int ModuleNo { get; }
        public float Angle { get; private set; }
        public List<Pad> PadList { get; set; } = new List<Pad>();

        public void AddPad(Pad pad)
        {
            PadList.Add(pad);
        }

        public RectangleF GetUnionRectF()
        {
            var unionRect = new RectangleF();
            foreach (Pad pad in PadList)
            {
                if (unionRect.IsEmpty)
                {
                    unionRect = pad.GetBoundRect();
                }
                else
                {
                    unionRect = RectangleF.Union(unionRect, pad.GetBoundRect());
                }
            }

            return unionRect;
        }

        public Component(int componentNo, string refCode, string partCode, float posX, float posY, float angle, int moduleNo)
        {
            No = componentNo;
            RefCode = refCode;
            PartCode = partCode;
            Pos = new PointF(posX, posY);
            Angle = angle;
            ModuleNo = moduleNo;
        }

        public void Offset(float offsetX, float offsetY)
        {
            var offset = new SizeF(offsetX, offsetY);
            Pos = PointF.Add(Pos, offset);
        }

        public void Rotate(float angle)
        {
            float deltaAngle = (angle - Angle);
            Angle += deltaAngle;
            Angle %= 360;
        }

        internal void FlipX(float centerY)
        {
            throw new NotImplementedException();
        }

        internal void FlipY(float centerX)
        {
            throw new NotImplementedException();
        }
    }

    public class Fov
    {
        public int No { get; }
        public PointF CenterPos { get; private set; }
        public SizeF Size { get; }
        public List<Pad> InnerPadList { get; set; } = new List<Pad>();

        public Fov(int fovNo, PointF centerPos, SizeF size)
        {
            No = fovNo;
            CenterPos = centerPos;
            Size = size;
        }

        public void AddPad(Pad pad)
        {
            pad.FovNo = No;
            InnerPadList.Add(pad);

            // Update position
            var unionRect = new RectangleF();
            foreach (Pad p in InnerPadList)
            {
                if (unionRect.IsEmpty)
                {
                    unionRect = p.GetBoundRect();
                }
                else
                {
                    unionRect = RectangleF.Union(unionRect, p.GetBoundRect());
                }
            }
            CenterPos = DrawingHelper.CenterPoint(unionRect);
        }
        public List<Component> InnerComponentList { get; set; } = new List<Component>();

        public void AddComponent(Component component)
        {
            InnerComponentList.Add(component);
            foreach (Pad pad in component.PadList)
            {
                AddPad(pad);
            }

        }

        public RectangleF GetBoundRect()
        {
            //RectangleF unionRect = new RectangleF();

            //foreach(Pad pad in innerPadList)
            //{
            //    if (unionRect.IsEmpty)
            //        unionRect = pad.GetBoundRect();
            //    else
            //        unionRect = RectangleF.Union(unionRect, pad.GetBoundRect());
            //}

            return DrawingHelper.FromCenterSize(CenterPos, Size);
        }

        public RectangleF GetRoi()
        {
            var unionRect = new RectangleF();

            foreach (Pad pad in InnerPadList)
            {
                if (unionRect.IsEmpty)
                {
                    unionRect = pad.GetBoundRect();
                }
                else
                {
                    unionRect = RectangleF.Union(unionRect, pad.GetBoundRect());
                }
            }

            return unionRect;
        }

        public void Offset(float offsetX, float offsetY)
        {
            var offset = new SizeF(offsetX, offsetY);
            CenterPos = PointF.Add(CenterPos, offset);
        }

        internal void FlipX(float centerY)
        {
            throw new NotImplementedException();
        }

        internal void FlipY(float centerX)
        {
            throw new NotImplementedException();
        }
    }

    public class PatternEdge
    {
        public int PatternNo { get; }

        private List<List<PointF>> pointChainList = new List<List<PointF>>();

        public PatternEdge(int patternNo)
        {
            PatternNo = patternNo;
        }

        public void AddNewList()
        {
            var pointList = new List<PointF>();
            pointChainList.Add(pointList);
        }

        public void AddNewList(PointF point)
        {
            var pointList = new List<PointF>();
            pointChainList.Add(pointList);

            pointList.Add(point);
        }

        public void AddPoint(PointF point)
        {
            pointChainList.Last().Add(point);
        }

        public Figure CreateFigure()
        {
            var figureGroup = new FigureGroup();
            foreach (List<PointF> pointList in pointChainList)
            {
                figureGroup.AddFigure(new PolygonFigure(pointList, new Pen(Color.Yellow, 1)));
            }

            return figureGroup;
        }
    }

    public class PadInfo
    {
        public Figure Figure { get; private set; }
        public int PatternNo { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Area { get; set; }

        private PointF centroid;
        public PointF Centroid
        {
            get => centroid;
            set => centroid = value;
        }
        public float Angle { get; set; }

        public PadInfo()
        {
        }

        public PadInfo(int patternNo, Figure figure, float area, float width, float height, PointF centroid, float angle)
        {
            PatternNo = patternNo;
            Figure = figure;
            Area = area;
            Width = width;
            Height = height;
            this.centroid = centroid;
            Angle = angle;
        }

        public void Copy(PadInfo srcPadInfo)
        {
            PatternNo = srcPadInfo.PatternNo;
            Area = srcPadInfo.Area;
            Width = srcPadInfo.Width;
            Height = srcPadInfo.Height;
            centroid = srcPadInfo.centroid;
            Figure = (Figure)srcPadInfo.Figure?.Clone();
            Angle = srcPadInfo.Angle;
        }

        public void LoadParam(XmlElement paramElement)
        {
            PatternNo = Convert.ToInt32(XmlHelper.GetValue(paramElement, "PatternNo", "0"));
            Area = Convert.ToSingle(XmlHelper.GetValue(paramElement, "Area", "0"));
            Width = Convert.ToSingle(XmlHelper.GetValue(paramElement, "Width", "0"));
            Height = Convert.ToSingle(XmlHelper.GetValue(paramElement, "Height", "0"));
            Angle = Convert.ToSingle(XmlHelper.GetValue(paramElement, "Angle", "0"));

            XmlHelper.GetValue(paramElement, "Centroid", ref centroid);

            XmlElement figureElement = paramElement["Figure"];
            if (figureElement != null)
            {
                string typeStr = XmlHelper.GetValue(figureElement, "Type", "");
                Figure = FigureFactory.Create(typeStr);

                Figure.Load(figureElement);
            }
        }

        public void SaveParam(XmlElement paramElement)
        {
            XmlHelper.SetValue(paramElement, "PatternNo", PatternNo.ToString());
            XmlHelper.SetValue(paramElement, "Area", Area.ToString());
            XmlHelper.SetValue(paramElement, "Width", Width.ToString());
            XmlHelper.SetValue(paramElement, "Height", Height.ToString());
            XmlHelper.SetValue(paramElement, "Centroid", centroid);
            XmlHelper.SetValue(paramElement, "Angle", Angle.ToString());

            XmlElement figureElement = paramElement.OwnerDocument.CreateElement("Figure");
            paramElement.AppendChild(figureElement);

            if (Figure.Type != FigureType.Group && Figure.Type != FigureType.Polygon)
            {
                Figure.Save(figureElement);
            }
            else
            {
                var rectFigure = new RectangleFigure(Figure.GetRectangle(), new Pen(Color.Yellow, 1));
                rectFigure.Save(figureElement);
            }
        }
    }

    public abstract class GerberData
    {
        public Unit Unit { get; set; } = Unit.mm;

        public float GetUnitScale() // To micron
        {
            if (Unit == Unit.inch)
            {
                return 2450;
            }

            return 1000;
        }
        public Coornidate Coornidate { get; set; }
        public int Version { get; set; }
        public SizeF BoardSize { get; set; }

        public virtual RectangleF BoardRectangleF()
        {
            return new RectangleF(new PointF(0, 0), BoardSize);
        }

        protected List<Fiducial> fiducialList = new List<Fiducial>();
        public List<Fiducial> FiducialList => fiducialList;

        protected List<BadMark> badMarkList = new List<BadMark>();
        public List<BadMark> BadMarkList => badMarkList;

        protected List<ModuleGroup> moduleGroupList = new List<ModuleGroup>();
        public List<ModuleGroup> ModuleGroupList => moduleGroupList;
        public List<Pattern> PatternList { get; } = new List<Pattern>();

        protected List<PatternEdge> patternEdgeList = new List<PatternEdge>();

        public void AddPattern(Pattern pattern)
        {
            PatternList.Add(pattern);
        }

        public void AddPatternEdge(PatternEdge patternEdge)
        {
            patternEdgeList.Add(patternEdge);
        }

        public Pattern GetPattern(int patternNo)
        {
            return PatternList.Find(x => x.No == patternNo);
        }

        public PatternEdge GetPatternEdge(int patternNo)
        {
            return patternEdgeList.Find(x => x.PatternNo == patternNo);
        }

        public void AddModuleGroup(ModuleGroup moduleGroup)
        {
            moduleGroupList.Add(moduleGroup);
        }

        public ModuleGroup GetModuleGroup(int groupNo)
        {
            return moduleGroupList.Find(x => x.GroupNo == groupNo);
        }

        protected List<Component> componentList = new List<Component>();
        public List<Component> ComponentList
        {
            get => componentList;
            set => componentList = value;
        }

        public virtual PadInfo CreatePadInfo(Pad pad)
        {
            Pattern pattern = GetPattern(pad.PatternNo);
            Figure figure = CreatePadFigure(pad);
            PointF newPadPos = pad.Pos;
            if (figure != null)
            {
                PointF centroid = DrawingHelper.Add(newPadPos, pattern.Centroid);
                figure.Offset(newPadPos.X, newPadPos.Y);
                return new PadInfo(pad.PatternNo, figure, pattern.Area, pattern.Size.Width, pattern.Size.Height, pad.Center, pad.Angle);
            }

            return null;
        }

        public virtual Figure CreatePadFigure(Pad pad, bool good = true)
        {
            Pattern pattern = GetPattern(pad.PatternNo);

            Figure figure = null;
            if (pattern.Shape == FigureShape.Undifined)
            {
                PatternEdge patternEdge = GetPatternEdge(pad.PatternNo);
                figure = patternEdge.CreateFigure();
                figure.Rotate(pattern.Angle);
            }
            else
            {
                figure = pattern.CreateFigure(good);
            }

            figure.Rotate(pad.Angle);

            return figure;
        }

        protected List<Fov> fovList = new List<Fov>();
        private List<Fov> optimizedFovList = null;

        public bool PathOptimized => optimizedFovList != null;

        protected void GenerateFovListForPad(ref List<Fov> fovList, SizeF fovSize, float fovMargin)
        {
            var fovRect = new RectangleF(new PointF(0, 0), fovSize);
            fovRect.Inflate(-fovMargin, -fovMargin);

            fovList = new List<Fov>();

            var padList = new List<Pad>();
            padList.AddRange(GetPadList());

            if (padList.Count == 0)
            {
                return;
            }

            var nextPadList = new List<Pad>();
            nextPadList.AddRange(padList);

            do
            {
                padList.Clear();
                padList.AddRange(nextPadList);
                nextPadList.Clear();

                var fov = new Fov(fovList.Count + 1, new PointF(0, 0), fovSize);
                fovList.Add(fov);

                fov.AddPad(padList[0]);
                padList.Remove(padList[0]);

                foreach (Pad pad in padList)
                {
                    RectangleF fovRoi = fov.GetRoi();
                    RectangleF padRect = pad.GetBoundRect();
                    var unionRect = RectangleF.Union(fovRoi, padRect);

                    //RectangleF interRect = RectangleF.Intersect(fovRect, padRect);
                    //if (interRect.Equals(padRect))
                    if (unionRect.Width < fovRect.Width && unionRect.Height < fovRect.Height)
                    {
                        fov.AddPad(pad);
                    }
                    else
                    {
                        nextPadList.Add(pad);
                    }
                }
            }
            while (nextPadList.Count > 0);

        }

        protected void GenerateFovListForComponent(ref List<Fov> fovList, SizeF fovSize, float fovMargin)
        {
            var fovRect = new RectangleF(new PointF(0, 0), fovSize);
            fovRect.Inflate(-fovMargin, -fovMargin);

            fovList = new List<Fov>();

            //GenerateComponentRect();

            var componentList = new List<Component>();
            componentList.AddRange(ComponentList);

            if (componentList.Count == 0)
            {
                return;
            }

            var nextComponentList = new List<Component>();
            nextComponentList.AddRange(componentList);

            do
            {
                componentList.Clear();
                componentList.AddRange(nextComponentList);
                nextComponentList.Clear();

                var fov = new Fov(fovList.Count + 1, new PointF(0, 0), fovSize);

                fov.AddComponent(componentList[0]);
                componentList.Remove(componentList[0]);

                foreach (Component component in componentList)
                {
                    RectangleF fovRoi = fov.GetRoi();
                    RectangleF componentRect = component.GetUnionRectF();
                    var unionRect = RectangleF.Union(fovRoi, componentRect);

                    //RectangleF interRect = RectangleF.Intersect(fovRect, padRect);
                    //if (interRect.Equals(padRect))
                    if (unionRect.Width < fovRect.Width && unionRect.Height < fovRect.Height)
                    {
                        fov.AddComponent(component);
                    }
                    else
                    {
                        nextComponentList.Add(component);
                    }
                }

                if (fov.InnerPadList.Count > 0)
                {
                    fovList.Add(fov);
                }
            }
            while (nextComponentList.Count > 0);

        }

        public void GenerateComponentRect()
        {
            var padList = new List<Pad>();
            padList.AddRange(GetPadList());

            foreach (Component component in componentList)
            {
                foreach (Pad pad in padList)
                {
                    if (component.RefCode == pad.RefCode)
                    {
                        component.AddPad(pad);
                    }
                }
            }

            AddComponentForNoRefCodePad(padList);
        }

        private void AddComponentForNoRefCodePad(List<Pad> padList)
        {
            foreach (Pad pad in padList)
            {
                if (string.IsNullOrEmpty(pad.RefCode) == true)
                {
                    int componentNo = componentList.Count;
                    string refCode = string.Format("comp_{0}", componentNo);
                    string partCode = string.Format("part_{0}", componentNo);
                    float posX = DrawingHelper.CenterPoint(pad.Rectangle.GetBoundRect()).X;
                    float posY = DrawingHelper.CenterPoint(pad.Rectangle.GetBoundRect()).Y;
                    float angle = pad.Angle;
                    int moduleNo = pad.ModuleNo;

                    var component = new Component(componentNo, refCode, partCode, posX, posY, angle, moduleNo);
                    component.AddPad(pad);
                    componentList.Add(component);
                }
            }
        }

        protected List<Fov> TravellingSalesmanPath(PointF fidPt, List<Fov> fovList, bool forceSearch)
        {
            if (optimizedFovList == null || forceSearch)
            {
                var startPoint = new Location(-1, fidPt.X, fidPt.Y);
                var locationList = new List<Location>();
                fovList.ForEach(f => locationList.Add(new Location(f.No, f.CenterPos.X, f.CenterPos.Y)));

                TSPAlgorithm ni = new NearestInsert();
                Location[] foundPath = ni.FindPath(startPoint, locationList.ToArray());

                //TSPAlgorithm ga = new Genatic(new Location(0, startPoint.X, startPoint.Y), locationList.ToArray(), 300);
                //Location[] foundPath = ga.FindPath(startPoint, locationList.ToArray());

                foundPath = TSPImprove.Improve2Opt(foundPath);

                optimizedFovList = new List<Fov>();
                foreach (Location location in foundPath)
                {
                    optimizedFovList.Add(fovList.Find(f => f.No == location.Index));
                }
            }
            //return fovList;
            return optimizedFovList;
        }

        public abstract Figure CreateFiducialFigure(Fiducial fiducial);
        public abstract Figure CreateBadMarkFigure(BadMark badMark);
        public abstract List<Fov> GetFovList(SizeF fovSize, float fovMargin, bool reSearch = false);
        public abstract List<Pad> GetPadList();
        public abstract void Offset(float offsetX, float offsetY);
        public abstract void FlipX(float centerY);
        public abstract void FlipY(float centerX);
    }
}
