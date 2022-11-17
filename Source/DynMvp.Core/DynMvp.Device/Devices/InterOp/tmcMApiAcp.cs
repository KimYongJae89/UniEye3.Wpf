///******************************************************************************
//*
//*	File Version: 1,0,0,0
//*
//*	Copyright (c) Alpha Motion 2011-
//*
//*	This file is strictly confidential and do not distribute it outside.
//*
//*	MODULE NAME :
//*		tmcMApiAcp.cs
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
    /// Import tmcMApiAcp에 대한 요약 설명입니다.
    /// </summary>
    public class TMCACDLL
    {
        public delegate void EventFunc(IntPtr lParam);


        //======================  Loading/Unloading function ====================================================//

        // 하드웨어 장치를 로드하고 초기화한다.
        // 1. TMC304A_LoadDevice
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_LoadDevice", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC304A_LoadDevice();

        // 하드웨어 장치를 언로드한다.
        // 2. TMC304A_UnloadDevice
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_UnloadDevice", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_UnloadDevice();


        //======================  						장치 초기화               ====================================================//

        // 하드웨어 및 소프트웨어를 초기화한다.
        // 3. TMC304A_Reset
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Reset", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Reset([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);

        // 소프트웨어를 초기화한다.			
        // 4. TMC304A_SetSystemDefault
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetSystemDefault", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetSystemDefault([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정 축의 Servo-On 신호를 출력한다.
        // 5. TMC304A_PutSvOn
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_PutSvOn", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_PutSvOn([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // 지정 축의 Servo-On신호의 출력 상태을 반환한다.
        // 6. TMC304A_GetSvOn
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetSvOn", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetSvOn([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        //======================  에러 처리               ====================================================//

        // 가장 최근에 실행된 함수의 에러코드를 반환한다.
        // 7. TMC304A_GetErrorCode
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetErrorCode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC304A_GetErrorCode();

        // 가장 최근에 실행된 함수의 에러코드를 문자열로 변환해줍니다.
        // 8. TMC304A_GetErrorString
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetErrorString", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string TMC304A_GetErrorString([MarshalAs(UnmanagedType.I4)] int nErrorCode);

        // 모션 완료 후 모션 정지 원인를 반환한다.
        // 9. TMC304A_GetErrorString
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetMotionErrCod", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC304A_GetMotionErrCod([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        //======================  시스템 I/O 환경 설정    ====================================================//

        // 지정 축의 서보 알람 입력 신호의 사용 유무 및 논리를 설정한다.
        // 10. TMC304A_SetSvAlm
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetSvAlm", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetSvAlm([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable, [MarshalAs(UnmanagedType.U2)] ushort wLogic);

        // 지정 축의 서보 알람 입력 신호의 사용 유무 및 논리를 반환한다.
        // 11. TMC304A_GetSvAlm
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetSvAlm", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetSvAlm([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort IsEnable, [MarshalAs(UnmanagedType.U2)] ref ushort Logic);

        // 지정 축의 Inpositon 신호 사용 여부 및 신호 입력 레벨을 설정한다.
        // 12. TMC304A_SetSvInpos
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetSvInpos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetSvInpos([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable, [MarshalAs(UnmanagedType.U2)] ushort wLogic);

        // 지정 축의 Inpositon 신호 사용 여부 및 신호 입력 레벨을 반환한다.
        // 13. TMC304A_GetSvInpos
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetSvInpos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetSvInpos([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort IsEnable, [MarshalAs(UnmanagedType.U2)] ref ushort Logic);

        // 지정 축의 Hardware Limit Sensor의 사용 유무 및 신호의 입력 레벨을 설정한다.
        // Hardware Limit Sensor 신호 입력 시 감속정지 또는 급정지에 대한 설정도 가능하다.
        // w_stop: CMD_EMG (0), CMD_DEC (1)
        // w_Logic: CMD_LOGIC_A(0), CMD_LOGIC_B(1)
        // 14. TMC304A_SetHlmt
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetHlmt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetHlmt([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wStopMethod, [MarshalAs(UnmanagedType.U2)] ushort wLogic);

        // 지정 축의 Hardware Limit Sensor의 사용 유무 및 신호의 입력 레벨을 반환한다.
        // 15. TMC304A_SetHlmt
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetHlmt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetHlmt([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort StopMethod, [MarshalAs(UnmanagedType.U2)] ref ushort Logic);

        // 지정 축의 Home sensor 의 입력 레벨을 설정한다. 
        // w_Logic: CMD_LOGIC_A(0), CMD_LOGIC_B(1)
        // 16. TMC304A_SetOrg
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetOrg", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetOrg([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wLogic);

        // 지정 축의 Home sensor 의 입력 레벨을 반환한다.
        // 17. TMC304A_GetOrg
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetOrg", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetOrg([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정 축의 Z 상 레벨을 설정한다.
        // w_Logic: CMD_LOGIC_A(0), CMD_LOGIC_B(1)
        // 18. TMC304A_SetEncoderZ
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetEncoderZ", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetEncoderZ([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wLogic);

        // 지정 축의 Z 상 레벨을 반환한다.
        // 19. TMC304A_GetEncoderZ
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetEncoderZ", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetEncoderZ([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        //===================클리어 카운트는 항상 사용함 

        // 지정 축의 Counter Clear 입력 신호을 사용 유무 및 입력 레벨을 설정한다.
        // w_Logic: CMD_LOGIC_A(0), CMD_LOGIC_B(1)
        // 20. TMC304A_SetSvCClr
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetSvCClr", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetSvCClr([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable, [MarshalAs(UnmanagedType.U2)] ushort wLogic);

        // 지정 축의 Counter Clear 입력 신호을 사용 유무 및 입력 레벨을 반환한다.
        // 21. TMC304A_GetSvCClr
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetSvCClr", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetSvCClr([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort IsEnable, [MarshalAs(UnmanagedType.U2)] ref ushort Logic);

        // 지정 축의 Counter Clear 출력 시간을 설정한다.
        // 22. TMC304A_SetSvCClrTime
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetSvCClrTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetSvCClrTime([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wTime);

        // 지정 축의 Counter Clear 출력 시간을 반환한다.
        // 23. TMC304A_GetSvCClrTime
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetSvCClrTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetSvCClrTime([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 24. TMC304A_PutSvCClrDO
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_PutSvCClrDO", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_PutSvCClrDO([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // 25. TMC304A_GetSvCClrDO
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetSvCClrDO", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetSvCClrDO([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정 축의 Counter Clear 신호를 On/Off 출력한다.
        // 26. TMC304A_PutSvCClrCmd
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_PutSvCClrCmd", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_PutSvCClrCmd([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정 축의 Servo-Alarm Reset 신호를 출력한다.
        // w_Status: CMD_OFF(0), CMD_ON(1)
        // 27. TMC304A_PutSvAlmRst
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_PutSvAlmRst", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_PutSvAlmRst([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // 지정 축의 Servo-Alarm Reset 신호를 반환한다. 
        // 28. TMC304A_GetSvAlmRst
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetSvAlmRst", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetSvAlmRst([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 비상 정지 신호의 레벨를 설정한다.
        // w_Logic: CMD_LOGIC_A(0), CMD_LOGIC_B(1)
        // 29. TMC304A_SetEmergency
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetEmergency", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetEmergency([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wLogic);

        // 비상 정지 신호의 레벨를 반환한다. 
        // 30. TMC304A_GetEmergency
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetEmergency", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetEmergency([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);


        //======================  모션 제어 환경 설정    ====================================================//

        // 지정 축의 최대 속도를 설정한다. 
        // w_Mode:  1 = 1[PPS]   ~ 8000[PPS]
        //			10 = 10[PPS]  ~ 80000[PPS]
        //			100 = 100[PPS] ~ 800000[PPS]
        //			200 = 200[PPS] ~ 1600000[PPS]
        //			300 = 300[PPS] ~ 2400000[PPS]
        //			400 = 400[PPS] ~ 3200000[PPS]
        //			500 = 500[PPS] ~ 4000000[PPS]
        // 31. TMC304A_SetRangeMode
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetRangeMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetRangeMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wMode);

        // 지정 축의 최대 속도를 반환한다. 
        // 32. TMC304A_GetRangeMode
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetRangeMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetRangeMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정 축의 Command 펄스 출력 모드를 설정한다.
        // w_OutMode : 0	CW/CCW Positive (2 Pulse 정논리)
        //      	   1	CW/CCW Negative (2 Pulse 부논리)
        //			   2	Pulse Positive/Direction Low (1 Pulse 정논리)
        //			   3	Pulse Positive/Direction High (1 Pulse 정논리)
        //			   4	Pulse Negative/Direction Low (1 Pulse 부논리)
        //			   5	Pulse Negative/Direction High (1 Pulse 부논리)
        // 33. TMC304A_SetPulseMode
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetPulseMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetPulseMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wOutMode);

        // 지정 축의 Command 펄스 출력 모드를 반환한다.
        // 34. TMC304A_GetPulseMode
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetPulseMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetPulseMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정 축의 Feedback 펄스의 입력 모드를 설정한다.
        // w_InMode : 0	X4 (A/B상 Pulse 입력을 4체배로 Counter함)
        //            1	X2 (A/B상 Pulse 입력을 2체배로 Counter함)
        //            2	X1 (A/B상 Pulse 입력을 1체배로 Counter함)
        //            3	Up/Down (Up/Down Pulse 입력으로 Counter함)
        // 35. TMC304A_SetEncoderMode
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetEncoderMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetEncoderMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wInMode);

        // 지정 축의 Feedback 펄스의 입력 모드를 반환한다.
        // 36. TMC304A_GetEncoderMode
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetEncoderMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetEncoderMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정 축의 Feedback 펄스 카운트 값의 UP/DOWN 방향을 설정한다.
        // w_InDir : 0 Feedback 카운트의 UP/DOWN 방향을 바꾸지 않습니다.
        //           1 Feedback 카운트의 UP/DOWN 방향을 반대로 합니다.
        // 37. TMC304A_SetEncoderDir
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetEncoderDir", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetEncoderDir([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wInDir);

        // 지정 축의 Feedback 펄스 카운트 값의 UP/DOWN 방향을 반환한다.
        // 38. TMC304A_GetEncoderDir
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetEncoderDir", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetEncoderDir([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정 축의 위치 비교 출력 모드를 설정한다.
        // w_Mode : CMD_COMM(0), CMD_FEED(1)
        // 39. TMC304A_SetCompCountMode
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetCompCountMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetCompCountMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wCmpMode);

        // 지정 축의 위치 비교 출력 모드를 반환한다.
        // 40. TMC304A_GetCompCountMode
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetCompCountMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetCompCountMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정 축의 소프트웨어 리미트 사용 유무 및 위치를 설정한다.
        // l_SlmtP :  -134217728 ~ 134217727
        // l_SlmtM :  -134217728 ~ 134217727
        // 41. TMC304A_SetSlmt
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetSlmt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetSlmt([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lSlmtP, [MarshalAs(UnmanagedType.I4)] int lSlmtM, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // 지정 축의 소프트웨어 리미트 사용 유무 및 위치를 반환한다.
        // 42. TMC304A_GetSlmt
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetSlmt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetSlmt([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] ref int SlmtP, [MarshalAs(UnmanagedType.I4)] ref int SlmtM, [MarshalAs(UnmanagedType.U2)] ref ushort IsEnable);

        // 지정 축의 위치 링 카운터 사용 유무 및 위치 초기화 를 설정한다.
        // dw_CommandPos :  0 ~ 134217727
        // dw_ActualPos  :  0 ~ 134217727
        // 43. TMC304A_SetCounterRing
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetCounterRing", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetCounterRing([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] uint dwCommandPos, [MarshalAs(UnmanagedType.U4)] uint dwFeedbackPos, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // 지정 축의 위치 링 카운터 사용 유무 및 위치 초기화 를 반환한다.
        // 44. TMC304A_GetCounterRing
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetCounterRing", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetCounterRing([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] ref uint CommandPos, [MarshalAs(UnmanagedType.U4)] ref uint FeedbackPos, [MarshalAs(UnmanagedType.U2)] ref ushort IsEnable);

        // 지정 축의 시스템 입력신호에 대한 필터 시간를 설정한다.
        // w_Time : 0x00 	0    [msec]
        //			0x01	0.25 [msec]
        //          0x02 	0.5  [msec]
        //          0x03	1    [msec]
        //          0x04	2    [msec]
        //          0x05	4    [msec]
        //          0x06	8    [msec]
        //          0x07	16   [msec]
        // 45. TMC304A_SetFilterTime
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetFilterTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetFilterTime([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wTime);

        // 지정 축의 시스템 입력 신호에 대한 필터 시간를 반환한다.
        // 46. TMC304A_GetFilterTime
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetFilterTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetFilterTime([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정 축의 Emergency, Hardware Limit Sensor 및 Origin(Home) Sensor 신호에 필터 레벨을 설정한다.
        // 47. TMC304A_SetFilterSensor
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetFilterSensor", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetFilterSensor([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // 지정 축의 Emergency, Hardware Limit Sensor 및 Origin(Home) Sensor 신호에 필터 레벨을 반환한다.
        // 48. TMC304A_GetFilterSensor
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetFilterSensor", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetFilterSensor([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정 축의  Z 상 입력 신호에 필터 사용 유무를 설정한다. 
        // 49. TMC304A_SetFilterEncoderZ
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetFilterEncoderZ", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetFilterEncoderZ([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // 지정 축의  Z 상 입력 신호에 필터 사용 유무를 반환한다.
        // 50. TMC304A_GetFilterEncoderZ
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetFilterEncoderZ", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetFilterEncoderZ([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정 축의  Servo Inposition 및 Servo Alarm  입력 신호에 필터 사용 유무를 설정한다.
        // 51. TMC304A_SetFilterSvIF
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetFilterSvIF", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetFilterSvIF([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // 지정 축의  Servo Inposition 및 Servo Alarm  입력 신호에 필터 사용 유무를 반환한다.
        // 52. TMC304A_GetFilterSvIF
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetFilterSvIF", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetFilterSvIF([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        //======================  단축 모션 제어         ====================================================//

        // 지정 축의 모션의 가감속도 프로파일을 설정한다.
        // w_SpeedMode : CMD_TMODE = 0        TRAPEZOIDAL(사다리꼴 사감속)
        //               CMD_SMODE = 1        S-CURVE (S-CURVE 가감속)
        // 53. TMC304A_SetSpeedMode
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetSpeedMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetSpeedMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wSpeedMode);

        // 지정 축의 모션의 가감속도 프로파일을 반환한다.
        // 54. TMC304A_GetSpeedMode
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetSpeedMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetSpeedMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정 축의 Jog 초기속도,작업속도,가속도를 설정한다.
        // dwInitVel : 1 ~ 4000000	최소 속도 1, 최대 속도 4000000 [PPS]
        // dwWorkVel : 1 ~ 4000000	최소 속도 1, 최대 속도 4000000 [PPS]
        // dwTacc    : 1 ~ 640000(5800)   가속시간, 단위는 [msec]
        // 55. TMC304A_SetJogSpeed
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetJogSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetJogSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] uint dwStartSpeed, [MarshalAs(UnmanagedType.U4)] uint dwWorkSpeed, [MarshalAs(UnmanagedType.U4)] uint dwAccTime);

        // 지정 축의 초기속도,작업속도,가속도를 반환한다.
        // 56. TMC304A_GetJogSpeed
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetJogSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetJogSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] ref uint StartSpeed, [MarshalAs(UnmanagedType.U4)] ref uint WorkSpeed, [MarshalAs(UnmanagedType.U4)] ref uint AccTime);

        // 지정 축의 작업속도까지 가속한 후에 작업속도를 유지하며 상위로부터의 정지명령 또는 외부로 부터 정지신호가 Active 될때까지 지정한 방향으로의 모션을 계속 수행한다.
        // w_Direction : CMD_DIR_N(0) (-) 방향, CMD_DIR_P(1) (+) 방향
        // 57. TMC304A_Jog_Move
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Jog_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Jog_Move([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wDir);

        // 지정 축의 Point to Point 초기속도,작업속도,가속도를 설정한다.  
        // dwInitVel : 1 ~ 4000000	최소 속도 1, 최대 속도 4000000 [PPS]
        // dwWorkVel : 1 ~ 4000000	최소 속도 1, 최대 속도 4000000 [PPS]
        // dwTacc    : 1 ~ 640000(5800)   가속시간, 단위는 [msec]
        // dwTdec    : 1 ~ 640000(5800)   감속속시간, 단위는 [msec]
        // 58. TMC304A_SetPosSpeed
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetPosSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetPosSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] uint dwStartSpeed, [MarshalAs(UnmanagedType.U4)] uint dwWorkSpeed, [MarshalAs(UnmanagedType.U4)] uint dwAccTime, [MarshalAs(UnmanagedType.U4)] uint dwDecTime);

        // 지정 축의 Point to Point 초기속도,작업속도,가속도를 반환한다.
        // 59. TMC304A_GetPosSpeed
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetPosSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetPosSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] ref uint dwpStartSpeed, [MarshalAs(UnmanagedType.U4)] ref uint dwpWorkSpeed, [MarshalAs(UnmanagedType.U4)] ref uint dwpAccTime, [MarshalAs(UnmanagedType.U4)] ref uint dwpDecTime);

        // 지정 축의 현재의 위치에서 지정한 거리(상대 거리)만큼 이동을 수행합니다. 모션을 시작시킨 후에 바로 반환합니다.
        // l_Position  : - 268,435,455 ~ 268,435,455	[pulses]
        // 60. TMC304A_Inc_Move
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Inc_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Inc_Move([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lDistance);

        // 지정 축의 현재의 위치에서 지정한 절대좌표 이동을 수행합니다. 모션을 시작시킨 후에 바로 반환합니다.
        // l_Distance   : - 268,435,455 ~ 268,435,455	[pulses]
        // 61. TMC304A_Abs_Move
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Abs_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Abs_Move([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lPosition);

        // 지정 축의 모션이 완료됐는지를 체크합니다  
        // 0 모션 작업이 완료됨
        // 1 모션 작업이 완료되지 않음
        // 62. TMC304A_Done
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Done", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_Done([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정 축의 감속 후 정지를 수행합니다.
        // 63. TMC304A_Decel_Stop
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Decel_Stop", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Decel_Stop([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정 축의 감속없이 즉시 정지를 수행한다.
        // 64. TMC304A_Sudden_Stop
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Sudden_Stop", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Sudden_Stop([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        //======================  다축 동시 모션 제어         ====================================================//

        // 여러 축에 대하여 모션 작업을 동시에 시작합니다. 
        // nAxisNum : 동시에 작업을 수행할 대상 축 개수
        // waAxisList : 동시에 작업을 수행할 대상 축의 배열 주소값
        // waDirList  : 방향을 지시하는 값의 배열 주소값
        // 65. TMC304A_Multi_Jog_Move
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Multi_Jog_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Multi_Jog_Move([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNoNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] nAxisNoList, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] wDirList);

        // 여러 축에 대하여 현재의 위치에서 지정한 거리만큼 이동을 동시에 시작합니다
        // nAxisNum : 동시에 작업을 수행할 대상 축 개수
        // naAxisList : 동시에 작업을 수행할 대상 축의 배열 주소값
        // laDisList : 이동할 거리값의 배열 주소값
        // 66. TMC304A_Multi_Abs_Move
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Multi_Abs_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Multi_Abs_Move([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNoNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] nAxisNoList, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] lDisList);

        // 여러 축에 대하여 지정한 절대좌표로의 이동을 시작합니다
        // nAxisNum : 동시에 작업을 수행할 대상 축 개수
        // naAxisList : 동시에 작업을 수행할 대상 축의 배열 주소값
        // laPosList : 이동할 거리값의 배열 주소값
        // 67. TMC304A_Multi_Inc_Move
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Multi_Inc_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Multi_Inc_Move([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNoNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] nAxisNoList, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] lPosList);

        // 여러 축에 대하여 지정한 모든 축의 모션이 완료됐는지를 체크합니다.
        // 68. TMC304A_Multi_Done
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Multi_Done", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_Multi_Done([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNoNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] nAxisNoList);

        // 여러 축에 대하여 감속 후 정지를 수행합니다.
        // 69. TMC304A_Multi_Decel_Stop
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Multi_Decel_Stop", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Multi_Decel_Stop([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNoNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] nAxisNoList);

        // 여러 축에 대하여 감속없이 즉시 정지를 수행한다.
        // 70. TMC304A_Multi_Sudden_Stop
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Multi_Sudden_Stop", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Multi_Sudden_Stop([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNoNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] nAxisNoList);


        //======================  원점 복귀                 ====================================================//

        // 지정 축의 원점 복귀 방향을 설정한다.
        // w_HomDir : CMD_CW (0), CMD_CCW (1)
        // 71. TMC304A_SetHomeDir
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetHomeDir", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetHomeDir([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wHomDir);

        // 지정 축의 원점 복귀 방향을 반환한다. 
        // 72. TMC304A_GetHomeDir
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetHomeDir", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetHomeDir([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정 축의 원점 복귀 모드를 설정한다.  
        // w_HomMode : 0	저속 ORIGIN 원점 (ORG)
        //			   1	저속 ORIGIN 원점 (ORG + EZ)
        // 			   2	고속 ORIGIN 원점 (ORG)
        // 			   3	고속 ORIGIN 원점 (ORG + EZ)
        // 			   4	저속 LIMIT 원점 (LMT)
        // 			   5	저속 LIMIT 원점 (LMT + EZ)
        // 			   6	고속 LIMIT 원점 (LMT)
        // 			   7	고속 LIMIT 원점 (LMT + EZ)
        // 			   8	저속 EZ 원점 (EZ)
        // 			   9	현재 위치 ORG 원점
        // 73. TMC304A_SetHomeMode
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetHomeMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetHomeMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wHomMode);

        // 지정 축의 원점 복귀 모드를 반환한다. 	
        // 74. TMC304A_GetHomeMode
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetHomeMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetHomeMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정 축의 원점 복귀 속도 및 가감속 시간을 설정한다.
        // dwInitVel : 1 ~ 4000000	최소 속도 1, 최대 속도 4000000 [PPS]
        // dwWorkVel : 1 ~ 4000000	최소 속도 1, 최대 속도 4000000 [PPS]
        // dwTacc    : 1 ~ 640000(5800)   가속시간, 단위는 [msec]
        // 75. TMC304A_SetHomeSpeed
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetHomeSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetHomeSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] uint dwStartSpeed, [MarshalAs(UnmanagedType.U4)] uint dwWorkSpeed, [MarshalAs(UnmanagedType.U4)] uint dwAccTime);

        // 지정 축의 원점 복귀 속도 및 가감속 시간을 반환한다.
        // 76. TMC304A_GetHomeSpeed
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetHomeSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetHomeSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] ref uint dwpStartSpeed, [MarshalAs(UnmanagedType.U4)] ref uint dwpWorkSpeed, [MarshalAs(UnmanagedType.U4)] ref uint dwpAccTime);

        // 지정 축의 원점 복귀시 기계적인 원점이 아닌 사용자가 임의적으로 작업 원점을 설정한다.
        // 77. TMC304A_SetHomeOffset
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetHomeOffset", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetHomeOffset([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lHomOffset);

        // 지정 축의 원점 복귀시 기계적인 원점이 아닌 사용자가 임의적으로 작업 원점을 반환한다.
        // 78. TMC304A_GetHomeDir
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetHomeOffset", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC304A_GetHomeOffset([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정 축의 원점 복귀 작업을 수행한다.
        // 79. TMC304A_Home_Move
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Home_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Home_Move([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 여러 축에 대한 원점 복귀 작업을 동시에 수행한다.
        // 80. TMC304A_Multi_Home_Move
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Multi_Home_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Multi_Home_Move([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNoNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] nAxisNoList);


        //======================  속도 및 위치 오버라이딩    ====================================================//

        // 지정 축의 모션이 진행되고 있는 중에 속도를 오버라이딩를 설정한다. 
        // 81. TMC304A_OverrideSpeed
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_OverrideSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_OverrideSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] uint dwNewWorkSpeed);

        // 지정 축의 모션이 진행되고 있는 중에 상대 위치를 오버라이딩를 설정한다.
        // 82. TMC304A_Inc_OverrideMove
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Inc_OverrideMove", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Inc_OverrideMove([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lNewDistance);

        // 지정 축의 모션이 진행되고 있는 중에 절대 위치를 오버라이딩를 설정한다.
        // 83. TMC304A_Abs_OverrideMove
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Abs_OverrideMove", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Abs_OverrideMove([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lNewPosition);

        // 여러 축에 대한 모션이 진행되고 있는 중에 속도를 오버라이딩를 설정한다.
        // 84. TMC304A_Multi_OverrideSpeed
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Multi_OverrideSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Multi_OverrideSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNoNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] nAxisNoList, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U4)] uint[] dwaNewWorkSpeed);

        // 여러 축에 대한 모션이 진행되고 있는 중에 상대 위치를 오버라이딩를 설정한다.
        // 85. TMC304A_Multi_Inc_OverrideMove
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Multi_Inc_OverrideMove", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Multi_Inc_OverrideMove([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNoNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] nAxisNoList, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] laNewDistance);

        // 여러 축에 대한 모션이 진행되고 있는 중에 절대 위치를 오버라이딩를 설정한다.
        // 86. TMC304A_Multi_Abs_OverrideMove
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Multi_Abs_OverrideMove", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Multi_Abs_OverrideMove([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNoNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] nAxisNoList, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] laNewPosition);


        //======================  모션 시스템 상태 모니터링 및 위치 및 속도 관리   ====================================================//

        // 지정 축의 모션 과 관련된 시스템의 입축력 상태를 반환한다.
        // Bit0		ORG	Origin(Home) Sensor (원점 센서) [Software Check]
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
        // 87. TMC304A_GetCardStatus
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetCardStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetCardStatus([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

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
        // 88. TMC304A_GetMainStatus
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetMainStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetMainStatus([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

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
        // 89. TMC304A_GetDrvStatus
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetDrvStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetDrvStatus([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

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
        // 90. TMC304A_GetErrStatus
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetErrStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetErrStatus([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

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
        // 91. TMC304A_GetInputStatus
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetInputStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetInputStatus([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정 축의 현재 모션의 이벤트 상태를 반환한다.
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
        // 92. TMC304A_GetEvtStatus
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetEvtStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetEvtStatus([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정 축의 지령위치(Command)값을 새로이 설정한다.
        // 93. TMC304A_SetCommandPos
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetCommandPos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetCommandPos([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lCommandPos);

        // 지정 축의 지령위치(Command)값을 새로이 반환한다.
        // 94. TMC304A_GetCommandPos
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetCommandPos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC304A_GetCommandPos([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정 축의 피드백위치(Feedback)값을 새로이 설정한다.
        // 95. TMC304A_SetActualPos
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetActualPos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetActualPos([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lFeedbackPos);

        // 지정 축의 피드백위치(Feedback)값을 새로이 반환한다.
        // 96. TMC304A_GetActualPos
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetActualPos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC304A_GetActualPos([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 97. TMC304A_GetCommandSpeed
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetCommandSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC304A_GetCommandSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        //======================  범용 디지털 입출력 ====================================================//

        // 32 점씩 Digital Output 채널에 출력한다.  ( Classic )
        // 98. TMC304A_PutDO	
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_PutDO", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_PutDO([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wOutStatus);

        // 32 점씩 Digital Output 채널에 반환한다.  ( Classic )
        // 99. TMC304A_GetDO
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetDO", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetDO([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);

        // 지정한 해당 채널에 신호를 출력한다.
        // nChannelNo : 각 비트값
        // 0 ~ 31:
        // 100. TMC304A_PutDOBit
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_PutDOBit", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_PutDOBit([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nChannelNo, [MarshalAs(UnmanagedType.U2)] ushort wOutStatus);

        // 지정한 해당 채널에 출력 신호를 반환한다.
        // 101. TMC304A_GetDOBit
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetDOBit", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetDOBit([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nChannelNo);

        // 8 점씩 Digital Output 채널에 출력한다.
        // wGroupNo : 입출력 설정 값 [0~7]
        // 0 : 0  ~ 7
        // 1 : 8  ~ 15
        // 2 : 16 ~ 23
        // 3 : 24 ~ 31
        // 102. TMC304A_PutDOByte
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_PutDOByte", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_PutDOByte([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U1)] byte bOutStatus);

        // 8 점씩 Digital Output 채널에 반환한다.
        // 103. TMC304A_GetDOByte
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetDOByte", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetDOByte([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U1)] ref byte bpOutStatus);

        // 16 점씩 Digital Output 채널에 출력한다.
        // wGroupNo : 입출력 설정 값 [0~3]
        // 0 : 0  ~ 15
        // 1 : 16 ~ 31
        // 104. TMC304A_PutDOWord
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_PutDOWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_PutDOWord([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U2)] ushort wOutStatus);

        // 16 점씩 Digital Output 채널에 반환한다.
        // 105. TMC304A_GetDOWord
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetDOWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetDOWord([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U2)] ref ushort OutStatus);

        // 32 점씩 Digital Output 채널에 출력한다.
        // wGroupNo : 입출력 설정 값 [0~1]
        // 0 : 0  ~ 31
        // 106. TMC304A_PutDODWord
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_PutDODWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_PutDODWord([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U4)] uint dwOutStatus);

        // 32 점씩 Digital Output 채널에 반환한다.
        // 107. TMC304A_GetDODWord
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetDODWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetDODWord([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U4)] ref uint OutStatus);

        // 32 점씩 Digital Input 채널에 반환한다. ( Classic )
        // 108. TMC304A_GetDI
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetDI", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetDI([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);

        // 지정한 해당 채널에 입력 신호를 반환한다.
        // 109. TMC304A_GetDIBit
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetDIBit", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetDIBit([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nChannelNo);

        // 8 점씩 Digital Input 채널에 반환한다.
        // wGroupNo : 입출력 설정 값 [0~3]
        // 0 : 0  ~ 7
        // 1 : 8  ~ 15
        // 2 : 16 ~ 23
        // 3 : 24 ~ 31
        // 110. TMC304A_GetDIByte
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetDIByte", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetDIByte([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U1)] ref byte InStatus);

        // 16 점씩 Digital Input 채널에 반환한다.
        // wGroupNo : 입출력 설정 값 [0~1]
        // 0 : 0  ~ 15
        // 1 : 16 ~ 31
        // 111. TMC304A_GetDIWord
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetDIWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetDIWord([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U2)] ref ushort InStatus);

        // 32 점씩 Digital Input 채널에 반환한다.
        // wGroupNo : 입출력 설정 값 [0]
        // 0 : 0  ~ 31
        // 112. TMC304A_GetDIDWord
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetDIDWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetDIDWord([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U4)] ref uint dwpInStatus);

        // Digital 입력 신호 필터 사용 유무를 설정한다.
        // 113. TMC304A_SetDiFilter
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetDiFilter", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetDiFilter([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // Digital 입력 신호 필터 사용 유무를 반환한다.
        // 114. TMC304A_GetDiFilter
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetDiFilter", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetDiFilter([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);

        // Digital 입력 신호 필터 시간를 설정한다.
        // 0	1.00(μsec)	0.875(μsec)	8	0.256(msec)	0.224(msec)
        // 1	2.00(μsec)	1.75(μsec)		9	0.512(msec)	0.448(msec)
        // 2	4.00(μsec)	3.50(μsec)		A	1.02(msec)	0.896(msec)
        // 3	8.00(μsec)	7.00(μsec)		B	2.05(msec)	1.79(msec)
        // 4	16.0(μsec)	14.0(μsec)		C	4.10(msec)	3.58(msec)
        // 5	32.0(μsec)	28.0(μsec)		D	8.19(msec)	7.17(msec)
        // 6	64.0(μsec)	56.0(μsec)		E	16.4(msec)	14.3(msec)
        // 7	128(μsec)	112(μsec)		F	32.8(msec)	28.7(msec)
        // 115. TMC304A_SetDiFilterTime
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetDiFilterTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetDiFilterTime([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wTime);

        // Digital 입력 신호 필터 시간를 반환한다.
        // 116. TMC304A_GetDiFilterTime
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetDiFilterTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetDiFilterTime([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);


        //======================  외부 신호에 의한 모션 제어    ====================================================//

        // 지정 축의 MPG(수동펄스) 사용 유무 설정한다.
        // w_Mode : CMD_DISABLE(0) , CMD_ENABLE(1) 
        // w_Rate : 1 ~ 10000
        // 117. TMC304A_SetExtMode
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetExtMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetExtMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wMode, [MarshalAs(UnmanagedType.U2)] ushort wRate);

        // 지정 축의 MPG(수동펄스) 사용 유무 반환한다.
        // 118. TMC304A_GetExtMode
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetExtMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetExtMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort wMode, [MarshalAs(UnmanagedType.U2)] ref ushort wRate);

        // 지정 축의 MPG(수동펄스) 필터 사용 유무 설정한다.
        // 119. TMC304A_SetFilterExt
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetFilterExt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetFilterExt([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // 지정 축의 MPG(수동펄스) 필터 사용 유무 반환한다.
        // 120. TMC304A_GetFilterExt
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetFilterExt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetFilterExt([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        //====================== 인터럽트 =============================================================//

        // 컨트롤러의 인터럽트 사용 유무를 설정한다.
        // 121. TMC304A_SetEventEnable
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetEventEnable", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetEventEnable([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wIsEvent);

        // hWnd : 윈도우 핸들, 윈도우 메세지를 받을때 사용. 사용하지 않으면 NULL을 입력.
        // wMsg : 윈도우 핸들의 메세지, 사용하지 않거나 디폴트값을 사용하려면 0을 입력.
        // 122. TMC304A_SetEventHandler
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetEventHandler", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetEventHandler([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wIsEvent, [MarshalAs(UnmanagedType.U4)] ref uint Handler, [MarshalAs(UnmanagedType.U4)] uint UiMessage);

        // 지정 축의 사용자가 설정한 인터럽트 발생 여부를 설정한다.
        // EVT_NONE  = &H0  disable all event
        // EVT_C_END = &H1  C-END,    interrupt active when end of constant drive
        // EVT_C_STA = &H2  C-STA,    interrupt active when start of constant drive
        // EVT_D_END = &H4  D-END,    interrupt active when end of drive
        // 123. TMC304A_SetMotEventMask
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetMotEventMask", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetMotEventMask([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort nEventMask);

        // 지정 축의 사용자가 설정한 인터럽트 발생 여부를 읽는다.
        // 124. TMC304A_GetMotEventStatus
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetMotEventStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetMotEventStatus([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort nEventMask);

        // 지정 채널의 사용자가 설정한 인터럽트 발생 여부를 설정한다.
        // nChNoMask1 : CH0  ~ CH31
        // nChNoMask2 : CH32 ~ CH63
        // 125. TMC304A_SetDiEventMask
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetDiEventMask", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetDiEventMask([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U4)] uint ChannelMask1, [MarshalAs(UnmanagedType.U4)] uint ChannelMask2);

        // 지정 채널의 사용자가 설정한 인터럽트 발생 여부를 읽는다.
        // nChNoMask1 : CH0  ~ CH31
        // nChNoMask2 : CH32 ~ CH63
        // 126. TMC304A_GetDiEventStatus
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetDiEventStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetDiEventStatus([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U4)] ref uint ChannelMask1, [MarshalAs(UnmanagedType.U4)] ref uint ChannelMask2);


        //====================== Advanced FUNCTIONS ===================================================//

        // 지정 축의 원점 작업 상태 반환한다.
        // 127. TMC304A_HomeIsBusy
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_HomeIsBusy", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_HomeIsBusy([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정 축의 원점 작업 성공 여부 설정한다.
        // 128. TMC304A_SetHomeSuccess
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetHomeSuccess", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetHomeSuccess([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // 지정 축의 원점 작업 성공 여부 반환한다.
        // 129. TMC304A_GetHomeSuccess
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetHomeSuccess", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetHomeSuccess([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정 축의 최고 속도 배율를 설정한다.
        // 130. TMC304A_SetFixedRange
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetFixedRange", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetFixedRange([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // 지정 축의 최고 속도 배율를 반환한다.
        // 131. TMC304A_GetFixedRange
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetFixedRange", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetFixedRange([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정된 컨트롤러 ID 번호를 반환한다.
        // 132. TMC302A_GetBoardID
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetBoardID", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetBoardID([MarshalAs(UnmanagedType.U2)] ushort nConNo);

        // 지정된 컨트롤러에서 지원하는 제어축 수를 반환한다.
        // 133. TMC304A_GetAxisNum
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetAxisNum", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetAxisNum([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);

        // 지정된 컨트롤러에서 지원하는 범용 디지털입력 채널 수를 반환한다.
        // 134. TMC304A_GetDiNum
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetDiNum", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetDiNum([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);

        // 지정된 컨트롤러에서 지원하는 범용 디지털출력 채널 수를 반환한다.
        // 135. TMC304A_GetDoNum
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetDoNum", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetDoNum([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);

        // 로그 파일 생성 유무를 설정한다.
        // 136. TMC304A_LogCheck
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_LogCheck", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_LogCheck([MarshalAs(UnmanagedType.U2)] ushort wLogCheck);

        // 지정된 컨트롤러의 LED 을 통해 보드 확인한다.
        // 137. TMC304A_PutSvRun
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_PutSvRun", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_PutSvRun([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // 지정된 컨트롤러의 LED 을 통해 보드 반환한다.	
        // 138. TMC304A_GetSvRun
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetSvRun", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetSvRun([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // Block Mode 동작 모드를 설정한다.
        // 139. TMC304A_SetBlockMode
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetBlockMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetBlockMode([MarshalAs(UnmanagedType.U2)] ushort wBlocking);

        // Block Mode 동작 모드를 반환한다.
        // 140. TMC304A_GetBlockMode
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetBlockMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetBlockMode();

        // 지정 축의 가속 옵셋 카운트 설정한다.
        // 141. TMC304A_SetAccOffset
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetAccOffset", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetAccOffset([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lOffset);

        // 지정 축의 가속 옵셋 카운트 반환한다.
        // 142. TMC304A_GetAccOffset
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetAccOffset", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC304A_GetAccOffset([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 컨트롤러의 정보를 반환한다.
        // 143. TMC304A_BoardInfo
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_BoardInfo", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_BoardInfo([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U4)] ref uint dwpBoard, [MarshalAs(UnmanagedType.U4)] ref uint dwpComm, [MarshalAs(UnmanagedType.U4)] ref uint dwpAxis, [MarshalAs(UnmanagedType.U4)] ref uint dwpDiNum, [MarshalAs(UnmanagedType.U4)] ref uint dwpDoNum);

        // 파라미터 정보를 파일에 저장한다.
        // 144. TMC304A_SaveFile
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SaveFile", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SaveFile([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);

    }
}
