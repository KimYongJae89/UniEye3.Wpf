using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.UI
{
    public abstract class MessageElement
    {
        public abstract MessageElement Clone();
    }

    public class TextElement : MessageElement
    {
        public Font Font { get; set; }
        public string Text { get; set; }

        public TextElement(string text, Font font)
        {
            Text = text;
            Font = font;
        }

        public override MessageElement Clone()
        {
            return new TextElement(Text, Font);
        }
    }

    public class TextBlockElement : MessageElement
    {
        public List<TextElement> ElementList { get; set; } = new List<TextElement>();

        public override MessageElement Clone()
        {
            var textBlockElement = new TextBlockElement();
            foreach (TextElement element in ElementList)
            {
                textBlockElement.ElementList.Add((TextElement)element.Clone());
            }

            return textBlockElement;
        }
    }

    public class TableCellElement : MessageElement
    {
        public TextBlockElement TextBlockElement { get; set; } = new TextBlockElement();
        public int ColSpan { get; set; }
        public int RowSpan { get; set; }
        public Color BackgroundColor { get; set; } = Color.Transparent;

        public TableCellElement()
        {
        }

        public TableCellElement(string text, Font font, Color backgroundColor)
        {
            BackgroundColor = backgroundColor;
            TextBlockElement.ElementList.Add(new TextElement(text, font));
        }

        public override MessageElement Clone()
        {
            var cellElement = new TableCellElement();
            cellElement.ColSpan = ColSpan;
            cellElement.RowSpan = RowSpan;
            cellElement.BackgroundColor = BackgroundColor;
            cellElement.TextBlockElement = (TextBlockElement)TextBlockElement.Clone();

            return cellElement;
        }
    }

    public class TableRowElement : MessageElement
    {
        public List<TableCellElement> ElementList { get; set; } = new List<TableCellElement>();

        public override MessageElement Clone()
        {
            var rowElement = new TableRowElement();
            foreach (TableCellElement element in ElementList)
            {
                rowElement.ElementList.Add((TableCellElement)element.Clone());
            }

            return rowElement;
        }
    }

    public class TableElement : MessageElement
    {
        public List<TableRowElement> ElementList { get; set; } = new List<TableRowElement>();

        public override MessageElement Clone()
        {
            var tableElement = new TableElement();
            foreach (TableRowElement element in ElementList)
            {
                tableElement.ElementList.Add((TableRowElement)element.Clone());
            }

            return tableElement;
        }
    }

    public class Message
    {
        public List<MessageElement> ElementList { get; set; } = new List<MessageElement>();

        private TableElement tableElement;
        private Font tableFont;

        public Message Clone()
        {
            var cloneMessage = new Message();

            foreach (MessageElement element in ElementList)
            {
                cloneMessage.ElementList.Add(element.Clone());
            }

            return cloneMessage;
        }

        public void Append(Message message)
        {
            foreach (MessageElement element in message.ElementList)
            {
                ElementList.Add(element.Clone());
            }
        }

        public void AddLine()
        {
            ElementList.Add(new TextElement("\n", null));
        }

        public void AddText(string text, Font font = null)
        {
            ElementList.Add(new TextElement(text, font));
        }

        public void AddTextLine(string text, Font font = null)
        {
            ElementList.Add(new TextElement(text, font));
            ElementList.Add(new TextElement("\n", null));
        }

        public void AddTextLine(List<string> textList, Font font = null)
        {
            foreach (string text in textList)
            {
                ElementList.Add(new TextElement(text, font));
                ElementList.Add(new TextElement("\n", null));
            }
        }

        public void BeginTable(Font font, params string[] headerText)
        {
            tableElement = new TableElement();
            ElementList.Add(tableElement);
            tableFont = font;

            AddTableRow(headerText);
        }

        public void AddTableRow(params string[] cellTexts)
        {
            AddTableRow(tableFont, Color.Transparent, cellTexts);
        }

        public void AddTableRow(Color backgroundColor, params string[] cellTexts)
        {
            AddTableRow(tableFont, backgroundColor, cellTexts);
        }

        public void AddTableRow(Font font, Color backgroundColor, params string[] cellTexts)
        {
            if (tableElement == null)
            {
                return;
            }

            var tableRowElement = new TableRowElement();

            foreach (string cellText in cellTexts)
            {
                tableRowElement.ElementList.Add(new TableCellElement(cellText, font, backgroundColor));
            }

            tableElement.ElementList.Add(tableRowElement);
        }

        public void EndTable()
        {
            tableElement = null;
        }
    }
}
