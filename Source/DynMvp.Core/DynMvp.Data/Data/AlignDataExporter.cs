using DynMvp.Base;
using DynMvp.Data.UI;
using DynMvp.Devices.Comm;
using DynMvp.InspectData;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace DynMvp.Data
{
    public class AlignDataInterfaceInfo
    {
        public int OffsetXAddress1 { get; set; }
        public int OffsetYAddress1 { get; set; }
        public int AngleAddress { get; set; }
        public int OffsetXAddress2 { get; set; }
        public int OffsetYAddress2 { get; set; }
        public float XAxisCalibration { get; set; } = 1.0f;
        public float YAxisCalibration { get; set; } = 1.0f;
        public float RAxisCalibration { get; set; } = 1.0f;

        public void Save(XmlElement configElement, string sectionName)
        {
            XmlElement modbusAlignerElement = configElement.OwnerDocument.CreateElement("", sectionName, "");
            configElement.AppendChild(modbusAlignerElement);

            XmlHelper.SetValue(modbusAlignerElement, "OffsetXAddress1", OffsetXAddress1.ToString());
            XmlHelper.SetValue(modbusAlignerElement, "OffsetYAddress1", OffsetYAddress1.ToString());
            XmlHelper.SetValue(modbusAlignerElement, "AngleAddress", AngleAddress.ToString());
            XmlHelper.SetValue(modbusAlignerElement, "OffsetXAddress2", OffsetXAddress2.ToString());
            XmlHelper.SetValue(modbusAlignerElement, "OffsetYAddress2", OffsetYAddress2.ToString());

            XmlHelper.SetValue(modbusAlignerElement, "XAxisCalibration", XAxisCalibration.ToString());
            XmlHelper.SetValue(modbusAlignerElement, "YAxisCalibration", YAxisCalibration.ToString());
            XmlHelper.SetValue(modbusAlignerElement, "RAxisCalibration", RAxisCalibration.ToString());
        }

        public void Load(XmlElement configElement, string sectionName)
        {
            XmlElement modbusAlignerElement = configElement[sectionName];
            if (modbusAlignerElement == null)
            {
                return;
            }

            OffsetXAddress1 = Convert.ToInt32(XmlHelper.GetValue(modbusAlignerElement, "OffsetXAddress1", "100"));
            OffsetYAddress1 = Convert.ToInt32(XmlHelper.GetValue(modbusAlignerElement, "OffsetYAddress1", "102"));
            AngleAddress = Convert.ToInt32(XmlHelper.GetValue(modbusAlignerElement, "AngleAddress", "104"));
            OffsetXAddress2 = Convert.ToInt32(XmlHelper.GetValue(modbusAlignerElement, "OffsetXAddress2", "100"));
            OffsetYAddress2 = Convert.ToInt32(XmlHelper.GetValue(modbusAlignerElement, "OffsetYAddress2", "102"));

            XAxisCalibration = Convert.ToSingle(XmlHelper.GetValue(modbusAlignerElement, "XAxisCalibration", "1"));
            YAxisCalibration = Convert.ToSingle(XmlHelper.GetValue(modbusAlignerElement, "YAxisCalibration", "1"));
            RAxisCalibration = Convert.ToSingle(XmlHelper.GetValue(modbusAlignerElement, "RAxisCalibration", "1"));
        }
    }

    public class MelsecConnectionInfo
    {
        public string IpAddress { get; set; } = string.Empty;
        public int Port { get; set; } = 2005;
        public int StationNumber { get; set; } = 5;
        public int PcStatusAddress { get; set; }
        public int PlcStatusAddress { get; set; }

        public void Save(XmlElement configElement, string sectionName)
        {
            XmlElement melsecElement = configElement.OwnerDocument.CreateElement("", sectionName, "");
            configElement.AppendChild(melsecElement);

            XmlHelper.SetValue(melsecElement, "IpAddress", IpAddress);
            XmlHelper.SetValue(melsecElement, "Port", Port.ToString());

            XmlHelper.SetValue(melsecElement, "PcStatusAddress", PcStatusAddress.ToString());
            XmlHelper.SetValue(melsecElement, "PlcStatusAddress", PlcStatusAddress.ToString());

            XmlHelper.SetValue(melsecElement, "StationNumber", StationNumber.ToString());
        }

        public void Load(XmlElement configElement, string sectionName)
        {
            XmlElement melsecElement = configElement[sectionName];
            if (melsecElement == null)
            {
                return;
            }

            IpAddress = XmlHelper.GetValue(melsecElement, "IpAddress", "");
            Port = Convert.ToInt32(XmlHelper.GetValue(melsecElement, "Port", "2005"));

            PcStatusAddress = Convert.ToInt32(XmlHelper.GetValue(melsecElement, "PcStatusAddress", "110"));
            PlcStatusAddress = Convert.ToInt32(XmlHelper.GetValue(melsecElement, "PlcStatusAddress", "111"));

            StationNumber = Convert.ToInt32(XmlHelper.GetValue(melsecElement, "StationNumber", "1"));
        }
    }

    public class MelsecDataExporter : IDataExporter
    {
        private bool plcConnected;
        private MelsecConnectionInfo melsecConnectionInfo;
        private AxActUtlTypeLib.AxActUtlType melsecUtil = null;

        protected AlignDataInterfaceInfo alignDataInterfaceInfo;
        public AlignDataInterfaceInfo AlignDataInterfaceInfo
        {
            get => alignDataInterfaceInfo;
            set => alignDataInterfaceInfo = value;
        }

        public MelsecDataExporter(MelsecConnectionInfo melsecConnectionInfo)
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(MelsecConnectionInfoForm));

            melsecUtil = new AxActUtlTypeLib.AxActUtlType();

            melsecUtil.Enabled = true;
            melsecUtil.Name = "melsecUtil";
            melsecUtil.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axActUtl.OcxState")));

            this.melsecConnectionInfo = melsecConnectionInfo;

            ConnectPlc();
        }

        public void ConnectPlc()
        {
            int iReturnCode = 0;
            try
            {
                melsecUtil.ActLogicalStationNumber = melsecConnectionInfo.StationNumber;
                iReturnCode = melsecUtil.Open();
            }
            catch (Exception exception)
            {
                //MessageBox.Show(exception.Message, Name,MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogHelper.Debug(LoggerType.Comm, exception.Message);
                plcConnected = false;
                return;
            }
            if (iReturnCode == 0)
            {
                LogHelper.Debug(LoggerType.Comm, string.Format("PLC Connect Success : 0x{0:x8}", iReturnCode));
                plcConnected = true;
            }
            else
            {
                LogHelper.Debug(LoggerType.Comm, string.Format("PLC Connect ERROR : 0x{0:x8}", iReturnCode));
                plcConnected = false;
            }
        }

        public void Export(ProductResult productResult)
        {
            if (plcConnected == false)
            {
                return;
            }

            if (alignDataInterfaceInfo != null)
            {
                int iReturnCode = 0;
                short[] arrDeviceValue = new short[6];		    //Data for 'DeviceValue
                try
                {
                    string szDeviceName = string.Format("D{0}\nD{1}\nD{2}\nD{3}\nD{4}\nD{5}", alignDataInterfaceInfo.OffsetXAddress1, alignDataInterfaceInfo.OffsetXAddress1 + 1,
                                    alignDataInterfaceInfo.OffsetYAddress1, alignDataInterfaceInfo.OffsetYAddress1 + 1, alignDataInterfaceInfo.AngleAddress, alignDataInterfaceInfo.AngleAddress + 1);

                    int alignmentX = (int)(Convert.ToSingle(productResult.GetAdditionalValue("AlignmentX")) * alignDataInterfaceInfo.XAxisCalibration);
                    int alignmentY = (int)(Convert.ToSingle(productResult.GetAdditionalValue("AlignmentY")) * alignDataInterfaceInfo.YAxisCalibration);
                    int alignmentAngle = (int)(Convert.ToSingle(productResult.GetAdditionalValue("AlignmentAngle")) * alignDataInterfaceInfo.RAxisCalibration);

                    arrDeviceValue[0] = (short)(alignmentX & 0xffff);
                    arrDeviceValue[1] = (short)((alignmentX >> 16) & 0xffff);
                    arrDeviceValue[2] = (short)(alignmentY & 0xffff);
                    arrDeviceValue[3] = (short)((alignmentY >> 16) & 0xffff);
                    arrDeviceValue[4] = (short)(alignmentAngle & 0xffff);
                    arrDeviceValue[5] = (short)((alignmentAngle >> 16) & 0xffff);

                    iReturnCode = melsecUtil.WriteDeviceRandom2(szDeviceName, 6, ref arrDeviceValue[0]);

                    if (iReturnCode == 0)
                    {
                        LogHelper.Debug(LoggerType.Comm, string.Format("Write Alignment Data : {0}, {1}, {2}, {3}, {4}, {5}", arrDeviceValue[0], arrDeviceValue[1], arrDeviceValue[2], arrDeviceValue[3], arrDeviceValue[4], arrDeviceValue[5]));
                    }
                    else
                    {
                        LogHelper.Debug(LoggerType.Comm, string.Format("PLC ERROR : 0x{0:x8}", iReturnCode));
                    }
                }
                catch (Exception exception)
                {
                    LogHelper.Debug(LoggerType.Comm, exception.Message);
                    return;
                }
            }
        }
    }

    public class MelsecAlignDataExporter : IDataExporter
    {
        //ClientSocket socket1;
        //ClientSocket socket2;

        //MelsecQAsciiProtocol readProtocol;
        //MelsecQAsciiProtocol writeProtocol;

        //bool plcConnected;
        private MelsecConnectionInfo melsecConnectionInfo;
        private AxActUtlTypeLib.AxActUtlType melsecUtil = null;

        protected AlignDataInterfaceInfo alignDataInterfaceInfo;
        public AlignDataInterfaceInfo AlignDataInterfaceInfo
        {
            get => alignDataInterfaceInfo;
            set => alignDataInterfaceInfo = value;
        }

        public MelsecAlignDataExporter(MelsecConnectionInfo melsecConnectionInfo)
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(MelsecConnectionInfoForm));

            melsecUtil = new AxActUtlTypeLib.AxActUtlType();

            melsecUtil.Enabled = true;
            melsecUtil.Name = "melsecUtil";
            melsecUtil.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axActUtl.OcxState")));

            this.melsecConnectionInfo = melsecConnectionInfo;
        }

        public void Export(ProductResult productResult)
        {
            //float xd1 = 0, yd1 = 0, xd2 = 0, yd2 = 0;
            //float y_measure = OperationSettings.Instance().YMeasure * 1000;

            //float camCenter_SensorLeft = OperationSettings.Instance().CamCenter_SensorLeft * 1000;
            //float camCenter_SensorRight = OperationSettings.Instance().CamCenter_SensorRight * 1000;
            //float cam_SensorDock = OperationSettings.Instance().Cam_SensorDock * 1000;
            //float camYOffset = OperationSettings.Instance().CamYOffset * 1000;
            //if (OperationSettings.Instance().ProductType == "NG_LOADER")
            //{
            //    xd1 = (camCenter_SensorLeft - cam_SensorDock - xValue_Align2) * -1;
            //    //yd1 = y_measure + yValue_Align2 + 27146;
            //    yd1 = y_measure + yValue_Align2 + camYOffset;

            //    xd2 = camCenter_SensorRight - cam_SensorDock - xValue_Align1;
            //    yd2 = y_measure;
            //}
            //else //BA Align
            //{
            //    xd1 = (camCenter_SensorLeft - cam_SensorDock - xValue_Align1) * -1;
            //    yd1 = y_measure + yValue_Align2 + camYOffset;

            //    xd2 = (camCenter_SensorRight - cam_SensorDock - xValue_Align2);
            //    yd2 = y_measure;
            //}

            //ProbeResult probe1Result = productResult.GetProbeResult("00.00.000", 0);
            //ProbeResult probe2Result = productResult.GetProbeResult("00.01.000", 0);

            //PointF leftProbeCenter = (PointF)(probe1Result.ResultValueList.GetResultValue("RealProbeCenter").Value);
            //leftProbeCenter.X = leftProbeCenter.X * 1000;
            //leftProbeCenter.Y = leftProbeCenter.Y * 1000;

            //PointF rightProbeCenter = (PointF)(probe2Result.ResultValueList.GetResultValue("RealProbeCenter").Value);
            //rightProbeCenter.X = rightProbeCenter.X * 1000;
            //rightProbeCenter.Y = rightProbeCenter.Y * 1000;

            //SizeF leftRealOffset = (SizeF)(probe1Result.ResultValueList.GetResultValue("RealOffset").Value);
            //float leftRealOffsetX = Convert.ToSingle(leftRealOffset.Width * 1000);
            //float leftRealOffsetY = Convert.ToSingle(leftRealOffset.Height * 1000);

            //SizeF rightRealOffset = (SizeF)(probe2Result.ResultValueList.GetResultValue("RealOffset").Value);
            //float rightRealOffsetX = Convert.ToSingle(rightRealOffset.Width * 1000);
            //float rightRealOffsetY = Convert.ToSingle(rightRealOffset.Height * 1000);

            //PointF realCamSize = (PointF)(probe2Result.ResultValueList.GetResultValue("RealCamSize").Value);
            //realCamSize = new PointF(realCamSize.X * 1000, realCamSize.Y * 1000);

            //PointF pd1 = new PointF(xd1, yd1);
            //PointF pd2 = new PointF(xd2, yd2);

            //PointF leftProbeOffset = new PointF(leftProbeCenter.X - (realCamSize.X / 2), leftProbeCenter.Y - (realCamSize.Y / 2));
            //PointF rightProbeOffset = new PointF(rightProbeCenter.X - (realCamSize.X / 2), rightProbeCenter.Y - (realCamSize.Y / 2));

            //PointF pt1 = new PointF(pd1.X + leftProbeOffset.X, pd1.Y - leftProbeOffset.Y);
            //PointF pt2 = new PointF(pd2.X + rightProbeOffset.X, pd2.Y - rightProbeOffset.Y);

            //PointF pf1 = new PointF(pt1.X + leftRealOffsetX, pt1.Y - leftRealOffsetY);
            //PointF pf2 = new PointF(pt2.X + rightRealOffsetX, pt2.Y - rightRealOffsetY);

            //float thetaOrigin = Convert.ToSingle(Math.Atan2(pt2.Y - pt1.Y, pt2.X - pt1.X));
            //float thetaFound = Convert.ToSingle(Math.Atan2(pf2.Y - pf1.Y, pf2.X - pf1.X));
            //offsetAngle = (thetaOrigin - thetaFound);

            //float tblXoffset = OperationSettings.Instance().TblXOffset * 1000;
            //float tblYoffset = OperationSettings.Instance().TblYOffset * 1000;

            ////PointF pc2 = new PointF(-10000, xValue_TBL-50000);
            //PointF pc2 = new PointF(tblXoffset, xValue_TBL + tblYoffset);

            //PointF pr1 = new PointF(
            //    ((pf1.X - pc2.X) * Convert.ToSingle(Math.Cos(offsetAngle))) - ((pf1.Y - pc2.Y) * Convert.ToSingle(Math.Sin(offsetAngle))),
            //    ((pf1.X - pc2.X) * Convert.ToSingle(Math.Sin(offsetAngle))) + ((pf1.Y - pc2.Y) * Convert.ToSingle(Math.Cos(offsetAngle))));
            //pr1 = new PointF(pr1.X + pc2.X, pr1.Y + pc2.Y);

            //offsetAngle = (offsetAngle * 180 / Convert.ToSingle(Math.PI)) * (-1);

            //offsetX = pr1.X - pt1.X;
            //offsetY = pr1.Y - pt1.Y;

            //bool leftGood = Convert.ToBoolean(probe1Result.ResultValueList.GetResultValue("Result").Value);
            //bool rightGood = Convert.ToBoolean(probe2Result.ResultValueList.GetResultValue("Result").Value);

            //if ((leftGood == true) && (rightGood == true))
            //{
            //    Result[0] = false;
            //}
            //else
            //    Result[0] = true;

            ////전송 메커니즘 수정 필요 - X, Y 현재는 반대인데 PLC에서 반대로 되어 있는지 확인 후 아니라면 우리꺼 출력 라벨만 반대로

            //MelsecQSendPacket resultPacket = new MelsecQSendPacket();
            //resultPacket.SetupBlockWriteWord(ADDR_VISION_RESULT);

            //offsetAngle_int = (int)(Math.Round(offsetAngle, 3) * 1000);

            //if (OperationSettings.Instance().ProductType == "NG_LOADER")
            //{
            //    offsetX_int = (int)(Math.Round(offsetX, 0)) * (-1);
            //    offsetY_int = (int)(Math.Round(offsetY, 0)) * (-1);
            //}
            //else
            //{
            //    offsetX_int = (int)(Math.Round(offsetY, 0)) * (-1);
            //    offsetY_int = (int)(Math.Round(offsetX, 0)) * (-1);
            //}

            //resultPacket.Add32BitData(offsetX_int);
            //resultPacket.Add32BitData(offsetY_int);
            //resultPacket.Add32BitData(offsetAngle_int);

            //if (realWrite)
            //{
            //    socket2.SendCommand(resultPacket);
            //}
        }
    }

    public class XgtAlignerDataExporter : IDataExporter
    {
        private SerialPortEx serialPort = null;

        protected AlignDataInterfaceInfo alignDataInterfaceInfo;
        public AlignDataInterfaceInfo AlignDataInterfaceInfo
        {
            get => alignDataInterfaceInfo;
            set => alignDataInterfaceInfo = value;
        }

        public XgtAlignerDataExporter(SerialPortEx serialPort)
        {
            this.serialPort = serialPort;
        }

        private byte[] MakePacket(int address, int value)
        {
            string valueHexStr = value.ToString("X08");
            string packet = string.Format("000WSS0208%RW{0:D5}{1}08%RW{2:D5}{3}0", address, valueHexStr.Substring(4), address + 1, valueHexStr.Substring(0, 4));
            byte[] data = Encoding.ASCII.GetBytes(packet);
            data[0] = 5;
            data[data.Length - 1] = 4;
            return data;
        }

        public void Export(ProductResult productResult)
        {
            if (alignDataInterfaceInfo != null)
            {
                int alignmentX1 = (int)(Convert.ToSingle(productResult.GetAdditionalValue("AlignmentX1")) * alignDataInterfaceInfo.XAxisCalibration);
                int alignmentY1 = (int)(Convert.ToSingle(productResult.GetAdditionalValue("AlignmentY1")) * alignDataInterfaceInfo.YAxisCalibration);
                int alignmentAngle = (int)(Convert.ToSingle(productResult.GetAdditionalValue("AlignmentAngle")) * alignDataInterfaceInfo.RAxisCalibration);
                int alignmentX2 = (int)(Convert.ToSingle(productResult.GetAdditionalValue("AlignmentX2")) * alignDataInterfaceInfo.XAxisCalibration);
                int alignmentY2 = (int)(Convert.ToSingle(productResult.GetAdditionalValue("AlignmentY2")) * alignDataInterfaceInfo.YAxisCalibration);
                if (productResult.IsGood())
                {
                    byte[] data = MakePacket(alignDataInterfaceInfo.OffsetXAddress1, alignmentX1);
                    serialPort.WritePacket(data, 0, data.Length);
                    string dataStr = Encoding.UTF8.GetString(data);
                    LogHelper.Debug(LoggerType.Inspection, string.Format("alignDataInterfaceInfo.OffsetXAddress1 X1 : {0}", dataStr));
                    Thread.Sleep(50);

                    data = MakePacket(alignDataInterfaceInfo.OffsetYAddress1, alignmentY1);
                    serialPort.WritePacket(data, 0, data.Length);
                    dataStr = Encoding.UTF8.GetString(data);
                    LogHelper.Debug(LoggerType.Inspection, string.Format("alignDataInterfaceInfo.OffsetXAddress1  Y1: {0}", dataStr));
                    Thread.Sleep(50);

                    data = MakePacket(alignDataInterfaceInfo.AngleAddress, alignmentAngle);
                    serialPort.WritePacket(data, 0, data.Length);
                    dataStr = Encoding.UTF8.GetString(data);
                    LogHelper.Debug(LoggerType.Inspection, string.Format("alignDataInterfaceInfo.AngleAddress : {0}", dataStr));
                    Thread.Sleep(50);

                    data = MakePacket(alignDataInterfaceInfo.OffsetXAddress2, alignmentX2);
                    serialPort.WritePacket(data, 0, data.Length);
                    dataStr = Encoding.UTF8.GetString(data);
                    LogHelper.Debug(LoggerType.Inspection, string.Format("alignDataInterfaceInfo.OffsetXAddress2 X2 : {0}", dataStr));
                    Thread.Sleep(50);

                    data = MakePacket(alignDataInterfaceInfo.OffsetYAddress2, alignmentY2);
                    serialPort.WritePacket(data, 0, data.Length);
                    dataStr = Encoding.UTF8.GetString(data);
                    LogHelper.Debug(LoggerType.Inspection, string.Format("alignDataInterfaceInfo.OffsetXAddress2 Y2 : {0}", dataStr));
                    Thread.Sleep(50);
                }
            }
        }
    }
}
