using System;
using System.Runtime.InteropServices;
using System.Text;

namespace DynMvp.Devices.MotionController
{
    internal enum DTK_MODE_TYPE
    {
        DM_GPASCII = 0,
        DM_GETSENDS_0 = 1,
        DM_GETSENDS_1 = 2,
        DM_GETSENDS_2 = 3,
        DM_GETSENDS_3 = 4,
        DM_GETSENDS_4 = 5
    };

    internal enum DTK_STATUS
    {
        DS_Ok = 0,
        DS_Exception = 1,
        DS_TimeOut = 2,
        DS_Connected = 3,
        DS_NotConnected = 4,
        DS_Failed = 5,
        DS_InvalidDevice = 11,
        DS_LengthExceeds = 21,
        DS_RunningDownload = 22,
        DS_RunningRead = 23
    };

    internal enum DTK_RESET_TYPE
    {
        DR_Reset = 0,
        DR_FullReset = 1
    };

    public class PowerPmacDLL
    {
        public delegate void PDOWNLOAD_PROGRESS(int nPercent);
        public delegate void PDOWNLOAD_MESSAGE_A(string lpMessage);
        public delegate void PRECEIVE_PROC_A(string lpReveive);

        private const string strDLL = "PComm64.dll";

        /// 장비 검색
        [DllImport(strDLL)]
        public static extern uint PmacSelect(int nullValue = 0);

        // 장비 연결
        [DllImport(strDLL)]
        public static extern bool OpenPmacDevice(uint deviceID);

        // 연결 해제
        [DllImport(strDLL)]
        public static extern bool ClosePmacDevice(uint DeviceID);

        // 명령 보내고, 응답 받기
        [DllImport(strDLL)]
        public static extern long PmacGetResponseA(uint deviceID, byte[] strResponse, uint MaxSize, byte[] strCommand);

        // 현재 위치 조회
        [DllImport(strDLL)]
        public static extern double PmacDPRPosition(uint deviceID, long motorID, double units);

        // 명령 위치 조회
        [DllImport(strDLL)]
        public static extern double PmacDPRGetCommandedPos(uint deviceID, long motorID, double units);

        // 극점에 위치하고 있는가?
        [DllImport(strDLL)]
        public static extern bool PmacDPROnPositionLimit(uint deviceID, long motorID);

        // 명령 보내기
        [DllImport(strDLL)]
        public static extern double PmacDPRCommanded(uint dwDevice, long crd, char maxchar);

        [DllImport(strDLL)]
        /// This function returns the velocity of the specified motor in units of (1/(Ix09*32) counts per servo cycle.
        public static extern double PmacDPRGetVel(uint dwDevice, long motor, double units);

        [DllImport(strDLL)]
        // This function returns the net (or vector) velocity of the chosen motors[] in units of counts per minute if the units[] parameter is unity.
        public static extern double PmacDPRVectorVelocity(uint dwDevice, long num, long[] motor, double[] units);

        // The functions above return the boolean status of a variety of motor flags.
        [DllImport(strDLL)]
        public static extern bool PmacDPRAmpEnabled(uint dwDevice, long motor);
        [DllImport(strDLL)]
        public static extern bool PmacDPRWarnFError(uint dwDevice, long motor);
        [DllImport(strDLL)]
        public static extern bool PmacDPRFatalFError(uint dwDevice, long motor);
        [DllImport(strDLL)]
        public static extern bool PmacDPRAmpFault(uint dwDevice, long motor);
        [DllImport(strDLL)]
        public static extern bool PmacDPRHomeComplete(uint dwDevice, long motor);
        [DllImport(strDLL)]
        public static extern bool PmacDPRInposition(uint dwDevice, long motor);
        [DllImport(strDLL)]
        public static extern double PmacDPRGetTargetPos(uint dwDevice, long motor, double posscale);
        [DllImport(strDLL)]
        public static extern double PmacDPRGetBiasPos(uint dwDevice, long motor, double posscale);
        [DllImport(strDLL)]
        public static extern bool PmacDPROnNegativeLimit(uint dwDevice, long motor);
        [DllImport(strDLL)]
        public static extern bool PmacDPROnPositiveLimit(uint dwDevice, long motor);

        // 라이브러리 오픈
        // 인자를 NULL로 할 경우 DTKDeviceSelect 함수를 사용하여 장치를 연결해야 한다.


        // 아래 PowerPmac32.dll은 기계연구원 Confocal 장비와 맞지 않음.
        /*
        [DllImport("PowerPmac32.dll")]
        public static extern UInt32 DTKPowerPmacOpen(UInt32 dwIPAddress, UInt32 uMode);

        // 라이브리리 클로즈
        [DllImport("PowerPmac32.dll")]
        public static extern UInt32 DTKPowerPmacClose(UInt32 uDeviceID);

        // 등록된 디바이스 갯수
        [DllImport("PowerPmac32.dll")]
        public static extern UInt32 DTKGetDeviceCount(out Int32 pnDeviceCount);

        // IP Address 확인
        [DllImport("PowerPmac32.dll")]
        public static extern UInt32 DTKGetIPAddress(UInt32 uDeviceID, out UInt32 pdwIPAddress);

        // 장치를 연결
        [DllImport("PowerPmac32.dll")]
        public static extern UInt32 DTKConnect(UInt32 uDeviceID);

        // 장치를 해제
        [DllImport("PowerPmac32.dll")]
        public static extern UInt32 DTKDisconnect(UInt32 uDeviceID);

        // 장치가 연결되었는지 확인
        [DllImport("PowerPmac32.dll")]
        public static extern UInt32 DTKIsConnected(UInt32 uDeviceID, out Int32 pbConnected);

        [DllImport("PowerPmac32.dll")]
        public static extern UInt32 DTKGetResponseA(UInt32 uDeviceID, Byte[] lpCommand, Byte[] lpResponse, Int32 nLength);

     //   [DllImport("PowerPmac32.dll")]
    //    public static extern UInt32 DTKGetResponseW(UInt32 uDeviceID, String lpwCommand, ref String lpwResponse, Int32 nLength);

        [DllImport("PowerPmac32.dll")]
        public static extern UInt32 DTKSendCommandA(UInt32 uDeviceID, Byte[] lpCommand);

        [DllImport("PowerPmac32.dll")]
        public static extern UInt32 DTKAbort(UInt32 uDeviceID);
        [DllImport("PowerPmac32.dll")]
        public static extern UInt32 DTKDownloadA(UInt32 uDeviceID, Byte[] lpwDownload, Int32 bDowoload, PDOWNLOAD_PROGRESS lpDownloadProgress, PDOWNLOAD_MESSAGE_A lpDownloadMessage);

        [DllImport("PowerPmac32.dll")]
        public static extern UInt32 DTKSetReceiveA(UInt32 uDeviceID, PRECEIVE_PROC_A lpReveiveProc);
        */
    }
}