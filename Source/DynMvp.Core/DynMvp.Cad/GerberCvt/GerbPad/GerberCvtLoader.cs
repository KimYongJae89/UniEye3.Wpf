using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace DynMvp.GerberCvt.GerbPad
{
    internal enum SectionType
    {
        Unknown, MSYSHEADER, SMGHEADER, UNIEHEADER, BOARD, ARRAY, FIDUCIAL, BADMARK, PATTERN, COMPONENT, PAD, FOV, PATTERNEDGE, END
    }

    internal delegate void ParseLineDelegate(string line);

    public class GerberCvtLoader : GerberCvt.GerberCvtLoader
    {
        private float unitScale = 1000;
        public GerberData GerberData { get; private set; }

        private ParseLineDelegate ParseLine = null;

        public override GerberCvt.GerberData GetGerberData()
        {
            return GerberData;
        }

        public override bool Load(string fileName)
        {
            GerberData = new GerberData();
            GerberData.ModuleGroupList.Add(new ModuleGroup());

            string[] lines = File.ReadAllLines(fileName, Encoding.Default);

            SectionType sectionType = SectionType.Unknown;

            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                if (line[0] == '@')
                {
                    sectionType = (SectionType)Enum.Parse(typeof(SectionType), line.Substring(1));
                    switch (sectionType)
                    {
                        case SectionType.SMGHEADER:
                        case SectionType.MSYSHEADER:
                        case SectionType.UNIEHEADER: ParseLine = ParseLineHeader; break;
                        case SectionType.BOARD: ParseLine = ParseLineBoard; break;
                        case SectionType.ARRAY: ParseLine = ParseLineArray; break;
                        case SectionType.FIDUCIAL: ParseLine = ParseLineFiducial; break;
                        case SectionType.BADMARK: ParseLine = ParseLineBadMark; break;
                        case SectionType.PATTERN: ParseLine = ParseLinePattern; break;
                        case SectionType.PAD: ParseLine = ParseLinePad; break;
                        case SectionType.COMPONENT: ParseLine = ParseLineComponent; break;
                        case SectionType.FOV: ParseLine = ParseLineFov; break;
                        case SectionType.PATTERNEDGE: ParseLine = ParseLinePatternEdge; break;
                    }
                }
                else
                {
                    ParseLine?.Invoke(line);
                }
            }

            return true;
        }

        private void ParseLineHeader(string line)
        {
            string[] tokens = line.Split('=');
            if (tokens.Count() != 2)
            {
                return;
            }

            switch (tokens[0])
            {
                case "Version": GerberData.Version = Convert.ToInt32(tokens[1]); break;
                case "Unit":
                    GerberData.Unit = (Unit)Enum.Parse(typeof(Unit), tokens[1].ToLower());
                    unitScale = GerberData.GetUnitScale();
                    break;
                case "Coordinate": GerberData.Coornidate = (Coornidate)Enum.Parse(typeof(Coornidate), tokens[1]); break;
                case "OffsetX": GerberData.OffsetX = Convert.ToSingle(tokens[1]); break;
                case "OffsetY": GerberData.OffsetY = Convert.ToSingle(tokens[1]); break;
                case "OffsetXFromRightBottom": GerberData.OffsetXFromRightBottom = Convert.ToSingle(tokens[1]) * unitScale; break;
                case "OffsetYFromRightBottom": GerberData.OffsetYFromRightBottom = Convert.ToSingle(tokens[1]) * unitScale; break;
                case "BmpOriginX": GerberData.BmpOriginX = Convert.ToSingle(tokens[1]); break;
                case "BmpOriginY": GerberData.BmpOriginY = Convert.ToSingle(tokens[1]); break;
                case "BmpSizeX": GerberData.BmpSizeX = Convert.ToSingle(tokens[1]); break;
                case "BmpSizeY": GerberData.BmpSizeY = Convert.ToSingle(tokens[1]); break;
                case "CombineArray": GerberData.CombineArray = Convert.ToInt32(tokens[1]) == 1; break;
                case "Arrays": GerberData.NumModule = Convert.ToInt32(tokens[1]); break;
                case "Fiducials": GerberData.NumFiducial = Convert.ToInt32(tokens[1]); break;
                case "Badmarks": GerberData.NumBadMark = Convert.ToInt32(tokens[1]); break;
                case "Patterns": GerberData.NumPattern = Convert.ToInt32(tokens[1]); break;
                case "Pads": GerberData.NumPad = Convert.ToInt32(tokens[1]); break;
                case "Fovs": GerberData.NumFov = Convert.ToInt32(tokens[1]); break;
            }
        }

        private void ParseLineBoard(string line)
        {
            string[] tokens = line.Split('\t');
            if (tokens.Count() != 2)
            {
                return;
            }

            float boardSizeX = Convert.ToSingle(tokens[0]) * unitScale;
            float boardSizeY = Convert.ToSingle(tokens[1]) * unitScale;

            GerberData.BoardSize = new SizeF(boardSizeX, boardSizeY);
        }

        private void ParseLineArray(string line)
        {
            string[] tokens = line.Split('\t');

            if (GerberData.Version == 1)
            {
                if (tokens.Count() != 4)
                {
                    return;
                }

                int moduleNo = Convert.ToInt32(tokens[0]);
                float posX = Convert.ToSingle(tokens[1]) * unitScale;
                float posY = Convert.ToSingle(tokens[2]) * unitScale;
                int angle = Convert.ToInt32(tokens[3]);

                var module = new Module(moduleNo, posX, posY, angle);
                GerberData.ModuleGroupList[0].AddModule(module);
            }
            else if (GerberData.Version == 2)
            {
                if (tokens.Count() != 10)
                {
                    return;
                }

                int moduleNo = Convert.ToInt32(tokens[0]);
                float shiftX = Convert.ToSingle(tokens[1]) * unitScale;
                float shiftY = Convert.ToSingle(tokens[2]) * unitScale;
                int angle = Convert.ToInt32(tokens[3]);
                MirrorType mirroType = GetMirrorType(tokens[4]);
                int cellNo = Convert.ToInt32(tokens[5]);
                float posX = Convert.ToSingle(tokens[6]) * unitScale;
                float posY = Convert.ToSingle(tokens[7]) * unitScale;
                float cellSizeX = Convert.ToSingle(tokens[8]) * unitScale;
                float cellSizeY = Convert.ToSingle(tokens[9]) * unitScale;

                var module = new Module(moduleNo, posX, posY, angle);
                GerberData.ModuleGroupList[0].AddModule(module);
            }
        }

        private MirrorType GetMirrorType(string typeStr)
        {
            switch (typeStr)
            {
                default:
                case "N": return MirrorType.NoMirror;
                case "X": return MirrorType.MirrorX;
                case "Y": return MirrorType.MirroY;
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

        private FigureShape GetFigureShape(string shapeStr)
        {
            switch (shapeStr)
            {
                default:
                case "R": return FigureShape.Rectangle;
                case "C": return FigureShape.Circle;
                case "U": return FigureShape.Undifined;
                case "O": return FigureShape.Oblong;
                case "S": return FigureShape.Sloped;
            }
        }

        private void ParseLineFiducial(string line)
        {
            string[] tokens = line.Split('\t');
            if (tokens.Count() != 11)
            {
                return;
            }

            int fidNo = Convert.ToInt32(tokens[0]);
            FiducialType fiducialType = GetFiducialType(tokens[1]);
            FigureShape fiducialShape = GetFigureShape(tokens[2]);
            float posX = Convert.ToSingle(tokens[3]) * unitScale;
            float posY = Convert.ToSingle(tokens[4]) * unitScale;
            float width = Convert.ToSingle(tokens[5]) * unitScale;
            float height = Convert.ToSingle(tokens[6]) * unitScale;
            float offsetX = Convert.ToSingle(tokens[7]) * unitScale;
            float offsetY = Convert.ToSingle(tokens[8]) * unitScale;
            string refCode = tokens[9];
            int moduleNo = Convert.ToInt32(tokens[10]);

            var fiducial = new Fiducial(fidNo, fiducialType, fiducialShape, posX, posY, width, height, offsetX, offsetY, refCode, moduleNo);
            GerberData.AddFiducial(fiducial);
        }

        private void ParseLineBadMark(string line)
        {
            string[] tokens = line.Split('\t');
            if (tokens.Count() != 7)
            {
                return;
            }

            int badMarkNo = Convert.ToInt32(tokens[0]);
            FigureShape badMarkShape = GetFigureShape(tokens[1]);
            float posX = Convert.ToSingle(tokens[2]) * unitScale;
            float posY = Convert.ToSingle(tokens[3]) * unitScale;
            float width = Convert.ToSingle(tokens[4]) * unitScale;
            float height = Convert.ToSingle(tokens[5]) * unitScale;
            int moduleNo = Convert.ToInt32(tokens[6]);

            var badMark = new BadMark(badMarkNo, badMarkShape, posX, posY, width, height, moduleNo);
            GerberData.AddBadMark(badMark);
        }

        private void ParseLinePattern(string line)
        {
            string[] tokens = line.Split('\t');
            if (tokens.Count() != 8)
            {
                return;
            }

            int patternNo = Convert.ToInt32(tokens[0]);
            FigureShape patternShape = GetFigureShape(tokens[1]);
            float width = Convert.ToSingle(tokens[2]) * unitScale;
            float height = Convert.ToSingle(tokens[3]) * unitScale;
            float centroidX = Convert.ToSingle(tokens[4]) * unitScale;
            float centroidY = Convert.ToSingle(tokens[5]) * unitScale;
            float area = Convert.ToSingle(tokens[6]) * unitScale * unitScale;
            float angle = Convert.ToSingle(tokens[7]);

            var pattern = new Pattern(patternNo, patternShape, width, height, centroidX, centroidY, area, angle);
            GerberData.AddPattern(pattern);
        }

        private void ParseLinePad(string line)
        {
            string[] tokens = line.Split('\t');
            if (tokens.Count() != 12 && tokens.Count() != 11)
            {
                return;
            }

            int padNo = Convert.ToInt32(tokens[0]);
            int patternNo = Convert.ToInt32(tokens[1]);
            float posX = Convert.ToSingle(tokens[2]) * unitScale;
            float posY = Convert.ToSingle(tokens[3]) * unitScale;
            float left = Convert.ToSingle(tokens[4]) * unitScale;
            float top = Convert.ToSingle(tokens[5]) * unitScale;
            float right = Convert.ToSingle(tokens[6]) * unitScale;
            float bottom = Convert.ToSingle(tokens[7]) * unitScale;
            string refCode = tokens[8].Trim().Trim('\"');

            int pinNo = 0, moduleNo = 0, fovNo = 0;
            if (GerberData.Version == 1)
            {
                if (tokens.Count() == 12)
                {
                    pinNo = Convert.ToInt32(tokens[9]);
                    moduleNo = Convert.ToInt32(tokens[10]);
                    fovNo = Convert.ToInt32(tokens[11]);
                }
                else
                {
                    pinNo = 0;
                    moduleNo = Convert.ToInt32(tokens[9]);
                    fovNo = Convert.ToInt32(tokens[10]);
                }
            }
            else if (GerberData.Version == 2)
            {
                pinNo = Convert.ToInt32(tokens[9]);
                moduleNo = Convert.ToInt32(tokens[10]);
            }

            var pad = new Pad(padNo, patternNo, posX, posY, 0, 0, new UI.RotatedRect(left, top, right - left, bottom - top, 0), refCode, pinNo, moduleNo, fovNo);
            GerberData.AddPad(pad);
        }

        private void ParseLineFov(string line)
        {
            string[] tokens = line.Split('\t');
            if (tokens.Count() != 3)
            {
                return;
            }

            int fovNo = Convert.ToInt32(tokens[0]);
            float posX = Convert.ToSingle(tokens[1]) * unitScale;
            float posY = Convert.ToSingle(tokens[2]) * unitScale;

            var fov = new Fov(fovNo, new PointF(posX, posY), new SizeF(0, 0));
            GerberData.AddFov(fov);
        }

        private void ParseLineComponent(string line)
        {
            string[] tokens = line.Split('\t');
            if (tokens.Count() != 7)
            {
                return;
            }

            int componentNo = Convert.ToInt32(tokens[0]);
            string refCode = tokens[1].Trim().Trim('\"');
            string partCode = tokens[2].Trim().Trim('\"');
            float posX = Convert.ToSingle(tokens[3]) * unitScale;
            float posY = Convert.ToSingle(tokens[4]) * unitScale;
            float angle = Convert.ToSingle(tokens[5]);
            int moduleNo = Convert.ToInt32(tokens[6]);

            var component = new Component(componentNo, refCode, partCode, posX, posY, angle, moduleNo);
            GerberData.ComponentList.Add(component);
        }

        private void ParseLinePatternEdge(string line)
        {
            /*
            string[] tokens = line.Split('\t');
            if (tokens.Count() == 2 && tokens[0] == "Pattern")
            {
                int patternNo = Convert.ToInt32(tokens[1]);
                PatternEdge patternEdge = new PatternEdge(patternNo);

                gerberData.AddPatternEdge(patternEdge);
            }
            else if (tokens.Count() == 3)
            {
                PatternEdge patternEdge = gerberData.GetLastPatternEdge();
                if (patternEdge != null)
                {
                    char drawType = tokens[0][0];
                    float posX = Convert.ToSingle(tokens[1]) * unitScale;
                    float posY = Convert.ToSingle(tokens[2]) * unitScale;

                    if (drawType == 'M')
                        patternEdge.AddNewList(new PointF(posX, posY));
                    else
                        patternEdge.AddPoint(new PointF(posX, posY));
                }
            }
            */
            int index = line.IndexOf("Pattern");
            if (index > -1)
            {
                int patternNo = Convert.ToInt32(line.Substring(7));
                var patternEdge = new PatternEdge(patternNo);

                GerberData.AddPatternEdge(patternEdge);
            }
            else if (index == -1)
            {
                string[] tokens = line.Split('\t');
                if (tokens.Count() == 3)
                {
                    PatternEdge patternEdge = GerberData.GetLastPatternEdge();
                    if (patternEdge != null)
                    {
                        char drawType = tokens[0][0];
                        float posX = Convert.ToSingle(tokens[1]) * unitScale;
                        float posY = Convert.ToSingle(tokens[2]) * unitScale;

                        if (drawType == 'M')
                        {
                            patternEdge.AddNewList(new PointF(posX, posY));
                        }
                        else
                        {
                            patternEdge.AddPoint(new PointF(posX, posY));
                        }
                    }
                }
            }

        }
    }
}
