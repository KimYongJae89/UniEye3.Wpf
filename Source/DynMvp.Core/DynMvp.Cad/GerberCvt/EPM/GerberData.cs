using DynMvp.Base;
using DynMvp.Data;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.GerberCvt.Epm
{
    public class Part
    {
        public string Name { get; set; }
        public string Footprint { get; set; }
        public string Package { get; set; }

        public Part(string name, string footprint, string package)
        {
            Name = name;
            Footprint = footprint;
            Package = package;
        }
    }

    public class Pin
    {
        public int PinNo { get; }
        public int PatternNo { get; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float Angle { get; set; }

        public Pin(int pinNo, int patternNo, float posX, float posY, float angle)
        {
            PinNo = pinNo;
            PatternNo = patternNo;
            PosX = posX;
            PosY = posY;
            Angle = angle;
        }
    }

    public class FootPrint
    {
        public string Name { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float OffsetX { get; set; }
        public float OffsetY { get; set; }
        public List<Pin> PinList { get; } = new List<Pin>();

        public FootPrint(string name, float width, float height, float offsetX, float offsetY)
        {
            Name = name;
            Width = width;
            Height = height;
            OffsetX = offsetX;
            OffsetY = offsetY;
        }

        public void AddPin(int pinNo, int patternNo, float posX, float posY, float angle)
        {
            PinList.Add(new Pin(pinNo, patternNo, posX, posY, angle));
        }
    }

    public class GerberData : GerberCvt.GerberData
    {
        public List<FootPrint> FootPrintList { get; set; } = new List<FootPrint>();
        public List<Part> PartList { get; set; } = new List<Part>();

        public override List<Fov> GetFovList(SizeF fovSize, float fovMargin, bool rebuild)
        {
            if (fovList.Count == 0)
            {
                GenerateFovListForPad(ref fovList, fovSize, fovMargin);
            }

            PointF startPt = fiducialList[fiducialList.Count - 1].Pos;
            if (badMarkList.Count != 0)
            {
                startPt = badMarkList[badMarkList.Count - 1].Pos;
            }

            return TravellingSalesmanPath(startPt, fovList, rebuild);
        }

        public static void ShortestPath(PointF fidPt, List<Fov> fovList)
        {
            var pointList = new List<PointF>();
            pointList.Add(fidPt);

            foreach (Fov fov in fovList)
            {
                pointList.Add(fov.CenterPos);
            }

            int posCount = pointList.Count;
            float[,] graph = new float[posCount, posCount];
            Array.Clear(graph, 0, graph.Length);

            for (int i = 0; i < posCount - 1; i++)
            {
                for (int j = i + 1; j < posCount; j++)
                {
                    graph[j, i] = graph[i, j] = MathHelper.GetLength(pointList[i], pointList[j]);
                }
            }

            var tempFovList = new List<Fov>();
            tempFovList.AddRange(fovList);

            fovList.Clear();

            int[] indexArr = Dijkstra(graph, 0, posCount);
            foreach (int index in indexArr)
            {
                if (index > 0)
                {
                    fovList.Add(tempFovList[index - 1]);
                }
            }
        }

        private static int MinimumDistance(Node[] distanceNode)
        {
            float min = float.MaxValue;
            int minIndex = 0;

            for (int v = 0; v < distanceNode.Length; ++v)
            {
                if (distanceNode[v].TreeSet == false && distanceNode[v].Distance <= min)
                {
                    min = distanceNode[v].Distance;
                    minIndex = v;
                }
            }

            return minIndex;
        }

        private struct Node
        {
            public int Index { set; get; }
            public float Distance { set; get; }
            public bool TreeSet { set; get; }
        };

        public static int[] Dijkstra(float[,] graph, int source, int verticesCount)
        {
            var distanceNode = new Node[verticesCount];
            for (int i = 0; i < verticesCount; ++i)
            {
                distanceNode[i].Index = i;
                distanceNode[i].Distance = int.MaxValue;
                distanceNode[i].TreeSet = false;
            }

            distanceNode[source].Distance = 0;

            for (int count = 0; count < verticesCount - 1; ++count)
            {
                int u = MinimumDistance(distanceNode);
                distanceNode[u].TreeSet = true;

                float curDistance = distanceNode[u].Distance;
                for (int v = 0; v < verticesCount; ++v)
                {
                    if (!distanceNode[v].TreeSet && Convert.ToBoolean(graph[u, v]) && curDistance != float.MaxValue && curDistance + graph[u, v] < distanceNode[v].Distance)
                    {
                        distanceNode[v].Distance = curDistance + graph[u, v];
                    }
                }
            }

            var indexList = new List<int>();

            var nodeList = distanceNode.ToList();
            nodeList.Sort((x, y) => (int)(x.Distance - y.Distance));

            foreach (Node node in nodeList)
            {
                indexList.Add(node.Index);
            }

            return indexList.ToArray();
        }

        private FootPrint GetFootPrint(string footPrintName)
        {
            return FootPrintList.Find(x => x.Name == footPrintName);
        }

        private Part GetPart(string partName)
        {
            return PartList.Find(x => x.Name == partName);
        }

        private List<Pad> GetComponentPadList(Component component)
        {
            var padList = new List<Pad>();

            Part part = GetPart(component.PartCode);

            FootPrint footPrint = GetFootPrint(part.Footprint);
            if (footPrint == null)
            {
                return padList;
            }

            int pinNo = 0;
            foreach (Pin pin in footPrint.PinList)
            {
                Pattern pattern = GetPattern(pin.PatternNo);
                RotatedRect patternRect = pattern.GetRectangle();
                patternRect.Offset(pin.PosX, pin.PosY);

                var pad = new Pad(0, pin.PatternNo, pin.PosX, pin.PosY, component.OffsetX, -component.OffsetY, patternRect, component.RefCode, pinNo, component.ModuleNo, 0);
                pad.Rotate(component.Angle);

                pad.Offset(component.Pos.X, component.Pos.Y);
                pad.Offset(-component.OffsetX, -component.OffsetY);

                padList.Add(pad);
            }

            return padList;
        }

        private List<Pad> padList = null;
        public override List<Pad> GetPadList()
        {
            if (padList == null)
            {
                padList = new List<Pad>();

                foreach (Component component in componentList)
                {
                    padList.AddRange(GetComponentPadList(component));
                }
            }

            return padList;
        }

        public override void Offset(float offsetX, float offsetY)
        {
            foreach (Fiducial fiducial in fiducialList)
            {
                fiducial.OffsetPos(offsetX, offsetY);
            }

            foreach (BadMark badMark in badMarkList)
            {
                badMark.OffsetPos(offsetX, offsetY);
            }

            foreach (ModuleGroup moduleGroup in moduleGroupList)
            {
                moduleGroup.Offset(offsetX, offsetY);
            }

            foreach (Component component in componentList)
            {
                component.Offset(offsetX, offsetY);
            }

            if (padList != null)
            {
                foreach (Pad pad in padList)
                {
                    pad.Offset(offsetX, offsetY);
                }
            }

            if (fovList != null)
            {
                foreach (Fov fov in fovList)
                {
                    fov.Offset(offsetX, offsetY);
                }
            }
        }

        public override void FlipX(float centerX)
        {
            foreach (Fiducial fiducial in fiducialList)
            {
                fiducial.FlipX(centerX);
            }

            foreach (BadMark badMark in badMarkList)
            {
                badMark.FlipX(centerX);
            }

            foreach (ModuleGroup moduleGroup in moduleGroupList)
            {
                moduleGroup.FlipX(centerX);
            }
        }

        public override void FlipY(float centerY)
        {
            foreach (Fiducial fiducial in fiducialList)
            {
                fiducial.FlipY(centerY);
            }

            foreach (BadMark badMark in badMarkList)
            {
                badMark.FlipY(centerY);
            }

            foreach (ModuleGroup moduleGroup in moduleGroupList)
            {
                moduleGroup.FlipY(centerY);
            }
        }

        public override Figure CreateFiducialFigure(Fiducial fiducial)
        {
            Part part = GetPart(fiducial.PartName);

            FigureShape figureShape = FigureShape.Rectangle;
            RectangleF figureRect = DrawingHelper.FromCenterSize(new PointF(0, 0), new SizeF(10, 10));

            FootPrint footPrint = GetFootPrint(part.Footprint);
            if (footPrint != null && footPrint.PinList.Count > 0)
            {
                Pin pin = footPrint.PinList[0];

                Pattern pattern = GetPattern(pin.PatternNo);
                figureShape = pattern.Shape;
                figureRect = pattern.GetRectangle().GetBoundRect();
                figureRect.Offset(pin.PosX, pin.PosY);

                figureRect.Offset(fiducial.Pos.X, fiducial.Pos.Y);
            }

            fiducial.Size = figureRect.Size;

            Figure figure = null;
            switch (figureShape)
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

            return figure;
        }

        public override Figure CreateBadMarkFigure(BadMark badMark)
        {
            return badMark.CreateFigure();
        }
    }
}
