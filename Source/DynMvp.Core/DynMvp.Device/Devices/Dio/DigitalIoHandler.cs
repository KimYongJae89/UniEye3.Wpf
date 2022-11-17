using DynMvp.Base;
using DynMvp.Devices.MotionController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DynMvp.Devices.Dio
{
    public class DioValue
    {
        private Dictionary<string, uint> valueList = new Dictionary<string, uint>();

        public int Count => valueList.Count;

        private string GetKeyName(int deviceNo, int groupNo)
        {
            return string.Format("{0:00}_{1:000}", deviceNo, groupNo);
        }

        public uint GetValue(int deviceNo, int groupNo)
        {
            return GetValue(GetKeyName(deviceNo, groupNo));
        }

        public uint GetValue(string keyName)
        {
            try
            {
                return valueList[keyName];
            }
            catch (Exception)
            {

            }

            return 0;
        }

        public void AddValue(int deviceNo, int groupNo, uint value)
        {
            valueList.Add(GetKeyName(deviceNo, groupNo), value);
        }

        public void Copy(DioValue dioValue)
        {
            valueList.Clear();
            foreach (KeyValuePair<string, uint> keyValue in valueList)
            {
                valueList.Add(keyValue.Key, keyValue.Value);
            }
        }

        public void UpdateBitFlag(IoPort ioPort, bool flag)
        {
            if (valueList.Count > ioPort.DeviceNo)
            {
                string keyName = GetKeyName(ioPort.DeviceNo, ioPort.GroupNo);
                uint value = valueList[keyName];
                DigitalIo.UpdateBitFlag(ref value, ioPort.PortNo, flag);

                valueList[keyName] = value;
            }
        }

        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            if (!(obj is DioValue p))
            {
                return false;
            }

            if (valueList.Count != p.valueList.Count)
            {
                return false;
            }

            foreach (KeyValuePair<string, uint> keyValue in valueList)
            {
                if (keyValue.Value != p.GetValue(keyValue.Key))
                {
                    return false;
                }
            }

            return true;
        }

        public bool Equals(DioValue p)
        {
            // If parameter is null return false:
            if (p == null)
            {
                return false;
            }

            if (valueList.Count != p.valueList.Count)
            {
                return false;
            }

            foreach (KeyValuePair<string, uint> keyValue in valueList)
            {
                if (keyValue.Value != p.GetValue(keyValue.Key))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            int value = 0;

            foreach (KeyValuePair<string, uint> keyValue in valueList)
            {
                value ^= (int)keyValue.Value;
            }

            return value;
        }
    }

    public class DigitalIoHandler
    {
        private object lockOutput = new object();
        private List<IDigitalIo> digitalIoList = new List<IDigitalIo>();

        public IEnumerator<IDigitalIo> GetEnumerator()
        {
            return digitalIoList.GetEnumerator();
        }

        public IDigitalIo Get(int index)
        {
            if (index > -1 && index < digitalIoList.Count())
            {
                return digitalIoList[index];
            }

            return null;
        }

        public IDigitalIo Get(string deviceName)
        {
            int index = GetIndex(deviceName);
            if (index > -1)
            {
                return Get(index);
            }

            return null;
        }

        public int GetIndex(string deviceName)
        {
            for (int i = 0; i < digitalIoList.Count; i++)
            {
                if (digitalIoList[i].GetName() == deviceName)
                {
                    return i;
                }
            }

            return -1;
        }

        public void Add(IDigitalIo digitalIo)
        {
            digitalIoList.Add(digitalIo);
        }

        public void Initialize(DigitalIoInfoList digitalIoInfoList, MotionHandler motionHandler)
        {
            foreach (DigitalIoInfo digitalIoInfo in digitalIoInfoList)
            {
                DigitalIo digitalIo = null;
                if (DeviceConfig.Instance().VirtualMode)
                {
                    DigitalIoInfo virtualDigitalIoInfo = new DigitalIoInfoVirtual(DigitalIoType.Virtual, digitalIoInfo.Index,
                        digitalIoInfo.NumInPortGroup, 0, digitalIoInfo.NumInPort,
                        digitalIoInfo.NumOutPortGroup, 0, digitalIoInfo.NumOutPort);

                    digitalIo = DigitalIoFactory.Create(virtualDigitalIoInfo);
                }
                else
                {
                    if (DigitalIoFactory.IsSlaveDevice(digitalIoInfo.Type))
                    {
                        var digitalIoIf = (IDigitalIo)motionHandler.GetMotion(digitalIoInfo.Type.ToString());

                        if (digitalIoIf.Initialize(digitalIoInfo) == false)
                        {
                            ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, (int)CommonError.FailToInitialize,
                                ErrorLevel.Error, ErrorSection.DigitalIo.ToString(), CommonError.FailToInitialize.ToString(), string.Format("Fail to initialize Digital I/O. {0}", digitalIoInfo.Type.ToString()));
                            digitalIoIf.UpdateState(DeviceState.Error, "DigitalIo is invalid.");
                        }
                        else
                        {
                            Add((IDigitalIo)motionHandler.GetMotion(digitalIoInfo.Type.ToString()));
                        }
                    }
                    else
                    {
                        digitalIo = DigitalIoFactory.Create(digitalIoInfo);
                    }
                }

                if (digitalIo != null)
                {
                    Add(digitalIo);
                    DeviceManager.Instance().AddDevice(digitalIo);
                }
            }
        }

        public void Release()
        {
            foreach (IDigitalIo digitalIo in digitalIoList)
            {
                digitalIo.Release();
            }
        }

        public void RealseIo()
        {
            // 디지털 아이오를 전부 리셋 해줘야 한다.
            Release();
        }

        public void WriteOutputGroup(int deviceNo, int groupNo, uint outputPortStatus)
        {
            if (deviceNo < digitalIoList.Count())
            {
                digitalIoList[deviceNo].WriteOutputGroup(groupNo, outputPortStatus);
            }
        }

        public uint ReadOutputGroup(int deviceNo, int groupNo)
        {
            if (deviceNo < digitalIoList.Count())
            {
                return digitalIoList[deviceNo].ReadOutputGroup(groupNo);
            }

            return 0;
        }

        public void WriteInputGroup(int deviceNo, int groupNo, uint outputPortStatus)
        {
            if (deviceNo < digitalIoList.Count())
            {
                //digitalIoList[deviceNo].Write WriteInputGroup(groupNo, outputPortStatus);
            }
        }

        public uint ReadInputGroup(int deviceNo, int groupNo)
        {
            if (deviceNo < digitalIoList.Count())
            {
                return digitalIoList[deviceNo].ReadInputGroup(groupNo);
            }

            return 0;
        }

        public void LockOutput()
        {
            Monitor.Enter(lockOutput);
        }

        public void UnlockOutput()
        {
            Monitor.Exit(lockOutput);
        }

        public DioValue ReadOutput(bool lockProcess = false)
        {
            if (lockProcess)
            {
                Monitor.Enter(lockOutput);
            }

            var dioValue = new DioValue();

            int index = 0;
            foreach (IDigitalIo digitalIo in digitalIoList)
            {
                for (int groupNo = 0; groupNo < digitalIo.GetNumOutPortGroup(); groupNo++)
                {
                    dioValue.AddValue(index, groupNo, digitalIo.ReadOutputGroup(groupNo));
                }

                index++;
            }

            return dioValue;
        }

        public bool ReadOutput(IoPort ioPort)
        {
            if (ioPort.DeviceNo < digitalIoList.Count())
            {
                uint value = digitalIoList[ioPort.DeviceNo].ReadOutputGroup(ioPort.GroupNo);
                return (((value >> ioPort.PortNo) & 0x1) == 1);
            }

            return false;
        }

        public void WriteOutput(DioValue dioValue, bool lockProcess = false)
        {
            for (int deviceNo = 0; deviceNo < digitalIoList.Count; deviceNo++)
            {
                for (int groupNo = 0; groupNo < digitalIoList[deviceNo].GetNumOutPortGroup(); groupNo++)
                {
                    digitalIoList[deviceNo].WriteOutputGroup(groupNo, dioValue.GetValue(deviceNo, groupNo));
                }
            }

            if (lockProcess == true)
            {
                Monitor.Exit(lockOutput);
            }
        }

        public void WriteOutput(IoPort ioPort, bool value)
        {
            if (ioPort == null || ioPort.PortNo == IoPort.UNUSED_PORT_NO)
            {
                return;
            }

            if (ioPort.DeviceNo < digitalIoList.Count())
            {
                // Group No 사용하지 않은 코드. I/O에 문제 있을 경우, 참고 요
                //int bitPos = ioPort.PortNo;
                //int groupNo = bitPos / 32;
                //bitPos = bitPos - groupNo * 32;
                //Monitor.Enter(lockOutput);

                //uint outputPortStatus = digitalIoList[ioPort.DeviceNo].ReadOutputGroup(ioPort.GroupNo);

                //DigitalIo.UpdateBitFlag(ref outputPortStatus, ioPort.PortNo, value);

                //digitalIoList[ioPort.DeviceNo].WriteOutputGroup(ioPort.GroupNo, outputPortStatus);

                //Monitor.Exit(lockOutput);

                digitalIoList[ioPort.DeviceNo].WriteOutputPort(ioPort.GroupNo, ioPort.PortNo, value);
            }
        }

        public void WriteInput(DioValue dioValue)
        {
            for (int deviceNo = 0; deviceNo < digitalIoList.Count; deviceNo++)
            {
                for (int groupNo = 0; groupNo < digitalIoList[deviceNo].GetNumInPortGroup(); groupNo++)
                {
                    digitalIoList[deviceNo].WriteInputGroup(groupNo, dioValue.GetValue(deviceNo, groupNo));
                }
            }
        }

        public void WriteInput(IoPort ioPort, bool value)
        {
            if (ioPort.DeviceNo < digitalIoList.Count())
            {
                //int bitPos = ioPort.PortNo;
                //int groupNo = bitPos / 32;
                //bitPos = bitPos - groupNo * 32;
                uint inputPortStatus = digitalIoList[ioPort.DeviceNo].ReadInputGroup(ioPort.GroupNo);

                DigitalIo.UpdateBitFlag(ref inputPortStatus, ioPort.PortNo, value);

                digitalIoList[ioPort.DeviceNo].WriteInputGroup(ioPort.GroupNo, inputPortStatus);
            }
        }

        public DioValue ReadInput()
        {
            var dioValue = new DioValue();

            int index = 0;
            foreach (IDigitalIo digitalIo in digitalIoList)
            {
                for (int groupNo = 0; groupNo < digitalIo.GetNumInPortGroup(); groupNo++)
                {
                    dioValue.AddValue(index, groupNo, digitalIo.ReadInputGroup(groupNo));
                }

                index++;
            }

            return dioValue;
        }

        public bool ReadInput(IoPort ioPort)
        {
            return ReadInput(ioPort.DeviceNo, ioPort.GroupNo, ioPort.PortNo);
        }

        public bool ReadInput(int deviceNo, int groupNo, int bitPos)
        {
            if (deviceNo < digitalIoList.Count())
            {
                //int groupNo = bitPos / 32;
                //bitPos = bitPos - groupNo * 32;
                uint inputPortStatus = digitalIoList[deviceNo].ReadInputGroup(groupNo);

                return (((inputPortStatus >> bitPos) & 0x1) == 1);
            }

            return false;
        }

        public string GetInputValueString()
        {
            string resultString = "";

            foreach (DigitalIo digitalIo in digitalIoList)
            {
                resultString += string.Format("0:X8", digitalIo.ReadInputGroup(0));
            }

            return resultString;
        }

        private IDigitalIo ConnectToMaster(DigitalIoInfo digitalIoInfo, MotionInfoList motionInfoList)
        {
            var slaveDigitalIoInfo = (SlaveDigitalIoInfo)digitalIoInfo;
            if (string.IsNullOrEmpty(slaveDigitalIoInfo.MasterDeviceName))
            {
                ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, (int)CommonError.InvalidSetting, ErrorLevel.Error,
                    ErrorSection.DigitalIo.ToString(), CommonError.InvalidSetting.ToString(), "Slave Digital Io must have the master device name.");
                return null;
            }

            IDigitalIo idigitalIo = null;

            var motion = (Motion)DeviceManager.Instance().GetDevice(DeviceType.MotionController, slaveDigitalIoInfo.MasterDeviceName);
            if (motion == null)
            {
                MotionInfo motionInfo = motionInfoList.GetMotionInfo(slaveDigitalIoInfo.MasterDeviceName);
                if (motionInfo == null)
                {
                    ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, (int)DigitalIoError.CantFindMasterMotion,
                        ErrorSection.DigitalIo.ToString(), DigitalIoError.CantFindMasterMotion.ToString(), "Can't find master device info.");
                    return null;
                }

                motion = MotionFactory.Create(motionInfo);
            }

            if (motion != null)
            {
                if (motion is IDigitalIo)
                {
                    idigitalIo = (IDigitalIo)motion;
                }
                else
                {
                    ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, (int)DigitalIoError.InvalidMasterMotion, ErrorLevel.Error,
                        ErrorSection.DigitalIo.ToString(), DigitalIoError.InvalidMasterMotion.ToString(), "The master device can't work for Digital I/O.");
                    motion.Release();
                }
            }

            return idigitalIo;
        }

        public void Build(DigitalIoInfoList digitalIoInfoList, MotionInfoList motionInfoList)
        {
            foreach (DigitalIoInfo digitalIoInfo in digitalIoInfoList)
            {
                IDigitalIo idigitalIo = null;
                if (DigitalIoFactory.IsSlaveDevice(digitalIoInfo.Type))
                {
                    idigitalIo = ConnectToMaster(digitalIoInfo, motionInfoList);
                }
                else
                {
                    idigitalIo = DigitalIoFactory.Create(digitalIoInfo);
                }

                if (idigitalIo != null)
                {
                    Add(idigitalIo);
                }
            }
        }
    }
}
