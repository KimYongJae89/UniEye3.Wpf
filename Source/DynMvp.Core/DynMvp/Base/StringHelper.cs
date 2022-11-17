using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Base
{
    public class StringHelper
    {
        public static string HexToString(string hexString)
        {
            var sb = new StringBuilder();

            string[] hexValuesSplit = hexString.Split(' ');
            foreach (string hex in hexValuesSplit)
            {
                int value = Convert.ToInt32(hex, 16);
                sb.Append((char)value);
            }

            return sb.ToString();
        }

        public static byte[] BinaryStringToByteArray(string binaryString, bool inverseOrder)
        {
            var byteList = new List<byte>();

            string remainString = binaryString;

            for (int i = 0; i < binaryString.Length; i += 8)
            {
                string subString = "";
                if (remainString.Length > 8)
                {
                    subString = remainString.Substring(0, 8);
                }
                else
                {
                    subString = remainString.Substring(0);
                }

                if (subString.Length < 8)
                {
                    subString += new string('0', 8 - subString.Length);
                }

                if (inverseOrder)
                {
                    subString = ReverseString(subString);
                }

                byteList.Add(Convert.ToByte(subString, 2));

                if (remainString.Length > 8)
                {
                    remainString = remainString.Substring(8);
                }
            }

            return byteList.ToArray();
        }


        public static string ByteArrayToHexString(byte[] byteArray)
        {
            string hexString = "";
            for (int i = 0; i < byteArray.Length; i++)
            {
                hexString += byteArray[i].ToString("X02");
            }

            return hexString;
        }

        public static byte[] HexStringToByteArray(string hexString)
        {
            var byteList = new List<byte>();
            int step = sizeof(byte) * 2;

            for (int i = 0; i < hexString.Length; i += step)
            {
                byte value = Convert.ToByte(hexString.Substring(i, step), 16);
                byteList.Add(value);
            }

            return byteList.ToArray();
        }


        public static short[] HexStringToShortArray(string hexString)
        {
            var byteList = new List<short>();
            int step = sizeof(short) * 2;

            for (int i = 0; i < hexString.Length; i += step)
            {
                short value = Convert.ToInt16(hexString.Substring(i, step), 16);
                byteList.Add(value);
            }

            return byteList.ToArray();
        }

        public static short[] HexStringToIntArray(string hexString)
        {
            var byteList = new List<short>();
            int step = sizeof(int) * 2;
            for (int i = 0; i < hexString.Length; i += step)
            {
                short value = Convert.ToInt16(hexString.Substring(i, step), 16);
                byteList.Add(value);
            }

            return byteList.ToArray();
        }


        public static string AlignWordSize(string hexString)
        {
            int remain = hexString.Length % 4;
            if (remain != 0)
            {
                hexString += new string('0', remain);
            }

            return hexString;
        }

        public static string SwapWordHex(string hexString)
        {
            hexString = AlignWordSize(hexString);

            string newHexString = "";
            for (int i = 0; i < hexString.Length; i += 4)
            {
                string subString1 = hexString.Substring(i, 2);
                string subString2 = hexString.Substring(i + 2, 2);

                newHexString += subString2 + subString1;
            }

            return newHexString;
        }

        public static string ReverseString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        public static string GetChecksum(string data, int checksumSize)
        {
            byte[] dataByte = Encoding.ASCII.GetBytes(data);
            return GetChecksum(dataByte, checksumSize);
        }

        public static string GetChecksum(byte[] dataByte, int checksumSize)
        {
            int checksum = 0;
            foreach (byte chData in dataByte)
            {
                checksum += chData;
            }

            string checksumStr = checksum.ToString("X4");

            return checksumStr.Substring(checksumStr.Length - checksumSize, checksumSize);
        }

        public static bool ByteCompare(string str, byte[] byteData)
        {
            byte[] strByteData = Encoding.ASCII.GetBytes(str);

            for (int i = 0; i < strByteData.Count(); i++)
            {
                if (strByteData[i] != byteData[i])
                {
                    return false;
                }
            }

            return true;
        }

        // Version Number Scheme : Major.Minor.Maintenence.Build
        public static int[] GetVersionNo(string versionStr)
        {
            int[] versionNo = new int[4];
            Array.Clear(versionNo, 0, 4);

            if (versionStr == "")
            {
                versionNo[0] = 1;
            }
            else
            {
                string[] subVersionStr = versionStr.Split('.');
                for (int i = 0; i < subVersionStr.Count() && i < 4; i++)
                {
                    versionNo[i] = Convert.ToInt32(subVersionStr[i]);
                }
            }

            return versionNo;
        }
    }
}
