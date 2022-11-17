using DynMvp.Base;
using DynMvp.Devices.Dio;
using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace DynMvp.Devices.Dio
{
    internal class DigitalIoAlphaMotionBBx : DigitalIo
    {
        private bool initialized = false;
        private ushort boardNo = 0;

        public DigitalIoAlphaMotionBBx(string name)
            : base(DigitalIoType.AlphaMotionBBxDIO, name)
        {
        }

        public override bool Initialize(DigitalIoInfo digitalIoInfo)
        {
            if (IsReady() == false)
            {
                try
                {
                    var pciDigitalIoInfo = (PciDigitalIoInfo)digitalIoInfo;
                    boardNo = (ushort)pciDigitalIoInfo.Index;

                    int iBoardNo = 0;
                    int result = nmiMNApi.nmiSysLoad(nmiMNApiDefs.emFALSE, ref iBoardNo);
                    // TMC_RV_OK 1 라이브러리 성공 
                    // TMC_RV_NOT_OPEN - 1001 라이브러리 초기화 실패 
                    // TMC_RV_LOC_MEM_ERR -1004 메모리 생성 에러 
                    // TMC_RV_HANDLE_ERR -1026 드바이스 핸들값 에러 
                    // TMC_RV_PCI_BUS_LINE_ERR -1058 PCI 버스 라인 이상 에러 
                    // TMC_RV_CON_DIP_SW_ERR -1056 동일한 DIP SWITCH를 설정 에러 
                    // TMC_RV_MODULE_POS_ERR -1059 모듈 순서 에러 
                    // TMC_RV_SUPPORT_PROCESS -1060 지원하지 않은 프로세스 에러

                    if (boardNo >= iBoardNo)
                    {
                        return false;
                    }

                    int cardNo = 0;

                    result = nmiMNApi.nmiSysComm(cardNo);
                    if (result < 1)
                    {
                        string str = string.Format("nmiSysBegin() ERR!\r\nErrNo = %ld", result);
                        throw new Exception(str);
                    }

                    result = nmiMNApi.nmiCyclicBegin(cardNo);

                    if (result < 1)
                    {
                        string str = string.Format("nmiSysBegin() ERR!\r\nErrNo = %ld", result);
                        throw new Exception(str);
                    }

                    int numDioIn = 0, numDioOut = 0;
                    result = nmiMNApi.nmiGnGetDioNum(boardNo, ref numDioIn, ref numDioOut);

                    if (result != nmiMNApiDefs.TMC_RV_OK)
                    {
                        throw new Exception(string.Format("Can not find DIO info. {0}", result));
                    }

                    NumInPort = numDioIn;
                    NumOutPort = numDioOut;

                    NumInPortGroup = digitalIoInfo.NumInPortGroup;
                    InPortStartGroupIndex = digitalIoInfo.InPortStartGroupIndex;
                    NumOutPortGroup = digitalIoInfo.NumOutPortGroup;
                    OutPortStartGroupIndex = digitalIoInfo.OutPortStartGroupIndex;

                    UpdateState(DeviceState.Ready, "Device Loaded");

                    initialized = true;

                    return true;
                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, (int)CommonError.FailToInitialize,
                        ErrorLevel.Fatal, ErrorSection.DigitalIo.ToString(), CommonError.FailToInitialize.ToString(), string.Format("[DigitalIoAlphaMotionBBx] {0}", errorMsg));

                    UpdateState(DeviceState.Error, "Can't find alpha motion Bx device.");
                    return false;
                }
            }

            return false;
        }

        public override bool IsReady()
        {
            return initialized;
        }

        public override uint ReadInputGroup(int groupNo)
        {
            uint value = 0;
            //nmiMNApi.nmiDiGetData(boardNo, groupNo * 16, ref value);
            int ret = nmiMNApi.nmiDiGetData(boardNo, groupNo + 1, ref value);
            //string errText = "";
            //nmiMNApi.nmiErrGetString(boardNo, ret, ref errText);
            return value;
        }

        public override uint ReadOutputGroup(int groupNo)
        {
            uint value = 0;
            //nmiMNApi.nmiDoGetData(boardNo, (groupNo * 16), ref value);
            nmiMNApi.nmiDoGetData(boardNo, groupNo + 1, ref value);
            return value;
        }

        public override void WriteInputGroup(int groupNo, uint inputPortStatus) { }

        public override void WriteOutputGroup(int groupNo, uint outputPortStatus)
        {
            nmiMNApi.nmiDoSetData(boardNo, 1 + groupNo, outputPortStatus);
        }

        public override void WriteOutputPort(int groupNo, int portNo, bool value)
        {
            nmiMNApi.nmiDoSetBit(boardNo, 1, (ushort)(groupNo * 16 + portNo), (ushort)(value ? 1 : 0));
        }
    }
}
