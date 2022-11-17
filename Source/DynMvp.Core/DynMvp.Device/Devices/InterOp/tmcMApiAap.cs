//******************************************************************************
//*
//*	File Version: 1,0,0,0
//*
//*	Copyright (c) Alpha Motion 2011-
//*
//*	This file is strictly confidential and do not distribute it outside.
//*
//*	MODULE NAME :
//*		tmcMApiAap.cs
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
    /// Import tmcMApiAap에 대한 요약 설명입니다.
    /// </summary>
    public class TMCAADLL
    {
        public delegate void EventFunc(IntPtr lParam);


        //======================  Loading/Unloading function ====================================================//
        //하드웨어 장치를 로드하고 초기화한다.
        // 1. TMC302A_LoadDevice
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_LoadDevice", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC302A_LoadDevice();

        //하드웨어 장치를 언로드한다.	
        // 2. TMC302A_UnloadDevice
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_UnloadDevice", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_UnloadDevice();


        //======================  장치 초기화     ====================================================//
        //하드웨어 및 소프트웨어를 초기화한다.
        //3. TMC302A_Reset
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Reset", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_Reset([MarshalAs(UnmanagedType.U2)] ushort nConNo);
        //소프트웨어를 초기화한다.	
        //4. TMC302A_SetSystemDefault
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetSystemDefault", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetSystemDefault([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정 축의 Servo-On 신호를 출력한다.                                            
        // wStatus : CMD_OFF(0), CMD_ON(1)
        //5. TMC302A_PutSvOn
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_PutSvOn", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_PutSvOn([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wStatus);

        // 지정 축의 Servo-On신호의 출력 상태을 반환한다. 	
        //6. TMC302A_GetSvOn
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetSvOn", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetSvOn([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        //======================  에러 처리               ====================================================//
        //가장 최근에 실행된 함수의 에러코드를 반환한다.
        //7. TMC302A_GetErrorCode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetErrorCode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC302A_GetErrorCode();
        //가장 최근에 실행된 함수의 에러코드를 문자열로 변환해줍니다.		
        //8. TMC302A_GetErrorString
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetErrorString", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string TMC302A_GetErrorString([MarshalAs(UnmanagedType.I4)] int nErrorCode);
        //모션 완료 후 모션 정지 원인를 반환한다.		
        //9. TMC302A_GetErrorString
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetMotionErrCod", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC302A_GetMotionErrCod([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //======================  시스템 입출력 신호 관련 설정함수    ====================================================//

        //지정 축의 서보 알람 입력 신호의 사용 유무 및 논리를 설정한다.                                            
        // wLogic   : CMD_LOGIC_A(0), CMD_LOGIC_B(1)			
        //10. TMC302A_SetSvAlm
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetSvAlm", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetSvAlm([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable, [MarshalAs(UnmanagedType.U2)] ushort wLogic);
        //지정 축의 서보 알람 입력 신호의 사용 유무 및 논리를 반환한다. 
        //11. TMC302A_GetSvAlm
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetSvAlm", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetSvAlm([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort IsEnable, [MarshalAs(UnmanagedType.U2)] ref ushort Logic);
        // 지정 축의 Inpositon 신호 사용 여부 및 신호 입력 레벨을 설정한다
        // wLogic   : CMD_LOGIC_A(0), CMD_LOGIC_B(1)				
        //12. TMC302A_SetSvInpos
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetSvInpos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetSvInpos([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable, [MarshalAs(UnmanagedType.U2)] ushort wLogic);

        // 지정 축의 Inpositon 신호 사용 여부 및 신호 입력 레벨을 반환한다 					
        //13. TMC302A_GetSvInpos
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetSvInpos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetSvInpos([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort IsEnable, [MarshalAs(UnmanagedType.U2)] ref ushort Logic);


        //지정 축의 Hardware Limit Sensor의 사용 유무 및 신호의 입력 레벨을 설정한다.
        // Hardware Limit Sensor 신호 입력 시 감속정지 또는 급정지에 대한 설정도 가능하다.
        // wStopMethod : CMD_EMG (0), CMD_DEC (1)
        // wLogic: CMD_LOGIC_A(0), CMD_LOGIC_B(1)
        //14. TMC302A_SetHlmt
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetHlmt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetHlmt([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wStopMethod, [MarshalAs(UnmanagedType.U2)] ushort wLogic);

        // 지정 축의 Hardware Limit Sensor의 사용 유무 및 신호의 입력 레벨을 반환한다.		
        //15. TMC302A_GetHlmt
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetHlmt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetHlmt([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort StopMethod, [MarshalAs(UnmanagedType.U2)] ref ushort Logic);

        // 지정 축의 Home sensor 의 입력 레벨을 설정한다.                                            
        // wLogic: CMD_LOGIC_A(0), CMD_LOGIC_B(1)			
        //16. TMC302A_SetOrg
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetOrg", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetOrg([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wLogic);
        //지정 축의 Home sensor 의 입력 레벨을 반환한다.				
        //17. TMC302A_GetOrg
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetOrg", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetOrg([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정 축의 Z 상 레벨을 설정한다. 
        // wLogic: CMD_LOGIC_A(0), CMD_LOGIC_B(1)  
        //18. TMC302A_SetEncoderZ
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetEncoderZ", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetEncoderZ([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wLogic);

        //지정 축의 Z 상 레벨을 반환한다.	
        //18. TMC302A_GetEncoderZ
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetEncoderZ", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetEncoderZ([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //지정 축의 Counter Clear 입력 신호을 사용 유무 및 입력 레벨을 설정한다.
        //wLogic: CMD_LOGIC_A(0), CMD_LOGIC_B(1)		  
        //클리어 카운트는 항상 사용함 
        //19. TMC302A_SetSvCClr
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetSvCClr", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetSvCClr([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable, [MarshalAs(UnmanagedType.U2)] ushort wLogic);

        // 지정 축의 Counter Clear 입력 신호을 사용 유무 및 입력 레벨을 반환한다.		
        //20. TMC302A_GetSvCClr
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetSvCClr", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetSvCClr([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort IsEnable, [MarshalAs(UnmanagedType.U2)] ref ushort Logic);

        // 지정 축의 Counter Clear 출력 시간을 설정한다.
        //wTime :  0x00 = 0.01[msec]
        //         0x01 = 0.02[msec]
        //         0x02 = 0.1 [msec]
        //         0x03 = 0.2 [msec]
        //         0x04 = 1   [msec]
        //	   0x05 = 2   [msec]
        //	   0x06 = 10  [msec]
        //	   0x07 = 20  [msec]
        //21. TMC302A_SetSvCClrTime
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetSvCClrTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetSvCClrTime([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wTime);

        // 지정 축의 Counter Clear 출력 시간을 반환한다.
        //22. TMC302A_GetSvCClrTime
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetSvCClrTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetSvCClrTime([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //23. TMC302A_PutSvCClrDO
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_PutSvCClrDO", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_PutSvCClrDO([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        //24. TMC302A_GetSvCClrDO
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetSvCClrDO", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetSvCClrDO([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //지정 축의 Counter Clear 신호를 On/Off 출력한다. 
        //25. TMC302A_PutSvCClrCmd
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_PutSvCClrCmd", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_PutSvCClrCmd([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정 축의 Servo-Alarm Reset 신호를 출력한다.
        // wStatus: CMD_OFF(0), CMD_ON(1)
        //26. TMC302A_PutSvAlmRst
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_PutSvAlmRst", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_PutSvAlmRst([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wStatus);

        // 지정 축의 Servo-Alarm Reset 신호를 반환한다.
        //27. TMC302A_GetSvAlmRst
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetSvAlmRst", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetSvAlmRst([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 비상 정지 신호의 레벨를 설정한다.
        // w_Logic: CMD_LOGIC_A(0), CMD_LOGIC_B(1)
        //28. TMC302A_SetEmergency
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetEmergency", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetEmergency([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort wLogic);

        // 비상 정지 신호의 레벨를 반환한다.
        //29. TMC302A_GetEmergency
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetEmergency", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetEmergency([MarshalAs(UnmanagedType.U2)] ushort nConNo);


        //======================  모션 제어 환경 설정    ====================================================//
        // 지정 축의 최대 속도를 설정한다. 
        // wMode:  1 = 1[PPS]   ~ 8000[PPS]
        //        10 = 10[PPS]  ~ 80000[PPS]
        //       100 = 100[PPS] ~ 800000[PPS]
        //       200 = 200[PPS] ~ 1600000[PPS]
        //       300 = 300[PPS] ~ 2400000[PPS]
        //       400 = 400[PPS] ~ 3200000[PPS]
        //       500 = 500[PPS] ~ 4000000[PPS]			
        //30. TMC302A_SetRangeMode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetRangeMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetRangeMode([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wMode);

        // 지정 축의 최대 속도를 반환한다. 
        //31. TMC302A_GetRangeMode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetRangeMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetRangeMode([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정 축의 Command 펄스 출력 모드를 설정한다.
        //wOutMode : 0	CW/CCW Positive (2 Pulse 정논리)
        //      	     1	CW/CCW Negative (2 Pulse 부논리)
        //	     2	Pulse Positive/Direction Low (1 Pulse 정논리)
        //	     3	Pulse Positive/Direction High (1 Pulse 정논리)
        //	     4	Pulse Negative/Direction Low (1 Pulse 부논리)
        //	     5	Pulse Negative/Direction High (1 Pulse 부논리)
        //32. TMC302A_SetPulseMode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetPulseMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetPulseMode([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wOutMode);

        // 지정 축의 Command 펄스 출력 모드를 반환한다. 
        //33. TMC302A_GetPulseMode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetPulseMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetPulseMode([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정 축의 Feedback 펄스의 입력 모드를 설정한다.
        //wInMode : 0	X4 (A/B상 Pulse 입력을 4체배로 Counter함)
        //          1	X2 (A/B상 Pulse 입력을 2체배로 Counter함)
        //          2	X1 (A/B상 Pulse 입력을 1체배로 Counter함)
        //          3	Up/Down (Up/Down Pulse 입력으로 Counter함)
        //34. TMC302A_SetEncoderMode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetEncoderMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetEncoderMode([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wInMode);

        // 지정 축의 Feedback 펄스의 입력 모드를 반환한다.
        //35. TMC302A_GetEncoderMode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetEncoderMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetEncoderMode([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정 축의 Feedback 펄스 카운트 값의 UP/DOWN 방향을 설정한다.
        //wInDir : 0 Feedback 카운트의 UP/DOWN 방향을 바꾸지 않습니다.
        //         1 Feedback 카운트의 UP/DOWN 방향을 반대로 합니다.
        //36. TMC302A_SetEncoderDir
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetEncoderDir", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetEncoderDir([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wInDir);

        // 지정 축의 Feedback 펄스 카운트 값의 UP/DOWN 방향을 반환한다.
        //37. TMC302A_GetEncoderDir
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetEncoderDir", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetEncoderDir([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정 축의 위치 비교 출력 모드를 설정한다.
        // wMode : CMD_COMM(0), CMD_FEED(1)
        //38. TMC302A_SetCompCountMode(
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetCompCountMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetCompCountMode([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wMode);

        // 지정 축의 위치 비교 출력 모드를 반환한다. 
        //39. TMC302A_GetCompCountMode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetCompCountMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetCompCountMode([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정 축의 소프트웨어 리미트 사용 유무 및 위치를 설정한다.
        // lSlmtP :  -134217728 ~ 134217727
        // lSlmtM :  -134217728 ~ 134217727
        //40. TMC302A_SetSlmt
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetSlmt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetSlmt([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lSlmtP, [MarshalAs(UnmanagedType.I4)] int lSlmtM, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // 지정 축의 소프트웨어 리미트 사용 유무 및 위치를 반환한다.
        //41. TMC302A_GetSlmt
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetSlmt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetSlmt([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] ref int SlmtP, [MarshalAs(UnmanagedType.I4)] ref int SlmtM, [MarshalAs(UnmanagedType.U2)] ref ushort IsEnable);

        // 지정 축의 위치 링 카운터 사용 유무 및 위치 초기화 를 설정한다.
        // dwCommandPos :  0 ~ 134217727
        // FeedbackPos  :  0 ~ 134217727
        //42. TMC302A_SetCounterRing
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetCounterRing", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetCounterRing([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] uint dwCommandPos, [MarshalAs(UnmanagedType.U4)] uint dwFeedbackPos, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // 지정 축의 위치 링 카운터 사용 유무 및 위치 초기화 를 반환한다.  
        //43. TMC302A_GetCounterRing
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetCounterRing", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetCounterRing([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] ref uint CommandPos, [MarshalAs(UnmanagedType.U4)] ref uint FeedbackPos, [MarshalAs(UnmanagedType.U2)] ref ushort IsEnable);


        // 지정 축의 시스템 입력신호에 대한 필터 시간를 설정한다.
        //wTime :  0x00 	0    [msec]
        //	  0x01	0.25 [msec]
        //         0x02 	0.5  [msec]
        //         0x03	1    [msec]
        //         0x04	2    [msec]
        //         0x05	4    [msec]
        //         0x06	8    [msec]
        //         0x07	16   [msec]
        //44. TMC302A_SetFilterTime
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetFilterTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetFilterTime([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wTime);

        // 지정 축의 시스템 입력 신호에 대한 필터 시간를 반환한다. 
        //45. TMC302A_GetFilterTime
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetFilterTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetFilterTime([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정 축의 Emergency, Hardware Limit Sensor 및 Origin(Home) Sensor 신호에 필터 레벨을 설정한다.
        //46. TMC302A_SetFilterSensor
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetFilterSensor", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetFilterSensor([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // 지정 축의 Emergency, Hardware Limit Sensor 및 Origin(Home) Sensor 신호에 필터 레벨을 반환한다. 
        //47. TMC302A_GetFilterSensor
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetFilterSensor", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetFilterSensor([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        // 지정 축의  Z 상 입력 신호에 필터 사용 유무를 설정한다.
        //48. TMC302A_SetFilterEncoderZ
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetFilterEncoderZ", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetFilterEncoderZ([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // 지정 축의  Z 상 입력 신호에 필터 사용 유무를 반환한다. 
        //49. TMC302A_GetFilterEncoderZ
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetFilterEncoderZ", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetFilterEncoderZ([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        // 지정 축의  Servo Inposition 및 Servo Alarm  입력 신호에 필터 사용 유무를 설정한다. 
        //50. TMC302A_SetFilterSvIF
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetFilterSvIF", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetFilterSvIF([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);


        // 지정 축의  Servo Inposition 및 Servo Alarm  입력 신호에 필터 사용 유무를 반환한다. 
        //51. TMC302A_GetFilterSvIF
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetFilterSvIF", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetFilterSvIF([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //======================  단축 모션 제어   ====================================================//
        //지정 축의 모션의 가감속도 프로파일을 설정한다.
        //wSpeedMode : CMD_TMODE = 0        TRAPEZOIDAL(사다리꼴 사감속)
        //             CMD_SMODE = 1        S-CURVE (S-CURVE 가감속)
        //52. TMC302A_SetSpeedMode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetSpeedMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetSpeedMode([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wSpeedMode);

        //지정 축의 모션의 가감속도 프로파일을 반환한다. 
        //53. TMC302A_GetSpeedMode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetSpeedMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetSpeedMode([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        //지정 축의 Jog 초기속도,작업속도,가속도를 설정한다.
        //dwInitVel : 1 ~ 4000000	최소 속도 1, 최대 속도 4000000 [PPS]
        //dwWorkVel : 1 ~ 4000000	최소 속도 1, 최대 속도 4000000 [PPS]
        //dwTacc    : 1 ~ 640000(5800)   가속시간, 단위는 [msec]
        //54. TMC302A_SetJogSpeed
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetJogSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetJogSpeed([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] uint dwInitVel, [MarshalAs(UnmanagedType.U4)] uint dwWorkVel, [MarshalAs(UnmanagedType.U4)] uint dwTacc);

        //지정 축의 Jog 초기속도,작업속도,가속도를 반환한다.
        //55. TMC302A_GetJogSpeed
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetJogSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetJogSpeed([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] ref uint StartSpeed, [MarshalAs(UnmanagedType.U4)] ref uint WorkSpeed, [MarshalAs(UnmanagedType.U4)] ref uint AccTime);

        //지정 축의 작업속도까지 가속한 후에 작업속도를 유지하며 상위로부터의 정지명령 또는 외부로 부터 정지신호가 Active 될때까지 지정한 방향으로의 모션을 계속 수행한다.
        //wDir : CMD_DIR_N(0) (-) 방향, CMD_DIR_P(1) (+) 방향
        //56. TMC302A_Jog_Move
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Jog_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_Jog_Move([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wDir);

        //지정 축의 Point to Point 초기속도,작업속도,가속도를 설정한다.  
        //dwInitVel : 1 ~ 4000000	최소 속도 1, 최대 속도 4000000 [PPS]
        //dwWorkVel : 1 ~ 4000000	최소 속도 1, 최대 속도 4000000 [PPS]
        //dwTacc    : 1 ~ 640000(5800)   가속시간, 단위는 [msec]
        //dwTdec    : 1 ~ 640000(5800)   감속속시간, 단위는 [msec]  
        //57. TMC302A_SetPosSpeed
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetPosSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetPosSpeed([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] uint dwInitVel, [MarshalAs(UnmanagedType.U4)] uint dwWorkVel, [MarshalAs(UnmanagedType.U4)] uint dwTacc, [MarshalAs(UnmanagedType.U4)] uint dwTdec);

        //지정 축의 Point to Point 초기속도,작업속도,가속도를 반환한다. 
        //58. TMC302A_GetPosSpeed
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetPosSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetPosSpeed([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] ref uint dwpStartSpeed, [MarshalAs(UnmanagedType.U4)] ref uint dwpWorkSpeed, [MarshalAs(UnmanagedType.U4)] ref uint dwpAccTime, [MarshalAs(UnmanagedType.U4)] ref uint dwpDecTime);

        //지정 축의 현재의 위치에서 지정한 거리(상대 거리)만큼 이동을 수행합니다. 모션을 시작시킨 후에 바로 반환합니다.
        // lDistance : - 268,435,455 ~ 268,435,455	[pulses]	
        //59. TMC302A_Inc_Move
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Inc_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_Inc_Move([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lDistance);

        //지정 축의 현재의 위치에서 지정한 절대좌표 이동을 수행합니다. 모션을 시작시킨 후에 바로 반환합니다.
        // lPosition : - 268,435,455 ~ 268,435,455	[pulses]
        //60. TMC302A_Abs_Move
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Abs_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_Abs_Move([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lPosition);

        //지정 축의 모션이 완료됐는지를 체크합니다  
        // 0 모션 작업이 완료됨
        // 1 모션 작업이 완료되지 않음
        //61. TMC302A_Done
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Done", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_Done([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //지정 축의 감속 후 정지를 수행합니다.
        //62. TMC302A_Decel_Stop
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Decel_Stop", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_Decel_Stop([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //지정 축의 감속없이 즉시 정지를 수행한다.
        //63. TMC302A_Sudden_Stop
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Sudden_Stop", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_Sudden_Stop([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        //======================  다축 동시 모션 제어         ====================================================//

        //여러 축에 대하여 모션 작업을 동시에 시작합니다. 
        //nAxisNum : 동시에 작업을 수행할 대상 축 개수
        //waAxisList : 동시에 작업을 수행할 대상 축의 배열 주소값
        //waDirList  : 방향을 지시하는 값의 배열 주소값
        //64. TMC302A_Multi_Jog_Move
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Multi_Jog_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_Multi_Jog_Move([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] naAxisList, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] waDirList);

        //여러 축에 대하여 현재의 위치에서 지정한 거리만큼 이동을 동시에 시작합니다
        //nAxisNum : 동시에 작업을 수행할 대상 축 개수
        //naAxisList : 동시에 작업을 수행할 대상 축의 배열 주소값
        //laDisList : 이동할 거리값의 배열 주소값
        //65. TMC302A_Multi_Inc_Move
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Multi_Inc_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_Multi_Inc_Move([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] naAxisList, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] laDisList);

        //여러 축에 대하여 지정한 절대좌표로의 이동을 시작합니다
        //nAxisNum : 동시에 작업을 수행할 대상 축 개수
        //naAxisList : 동시에 작업을 수행할 대상 축의 배열 주소값
        //laPosList : 이동할 거리값의 배열 주소값
        //66. TMC302A_Multi_Abs_Move
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Multi_Abs_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_Multi_Abs_Move([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] naAxisList, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] laPosList);


        //여러 축에 대하여 지정한 모든 축의 모션이 완료됐는지를 체크합니다
        //67. TMC302A_Multi_Done
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Multi_Done", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_Multi_Done([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] naAxisList);

        //여러 축에 대하여 감속 후 정지를 수행합니다. 
        //68. TMC302A_Multi_Decel_Stop
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Multi_Decel_Stop", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_Multi_Decel_Stop([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] naAxisList);

        //여러 축에 대하여 감속없이 즉시 정지를 수행한다. 
        //69. TMC302A_Multi_Sudden_Stop
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Multi_Sudden_Stop", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_Multi_Sudden_Stop([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] naAxisList);


        //======================  원점 복귀                 ====================================================//

        //지정 축의 원점 복귀 방향을 설정한다.
        // wHomDir : CMD_CW (0), CMD_CCW (1)
        //70. TMC302A_SetHomeDir
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetHomeDir", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetHomeDir([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wHomDir);

        //지정 축의 원점 복귀 방향을 반환한다.
        //71. TMC302A_GetHomeDir
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetHomeDir", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetHomeDir([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //지정 축의 원점 복귀 모드를 설정한다.  
        //wHomMode :  0	저속 ORIGIN 원점 (ORG)
        //	     1	저속 ORIGIN 원점 (ORG + EZ)
        //	     2	고속 ORIGIN 원점 (ORG)
        //	     3	고속 ORIGIN 원점 (ORG + EZ)
        //	     4	저속 LIMIT 원점 (LMT)
        //	     5	저속 LIMIT 원점 (LMT + EZ)
        //	     6	고속 LIMIT 원점 (LMT)
        //	     7	고속 LIMIT 원점 (LMT + EZ)
        //	     8	저속 EZ 원점 (EZ)
        //	     9	현재 위치 ORG 원점
        //72. TMC302A_SetHomeMode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetHomeMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetHomeMode([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wHomMode);

        //지정 축의 원점 복귀 모드를 반환한다.  
        //73. TMC302A_GetHomeMode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetHomeMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetHomeMode([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //지정 축의 원점 복귀 속도 및 가감속 시간을 설정한다.
        //dwInitVel : 1 ~ 4000000	최소 속도 1, 최대 속도 4000000 [PPS]
        //dwWorkVel : 1 ~ 4000000	최소 속도 1, 최대 속도 4000000 [PPS]
        //dwTacc    : 1 ~ 640000(5800)   가속시간, 단위는 [msec]
        //74. TMC302A_SetHomeSpeed
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetHomeSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetHomeSpeed([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] uint dwInitVel, [MarshalAs(UnmanagedType.U4)] uint dwWorkVel, [MarshalAs(UnmanagedType.U4)] uint dwTacc);

        //지정 축의 원점 복귀 속도 및 가감속 시간을 반환한다.
        //75. TMC302A_GetHomeSpeed
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetHomeSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetHomeSpeed([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] ref uint dwpStartSpeed, [MarshalAs(UnmanagedType.U4)] ref uint dwpWorkSpeed, [MarshalAs(UnmanagedType.U4)] ref uint dwpAccTime);

        //지정 축의 원점 복귀시 기계적인 원점이 아닌 사용자가 임의적으로 작업 원점을 설정한다.
        //76. TMC302A_SetHomeOffset
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetHomeOffset", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetHomeOffset([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lHomOffset);

        //지정 축의 원점 복귀시 기계적인 원점이 아닌 사용자가 임의적으로 작업 원점을 반환한다. 
        //77. TMC302A_GetHomeOffset
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetHomeOffset", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC302A_GetHomeOffset([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //지정 축의 원점 복귀 작업을 수행한다.
        //78. TMC302A_Home_Move
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Home_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_Home_Move([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //여러 축에 대한 원점 복귀 작업을 동시에 수행한다. 
        //79. TMC302A_Multi_Home_Move
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Multi_Home_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_Multi_Home_Move([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] naAxisList);


        //======================  속도 및 위치 오버라이딩    ====================================================//


        //지정 축의 모션이 진행되고 있는 중에 상대 위치를 오버라이딩를 설정한다.
        //80. TMC302A_Inc_OverrideMove
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Inc_OverrideMove", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_Inc_OverrideMove([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lNewDistance);

        //지정 축의 모션이 진행되고 있는 중에 절대 위치를 오버라이딩를 설정한다.
        //81. TMC302A_Abs_OverrideMove
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Abs_OverrideMove", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_Abs_OverrideMove([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lNewPosition);

        //지정 축의 모션이 진행되고 있는 중에 속도를 오버라이딩를 설정한다.
        //82. TMC302A_OverrideSpeed
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_OverrideSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_OverrideSpeed([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] uint dwNewWorkSpeed);

        //여러 축에 대한 모션이 진행되고 있는 중에 속도를 오버라이딩를 설정한다.
        //83. TMC302A_Multi_OverrideSpeed
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Multi_OverrideSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_Multi_OverrideSpeed([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] naAxisList, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U4)] uint[] dwaNewWorkSpeed);

        //여러 축에 대한 모션이 진행되고 있는 중에 상대 위치를 오버라이딩를 설정한다
        //84. TMC302A_Multi_Inc_OverrideMove
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Multi_Inc_OverrideMove", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_Multi_Inc_OverrideMove([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] naAxisList, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] laNewDistance);

        //여러 축에 대한 모션이 진행되고 있는 중에 절대 위치를 오버라이딩를 설정한다.
        //85. TMC302A_Multi_Abs_OverrideMove
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Multi_Abs_OverrideMove", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_Multi_Abs_OverrideMove([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] naAxisList, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] laNewPosition);


        //======================  모션 시스템 상태 모니터링 및 위치 및 속도 관리   ====================================================//
        //지정 축의 모션 과 관련된 시스템의 입축력 상태를 반환한다.
        //Bit0	ORG	Origin(Home) Sensor (원점 센서) [Software Check]
        //Bit1	EZ	Encoder Z (엔코더 Z상) [Software Check]
        //Bit2	EMG	Emergency (비상정지) [Hardware Check]
        //Bit3	INP	Servo Inposition (서보 위치 결정 완료) [Software Check]
        //Bit4	ALM	Servo Alarm (서보 알람) [Software Check]
        //Bit5	LMT+	+Limit Sensor (+리미트 센서) [Software Check]
        //Bit6	LMT-	-Limit Sensor (-리미트 센서) [Software Check]
        //Bit7	NC	NC
        //Bit8	RUN	Motion 동작 수행 중
        //Bit9	ERR	Error 발생
        //Bit10	HOME	원점 복귀 Motion 수행 중
        //Bit11	H_OK	원점 복귀 Motion 완료
        //Bit12	NC	NC
        //Bit13	C.CLR	Servo Error Counter Clear (서보 편차 카운터 클리어)
        //Bit14	SON	Servo On (서보 온)
        //Bit15	A.RST	Servo Alarm Reset (서보 알람 리셋)
        //86. TMC302A_GetCardStatus
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetCardStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetCardStatus([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        //지정 축의 모션 과 에러 상태를 반환한다. 
        //Bit0	RUN	0축 Motion 동작 수행 중
        //Bit1	RUN	1축 Motion 동작 수행 중
        //Bit2	RUN	2축 Motion 동작 수행 중
        //Bit3	RUN	3축 Motion 동작 수행 중
        //Bit4	ERR	0축 ERROR (Error 발생)
        //Bit5	ERR 	1축 ERROR (Error 발생)
        //Bit6	ERR 	2축 ERROR (Error 발생)
        //Bit7	ERR	3축 ERROR (Error 발생)
        //Bit8	HOME	0축 원점 복귀 Motion 수행 중
        //Bit9	HOME	1축 원점 복귀 Motion 수행 중
        //Bit10	HOME	2축 원점 복귀 Motion 수행 중
        //Bit11	HOME	3축 원점 복귀 Motion 수행 중
        //Bit12	NC	NC
        //Bit13	NC	NC
        //Bit14	NC	NC
        //Bit15	NC	NC
        //87. TMC302A_GetMainStatus
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetMainStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetMainStatus([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //지정 축의 모션의 동작 및 정지 상태를 반환한다.
        //Bit0	CMP+	현재 Position 값이 COMP+ 값보다 크거나 같을 때
        //Bit1	CMP-	현재 Position 값이 COMP- 값보다 작을 때
        //Bit2	ASND	직선 가감속에서 가속할 때
        //Bit3	CNST	직선 가감속에서 등속할 때
        //Bit4	DSND	직선 가감속에서 감속할 때
        //Bit5	AASND	S자 가감속에서 가감속도가 증가할 때
        //Bit6	ACNST	S자 가감속에서 가감속도가 일정할 때
        //Bit7	ADSND	S자 가감속에서 가감속도가 감소할 때
        //Bit8	NC	NC
        //Bit9	S-ORG	ORG 신호에 의해 정지할 때
        //Bit10	S-EZ	EZ 신호에 의해 정지할 때
        //Bit11	NC	
        //Bit12	S-LMT+	+LMT 신호에 의해 정지할 때
        //Bit13	S-LMT-	-LMT 신호에 의해 정지할 때
        //Bit14	S-ALM	ALM 신호에 의해 정지할 때
        //88. TMC302A_GetDrvStatus
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetDrvStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetDrvStatus([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //지정 축의 현재 모션의 에러 상태를 반환한다.  
        //Bit0	E-SLMT+	+Soft Limit가 동작했을 때
        //Bit1	E-SLMT-	-Soft Limit가 동작했을 때
        //Bit2	E-HLMT+	LMT+ 신호가 동작했을 때
        //Bit3	E-HLMT-	LMT- 신호가 동작했을 때
        //Bit4	E-ALM	ALM 신호가 동작했을 때
        //Bit5	E-EMG	EMG 신호가 동작했을 때
        //Bit6	NC	NC
        //Bit7	E-HOME	원점 복귀 Motion 수행 중 Error가 발생했을 때
        //Bit8	HMST0	원점 복귀 Motion 수행 중 Step 동작 내용(참고 참조)
        //Bit9	HMST1	원점 복귀 Motion 수행 중 Step 동작 내용(참고 참조)
        //Bit10	HMST2	원점 복귀 Motion 수행 중 Step 동작 내용(참고 참조)
        //Bit11	HMST3	원점 복귀 Motion 수행 중 Step 동작 내용(참고 참조)
        //Bit12	HMST4	원점 복귀 Motion 수행 중 Step 동작 내용(참고 참조)
        //Bit13	NC	NC
        //Bit14	NC	NC
        //Bit15	NC	NC
        //89. TMC302A_GetErrStatus
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetErrStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetErrStatus([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //지정 축의 현재 모션의 시스템 입출력 상태를 반환한다.
        //Bit0	NC	NC
        //Bit1	ORG	Origin(Home) Sensor (원점 센서) 감지 됨[Hardware Check]
        //Bit2	EZ	Encoder Z (엔코더 Z상) 감지 됨[Hardware Check]
        //Bit3	EMG	EMG 신호 감지 됨
        //Bit4	EX+	외부에서 + 방향의 드라이브
        //Bit5	EX-	외부에서 - 방향의 드라이브
        //Bit6	INP	Servo InPosition (서보 위치 결정 완료) 감지 됨[Hardware Check]
        //Bit7	ALM	Servo Alarm (서보 알람) 감지 됨[Hardware Check]
        //Bit8	NC	NC
        //Bit9	NC	NC
        //Bit10	NC	NC
        //Bit11	NC	NC
        //Bit12	NC	NC
        //Bit13	NC	NC
        //Bit14	LMT+	+Limit Sensor (+리미트 센서) 감지 됨[Hardware Check]
        //Bit15	LMT-	Limit Sensor (-리미트 센서) 감지 됨[Hardware Check]
        //90. TMC302A_GetInputStatus
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetInputStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetInputStatus([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //지정 축의 현재 모션의 이벤트 상태를 반환한다.
        //Bit0	NC	NC
        //Bit1	P>=C-	펄스 카운트가 CMP- 값 보다 같거나 큼
        //Bit2	P<C-	펄스 카운트가 CMP- 값 보다 작음
        //Bit3	P<C+	펄스 카운트가 CMP+ 값 보다 작음
        //Bit4	P>=C+	펄스 카운트가 CMP+ 값 보다 같거나 큼
        //Bit5	C-END	정속 Motion 종료
        //Bit6	C-STA	정속 Motion 시작
        //Bit7	D-END	Motion 종료
        //Bit8	NC	NC
        //Bit9	NC	NC
        //Bit10	NC	NC
        //Bit11	NC	NC
        //Bit12	NC	NC
        //Bit13	NC	NC
        //Bit14	NC	NC
        //Bit15	NC	NC
        //91. TMC302A_GetEvtStatus
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetEvtStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetEvtStatus([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //지정 축의 지령위치(Command)값을 새로이 설정한다.
        //92. TMC302A_SetCommandPos
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetCommandPos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetCommandPos([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lCommandPos);

        //지정 축의 지령위치(Command)값을 새로이 반환한다. 
        //93. TMC302A_GetCommandPos
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetCommandPos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC302A_GetCommandPos([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //지정 축의 피드백위치(Feedback)값을 새로이 설정한다. 
        //94. TMC302A_SetActualPos
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetActualPos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetActualPos([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lFeedbackPos);

        //지정 축의 피드백위치(Feedback)값을 새로이 반환한다. 
        //95. TMC302A_GetActualPos
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetActualPos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC302A_GetActualPos([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //지정 축의 지령 속도값을 반환한다.  
        //96. TMC302A_GetCommandSpeed
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetCommandSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC302A_GetCommandSpeed([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //======================  Trigger Motion ====================================================//

        //지정 축의 비교 위치 출력 펄스 폭를 설정한다.
        //97. TMC302A_SetCompTrgWidth
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetCompTrgWidth", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetCompTrgWidth([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wCmpWidth);


        //지정 축의 비교 위치 출력 펄스 폭를 반환한다.
        //98. TMC302A_GetCompTrgWidth
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetCompTrgWidth", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetCompTrgWidth([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //지정 축의 비교 위치 출력 사용유무를 설정한다.
        //99. TMC302A_SetCompTrgMode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetCompTrgMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetCompTrgMode([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort wLogic, [MarshalAs(UnmanagedType.U2)] ushort wEnable);


        //지정 축의 비교 위치 출력 사용유무를 반환한다.
        //100. TMC302A_GetCompTrgMode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetCompTrgMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetCompTrgMode([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ref ushort wLogic, [MarshalAs(UnmanagedType.U2)] ref ushort wEnable);

        //지정 축의 위치비교출력기에 1회 비교데이터를 설정한다.
        //101. TMC302A_SetCompTrgOneData
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetCompTrgOneData", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetCompTrgOneData([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lStartData);


        //지정 축의 연속적인 위치 비교 출력 기능을 사용하기 위해서 연속적인위치 데이터를 등록한다.
        //102. TMC302A_SetCompTrgTable
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetCompTrgTable", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetCompTrgTable([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wNumData, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] lPosition);


        //지정 축의 연속적인 위치 비교 출력 기능을 사용하기 위해서 일정한 위치 간격을 가지는 연속적인 위치 데이터를 자동으로 생성하여 등록한다,
        //103. TMC302A_SetCompTrgContTable
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetCompTrgContTable", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetCompTrgContTable([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wNumData, [MarshalAs(UnmanagedType.I4)] int StartData, [MarshalAs(UnmanagedType.I4)] int lInterval);


        ////연속적인 위치 비교 출력 기능을 시작한다.
        //104. TMC302A_SetInitCompTrg
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetInitCompTrg", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetInitCompTrg([MarshalAs(UnmanagedType.U2)] ushort nConNo);

        ////연속적인 위치 비교 출력 기능을 종료한다.
        //105. TMC302A_SetFreeCompTrg
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetFreeCompTrg", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetFreeCompTrg([MarshalAs(UnmanagedType.U2)] ushort nConNo);

        //======================  범용 디지털 입출력 ====================================================//

        //지정한 해당 채널에 신호를 출력한다. 
        //108. TMC302A_PutDOBit
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_PutDOBit", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_PutDOBit([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nChannelNo, [MarshalAs(UnmanagedType.U2)] ushort wOutStatus);

        //지정한 해당 채널에 출력 신호를 반환한다.
        //109. TMC302A_GetDOBit
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetDOBit", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetDOBit([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nChannelNo);

        //8 점씩 Digital Output 채널에 출력한다.
        //nGroupNo : 0 	CH0  ~ CH7
        //	    1	CH8  ~ CH15
        //	    2 	CH16 ~ CH23
        //           3	CH24 ~ CH31
        //110. TMC302A_PutDOByte
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_PutDOByte", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_PutDOByte([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U1)] byte bOutStatus);

        //8 점씩 Digital Output 채널에 반환한다.
        //111. TMC302A_GetDOByte
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetDOByte", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetDOByte([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U1)] ref byte bpOutStatus);

        //16 점씩 Digital Output 채널에 출력한다.                                            
        //nGroupNo : 0 	CH0  ~ CH15
        //	    1 	CH16 ~ CH31
        //112. TMC302A_PutDOWord
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_PutDOWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_PutDOWord([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U2)] ushort wOutStatus);

        //16 점씩 Digital Output 채널에 반환한다.
        //113. TMC302A_GetDOWord
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetDOWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetDOWord([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U2)] ref ushort OutStatus);

        //32 점씩 Digital Output 채널에 출력한다.                                            
        //nGroupNo : 0 	CH0  ~ CH31
        //113. TMC302A_PutDODWord
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_PutDODWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_PutDODWord([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U4)] uint dwOutStatus);

        //32 점씩 Digital Output 채널에 반환한다.      
        //114. TMC302A_GetDODWord
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetDODWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetDODWord([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U4)] ref uint OutStatus);

        //지정한 해당 채널에 입력 신호를 반환한다. 
        //116. TMC302A_GetDIBit
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetDIBit", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetDIBit([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nChannelNo);

        //8 점씩 Digital Input 채널에 반환한다.   
        //nGroupNo : 0 	CH0  ~ CH7
        //	    1	CH8  ~ CH15
        //	    2 	CH16 ~ CH23
        //           3	CH24 ~ CH31 
        //117. TMC302A_GetDIByte
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetDIByte", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetDIByte([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U1)] ref byte InStatus);

        //16 점씩 Digital Input 채널에 반환한다.   
        //nGroupNo : 0 	CH0  ~ CH15
        //	    1 	CH16 ~ CH31 
        //118. TMC302A_GetDIWord
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetDIWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetDIWord([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U2)] ref ushort InStatus);

        //32 점씩 Digital Input 채널에 반환한다.   
        //nGroupNo : 0 	CH0  ~ CH31 
        //119. TMC302A_GetDIDWord
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetDIDWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetDIDWord([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U4)] ref uint dwpInStatus);

        // Digital 입력 신호 필터 사용 유무를 설정한다.
        //120. TMC302A_SetDiFilter
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetDiFilter", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetDiFilter([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);


        // Digital 입력 신호 필터 사용 유무를 반환한다.
        //121. TMC302A_GetDiFilter
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetDiFilter", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetDiFilter([MarshalAs(UnmanagedType.U2)] ushort nConNo);


        // Digital 입력 신호 필터 시간를 설정한다.
        //0	1.00(μsec)	0.875(μsec)	8	0.256(msec)	0.224(msec)
        //1	2.00(μsec)	1.75(μsec)	9	0.512(msec)	0.448(msec)
        //2	4.00(μsec)	3.50(μsec)	A	1.02(msec)	0.896(msec)
        //3	8.00(μsec)	7.00(μsec)	B	2.05(msec)	1.79(msec)
        //4	16.0(μsec)	14.0(μsec)	C	4.10(msec)	3.58(msec)
        //5	32.0(μsec)	28.0(μsec)	D	8.19(msec)	7.17(msec)
        //6	64.0(μsec)	56.0(μsec)	E	16.4(msec)	14.3(msec)
        //7	128(μsec)	112(μsec)	F	32.8(msec)	28.7(msec)
        //122. TMC302A_SetDiFilterTime
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetDiFilterTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetDiFilterTime([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort wTime);

        // Digital 입력 신호 필터 시간를 반환한다.  
        //123. TMC302A_GetDiFilterTime
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetDiFilterTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetDiFilterTime([MarshalAs(UnmanagedType.U2)] ushort nConNo);


        //======================  외부 신호에 의한 모션 제어    ====================================================//
        // 지정 축의 MPG(수동펄스) 사용 유무 설정한다.
        //w_Mode : CMD_DISABLE(0) , CMD_ENABLE(1) 
        //w_Rate : 1 ~ 10000
        //124. TMC302A_SetExtMode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetExtMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetExtMode([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wMode, [MarshalAs(UnmanagedType.U2)] ushort wRate);


        // 지정 축의 MPG(수동펄스) 사용 유무 반환한다. 
        //125. TMC302A_GetExtMode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetExtMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetExtMode([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort wMode, [MarshalAs(UnmanagedType.U2)] ref ushort wRate);


        //지정 축의 MPG(수동펄스) 필터 사용 유무 설정한다.
        //126. TMC302A_SetFilterExt
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetFilterExt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetFilterExt([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        //지정 축의 MPG(수동펄스) 필터 사용 유무 반환한다. 
        //127. TMC302A_GetFilterExt
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetFilterExt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetFilterExt([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        //====================== 인터럽트 함수 ===================================================//

        //컨트롤러의 인터럽트 사용 유무를 설정한다.
        //128. TMC302A_SetEventEnable
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetEventEnable", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetEventEnable([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort wIsEvent);

        //hWnd : 윈도우 핸들, 윈도우 메세지를 받을때 사용. 사용하지 않으면 NULL을 입력.
        //wMsg : 윈도우 핸들의 메세지, 사용하지 않거나 디폴트값을 사용하려면 0을 입력.
        //129. TMC302A_SetEventHandler
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetEventHandler", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetEventHandler([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort wIsEvent, [MarshalAs(UnmanagedType.U4)] ref uint hWnd, [MarshalAs(UnmanagedType.U4)] uint wMsg);

        // 지정 축의 사용자가 설정한 인터럽트 발생 여부를 설정한다.
        //EVT_NONE  = &H0  disable all event
        //EVT_C_END = &H1  C-END,    interrupt active when end of constant drive
        //EVT_C_STA = &H2  C-STA,    interrupt active when start of constant drive
        //EVT_D_END = &H4  D-END,    interrupt active when end of drive
        //130. TMC302A_SetMotEventMask
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetMotEventMask", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetMotEventMask([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEventMask);

        // 지정 축의 사용자가 설정한 인터럽트 발생 여부를 읽는다.
        //131. TMC302A_GetMotEventStatus
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetMotEventStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetMotEventStatus([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort wEventStatus);


        // 지정 채널의의 사용자가 설정한 인터럽트 발생 여부를 설정한다.
        // nChNoMask1 : CH0  ~ CH31
        // nChNoMask2 : CH32 ~ CH63
        //132. TMC302A_SetDiEventMask
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetDiEventMask", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetDiEventMask([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U4)] uint dwChannelMask1, [MarshalAs(UnmanagedType.U4)] uint dwChannelMask2);


        // 지정 채널의의 사용자가 설정한 인터럽트 발생 여부를 읽는다.
        // nChNoMask1 : CH0  ~ CH31
        // nChNoMask2 : CH32 ~ CH63
        //133. TMC302A_GetDiEventStatus
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetDiEventStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetDiEventStatus([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U4)] ref uint dwChannelStatus1, [MarshalAs(UnmanagedType.U4)] ref uint dwChannelStatus2);


        //====================== 기타 ===================================================//

        // 지정 축의 원점 작업 상태 반환한다.
        //134. TMC302A_HomeIsBusy
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_HomeIsBusy", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_HomeIsBusy([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정 축의 원점 작업 성공 여부 설정한다.
        //135. TMC302A_SetHomeSuccess
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetHomeSuccess", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetHomeSuccess([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // 지정 축의 원점 작업 성공 여부 반환한다.
        //136. TMC302A_GetHomeSuccess
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetHomeSuccess", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetHomeSuccess([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 지정 축의 최고 속도 배율를 설정한다.
        //137. TMC302A_SetFixedRange
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetFixedRange", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetFixedRange([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // 지정 축의 최고 속도 배율를 반환한다.
        //138. TMC302A_GetFixedRange
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetFixedRange", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetFixedRange([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        //지정된 컨트롤러 ID 번호를 반환한다.
        //139. TMC302A_GetBoardID
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetBoardID", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetBoardID([MarshalAs(UnmanagedType.U2)] ushort nConNo);

        //지정된 컨트롤러에서 지원하는 제어축 수를 반환한다.
        //140. TMC302A_GetAxisNum
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetAxisNum", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetAxisNum([MarshalAs(UnmanagedType.U2)] ushort nConNo);

        //지정된 컨트롤러에서 지원하는 범용 디지털입력 채널 수를 반환한다.
        //141. TMC302A_GetDiNum
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetDiNum", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetDiNum([MarshalAs(UnmanagedType.U2)] ushort nConNo);

        //지정된 컨트롤러에서 지원하는 범용 디지털출력 채널 수를 반환한다. 
        //142. TMC302A_GetDoNum
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetDoNum", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetDoNum([MarshalAs(UnmanagedType.U2)] ushort nConNo);

        //로그 파일 생성 유무를 설정한다.
        //143. TMC302A_LogCheck
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_LogCheck", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_LogCheck([MarshalAs(UnmanagedType.U2)] ushort wLogCheck);

        //지정된 컨트롤러의 LED 을 통해 보드 확인한다.
        //144. TMC302A_PutSvRun
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_PutSvRun", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_PutSvRun([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        //지정된 컨트롤러의 LED 을 통해 보드 반환한다.
        //145. TMC302A_GetSvRun
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetSvRun", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetSvRun([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //Block Mode 동작 모드를 설정한다.
        //146. TMC302A_SetBlockMode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetBlockMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetBlockMode([MarshalAs(UnmanagedType.U2)] ushort wBlocking);

        //Block Mode 동작 모드를 반환한다.			
        //147. TMC302A_GetBlockMode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetBlockMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetBlockMode();

        //지정 축의 가속 옵셋 카운트 설정한다.
        //148. TMC302A_SetAccOffset
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetAccOffset", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetAccOffset([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lOffset);

        //지정 축의 가속 옵셋 카운트 설정한다.			
        //149. TMC302A_GetAccOffset
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetAccOffset", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC302A_GetAccOffset([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //컨트롤러의 정보를 반환한다.
        //150. TMC302A_SaveFile
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SaveFile", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SaveFile([MarshalAs(UnmanagedType.U2)] ushort nConNo);


        //컨트롤러의 정보를 반환한다.
        //151. TMC302A_BoardInfo
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_BoardInfo", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_BoardInfo([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U4)] ref uint dwpBoard, [MarshalAs(UnmanagedType.U4)] ref uint dwpComm, [MarshalAs(UnmanagedType.U4)] ref uint dwpAxis, [MarshalAs(UnmanagedType.U4)] ref uint dwpDiNum, [MarshalAs(UnmanagedType.U4)] ref uint dwpDoNum);
        //-------------------------------------------------------------------------------------------------------------------
    }
}
