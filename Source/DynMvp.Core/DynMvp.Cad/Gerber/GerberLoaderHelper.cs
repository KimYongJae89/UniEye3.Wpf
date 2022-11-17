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
    public class GerberLoaderHelper
    {
        public static int GetOpCodeOrder(OpCode opCode)
        {
            switch (opCode)
            {
                case OpCode.Add:
                case OpCode.Sub:
                    return 1;
                case OpCode.Mul:
                case OpCode.Div:
                    return 2;
            }

            return 0;
        }

        public static string ReadString(BinaryReader reader, char delimiter)
        {
            int len = 0;
            return ReadString(reader, delimiter, ref len);
        }

        public static string ReadString(BinaryReader reader, char delimiter, ref int len)
        {
            len = 0;

            var sb = new StringBuilder();
            while (true)
            {
                char ch = (char)reader.ReadByte();
                if (ch != delimiter)
                {
                    sb.Append(ch);
                }
                else
                {
                    reader.BaseStream.Position--;
                    break;
                }
            }

            string valueStr = sb.ToString();

            if (string.IsNullOrEmpty(valueStr))
            {
                return "";
            }

            len = valueStr.Length;
            return valueStr;
        }

        public static double ReadDouble(BinaryReader reader)
        {
            int len = 0;
            return ReadDouble(reader, ref len);
        }

        public static double ReadDouble(BinaryReader reader, ref int len)
        {
            len = 0;

            var sb = new StringBuilder();
            while (true)
            {
                char ch = (char)reader.ReadByte();
                if (char.IsNumber(ch) || ch == '.' || ch == '-')
                {
                    sb.Append(ch);
                }
                else
                {
                    reader.BaseStream.Position--;
                    break;
                }
            }

            string valueStr = sb.ToString();

            if (string.IsNullOrEmpty(valueStr))
            {
                return 0;
            }

            len = valueStr.Length;
            return Convert.ToDouble(valueStr);
        }

        public static int ReadInteger(BinaryReader reader)
        {
            int len = 0;
            return ReadInteger(reader, ref len);
        }

        public static int ReadInteger(BinaryReader reader, ref int len)
        {
            len = 0;

            var sb = new StringBuilder();
            while (true)
            {
                char ch = (char)reader.ReadByte();
                if (char.IsNumber(ch) || ch == '-')
                {
                    sb.Append(ch);
                }
                else
                {
                    reader.BaseStream.Position--;
                    break;
                }
            }

            string valueStr = sb.ToString();

            if (string.IsNullOrEmpty(valueStr))
            {
                return 0;
            }

            len = valueStr.Length;
            return Convert.ToInt32(valueStr);
        }
    }
}
