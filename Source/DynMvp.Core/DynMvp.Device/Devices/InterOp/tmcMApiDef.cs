///******************************************************************************
//*
//*	File Version: 1,0,0,0
//*
//*	Copyright (c) Alpha Motion 2011-
//*
//*	This file is strictly confidential and do not distribute it outside.
//*
//*	MODULE NAME :
//*		tmcMApiDef.cs
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
using System.Text;

namespace Shared
{
    public sealed class tmcDef
    {


        public const int MAX_STR_LEN_AXIS_TITLE = 30; // Maximum string length of axis title
        public const int MAX_STR_LEN_DIST_UNIT = 30; // Maximum string length of distance unit
        public const int MAX_STR_LEN_VEL_UNIT = 30; // Maximum string length of velocity unit
        public const int MAX_STR_LEN_ERR = 128;  // Maximum error string length: a buffer to receive error string must be larger than this size.

        //***********************************************************************************************
        //									ERROR CODE DEFINITIONs										*
        //***********************************************************************************************
        //'사용자 이벤트 메시지
        public const uint WM_USER = 0x400;
        public const uint WM_MC_INTERRUPT = (WM_USER + 2006);
        public const uint WM_DI_INTERRUPT = (WM_USER + 2007);

        /* Error code = Erroc code */
        public const int ERR_SUCCESS = 0;       // 에러 없음

        public const int ERR_DEVICE_LOAD = -1;  // 디바이스가 로드 되어 있지 않습니다.
        public const int ERR_PRM_LOAD = -2; // 파라미터 일치 하지 않습니다.
        public const int ERR_DEVICE_EXIST = -3; // 동일한 DEVICE ID가 존재 합니다.
        public const int ERR_DEVICE_PCI_BUS = -4;   // PCI 버스 데이타가 이상합니다.
        public const int ERR_UPBOARD_LOAD = -5; // 모듈 순서가 잘못되었습니다.=  MCX302에는 없는 에러임  
        public const int ERR_CPLD_VER = -6; // CPLD VERSION 에러 
        public const int ERR_HW_TYPE = -7;  // H/W TYPE 에러 

        public const int ERR_INVALID_HANDLE = -100; // 드바이스 핸들값이 에러
        public const int ERR_CREATE_KERNEL_DRIVER_FAIL = -101;  // 커널 드라이브 생성 에러=  MCX302에는 없는 에러임 
        public const int ERR_CREATE_MEM_FAIL = -102;    // 메모리 생성 에러
        public const int ERR_INVALID_CHANNEL = -200;    // 채널 세팅이 잘못 되었습니다.
        public const int ERR_INVALID_PARAMETER = -300;  // 하나 또는 다수의 파라미터 에러
        public const int ERR_INVALID_BOARD_ID = -400;   // 보드 ID 설정 에러

        public const int ERR_FILE_CREATE = -500;    // 파일 생성 에러=  MCX302에는 없는 에러임 
        public const int ERR_EVENT_CREATE = -510;   // 이벤트 생성 에러=  MCX302에는 없는 에러임 
        public const int ERR_THREAD_CREATE = -520;  // 스레드 생성 에러=  MCX302에는 없는 에러임 

        public const int ERR_MIN_STARTV = -512; // START SPEED VALUE IS UNDER VALID SV
        public const int ERR_MAX_STARTV = -533; // START SPEED VALUE IS OVER VALID SV
        public const int ERR_MIN_WORKV = -534;  // WORK SPEED VALUE IS UNDER VALID WV
        public const int ERR_MAX_WORKV = -535;  // WORK SPEED IS OVER VALID WV
        public const int ERR_MIN_ACC = -536;    // ACC TIME IS UNDER VALID AC
        public const int ERR_MAX_ACC = -537;    // ACC TIME IS OVER VALID AC
        public const int ERR_MIN_DEC = -538;    // DEC TIME IS UNDER VALID AC
        public const int ERR_MAX_DEC = -539;    // DEC TIME IS OVER VALID AC

        public const int ERR_AXIS_NUM = -540;   // 축 번호 에러

        public const int ERR_MAX_DISTANCE = -541;   // 이동 거리가 최대값 이상 

        public const int ERR_ALLOCATE_BUF = -542;   // 버퍼 생성 실패=  MCX302에는 없는 에러임 
        public const int ERR_FREE_BUF = -543;   // 버퍼 핸들 실패=  MCX302에는 없는 에러임 
        public const int ERR_BUF_DATA_NOT_ENOUGH = -544;    // 버퍼 한개의 데이타만 생성=  MCX302에는 없는 에러임 
        public const int ERR_BUF_DATA_OVERFLOW = -545;  // 버퍼 갯수 OVERFLOW=  MCX302에는 없는 에러임 

        public const int ERR_AXIS_DRV_BUSY = -546;  // 할당된 축의 모션 중인 상태
        public const int ERR_AXIS_HOME_BUSY = -547; // 할당된 축의 ORG SEARCH 중인 상태
        public const int ERR_AXIS_STEADY = -548;    // 정지 상태

        public const int ERR_OVERRIDE_TYPE = -549;  // 오버라이드 타입 에러

        public const int ERR_MPG_MODE = -550;   // MPG 사용중 에러

        public const int ERR_COUNTER_MODE = -551;   // 소프트 리미트 사용중 모드 에러
        public const int ERR_COMP_MODE = -552;  // COMP 트리거 사용중 모드 에러

        public const int ERR_STOP_BY_SLPT = -600;   // = + 방향 소프트웨어 리미트에 의한 정지
        public const int ERR_STOP_BY_SLMT = -610;   // = - 방향 소프트웨어 리미트에 의한 정지
        public const int ERR_STOP_BY_HLPT = -620;   // = + 방향 하드웨어 리미트에 의한 정지
        public const int ERR_STOP_BY_HLMT = -630;   // = - 방향 하드웨어 리미트에 의한 정지
        public const int ERR_STOP_BY_ALM = -640;    // 서보 알람에 의한 정지
        public const int ERR_STOP_BY_EMG = -650;    // 비상정지에 의한 정지
        public const int ERR_STOP_BY_EZ = -660; // EZ에 의한 정지

        public const int ERR_STOP_BY_ERR_LIMIT = -700;  // 스케일 보정 에러 범위 초과
        public const int ERR_STOP_BY_COMPARE_NO = -701; // 스케일 보정 보정 회수 초과
        public const int ERR_STOP_BY_HUNTING_NO = -702; // 스케일 보정 헌팅 회수 초과

        public const int ERR_UNKNOWN = -9999;   // 알수 없는 에러	


        public const int CMD_FALSE = 0;  //OFF
        public const int CMD_TRUE = 1;  // ON

        public const int CMD_TMODE = 0;   //TRAPEZOIDAL= 사다리꼴 사감속
        public const int CMD_SMODE = 1;  //S-CURVE = S-CURVE 가감속

        public const int CMD_DIR_N = 0;  //= - 방향
        public const int CMD_DIR_P = 1;  //= + 방향

        public const int CMD_CW = 0;  //CW 방향 
        public const int CMD_CCW = 1;  //CCW 방향				

        public const int CMD_STAND = 0;  //모션 정지 상태
        public const int CMD_RUNNING = 1;  //모션중인 상태

        public const int ARC_CW = 0;  //원호시계방향= CW 회전
        public const int ARC_CCW = 1;  //원호반시계방향= CCW 회전				

        public const int CMD_LOGIC_A = 0;   //A 접점 
        public const int CMD_LOGIC_B = 1;   //B 접점

        public const int CMD_EMG = 0;  //감속 없이 즉시 정지
        public const int CMD_DEC = 1;  //감속 정지

        public const int CMD_OFF = 0;   // OFF = 0
        public const int CMD_ON = 1;    // ON = 1 

        public const int CMD_DISABLE = 0;   // DISABLE = 0
        public const int CMD_ENABLE = 1;    // ONLY DEBUG = 1
        public const int CMD_PARAMETER = 2; // ONLY PARAMTER = 2
        public const int CMD_ALL = 3;   // DEBUG AND PARAMETER = 3				

        public const int CMD_COMM = 0;   // 지령위치= Command Position
        public const int CMD_FEED = 1;   // 실제위치= Feedback Position

        public const int CMD_EAX4 = 0;   //4체배
        public const int CMD_EAX2 = 1;   //2체배
        public const int CMD_EAX1 = 2;   //1체배
        public const int CMD_UP_DOWN = 3;   //UP_DOWN

        public const int CMD_OVER_NONE = 0;   // 오버라이드 안됨
        public const int CMD_OVER_INC = 1;   // INC 오버라이드 
        public const int CMD_OVER_ABS = 2;   // ABS 오버라이드 


        public const int CMD_INT_MESSAGE = 0;            //원도우 메시지 전달 방식을 통하여 인터럽트 통지
        public const int CMD_INT_CALLBACK = 1;           //콜백함수를 통하여 인터럽트 통지
        public const int CMD_INT_EVENT = 2;           //이벤트를 통하여 인터럽트 통지

        public const int CMD_EVT_MC = 0;                 //motion evnet 발생
        public const int CMD_EVT_IO = 1;                 //범용 I/O evnet 발생

        public const int EVT_DISABLE = 0;                // disable all event
        public const int EVT_ENABLE = 1;                 // enable  all event

        public const int EVT_NONE = 0x00;               //disable all event
        public const int EVT_C_END = 0x01;               //C-END,    interrupt active when end of constant drive
        public const int EVT_C_STA = 0x02;               //C-STA,    interrupt active when start of constant drive
        public const int EVT_D_END = 0x04;               // D-END,    interrupt active when end of drive


        public const int EVT_P_GE_CM = 0x02;             // P >= C-, interrupt active when P counter >= Comp- counter
        public const int EVT_P_L_CM = 0x04;             // P < C-,  interrupt active when P counter < Comp- counter
        public const int EVT_P_L_CP = 0x08;             // P < C-,   interrupt active when P counter < Comp- counter
        public const int EVT_P_GE_CP = 0x10;              // P >= C+,  interrupt active when P counter >= Comp+ counter














    }
}
