using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DynMvp.GerberCvt.Epm
{
    public class GerberCvtLoader : GerberCvt.GerberCvtLoader
    {
        private float unitScale = 1000;
        public GerberData GerberData { get; private set; }

        public override GerberCvt.GerberData GetGerberData()
        {
            return GerberData;
        }

        public override bool Load(string fileName)
        {
            XmlDocument xmlDocument = XmlHelper.Load(fileName);
            if (xmlDocument == null)
            {
                return false;
            }

            GerberData = new GerberData();

            XmlElement documentElement = xmlDocument.DocumentElement;

            LoadUnitInfo(documentElement["unit"]);
            LoadBoardInfo(documentElement["board"]);
            LoadBoardArrayInfo(documentElement["boardarrays"]);
            LoadPadInfo(documentElement["pds"]);
            LoadFootPrintInfo(documentElement["footprints"]);
            LoadPartInfo(documentElement["parts"]);
            LoadFiducialInfo(documentElement["fiducials"]);
            LoadComponentInfo(documentElement["components"]);

            return true;
        }

        private void LoadUnitInfo(XmlElement unitElement)
        {
            GerberData.Unit = (Unit)Enum.Parse(typeof(Unit), XmlHelper.GetAttributeValue(unitElement, "name", "mm"));
            unitScale = GerberData.GetUnitScale();

            GerberData.Coornidate = (Coornidate)Enum.Parse(typeof(Coornidate), XmlHelper.GetAttributeValue(unitElement, "Coord", "LL"));
        }

        private void LoadBoardInfo(XmlElement boardElement)
        {
            float boardSizeX = Convert.ToSingle(XmlHelper.GetAttributeValue(boardElement, "w", "0")) * unitScale;
            float boardSizeY = Convert.ToSingle(XmlHelper.GetAttributeValue(boardElement, "h", "0")) * unitScale;

            GerberData.BoardSize = new SizeF(boardSizeX, boardSizeY);
        }

        private void LoadBoardArrayInfo(XmlElement boardArrayElement)
        {
            foreach (XmlElement groupElement in boardArrayElement)
            {
                if (groupElement.Name == "group")
                {
                    var moduleGroup = new ModuleGroup();
                    LoadGroupInfo(moduleGroup, groupElement);

                    GerberData.AddModuleGroup(moduleGroup);
                }
            }
        }

        private void LoadGroupInfo(ModuleGroup moduleGroup, XmlElement groupElement)
        {
            moduleGroup.GroupNo = Convert.ToInt32(XmlHelper.GetAttributeValue(groupElement, "name", "0"));
            float groupPosX = Convert.ToSingle(XmlHelper.GetAttributeValue(groupElement, "x", "0")) * unitScale;
            float groupPosY = Convert.ToSingle(XmlHelper.GetAttributeValue(groupElement, "y", "0")) * unitScale;
            float groupWidth = Convert.ToSingle(XmlHelper.GetAttributeValue(groupElement, "w", "0")) * unitScale;
            float groupHeight = Convert.ToSingle(XmlHelper.GetAttributeValue(groupElement, "h", "0")) * unitScale;

            moduleGroup.Region = new RectangleF(groupPosX, groupPosY, groupWidth, groupHeight);

            foreach (XmlElement arrayElement in groupElement)
            {
                if (arrayElement.Name == "boardarray")
                {
                    int moduleNo = Convert.ToInt32(XmlHelper.GetAttributeValue(arrayElement, "num", "1"));
                    float modulePosX = Convert.ToSingle(XmlHelper.GetAttributeValue(arrayElement, "x", "0")) * unitScale;
                    float modulePosY = Convert.ToSingle(XmlHelper.GetAttributeValue(arrayElement, "y", "0")) * unitScale;
                    float angle = Convert.ToSingle(XmlHelper.GetAttributeValue(arrayElement, "rot", "0"));

                    var module = new Module(moduleNo, modulePosX, modulePosY, angle);

                    module.Skip = XmlHelper.GetAttributeValue(arrayElement, "skip", "0") == "1";
                    module.Name = XmlHelper.GetAttributeValue(arrayElement, "name", "");

                    moduleGroup.AddModule(module);
                }
            }
        }

        private PatternEdge LoadPolyShape(int patternNo, XmlElement polyShapeElement)
        {
            var patternEdge = new PatternEdge(patternNo);

            patternEdge.AddNewList();

            foreach (XmlElement ptElement in polyShapeElement)
            {
                if (ptElement.Name == "pt")
                {
                    float posX = Convert.ToSingle(XmlHelper.GetAttributeValue(ptElement, "x", "0")) * unitScale;
                    float posY = Convert.ToSingle(XmlHelper.GetAttributeValue(ptElement, "y", "0")) * unitScale;

                    patternEdge.AddPoint(new PointF(posX, posY));
                }
                //else if (ptElement.Name == "arc")
                //{
                //    int dir = Convert.ToInt32(XmlHelper.GetValue(shapeElement, "dir", "0"));
                //    float posX = Convert.ToSingle(XmlHelper.GetValue(shapeElement, "x", "0")) * unitScale;
                //    float posY = Convert.ToSingle(XmlHelper.GetValue(shapeElement, "y", "0")) * unitScale;
                //    float orgX = Convert.ToSingle(XmlHelper.GetValue(shapeElement, "orgx", "0")) * unitScale;
                //    float orgY = Convert.ToSingle(XmlHelper.GetValue(shapeElement, "orgy", "0")) * unitScale;

                //    shapeNodeList.Add(new ArcShapeNode(dir, posX, posY, orgX, orgY));
                //}
            }

            return patternEdge;
        }

        private FigureShape GetFigureShape(string shapeStr)
        {
            switch (shapeStr)
            {
                default:
                case "rc": return FigureShape.Rectangle;
                case "cir": return FigureShape.Circle;
                case "poly": return FigureShape.Undifined;
                case "ob": return FigureShape.Oblong;
            }
        }

        private void LoadPadInfo(XmlElement padListElement)
        {
            foreach (XmlElement padElement in padListElement)
            {
                if (padElement.Name == "pd")
                {
                    int patternNo = Convert.ToInt32(XmlHelper.GetAttributeValue(padElement, "num", "0"));

                    XmlElement roiElement = padElement["roi"];
                    FigureShape patternShape = GetFigureShape(XmlHelper.GetAttributeValue(roiElement, "type", "rc"));
                    float offsetX = Convert.ToSingle(XmlHelper.GetAttributeValue(roiElement, "offx", "0")) * unitScale;
                    float offsetY = Convert.ToSingle(XmlHelper.GetAttributeValue(roiElement, "offy", "0")) * unitScale;
                    float area = Convert.ToSingle(XmlHelper.GetAttributeValue(roiElement, "area", "0")) * unitScale * unitScale;
                    float perimeter = Convert.ToSingle(XmlHelper.GetAttributeValue(roiElement, "perimeter", "0")) * unitScale;

                    float width = 0;
                    float height = 0;

                    if (patternShape == FigureShape.Circle)
                    {
                        width = height = Convert.ToSingle(XmlHelper.GetAttributeValue(roiElement, "w", "0")) * unitScale;
                    }
                    else
                    {
                        width = Convert.ToSingle(XmlHelper.GetAttributeValue(roiElement, "w", "0")) * unitScale;
                        height = Convert.ToSingle(XmlHelper.GetAttributeValue(roiElement, "h", "0")) * unitScale;
                    }

                    XmlElement shapeElement = padElement["shape"];
                    FigureShape shapeType = GetFigureShape(XmlHelper.GetAttributeValue(shapeElement, "type", "rc"));
                    float angle = Convert.ToSingle(XmlHelper.GetAttributeValue(shapeElement, "rot", "0"));
                    float shapeArea = Convert.ToSingle(XmlHelper.GetAttributeValue(shapeElement, "area", "0")) * unitScale * unitScale;

                    var pattern = new Pattern(patternNo, shapeType, width, height, offsetX, offsetY, shapeArea, angle);
                    GerberData.AddPattern(pattern);

                    if (shapeType == FigureShape.Undifined)
                    {
                        PatternEdge patternEdge = LoadPolyShape(patternNo, shapeElement);
                        GerberData.AddPatternEdge(patternEdge);
                    }
                }
            }
        }

        private void LoadFootPrintInfo(XmlElement footprintListElement)
        {
            foreach (XmlElement footprintElement in footprintListElement)
            {
                if (footprintElement.Name == "footprint")
                {
                    string name = XmlHelper.GetAttributeValue(footprintElement, "name", "");
                    float width = Convert.ToSingle(XmlHelper.GetAttributeValue(footprintElement, "bodywidth", "0")) * unitScale;
                    float height = Convert.ToSingle(XmlHelper.GetAttributeValue(footprintElement, "bodyheight", "0")) * unitScale;
                    float offsetX = Convert.ToSingle(XmlHelper.GetAttributeValue(footprintElement, "offx", "0")) * unitScale;
                    float offsetY = Convert.ToSingle(XmlHelper.GetAttributeValue(footprintElement, "offy", "0")) * unitScale;

                    var footprint = new FootPrint(name, width, height, offsetX, offsetY);

                    foreach (XmlElement pinElement in footprintElement)
                    {
                        if (pinElement.Name == "pin")
                        {
                            int pinNo = Convert.ToInt32(XmlHelper.GetValue(pinElement, "name", "1"));
                            int patternNo = Convert.ToInt32(XmlHelper.GetAttributeValue(pinElement, "pd", "0"));
                            float posX = Convert.ToSingle(XmlHelper.GetAttributeValue(pinElement, "x", "0")) * unitScale;
                            float posY = Convert.ToSingle(XmlHelper.GetAttributeValue(pinElement, "y", "0")) * unitScale;
                            float angle = Convert.ToSingle(XmlHelper.GetAttributeValue(footprintElement, "rot", "0"));

                            Pattern padPattern = GerberData.GetPattern(patternNo);
                            if (padPattern != null)
                            {
                                footprint.AddPin(pinNo, patternNo, posX, posY, angle);
                            }
                        }
                    }

                    GerberData.FootPrintList.Add(footprint);
                }
            }
        }

        private void LoadPartInfo(XmlElement partListElement)
        {
            foreach (XmlElement partElement in partListElement)
            {
                if (partElement.Name == "part")
                {
                    string name = XmlHelper.GetAttributeValue(partElement, "name", "");
                    string footprint = XmlHelper.GetAttributeValue(partElement, "footprint", "");
                    string package = XmlHelper.GetAttributeValue(partElement, "pkg", "");

                    if (name != "" && footprint != "")
                    {
                        var part = new Part(name, footprint, package);
                        GerberData.PartList.Add(part);
                    }
                }
            }
        }

        private FiducialType GetFiducialType(string typeStr)
        {
            switch (typeStr)
            {
                default:
                case "G": return FiducialType.Global;
                case "A": return FiducialType.Module;
                case "L": return FiducialType.Local;
            }
        }

        private void LoadFiducialInfo(XmlElement fiducialListElement)
        {
            int no = 0;
            foreach (XmlElement fiducialElement in fiducialListElement)
            {
                if (fiducialElement.Name == "fiducial")
                {
                    string name = XmlHelper.GetAttributeValue(fiducialElement, "name", "");
                    string partName = XmlHelper.GetAttributeValue(fiducialElement, "part", "");
                    string side = XmlHelper.GetAttributeValue(fiducialElement, "si", "");
                    float posX = Convert.ToSingle(XmlHelper.GetAttributeValue(fiducialElement, "x", "")) * unitScale;
                    float posY = Convert.ToSingle(XmlHelper.GetAttributeValue(fiducialElement, "y", "")) * unitScale;
                    float angle = Convert.ToSingle(XmlHelper.GetAttributeValue(fiducialElement, "rot", ""));
                    FiducialType fiducialType = GetFiducialType(XmlHelper.GetAttributeValue(fiducialElement, "type", ""));
                    int groupNo = Convert.ToInt32(XmlHelper.GetAttributeValue(fiducialElement, "group", ""));

                    int boardArrayNum = 0;
                    string boardArrayNumStr = XmlHelper.GetAttributeValue(fiducialElement, "boardarray_num", "");
                    if (string.IsNullOrEmpty(boardArrayNumStr) == false)
                    {
                        boardArrayNum = Convert.ToInt32(boardArrayNumStr);
                    }

                    string blockName = XmlHelper.GetAttributeValue(fiducialElement, "block_name", "");

                    var fiducial = new Fiducial(no, partName, fiducialType, posX, posY, angle, boardArrayNum);
                    GerberData.FiducialList.Add(fiducial);
                    no++;

                    //Part part = new Part(name, footprint, package);
                    //partList.Add(part);
                }
            }
        }

        private void LoadGroupComponent(XmlElement groupElement)
        {
            int groupNo = Convert.ToInt32(XmlHelper.GetValue(groupElement, "name", "0"));

            ModuleGroup moduleGroup = GerberData.GetModuleGroup(groupNo);
            if (moduleGroup == null)
            {
                return;
            }

            foreach (XmlElement boardArrayElement in groupElement)
            {
                if (boardArrayElement.Name == "boardarray")
                {
                    int boardArrayNum = Convert.ToInt32(XmlHelper.GetAttributeValue(boardArrayElement, "num", ""));
                    Module module = moduleGroup.GetModule(boardArrayNum);
                    if (module == null)
                    {
                        continue;
                    }

                    string blockName = XmlHelper.GetAttributeValue(boardArrayElement, "block_name", "");

                    int componentNo = 0;
                    foreach (XmlElement componentElement in boardArrayElement)
                    {
                        string name = XmlHelper.GetAttributeValue(componentElement, "name", "");
                        string partName = XmlHelper.GetAttributeValue(componentElement, "part", "");

                        float posX = Convert.ToSingle(XmlHelper.GetAttributeValue(componentElement, "x", "0")) * unitScale;
                        float posY = Convert.ToSingle(XmlHelper.GetAttributeValue(componentElement, "y", "0")) * unitScale;
                        float offsetX = Convert.ToSingle(XmlHelper.GetAttributeValue(componentElement, "offx", "0")) * unitScale;
                        float offsetY = Convert.ToSingle(XmlHelper.GetAttributeValue(componentElement, "offy", "0")) * unitScale;
                        float angle = Convert.ToSingle(XmlHelper.GetAttributeValue(componentElement, "rot", "0"));

                        if (string.IsNullOrEmpty(partName) == false)
                        {
                            var component = new Component(componentNo, name, partName, posX, posY, angle, boardArrayNum);
                            component.OffsetX = offsetX;
                            component.OffsetY = offsetY;
                            GerberData.ComponentList.Add(component);
                        }

                        componentNo++;
                    }
                }
            }
        }

        private void LoadComponentInfo(XmlElement componentSectionElement)
        {
            foreach (XmlElement groupElement in componentSectionElement)
            {
                if (groupElement.Name == "group")
                {
                    LoadGroupComponent(groupElement);
                }
            }
        }
    }
}
