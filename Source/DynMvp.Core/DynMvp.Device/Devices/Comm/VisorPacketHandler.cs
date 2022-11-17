using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DynMvp.Devices.Comm
{
    public enum VisorPacketType
    {
        Ascii, Binary
    }

    internal class VisorUtil
    {
        public static float ConvertSingle(byte[] packetContents)
        {
            string valueStr = System.Text.Encoding.Default.GetString(packetContents).TrimStart(new char[] { '0' });
            if (string.IsNullOrEmpty(valueStr))
            {
                return 0;
            }

            return Convert.ToSingle(valueStr);
        }

        public static int ConvertInt(byte[] packetContents)
        {
            string valueStr = System.Text.Encoding.Default.GetString(packetContents).TrimStart(new char[] { '0' });
            if (string.IsNullOrEmpty(valueStr))
            {
                return 0;
            }

            return Convert.ToInt32(valueStr);
        }
    }

    public class VisorDefaultReceivedPacket : ReceivedPacket
    {
        public bool Failed { get; set; }

        public VisorDefaultReceivedPacket(bool failed)
        {
            Failed = failed;
        }
    }

    public class VisorImageReceivedPacket : ReceivedPacket
    {
        public Bitmap Bitmap { get; set; }
    }

    public class VisorImgCommand : PacketParser
    {

        public override byte[] GetRequestPacket()
        {
            return Encoding.ASCII.GetBytes("GIM0");
        }

        public override bool ParsePacket(byte[] packetContents)
        {
            if (packetContents[0] == 'G' && packetContents[1] == 'I' && packetContents[2] == 'M')
            {
                PacketValid = true;
            }
            else
            {
                return false;
            }

            CommandPass = (packetContents[3] == 'P');
            if (CommandPass == false)
            {
                switch (packetContents[4])
                {
                    case (byte)'1': ErrorMessage = "Recorder Off"; break;
                    case (byte)'2': ErrorMessage = "No Matching Image of requested type"; break;
                }
            }

            if (CommandPass == false)
            {
                return true;
            }

            int imageHeight = Convert.ToInt32(System.Text.Encoding.Default.GetString(packetContents.Skip(7).Take(4).ToArray()));
            int imageWidth = Convert.ToInt32(System.Text.Encoding.Default.GetString(packetContents.Skip(11).Take(4).ToArray()));

            int packetSize = packetContents.Count();
            if (imageWidth * imageHeight > packetSize - 15)
            {
                return false;
            }

            var receivedPacket = new VisorImageReceivedPacket();
            receivedPacket.Bitmap = ImageHelper.CreateBitmap(imageWidth, imageHeight, imageWidth, 1, packetContents.Skip(15).ToArray());

            DataReceived?.Invoke(receivedPacket);

            return true;
        }

        public override byte[] EncodePacket(string protocol)
        {
            return Encoding.Default.GetBytes(protocol);
        }
        public override string DecodePacket(byte[] packet)
        {
            return Encoding.Default.GetString(packet);
        }
    }

    public class VisorTriggerCommand : PacketParser
    {
        public override byte[] GetRequestPacket()
        {
            return Encoding.ASCII.GetBytes("TRG");
        }

        public override bool ParsePacket(byte[] packetContents)
        {
            packetContents.ToArray();

            if (packetContents.Count() < 4)
            {
                return false;
            }

            if (packetContents[0] == 'T' && packetContents[1] == 'R' && packetContents[2] == 'G')
            {
                PacketValid = true;
            }
            else
            {
                return true;
            }

            VisorDefaultReceivedPacket receivedPacket;

            CommandPass = (packetContents[3] == 'P');
            receivedPacket = new VisorDefaultReceivedPacket(CommandPass == false);

            DataReceived?.Invoke(receivedPacket);

            return true;
        }

        public override byte[] EncodePacket(string protocol)
        {
            return Encoding.Default.GetBytes(protocol);
        }

        public override string DecodePacket(byte[] packet)
        {
            return Encoding.Default.GetString(packet);
        }
    }

    public class VisorGetRoiReceivedPacket : ReceivedPacket
    {
        public int DetectorNo { get; }
        public int RoiIndex { get; }
        public int RoiShape { get; }
        public float CenterX { get; }
        public float CenterY { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Angle { get; set; }
        public bool Failed { get; set; }


        public VisorGetRoiReceivedPacket()
        {
            Failed = true;
        }

        public VisorGetRoiReceivedPacket(int detectorNo, int roiIndex, int roiShape, float centerX, float centerY, float width, float height, float angle)
        {
            Failed = false;

            DetectorNo = detectorNo;
            RoiIndex = roiIndex;
            RoiShape = roiShape;
            CenterX = centerX;
            CenterY = centerY;
            Width = width;
            Height = height;
            Angle = angle;
        }

        public Figure GetFigure(bool result = true)
        {
            Color color = Color.Green;
            if (result == false)
            {
                color = Color.Red;
            }

            switch (RoiShape)
            {
                case 1:
                    Height = Width;
                    return new EllipseFigure(new RectangleF(CenterX - Width, CenterY - Height, Width * 2, Height * 2), new Pen(color, 2));
                case 3:
                    return new EllipseFigure(new RectangleF(CenterX - Width, CenterY - Height, Width * 2, Height * 2), new Pen(color, 2));
                default:
                case 2:
                    return new RectangleFigure(new RotatedRect(CenterX - Width, CenterY - Height, Width * 2, Height * 2, Angle), new Pen(color, 2));
            }
        }
    }

    public class VisorGetRoiCommand : PacketParser
    {
        private int detectorNo;
        private int roiIndex;

        public VisorGetRoiCommand(int detectorNo, int roiIndex)
        {
            this.detectorNo = detectorNo;
            this.roiIndex = roiIndex;
        }

        public override byte[] GetRequestPacket()
        {
            string packet = string.Format("GRI{0:000}{1:00}", detectorNo, roiIndex);
            return Encoding.ASCII.GetBytes(packet);
        }

        public override bool ParsePacket(byte[] packetContents)
        {
            LogHelper.Debug(LoggerType.Comm, "ROI ParsePacket");

            packetContents.ToArray();

            if (packetContents.Count() < 4)
            {
                LogHelper.Debug(LoggerType.Comm, "ROI ParsePacket : Too Short");
                return false;
            }

            if (packetContents[0] == 'G' && packetContents[1] == 'R' && packetContents[2] == 'I')
            {
                PacketValid = true;
            }
            else
            {
                return true;
            }

            VisorGetRoiReceivedPacket receivedPacket;

            CommandPass = (packetContents[3] == 'P');
            if (CommandPass == false)
            {
                LogHelper.Debug(LoggerType.Comm, "ROI Received Packet : Fail");
                receivedPacket = new VisorGetRoiReceivedPacket();
            }
            else
            {
                //if (packetContents.Count() < 59)
                // false;

                string packetContentString = System.Text.Encoding.Default.GetString(packetContents);
                LogHelper.Debug(LoggerType.Comm, "ROI Received Packet : " + packetContentString);

                int detectorNo = VisorUtil.ConvertInt(packetContents.Skip(12).Take(3).ToArray());
                int roiIndex = VisorUtil.ConvertInt(packetContents.Skip(15).Take(2).ToArray());
                int roiShape = VisorUtil.ConvertInt(packetContents.Skip(17).Take(2).ToArray());

                float centerX = VisorUtil.ConvertSingle(packetContents.Skip(19).Take(8).ToArray()) / 1000;
                float centerY = VisorUtil.ConvertSingle(packetContents.Skip(27).Take(8).ToArray()) / 1000;
                float width = VisorUtil.ConvertSingle(packetContents.Skip(35).Take(8).ToArray()) / 1000;
                float height = VisorUtil.ConvertSingle(packetContents.Skip(43).Take(8).ToArray()) / 1000;

                float angle = 0.0F;
                if (roiShape == 2)
                {
                    angle = VisorUtil.ConvertSingle(packetContents.Skip(51).Take(8).ToArray()) / 1000;
                }

                receivedPacket = new VisorGetRoiReceivedPacket(detectorNo, roiIndex, roiShape, centerX, centerY, width, height, angle);
            }

            DataReceived?.Invoke(receivedPacket);

            return true;
        }

        public override byte[] EncodePacket(string protocol)
        {
            return Encoding.Default.GetBytes(protocol);
        }
        public override string DecodePacket(byte[] packet)
        {
            return Encoding.Default.GetString(packet);
        }
    }

    public class VisorJobChangeOverCommand : PacketParser
    {
        private int jobNumber;

        public VisorJobChangeOverCommand(int jobNumber)
        {
            this.jobNumber = jobNumber;
        }

        public override byte[] GetRequestPacket()
        {
            string packet = string.Format("CJB{0:000}", jobNumber);
            return Encoding.ASCII.GetBytes(packet);
        }

        public override bool ParsePacket(byte[] packetContents)
        {
            packetContents.ToArray();

            if (packetContents.Count() < 4)
            {
                return false;
            }

            if (packetContents[0] == 'C' && packetContents[1] == 'J' && packetContents[2] == 'B')
            {
                PacketValid = true;
            }
            else
            {
                return true;
            }

            VisorDefaultReceivedPacket receivedPacket;

            CommandPass = (packetContents[3] == 'P');
            receivedPacket = new VisorDefaultReceivedPacket(CommandPass == false);

            DataReceived?.Invoke(receivedPacket);

            return true;
        }

        public override byte[] EncodePacket(string protocol)
        {
            return Encoding.Default.GetBytes(protocol);
        }
        public override string DecodePacket(byte[] packet)
        {
            return Encoding.Default.GetString(packet);
        }
    }

    public class VisorListeningParser : PacketParser
    {
        public override byte[] GetRequestPacket()
        {
            return null;
        }

        public override bool ParsePacket(byte[] packetContents)
        {
            string contentsString = System.Text.Encoding.Default.GetString(packetContents.ToArray());

            var receivedPacket = new ReceivedPacket();
            receivedPacket.ReceivedData = contentsString;

            DataReceived?.Invoke(receivedPacket);

            return true;
        }

        public override byte[] EncodePacket(string protocol)
        {
            return Encoding.Default.GetBytes(protocol);
        }
        public override string DecodePacket(byte[] packet)
        {
            return Encoding.Default.GetString(packet);
        }
    }

    public class VisorGetShutterCommand : PacketParser
    {
        public override byte[] GetRequestPacket()
        {
            return Encoding.ASCII.GetBytes("GSH");
        }

        public override bool ParsePacket(byte[] packetContents)
        {
            if (packetContents[0] == 'G' && packetContents[1] == 'S' && packetContents[2] == 'H')
            {
                PacketValid = true;
            }
            else
            {
                return false;
            }

            CommandPass = (packetContents[3] == 'P');
            if (CommandPass == false)
            {
                LogHelper.Debug(LoggerType.Comm, "GetShutter Received Packet : Fail");
                return true;
            }

            string contentsString = System.Text.Encoding.Default.GetString(packetContents.ToArray());

            var receivedPacket = new ReceivedPacket();
            receivedPacket.ReceivedData = contentsString;

            DataReceived?.Invoke(receivedPacket);

            return true;
        }

        public override byte[] EncodePacket(string protocol)
        {
            return Encoding.Default.GetBytes(protocol);
        }
        public override string DecodePacket(byte[] packet)
        {
            return Encoding.Default.GetString(packet);
        }
    }

    /*
    public class VisorPacketHandler : PacketHandler
    {
        private static VisorPacketType packetType = VisorPacketType.Ascii;
        public static VisorPacketType PacketType
        {
            get { return VisorPacketHandler.packetType; }
            set { VisorPacketHandler.packetType = value; }
        }

        public VisorPacketHandler()
        {

        }
    }
    */
}
