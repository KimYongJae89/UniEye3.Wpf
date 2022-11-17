using DynMvp.Base;
using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace DynMvp.Devices.Comm
{
    public class SerialEncoderInfo : SerialDeviceInfo
    {
        public double Resolution { get; set; } = 7.0;

        public override SerialDevice BuildSerialDevice(bool virtualMode)
        {
            if (virtualMode)
            {
                SerialPortInfo.PortName = "Virtual";
            }

            return SerialEncoder.Create(this);
        }

        public override SerialDeviceInfo Clone()
        {
            var serialEncoderInfo = new SerialEncoderInfo();
            serialEncoderInfo.CopyFrom(this);
            return serialEncoderInfo;
        }

        public override void CopyFrom(SerialDeviceInfo serialDeviceInfo)
        {
            var serialEncoderInfo = (SerialEncoderInfo)serialDeviceInfo;

            base.CopyFrom(serialDeviceInfo);
            Resolution = serialEncoderInfo.Resolution;
        }

        public override void SaveXml(XmlElement xmlElement)
        {
            base.SaveXml(xmlElement);
            XmlHelper.SetValue(xmlElement, "Resolution", Resolution.ToString());
        }

        public override void LoadXml(XmlElement xmlElement)
        {
            base.LoadXml(xmlElement);
            Resolution = Convert.ToDouble(XmlHelper.GetValue(xmlElement, "Resolution", Resolution.ToString()));
        }
    }

    public abstract class SerialEncoder : SerialDevice
    {
        public bool IsInit { get; set; } = false;
        public bool IsOpen => SerialPortEx != null && SerialPortEx.IsOpen;

        public abstract string Version { get; }
        public abstract string[] GetSendCommandString();
        public abstract void StartPulseMonitor();
        public abstract void StopPulseMonitor();

        public SerialEncoder(SerialDeviceInfo deviceInfo) : base(deviceInfo)
        {
        }

        public abstract uint GetPositionPls();
        public abstract double GetSpeedPlsPerMs();

        public abstract bool IsCompatible(string command);
        public bool IsCompatible(Enum command)
        {
            return IsCompatible(command.ToString());
        }

        public static SerialEncoder Create(SerialDeviceInfo deviceInfo)
        {
            if (deviceInfo.IsVirtual)
            {
                return new SerialEncoderVirtual(deviceInfo);
            }

            SerialEncoder serialEncoder = null;
            switch (CheckVersion(deviceInfo))
            {
                default:
                case "1.07":
                    serialEncoder = new SerialEncoderV107(deviceInfo);
                    break;
                case "1.05":
                    serialEncoder = new SerialEncoderV105(deviceInfo);
                    break;
                    //default:
                    //    ErrorManager.Instance().Report((int)ErrorSection.Machine, (int)ErrorSubSection.CommonReason, ErrorLevel.Error, ErrorSection.Machine.ToString(), ErrorSubSection.CommonReason.ToString(), "SerialEncoder Initialize Fail");
                    //    serialEncoder = new SerialEncoderVirtual(deviceInfo);
                    //    break;
            }
            return serialEncoder;
        }

        private static string CheckVersion(SerialDeviceInfo deviceInfo)
        {
            var serialDevice = new SerialDevice(deviceInfo);
            if (serialDevice.Initialize() == false)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Machine, (int)MachineError.Serial,
                    ErrorLevel.Fatal, MachineError.Serial.ToString(), deviceInfo.DeviceName, "Serial Device initialize fail.");
                return null;
            }

            string[] token = serialDevice.ExcuteCommand("VR\r\n");
            serialDevice.Release();

            if (token == null)
            {
                return null;
            }
            else if (token[0] == "ER")
            {
                return "1.05";
            }

            return token[1].Trim();
            //return "1.05";
            //return "1.07";
        }
    }

    public class SerialEncoderV105 : SerialEncoder
    {
        public enum ESendCommand
        {
            AP, CP, GR,     // Send
        }

        public enum EResponseCommand
        {
            DL, PW, DV, FQ, ED, OS, EN, AR, IN, // Response
        }

        public enum ECommand
        {
            AP, CP, GR,     // Send
            DL, PW, DV, FQ, ED, OS, EN, AR, IN, // Response
        }

        /// <summary>
        /// PosDiff per Ms
        /// </summary>
        private double[] speedBuffer = new double[9];
        private int speedBufferIdx = 0;
        private ThreadHandler threadHandler = null;

        public override string Version => "1.05";

        public override Enum GetCommand(string command)
        {
            if (Enum.TryParse<EResponseCommand>(command, out EResponseCommand res))
            {
                return res;
            }

            return null;
        }

        public override bool IsCompatible(string command)
        {
            return Enum.GetNames(typeof(ESendCommand)).Contains(command) || Enum.GetNames(typeof(EResponseCommand)).Contains(command);
        }

        //public bool IsCompatible(Enum command)
        //{
        //    //return command.GetType() == typeof(ECommand);
        //    return IsCompatible(command.ToString());
        //}

        public SerialEncoderV105(SerialDeviceInfo deviceInfo) : base(deviceInfo)
        {
            timeOutTimer = new TimeOutTimer();
        }

        public override bool Initialize()
        {
            bool ok = base.Initialize();
            if (ok)
            {
                string[] token = ExcuteCommand(ESendCommand.GR, "");
                IsInit = token != null && token.Length > 0;
                if (IsInit)
                {
                    //ExcuteCommand(ECommand.DL, "0");
                    //ExcuteCommand(ECommand.PW, "50");
                    //ExcuteCommand(ECommand.IN, "0");
                    //ExcuteCommand(ECommand.ED, "0");
                    //ExcuteCommand(ECommand.DV, "4");
                    //ExcuteCommand(ECommand.CP);
                    //ExcuteCommand(ECommand.EN, "1");
                }
            }
            return ok;
        }

        public override void StartPulseMonitor()
        {
            if (threadHandler != null)
            {
                StopPulseMonitor();
            }

            threadHandler = new ThreadHandler("SerialEncoderV105", new Thread(SpeedMeasureProc));
            threadHandler.Start();
        }

        public override void StopPulseMonitor()
        {
            threadHandler?.Stop(1000);
            threadHandler = null;
        }

        public override void Release()
        {
            StopPulseMonitor();
            base.Release();
        }

        public override string MakePacket(string command, params string[] args)
        {
            var sb = new StringBuilder();
            sb.Append(command.ToString());
            foreach (string arg in args)
            {
                sb.AppendFormat(",{0}", arg);
            }
            return sb.ToString();
        }

        public override PacketParser CreatePacketParser()
        {
            var packetParser = new SimplePacketParser();
            return packetParser;
        }

        public override uint GetPositionPls()
        {
            string[] token = ExcuteCommand(ESendCommand.AP);
            if (token == null || token.Length < 2 || (uint.TryParse(token[1], out uint pos) == false))
            {
                return 0;
            }

            return pos;
        }

        private void SpeedMeasureProc()
        {
            DateTime startTime = DateTime.Now;
            DateTime prevDateTime = DateTime.Now;
            long prevPos = GetPositionPls();
            var commStopwatch = new System.Diagnostics.Stopwatch();

            while (threadHandler.RequestStop == false)
            {
                Thread.Sleep(20);
                long curPos;
                commStopwatch.Restart();
                try
                {
                    curPos = GetPositionPls();
                }
                catch (TimeoutException)
                {
                    continue;
                }
                DateTime curDateTime = DateTime.Now;
                commStopwatch.Stop();
                long commTime = commStopwatch.ElapsedMilliseconds;

                //long posDiff = Math.Abs(curPos - prevPos);
                //if (Math.Abs(posDiff) > uint.MaxValue / 4)    // overflow or underflow
                //{
                //    prevDateTime = curDateTime;
                //    prevPos = curPos;
                //    continue;
                //}
                long posDiff = curPos - prevPos;
                if (posDiff >= 0)
                {
                    double timeDiff = (curDateTime - prevDateTime).TotalMilliseconds;
                    double curVel = posDiff / timeDiff; // [pulse/milisec]
                                                        //LogHelper.Debug(LoggerType.Grab, string.Format("curVelosity: {0} = {1} / {2}, CommTime: {3}", curVel, posDiff, timeDiff, commTime));

                    // Median Filter
                    speedBuffer[speedBufferIdx] = curVel;
                    speedBufferIdx = (speedBufferIdx + 1) % speedBuffer.Count();
                }
                prevDateTime = curDateTime;
                prevPos = curPos;
            }
        }

        private double Median(double[] filterBuffer)
        {
            double[] sortedFilterBuffer = (double[])filterBuffer.Clone();
            Array.Sort(sortedFilterBuffer);

            int halfLen = filterBuffer.Length / 2;
            if ((filterBuffer.Length % 2) == 1)
            {
                return sortedFilterBuffer[halfLen];
            }
            else
            {
                return (sortedFilterBuffer[halfLen] + sortedFilterBuffer[halfLen + 1]) / 2;
            }
        }

        public override double GetSpeedPlsPerMs()
        {
            return Median(speedBuffer);
        }

        public override string[] GetSendCommandString()
        {
            return Enum.GetNames(typeof(ESendCommand));
        }
    }

    public class SerialEncoderVirtual : SerialEncoderV105
    {
        private Dictionary<object, string> dic = new Dictionary<object, string>();
        private ThreadHandler virtualModeThread = null;

        public SerialEncoderVirtual(SerialDeviceInfo deviceInfo) : base(deviceInfo)
        {
            deviceInfo.SerialPortInfo.PortName = "Virtual";

            dic.Add(ESendCommand.AP, "0");
            dic.Add(EResponseCommand.DL, "0");
            dic.Add(EResponseCommand.PW, "0");
            dic.Add(EResponseCommand.DV, "0");
            dic.Add(EResponseCommand.FQ, "0");
            dic.Add(EResponseCommand.ED, "0");
            dic.Add(EResponseCommand.OS, "0");
            dic.Add(EResponseCommand.EN, "0");
            dic.Add(EResponseCommand.AR, "0,1000000");
            dic.Add(EResponseCommand.IN, "0");
        }

        public override bool Initialize()
        {
            bool ok = base.Initialize();
            if (ok)
            {
                virtualModeThread = new ThreadHandler("SerialEncoderVirtual", new Thread(Thread_WorkingThread));
                virtualModeThread.WorkingThread.Start();
            }
            return ok;
        }

        public override void Release()
        {
            base.Release();
            virtualModeThread?.Stop();
            virtualModeThread = null;
        }

        private void Thread_WorkingThread()
        {
            // 80 [m/m]
            // 1.333 [m/s]
            // 7 [um/line]
            // 1142857.142857143 [line/1000ms]
            long virtualCount = 0;
            while (virtualModeThread.RequestStop == false)
            {
                Thread.Sleep(10);

                double step = double.Parse(dic[EResponseCommand.FQ]);
                step /= 100;
                if (step == 0)
                {
                    step = 80.0 / 60.0 / 0.000007 / 100.0;
                }

                if (dic[EResponseCommand.IN] == "1")
                {
                    virtualCount++;
                    dic[ESendCommand.AP] = ((long)(virtualCount * step)).ToString();
                }
            }
        }

        protected override bool SendCommand(string v)
        {
            Task.Factory.StartNew(() =>
            {
                ProcessCommand(v);
                serialPortEx.PacketHandler.PacketParser.DataReceived(CreateReceivedPacket(v));
            });
            return true;
        }

        private void ProcessCommand(string v)
        {
            string[] token = v.Trim().Split(',');
            var command = (ECommand)Enum.Parse(typeof(ECommand), token[0]);

            switch (command)
            {
                case ECommand.CP:
                    dic[ECommand.AP] = "0";
                    break;
                case ECommand.AR:
                    dic[command] = token[1] + "," + token[2];
                    break;
                case ECommand.AP:
                case ECommand.GR:
                    break;
                default:
                    dic[command] = token[1];
                    break;
            }
        }

        private ReceivedPacket CreateReceivedPacket(string wirtePacket)
        {
            var sb = new StringBuilder();
            string[] token = wirtePacket.Trim().Split(',');
            var command = (ESendCommand)Enum.Parse(typeof(ESendCommand), token[0]);
            switch (command)
            {
                case ESendCommand.CP:
                case ESendCommand.AP:
                    sb.AppendLine(string.Format("{0},{1}", ESendCommand.AP, dic[ESendCommand.AP]));
                    break;

                case ESendCommand.GR:
                    sb.Append(string.Format("{0},{1},", EResponseCommand.DL, dic[EResponseCommand.DL]));
                    sb.Append(string.Format("{0},{1},", EResponseCommand.PW, dic[EResponseCommand.PW]));
                    sb.Append(string.Format("{0},{1},", EResponseCommand.DV, dic[EResponseCommand.DV]));
                    sb.Append(string.Format("{0},{1},", EResponseCommand.FQ, dic[EResponseCommand.FQ]));
                    sb.Append(string.Format("{0},{1},", EResponseCommand.ED, dic[EResponseCommand.ED]));
                    sb.Append(string.Format("{0},{1},", EResponseCommand.OS, dic[EResponseCommand.OS]));
                    sb.Append(string.Format("{0},{1},", EResponseCommand.EN, dic[EResponseCommand.EN]));
                    sb.Append(string.Format("{0},{1},", EResponseCommand.AR, dic[EResponseCommand.AR]));
                    sb.AppendLine(string.Format("{0},{1}", EResponseCommand.IN, dic[EResponseCommand.IN]));
                    break;

                default:
                    sb.Append(wirtePacket);
                    break;
            }

            var receivedPacket = new ReceivedPacket();
            receivedPacket.ReceivedDataByte = serialPortEx.PacketHandler.PacketParser.EncodePacket(sb.ToString());
            receivedPacket.ReceivedData = sb.ToString();

            return receivedPacket;
        }
    }

    public class SerialEncoderV107 : SerialEncoderV105
    {
        public enum ESendCommandV2
        {
            CY, PC, CC, // Send
        }

        public enum EResponseCommandV2
        {
            RC  // Responce
        }

        public enum ECommandV2
        {
            CY, PC, CC, // Send
            RC  // Responce
        }

        public override string Version => "1.07";
        public override Enum GetCommand(string command)
        {
            Enum e = base.GetCommand(command);
            if (e == null)
            {
                if (Enum.TryParse<EResponseCommandV2>(command, out EResponseCommandV2 res))
                {
                    return res;
                }

                return null;
            }
            return e;
        }

        public override bool IsCompatible(string command)
        {
            if (base.IsCompatible(command))
            {
                return true;
            }

            return Enum.GetNames(typeof(ESendCommandV2)).Contains(command);
        }

        public SerialEncoderV107(SerialDeviceInfo deviceInfo) : base(deviceInfo)
        {
        }

        public override bool Initialize()
        {
            bool ok = base.Initialize();
            if (ok)
            {
                string[] token = ExcuteCommand(ESendCommandV2.CC);
                IsInit = token != null && token.Length > 0;

                //100000
                if (IsInit)
                {
                    ExcuteCommand(ESendCommandV2.CY, "5000000");  // 5000000 * 20ns = 100ms
                }
            }
            return ok;
        }

        public override double GetSpeedPlsPerMs()
        {
            string[] paramArray = ExcuteCommand(ESendCommand.GR);
            if (paramArray == null || paramArray.Length < 25)
            {
                return -1;
            }

            double timeMs = int.Parse(paramArray[24]) / 50e6 * 1000;

            string[] token = ExcuteCommand(ESendCommandV2.PC);
            if (token == null || token.Length != 2)
            {
                return -1;
            }

            int pls = int.Parse(token[1]);

            return pls / timeMs;
        }

        public override string[] GetSendCommandString()
        {
            var commands = base.GetSendCommandString().ToList();
            commands.AddRange(Enum.GetNames(typeof(ESendCommandV2)));
            return commands.ToArray();
        }
    }
}
