using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.UI
{
    public class HtmlMessageBuilder
    {
        public static string Build(Message message)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<HTML><HEAD></HEAD><BODY style='font-family=\"sans-serif\"'>");

            foreach (MessageElement element in message.ElementList)
            {
                stringBuilder.Append(ConvertHtml(element));
            }
            stringBuilder.Append("</BODY></HTML>");

            return stringBuilder.ToString();
        }

        public static string ConvertHtml(MessageElement element)
        {
            string htmlString = "";
            if (element is TextElement)
            {
                htmlString = ConvertTextElement((TextElement)element);
            }
            else if (element is TextBlockElement)
            {
                htmlString = ConvertTextBlockElement((TextBlockElement)element);
            }
            else if (element is TableElement tableElement)
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("<Table border= \"1\">");
                foreach (TableRowElement rowElement in tableElement.ElementList)
                {
                    stringBuilder.AppendLine(ConvertTableRowElement(rowElement));
                }
                stringBuilder.AppendLine("</Table>");

                htmlString = stringBuilder.ToString();
            }

            return htmlString;
        }

        private static string ConvertTextElement(TextElement textElement)
        {
            if (textElement.Font != null)
            {
                return string.Format("<font face=\"{0}\" size=\"{1}\">{2}</font>", textElement.Font.FontFamily.Name, textElement.Font.SizeInPoints, textElement.Text);
            }
            else
            {
                if (textElement.Text == "\n")
                {
                    return "<br>";
                }
                else
                {
                    return textElement.Text;
                }
            }
        }

        private static string ConvertTextBlockElement(TextBlockElement textBlockElement)
        {
            string htmlString = "<p>";
            foreach (TextElement textElement in textBlockElement.ElementList)
            {
                htmlString += ConvertTextElement(textElement);
            }
            htmlString += "</p>";

            return htmlString;
        }

        private static string ConvertTableRowElement(TableRowElement rowElement)
        {
            string htmlString = "<tr>";
            foreach (TableCellElement cellElement in rowElement.ElementList)
            {
                if (cellElement.BackgroundColor == Color.Transparent)
                {
                    htmlString += string.Format("<td>{0}</td>", ConvertTextBlockElement(cellElement.TextBlockElement));
                }
                else
                {
                    htmlString += string.Format("<td bgcolor = \"{1}\">{0}</td>",
                        ConvertTextBlockElement(cellElement.TextBlockElement), ColorTranslator.ToHtml(cellElement.BackgroundColor));
                }

            }
            htmlString += "</tr>";
            return htmlString;
        }
    }
}
