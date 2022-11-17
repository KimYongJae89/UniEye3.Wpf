using DynMvp.Base;
using MicroEpsilon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Devices.Daq
{
    public class DaqChannelMedaqProperty : DaqChannelProperty
    {
        public string IpAddress { get; set; }

        public DaqChannelMedaqProperty(DaqChannelType daqChannelType) : base(daqChannelType)
        {

        }

        public override void LoadXml(XmlElement daqPropertyElement)
        {
            base.LoadXml(daqPropertyElement);

            IpAddress = XmlHelper.GetValue(daqPropertyElement, "IpAddress", "");
        }

        public override void SaveXml(XmlElement daqPropertyElement)
        {
            base.SaveXml(daqPropertyElement);

            XmlHelper.SetValue(daqPropertyElement, "IpAddress", IpAddress);
        }
    }

    public class DaqChannelMedaq : DaqChannel
    {
        private MEDAQLib sensor = null;

        public DaqChannelMedaq() : base(DaqChannelType.MeDAQ)
        {

        }

        public override void Initialize(DaqChannelProperty daqChannelProperty)
        {
            ChannelProperty = daqChannelProperty;
            var daqChannelMedaqProperty = (DaqChannelMedaqProperty)daqChannelProperty;
            // Create sensor object
            sensor = new MEDAQLib(ChannelProperty.DeviceName);

            // Open sensor
            sensor.SetParameterInt("IP_EnableLogging", 0);
            ERR_CODE errCode = ERR_CODE.ERR_NOERROR;
            int i = 0;
            bool bRun = true;

            while (bRun)
            {
                switch (i)
                {
                    case 0:
                        errCode = sensor.OpenSensorTCPIP(ChannelProperty.ChannelName);
                        break;
                    case 1:
                        errCode = sensor.SetIntExecSCmd("Set_EthernetMode", "SP_EthernetMode", 0); // Set to Data output interface
                        //errCode = sensor.SetParameterDouble("SP_EthernetMode", 0); // Set to Ethernet
                        break;
                    case 2:
                        //errCode = sensor.SetIntExecSCmd("Set_IPDataTransferMode", "SP_Protocol", 0); // Set to Data output interface
                        //errCode = sensor.SetParameterDouble("SP_Protocol", 0); // Set to TCP Server
                        break;
                    case 3:
                        errCode = sensor.SetIntExecSCmd("Set_DataOutInterface", "SP_DataOutInterface", 2); // Set to Data output interface
                        //errCode = sensor.SetParameterDouble("SP_DataOutInterface", 2); // Set to Data output interface
                        break;
                    case 4:
                        //errCode = sensor.SetIntExecSCmd("Set_OutputDistance_ETH", "SP_OutputDistance1_ETH", 1); // Set to Data output interface
                        //errCode = sensor.SetParameterDouble("SP_OutputDistance1_ETH", 1); // Set to Data output interface
                        break;

                    case 5:
                        //errCode = sensor.SetIntExecSCmd("Set_MeasureMode", "SP_MeasureMode", 1); // Set to Data output interface
                        errCode = sensor.SetIntExecSCmd("Set_MeasureMode", "SP_MeasureMode", 0); // Set to Data output interface
                        break;
                    case 6:
                        errCode = sensor.SetDoubleExecSCmd("Set_Samplerate", "SP_Measrate", 2.5); // Set to Data output interface
                        //errCode = sensor.SetParameterDouble("SP_Measrate", 1.0); // Set to Sampling rate at 1kHz
                        break;
                    default:
                        bRun = false;
                        break;
                }

                if (errCode != ERR_CODE.ERR_NOERROR)
                {
                    UpdateState(DeviceState.Error, "Can't connection sensor.");
                    return;
                }
                i++;
            }
            int itemp = -1;
            errCode = sensor.ExecSCmdGetInt("Get_DataOutInterface", "SA_DataOutInterface", ref itemp); // Set to Data output interface

            //sensor.SetParameterDouble("SP_AveragingType", 1); // Set to Averaging type at moving
            //sensor.SetParameterDouble("SP_Averaging", 0); // Set to Averageing factor

            //sensor.SetParameterDouble("SP_TriggerMode", 3); // Set to SW Trigger mode
            //sensor.SetParameterDouble("SP_TriggerLevel", 0); // Set to Active High Trigger

            UpdateState(DeviceState.Ready, "Connected.");
        }

        public override double[] ReadData(int numSamples)
        {
            //System.Threading.Thread.Sleep(100); // Wait for data

            int avail = 0;
            ERR_CODE errCode = sensor.DataAvail(ref avail);
            if ((errCode != ERR_CODE.ERR_NOERROR) && (errCode != ERR_CODE.ERR_WARNING))
            {
                string strError = "";
                sensor.GetError(ref strError);
                Debug.WriteLine(strError);
                return null;
            }

            int samples = Math.Min(avail, avail);

            int[] rawData = new int[samples];
            double[] scaledData = new double[samples];
            int readed = 0;
            double timestamp = -1;

            ERR_CODE errCode2 = sensor.TransferDataTS(rawData, scaledData, samples, ref readed, ref timestamp);
            if ((errCode2 != ERR_CODE.ERR_NOERROR) && (errCode2 != ERR_CODE.ERR_WARNING))
            {
                string strError = "";
                sensor.GetError(ref strError);
                Debug.WriteLine(strError);
                return null;
            }

            if (samples != readed)
            {
                return null;
            }

            double[] result = new double[samples];
            rawData.CopyTo(result, 0);
            return scaledData;
        }

        public override double[] ReadVoltage(int numSamples)
        {
            throw new NotImplementedException();
        }
    }
}
