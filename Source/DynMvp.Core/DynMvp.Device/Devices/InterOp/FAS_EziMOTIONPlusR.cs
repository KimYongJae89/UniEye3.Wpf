using System.Runtime.InteropServices;

internal class FAS_EziMOTIONPlusR
{
    // Referred by ReturnCodes_Define.h
    // typedef enum _FMM_ERROR
    public const int FMM_OK = 0;

    public const int FMM_INVALID_PORT_NUM = 1;
    public const int FMM_INVALID_SLAVE_NUM = 2;

    public const int FMC_DISCONNECTED = 5;
    public const int FMC_TIMEOUT_ERROR = 6;
    public const int FMC_CRCFAILED_ERROR = 7;
    public const int FMC_RECVPACKET_ERROR = 8;

    public const int FMM_POSTABLE_ERROR = 9;

    public const int FMP_FRAMETYPEERROR = 0x80;
    public const int FMP_DATAERROR = 0x81;
    public const int FMP_PACKETERROR = 0x82;

    public const int FMP_RUNFAIL = 0x85;
    public const int FMP_RESETFAIL = 0x86;
    public const int FMP_SERVOONFAIL1 = 0x87;
    public const int FMP_SERVOONFAIL2 = 0x88;
    public const int FMP_SERVOONFAIL3 = 0x89;

    public const int FMP_PACKETCRCERROR = 0x8A;

    // Referred by COMM_Define.h
    // Constants
    public const int MAX_SLAVE_NUMS = 16;

    // Referred by FAS_EziMOTIONPlusR.h
    // Functions.

    //FAPI BOOL WINAPI	FAS_Connect(BYTE nPortNo, DWORD dwBaud);
    [DllImport("EziMOTIONPlusRx64.dll")]
    public static extern int FAS_Connect(byte nPortNo, uint dwBaud);    // BOOL

    //FAPI void WINAPI	FAS_Close(BYTE nPortNo);
    [DllImport("EziMOTIONPlusRx64.dll")]
    public static extern void FAS_Close(byte nPortNo);

    //FAPI BOOL WINAPI	FAS_IsSlaveExist(BYTE nPortNo, BYTE iSlaveNo);
    [DllImport("EziMOTIONPlusRx64.dll")]
    public static extern int FAS_IsSlaveExist(byte nPortNo, byte iSlaveNo); // BOOL

    //FAPI int WINAPI	FAS_GetSlaveInfo(BYTE nPortNo, BYTE iSlaveNo, BYTE* pType, LPSTR lpBuff, int nBuffSize);
    //FAPI int WINAPI	FAS_GetMotorInfo(BYTE nPortNo, BYTE iSlaveNo, BYTE* pType, LPSTR lpBuff, int nBuffSize);

    //FAPI int WINAPI	FAS_SaveAllParameters(BYTE nPortNo, BYTE iSlaveNo);

    //FAPI int WINAPI	FAS_SetParameter(BYTE nPortNo, BYTE iSlaveNo, BYTE iParamNo, long lParamValue);
    [DllImport("EziMOTIONPlusRx64.dll")]
    public static extern int FAS_SetParameter(byte nPortNo, byte iSlaveNo, byte iParamNo, int lParamValue);

    //FAPI int WINAPI	FAS_GetParameter(BYTE nPortNo, BYTE iSlaveNo, BYTE iParamNo, long* lParamValue);
    [DllImport("EziMOTIONPlusRx64.dll")]
    public static extern int FAS_GetParameter(byte nPortNo, byte iSlaveNo, byte iParamNo, ref int lParamValue);

    //FAPI int WINAPI	FAS_GetROMParameter(BYTE nPortNo, BYTE iSlaveNo, BYTE iParamNo, long* lRomParam);

    ////------------------------------------------------------------------------------
    ////					IO Functions
    ////------------------------------------------------------------------------------
    //FAPI int WINAPI	FAS_SetIOInput(BYTE nPortNo, BYTE iSlaveNo, DWORD dwIOSETMask, DWORD dwIOCLRMask);
    [DllImport("EziMOTIONPlusRx64.dll")]
    public static extern int FAS_SetIOInput(byte nPortNo, byte iSlaveNo, uint dwIOSETMask, uint dwIOCLRMask);

    //FAPI int WINAPI	FAS_GetIOInput(BYTE nPortNo, BYTE iSlaveNo, DWORD* dwIOInput);
    [DllImport("EziMOTIONPlusRx64.dll")]
    public static extern int FAS_GetIOInput(byte nPortNo, byte iSlaveNo, ref uint dwIOInput);

    //FAPI int WINAPI	FAS_SetIOOutput(BYTE nPortNo, BYTE iSlaveNo, DWORD dwIOSETMask, DWORD dwIOCLRMask);
    [DllImport("EziMOTIONPlusRx64.dll")]
    public static extern int FAS_SetIOOutput(byte nPortNo, byte iSlaveNo, uint dwIOSETMask, uint dwIOCLRMask);

    //FAPI int WINAPI	FAS_GetIOOutput(BYTE nPortNo, BYTE iSlaveNo, DWORD* dwIOOutput);
    [DllImport("EziMOTIONPlusRx64.dll")]
    public static extern int FAS_GetIOOutput(byte nPortNo, byte iSlaveNo, ref uint dwIOInput);

    //FAPI int WINAPI	FAS_GetIOAssignMap(BYTE nPortNo, BYTE iSlaveNo, BYTE iIOPinNo, DWORD* dwIOLogicMask, BYTE* bLevel);
    //FAPI int WINAPI	FAS_SetIOAssignMap(BYTE nPortNo, BYTE iSlaveNo, BYTE iIOPinNo, DWORD dwIOLogicMask, BYTE bLevel);
    //FAPI int WINAPI	FAS_IOAssignMapReadROM(BYTE nPortNo, BYTE iSlaveNo);

    ////------------------------------------------------------------------------------
    ////					Servo Driver Control Functions
    ////------------------------------------------------------------------------------		
    //FAPI int WINAPI	FAS_ServoEnable(BYTE nPortNo, BYTE iSlaveNo, BOOL bOnOff);
    [DllImport("EziMOTIONPlusRx64.dll")]
    public static extern int FAS_ServoEnable(byte nPortNo, byte iSlaveNo, int bOnOff);

    //FAPI int WINAPI	FAS_ServoAlarmReset(BYTE nPortNo, BYTE iSlaveNo);
    [DllImport("EziMOTIONPlusRx64.dll")]
    public static extern int FAS_ServoAlarmReset(byte nPortNo, byte iSlaveNo);

    //FAPI int WINAPI	FAS_StepAlarmReset(BYTE nPortNo, BYTE iSlaveNo, BOOL bReset);
    [DllImport("EziMOTIONPlusRx64.dll")]
    public static extern int FAS_StepAlarmReset(byte nPortNo, byte iSlaveNo, int bReset);

    ////------------------------------------------------------------------------------
    ////					Read Status and Position
    ////------------------------------------------------------------------------------
    //FAPI int WINAPI	FAS_GetAxisStatus(BYTE nPortNo, BYTE iSlaveNo, DWORD* dwAxisStatus);
    [DllImport("EziMOTIONPlusRx64.dll")]
    public static extern int FAS_GetAxisStatus(byte nPortNo, byte iSlaveNo, ref uint dwAxisStatus);

    //FAPI int WINAPI	FAS_GetIOAxisStatus(BYTE nPortNo, BYTE iSlaveNo, DWORD* dwInStatus, DWORD* dwOutStatus, DWORD* dwAxisStatus);
    //FAPI int WINAPI	FAS_GetMotionStatus(BYTE nPortNo, BYTE iSlaveNo, long* lCmdPos, long* lActPos, long* lPosErr, long* lActVel, WORD* wPosItemNo);
    //FAPI int WINAPI	FAS_GetAllStatus(BYTE nPortNo, BYTE iSlaveNo, DWORD* dwInStatus, DWORD* dwOutStatus, DWORD* dwAxisStatus, long* lCmdPos, long* lActPos, long* lPosErr, long* lActVel, WORD* wPosItemNo);
    [DllImport("EziMOTIONPlusRx64.dll")]
    public static extern int FAS_GetAllStatus(byte nPortNo, byte iSlaveNo, ref uint dwInStatus, ref uint dwOutStatus, ref uint dwAxisStatus, ref int lCmdPos, ref int lActPos, ref int lPosErr, ref int lActVel, ref ushort wPosItemNo);

    //FAPI int WINAPI	FAS_SetCommandPos(BYTE nPortNo, BYTE iSlaveNo, long lCmdPos);
    //FAPI int WINAPI	FAS_SetActualPos(BYTE nPortNo, BYTE iSlaveNo, long lActPos);

    //FAPI int WINAPI	FAS_ClearPosition(BYTE nPortNo, BYTE iSlaveNo);
    [DllImport("EziMOTIONPlusRx64.dll")]
    public static extern int FAS_ClearPosition(byte nPortNo, byte iSlaveNo);

    //FAPI int WINAPI	FAS_GetCommandPos(BYTE nPortNo, BYTE iSlaveNo, long* lCmdPos);
    [DllImport("EziMOTIONPlusRx64.dll")]
    public static extern int FAS_GetCommandPos(byte nPortNo, byte iSlaveNo, ref int lCmdPos);

    //FAPI int WINAPI	FAS_GetActualPos(BYTE nPortNo, BYTE iSlaveNo, long* lActPos);
    [DllImport("EziMOTIONPlusRx64.dll")]
    public static extern int FAS_GetActualPos(byte nPortNo, byte iSlaveNo, ref int lActPos);

    //FAPI int WINAPI	FAS_GetPosError(BYTE nPortNo, BYTE iSlaveNo, long* lPosErr);
    [DllImport("EziMOTIONPlusRx64.dll")]
    public static extern int FAS_GetPosError(byte nPortNo, byte iSlaveNo, ref int lPosErr);

    //FAPI int WINAPI	FAS_GetActualVel(BYTE nPortNo, BYTE iSlaveNo, long* lActVel);
    [DllImport("EziMOTIONPlusRx64.dll")]
    public static extern int FAS_GetActualVel(byte nPortNo, byte iSlaveNo, ref int lActVel);

    //FAPI int WINAPI	FAS_GetAlarmType(BYTE nPortNo, BYTE iSlaveNo, BYTE* nAlarmType);
    [DllImport("EziMOTIONPlusRx64.dll")]
    public static extern int FAS_GetAlarmType(byte nPortNo, byte iSlaveNo, ref byte nAlarmType);

    ////------------------------------------------------------------------
    ////					Motion Functions.
    ////------------------------------------------------------------------
    //FAPI int WINAPI	FAS_MoveStop(BYTE nPortNo, BYTE iSlaveNo);
    [DllImport("EziMOTIONPlusRx64.dll")]
    public static extern int FAS_MoveStop(byte nPortNo, byte iSlaveNo);

    //FAPI int WINAPI	FAS_EmergencyStop(BYTE nPortNo, BYTE iSlaveNo);
    [DllImport("EziMOTIONPlusRx64.dll")]
    public static extern int FAS_EmergencyStop(byte nPortNo, byte iSlaveNo);

    //FAPI int WINAPI	FAS_MoveOriginSingleAxis(BYTE nPortNo, BYTE iSlaveNo);
    [DllImport("EziMOTIONPlusRx64.dll")]
    public static extern int FAS_MoveOriginSingleAxis(byte nPortNo, byte iSlaveNo);

    //FAPI int WINAPI	FAS_MoveSingleAxisAbsPos(BYTE nPortNo, BYTE iSlaveNo, long lAbsPos, DWORD lVelocity);
    [DllImport("EziMOTIONPlusRx64.dll")]
    public static extern int FAS_MoveSingleAxisAbsPos(byte nPortNo, byte iSlaveNo, int lAbsPos, uint lVelocity);

    //FAPI int WINAPI	FAS_MoveSingleAxisIncPos(BYTE nPortNo, BYTE iSlaveNo, long lIncPos, DWORD lVelocity);
    [DllImport("EziMOTIONPlusRx64.dll")]
    public static extern int FAS_MoveSingleAxisIncPos(byte nPortNo, byte iSlaveNo, int lIncPos, uint lVelocity);

    //FAPI int WINAPI	FAS_MoveToLimit(BYTE nPortNo, BYTE iSlaveNo, DWORD lVelocity, int iLimitDir);
    [DllImport("EziMOTIONPlusRx64.dll")]
    public static extern int FAS_MoveToLimit(byte nPortNo, byte iSlaveNo, uint lVelocity, int iLimitDir);

    //FAPI int WINAPI	FAS_MoveVelocity(BYTE nPortNo, BYTE iSlaveNo, DWORD lVelocity, int iVelDir);
    [DllImport("EziMOTIONPlusRx64.dll")]
    public static extern int FAS_MoveVelocity(byte nPortNo, byte iSlaveNo, uint lVelocity, int iVelDir);

    //FAPI int WINAPI	FAS_PositionAbsOverride(BYTE nPortNo, BYTE iSlaveNo, long lOverridePos);
    //FAPI int WINAPI	FAS_PositionIncOverride(BYTE nPortNo, BYTE iSlaveNo, long lOverridePos);
    //FAPI int WINAPI	FAS_VelocityOverride(BYTE nPortNo, BYTE iSlaveNo, DWORD lVelocity);

    //FAPI int WINAPI	FAS_MoveLinearAbsPos(BYTE nPortNo, BYTE nNoOfSlaves, BYTE* iSlavesNo, long* lAbsPos, DWORD lFeedrate, WORD wAccelTime);
    //FAPI int WINAPI	FAS_MoveLinearIncPos(BYTE nPortNo, BYTE nNoOfSlaves, BYTE* iSlavesNo, long* lIncPos, DWORD lFeedrate, WORD wAccelTime);

    ////------------------------------------------------------------------
    ////					Ex-Motion Functions.
    ////------------------------------------------------------------------
    //FAPI int WINAPI	FAS_MoveSingleAxisAbsPosEx(BYTE nPortNo, BYTE iSlaveNo, long lAbsPos, DWORD lVelocity, MOTION_OPTION_EX* lpExOption);
    //FAPI int WINAPI	FAS_MoveSingleAxisIncPosEx(BYTE nPortNo, BYTE iSlaveNo, long lIncPos, DWORD lVelocity, MOTION_OPTION_EX* lpExOption);
    //FAPI int WINAPI	FAS_MoveVelocityEx(BYTE nPortNo, BYTE iSlaveNo, DWORD lVelocity, int iVelDir, VELOCITY_OPTION_EX* lpExOption);

    ////------------------------------------------------------------------
    ////					All-Motion Functions.
    ////------------------------------------------------------------------
    //FAPI int WINAPI	FAS_AllMoveStop(BYTE nPortNo);
    //FAPI int WINAPI	FAS_AllEmergencyStop(BYTE nPortNo);
    //FAPI int WINAPI	FAS_AllMoveOriginSingleAxis(BYTE nPortNo);
    //FAPI int WINAPI	FAS_AllMoveSingleAxisAbsPos(BYTE nPortNo, long lAbsPos, DWORD lVelocity);
    //FAPI int WINAPI	FAS_AllMoveSingleAxisIncPos(BYTE nPortNo, long lIncPos, DWORD lVelocity);

    ////------------------------------------------------------------------
    ////					Position Table Functions.
    ////------------------------------------------------------------------
    //FAPI int WINAPI	FAS_PosTableReadItem(BYTE nPortNo, BYTE iSlaveNo, WORD wItemNo, LPITEM_NODE lpItem);
    //FAPI int WINAPI	FAS_PosTableWriteItem(BYTE nPortNo, BYTE iSlaveNo, WORD wItemNo, LPITEM_NODE lpItem);
    //FAPI int WINAPI	FAS_PosTableWriteROM(BYTE nPortNo, BYTE iSlaveNo);
    //FAPI int WINAPI	FAS_PosTableReadROM(BYTE nPortNo, BYTE iSlaveNo);
    //FAPI int WINAPI	FAS_PosTableRunItem(BYTE nPortNo, BYTE iSlaveNo, WORD wItemNo);

    //FAPI int WINAPI	FAS_PosTableReadOneItem(BYTE nPortNo, BYTE iSlaveNo, WORD wItemNo, WORD wOffset, long* lPosItemVal);
    //FAPI int WINAPI	FAS_PosTableWriteOneItem(BYTE nPortNo, BYTE iSlaveNo, WORD wItemNo, WORD wOffset, long lPosItemVal);

    //FAPI int WINAPI	FAS_PosTableSingleRunItem(BYTE nPortNo, BYTE iSlaveNo, BOOL bNextMove, WORD wItemNo);

}
