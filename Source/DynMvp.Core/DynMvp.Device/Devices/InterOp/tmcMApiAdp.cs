///******************************************************************************
//*
//*	File Version: 1,0,0,0
//*
//*	Copyright (c) Alpha Motion 2011-
//*
//*	This file is strictly confidential and do not distribute it outside.
//*
//*	MODULE NAME :
//*		tmcMApiAdp.cs
//*
//*	AUTHOR :
//*		K.C. Lee
//*
//*	DESCRIPTION :
//*		the header file for RC files of project.
//*
//*
///****************************************************************************/


using System;
using System.Runtime.InteropServices;

namespace Shared
{
    /// <summary>
    /// Import tmcMApiAdp에 대한 요약 설명입니다.
    /// </summary>
    public class TMCADDLL
    {
        public delegate void EventFunc(IntPtr lParam);


        //======================  Loading/Unloading function ====================================================//
        // 1. TMC314A_LoadDevice
        // 하드웨어 장치를 로드하고 초기화한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_LoadDevice", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC314A_LoadDevice();

        // 2. TMC314A_UnloadDevice
        // 하드웨어 장치를 언로드한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_UnloadDevice", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_UnloadDevice();


        //======================  						장치 초기화               ====================================================//
        //3. TMC314A_Reset
        // 하드웨어 및 소프트웨어를 초기화한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Reset", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_Reset([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);

        //4. TMC314A_SetSystemDefault
        // 소프트웨어를 초기화한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetSystemDefault", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetSystemDefault([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //5. TMC314A_PutSvOn
        // 지정 축의 Servo-On 신호를 출력한다.
        // w_Status : CMD_OFF(0), CMD_ON(1)
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_PutSvOn", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_PutSvOn([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        //6. TMC314A_GetSvOn
        // 지정 축의 Servo-On신호의 출력 상태을 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetSvOn", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetSvOn([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        //======================  에러 처리               ====================================================//

        //7. TMC314A_GetErrorCode
        // 가장 최근에 실행된 함수의 에러코드를 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetErrorCode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC314A_GetErrorCode();

        //8. TMC314A_GetErrorString
        // 가장 최근에 실행된 함수의 에러코드를 문자열로 변환해줍니다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetErrorString", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string TMC314A_GetErrorString([MarshalAs(UnmanagedType.I4)] int nErrorCode);

        //9. TMC314A_GetErrorString
        // 모션 완료 후 모션 정지 원인를 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetMotionErrCod", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC314A_GetMotionErrCod([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //======================  시스템 I/O 환경 설정    ====================================================//
        //10. TMC314A_SetSvAlm
        // 지정 축의 서보 알람 입력 신호의 사용 유무 및 논리를 설정한다.                                            
        // w_Logic   : CMD_LOGIC_A(0), CMD_LOGIC_B(1)
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetSvAlm", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetSvAlm([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable, [MarshalAs(UnmanagedType.U2)] ushort wLogic);

        //11. TMC314A_GetSvAlm
        // 지정 축의 서보 알람 입력 신호의 사용 유무 및 논리를 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetSvAlm", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_GetSvAlm([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort IsEnable, [MarshalAs(UnmanagedType.U2)] ref ushort Logic);

        //12. TMC314A_SetSvInpos
        // 지정 축의 Inpositon 신호 사용 여부 및 신호 입력 레벨을 설정한다.
        // w_Logic   : CMD_LOGIC_A(0), CMD_LOGIC_B(1)
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetSvInpos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetSvInpos([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable, [MarshalAs(UnmanagedType.U2)] ushort wLogic);

        //13. TMC314A_GetSvInpos
        // 지정 축의 Inpositon 신호 사용 여부 및 신호 입력 레벨을 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetSvInpos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_GetSvInpos([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort IsEnable, [MarshalAs(UnmanagedType.U2)] ref ushort Logic);

        //14. TMC314A_SetHlmt
        // 지정 축의 Hardware Limit Sensor의 사용 유무 및 신호의 입력 레벨을 설정한다.
        // Hardware Limit Sensor 신호 입력 시 감속정지 또는 급정지에 대한 설정도 가능하다.
        // w_stop: CMD_EMG (0), CMD_DEC (1)
        // w_Logic: CMD_LOGIC_A(0), CMD_LOGIC_B(1)
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetHlmt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetHlmt([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wStopMethod, [MarshalAs(UnmanagedType.U2)] ushort wLogic);

        //15. TMC314A_SetHlmt
        // 지정 축의 Hardware Limit Sensor의 사용 유무 및 신호의 입력 레벨을 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetHlmt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_GetHlmt([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort StopMethod, [MarshalAs(UnmanagedType.U2)] ref ushort Logic);

        //16. TMC314A_SetHlmt
        // 지정 축의 Home sensor 의 입력 레벨을 설정한다.                                            
        // w_Logic: CMD_LOGIC_A(0), CMD_LOGIC_B(1)
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetOrg", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetOrg([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wLogic);

        //17. TMC314A_SetHlmt
        // 지정 축의 Home sensor 의 입력 레벨을 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetOrg", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetOrg([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //18. TMC314A_SetEncoderZ
        // 지정 축의 Z 상 레벨을 설정한다. 
        // w_Logic: CMD_LOGIC_A(0), CMD_LOGIC_B(1)
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetEncoderZ", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetEncoderZ([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wLogic);

        //18. TMC314A_GetEncoderZ
        // 지정 축의 Z 상 레벨을 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetEncoderZ", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetEncoderZ([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        //클리어 카운트는 항상 사용함 
        //19. TMC314A_SetSvCClr
        // 지정 축의 Counter Clear 입력 신호을 사용 유무 및 입력 레벨을 설정한다.
        // w_Logic: CMD_LOGIC_A(0), CMD_LOGIC_B(1)
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetSvCClr", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetSvCClr([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable, [MarshalAs(UnmanagedType.U2)] ushort wLogic);

        //20. TMC314A_GetSvCClr
        // 지정 축의 Counter Clear 입력 신호을 사용 유무 및 입력 레벨을 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetSvCClr", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_GetSvCClr([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort IsEnable, [MarshalAs(UnmanagedType.U2)] ref ushort Logic);

        //21. TMC314A_SetSvCClrTime
        // 지정 축의 Counter Clear 출력 시간을 설정한다.
        // w_Time : 0x00 = 0.01[msec]
        //          0x01 = 0.02[msec]
        //          0x02 = 0.1 [msec]
        //          0x03 = 0.2 [msec]
        //          0x04 = 1   [msec]
        //			0x05 = 2   [msec]
        //	 		0x06 = 10  [msec]
        //		 	0x07 = 20  [msec]
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetSvCClrTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetSvCClrTime([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wTime);

        //22. TMC314A_GetSvCClrTime
        // 지정 축의 Counter Clear 출력 시간을 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetSvCClrTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetSvCClrTime([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //23. TMC314A_PutSvCClrDO
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_PutSvCClrDO", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_PutSvCClrDO([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        //24. TMC314A_GetSvCClrDO
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetSvCClrDO", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetSvCClrDO([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //25. TMC314A_PutSvCClrCmd
        // 지정 축의 Counter Clear 신호를 On/Off 출력한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_PutSvCClrCmd", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_PutSvCClrCmd([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //26. TMC314A_PutSvAlmRst
        // 지정 축의 Servo-Alarm Reset 신호를 출력한다.
        // w_Status: CMD_OFF(0), CMD_ON(1)	
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_PutSvAlmRst", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_PutSvAlmRst([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        //27. TMC314A_GetSvAlmRst
        // 지정 축의 Servo-Alarm Reset 신호를 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetSvAlmRst", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetSvAlmRst([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //28. TMC314A_SetEmergency
        // 비상 정지 신호의 레벨를 설정한다.
        // w_Logic: CMD_LOGIC_A(0), CMD_LOGIC_B(1)
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetEmergency", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetEmergency([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wLogic);

        //29. TMC314A_GetEmergency
        // 비상 정지 신호의 레벨를 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetEmergency", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetEmergency([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);

        //======================  모션 제어 환경 설정    ====================================================//

        //30. TMC314A_SetRangeMode
        // 지정 축의 최대 속도를 설정한다. 
        // w_Mode:  1 = 1[PPS]   ~ 8000[PPS]
        //			10 = 10[PPS]  ~ 80000[PPS]
        //			100 = 100[PPS] ~ 800000[PPS]
        //			200 = 200[PPS] ~ 1600000[PPS]
        //			300 = 300[PPS] ~ 2400000[PPS]
        //			400 = 400[PPS] ~ 3200000[PPS]
        //			500 = 500[PPS] ~ 4000000[PPS]
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetRangeMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetRangeMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wMode);

        //31. TMC314A_GetRangeMode
        // 지정 축의 최대 속도를 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetRangeMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetRangeMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //32. TMC314A_SetPulseMode
        // 지정 축의 Command 펄스 출력 모드를 설정한다.
        // w_OutMode : 0	CW/CCW Positive (2 Pulse 정논리)
        //       	   1	CW/CCW Negative (2 Pulse 부논리)
        // 			   2	Pulse Positive/Direction Low (1 Pulse 정논리)
        //			   3	Pulse Positive/Direction High (1 Pulse 정논리)
        // 			   4	Pulse Negative/Direction Low (1 Pulse 부논리)
        //			   5	Pulse Negative/Direction High (1 Pulse 부논리)
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetPulseMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetPulseMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wOutMode);

        //33. TMC314A_GetPulseMode
        // 지정 축의 Command 펄스 출력 모드를 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetPulseMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetPulseMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //추가. TMC314A_SetPulseDir
        // 지정 축의 Feedback 펄스의 입력 모드를 설정한다.
        // w_InMode : 0	X4 (A/B상 Pulse 입력을 4체배로 Counter함)
        //            1	X2 (A/B상 Pulse 입력을 2체배로 Counter함)
        //            2	X1 (A/B상 Pulse 입력을 1체배로 Counter함)
        //            3	Up/Down (Up/Down Pulse 입력으로 Counter함)
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetPulseDir", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetPulseDir([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wInDir);

        //추가. TMC314A_GetPulseDir
        // 지정 축의 Feedback 펄스의 입력 모드를 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetPulseDir", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetPulseDir([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //34. TMC314A_SetEncoderMode
        // 지정 축의 Feedback 펄스 카운트 값의 UP/DOWN 방향을 설정한다.
        // w_InDir : 0 Feedback 카운트의 UP/DOWN 방향을 바꾸지 않습니다.
        //           1 Feedback 카운트의 UP/DOWN 방향을 반대로 합니다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetEncoderMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetEncoderMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wInMode);

        //35. TMC314A_GetEncoderMode
        // 지정 축의 Feedback 펄스 카운트 값의 UP/DOWN 방향을 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetEncoderMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetEncoderMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //36. TMC314A_SetEncoderDir
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetEncoderDir", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetEncoderDir([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wInDir);

        //37. TMC314A_GetEncoderDir
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetEncoderDir", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetEncoderDir([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //38. TMC314A_SetCompCountMode
        // 지정 축의 위치 비교 출력 모드를 설정한다.
        // w_Mode : CMD_COMM(0), CMD_FEED(1) 
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetCompCountMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetCompCountMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wCmpMode);

        //39. TMC314A_GetCompCountMode
        // 지정 축의 위치 비교 출력 모드를 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetCompCountMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetCompCountMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //40. TMC314A_SetSlmt
        // 지정 축의 소프트웨어 리미트 사용 유무 및 위치를 설정한다.
        // l_SlmtP :  -134217728 ~ 134217727
        // l_SlmtM :  -134217728 ~ 134217727
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetSlmt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetSlmt([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lSlmtP, [MarshalAs(UnmanagedType.I4)] int lSlmtM, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        //41. TMC314A_GetSlmt
        // 지정 축의 소프트웨어 리미트 사용 유무 및 위치를 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetSlmt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_GetSlmt([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] ref int SlmtP, [MarshalAs(UnmanagedType.I4)] ref int SlmtM, [MarshalAs(UnmanagedType.U2)] ref ushort IsEnable);

        //42. TMC314A_SetCounterRing
        // 지정 축의 위치 링 카운터 사용 유무 및 위치 초기화 를 설정한다.
        // dw_CommandPos :  0 ~ 134217727
        // dw_ActualPos  :  0 ~ 134217727
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetCounterRing", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetCounterRing([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] uint dwCommandPos, [MarshalAs(UnmanagedType.U4)] uint dwFeedbackPos, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        //43. TMC314A_GetCounterRing
        // 지정 축의 위치 링 카운터 사용 유무 및 위치 초기화 를 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetCounterRing", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_GetCounterRing([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] ref uint CommandPos, [MarshalAs(UnmanagedType.U4)] ref uint FeedbackPos, [MarshalAs(UnmanagedType.U2)] ref ushort IsEnable);

        //44. TMC314A_SetFilterTime
        // 지정 축의 시스템 입력신호에 대한 필터 시간를 설정한다.
        // w_Time : 0x00 	0    [msec]
        // 			0x01	0.25 [msec]
        //          0x02 	0.5  [msec]
        //          0x03	1    [msec]
        //          0x04	2    [msec]
        //          0x05	4    [msec]
        //          0x06	8    [msec]
        //          0x07	16   [msec]
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetFilterTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetFilterTime([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wTime);

        //45. TMC314A_GetFilterTime
        // 지정 축의 시스템 입력 신호에 대한 필터 시간를 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetFilterTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetFilterTime([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //46. TMC314A_SetFilterSensor
        // 지정 축의 Emergency, Hardware Limit Sensor 및 Origin(Home) Sensor 신호에 필터 레벨을 설정한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetFilterSensor", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetFilterSensor([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        //47. TMC314A_GetFilterSensor
        // 지정 축의 Emergency, Hardware Limit Sensor 및 Origin(Home) Sensor 신호에 필터 레벨을 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetFilterSensor", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetFilterSensor([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //48. TMC314A_SetFilterEncoderZ
        // 지정 축의  Z 상 입력 신호에 필터 사용 유무를 설정한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetFilterEncoderZ", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetFilterEncoderZ([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        //49. TMC314A_GetFilterEncoderZ
        // 지정 축의  Z 상 입력 신호에 필터 사용 유무를 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetFilterEncoderZ", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetFilterEncoderZ([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //50. TMC314A_SetFilterSvIF
        // 지정 축의  Servo Inposition 및 Servo Alarm  입력 신호에 필터 사용 유무를 설정한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetFilterSvIF", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetFilterSvIF([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        //51. TMC314A_GetFilterSvIF
        // 지정 축의  Servo Inposition 및 Servo Alarm  입력 신호에 필터 사용 유무를 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetFilterSvIF", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetFilterSvIF([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //======================  단축 모션 제어         ====================================================//

        //52. TMC314A_SetSpeedMode
        // 지정 축의 모션의 가감속도 프로파일을 설정한다.
        // w_SpeedMode : CMD_TMODE = 0        TRAPEZOIDAL(사다리꼴 사감속)
        //               CMD_SMODE = 1        S-CURVE (S-CURVE 가감속)
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetSpeedMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetSpeedMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wSpeedMode);

        //53. TMC314A_GetSpeedMode
        // 지정 축의 모션의 가감속도 프로파일을 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetSpeedMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetSpeedMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //54. TMC314A_SetJogSpeed
        // 지정 축의 Jog 초기속도,작업속도,가속도를 설정한다.
        // dwInitVel : 1 ~ 4000000	최소 속도 1, 최대 속도 4000000 [PPS]
        // dwWorkVel : 1 ~ 4000000	최소 속도 1, 최대 속도 4000000 [PPS]
        // dwTacc    : 1 ~ 640000(5800)   가속시간, 단위는 [msec]
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetJogSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetJogSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] uint dwStartSpeed, [MarshalAs(UnmanagedType.U4)] uint dwWorkSpeed, [MarshalAs(UnmanagedType.U4)] uint dwAccTime);

        //55. TMC314A_GetJogSpeed
        // 지정 축의 초기속도,작업속도,가속도를 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetJogSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_GetJogSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] ref uint dwpStartSpeed, [MarshalAs(UnmanagedType.U4)] ref uint dwpWorkSpeed, [MarshalAs(UnmanagedType.U4)] ref uint dwpAccTime);

        //56. TMC314A_Jog_Move
        // 지정 축의 작업속도까지 가속한 후에 작업속도를 유지하며 상위로부터의 정지명령 또는 외부로 부터 정지신호가 Active 될때까지 지정한 방향으로의 모션을 계속 수행한다.
        // w_Direction : CMD_DIR_N(0) (-) 방향, CMD_DIR_P(1) (+) 방향
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Jog_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_Jog_Move([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wDir);

        //57. TMC314A_SetPosSpeed
        // 지정 축의 Point to Point 초기속도,작업속도,가속도를 설정한다.  
        // dwInitVel : 1 ~ 4000000	최소 속도 1, 최대 속도 4000000 [PPS]
        // dwWorkVel : 1 ~ 4000000	최소 속도 1, 최대 속도 4000000 [PPS]
        // dwTacc    : 1 ~ 640000(5800)   가속시간, 단위는 [msec]
        // dwTdec    : 1 ~ 640000(5800)   감속속시간, 단위는 [msec]
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetPosSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetPosSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] uint dwStartSpeed, [MarshalAs(UnmanagedType.U4)] uint dwWorkSpeed, [MarshalAs(UnmanagedType.U4)] uint dwAccTime, [MarshalAs(UnmanagedType.U4)] uint dwDecTime);

        //58. TMC314A_GetPosSpeed
        // 지정 축의 Point to Point 초기속도,작업속도,가속도를 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetPosSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_GetPosSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] ref uint dwpStartSpeed, [MarshalAs(UnmanagedType.U4)] ref uint dwpWorkSpeed, [MarshalAs(UnmanagedType.U4)] ref uint dwpAccTime, [MarshalAs(UnmanagedType.U4)] ref uint dwpDecTime);

        //59. TMC314A_Inc_Move
        // 지정 축의 현재의 위치에서 지정한 거리(상대 거리)만큼 이동을 수행합니다. 모션을 시작시킨 후에 바로 반환합니다.
        // l_Position  : - 268,435,455 ~ 268,435,455	[pulses]
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Inc_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_Inc_Move([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lDistance);

        //60. TMC314A_Abs_Move
        // 지정 축의 현재의 위치에서 지정한 절대좌표 이동을 수행합니다. 모션을 시작시킨 후에 바로 반환합니다.
        // l_Distance   : - 268,435,455 ~ 268,435,455	[pulses]
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Abs_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_Abs_Move([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lPosition);

        //61. TMC314A_Done
        // 지정 축의 모션이 완료됐는지를 체크합니다  
        // 0 모션 작업이 완료됨
        // 1 모션 작업이 완료되지 않음
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Done", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_Done([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //62. TMC314A_Decel_Stop
        // 지정 축의 감속 후 정지를 수행합니다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Decel_Stop", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_Decel_Stop([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //63. TMC314A_Sudden_Stop
        // 지정 축의 감속없이 즉시 정지를 수행한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Sudden_Stop", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_Sudden_Stop([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        //======================  다축 동시 모션 제어         ====================================================//

        //64. TMC314A_Multi_Jog_Move
        // 여러 축에 대하여 모션 작업을 동시에 시작합니다. 
        // nAxisNum : 동시에 작업을 수행할 대상 축 개수
        // waAxisList : 동시에 작업을 수행할 대상 축의 배열 주소값
        // waDirList  : 방향을 지시하는 값의 배열 주소값
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Multi_Jog_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_Multi_Jog_Move([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNoNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] nAxisNoList, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] wDirList);

        //65. TMC314A_Multi_Abs_Move
        // 여러 축에 대하여 지정한 절대좌표로의 이동을 시작합니다.
        // nAxisNum : 동시에 작업을 수행할 대상 축 개수
        // naAxisList : 동시에 작업을 수행할 대상 축의 배열 주소값
        // laPosList : 이동할 거리값의 배열 주소값
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Multi_Abs_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_Multi_Abs_Move([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNoNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] nAxisNoList, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] lDisList);

        //66. TMC314A_Multi_Inc_Move
        // 여러 축에 대하여 현재의 위치에서 지정한 거리만큼 이동을 동시에 시작합니다.
        // nAxisNum : 동시에 작업을 수행할 대상 축 개수
        // naAxisList : 동시에 작업을 수행할 대상 축의 배열 주소값
        // laDisList : 이동할 거리값의 배열 주소값
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Multi_Inc_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_Multi_Inc_Move([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNoNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] nAxisNoList, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] lPosList);

        //67. TMC314A_Multi_Done
        // 여러 축에 대하여 지정한 모든 축의 모션이 완료됐는지를 체크합니다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Multi_Done", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_Multi_Done([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNoNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] nAxisNoList);

        //68. TMC314A_Multi_Decel_Stop
        // 여러 축에 대하여 감속 후 정지를 수행합니다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Multi_Decel_Stop", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_Multi_Decel_Stop([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNoNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] nAxisNoList);

        //69. TMC314A_Multi_Sudden_Stop
        // 여러 축에 대하여 감속없이 즉시 정지를 수행한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Multi_Sudden_Stop", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_Multi_Sudden_Stop([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNoNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] nAxisNoList);

        //======================  원점 복귀                 ====================================================//

        //70. TMC314A_SetHomeDir
        // 지정 축의 원점 복귀 방향을 설정한다.
        // w_HomDir : CMD_CW (0), CMD_CCW (1)
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetHomeDir", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetHomeDir([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wHomDir);

        //71. TMC314A_GetHomeDir
        // 지정 축의 원점 복귀 방향을 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetHomeDir", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetHomeDir([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //72. TMC314A_SetHomeMode
        // 지정 축의 원점 복귀 모드를 설정한다.  
        // w_HomMode : 0	저속 ORIGIN 원점 (ORG)
        //		       1	저속 ORIGIN 원점 (ORG + EZ)
        //	 	       2	고속 ORIGIN 원점 (ORG)
        //	 	       3	고속 ORIGIN 원점 (ORG + EZ)
        //	 	       4	저속 LIMIT 원점 (LMT)
        //	 	       5	저속 LIMIT 원점 (LMT + EZ)
        //	 	       6	고속 LIMIT 원점 (LMT)
        //	 	       7	고속 LIMIT 원점 (LMT + EZ)
        //	 	       8	저속 EZ 원점 (EZ)
        //	 		   9	현재 위치 ORG 원점
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetHomeMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetHomeMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wHomMode);

        //73. TMC314A_GetHomeMode
        // 지정 축의 원점 복귀 모드를 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetHomeMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetHomeMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //74. TMC314A_SetHomeSpeed
        // 지정 축의 원점 복귀 속도 및 가감속 시간을 설정한다.
        // dwInitVel : 1 ~ 4000000	최소 속도 1, 최대 속도 4000000 [PPS]
        // dwWorkVel : 1 ~ 4000000	최소 속도 1, 최대 속도 4000000 [PPS]
        // dwTacc    : 1 ~ 640000(5800)   가속시간, 단위는 [msec]
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetHomeSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetHomeSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] uint dwStartSpeed, [MarshalAs(UnmanagedType.U4)] uint dwWorkSpeed, [MarshalAs(UnmanagedType.U4)] uint dwAccTime);

        //75. TMC314A_GetHomeSpeed
        // 지정 축의 원점 복귀 속도 및 가감속 시간을 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetHomeSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_GetHomeSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] ref uint dwpStartSpeed, [MarshalAs(UnmanagedType.U4)] ref uint dwpWorkSpeed, [MarshalAs(UnmanagedType.U4)] ref uint dwpAccTime);

        //76. TMC314A_SetHomeOffset
        // 지정 축의 원점 복귀시 기계적인 원점이 아닌 사용자가 임의적으로 작업 원점을 설정한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetHomeOffset", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetHomeOffset([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lHomOffset);

        //77. TMC314A_GetHomeOffset
        // 지정 축의 원점 복귀시 기계적인 원점이 아닌 사용자가 임의적으로 작업 원점을 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetHomeOffset", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC314A_GetHomeOffset([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //78. TMC314A_Home_Move
        // 지정 축의 원점 복귀 작업을 수행한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Home_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_Home_Move([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //79. TMC314A_Multi_Home_Move
        // 여러 축에 대한 원점 복귀 작업을 동시에 수행한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Multi_Home_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_Multi_Home_Move([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNoNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] nAxisNoList);


        //======================  속도 및 위치 오버라이딩    ====================================================//


        //80. TMC314A_OverrideSpeed
        // 지정 축의 모션이 진행되고 있는 중에 속도를 오버라이딩를 설정한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_OverrideSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_OverrideSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] uint dwNewWorkSpeed);

        //81. TMC314A_Inc_OverrideMove
        // 지정 축의 모션이 진행되고 있는 중에 상대 위치를 오버라이딩를 설정한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Inc_OverrideMove", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_Inc_OverrideMove([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lNewDistance);

        //82. TMC314A_Abs_OverrideMove
        // 지정 축의 모션이 진행되고 있는 중에 절대 위치를 오버라이딩를 설정한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Abs_OverrideMove", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_Abs_OverrideMove([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lNewPosition);

        //======================  보간 모션 제어 (추가)    ====================================================//

        // TMC314A_SetIPMapAxes
        // nMapNo		: 보간설정하기 전에 반드시 0으로 설정
        // wMapIndex	: BIT 0		: 0x01
        //				  BIT 1		: 0x02
        //				  BIT 2		: 0x04
        //				  BIT 3		: 0x08
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetIPMapAxes", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetIPMapAxes([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wMapIndex);

        // TMC314A_SetIPPosSpeed
        // 보간 이송의 속도를 설정한다.
        // dwInitVel : 1 ~ 4000000	최소 속도 1, 최대 속도 4000000 [PPS]
        // dwWorkVel : 1 ~ 4000000	최소 속도 1, 최대 속도 4000000 [PPS]
        // dwTacc    : 1 ~ 640000(5800)   가속시간, 단위는 [msec]
        // dwTdec	 : 1 ~ 640000(5800)   감속시간, 단위는 [msec]
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetIPPosSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetIPPosSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] uint dwInitVel, [MarshalAs(UnmanagedType.U4)] uint dwWorkVel, [MarshalAs(UnmanagedType.U4)] uint dwTacc, [MarshalAs(UnmanagedType.U4)] uint dwTdec);

        // TMC314A_GetIPPosSpeed
        // 보간 이송의 속도를 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetIPPosSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_GetIPPosSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] ref uint dwInitVel, [MarshalAs(UnmanagedType.U4)] ref uint dwWorkVel, [MarshalAs(UnmanagedType.U4)] ref uint dwTacc, [MarshalAs(UnmanagedType.U4)] ref uint dwTdec);

        // TMC314A_Inc_Line
        // 두 축 이상에 대하여 현재의 위치에서 지정한 거리만큼 직선 보간 운동을 시작합니다.
        // laDisList : 이동할 거리값의 배열 주소값
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Inc_Line", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_Inc_Line([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] ushort[] laDisList);

        // TMC314A_Abs_Line
        // 두 축 이상에 대하여 절대 위치까지 직선 보간 운동을 시작합니다.
        // laDisList : 이동할 거리값의 배열 주소값
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Abs_Line", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_Abs_Line([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] ushort[] laDisList);

        // TMC314A_Inc_Arc
        // 두 축에 대하여 절대 위치까지 원호 보간 운동을 시작합니다.
        // lXCentOffset : 주축 중심값
        // lYCentOffset : 보조축 중심값
        // lXEndPtDist  : 주축 종심값 
        // lYEndPtDist  : 보조축 종심값 
        // wDirection 회전방향 0 : CW, 1 : CCW
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Inc_Arc", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_Inc_Arc([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lXCentOffset, [MarshalAs(UnmanagedType.I4)] int lYCentOffset, [MarshalAs(UnmanagedType.I4)] int lXEndPtDist, [MarshalAs(UnmanagedType.I4)] int lYEndPtDist, [MarshalAs(UnmanagedType.U2)] ushort wDirection);

        // TMC314A_Abs_Arc
        // 두 축에 대하여 현재의 위치에서 지정한 거리만큼 원호 보간 운동을 시작합니다.
        // lXCentOffset : 주축 중심값
        // lYCentOffset : 보조축 중심값
        // lXEndPtDist  : 주축 종심값 
        // lYEndPtDist  : 보조축 종심값 
        // wDirection 회전방향 0 : CW, 1 : CCW
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Abs_Arc", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_Abs_Arc([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lXCentOffset, [MarshalAs(UnmanagedType.I4)] int lYCentOffset, [MarshalAs(UnmanagedType.I4)] int lXEndPtDist, [MarshalAs(UnmanagedType.I4)] int lYEndPtDist, [MarshalAs(UnmanagedType.U2)] ushort wDirection);

        // TMC314A_Inc_Theta
        // 두 축에 대하여 현재의 위치에서 지정한 각도만큼 원호 보간 운동을 시작합니다.
        // lCentOffset : 주축   중심값
        // lCentOffset : 보조축 중심값
        // lEnaAngle   : +이면 CCW 방향 
        //             : -이면 CW  방향
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Inc_Theta", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_Inc_Theta([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lXCentOffset, [MarshalAs(UnmanagedType.I4)] int lYCentOffset, [MarshalAs(UnmanagedType.I4)] int lEndAngle);

        // TMC314A_Abs_Theta
        // 두 축에 대하여 절대 좌표에서 지정한 각도만큼 원호 보간 운동을 시작합니다.
        // lCentOffset : 주축   중심값
        // lCentOffset : 보조축 중심값
        // lEnaAngle   : +이면 CCW 방향 
        //             : -이면 CW  방향
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Abs_Theta", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_Abs_Theta([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lXCentOffset, [MarshalAs(UnmanagedType.I4)] int lYCentOffset, [MarshalAs(UnmanagedType.I4)] int lEndAngle);

        // TMC314A_IPDone
        // 보간 지정 축의 모션이 완료됐는지를 체크합니다.
        // 0 모션 작업이 완료됨
        // 1 모션 작업이 완료되지 않음
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_IPDone", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_IPDone([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // TMC314A_IPDecel_Stop
        // 보간 지정 축에 대해서 감속 정지를 수행한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_IPDecel_Stop", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_IPDecel_Stop([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // TMC314A_IPSudden_Stop
        // 보간 지정 축에 대해서 감속없이 즉시 정지를 수행한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_IPSudden_Stop", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_IPSudden_Stop([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // TMC314A_SetInitContiTable
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetInitContiTable", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern uint TMC314A_SetInitContiTable();

        // TMC314A_SetFreeContiTable
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetFreeContiTable", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern uint TMC314A_SetFreeContiTable();

        // TMC314A_Conti_Move
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Conti_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_Conti_Move([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // TMC314A_Const_Move
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Const_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_Const_Move([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //====================== 비교기 (추가) ====================================================//

        // TMC314A_SetCompTrgWidth
        // 지정 축의 비교 위치 출력 펄스 폭를 설정한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetCompTrgWidth", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetCompTrgWidth([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] uint dwCmpWidth);

        // TMC314A_GetCompTrgWidth
        // 지정 축의 비교 위치 출력 펄스 폭를 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetCompTrgWidth", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern uint TMC314A_GetCompTrgWidth([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // TMC314A_SetCompTrgMode
        // 지정 축의 비교 위치 출력 사용유무를 설정한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetCompTrgMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetCompTrgMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wCmpMode);

        // TMC314A_GetCompTrgMode
        // 지정 축의 비교 위치 출력 사용유무를 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetCompTrgMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_GetCompTrgMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ref ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort wCmpMode);

        // TMC314A_SetCompTrgOneData
        // 지정 축의 위치비교출력기에 1회 비교데이터를 설정한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetCompTrgOneData", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetCompTrgOneData([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lStartData);

        // TMC314A_SetCompTrgTable
        // 지정 축의 연속적인 위치 비교 출력 기능을 사용하기 위해서 연속적인위치 데이터를 등록한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetCompTrgTable", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetCompTrgTable([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort nNumData, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] lPositionList);

        // TMC314A_SetCompTrgContTable
        // 지정 축의 연속적인 위치 비교 출력 기능을 사용하기 위해서 일정한 위치 간격을 가지는 연속적인 위치 데이터를 자동으로 생성하여 등록한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetCompTrgContTable", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetCompTrgContTable([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort nNumData, [MarshalAs(UnmanagedType.I4)] int lStartData, [MarshalAs(UnmanagedType.I4)] int lInterval);

        // TMC314A_SetInitCompTrg
        // 연속적인 위치 비교 출력 기능을 시작한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetInitCompTrg", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetInitCompTrg([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);

        // TMC314A_SetFreeCompTrg
        // 연속적인 위치 비교 출력 기능을 종료한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetFreeCompTrg", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetFreeCompTrg([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);


        //======================  모션 시스템 상태 모니터링 및 위치 및 속도 관리   ====================================================//

        //86. TMC314A_GetCardStatus
        // 지정 축의 모션 과 관련된 시스템의 입축력 상태를 반환한다.
        // Bit0	ORG	Origin(Home) Sensor (원점 센서) [Software Check]
        // Bit1		EZ	Encoder Z (엔코더 Z상) [Software Check]
        // Bit2		EMG	Emergency (비상정지) [Hardware Check]
        // Bit3		INP	Servo Inposition (서보 위치 결정 완료) [Software Check]
        // Bit4		ALM	Servo Alarm (서보 알람) [Software Check]
        // Bit5		LMT+	+Limit Sensor (+리미트 센서) [Software Check]
        // Bit6		LMT-	-Limit Sensor (-리미트 센서) [Software Check]
        // Bit7		NC	NC
        // Bit8		RUN	Motion 동작 수행 중
        // Bit9		ERR	Error 발생
        // Bit10	HOME	원점 복귀 Motion 수행 중
        // Bit11	H_OK	원점 복귀 Motion 완료
        // Bit12	NC	NC
        // Bit13	C.CLR	Servo Error Counter Clear (서보 편차 카운터 클리어)
        // Bit14	SON	Servo On (서보 온)
        // Bit15	A.RST	Servo Alarm Reset (서보 알람 리셋)
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetCardStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetCardStatus([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //87. TMC314A_GetMainStatus
        // 지정 축의 모션 과 에러 상태를 반환한다. 
        // Bit0		RUN	0축 Motion 동작 수행 중
        // Bit1		RUN	1축 Motion 동작 수행 중
        // Bit2		RUN	2축 Motion 동작 수행 중
        // Bit3		RUN	3축 Motion 동작 수행 중
        // Bit4		ERR	0축 ERROR (Error 발생)
        // Bit5		ERR +	1축 ERROR (Error 발생)
        // Bit6		ERR -	2축 ERROR (Error 발생)
        // Bit7		ERR	3축 ERROR (Error 발생)
        // Bit8		HOME	0축 원점 복귀 Motion 수행 중
        // Bit9		HOME	1축 원점 복귀 Motion 수행 중
        // Bit10	HOME	2축 원점 복귀 Motion 수행 중
        // Bit11	HOME	3축 원점 복귀 Motion 수행 중
        // Bit12	NC	NC
        // Bit13	NC	NC
        // Bit14	NC	NC
        // Bit15	NC	NC
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetMainStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetMainStatus([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //88. TMC314A_GetDrvStatus
        // 지정 축의 모션의 동작 및 정지 상태를 반환한다.
        // Bit0		CMP+	현재 Position 값이 COMP+ 값보다 크거나 같을 때
        // Bit1		CMP-	현재 Position 값이 COMP- 값보다 작을 때
        // Bit2		ASND	직선 가감속에서 가속할 때
        // Bit3		CNST	직선 가감속에서 등속할 때
        // Bit4		DSND	직선 가감속에서 감속할 때
        // Bit5		AASND	S자 가감속에서 가감속도가 증가할 때
        // Bit6		ACNST	S자 가감속에서 가감속도가 일정할 때
        // Bit7		ADSND	S자 가감속에서 가감속도가 감소할 때
        // Bit8		NC	NC
        // Bit9		S-ORG	ORG 신호에 의해 정지할 때
        // Bit10	S-EZ	EZ 신호에 의해 정지할 때
        // Bit11	NC	
        // Bit12	S-LMT+	+LMT 신호에 의해 정지할 때
        // Bit13	S-LMT-	-LMT 신호에 의해 정지할 때
        // Bit14	S-ALM	ALM 신호에 의해 정지할 때
        // Bit15	S-EMG	EMG 신호에 의해 정지할 때
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetDrvStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetDrvStatus([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //89. TMC314A_GetErrStatus
        // 지정 축의 현재 모션의 에러 상태를 반환한다.  
        // Bit0		E-SLMT+	+Soft Limit가 동작했을 때
        // Bit1		E-SLMT-	-Soft Limit가 동작했을 때
        // Bit2		E-HLMT+	LMT+ 신호가 동작했을 때
        // Bit3		E-HLMT-	LMT- 신호가 동작했을 때
        // Bit4		E-ALM	ALM 신호가 동작했을 때
        // Bit5		E-EMG	EMG 신호가 동작했을 때
        // Bit6		NC	NC
        // Bit7		E-HOME	원점 복귀 Motion 수행 중 Error가 발생했을 때
        // Bit8		HMST0	원점 복귀 Motion 수행 중 Step 동작 내용(참고 참조)
        // Bit9		HMST1	원점 복귀 Motion 수행 중 Step 동작 내용(참고 참조)
        // Bit10	HMST2	원점 복귀 Motion 수행 중 Step 동작 내용(참고 참조)
        // Bit11	HMST3	원점 복귀 Motion 수행 중 Step 동작 내용(참고 참조)
        // Bit12	HMST4	원점 복귀 Motion 수행 중 Step 동작 내용(참고 참조)
        // Bit13	NC	NC
        // Bit14	NC	NC
        // Bit15	NC	NC
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetErrStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetErrStatus([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //90. TMC314A_GetInputStatus
        // 지정 축의 현재 모션의 시스템 입출력 상태를 반환한다.
        // Bit0		NC	NC
        // Bit1		ORG	Origin(Home) Sensor (원점 센서) 감지 됨[Hardware Check]
        // Bit2		EZ	Encoder Z (엔코더 Z상) 감지 됨[Hardware Check]
        // Bit3		EMG	EMG 신호 감지 됨
        // Bit4		EX+	외부에서 + 방향의 드라이브
        // Bit5		EX-	외부에서 - 방향의 드라이브
        // Bit6		INP	Servo InPosition (서보 위치 결정 완료) 감지 됨[Hardware Check]
        // Bit7		ALM	Servo Alarm (서보 알람) 감지 됨[Hardware Check]
        // Bit8		NC	NC
        // Bit9		NC	NC
        // Bit10	NC	NC
        // Bit11	NC	NC
        // Bit12	NC	NC
        // Bit13	NC	NC
        // Bit14	LMT+	+Limit Sensor (+리미트 센서) 감지 됨[Hardware Check]
        // Bit15	LMT-	Limit Sensor (-리미트 센서) 감지 됨[Hardware Check]
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetInputStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetInputStatus([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //91. TMC314A_GetEventStatus
        //지정 축의 현재 모션의 이벤트 상태를 반환한다.
        // Bit0		NC	NC
        // Bit1		P>=C-	펄스 카운트가 CMP- 값 보다 같거나 큼
        // Bit2		P<C-	펄스 카운트가 CMP- 값 보다 작음
        // Bit3		P<C+	펄스 카운트가 CMP+ 값 보다 작음
        // Bit4		P>=C+	펄스 카운트가 CMP+ 값 보다 같거나 큼
        // Bit5		C-END	정속 Motion 종료
        // Bit6		C-STA	정속 Motion 시작
        // Bit7		D-END	Motion 종료
        // Bit8		NC	NC
        // Bit9		NC	NC
        // Bit10	NC	NC
        // Bit11	NC	NC
        // Bit12	NC	NC
        // Bit13	NC	NC
        // Bit14	NC	NC
        // Bit15	NC	NC
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetEventStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetEventStatus([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //92. TMC314A_SetCommandPos
        // 지정 축의 지령위치(Command)값을 새로이 설정한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetCommandPos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetCommandPos([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lCommandPos);

        //93. TMC314A_GetCommandPos
        // 지정 축의 지령위치(Command)값을 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetCommandPos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC314A_GetCommandPos([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //94. TMC314A_SetActualPos
        // 지정 축의 피드백위치(Feedback)값을 새로이 설정한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetActualPos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetActualPos([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lFeedbackPos);

        //95. TMC314A_GetActualPos
        // 지정 축의 피드백위치(Feedback)값을 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetActualPos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC314A_GetActualPos([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //96. TMC314A_GetCommandSpeed
        // 지정 축의 지령 속도값을 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetCommandSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC314A_GetCommandSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        //======================  범용 디지털 입출력 ====================================================//

        // TMC314A_PutDO
        // 32 점씩 Digital Output 채널에 출력한다.  ( Classic )
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_PutDO", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_PutDO([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U4)] uint wOutStatus);

        // TMC314A_GetDO
        // 32 점씩 Digital Input 채널에 반환한다.  ( Classic )
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetDO", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern uint TMC314A_GetDO([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);

        //108. TMC314A_PutDOBit
        // 지정한 해당 채널에 신호를 출력한다.
        // nChannelNo : 각 비트값
        // 0 ~ 31:
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_PutDOBit", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_PutDOBit([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nChannelNo, [MarshalAs(UnmanagedType.U2)] ushort wOutStatus);

        //109. TMC314A_GetDOBit
        // 지정한 해당 채널에 출력 신호를 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetDOBit", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetDOBit([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nChannelNo);

        //110. TMC314A_PutDOByte
        // 8 점씩 Digital Output 채널에 출력한다.
        // nGroupNo : 0 	CH0  ~ CH7
        //		 	  1		CH8  ~ CH15
        // 			  2 	CH16 ~ CH23
        //            3		CH24 ~ CH31
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_PutDOByte", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_PutDOByte([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U1)] byte bOutStatus);

        //111. TMC314A_GetDOByte
        // 8 점씩 Digital Output 채널에 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetDOByte", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_GetDOByte([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U1)] ref byte bpOutStatus);

        //112. TMC314A_PutDOWord
        // 16 점씩 Digital Output 채널에 출력한다.
        // nGroupNo : 0 	CH0  ~ CH15
        // 			  1 	CH16 ~ CH31
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_PutDOWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_PutDOWord([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U2)] ushort wOutStatus);

        //113. TMC314A_GetDOWord
        // 16 점씩 Digital Output 채널에 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetDOWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_GetDOWord([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U2)] ref ushort wpOutStatus);

        //113. TMC314A_PutDODWord
        // 32 점씩 Digital Output 채널에 출력한다.
        // nGroupNo : 0 	CH0  ~ CH31
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_PutDODWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_PutDODWord([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U4)] uint dwOutStatus);

        //114. TMC314A_GetDODWord
        // 32 점씩 Digital Output 채널에 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetDODWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_GetDODWord([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U4)] ref uint dwpOutStatus);

        // TMC314A_GetDI
        // 32 점씩 Digital Input 채널에 반환한다. ( Classic )
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetDI", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern uint TMC314A_GetDI([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);

        //116. TMC314A_GetDIBit
        // 지정한 해당 채널에 입력 신호를 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetDIBit", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetDIBit([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nChannelNo);

        //117. TMC314A_GetDIByte
        // 8 점씩 Digital Input 채널에 반환한다.
        // nGroupNo : 0 	CH0  ~ CH7
        // 			  1		CH8  ~ CH15
        // 			  2 	CH16 ~ CH23
        //            3		CH24 ~ CH31
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetDIByte", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_GetDIByte([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U1)] ref byte bpInStatus);

        //118. TMC314A_GetDIWord
        // 16 점씩 Digital Input 채널에 반환한다. 
        // nGroupNo : 0 	CH0  ~ CH15
        //			  1 	CH16 ~ CH31
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetDIWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_GetDIWord([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U2)] ref ushort wpInStatus);

        //119. TMC314A_GetDIDWord
        // 32 점씩 Digital Input 채널에 반환한다. 
        // nGroupNo : 0 	CH0  ~ CH31
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetDIDWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_GetDIDWord([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U4)] ref uint dwpInStatus);

        //120. TMC314A_SetDiFilter		
        // Digital 입력 신호 필터 사용 유무를 설정한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetDiFilter", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetDiFilter([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        //121. TMC314A_GetDiFilter
        // Digital 입력 신호 필터 사용 유무를 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetDiFilter", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_GetDiFilter([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);

        //122. TMC314A_SetDiFilterTime
        // Digital 입력 신호 필터 시간를 설정한다.
        // 0	1.00(μsec)	0.875(μsec)	8	0.256(msec)	0.224(msec)
        // 1	2.00(μsec)	1.75(μsec)		9	0.512(msec)	0.448(msec)
        // 2	4.00(μsec)	3.50(μsec)		A	1.02(msec)	0.896(msec)
        // 3	8.00(μsec)	7.00(μsec)		B	2.05(msec)	1.79(msec)
        // 4	16.0(μsec)	14.0(μsec)		C	4.10(msec)	3.58(msec)
        // 5	32.0(μsec)	28.0(μsec)		D	8.19(msec)	7.17(msec)
        // 6	64.0(μsec)	56.0(μsec)		E	16.4(msec)	14.3(msec)
        // 7	128(μsec)	112(μsec)		F	32.8(msec)	28.7(msec)
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetDiFilterTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetDiFilterTime([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wTime);

        //123. TMC314A_GetDiFilterTime
        // Digital 입력 신호 필터 시간를 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetDiFilterTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetDiFilterTime([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);


        //======================  외부 신호에 의한 모션 제어    ====================================================//

        //124. TMC314A_SetExtMode
        // 지정 축의 MPG(수동펄스) 사용 유무 설정한다.
        // w_Mode : CMD_DISABLE(0) , CMD_ENABLE(1) 
        // w_Rate : 1 ~ 10000 
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetExtMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetExtMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wMode, [MarshalAs(UnmanagedType.U2)] ushort wRate);

        //125. TMC314A_GetExtMode
        // 지정 축의 MPG(수동펄스) 사용 유무 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetExtMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_GetExtMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort wMode, [MarshalAs(UnmanagedType.U2)] ref ushort wRate);

        //126. TMC314A_SetFilterExt
        // 지정 축의 MPG(수동펄스) 필터 사용 유무 설정한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetFilterExt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetFilterExt([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        //127. TMC314A_GetFilterExt
        // 지정 축의 MPG(수동펄스) 필터 사용 유무 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetFilterExt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetFilterExt([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);



        //====================== Advanced FUNCTIONS ===================================================//

        //128. TMC314A_HomeIsBusy
        // 지정 축의 원점 작업 상태 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_HomeIsBusy", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_HomeIsBusy([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        //129. TMC314A_SetHomeSuccess
        // 지정 축의 원점 작업 성공 여부 설정한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetHomeSuccess", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetHomeSuccess([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);


        //130. TMC314A_GetHomeSuccess
        // 지정 축의 원점 작업 성공 여부 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetHomeSuccess", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetHomeSuccess([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        //131. TMC314A_SetFixedRange
        // 지정 축의 최고 속도 배율를 설정한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetFixedRange", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetFixedRange([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);


        //132. TMC314A_GetFixedRange
        // 지정 축의 최고 속도 배율를 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetFixedRange", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetFixedRange([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        //133. TMC314A_GetBoardID
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetBoardID", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetBoardID([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);

        //134. TMC314A_GetAxisNum
        // 지정된 컨트롤러에서 지원하는 제어축 수를 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetAxisNum", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetAxisNum([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);

        //135. TMC314A_GetDiNum
        // 지정된 컨트롤러에서 지원하는 범용 디지털입력 채널 수를 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetDiNum", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetDiNum([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);

        //136. TMC314A_GetDoNum
        // 지정된 컨트롤러에서 지원하는 범용 디지털출력 채널 수를 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetDoNum", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetDoNum([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);

        //137. TMC314A_LogCheck
        // 로그 파일 생성 유무를 설정한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_LogCheck", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_LogCheck([MarshalAs(UnmanagedType.U2)] ushort wLogCheck);

        //138. TMC314A_PutSvRun
        // 지정된 컨트롤러의 LED 을 통해 보드 확인한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_PutSvRun", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_PutSvRun([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        //139. TMC314A_GetSvRun
        // 지정된 컨트롤러의 LED 을 통해 보드 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetSvRun", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetSvRun([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //140. TMC314A_SetBlockMode
        // Block Mode 동작 모드를 설정한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetBlockMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetBlockMode([MarshalAs(UnmanagedType.U2)] ushort wBlocking);

        //141. TMC314A_GetBlockMode
        // Block Mode 동작 모드를 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetBlockMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC314A_GetBlockMode();

        //142. TMC314A_SetAccOffset
        // 지정 축의 가속 옵셋 카운트 설정한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetAccOffset", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetAccOffset([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lOffset);

        //143. TMC314A_GetAccOffset
        // 지정 축의 가속 옵셋 카운트 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetAccOffset", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC314A_GetAccOffset([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //144. TMC314A_SaveFile
        // 설정된 파라미터를 저장한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SaveFile", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SaveFile([MarshalAs(UnmanagedType.U2)] ushort nConNo);

        //145. TMC314A_BoardInfo
        // 컨트롤러의 정보를 반환한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_BoardInfo", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_BoardInfo([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U4)] ref uint dwpBoard, [MarshalAs(UnmanagedType.U4)] ref uint dwpComm, [MarshalAs(UnmanagedType.U4)] ref uint dwpAxis, [MarshalAs(UnmanagedType.U4)] ref uint dwpDiNum, [MarshalAs(UnmanagedType.U4)] ref uint dwpDoNum);


        //====================== 인터럽트 ( 추가 ) ===================================================//

        // TMC314A_SetEventEnable
        // 컨트롤러의 인터럽트 사용 유무를 설정한다.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetEventEnable", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetEventEnable([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wIsEvent);

        ///hWnd : 윈도우 핸들, 윈도우 메세지를 받을때 사용. 사용하지 않으면 NULL을 입력.
        //wMsg : 윈도우 핸들의 메세지, 사용하지 않거나 디폴트값을 사용하려면 0을 입력.
        //129. TMC314A_SetEventHandler
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetEventHandler", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetEventHandler([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort wIsEvent, [MarshalAs(UnmanagedType.U4)] ref uint hWnd, [MarshalAs(UnmanagedType.U4)] uint wMsg);

        // 지정 축의 사용자가 설정한 인터럽트 발생 여부를 설정한다.
        //EVT_NONE  = &H0  disable all event
        //EVT_C_END = &H1  C-END,    interrupt active when end of constant drive
        //EVT_C_STA = &H2  C-STA,    interrupt active when start of constant drive
        //EVT_D_END = &H4  D-END,    interrupt active when end of drive
        //130. TMC314A_SetMotEventMask
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetMotEventMask", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetMotEventMask([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEventMask);

        // 지정 축의 사용자가 설정한 인터럽트 발생 여부를 읽는다.
        //131. TMC314A_GetMotEventStatus
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetMotEventStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_GetMotEventStatus([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort wEventStatus);


        // 지정 채널의의 사용자가 설정한 인터럽트 발생 여부를 설정한다.
        // nChNoMask1 : CH0  ~ CH31
        // nChNoMask2 : CH32 ~ CH63
        //132. TMC314A_SetDiEventMask
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetDiEventMask", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_SetDiEventMask([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U4)] uint dwChannelMask1, [MarshalAs(UnmanagedType.U4)] uint dwChannelMask2);


        // 지정 채널의의 사용자가 설정한 인터럽트 발생 여부를 읽는다.
        // nChNoMask1 : CH0  ~ CH31
        // nChNoMask2 : CH32 ~ CH63
        //133. TMC314A_GetDiEventStatus
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetDiEventStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_GetDiEventStatus([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U4)] ref uint dwChannelStatus1, [MarshalAs(UnmanagedType.U4)] ref uint dwChannelStatus2);
    }
}
