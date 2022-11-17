using DynMvp.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ModbusRTU
{
    public class ModbusLib
    {
        public class CRCErrorException : Exception
        {

        }

        private SerialPort ser = new SerialPort();
        private byte[] baResponse = new byte[1024];
        #region Property
        public bool IsOpen => ser.IsOpen;
        public int Timeout { get; set; } = 100;
        public string Port { get; set; } = "COM1";
        public int Baudrate { get; set; } = 115200;
        #endregion
        #region Interop
        [DllImport("kernel32.dll")]
        private static extern int GetTickCount();
        #endregion
        #region Construct
        public ModbusLib()
        {

        }
        #endregion
        #region Open / Close
        public bool Open()
        {
            try
            {
                if (!ser.IsOpen)
                {
                    ser.BaudRate = Baudrate;
                    ser.PortName = Port;
                    ser.DataBits = 8;
                    ser.StopBits = StopBits.One;
                    ser.Parity = Parity.None;
                    ser.ReadTimeout = Timeout;
                    ser.Open();
                    return true;
                }
            }
            catch (Exception) { }
            return false;
        }
        public bool Close()
        {
            if (ser.IsOpen)
            {
                ser.Close();
                return true;
            }
            return false;
        }
        #endregion

        #region CRC LRC CHECK
        private byte[] auchCRCHi =  {0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81,
         0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
         0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01,
         0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
         0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81,
         0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
         0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01,
         0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
         0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81,
         0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
         0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01,
         0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
         0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81,
         0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
         0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01,
         0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
         0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81,0x40};
        private byte[] auchCRCLo = {
         0x00, 0xC0, 0xC1, 0x01, 0xC3, 0x03, 0x02, 0xC2, 0xC6, 0x06, 0x07, 0xC7, 0x05, 0xC5, 0xC4,
         0x04, 0xCC, 0x0C, 0x0D, 0xCD, 0x0F, 0xCF, 0xCE, 0x0E, 0x0A, 0xCA, 0xCB, 0x0B, 0xC9, 0x09,
         0x08, 0xC8, 0xD8, 0x18, 0x19, 0xD9, 0x1B, 0xDB, 0xDA, 0x1A, 0x1E, 0xDE, 0xDF, 0x1F, 0xDD,
         0x1D, 0x1C, 0xDC, 0x14, 0xD4, 0xD5, 0x15, 0xD7, 0x17, 0x16, 0xD6, 0xD2, 0x12, 0x13, 0xD3,
         0x11, 0xD1, 0xD0, 0x10, 0xF0, 0x30, 0x31, 0xF1, 0x33, 0xF3, 0xF2, 0x32, 0x36, 0xF6, 0xF7,
         0x37, 0xF5, 0x35, 0x34, 0xF4, 0x3C, 0xFC, 0xFD, 0x3D, 0xFF, 0x3F, 0x3E, 0xFE, 0xFA, 0x3A,
         0x3B, 0xFB, 0x39, 0xF9, 0xF8, 0x38, 0x28, 0xE8, 0xE9, 0x29, 0xEB, 0x2B, 0x2A, 0xEA, 0xEE,
         0x2E, 0x2F, 0xEF, 0x2D, 0xED, 0xEC, 0x2C, 0xE4, 0x24, 0x25, 0xE5, 0x27, 0xE7, 0xE6, 0x26,
         0x22, 0xE2, 0xE3, 0x23, 0xE1, 0x21, 0x20, 0xE0, 0xA0, 0x60, 0x61, 0xA1, 0x63, 0xA3, 0xA2,
         0x62, 0x66, 0xA6, 0xA7, 0x67, 0xA5, 0x65, 0x64, 0xA4, 0x6C, 0xAC, 0xAD, 0x6D, 0xAF, 0x6F,
         0x6E, 0xAE, 0xAA, 0x6A, 0x6B, 0xAB, 0x69, 0xA9, 0xA8, 0x68, 0x78, 0xB8, 0xB9, 0x79, 0xBB,
         0x7B, 0x7A, 0xBA, 0xBE, 0x7E, 0x7F, 0xBF, 0x7D, 0xBD, 0xBC, 0x7C, 0xB4, 0x74, 0x75, 0xB5,
         0x77, 0xB7, 0xB6, 0x76, 0x72, 0xB2, 0xB3, 0x73, 0xB1, 0x71, 0x70, 0xB0, 0x50, 0x90, 0x91,
         0x51, 0x93, 0x53, 0x52, 0x92, 0x96, 0x56, 0x57, 0x97, 0x55, 0x95, 0x94, 0x54, 0x9C, 0x5C,
         0x5D, 0x9D, 0x5F, 0x9F, 0x9E, 0x5E, 0x5A, 0x9A, 0x9B, 0x5B, 0x99, 0x59, 0x58, 0x98, 0x88,
         0x48, 0x49, 0x89, 0x4B, 0x8B, 0x8A, 0x4A, 0x4E, 0x8E, 0x8F, 0x4F, 0x8D, 0x4D, 0x4C, 0x8C,
         0x44, 0x84, 0x85, 0x45, 0x87, 0x47, 0x46, 0x86, 0x82, 0x42, 0x43, 0x83, 0x41, 0x81, 0x80,0x40};

        //public void GetCRC(byte[] pby, int nSize, ref byte byFirstReturn, ref byte bySecondReturn)
        //{
        //    int uIndex;
        //    byte uchCRCHi = 0xff;
        //    byte uchCRCLo = 0xff;
        //    for (int i = 0; i < pby.Count(); i++)
        //    {
        //        uIndex = uchCRCHi ^ pby[i];
        //        uchCRCHi = (byte)(uchCRCLo ^ auchCRCHi[uIndex]);
        //        uchCRCLo = auchCRCLo[uIndex];
        //    }
        //    int CRC = (uchCRCHi << 8) | uchCRCLo;
        //    byFirstReturn = (byte)(CRC / 256);
        //    bySecondReturn = (byte)(CRC % 256);
        //    pby = null;
        //}

        private void GetCRC(byte[] message, int nSize, ref byte byFirstReturn, ref byte bySecondReturn)
        {
            //Function expects a modbus message of any length as well as a 2 byte CRC array in which to 
            //return the CRC values:

            ushort CRCFull = 0xFFFF;
            byte CRCHigh = 0xFF, CRCLow = 0xFF;
            char CRCLSB;

            for (int i = 0; i < nSize; i++)
            {
                CRCFull = (ushort)(CRCFull ^ message[i]);

                for (int j = 0; j < 8; j++)
                {
                    CRCLSB = (char)(CRCFull & 0x0001);
                    CRCFull = (ushort)((CRCFull >> 1) & 0x7FFF);

                    if (CRCLSB == 1)
                    {
                        CRCFull = (ushort)(CRCFull ^ 0xA001);
                    }
                }
            }
            bySecondReturn = CRCHigh = (byte)((CRCFull >> 8) & 0xFF);
            byFirstReturn = CRCLow = (byte)(CRCFull & 0xFF);
        }

        public void GetLRC(byte[] pby, int nSize, ref byte byFirstReturn, ref byte bySecondReturn)
        {
            int nLRC = 0;
            for (int i = 0; i < nSize; i++)
            {
                if (i % 2 == 1)
                {
                    continue;
                }

                byte[] src = new byte[pby.Length - i];
                for (int j = i; j < pby.Length; j++)
                {
                    src[j - i] = pby[j];
                }

                int n = HexToInt(src, 2);
                nLRC += n;
            }
            byte byLRC = (byte)nLRC;
            byLRC = (byte)(-byLRC);
            string sLRC = IntToHex(byLRC, 2);
            byFirstReturn = (byte)sLRC[0];
            bySecondReturn = (byte)sLRC[1];
            pby = null;
        }

        private int HexToInt(byte[] pby, int n)
        {
            string str = "";
            for (int i = 0; i < n; i++)
            {
                str += (char)pby[i];
            }

            return int.Parse(str, System.Globalization.NumberStyles.AllowHexSpecifier);
        }

        private string IntToHex(byte pby, int n)
        {
            string format = "{0:x" + n.ToString() + "}";
            return string.Format(format, pby);
        }
        #endregion

        #region Method
        public BitArray BitRead(int Slave, int StartAddr, int Length)
        {
            LogHelper.Debug(LoggerType.Device, "Start BitRead");

            byte[] data = new byte[8];
            byte addrHi = 0, addrLow = 0;
            byte lenHi = 0, lenLow = 0;
            byte crcHi = 0xff, crcLo = 0xff;

            DevideShort(StartAddr, ref addrHi, ref addrLow);
            DevideShort(Length, ref lenHi, ref lenLow);

            data[0] = Convert.ToByte(Slave);
            data[1] = 0x01;
            data[2] = addrHi;
            data[3] = addrLow;
            data[4] = lenHi;
            data[5] = lenLow;

            GetCRC(data, data.Length - 2, ref crcHi, ref crcLo);
            data[6] = crcHi;
            data[7] = crcLo;

            // System.Threading.Monitor.Enter(ser);

            int nResCount = Length / 8;
            if (Length % 8 != 0)
            {
                nResCount++;
            }

            int ResponseByteCount = nResCount + 5;
            ser.DiscardInBuffer();
            ser.DiscardOutBuffer();
            ser.Write(data, 0, data.Length);
            ser.BaseStream.Flush();
            int nPrev = GetTickCount();
            int gap = 0;
            int nRecv = 0;
            int nLen = 0;

            while (nRecv < ResponseByteCount)
            {
                try
                {
                    nLen = ser.Read(baResponse, nRecv, ResponseByteCount);
                    nRecv += nLen;
                }
                catch (TimeoutException) { };

                gap = GetTickCount() - nPrev;
                if (gap >= Timeout)
                {
                    break;
                }

                if (nRecv == ResponseByteCount)
                {
                    break;
                }
            }

            LogHelper.Error("BitRead - Middle");


            if (gap < Timeout)
            {
                byte rcrcHi = 0, rcrcLo = 0;
                GetCRC(baResponse, nRecv - 2, ref rcrcHi, ref rcrcLo);
                if (rcrcHi == baResponse[nRecv - 2] && rcrcLo == baResponse[nRecv - 1])
                {
                    int ByteCount = baResponse[2];
                    byte[] baData = new byte[ByteCount];
                    Array.Copy(baResponse, 3, baData, 0, ByteCount);
                    var ba = new BitArray(baData);

                    LogHelper.Debug(LoggerType.Device, "End BitRead");

                    return ba;
                }
                else
                {
                    LogHelper.Error("CRC Error");
                    throw new CRCErrorException();
                }
            }
            else
            {
                LogHelper.Error("Timeout");
                throw new TimeoutException();
            }

        }

        public int[] WordRead(int Slave, int StartAddr, int Length)
        {
            byte[] data = new byte[8];
            byte addrHi = 0, addrLow = 0;
            byte lenHi = 0, lenLow = 0;
            byte crcHi = 0xff, crcLo = 0xff;

            DevideShort(StartAddr, ref addrHi, ref addrLow);
            DevideShort(Length, ref lenHi, ref lenLow);

            data[0] = Convert.ToByte(Slave);
            data[1] = 0x03;
            data[2] = addrHi;
            data[3] = addrLow;
            data[4] = lenHi;
            data[5] = lenLow;

            GetCRC(data, data.Length - 2, ref crcHi, ref crcLo);
            data[6] = crcHi;
            data[7] = crcLo;

            int ResponseByteCount = Length * 2 + 5;

            System.Threading.Monitor.Enter(ser);

            ser.DiscardInBuffer();
            ser.DiscardOutBuffer();
            ser.Write(data, 0, data.Length);
            ser.BaseStream.Flush();
            int nPrev = GetTickCount();
            int gap = 0;
            int nRecv = 0;
            int nLen = 0;

            while (nRecv < ResponseByteCount)
            {
                try
                {
                    nLen = ser.Read(baResponse, nRecv, ResponseByteCount);
                    nRecv += nLen;
                }
                catch (TimeoutException) { };

                gap = GetTickCount() - nPrev;
                if (gap >= Timeout)
                {
                    break;
                }

                if (nRecv == ResponseByteCount)
                {
                    break;
                }
            }
            if (gap < Timeout)
            {
                byte rcrcHi = 0, rcrcLo = 0;
                GetCRC(baResponse, nRecv - 2, ref rcrcHi, ref rcrcLo);
                if (rcrcHi == baResponse[nRecv - 2] && rcrcLo == baResponse[nRecv - 1])
                {
                    int ByteCount = baResponse[2];
                    int[] rdata = new int[ByteCount / 2];
                    for (int i = 0; i < data.Length; i++)
                    {
                        rdata[i] = Convert.ToUInt16(baResponse[3 + (i * 2)] << 8 | baResponse[4 + (i * 2)]);
                    }

                    System.Threading.Monitor.Exit(ser);
                    return rdata;
                }
                else
                {
                    System.Threading.Monitor.Exit(ser);
                    throw new CRCErrorException();
                }
            }
            else
            {
                System.Threading.Monitor.Exit(ser);
                throw new TimeoutException();
            }
        }

        public void BitWrite(int Slave, int StartAddr, bool Value)
        {
            byte[] data = new byte[8];
            byte addrHi = 0, addrLow = 0;
            byte valHi = 0, valLow = 0;
            byte crcHi = 0xff, crcLo = 0xff;

            DevideShort(StartAddr, ref addrHi, ref addrLow);
            DevideShort(Value ? 0xFF00 : 0x0000, ref valHi, ref valLow);

            data[0] = Convert.ToByte(Slave);
            data[1] = 0x05;
            data[2] = addrHi;
            data[3] = addrLow;
            data[4] = valHi;
            data[5] = valLow;

            GetCRC(data, data.Length - 2, ref crcHi, ref crcLo);
            data[6] = crcHi;
            data[7] = crcLo;
            System.Threading.Monitor.Enter(ser);
            int ResponseByteCount = 8;
            ser.DiscardInBuffer();
            ser.DiscardOutBuffer();
            ser.Write(data, 0, data.Length);
            ser.BaseStream.Flush();
            int nPrev = GetTickCount();
            int gap = 0;
            int nRecv = 0;
            int nLen = 0;

            while (nRecv < ResponseByteCount)
            {
                try
                {
                    nLen = ser.Read(baResponse, nRecv, ResponseByteCount);
                    nRecv += nLen;
                }
                catch (TimeoutException) { };

                gap = GetTickCount() - nPrev;
                if (gap >= Timeout)
                {
                    break;
                }

                if (nRecv == ResponseByteCount)
                {
                    break;
                }
            }
            if (gap < Timeout)
            {
                byte rcrcHi = 0, rcrcLo = 0;
                GetCRC(baResponse, nRecv - 2, ref rcrcHi, ref rcrcLo);
                if (rcrcHi == baResponse[nRecv - 2] && rcrcLo == baResponse[nRecv - 1])
                {
                    System.Threading.Monitor.Exit(ser);
                }
                else
                {
                    throw new CRCErrorException();
                }
            }
            else
            {
                throw new TimeoutException();
            }

            data = null;
        }
        public void WordWrite(int Slave, int StartAddr, int Value)
        {
            byte[] data = new byte[8];
            byte addrHi = 0, addrLow = 0;
            byte valHi = 0, valLow = 0;
            byte crcHi = 0xff, crcLo = 0xff;

            DevideShort(StartAddr, ref addrHi, ref addrLow);
            DevideShort(Value, ref valHi, ref valLow);

            data[0] = Convert.ToByte(Slave);
            data[1] = 0x06;
            data[2] = addrHi;
            data[3] = addrLow;
            data[4] = valHi;
            data[5] = valLow;
            System.Threading.Monitor.Enter(ser);
            GetCRC(data, data.Length - 2, ref crcHi, ref crcLo);
            data[6] = crcHi;
            data[7] = crcLo;
            int ResponseByteCount = 8;
            ser.DiscardInBuffer();
            ser.DiscardOutBuffer();
            ser.Write(data, 0, data.Length);
            ser.BaseStream.Flush();
            int nPrev = GetTickCount();
            int gap = 0;
            int nRecv = 0;
            int nLen = 0;

            while (nRecv < ResponseByteCount)
            {
                try
                {
                    nLen = ser.Read(baResponse, nRecv, ResponseByteCount);
                    nRecv += nLen;
                }
                catch (TimeoutException) { };

                gap = GetTickCount() - nPrev;
                if (gap >= Timeout)
                {
                    break;
                }

                if (nRecv == ResponseByteCount)
                {
                    break;
                }
            }
            if (gap < Timeout)
            {
                byte rcrcHi = 0, rcrcLo = 0;
                GetCRC(baResponse, nRecv - 2, ref rcrcHi, ref rcrcLo);
                if (rcrcHi == baResponse[nRecv - 2] && rcrcLo == baResponse[nRecv - 1])
                {
                    System.Threading.Monitor.Exit(ser);
                }
                else
                {
                    throw new CRCErrorException();
                }
            }
            else
            {
                throw new TimeoutException();
            }
            data = null;
        }
        public void MultiBitWrite(int Slave, int StartAddr, bool[] Value)
        {
            LogHelper.Debug(LoggerType.Device, "Start MultiBitWrite");

            int Length = Value.Length / 8;
            Length += (Value.Length % 8 == 0) ? 0 : 1;

            byte[] data = new byte[9 + Length];
            byte addrHi = 0, addrLow = 0;
            byte lenHi = 0, lenLow = 0;
            byte crcHi = 0xff, crcLo = 0xff;

            DevideShort(StartAddr, ref addrHi, ref addrLow);
            DevideShort(Value.Length, ref lenHi, ref lenLow);

            data[0] = Convert.ToByte(Slave);
            data[1] = 0x0F;
            data[2] = addrHi;
            data[3] = addrLow;
            data[4] = lenHi;
            data[5] = lenLow;
            data[6] = Convert.ToByte(Length);

            for (int i = 0; i < Length; i++)
            {
                byte val = 0;
                int nTemp = 0;
                for (int j = (i * 8); j < Value.Length && j < (i * 8) + 8; j++)
                {
                    if (Value[j])
                    {
                        val |= Convert.ToByte(Math.Pow(2, nTemp));
                    }

                    nTemp++;
                }
                data[7 + i] = val;
            }
            // System.Threading.Monitor.Enter(ser);
            GetCRC(data, data.Length - 2, ref crcHi, ref crcLo);
            data[data.Length - 2] = crcHi;
            data[data.Length - 1] = crcLo;
            int ResponseByteCount = 8;
            ser.DiscardInBuffer();
            ser.DiscardOutBuffer();
            ser.Write(data, 0, data.Length);
            ser.BaseStream.Flush();
            int nPrev = GetTickCount();
            int gap = 0;
            int nRecv = 0;
            int nLen = 0;

            while (nRecv < ResponseByteCount)
            {
                try
                {
                    nLen = ser.Read(baResponse, nRecv, ResponseByteCount);
                    nRecv += nLen;
                }
                catch (TimeoutException) { };

                gap = GetTickCount() - nPrev;
                if (gap >= Timeout)
                {
                    break;
                }

                if (nRecv == ResponseByteCount)
                {
                    break;
                }
            }
            if (gap < Timeout)
            {
                byte rcrcHi = 0, rcrcLo = 0;
                GetCRC(baResponse, nRecv - 2, ref rcrcHi, ref rcrcLo);
                if (rcrcHi == baResponse[nRecv - 2] && rcrcLo == baResponse[nRecv - 1])
                {
                    // System.Threading.Monitor.Exit(ser);
                }
                else
                {
                    LogHelper.Error("MultiBitWrite : CRCErrorException");
                    throw new CRCErrorException();
                }
            }
            else
            {
                LogHelper.Error("MultiBitWrite : TimeoutException");

                throw new TimeoutException();
            }
            Value = null;
            data = null;

            LogHelper.Debug(LoggerType.Device, "End MultiBitWrite");
        }

        public void MultiWordWrite(int Slave, int StartAddr, int[] Value)
        {
            byte[] data = new byte[9 + (Value.Length * 2)];
            byte addrHi = 0, addrLow = 0;
            byte lenHi = 0, lenLow = 0;
            byte crcHi = 0xff, crcLo = 0xff;

            DevideShort(StartAddr, ref addrHi, ref addrLow);
            DevideShort(Value.Length, ref lenHi, ref lenLow);

            data[0] = Convert.ToByte(Slave);
            data[1] = 0x10;
            data[2] = addrHi;
            data[3] = addrLow;
            data[4] = lenHi;
            data[5] = lenLow;
            data[6] = Convert.ToByte(Value.Length * 2);

            for (int i = 0; i < Value.Length; i++)
            {
                byte valHi = 0, valLow = 0;
                DevideShort(Value[i], ref valHi, ref valLow);
                data[7 + (i * 2)] = valHi;
                data[8 + (i * 2)] = valLow;
            }
            System.Threading.Monitor.Enter(ser);
            GetCRC(data, data.Length - 2, ref crcHi, ref crcLo);
            data[data.Length - 2] = crcHi;
            data[data.Length - 1] = crcLo;
            int ResponseByteCount = 8;
            ser.DiscardInBuffer();
            ser.DiscardOutBuffer();
            ser.Write(data, 0, data.Length);
            ser.BaseStream.Flush();
            int nPrev = GetTickCount();
            int gap = 0;
            int nRecv = 0;
            int nLen = 0;

            while (nRecv < ResponseByteCount)
            {
                try
                {
                    nLen = ser.Read(baResponse, nRecv, ResponseByteCount);
                    nRecv += nLen;
                }
                catch (TimeoutException) { };

                gap = GetTickCount() - nPrev;
                if (gap >= Timeout)
                {
                    break;
                }

                if (nRecv == ResponseByteCount)
                {
                    break;
                }
            }
            if (gap < Timeout)
            {
                byte rcrcHi = 0, rcrcLo = 0;
                GetCRC(baResponse, nRecv - 2, ref rcrcHi, ref rcrcLo);
                if (rcrcHi == baResponse[nRecv - 2] && rcrcLo == baResponse[nRecv - 1])
                {
                    System.Threading.Monitor.Exit(ser);
                }
                else
                {
                    throw new CRCErrorException();
                }
            }
            else
            {
                throw new TimeoutException();
            }
            Value = null;
            data = null;
        }
        public void DevideShort(int value, ref byte high, ref byte low)
        {
            high = (byte)((value & 0xFF00) >> 8);
            low = (byte)(value & 0xFF);
        }
        #endregion
    }
}
