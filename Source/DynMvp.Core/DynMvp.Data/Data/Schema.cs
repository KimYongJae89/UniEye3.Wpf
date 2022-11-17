using DynMvp.Base;
using DynMvp.Data.UI;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Data
{
    public enum SchemaFigureType
    {
        Value, Judgment
    }

    public class SchemaFigure : FigureGroup
    {
        public SchemaFigureType SchemaFigureType { get; set; }
    }

    public class Schema
    {
        public FigureProperty DefaultFigureProperty { get; set; } = new FigureProperty();
        public RectangleF Region { get; set; }
        public bool InvertY { get; set; } = false;
        public bool AutoFit { get; set; } = false;
        public float ViewScale { get; set; } = 1;

        public FigureGroup FigureGroup { get; set; } = new FigureGroup();
        public FigureGroup TempFigureGroup { get; set; } = null;
        public Color BackColor { get; set; }


        public IEnumerator<Figure> GetEnumerator()
        {
            return FigureGroup.GetEnumerator();
        }

        public Schema Clone()
        {
            var schema = new Schema();

            schema.Region = new RectangleF(Region.X, Region.Y, Region.Width, Region.Height);
            schema.ViewScale = ViewScale;
            schema.AutoFit = AutoFit;
            schema.BackColor = BackColor;
            schema.InvertY = InvertY;

            schema.FigureGroup = (FigureGroup)FigureGroup.Clone();

            return schema;
        }

        public void ResetTempProperty()
        {
            FigureGroup.ResetTempProperty();
        }

        public void AddFigure(Figure figure)
        {
            //FigureGroup figureGroup = this.figureGroup[category] as FigureGroup;
            //if (figureGroup == null)
            //{
            //    figureGroup = new FigureGroup(category);
            //    figureGroup.Deletable = false;
            //    figureGroup.Movable = false;
            //    figureGroup.Selectable = false;
            //    this.figureGroup.AddFigure(figureGroup);
            //}

            FigureGroup.AddFigure(figure);
            figure.Id = CreateFigureId();
        }

        public void RemoveFigure(Figure figure)
        {
            FigureGroup.RemoveFigure(figure);
        }

        private string CreateFigureId()
        {
            return string.Format("figure{0}", FigureGroup.NumFigure);
            //string newFigureId;
            //for (int i = 1; i < int.MaxValue; i++)
            //{
            //	newFigureId = String.Format("figure{0}", i);
            //	if (GetFigure(newFigureId) == null)
            //		return newFigureId;
            //}

            //throw new TooManyItemsException();
        }

        public Figure GetFigure(string id)
        {
            foreach (Figure figure in FigureGroup)
            {
                if (figure.Id == id)
                {
                    return figure;
                }
            }

            return null;
        }

        public List<Figure> GetFigureByName(string name, bool wildSearch = false)
        {
            var figureList = new List<Figure>();

            figureList = FigureGroup.GetFigureByName(name, false, wildSearch);

            return figureList;
        }

        public List<Figure> GetFigureByTag(string tag, bool wildSearch = false)
        {
            var figureList = new List<Figure>();

            figureList = FigureGroup.GetFigureByTag(tag, wildSearch);

            return figureList;
        }

        public Figure GetFigure(Point point)
        {
            return FigureGroup.GetFigure(point);
        }

        public void MoveUp(Figure figure)
        {
            FigureGroup.MoveUp(figure);
        }

        public void MoveTop(Figure figure)
        {
            FigureGroup.MoveTop(figure);
        }

        public void MoveDown(Figure figure)
        {
            FigureGroup.MoveDown(figure);
        }

        public void MoveBottom(Figure figure)
        {
            FigureGroup.MoveBottom(figure);
        }

        public void Draw(Graphics g, CoordTransformer coordTransformer, bool editable, string category)
        {
            List<Figure> figures = FigureGroup.FigureList.FindAll(f => f.Name == category);

            foreach (Figure figure in figures)
            {
                if (figure != null)
                {
                    figure.Draw(g, coordTransformer, editable);
                }
            }

            if (TempFigureGroup != null)
            {
                TempFigureGroup.Draw(g, coordTransformer, editable);
                TempFigureGroup = null;
            }
        }

        public void Draw(Graphics g, CoordTransformer coordTransformer, bool editable)
        {
            FigureGroup.Draw(g, coordTransformer, editable);
            if (TempFigureGroup != null)
            {
                TempFigureGroup.Draw(g, coordTransformer, editable);
                TempFigureGroup = null;
            }
        }

        public void Load(string fileName)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);

            XmlElement schemaElement = xmlDocument.DocumentElement;

            float x = Convert.ToSingle(XmlHelper.GetValue(schemaElement, "X", "0"));
            float y = Convert.ToSingle(XmlHelper.GetValue(schemaElement, "Y", "0"));
            float width = Convert.ToSingle(XmlHelper.GetValue(schemaElement, "Width", "0"));
            float height = Convert.ToSingle(XmlHelper.GetValue(schemaElement, "Height", "0"));
            Region = new RectangleF(x, y, width, height);
            ViewScale = Convert.ToSingle(XmlHelper.GetValue(schemaElement, "ViewScale", "1"));
            AutoFit = Convert.ToBoolean(XmlHelper.GetValue(schemaElement, "AutoFit", "False"));
            InvertY = Convert.ToBoolean(XmlHelper.GetValue(schemaElement, "InvertY", "False"));

            DefaultFigureProperty.Load(schemaElement);

            FigureGroup = new FigureGroup();
            FigureGroup.Load(schemaElement["FigureGroup"]);
        }

        public void Save(string fileName)
        {
            var xmlDocument = new XmlDocument();

            XmlElement schemaElement = xmlDocument.CreateElement("", "Schema", "");
            xmlDocument.AppendChild(schemaElement);

            XmlHelper.SetValue(schemaElement, "X", Region.X.ToString());
            XmlHelper.SetValue(schemaElement, "Y", Region.Y.ToString());
            XmlHelper.SetValue(schemaElement, "Width", Region.Width.ToString());
            XmlHelper.SetValue(schemaElement, "Height", Region.Height.ToString());
            XmlHelper.SetValue(schemaElement, "ViewScale", ViewScale.ToString());
            XmlHelper.SetValue(schemaElement, "AutoFit", AutoFit.ToString());
            XmlHelper.SetValue(schemaElement, "InvertY", InvertY.ToString());

            DefaultFigureProperty.Save(schemaElement);

            XmlHelper.SetValue(schemaElement, "BackColor", BackColor);

            XmlElement figureGroupElement = xmlDocument.CreateElement("", "FigureGroup", "");
            schemaElement.AppendChild(figureGroupElement);

            FigureGroup.Save(figureGroupElement);

            xmlDocument.Save(fileName);
        }
    }
}
