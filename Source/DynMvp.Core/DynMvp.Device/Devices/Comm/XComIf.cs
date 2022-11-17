using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Globalization;

using XComPro.Library;

namespace FrameGap
{
    public class XComIf
    {
        public bool SECSCommunicationOK { get; private set; } = false;

        private XComProW XComPro = new XComProW();
        public short DeviceId { get; set; } = 1;

        private int systemByte_mine;
        public bool Initialize(string configPath)
        {
            XComPro.OnSecsEvent += new ON_SECSEVENT(XComPro_OnSecsEvent);
            XComPro.OnSecsMsg += new ON_SECSMSG(XComPro_OnSecsMsg);
            int ret = XComPro.Initialize(configPath);
            if (ret < 0)
            {
                LogHelper.Error(string.Format("XComPro initialization failed: XComPro error={0}", ret));
                return false;
            }

            ret = XComPro.Start();
            if (ret < 0)
            {
                LogHelper.Error(string.Format("XComPro start failed: error={0}", ret));
                return false;
            }

            return true;
        }

        private void XComPro_OnSecsMsg()
        {
            int lMsgId = 0, lSysbyte = 0;
            short nStream = 0, nFunc = 0, nDeviceID = 0, nWbit = 0;
            int nReturn = 0;


            while (XComPro.LoadSecsMsg(ref lMsgId, ref nDeviceID, ref nStream, ref nFunc, ref lSysbyte, ref nWbit) >= 0)
            {
                LogHelper.Debug(LoggerType.Comm, string.Format("Received S{0}F{1}, Sysbyte={2:X8}", nStream, nFunc, lSysbyte));
                if (nStream == 1 && nFunc == 1)     // Are you online?
                {
                    XComPro.CloseSecsMsg(lMsgId);

                    if (nWbit != 0)
                    {
                        ReplyOnlineData(nDeviceID, "FrameGap", "1.0", lSysbyte);
                    }
                }
                else if (nStream == 1 && nFunc == 17) // Request ON-LINE
                {
                    XComPro.CloseSecsMsg(lMsgId);

                    if (nWbit != 0)
                    {
                        ReplyOnlineACK(nDeviceID, lSysbyte);
                    }
                }
                else if (nStream == 2 && nFunc == 31)   // Time
                {
                    XComPro.CloseSecsMsg(lMsgId);

                    string sBuff = "";
                    int nLen = 0;
                    nReturn = XComPro.GetAsciiItem(lMsgId, ref sBuff, ref nLen); LogHelper.Debug(LoggerType.Comm, string.Format("     ASCII {0}: next={1}", sBuff, nReturn));
                    if (nReturn >= 0)
                    {
                        if (DateTime.TryParseExact(sBuff, "yyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime newTime))
                        {
                            EtcHelper.SetSystemTime(newTime);
                        }
                    }

                    if (nWbit != 0)
                    {
                        ReplyTimeACK(nDeviceID, lSysbyte);
                    }
                }
                else if (nStream == 6 && nFunc == 12)
                {
                    XComPro.CloseSecsMsg(lMsgId);
                }
                else
                {
                    LogHelper.Error(string.Format("Undefined message received (S{0}F{1})", nStream, nFunc));
                    XComPro.CloseSecsMsg(lMsgId);
                    return;
                }
            }
        }

        private void XComPro_OnSecsEvent(short nEventId, int lParam)
        {
            switch (nEventId)
            {
                case 101:
                    LogHelper.Debug(LoggerType.Comm, "[EVENT] HSMS NOT CONNECTED");
                    SECSCommunicationOK = false;
                    break;
                case 102:
                    LogHelper.Debug(LoggerType.Comm, "[EVENT] HSMS NOT SELECTED");
                    SECSCommunicationOK = false;
                    break;
                case 103:
                    LogHelper.Debug(LoggerType.Comm, "[EVENT] HSMS SELECTED");
                    SECSCommunicationOK = true;
                    break;
                default:
                    LogHelper.Debug(LoggerType.Comm, string.Format("[EVENT] Other event: eventId = {0}", nEventId));
                    break;
            }
        }

        public void ReplyOnlineData(short deviceId, string modelType, string swRev, int systemByte) // S1F2
        {
            int lMsgId = 0;
            systemByte_mine = systemByte;
            int ret = XComPro.MakeSecsMsg(ref lMsgId, deviceId, 1, 2, systemByte);
            if (ret < 0)
            {
                LogHelper.Error(string.Format("MakeSecsMsg failed: error={0}", ret));
                return;
            }

            ret = XComPro.SetListItem(lMsgId, 2); LogHelper.Debug(LoggerType.Comm, string.Format("LIST {0}: error={1}", 0, ret));
            ret = XComPro.SetAsciiItem(lMsgId, "0"); LogHelper.Debug(LoggerType.Comm, string.Format("   ASCII {0}: error={1}", modelType, ret));
            ret = XComPro.SetAsciiItem(lMsgId, "0"); LogHelper.Debug(LoggerType.Comm, string.Format("   ASCII {0}: error={1}", modelType, ret));
            //ret = XComPro.SetAsciiItem(lMsgId, modelType);  LogHelper.Debug(LoggerType.Comm, String.Format("   ASCII {0}: error={1}", modelType, ret));
            //ret = XComPro.SetAsciiItem(lMsgId, swRev);      LogHelper.Debug(LoggerType.Comm, String.Format("   ASCII {0}: error={1}", swRev, ret));

            ret = XComPro.Send(lMsgId);
            if (ret < 0)
            {
                LogHelper.Error(string.Format("Failed to send S1F2(Online Data) : error={0}", ret));
            }
            else
            {
                LogHelper.Debug(LoggerType.Comm, "Reply S1F2(Online Data) was sent successfully");
            }
        }

        public void ReplyOnlineACK(short deviceId, int systemByte) // S1F18
        {
            int lMsgId = 0;
            systemByte_mine = systemByte;
            int ret = XComPro.MakeSecsMsg(ref lMsgId, deviceId, 1, 18, systemByte);
            if (ret < 0)
            {
                LogHelper.Error(string.Format("MakeSecsMsg failed: error={0}", ret));
                return;
            }
            //ret = XComPro.SetListItem(lMsgId, 1);
            ret = XComPro.SetBinaryItem(lMsgId, 18);
            //ret = XComPro.SetAsciiItem(lMsgId, "0");
            //LogHelper.Debug(LoggerType.Comm, String.Format("   ASCII {0}: error={1}", modelType, ret));
            //ret = XComPro.SetBoolItem(lMsgId, false); LogHelper.Debug(LoggerType.Comm, String.Format("   Boolean {0}: error={1}", false, ret));

            ret = XComPro.Send(lMsgId);
            if (ret < 0)
            {
                LogHelper.Error(string.Format("Failed to send S1F18(Online ACK) : error={0}", ret));
            }
            else
            {
                LogHelper.Debug(LoggerType.Comm, "Reply S1F18(Online ACK) was sent successfully");
            }
        }

        public void ReplyTimeACK(short deviceId, int systemByte) // S2F32
        {
            int lMsgId = 0;
            systemByte_mine = systemByte;
            int ret = XComPro.MakeSecsMsg(ref lMsgId, deviceId, 2, 32, systemByte);
            if (ret < 0)
            {
                LogHelper.Error(string.Format("MakeSecsMsg failed: error={0}", ret));
                return;
            }

            //ret = XComPro.SetBoolItem(lMsgId, false); LogHelper.Debug(LoggerType.Comm, String.Format("   Boolean {0}: error={1}", false, ret));
            ret = XComPro.SetBinaryItem(lMsgId, 0);

            ret = XComPro.Send(lMsgId);
            if (ret < 0)
            {
                LogHelper.Error(string.Format("Failed to send S2F32(Time ACK) : error={0}", ret));
            }
            else
            {
                LogHelper.Debug(LoggerType.Comm, "Reply S2F32(Time ACK) was sent successfully");
            }
        }

        public void SendOnline()
        {
            SendOnlineEventReport(1, 103, 101, true);
        }

        public void SendOffline()
        {
            SendOnlineEventReport(1, 101, 101, false);
        }

        public void SendOnlineEventReport(uint dataId, uint eventId, uint reportId, bool value) // S6F11
        {
            int lMsgId = 0;
            systemByte_mine++;
            int ret = XComPro.MakeSecsMsg(ref lMsgId, DeviceId, 6, 11, systemByte_mine);
            if (ret < 0)
            {
                LogHelper.Error(string.Format("MakeSecsMsg failed: error={0}", ret));
                return;
            }

            string valueStr = (value == true ? "R" : "F");
            ret = XComPro.SetListItem(lMsgId, 3); LogHelper.Debug(LoggerType.Comm, string.Format("LIST {0}: error={1}", 3, ret));
            ret = XComPro.SetU4Item(lMsgId, dataId); LogHelper.Debug(LoggerType.Comm, string.Format("   U4 {0}: error={1}", dataId, ret));
            ret = XComPro.SetU4Item(lMsgId, eventId); LogHelper.Debug(LoggerType.Comm, string.Format("   U4 {0}: error={1}", eventId, ret));
            ret = XComPro.SetListItem(lMsgId, 1); LogHelper.Debug(LoggerType.Comm, string.Format("LIST {0}: error={1}", 1, ret));
            ret = XComPro.SetListItem(lMsgId, 2); LogHelper.Debug(LoggerType.Comm, string.Format("LIST {0}: error={1}", 2, ret));
            ret = XComPro.SetU4Item(lMsgId, reportId); LogHelper.Debug(LoggerType.Comm, string.Format("   U4 {0}: error={1}", dataId, ret));
            ret = XComPro.SetListItem(lMsgId, 1); LogHelper.Debug(LoggerType.Comm, string.Format("LIST {0}: error={1}", 2, ret));
            ret = XComPro.SetAsciiItem(lMsgId, valueStr); LogHelper.Debug(LoggerType.Comm, string.Format("   ASCII {0}: error={1}", valueStr, ret));

            ret = XComPro.Send(lMsgId);
            if (ret < 0)
            {
                LogHelper.Error(string.Format("Failed to send S6F11(Online Event) : error={0}", ret));
            }
            else
            {
                LogHelper.Debug(LoggerType.Comm, "Send S6F11(Online Event) was sent successfully");
            }
        }

        public class CornerResult
        {
            public bool GapJudge { get; }
            public string GapJudgeStr => (GapJudge ? "OK" : "NG");
            public float GapValue { get; }
            public bool AlignJudge { get; }
            public string AlignJudgeStr => (AlignJudge ? "OK" : "NG");
            public float AlignValue { get; }

            public CornerResult(bool gapJudge, float gapValue, bool alignJudge, float alignValue)
            {
                GapJudge = gapJudge;
                GapValue = gapValue;
                AlignJudge = alignJudge;
                AlignValue = alignValue;
            }
        }

        public class InspResult
        {
            public ushort UnitId { get; }
            public string MaterialId { get; }
            public bool Judge { get; }
            public string JudgeStr => (Judge ? "OK" : "NG");
            public DateTime InspTime { get; }
            public string InspTimeStr => InspTime.ToString("yyyyMMddHHmmss");
            public string FileName { get; }
            public List<CornerResult> CornerResultList { get; } = new List<CornerResult>();

            public InspResult(ushort unitId, string materialId, bool judge, DateTime inspTime, string fileName)
            {
                UnitId = unitId;
                MaterialId = materialId;
                Judge = judge;
                InspTime = inspTime;
                FileName = fileName;
            }

            public void AddCornerResult(CornerResult cornerResult)
            {
                CornerResultList.Add(cornerResult);
            }
        }

        public void SendInspResult(InspResult result) // S6F11
        {
            SendInspResult(1, 4004, 101, result);
        }

        public void SendInspResult(uint dataId, uint eventId, uint reportId, InspResult result) // S6F11
        {
            int lMsgId = 0;
            systemByte_mine++;
            int ret = XComPro.MakeSecsMsg(ref lMsgId, DeviceId, 6, 11, systemByte_mine);
            if (ret < 0)
            {
                LogHelper.Error(string.Format("MakeSecsMsg failed: error={0}", ret));
                return;
            }

            ret = XComPro.SetListItem(lMsgId, 3); LogHelper.Debug(LoggerType.Comm, string.Format("LIST {0}: error={1}", 3, ret));
            ret = XComPro.SetU4Item(lMsgId, dataId); LogHelper.Debug(LoggerType.Comm, string.Format("    U4 {0}: error={1}", dataId, ret));
            ret = XComPro.SetU4Item(lMsgId, eventId); LogHelper.Debug(LoggerType.Comm, string.Format("    U4 {0}: error={1}", eventId, ret));
            ret = XComPro.SetListItem(lMsgId, 1); LogHelper.Debug(LoggerType.Comm, string.Format("    LIST {0}: error={1}", 1, ret));
            ret = XComPro.SetListItem(lMsgId, 2); LogHelper.Debug(LoggerType.Comm, string.Format("        LIST {0}: error={1}", 2, ret));
            ret = XComPro.SetU4Item(lMsgId, reportId); LogHelper.Debug(LoggerType.Comm, string.Format("            U4 {0}: error={1}", reportId, ret));
            ret = XComPro.SetListItem(lMsgId, 5); LogHelper.Debug(LoggerType.Comm, string.Format("            LIST {0}: error={1}", 5, ret));
            ret = XComPro.SetU2Item(lMsgId, result.UnitId); LogHelper.Debug(LoggerType.Comm, string.Format("                U4 {0}: error={1}", result.UnitId.ToString(), ret));
            ret = XComPro.SetAsciiItem(lMsgId, result.MaterialId); LogHelper.Debug(LoggerType.Comm, string.Format("                ASCII {0}: error={1}", result.MaterialId, ret));
            ret = XComPro.SetAsciiItem(lMsgId, result.JudgeStr); LogHelper.Debug(LoggerType.Comm, string.Format("                ASCII {0}: error={1}", result.JudgeStr, ret));
            ret = XComPro.SetAsciiItem(lMsgId, result.InspTimeStr); LogHelper.Debug(LoggerType.Comm, string.Format("                ASCII {0}: error={1}", result.InspTimeStr, ret));
            ret = XComPro.SetListItem(lMsgId, 17); LogHelper.Debug(LoggerType.Comm, string.Format("                LIST {0}: error={1}", 17, ret));

            int index = 1;
            foreach (CornerResult cornerResult in result.CornerResultList)
            {
                ret = XComPro.SetListItem(lMsgId, 2); LogHelper.Debug(LoggerType.Comm, string.Format("                    LIST {0}: error={1}", 2, ret));
                ret = XComPro.SetAsciiItem(lMsgId, string.Format("POS{0}_GAP_JUDGE", index)); LogHelper.Debug(LoggerType.Comm, string.Format("                        ASCII {0}: error={1}", string.Format("POS{0}_GAP_JUDGE", index), ret));
                ret = XComPro.SetAsciiItem(lMsgId, cornerResult.GapJudgeStr); LogHelper.Debug(LoggerType.Comm, string.Format("                        ASCII {0}: error={1}", cornerResult.GapJudgeStr, ret));
                ret = XComPro.SetListItem(lMsgId, 2); LogHelper.Debug(LoggerType.Comm, string.Format("                    LIST {0}: error={1}", 2, ret));
                ret = XComPro.SetAsciiItem(lMsgId, string.Format("POS{0}_GAP_VALUE", index)); LogHelper.Debug(LoggerType.Comm, string.Format("                        ASCII {0}: error={1}", string.Format("POS{0}_GAP_VALUE", index), ret));
                ret = XComPro.SetAsciiItem(lMsgId, cornerResult.GapValue.ToString()); LogHelper.Debug(LoggerType.Comm, string.Format("                        ASCII {0}: error={1}", cornerResult.GapValue.ToString(), ret));
                ret = XComPro.SetListItem(lMsgId, 2); LogHelper.Debug(LoggerType.Comm, string.Format("                    LIST {0}: error={1}", 2, ret));
                ret = XComPro.SetAsciiItem(lMsgId, string.Format("POS{0}_ALIGN_JUDGE", index)); LogHelper.Debug(LoggerType.Comm, string.Format("                        ASCII {0}: error={1}", string.Format("POS{0}_ALIGN_JUDGE", index), ret));
                ret = XComPro.SetAsciiItem(lMsgId, cornerResult.AlignJudgeStr); LogHelper.Debug(LoggerType.Comm, string.Format("                        ASCII {0}: error={1}", cornerResult.AlignJudgeStr, ret));
                ret = XComPro.SetListItem(lMsgId, 2); LogHelper.Debug(LoggerType.Comm, string.Format("                    LIST {0}: error={1}", 2, ret));
                ret = XComPro.SetAsciiItem(lMsgId, string.Format("POS{0}_ALIGN_VALUE", index)); LogHelper.Debug(LoggerType.Comm, string.Format("                        ASCII {0}: error={1}", string.Format("POS{0}_ALIGN_VALUE", index), ret));
                ret = XComPro.SetAsciiItem(lMsgId, cornerResult.AlignValue.ToString()); LogHelper.Debug(LoggerType.Comm, string.Format("                        ASCII {0}: error={1}", cornerResult.AlignValue.ToString(), ret));
                index++;
            }
            ret = XComPro.SetListItem(lMsgId, 2); LogHelper.Debug(LoggerType.Comm, string.Format("                    LIST {0}: error={1}", 2, ret));
            ret = XComPro.SetAsciiItem(lMsgId, "FILENAME"); LogHelper.Debug(LoggerType.Comm, string.Format("                        ASCII {0}: error={1}", "FILENAME", ret));
            ret = XComPro.SetAsciiItem(lMsgId, result.FileName); LogHelper.Debug(LoggerType.Comm, string.Format("                        ASCII {0}: error={1}", result.FileName, ret));

            ret = XComPro.Send(lMsgId);
            if (ret < 0)
            {
                LogHelper.Error(string.Format("Failed to send S6F11(Online Event) : error={0}", ret));
            }
            else
            {
                LogHelper.Debug(LoggerType.Comm, "Send S6F11(Online Event) was sent successfully");
            }
        }
    }
}
