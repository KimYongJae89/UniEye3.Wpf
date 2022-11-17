using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DynMvp.UI
{
    public class FigureProperty
    {
        public string Name { get; set; }
        public Font Font { get; set; }
        public Color TextColor { get; set; }
        public StringAlignment Alignment { get; set; }
        public Pen Pen { get; set; }
        public Brush Brush { get; set; }

        public FigureProperty(string name = "")
        {
            Name = name;
            Font = new Font("Arial", 12, FontStyle.Regular);
            Pen = new Pen(Color.Yellow, 0);
            Brush = null;
            Alignment = StringAlignment.Near;
            TextColor = Color.Black;
        }

        ~FigureProperty()
        {
            if (Pen != null)
            {
                Pen.Dispose();
                Pen = null;
            }

            if (Brush != null)
            {
                Brush.Dispose();
                Brush = null;
            }

            if (Font != null)
            {
                Font.Dispose();
                Font = null;
            }
        }

        public FigureProperty Clone()
        {
            var cloneFigureProperty = new FigureProperty();

            cloneFigureProperty.Font = (Font)Font.Clone();
            cloneFigureProperty.Pen = (Pen)Pen.Clone();
            if (Brush != null)
            {
                cloneFigureProperty.Brush = (Brush)Brush.Clone();
            }

            cloneFigureProperty.Alignment = Alignment;
            cloneFigureProperty.TextColor = TextColor;

            return cloneFigureProperty;
        }

        public void Load(XmlElement xmlElement)
        {
            var font = new Font("Arial", 12, FontStyle.Regular);
            if (XmlHelper.GetValue(xmlElement, "Font", ref font))
            {
                Font = font;
            }

            Brush brush = new SolidBrush(Color.Ivory);
            if (XmlHelper.GetValue(xmlElement, "Brush", ref brush))
            {
                Brush = brush;
            }

            var pen = new Pen(Color.Black);
            if (XmlHelper.GetValue(xmlElement, "Pen", ref pen))
            {
                Pen = pen;
            }

            Color textColor = Color.Black;
            if (XmlHelper.GetValue(xmlElement, "TextColor", ref textColor))
            {
                TextColor = textColor;
            }

            Alignment = (StringAlignment)Enum.Parse(typeof(StringAlignment), XmlHelper.GetValue(xmlElement, "Alignment", StringAlignment.Near.ToString()));
        }

        public void Save(XmlElement xmlElement)
        {
            if (Font != null)
            {
                XmlHelper.SetValue(xmlElement, "Font", Font);
            }

            if (Brush != null)
            {
                XmlHelper.SetValue(xmlElement, "Brush", Brush);
            }

            if (Pen != null)
            {
                XmlHelper.SetValue(xmlElement, "Pen", Pen);
            }

            if (TextColor != null)
            {
                XmlHelper.SetValue(xmlElement, "TextColor", TextColor);
            }

            XmlHelper.SetValue(xmlElement, "Alignment", Alignment.ToString());
        }
    }

    /// <summary>
    /// Figure 객체의 공용 Drawing Object 관리 객체
    /// 이 클래스를 상속 받아 프로그램에서 사용할 FigureProperty 목록을 관리할 수 있다.
    /// </summary>
    public class FigurePropertyPool
    {
        private static FigurePropertyPool _instance;
        public static void SetInstance(FigurePropertyPool instance)
        {
            _instance = instance;
        }
        public static FigurePropertyPool Instance()
        {
            if (_instance == null)
            {
                _instance = new FigurePropertyPool();
            }

            return _instance;
        }
        public List<FigureProperty> FigurePropertyList { get; set; } = new List<FigureProperty>();

        /// <summary>
        /// FigurePropertyPool을 파일로 부터 읽어 온다.
        /// </summary>
        /// <param name="fileName">FigurePropertyPool을 읽어 올 파일명</param>
        public void Load(string fileName)
        {
            XmlDocument xmlDocument = XmlHelper.Load(fileName);
            if (xmlDocument == null)
            {
                return;
            }

            XmlElement docElement = xmlDocument["FigurePropertyPool"];

            FigurePropertyList.Clear();

            foreach (XmlElement figurePropertyElement in docElement)
            {
                string figurePropertyName = XmlHelper.GetValue(figurePropertyElement, "Name", "");
                GetFigureProperty(figurePropertyName).Load(figurePropertyElement);
            }
        }

        /// <summary>
        /// FigurePropertyPool을 저장한다.
        /// </summary>
        /// <param name="fileName">FigurePropertyPool을 저장할 파일명</param>
        public void Save(string fileName)
        {
            var xmlDocument = new XmlDocument();

            XmlElement figurePropertyPoolElement = xmlDocument.CreateElement("", "FigurePropertyPool", "");
            xmlDocument.AppendChild(figurePropertyPoolElement);

            foreach (FigureProperty figureProperty in FigurePropertyList)
            {
                XmlElement figurePropertyElement = xmlDocument.CreateElement("", "FigureProperty", "");
                figurePropertyPoolElement.AppendChild(figurePropertyElement);

                figureProperty.Save(figurePropertyElement);
            }

            xmlDocument.Save(fileName);
        }


        /// <summary>
        /// Figure Property Pool에 저장된 FigureProperty를 가져 온다.
        /// 만약, 전달된 이름을 가진 Figure Property가 없다면 새로운 객체를 생성하여 반환한다.
        /// </summary>
        /// <param name="figurePropertyName">Figure Property 이름</param>
        /// <returns></returns>
        public virtual FigureProperty GetFigureProperty(string figurePropertyName)
        {
            FigureProperty figureProperty = FigurePropertyList.Find(x => figurePropertyName == x.Name);

            if (figureProperty == null)
            {
                figureProperty = new FigureProperty(figurePropertyName);
                FigurePropertyList.Add(figureProperty);
            }

            return figureProperty;
        }

        /// <summary>
        /// 새로운 FIgureProperty를 추가한다.
        /// </summary>
        /// <param name="figureProperty">추가할 FIgure Property</param>
        public void AddFigure(FigureProperty figureProperty)
        {
            FigurePropertyList.Add(figureProperty);
        }
    }
}
