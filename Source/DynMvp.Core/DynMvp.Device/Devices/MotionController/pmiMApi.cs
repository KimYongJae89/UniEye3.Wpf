/******************************************************************************
*
*	File Version: 3,0,1,0
*
*	Copyright(c) 2012 Alpha Motion Co,. Ltd. All Rights Reserved.
*
*	This file is strictly confidential and do not distribute it outside.
*
*	MODULE NAME :
*		pmiMApi.cs
*
*	AUTHOR :
*		K.C. Lee
*
*	DESCRIPTION :
*		the header file for RC files of project.
*
*
* - Phone: +82-31-303-5050
* - Fax  : +82-31-303-5051
* - URL  : http://www.alphamotion.co.kr,
*
*
/****************************************************************************/


using System.Runtime.InteropServices;

public class pmiMApi
{

    //***********************************************************************************************
    //		디바이스 시작/종료 및 초기화
    //***********************************************************************************************

    //하드웨어 장치를 로드하고 초기합니다
    //모든 함수의 제어기번호 설정은 0 ~ (실제 시스템에 장착된 제어기 - 1) 범위에서 유효
    //bManual = TRUE, Con Number is set to dip switch(Default)
    //nNumCons In a computer-set number of board
    [DllImport("pmiMApi.dll")]
    public static extern int pmiSysLoad(int bManual, ref int npNumCons);

    //하드웨어 장치를 언로드합니다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiSysUnload();

    //하드웨어 및 소프트웨어를 초기화합니다
    [DllImport("pmiMApi.dll")]
    public static extern int pmiConInit(int nCon);

    //========================================================================================================
    //                                   Motion Parameter Management public static extern int
    //========================================================================================================

    //모든 Controller에 설정값을 지정한 파일에서 읽어 제어기에 설정한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiConParamLoad(string szFilename);

    //현재 모든 Controller에 대한 모션 파라메타를 축 별로 .prm 파일에 저장한다
    [DllImport("pmiMApi.dll")]
    public static extern int pmiConParamSave(string szFilename);

    //============================================================================================
    //                       Motion interface I/O Configure and Control public static extern int
    //===========================================================================================

    //지정 축의 Servo-On 신호를 출력한다.
    //on=0, emOFF.
    //on=1, emON.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetServoOn(int nCon, int nAxis, int nState);

    //지정 축의 Servo-On 신호 상태을 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetServoOn(int nCon, int nAxis, ref int npState);

    //지정 축의 Servo-Alarm Reset 신호의 출력을 설정한다.
    //nReset=0, emOFF.
    //nReset=1, emON.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetServoReset(int nCon, int nAxis, int nState);

    //지정 축의 Servo-Alarm Reset 신호의 출력 상태을 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetServoReset(int nCon, int nAxis, ref int npState);

    //지정 축의 Inposition 신호  Active Level를 설정한다.
    //nLevel=0, emLOGIC_A.
    //nLevel=1, emLOGIC_B.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetServoInpLevel(int nCon, int nAxis, int nLevel);

    //지정 축의 Inpos 신호 Active Level 설정를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetServoInpLevel(int nCon, int nAxis, ref int npLevel);

    //지정 축의 Inpos 신호 사용 여부를 설정한다.
    //nEnable=0, emFALSE.
    //nEnable=1, emTRUE.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetServoInpEnable(int nCon, int nAxis, int nEnable);

    //지정 축의 Inpos 신호 사용 여부를 반환한다.
    //nEnable=0, emFALSE.
    //nEnable=1, emTRUE.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetServoInpEnable(int nCon, int nAxis, ref int npEnable);

    //지정 축의 Inpos 신호의 입력 상태를 반환한다.
    //nInp=0, emINACTIVED.
    //nInp=1, emACTIVED.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetServoInp(int nCon, int nAxis, ref int npInp);

    //지정 축의 알람 신호의 Active Level을 설정한다.
    //nLevel=0, emLOGIC_A.
    //nLevel=1, emLOGIC_B.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetServoAlarmLevel(int nCon, int nAxis, int nLevel);

    //지정 축의 알람 신호의 Active Level 설정을 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetServoAlarmLevel(int nCon, int nAxis, ref int npLevel);

    //지정 축의 알람 신호 입력 시 정지의 방법를 설정한다.
    //nAction=0, emESTOP.
    //nAction=1, emSSTOP.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetServoAlarmAction(int nCon, int nAxis, int nAction);

    //지정 축의 알람 신호 입력시 정지 방법를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetServoAlarmAction(int nCon, int nAxis, ref int npAction);

    //지정 축의 기계적 +/- 리미티 센서 신호 Active Level를 설정한다.
    //nLevel=0, emLOGIC_A.
    //nLevel=1, emLOGIC_B.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetLimitLevel(int nCon, int nAxis, int nLevel);

    //지정 축의 기계적 +/- 리미티 센서 신호 Active Level를 설정 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetLimitLevel(int nCon, int nAxis, ref int npLevel);

    //지정 축의 기계적 +/- 리미티 입력 신호 검출 후 정지 방법를 설정한다.
    //nAction=0, emESTOP.
    //nAction=1, emSSTOP.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetLimitAction(int nCon, int nAxis, int nAction);

    //지정 축의 기계적 +/- 리미티 입력 신호 검출 후 정지 방법를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetLimitAction(int nCon, int nAxis, ref int npAction);

    //지정 축의 원점 입력 신호 Active Level를 설정한다.
    //nLevel=0, emLOGIC_A.
    //nLevel=1, emLOGIC_B.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetOrgLevel(int nCon, int nAxis, int nLevel);

    //지정 축의 원점 센서 신호 Active Level를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetOrgLevel(int nCon, int nAxis, ref int npLevel);

    //지정 축의 Z상 입력 신호 Active Level를 설정한다.
    //nLevel=0, emLOGIC_A.
    //nLevel=1, emLOGIC_B.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetEzLevel(int nCon, int nAxis, int nLevel);

    //지정 축의 Z상 신호 Active Level 설정를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetEzLevel(int nCon, int nAxis, ref int npLevel);

    //지정 축의 소프트 리미트 위치값을 설정한다.
    //dLimitP = -134217728 ~ 134217727.
    //dLimitN = -134217728 ~ 134217727.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetSoftLimitPos(int nCon, int nAxis, double dLimitP, double dLimitN);

    //지정 축의 소프트 리미트 위치값를 반환한다
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetSoftLimitPos(int nCon, int nAxis, ref double dpLimitP, ref double dpLimitN);

    //지정 축의 소프트 리미트  +/- 리미티 입력 신호 검출 후 정지 방법를 반환한다.
    //nAction=0, 정지하지 않음.
    //nAction=1, emESTOP.
    //nAction=2, emSSTOP.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetSoftLimitAction(int nCon, int nAxis, int nAction);

    //지정 축의 소프트 리미트 +/- 리미티 입력 신호 검출 후 정지 방법를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetSoftLimitAction(int nCon, int nAxis, ref int npAction);

    //지정 축의 소프트 리미트 기능 사용 유무을 설정한다.
    //nEnable=0, emFALSE.
    //nEnable=1, emTRUE.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetSoftLimitEnable(int nCon, int nAxis, int nEnable);

    //지정 축의 소프트 리미트 기능 사용 유무를 반환한다
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetSoftLimitEnable(int nCon, int nAxis, ref int npEnable);

    //지정 축의 링 카운터 초기화 위치값을 설정한다
    //dPos = 0 ~ 134217727.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetRCountResetPos(int nCon, int nAxis, double dPos);

    //지정 축의 링 카운터 초기화 위치값를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetRCountResetPos(int nCon, int nAxis, ref double dpPos);

    //지정 축의 링 카운터 기능 사용 유무을 설정한다.
    //nEnable=0, emFALSE.
    //nEnable=1, emTRUE.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetRCountEnable(int nCon, int nAxis, int nEnable);

    //지정 축의 링 카운터 기능 사용 유무를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetRCountEnable(int nCon, int nAxis, ref int npEnable);

    //지정 축의 CRC(Current Remaining Clear) 신호 Active Level를 설정한다.
    //nLevel =0, emLOGIC_A.
    //nLevel =1, emLOGIC_B.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetServoCrcLevel(int nCon, int nAxis, int nLevel);

    //지정 축의 CRC(Current Remaining Clear) 신호 Active Level를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetServoCrcLevel(int nCon, int nAxis, ref int npLevel);

    //지정 축의 CRC(Current Remaining Clear)  제거 출력 신호 펄스 시간를 설정한다.
    //nOnTime =0, 12us.
    //nOnTime =1, 102us.
    //nOnTime =2, 408us.
    //nOnTime =3, 1.1ms.
    //nOnTime =4, 13ms.
    //nOnTime =5, 52ms.
    //nOnTime =6, 104ms.
    //nOnTime =7, Level
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetServoCrcTime(int nCon, int nAxis, int nOnTime);

    //지정 축의 CRC(Current Remaining Clear)  제거 출력 신호 펄스 시간을 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetServoCrcTime(int nCon, int nAxis, ref int npOnTime);

    //지정 축의 EL, ALM, EMG 입력에 의해 급 정지 했을 때에 CRC 신호 출력여부를 설정한다..
    //nEnable=0, emFALSE.
    //nEnable=1, emTRUE.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetServoCrcEnable(int nCon, int nAxis, int nEnable);

    //지정 축의 EL, ALM, EMG 입력에 의해 급 정지 했을 때에 CRC 신호 출력여부를 반환한다..
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetServoCrcEnable(int nCon, int nAxis, ref int npEnable);

    //지정 축의 CRC 신호를 소프트웨어로 ON 시킨다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetServoCrcOn(int nCon, int nAxis);

    //지정 축의CRC 신호를 소프트웨어로 OFF 시킨다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetServoCrcReset(int nCon, int nAxis);

    //Reset COUNTER when the CLR input turns OFF->ON.
    //Action = 0, Falling edge clear
    //Action = 1, Rising edge clear
    //Action = 2, Low Level clear
    //Action = 3, Hi Level clear
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetCountAction(int nCon, int nAxis, int nAction);

    //Reset COUNTER when the CLR input turns OFF->ON.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetCountAction(int nCon, int nAxis, ref int npAction);

    //지정 축의 4개의 COUNTER(지령위치,실위치,편차,범용) 초기화 시킬 입력 신호 사용 유무를 설정한다.
    //nReset=0, emFALSE.
    //nReset=1, emTRUE.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetCountReset(int nCon, int nAxis, int nEnable);

    //지정 축의 COUNTER(지령위치,실위치,편차,범용)를 초기화 사용 유무를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetCountReset(int nCon, int nAxis, ref int npEnable);

    //지정 축의 SD(Start of Deceleration) 신호 Active Level를 설정한다.
    //nLevel=0, emLOGIC_A.
    //nLevel=1, emLOGIC_B.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetSdLevel(int nCon, int nAxis, int nLevel);

    //지정 축의 SD(Start of Deceleration) 신호 Active Level를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetSdLevel(int nCon, int nAxis, ref int npLevel);

    //지정 축의 SD(Start of Deceleration) 신호 를 설정한다.
    //nAction=0, emSDSLOW.
    //nAction=1, emSDSTOP.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetSdAction(int nCon, int nAxis, int nStop);

    //지정 축의 SD(Start of Deceleration) 신호를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetSdAction(int nCon, int nAxis, ref int npStop);

    //지정 축의 SD(Start of Deceleration) 신호 ON때의 LATCH를 설정한다.
    //nLatch=0, emFALSE.
    //nLatch=1, emTRUE.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetSdLatch(int nCon, int nAxis, int nLatch);

    //지정 축의 SD(Start of Deceleration) 신호 ON때의 LATCH를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetSdLatch(int nCon, int nAxis, ref int npLatch);

    //지정 축의 SD(Start of Deceleration) 신호 사용 유무를 설정한다.
    //nEnable=0, emFALSE.
    //nEnable=1, emTRUE.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetSdEnable(int nCon, int nAxis, int nEnable);

    //지정 축의 SD(Start of Deceleration) 신호 사용 유무를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetSdEnable(int nCon, int nAxis, ref int npEnable);

    //지정 축의 PCS(Target Position Override) 신호 Active Level를 설정한다.
    //nLevel=0, emLOGIC_A.
    //nLevel=1, emLOGIC_B.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetPcsLevel(int nCon, int nAxis, int nLevel);

    //지정 축의 PCS(Target Position Override) 신호 Active Level 설정를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetPcsLevel(int nCon, int nAxis, ref int npLevel);

    //지정 축의 PCS(Target Position Override) 신호 사용 유무를 설정한다.
    //nEnable =0, emFALSE.
    //nEnable =1, emTRUE.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetPcsEnable(int nCon, int nAxis, int nEnable);

    //지정 축의 PCS(Target Position Override) 신호 사용 유무를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetPcsEnable(int nCon, int nAxis, ref int npEnable);

    //지정 축의 STA(Start simultaneously from an external circuit) Active 신호를 설정한다.
    //nAction =0, emACT_LEVEL.
    //nAction =1, emACT_FALL.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetStaAction(int nCon, int nAxis, int nAction);

    //지정 축의 STA(Start simultaneously from an external circuit) Active 신호를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetStaAction(int nCon, int nAxis, ref int npAction);

    //지정 축의 STA(Start simultaneously from an external circuit) 입력 신호 사용 유무를 설정한다..
    //nEnable =0, emFALSE.
    //nEnable =1, emTRUE.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetStaEnable(int nCon, int nAxis, int nEnable);

    //지정 축의 STA(Start simultaneously from an external circuit) 입력 신호 사용 유무를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetStaEnable(int nCon, int nAxis, ref int npEnable);

    //동시 시작 S/W command
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxStaBeginCmd(int nCon, int nAxis);

    //지정 축의 STP(Stop simultaneously from an external circuit) 신호 ON 될 때 정지 방법을 설정한다.
    //nAction =0, emESTOP.
    //nAction =1, emSSTOP.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetStpAction(int nCon, int nAxis, int nAction);

    //지정 축의 STP(Stop simultaneously from an external circuit) 신호 ON 될 때 정지 방법을 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetStpAction(int nCon, int nAxis, ref int npAction);

    //지정 축의 STP(Stop simultaneously from an external circuit) 입력 신호 사용 유무를 설정한다.
    //nEnable =0, emFALSE.
    //nEnable =1, emTRUE.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetStpEnable(int nCon, int nAxis, int nEnable);

    //지정 축의 STP(Stop simultaneously from an external circuit) 입력 신호 사용 유무를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetStpEnable(int nCon, int nAxis, ref int npEnable);

    //동시 정지 S/W command
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxStpBeginCmd(int nCon, int nAxis);

    //지정 축의 EA/EB/EZ 및 PA/PB 입력 신호 필터 사용 유무를 설정한다.
    //nTarget =0, PA/PB
    //nTarget =1, EA/EB/EZ
    //nEnable =0, emFALSE.
    //nEnable =1, emTRUE.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetFilterABEnable(int nCon, int nAxis, int nTarget, int nEnable);

    //지정 축의 EA/EB/EZ 및 PA/PB 입력 신호 필터 사용 유무를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetFilterABEnable(int nCon, int nAxis, ref int nTarget, ref int npEnable);

    //nTarget =0, PA/PB
    //nTarget =1, EA/EB/EZ
    //지정 축의 EA/EB/EZ 및 PA/PB 입력 신호 필터 시간을 설정한다.
    //0000 : 20[ns]    0001 : 40[ns]    0010 : 60[ns]    0010 : 80[ns]
    //0011 : 100[ns]   0100 : 120[ns]   0101 : 140[ns]   0110 : 160[ns]
    //0111 : 180[ns]   1000 : 200[ns]   1001 : 220[ns]   1010 : 240[ns]
    //1100 : 260[ns]   1101 : 280[ns]   1110 : 300[ns]   1111 : 320[ns]
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetFilterABTime(int nCon, int nAxis, int nTarget, int nTime);

    //지정 축의 EA/EB/EZ 및 PA/PB 입력 신호 필터 시간을 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetFilterABTime(int nCon, int nAxis, ref int nTarget, ref int npTime);

    //지정 축의 제어 기준 모드를 설정한다.
    //nCtrCount =0, emCOMM.
    //nCtrCount =1, emFEED.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiCtrSetCount(int nCon, int nAxis, int nCount);

    //지정 축의 제어 모드를 반환한다..
    [DllImport("pmiMApi.dll")]
    public static extern int pmiCtrGetCount(int nCon, int nAxis, ref int npCount);

    //지정 축의 지령 펄스 출력 방식을 설정한다.
    //nType =0, emONELOWHIGHLOW.	1펄스 방식, PULSE(Active High), 정방향(DIR=Low)  / 역방향(DIR=High)
    //nType =1, emONEHIGHHIGHLOW.	1펄스 방식, PULSE(Active High), 정방향(DIR=High) / 역방향(DIR=Low)
    //nType =2, emONELOWLOWHIGH.	1펄스 방식, PULSE(Active Low),  정방향(DIR=Low)  / 역방향(DIR=High)
    //nType =3, emONEHIGHLOWHIGH.	1펄스 방식, PULSE(Active Low),  정방향(DIR=High) / 역방향(DIR=Low)
    //nType =4, emTWOCWCCWLOW.		2펄스 방식, PULSE(CCW:역방향),  DIR(CW:정방향),  Active High 
    //nType =5, emTWOCWCCWHIGH.		2펄스 방식, PULSE(CCW:역방향),  DIR(CW:정방향),  Active Low
    //nType =6, emTWOCCWCWLOW.		2펄스 방식, PULSE(CW:정방향),   DIR(CCW:역방향), Active High
    //nType =7, emTWOCCWCWHIGH.		2펄스 방식, PULSE(CW:정방향),   DIR(CCW:역방향), Active Low
    //nType =8, emTWOPHASE.			2상(90' 위상차),  PULSE lead DIR(CW: 정방향), PULSE lag DIR(CCW:역방향)
    //nType =9, emTWOPHASERESERVE.  2상(90' 위상차),  PULSE lead DIR(CCW: 정방향), PULSE lag DIR(CW:역방향)
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetPulseType(int nCon, int nAxis, int nType);

    //지정 축의 제어 모드를 반환한다..
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetPulseType(int nCon, int nAxis, ref int npType);

    //지정 축의 피드백 펄스 입력 방식을 설정한다.
    //nType =0, emEAB1X.   1체배
    //nType =1, emEAB2X.   2체배
    //nType =2, emEAB4X.   4체배
    //nType =3, emCWCCW.   Up/Down
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetEncType(int nCon, int nAxis, int nType);

    //지정 축의 피드백 펄스 입력 방식을 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetEncType(int nCon, int nAxis, ref int npType);

    //지정 축의 피드백 카운터 방향을 설정한다.
    //0 normal counting(Default)
    //1 reverse counting
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetEncDir(int nCon, int nAxis, int nDir);

    //지정 축의 피드백 카운터 방향을 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetEncDir(int nCon, int nAxis, ref int npDir);

    //지정 축의 피드백 카운터의 카운터 종류를 설정한다.
    //nType =0, emCOMM.
    //nType =1, emFEED.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetEncCount(int nCon, int nAxis, int nCount);

    //지정 축의 피드백 카운터의 카운터를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetEncCount(int nCon, int nAxis, ref int nㅔCount);

    //지정 축의 이송함수의 최대 이송 속도를 지정한 속도로 제한한다.
    //dVel = 0 ~ 6,553,500[pps].
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetMaxVel(int nCon, int nAxis, double dVel);

    //지정 축의 이송함수의 최대 이송 속도를 지정한 속도를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetMaxVel(int nCon, int nAxis, ref double dpVel);

    //지정 축의 조그 시작 속도 및 종료 속도를 설정한다.
    //dVel = 0 ~ 6,553,500[pps].
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetInitJogVel(int nCon, int nAxis, double dVel);

    //지정 축의 조그 시작 속도 및 종료 속도를 반환한다..
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetInitJogVel(int nCon, int nAxis, ref double dpVel);

    //지정 축의 시작 속도 및 종료 속도를 설정한다.
    //dVel = 0 ~ 6,553,500[pps].
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetInitVel(int nCon, int nAxis, double dVel);

    //지정 축의 초기 속도 및 종료 속도를 반환한다..
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetInitVel(int nCon, int nAxis, ref double dpVel);

    //조그 이송 속도 프로파일, 운전속도, 가속도를 설정한다.
    //nVelType = 0, emCONST.        가감속없는 속도 프로파일
    //nVelType = 1, emTCURVE.       Trapezode 속도 프로파일
    //nVelType = 2, emSCURVE.       S Curve   속도 프로파일
    //dVel     = 0 ~ 6,553,500[pps].
    //dTacc    = 0 ~ 65000[msec].
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetJogVelProf(int nCon, int nAxis, int nType, double dVel, double dTacc);

    //조그 이송 속도 프로파일, 운전속도, 가속도를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetJogVelProf(int nCon, int nAxis, ref int npType, ref double dpVel, ref double dpTacc);

    //지정 축의 이송 속도 프로파일, 운전속도, 가속도 및 감속도를 설정한다.
    //nVelType = 0, emCONST.        가감속없는 속도 프로파일
    //nVelType = 1, emTCURVE.       Trapezode 속도 프로파일
    //nVelType = 2, emSCURVE.       S Curve   속도 프로파일
    //dVel     = 0 ~ 6,553,500[pps].
    //dTacc    = 0 ~ 65000[msec].
    //dTdec    = 0 ~ 65000[msec].
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetVelProf(int nCon, int nAxis, int nType, double dVel, double dTacc, double dTdec);

    //지정 축의 이송 속도 프로파일, 운전속도, 가속도 및 감속도를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetVelProf(int nCon, int nAxis, ref int npType, ref double dpVel, ref double dpTacc, ref double dpTdec);


    //지정 축의 감속 방법 대하여 설정한다.
    //nType = 0, AutoDetect.        자동 감속점 설정
    //nType = 1, ManualDetect.       수동 감속점 설정
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetDecelType(int nCon, int nAxis, int nType);

    //지정 축의 감속 방법 대하여 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetDecelType(int nCon, int nAxis, ref int npType);

    //지정 축의 감속 시작 위치에 대하여 설정한다.
    //dPul = -8,388,608 ~ 8,388,607[pulses]
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetRemainPul(int nCon, int nAxis, double dPulse);

    //지정 축의 감속 시작 위치에 대하여를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetRemainPul(int nCon, int nAxis, ref double dpPulse);

    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetFilterEnable(int nCon, int nAxis, int nEnable);

    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetFilterEnable(int nCon, int nAxis, ref int nEnable);


    //========================================================================================================
    //                                   HOME-RETURN public static extern intS
    //========================================================================================================

    //지정 축의 원점 검색 완료 후 카운터 Reset을 설정하는 함수이다
    //nResetPos =0, 하드웨어 신호가 입력되는 순간에 Command 및 Feedback의 위치가 0 으로 설정. 일정량의 Feedback 위치 편차가 보임.
    //nResetPos =1, 원점 복귀가 완료 된 후 Command 및 Feedback의 위치가 모두 자동으로 0 으로 설정
    //nResetPos =2, 원점 복귀가 완료 된 후 Feedback 위치와 동일한 값으로 Command 위치를 설정
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxHomeSetResetPos(int nCon, int nAxis, int nResetPos);

    //지정 축의 원점 검색 완료 후 카운터 Reset을 반환하는 함수이다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxHomeGetResetPos(int nCon, int nAxis, ref int npResetPos);

    //Automatically outputs an CRC signal when the axis is stopped immediately by a ORG input signal. However, the CRC signal is not output when a
    //deceleration stop occurs on the axis. 
    //Even if the EL signal is specified for a normal stop, by setting MOD = "010X000" (feed to the EL position) in the RMD register, 
    //the CRC signal is output if an immediate stop occurs
    //crc_enable=0
    //crc_enable=1
    //TMC-BBxxP is not Supported a Function.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxHomeSetCrcEnable(int nCon, int nAxis, int nEnable);

    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxHomeGetCrcEnable(int nCon, int nAxis, ref int npEnable);

    //지정 축의 원점검색을 수행하기 위해 원점 검색 방법을 설정한다.
    //TYPE=0 ORG ON -> Slow down -> Stop 
    //TYPE=1 ORG ON -> Stop -> Go back(Rev Spd) -> ORG OFF -> Go forward(Rev Spd) -> ORG ON -> Stop(Default)
    //TYPE=2 ORG ON -> Slow down(Low Spd) -> Stop on EZ signal
    //TYPE=3 ORG ON -> EZ signal -> Slow down -> Stop
    //TYPE=4 ORG ON -> Stop -> Go back(Rev Spd) -> ORG OFF -> Stop on EZ signal
    //TYPE=5 ORG ON -> Stop -> Go back(High Spd) -> ORG OFF -> EZ signal -> Slow down -> Stop
    //TYPE=6 EL ON -> Stop -> Go back(Rev Spd) -> EL OFF -> Stop
    //TYPE=7 EL ON -> Stop -> Go back(Rev Spd) -> EL OFF -> Stop on EZ signal
    //TYPE=8 EL ON -> Stop -> Go back(High Spd) -> EL OFF -> Stop on EZ signal
    //TYPE=9 ORG ON -> Slow down -> Stop -> Go back -> Stop at beginning edge of ORG
    //TYPE=10 ORG ON -> EZ signal -> Slow down -> Stop -> Go back -> Stop at beginning edge of EZ;
    //TYPE=11 ORG ON -> Slow down -> Stop -> Go back (High Spd) -> ORG OFF -> EZ signal -> Slow down -> Stop -> Go forward(High Spd) -> Stop at beginning edge of EZ
    //TYPE=12 EL ON -> Stop -> Go back (High Spd) -> EL OFF -> EZ signal -> Slow down -> Stop -> Go forward(High Spd) -> Stop at beginning edge of EZ
    //TYPE=13 EZ signal -> Slow down -> Stop
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxHomeSetType(int nCon, int nAxis, int nType);

    //지정 축의 원점검색을 수행하기 위한 원점 검색 방법를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxHomeGetType(int nCon, int nAxis, ref int npType);

    //지정 축의 원점검색을 초기에 진행 할 방향을 설정한다.
    //nDir =0, emDIR_P
    //nDir =1, emDIR_N
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxHomeSetDir(int nCon, int nAxis, int nDir);

    //지정 축의 원점검색을 초기에 진행 할 방향를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxHomeGetDir(int nCon, int nAxis, ref int npDir);

    //The origin of the origin of the behavior to escape from the distance sensor
    //pps ( -134217728 ~ 134217727 )
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxHomeSetEscapePul(int nCon, int nAxis, double dEscape);

    //The origin of the origin of the behavior to escape from the distance sensor
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxHomeGetEscapePul(int nCon, int nAxis, ref double dpEscape);

    //지정 축의 원점검색 시 Ez상을 몇번 카운터 할지 설정한다.
    //nEzCount(1 ~ 16)
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxHomeSetEzCount(int nCon, int nAxis, int nCount);

    //지정 축의 원점검색 시 Ez상을 몇번 카운터 할지 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxHomeGetEzCount(int nCon, int nAxis, ref int npCount);

    //지정 축의 원점검색이 완료된 후 기구 원점으로 이동 후 원점 재설정 할 위치를 설정한다.
    //dShift = 0 ~ 6553500(pulses)
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxHomeSetShiftDist(int nCon, int nAxis, double dShift);

    //지정 축의 원점검색이 완료된 후 기구 원점으로 이동 후 원점 재설정 할 위치를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxHomeGetShiftDist(int nCon, int nAxis, ref double dpShift);

    //지정 축의 원점센서가 감지될 때 이송 속도를 설정한다.
    //dVel = 0 ~ 6553500(pps)
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxHomeSetInitVel(int nCon, int nAxis, double dVel);

    //지정 축의 원점센서가 감지될 때 이송 속도를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxHomeGetInitVel(int nCon, int nAxis, ref double dpVel);

    //지정 축의 원점 검색 속도 프로파일, 운전속도, 역방향속도 및 가속도를 설정한다.
    //nVelType = 0, emCONST.        가감속없는 속도 프로파일
    //nVelType = 1, emTCURVE.       Trapezode 속도 프로파일
    //nVelType = 2, emSCURVE.       S Curve   속도 프로파일
    //dVel     = 0 ~ 6,553,500[pps].
    //dRevVel  = 0 ~ 65000[msec].   원점 탈출 속도
    //dTacc    = 0 ~ 65000[msec].
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxHomeSetVelProf(int nCon, int nAxis, int nType, double dVel, double dRevVel, double dTacc);

    //지정 축의 원점 검색 속도 프로파일, 운전속도, 역방향속도 및 가속도를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxHomeGetVelProf(int nCon, int nAxis, ref int npType, ref double dpVel, ref double dpRevVel, ref double dpTacc);

    //지정 축에 원점 검색 시작 함수이다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxHomeMove(int nCon, int nAxis);

    //다 축에 원점 검색 시작 함수이다.
    //nNAxis  배열 개수
    //naAxis  축 번호 (0 ~ (최대축수 - 1)) 배열(nNAxis에서 설정한 개수보다 같거나 크게 선언해야됨)
    [DllImport("pmiMApi.dll")]
    public static extern int pmiMxHomeMove(int nCon, int nNAxis, int[] naAxis);

    //지정 축에 원점 검색 모션이 완료됐는지를 반환한다.
    //nDone =0, emSTAND
    //nDone =1, emRUNNING
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxHomeCheckDone(int nCon, int nAxis, ref int npDone);

    //지정 축의 원점 검색 함수를 이용해 원점 검색이 수행되고 나면 검색 결과를 강제적으로 설정한다.
    //nStatus =0, emFALSE
    //nStatus =1, emTRUE
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxHomeSetStatus(int nCon, int nAxis, int nStatus);

    //지정 축의 원점 검색 함수를 이용해 원점 검색이 수행되고 나면 검색 결과를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxHomeGetStatus(int nCon, int nAxis, ref int npStatus);

    //========================================================================================================
    //                                  Velocity mode And Single Axis Position Motion Configure
    //========================================================================================================

    //지정 축의 설정된 방향으로 연속 이송하는 속도모드 함수이다.
    //nDir =0, emDIR_P,    CW 방향
    //nDir =1, emDIR_N,    CCW 방향
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxJogMove(int nCon, int nAxis, int nDir);

    //지정 축의 설정된 위치까지 이송 한다
    //nAbsMode = 0, emINC   상대좌표
    //nAbsMode = 1, emABS   절대좌표
    //dPos     = -134217728 ~ 134217727[Pulses]  이송 거리
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxPosMove(int nCon, int nAxis, int nAbsMode, double dPos);

    //지정 축에 모션이 완료됐는지를 반환한다.
    //nDone =0, emSTAND
    //nDone =1, emRUNNING
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxCheckDone(int nCon, int nAxis, ref int npDone);

    //[DllImport("pmiMApi.dll")]
    //public static extern int pmiAxWaitDone(int nCon, int nAxis);    

    //지정 축의 이송 중인 모션 동작을 사용자가 준 감속 시간으로 정지하는 함수이다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxStop(int nCon, int nAxis);

    //지정 축의 현재 이송 중인 모션 동작을 감속 없이 급정지하는 함수이다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxEStop(int nCon, int nAxis);

    //지정 축에 대하여 이송명령을 전달되었을 때 이송동작의 시작이 다른축의 동작 상황에 동기되어 시작되도록 설정한다..
    //nType =0    다른 축 동기 사용하지 않음.
    //nType =1,   다른축 동기 시작 신호에 의해 시작.
    //nType =2    지정축 이송이 완료될 때 이송 시작.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetSyncType(int nCon, int nAxis, int nType);

    //지정 축에 대하여 이송명령을 전달되었을 때 이송동작의 시작이 다른축의 동작 상황를 반환한다..
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetSyncType(int nCon, int nAxis, ref int npType);

    //지정 축에 대하여 이송명령을 전달되었을 때 이송동작의 시작이 다른축의 동작 상황에 동기되어 시작되도록 설정한다..
    //nType = 1    이 모드에서는 이 값에 축 번호를 지정한다. 단 주의할 것은 nAxis 매개 변수가 0 ~ 3 사이의 축인 경우에는 이 값도 0 ~ 3 이어야 한다. 그리고 nAxis 매개 변수가 4 ~ 7 사이의 축인 경우에는 이 값도 4 ~ 7 이어야 한다.
    //nType = 2    이 모드에서는 이 값에 축 마스트를 지정한다. 이 경우에는 참조 축을 여러 개 설정 할 수 있으며, 각 비트별로 값이 1인 경우 해당축이 참조된다.
    //             예를 들어 emAXIS0, emAXIS2의 두축이 모두 정지하는 시점에 출발하고자 한다면 nMaskAxes값은 0x5로 설정한다. 단 주의할 것은 nAxis 매개 변수가 0 ~ 3 사이의 축인 경우에는 nMaskAxes 값도 BIT0 ~ BIT3 만 사용할 수 있으며, nAxis 매개 변수가 4 ~ 7 사이의 축인 경우에는 nMaskAxes 값도 BIT4 ~ BIT7 만 사용할 수 있다.

    //nCondition =0    내부 동기 신호 출력 OFF
    //nCondition =1    참조축이 가속을 시작 할 때 이송 시작
    //nCondition =2    참조축이 가속이 끝나고 정속 시작 할 때 이송 시작
    //nCondition =3    참조축이 감속을 시작 할 때 이송 시작
    //nCondition =4    참조축이 감속을 끝날 때 이송 시작
    //nCondition =5    -SL 신호가 검출되었을 때 이송 시작
    //nCondition =6    +SL 신호가 검출되었을 때 이송 시작
    //nCondition =7    범용 비교기에 설정된 조건이 만족되었을 때 이송 시작
    //nCondition =8    TRG-CMP 조건이 만족되었을 때 이송 시작
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetSyncAction(int nCon, int nAxis, int nMaskAxes, int nCondition);

    //지정 축에 대하여 이송명령을 전달되었을 때 이송동작의 시작이 다른축의 동작를 반환한다..
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetSyncAction(int nCon, int nAxis, int nMaskAxes, ref int npContion);

    //========================================================================================================
    //                                  Velocity mode And Multi Axis Point to Point Motion Configure
    //========================================================================================================

    //다 축의 설정된 방향으로 연속 이송하는 속도모드 함수이다.
    //nNAxis  배열 개수
    //naAxis  축 번호 (0 ~ (최대축수 - 1)) 배열(nNAxis에서 설정한 개수보다 같거나 크게 선언해야됨)
    //naDir   이송 방향 배열(nNAxis에서 설정한 개수보다 같거나 크게 선언해야됨)
    //nDir = 0, emDIR_P,    CW 방향
    //nDir = 1, emDIR_N,    CCW 방향
    [DllImport("pmiMApi.dll")]
    public static extern int pmiMxJogMove(int nCon, int nNAxis, int[] naAxis, int[] naDir);

    //다 축의 설정된 방향으로 연속 이송하는 속도모드 함수이다.
    //nNAxis  배열 개수
    //nAbsMode =0, emINC. 상대좌표
    //nAbsMode =1, emABS. 절대좌표
    //naAxis  축 번호 (0 ~ (최대축수 - 1)) 배열(nNAxis에서 설정한 개수보다 같거나 크게 선언해야됨)
    //daDist  이송 거리(Pulses) 배열(nNAxis에서 설정한 개수보다 같거나 크게 선언해야됨)
    [DllImport("pmiMApi.dll")]
    public static extern int pmiMxPosMove(int nCon, int nNAxis, int nAbsMode, int[] naAxis, double[] daDist);

    //다 축에 대해서 모션이 완료됐는지를 반환한다
    //nNAxis  배열 개수
    //naAxis  축 번호 (0 ~ (최대축수 - 1)) 배열(nNAxis에서 설정한 개수보다 같거나 크게 선언해야됨)
    //nDone =0, emSTAND
    //nDone =1, emRUNNING
    [DllImport("pmiMApi.dll")]
    public static extern int pmiMxCheckDone(int nCon, int nNAxis, int[] naAxis, ref int npDone);

    //다 축에 대해서 이송 중인 모션 동작을 감속 정지하는 함수이다.
    //nNAxis  배열 개수
    //naAxis  축 번호 (0 ~ (최대축수 - 1)) 배열(nNAxis에서 설정한 개수보다 같거나 크게 선언해야됨)
    [DllImport("pmiMApi.dll")]
    public static extern int pmiMxStop(int nCon, int nNAxis, int[] naAxis);

    //다 축에 대해서 이송 중인 모션 동작을 급정지하는 함수이다.
    //nNAxis  배열 개수
    //naAxis  축 번호 (0 ~ (최대축수 - 1)) 배열(nNAxis에서 설정한 개수보다 같거나 크게 선언해야됨)
    [DllImport("pmiMApi.dll")]
    public static extern int pmiMxEStop(int nCon, int nNAxis, int[] naAxis);

    //========================================================================================================
    //                                  Motion I/O Monitoring public static extern intS
    //========================================================================================================

    //지정 축의 외부 센서 및 모터 관련 신호들의 상태를 반환한다..
    //BIT0    비상정지(EMG) 신호 입력 상태
    //BIT1    Alarm 신호 입력 상태
    //BIT2    +EL 정지 신호 입력 상태
    //BIT3    -EL 정지 신호 입력 상태
    //BIT4    원점 신호 상태
    //BIT5    펄스 출력 방향 상태(  0 : +방향, - : -방향 )
    //BIT6    원점 검색 완료 성공 여부
    //BIT7    PCS(Position Override) 신호 입력 상태
    //BIT8    CRC 신호 입력 상태
    //BIT9    Z상 신호 입력 상태
    //BIT10   CLR 입력 신호 상태
    //BIT11   LATCH(Position Latch) 신호 입력 상태
    //BIT12   SD(Slow Down) 신호 입력 상태
    //BIT13   Inpos 신호 입력 상태
    //BIT14   서보 온 신호 입력 상태
    //BIT15   알람 리셋 신호 입력 상태
    //BIT16   STA 신호 입력 상태
    //BIT17   STP 신호 입력 상태
    //BIT18   SYNC Pos Error signal input
    //BIT19   GANT Pos Erorr signal input
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetMechanical(int nCon, int nAxis, ref int npMechanical);

    //지정 축의 모션 이송 상태를 반환한다.
    //BIT0    정지 상태 중
    //BIT1    외부 스위치 신호 기다림
    //BIT2    동시 시작 신호 기다림
    //BIT3    내부 동기 신호 기다림
    //BIT4    타축 동기 신호 기다림
    //BIT5    CRC 출력 완료 기다림
    //BIT6    방향 변화 기다림
    //BIT7    백래쉬 상태
    //BIT8    PA/PB 신호 기다림
    //BIT9    원점 검색 속도 이송 중
    //BIT10   시작 속도 이송 중
    //BIT11   가속 중
    //BIT12   작업 속도 이송 중
    //BIT13   감속 중
    //BIT14   InPos 신호 기다림
    //BIT15   Reserved
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetMotion(int nCon, int nAxis, ref int npMotion);

    //BIT0 EMG Error
    //BIT1 ALM Alarm Signal Error
    //BIT2 +EL Positive Limit Switch Error
    //BIT3 -EL Negative Limit Switch Error
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetErrStatus(int nCon, int nAxis, ref int npErrStatus);

    //지정 축의 현재 이송 지령 속도를 읽어온다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetCmdVel(int nCon, int nAxis, ref double dpVel);

    //지정 축의 현재 피드백 속도를 읽어온다.
    //이 함수를 사용할려면 pmiGnSetCheckActSpeed 함수를 사용해야한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetActVel(int nCon, int nAxis, ref double dpVel);

    //지정 축의 지령 위치를 설정한다.
    //dPos = -134,217,727 ~ +134,217,727
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetCmdPos(int nCon, int nAxis, double dPos);

    //지정 축의 지령 위치를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetCmdPos(int nCon, int nAxis, ref double dpPos);

    //지정 축의 피드백 위치를 설정한다.
    //dPos = -134,217,727 ~ +134,217,727
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetActPos(int nCon, int nAxis, double dPos);

    //지정 축의 피드백 위치를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetActPos(int nCon, int nAxis, ref double dpPos);


    //지정 축의 지령위치(Command)과 피드백위치(Encoder)의 편차를 설정한다.
    //dErrPos = -32,768 ~ +32,767   편차 카운터
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetPosError(int nCon, int nAxis, double dErrPos);

    //지정 축의 지령위치(Command)과 피드백위치(Encoder)의 편차를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetPosError(int nCon, int nAxis, ref double dpErrPos);

    //지정 축의 편차 카운터 에러 발생를 설정한다.
    //dMethod
    //emMTH_NONE      0    성립하지 않음
    //emMTH_EQ_DIR    1    Counter 방향과 무관
    //emMTH_EQ_PDIR   2    Counter Up 중
    //emMTH_EQ_NDIR   3    Counter Down 중
    //dAction
    //emEVT_ONLY      0    처리 하지 않음
    //emEVT_ESTOP     1    즉시 정지
    //emEVT_STOP      2    감속 정지
    //emEVT_SPDCHG    3    속도 변경
    //dPos =-134,217,727 ~ +134,217,727[pulses] 편차 카운터
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetPosErrAction(int nCon, int nAxis, int nMethod, int nAction, double dPos);

    //지정 축의 편차 카운터를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetPosErrAction(int nCon, int nAxis, ref int npMethod, ref int npAction, ref double dpPos);

    //지정 축의 범용 비교 카운터를 설정한다.
    //emCOMM  0    지령 위치 카운터
    //emFEED  1    실제 위치 카운터
    //emDEV   2    편차 카운터
    //emGEN   3    범용 카운터
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetGenSource(int nCon, int nAxis, int nCounter);

    //지정 축의 범용 비교 카운터를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetGenSource(int nCon, int nAxis, ref int npCounter);

    //지정 축의 범용 카운터 에러 발생를 설정한다.
    //dMethod
    //emMTH_NONE      0    성립하지 않음
    //emMTH_EQ_DIR    1    Counter 방향과 무관
    //emMTH_EQ_PDIR   2    Counter Up 중
    //emMTH_EQ_NDIR   3    Counter Down 중
    //dAction
    //emEVT_ONLY      0    처리 하지 않음
    //emEVT_ESTOP     1    즉시 정지
    //emEVT_STOP      2    감속 정지
    //emEVT_SPDCHG    3    속도 변경
    //dPos =-134,217,727 ~ +134,217,727 편차 카운터
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetGenAction(int nCon, int nAxis, int nMethod, int nAction, double dPos);

    //지정 축의 범용 비교 카운터 값를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetGenAction(int nCon, int nAxis, ref int npMethod, ref int npAction, ref double dpPos);

    //This function is select a auto speed change method to use when the comparator conditions are satisfied. 
    //emCOMM  0    지령 위치 카운터
    //emFEED  1    실제 위치 카운터
    //emDEV   2    편차 카운터
    //emGEN   3    범용 카운터
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetCmpModifySource(int nCon, int nAxis, int nCounter);

    //
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetCmpModifySource(int nCon, int nAxis, ref int npCounter);

    //nMethod
    //emMTH_NONE      0    성립하지 않음
    //emMTH_EQ_DIR    1    Counter 방향과 무관
    //emMTH_EQ_PDIR   2    Counter Up 중
    //emMTH_EQ_NDIR   3    Counter Down 중
    //dAction
    //emEVT_ONLY      0    처리 하지 않음
    //emEVT_ESTOP     1    즉시 정지
    //emEVT_STOP      2    감속 정지
    //emEVT_SPDCHG    3    속도 변경
    //dPos =-134,217,727 ~ +134,217,727 편차 카운터
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetCmpModifyAction(int nCon, int nAxis, int nMethod, int nAction, double dPos);

    //지정 축의 범용 비교 카운터 값를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetCmpModifyAction(int nCon, int nAxis, ref int npMethod, ref int npAction, ref double dpPos);

    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetCmpModifyVel(int nCon, int nAxis, double dVel);

    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetCmpModifyVel(int nCon, int nAxis, ref double dpVel);

    //========================================================================================================
    //                                  Overriding public static extern intS
    //========================================================================================================

    //지정 축의 모션 동작 중 설정한 이동 거리를 오버라이드 한다.
    //dPos = -134,217,728 <= new pos <= 134,217,727
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxModifyPos(int nCon, int nAxis, double dPos);

    //지정 축의 모션 동작 중 설정한 이동 거리를 오버라이드 한다.
    //dPos = -134,217,728 <= new pos <= 134,217,727
    [DllImport("pmiMApi.dll")]
    public static extern int pmiMxModifyPos(int nCon, int nNAxis, int[] naAxes, double[] daPos);

    //지정 축의 모션 동작 중 설정한 이송 속도를 오버라이드 한다.
    //dOvr = 1 ~ 6553500
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxModifyVel(int nCon, int nAxis, double dOvr);

    //지정 축의 모션 동작 중 설정한 이송 속도를 오버라이드 한다.
    //dOvr = 1 ~ 6553500
    [DllImport("pmiMApi.dll")]
    public static extern int pmiMxModifyVel(int nCon, int nNAxis, int[] naAxes, double[] daOvr);

    //지정 축의 모션 동작 중 설정한 이송 속도와 타입등을 설정한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxModifyVelProf(int nCon, int nAxis, double dVel, double dTacc, double dTdec);

    //========================================================================================================
    //                                  Coordinat Motion Control
    //========================================================================================================

    //보간 대상 축 그룹을 설정한다...
    //nCs       : 보간 인덱스 번호( 0 ~ 3 )
    //nNAxis    : 보간 축의 개수를 설정
    //naAxis    : 보간 할 축의 배열(nNAxis에서 설정한 개수보다 같거나 크게 선언해야됨)
    [DllImport("pmiMApi.dll")]
    public static extern int pmiCsSetAxis(int nCon, int nCs, int nNAxis, int[] naAxis);

    //보간 대상 축 시작 속도 및 종료 속도를  설정한다...
    //nCs       : 보간 인덱스 번호( 0 ~ 3 )
    //dVel      : 0 ~ 6553500
    [DllImport("pmiMApi.dll")]
    public static extern int pmiCsSetInitVel(int nCon, int nCs, double dVel);

    //보간 대상 축 시작 속도 및 종료 속도를  반환한다...
    //nCs In    : 보간 인덱스 번호( 0 ~ 3 )
    //dVel      : 0 ~ 6553500
    [DllImport("pmiMApi.dll")]
    public static extern int pmiCsGetInitVel(int nCon, int nCs, ref double dpVel);

    //보간 대상 축 이송 속도를 설정한다.
    //nCs       : 보간 인덱스 번호( 0 ~ 3 )
    //nOpType  = 0,  emCNS_VECTOR.    벡터 스피드
    //         = 1,  emCNS_MASTER.   마스터 스피드
    //nVelType = 0,  emCONST.        가감속없는 속도 프로파일
    //nVelType = 1,  emTCURVE.       Trapezode 속도 프로파일
    //nVelType = 2,  emSCURVE.       S Curve   속도 프로파일
    //dVel    : 0 ~ 6553500 작업 속도(pps) - 벡터 스피드 타입 일 때는 PPS 단위를 설정한다
    //          0 ~ 100 작업 속도 비율(%)  - 마스터 스피드 타입 일 때는 작업 속도 비율(%)을 설정한다.
    //dTacc   : 0 ~ 65000   가속도(msec)   - 벡터 스피드 타입 일 때는 PPS 단위를 설정한다
    //          0 ~ 100     가속도 비율(%) - 마스터 스피드 타입 일 때는 작업 속도 비율(%)을 설정한다.
    //dTdec   : 0 ~ 65000   감속도(msec)   - 벡터 스피드 타입 일 때는 PPS 단위를 설정한다
    //          0 ~ 100     감속도 비율(%) - 마스터 스피드 타입 일 때는 작업 속도 비율(%)을 설정한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiCsSetVelProf(int nCon, int nCs, int nOpType, int nType, double dVel, double dTacc, double dTdec);

    //보간 대상 축 이송 속도를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiCsGetVelProf(int nCon, int nCs, int nOpType, ref int npType, ref double dpVel, ref double dpTacc, ref double dpTdec);

    //직선 이송 보간를 실행 한다.
    //nCs       : 보간 인덱스 번호( 0 ~ 3 )
    //nAbsMode  : emINC   0    상대좌표
    //          : emABS   1    절대좌표
    //daPos     : 이송 거리 배열
    [DllImport("pmiMApi.dll")]
    public static extern int pmiCsLinMove(int nCon, int nCs, int nAbsMode, double[] daPos);

    [DllImport("pmiMApi.dll")]
    public static extern int pmiCsLinMoveEx(int nCon, int nCs, int nNAxis, int[] naAxis, int nAbsMode, double[] daPos);

    //원호 보간 이송(중심 좌표 와 종점 좌표)를 실행 한다.
    //nCs       : 보간 인덱스 번호( 0 ~ 3 )
    //nAbsMode  : emINC   0    상대좌표
    //          : emABS   1    절대좌표
    //daCen     : 중심 좌표 배열
    //daPos     : 종점 좌표 배열
    //nDir      : emARC_CW   0  - 시계방향(CW)으로 회전
    //            emARC_CCW  1  - 반시계방향(CCW)으로 회전
    [DllImport("pmiMApi.dll")]
    public static extern int pmiCsArcPMove(int nCon, int nCs, int nAbsMode, double[] daCen, double[] daPos, int nDir);

    [DllImport("pmiMApi.dll")]
    public static extern int pmiCsArcPMoveEx(int nCon, int nCs, int nNAxis, int[] naAxis, int nAbsMode, double[] daCen, double[] daPos, int nDir);


    //원호 보간 이송(중심 좌표 와 각도) 를 실행 한다.
    //nCs       : 보간 인덱스 번호( 0 ~ 3 )
    //nAbsMode  : emINC   0    상대좌표
    //          : emABS   1    절대좌표
    //daCen     : 중심 좌표 배열
    //dAngle    : 0 ~  360 각도의 부호(-)  - 시계방향(CW)으로 회전
    //          : 0 ~  360 각도의 부호(+)  - 반시계방향(CCW)으로 회전
    [DllImport("pmiMApi.dll")]
    public static extern int pmiCsArcAMove(int nCon, int nCs, int nAbsMode, double[] daCen, double dAngle);

    [DllImport("pmiMApi.dll")]
    public static extern int pmiCsArcAMoveEx(int nCon, int nCs, int nNAxis, int[] naAxis, int nAbsMode, double[] daCen, double dAngle);

    //헬리컬 보간.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiCsHelMove(int nCon, int nCs, double dCenX, double dCenY, double dPosX, double dPosY, double dPosZ, int nDir);

    [DllImport("pmiMApi.dll")]
    public static extern int pmiCsHelMoveEx(int nCon, int nCs, int nNAxis, int[] naAxis, double dCenX, double dCenY, double dPosX, double dPosY, double dPosZ, int nDir);

    //보간 이송 대해서 모션이 완료됐는지를 반환한다
    //nDone         :  emSTAND     0    이송 하지 않음
    //              :  emRUNNING   1    이송 중
    [DllImport("pmiMApi.dll")]
    public static extern int pmiCsCheckDone(int nCon, int nCs, ref int npDone);

    //보간 이송 감속 정지를 수행한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiCsStop(int nCon, int nCs);

    //보간 이송 급 정지를 수행한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiCsEStop(int nCon, int nCs);

    //========================================================================================================
    //                                  Continuous Motion
    //========================================================================================================

    //연속 보간 구동을 위해 저장된 내부 Queue를 모두 삭제하는 함수이다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiCsContClearQueue(int nCon);

    //지정된 좌표계에 연속보간에서 수행할 작업들의 등록을 시작한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiCsContBeginQueue(int nCon);

    //지정된 좌표계에서 연속보간을 수행할 작업들의 등록을 종료한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiCsContEndQueue(int nCon);

    //저장된 내부 연속 보간 Queue의 구동을 시작하는 함수이다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiCsContMove(int nCon);

    //저장된 내부 연속 보간 Queue의 구동을 정지하는 함수이다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiCsContStop(int nCon);

    //연속 보간 구동 중 현재 구동중인 연속 보간 인덱스 번호를 확인하는 함수이다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiCsContGetCurIndex(int nCon, int nLsi, ref int npIndex);

    //========================================================================================================
    //                                  Position Compare public static extern intS
    //========================================================================================================

    //Position Trigger 출력 신호 Active Level를 설정한다.
    //nCmp           : 0    비교기[0]
    //               : 1    비교기[1]
    //nLevel         : 0    emLOGIC_A   A접점(NORMAL OPEN) 및 Active Low Level Trigger
    //               : 1    emLOGIC_B   B접점(NORMAL CLOSE) 및 Active High Level Trigger
    [DllImport("pmiMApi.dll")]
    public static extern int pmiCmpSetLevel(int nCon, int nCmp, int nLevel);

    //Position Trigger 출력 신호 Active Level를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiCmpGetLevel(int nCon, int nCmp, ref int npLevel);

    //Position Trigger를 출력할 축를 설정한다.
    //nCmp           : 0    비교기[0]
    //               : 1    비교기[1]
    [DllImport("pmiMApi.dll")]
    public static extern int pmiCmpSetAxis(int nCon, int nCmp, int nAxis);

    //Position Trigger를 출력할 축를 반환한다.
    //nCmp           : 0    비교기[0]
    //               : 1    비교기[1]
    [DllImport("pmiMApi.dll")]
    public static extern int pmiCmpGetAxis(int nCon, int nCmp, ref int npAxis);

    //Position Trigger 출력 신호 펄스 폭를 설정한다.
    //nCmp           : 0    비교기[0]
    //               : 1    비교기[1]
    //nPul           : 1 ~ 50000(Pulses)
    [DllImport("pmiMApi.dll")]
    public static extern int pmiCmpSetHoldTime(int nCon, int nCmp, int nPulse);

    //Position Trigger 출력 신호 펄스 폭를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiCmpGetHoldTime(int nCon, int nCmp, ref int npPulse);

    //Position Trigger 출력 신호를 사용자 지정한 위치에서 한 개의 트리거 펄스를 출력한다.
    //nCmp           : 0    비교기[0]
    //               : 1    비교기[1]
    //dPos           :
    [DllImport("pmiMApi.dll")]
    public static extern int pmiCmpSetSinglePos(int nCon, int nCmp, int nMethod, double dPos);

    //Position Trigger 출력 신호를 사용자 지정한 위치 구간에서 트리거 펄스를 출력한다.
    //nCmp           : 0    비교기[0]
    //               : 1    비교기[1]
    //nMethod        : 0    emEQ_PDIR   - Counting up 중
    //               : 1    emEQ_NDIR   - Counting down 중
    //dNPos          :  -134217728 ~ +134217727    트리거 출력 시작 위치
    //dPPos          :  -134217728 ~ +134217727    트리거 출력 종료 위치
    [DllImport("pmiMApi.dll")]
    public static extern int pmiCmpSetRangePos(int nCon, int nCmp, int nMethod, double dNPos, double dPPos);

    //Position Trigger 출력 신호를 사용자 지정한 시작위치부터 종료위치까지 일정구간마다 트리거 출력을 설정한다.
    //nCmp           : 0    비교기[0]
    //               : 1    비교기[1]
    //nMethod        : 0    emEQ_PDIR   - Counting up 중
    //               : 1    emEQ_NDIR   - Counting down 중
    //nNum           : 0 ~ 1024                    - 출력할 갯수
    //dSPos          :  -134217728 ~ +134217727    - 트리거 출력 시작 위치
    //dDist          : 0 ~ +134217727              - 트리거 출력 주기 간격
    [DllImport("pmiMApi.dll")]
    public static extern int pmiCmpSetMultPos(int nCon, int nCmp, int nMethod, int nNum, double dSPos, double dDist);

    //Position Trigger 출력 신호를 사용자 지정한 시작위치부터 종료위치까지 일정구간마다 트리거 출력을 설정한다.
    //nCmp           : 0    비교기[0]
    //               : 1    비교기[1]
    //nMethod        : 0    emEQ_PDIR  - Counting up 중
    //               : 1    emEQ_NDIR  - Counting down 중
    //nNum           : 0 ~ 1024        - 출력할 갯수(배열 갯수)
    //daPos          :                 -트리거 출력 위치배열(nNum 설정한 개수보다 같거나 크게 선언해야됨)
    [DllImport("pmiMApi.dll")]
    public static extern int pmiCmpSetPosTable(int nCon, int nCmp, int nMethod, int nNum, double[] daPos);

    //Position Trigger 출력 신호 트리거를 시작한다.
    //nCmp           : 0   비교기[0]
    //               : 1    비교기[1]
    [DllImport("pmiMApi.dll")]
    public static extern int pmiCmpBegin(int nCon, int nCmp);

    //Position Trigger 출력 신호 트리거를 해체한다.
    //nCmp           : 0    비교기[0]
    //               : 1    비교기[1]
    [DllImport("pmiMApi.dll")]
    public static extern int pmiCmpEnd(int nCon, int nCmp);

    //Position Compare Trigger 신호 출력 발생 할 위치값을 반환한다.
    //nCmp           : 0    비교기[0]
    //               : 1    비교기[1]
    [DllImport("pmiMApi.dll")]
    public static extern int pmiCmpGetPos(int nCon, int nCmp, ref int npNum, ref double dpPos);

    //========================================================================================================
    //                                  Master/Slave Motion Control
    //========================================================================================================

    //동기 제어 마스터축을 설정한다.
    //int nMAxisNo   :   마스터축 번호( 0 ~ [최대 축개수 - 1] )
    [DllImport("pmiMApi.dll")]
    public static extern int pmiSyncSetMaster(int nCon, int nMAxis);

    //동기 제어 마스터축을 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiSyncGetMaster(int nCon, ref int npMAxis);

    //동기 제어 오차 검출시 동기 오차 알람 발생 여부을 설정한다.
    //int nSAxis   : 슬레이브축 번호( 0 ~ [최대 축개수 - 1] )
    //nAction    : 0 emNOTUSED    - 동기 오차 알람 발생하지 않음
    //             1 emUSED      -  동기 오차 알람 발생
    [DllImport("pmiMApi.dll")]
    public static extern int pmiSyncSetAction(int nCon, int nSAxis, int nAction);

    //동기 제어 오차 검출시 동기 오차 알람를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiSyncGetAction(int nCon, int nSAxis, ref int npAction);

    //동기 제어 오차 검출시 동기 오차 알람 발생 여부을 설정한다.
    //int nSAxis   : 슬레이브축 번호( 0 ~ [최대 축개수 - 1] )
    //dLimit     : 1 ~ 134217727   - 마스터 축과 슬레이브 사이의 제어 편차 허용량
    [DllImport("pmiMApi.dll")]
    public static extern int pmiSyncSetPosErrorLimit(int nCon, int nSAxis, double dLimit);

    //동기 제어 오차 검출시 동기 오차 알람를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiSyncGetPosErrorLimit(int nCon, int nSAxis, ref double dpLimit);

    //동기 제어 현재 오차 값, 최대 오차값을 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiSyncGetPosError(int nCon, int nSAxis, ref double dpError, ref double dpMaxError);

    //Position Trigger 출력 신호 트리거를 시작한다.
    //int nSAxis   : 슬레이브축 번호( 0 ~ [최대 축개수 - 1] )
    [DllImport("pmiMApi.dll")]
    public static extern int pmiSyncBegin(int nCon, int nSAxis);

    //지정 축에 대하여 마스터 축과 동기를 해체 시킨다.
    //int nSAxis   : 슬레이브축 번호( 0 ~ [최대 축개수 - 1] )
    [DllImport("pmiMApi.dll")]
    public static extern int pmiSyncEnd(int nCon, int nSAxis);

    //========================================================================================================
    //                                  Gantry Motion Control
    //========================================================================================================

    //겐트리(Gantry) 제어 마스터축을 설정한다.
    //int nMAxisNo   :   마스터축 번호( 0 ~ [최대 축개수 - 1] )
    [DllImport("pmiMApi.dll")]
    public static extern int pmiGantSetMaster(int nCon, int nId, int nMAxis);

    //겐트리(Gantry) 제어 마스터축을 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiGantGetMaster(int nCon, int nId, ref int npMAxis);

    //겐트리(Gantry) 제어 오차 검출시 동기 오차 알람을 설정한다.
    //int nSAxis   : 슬레이브축 번호( 1,3,5,7 )
    //nAction    : 0  emNOTUSED   동기 오차 알람 발생하지 않음
    //             1  emUSED      동기 오차 알람 발생
    [DllImport("pmiMApi.dll")]
    public static extern int pmiGantSetAction(int nCon, int nId, int nSAxis, int nAction);

    //겐트리(Gantry) 제어 오차 검출시 동기 오차 알람를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiGantGetAction(int nCon, int nId, int nSAxis, ref int npAction);

    //겐트리(Gantry) 제어 마스터 축과 슬레이브 축 사이의 제어 편차의 허용량를 설정한다.
    //int nSAxis   : 슬레이브축 번호( 1,3,5,7 )
    //dLimit     : 1 ~ 134217727   - 마스터 축과 슬레이브 사이의 제어 편차 허용량
    [DllImport("pmiMApi.dll")]
    public static extern int pmiGantSetPosErrorLimit(int nCon, int nId, int nSAxis, double dLimit);

    //겐트리(Gantry) 제어 마스터 축과 슬레이브 축 사이의 제어 편차 허용량 설정 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiGantGetPosErrorLimit(int nCon, int nId, int nSAxis, ref double dpLimit);

    //겐트리(Gantry) 제어 현재 오차 값, 최대 오차값을 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiGantGetPosError(int nCon, int nId, int nSAxis, ref double dpError, ref double dpMaxError);

    //겐트리(Gantry) 제어 마스터 축과 슬레이브 축을 연결 시킨다.
    //int nSAxis   : 슬레이브축 번호( 1,3,5,7 )
    [DllImport("pmiMApi.dll")]
    public static extern int pmiGantBegin(int nCon, int nId, int nSAxis);

    //겐트리(Gantry) 제어 마스터 축과 슬레이브 축 연결을 해제 시킨다.
    //int nSAxis   : 슬레이브축 번호( 1,3,5,7 )
    [DllImport("pmiMApi.dll")]
    public static extern int pmiGantEnd(int nCon, int nId, int nSAxis);

    //========================================================================================================
    //                                   Manual Pulsar public static extern intS
    //========================================================================================================

    //PA/PB(MPG) input signal mode
    //0x00 1X A/B
    //0x01 2X A/B
    //0x02 4X A/B
    //0x03 CW/CCW(Default)
    [DllImport("pmiMApi.dll")]
    public static extern int pmiMpgSetInType(int nCon, int nAxis, int nDir);

    //PA/PB(MPG) input signal mode
    [DllImport("pmiMApi.dll")]
    public static extern int pmiMpgGetInType(int nCon, int nAxis, ref int npDir);

    //지정 축에서 MPG(Manual Pulsar)  펄스 입력 방향을 설정한다.
    //nDir        : 0   emNORMAL    정방향
    //            : 1   emRESERVE   역방향
    [DllImport("pmiMApi.dll")]
    public static extern int pmiMpgSetDir(int nCon, int nAxis, int nDir);

    //지정 축에서 MPG(Manual Pulsar)  펄스 입력 방향을 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiMpgGetDir(int nCon, int nAxis, ref int npDir);

    //지정 축에서 MPG(Manual Pulsar)  펄스 기어비를 설정한다.
    //nMultiFactor        :  1 ~ 32      1차 출력펄스를 1 ~ 32 배수의 펄스를 재 생성
    //nDivFactor          :  1 ~ 2048    2차 출력펄스에 (nDivFactor/2048)가 곱해져서 최종 출력펄스 생성
    [DllImport("pmiMApi.dll")]
    public static extern int pmiMpgSetGain(int nCon, int nAxis, int nMultiFactor, int nDivFactor);

    //지정 축에서 MPG(Manual Pulsar)  펄스 기어비를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiMpgGetGain(int nCon, int nAxis, ref int npMultiFactor, ref int npDivFactor);

    //지정 축에서 MPG(Manual Pulsar)  펄스 입력 작업을 수행한다..
    [DllImport("pmiMApi.dll")]
    public static extern int pmiMpgBegin(int nCon, int nAxis);

    //지정 축에서 MPG(Manual Pulsar)  펄스 입력 작업을 해체한다..
    [DllImport("pmiMApi.dll")]
    public static extern int pmiMpgEnd(int nCon, int nAxis);

    //========================================================================================================
    //                                   Position LATCH public static extern intS
    //========================================================================================================

    //지정 축의 Latch 카운터 신호 Active Level 설정한다.
    //nLevel   :  0   emLOGIC_A     A접점(NORMAL OPEN)
    //         :  1   emLOGIC_B     B접점(NORMAL CLOSE)
    [DllImport("pmiMApi.dll")]
    public static extern int pmiLtcSetLevel(int nCon, int nAxis, int nLevel);

    //지정 축의 Latch 카운터 신호 Active Level 설정을 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiLtcGetLevel(int nCon, int nAxis, ref int npLevel);

    //Counter = 0 , Command counter
    //Counter = 1 , Feedback counter
    //Counter = 2 , Error Counter 
    //Counter = 3 , General Counter
    //Counter = 4 , Command Speed
    [DllImport("pmiMApi.dll")]
    public static extern int pmiLtcGetPos(int nCon, int nAxis, int nCount, ref double dpPos);

    //지정 축의 Latch 카운터 활성화를 수행한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiLtcSetEnable(int nCon, int nAxis, int nEnable);

    //지정 축의 Latch 카운터 활성화를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiLtcGetEnable(int nCon, int nAxis, ref int npEnable);

    //========================================================================================================
    //                                  Interrupt public static extern intS
    //========================================================================================================
    [DllImport("pmiMApi.dll")]
    public static extern int pmiIntSetHandler(int nCon, int nType, uint hWnd, ref int hHandler, uint nMsg);

    //
    [DllImport("pmiMApi.dll")]
    public static extern int pmiIntSetHandlerEnable(int nCon, int nEnable);

    //-------- Axis--------------------------------------------------------------------------------------
    //
    [DllImport("pmiMApi.dll")]
    public static extern int pmiIntSetAxisEnable(int nCon, int nAxis, uint nMask);

    //
    [DllImport("pmiMApi.dll")]
    public static extern int pmiIntGetAxisEnable(int nCon, int nAxis, ref uint npMask);

    //BIT0	; 자동 정지때
    //BIT1	; 다음 동작 계속 START 때
    //BIT2	; 동작용 2nd pre register 기입 가능 때
    //BIT3	; Comparator 5용 2nd pre register 기입 가능 때
    //BIT4	; 가속 개시 때
    //BIT5	; 가속 종료 때
    //BIT6	; 감속 개시 때
    //BIT7	; 감속 종료 때
    //BIT8	; Compatator1 조건 성립 때
    //BIT9	; Compatator2 조건 성립 때
    //BIT10	; Compatator3 조건 성립 때
    //BIT11	; Compatator4 조건 성립 때
    //BIT12	; Compatator6 조건 성립 때
    //BIT13	; CLR 신호 입력에 의해 COUNTER 값 RESET 때
    //BIT14	; LTC 신호 입력에 의해 COUNTER 값 LATCH 때
    //BIT15	; ORG 신호 입력에 의해 COUNTER 값 LATCH 때
    //BIT16	; SD 입력 ON 때
    //BIT17	; +DR 입력 변화 때
    //BIT18	; -DR 입력 변화 때
    //BIT19	; /STA 입력 ON 때

    //지정 축의 상태를 반환합니다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiIntGetAxisStatus(int nCon, int nAxis, ref uint npStatus);

    //BIT0 STOP_BY_SLP:   1;	양의 소프트 리미트에 의해 정지
    //BIT1 STOP_BY_SLN:   1;	음의 소프트 리미트에 의해 정지
    //BIT2 STOP_BY_CMP3:  1;	비교기3에 의해 정지
    //BIT3 STOP_BY_CMP4:  1;	비교기4에 의해 정지
    //BIT4 STOP_BY_CMP5:  1;	비교기5에 의해 정지
    //BIT5 STOP_BY_ELP:   1;	+EL 에 의해 정지
    //BIT6 STOP_BY_ELN:   1;	-EL 에 의해 정지
    //BIT7 STOP_BY_ALM:   1;	알람에 의해 정지
    //BIT8 STOP_BY_STP:   1;	CSTP에 의해 정지
    //BIT9 STOP_BY_EMG:   1;	EMG에 의해 정지
    //BIT10 STOP_BY_SD:   1;	SD 입력에 의해 정지
    //BIT11 STOP_BY_DT:   1;	보간 동작 DATA 이상에 의해 정지
    //BIT12 STOP_BY_IP:   1;	보간 동작 중에 타 축의 이상 정지에 의해 동시 정지
    //BIT13 STOP_BY_PO:   1;	PA/PB 입력용 buffer counter dml overflow 에 의해 정지
    //BIT14 STOP_BY_AO:   1;	보간 동작 때의 위치 범위를 벗어나서 정지
    //BIT15	STOP_BY_EE:   1;	EA/EB 입력 에러 발생 (정지 하지 않음)
    //BIT16	STOP_BY_PE:   1;	PA/PB 입력 에러 발생 (정지 하지 않음)

    //지정 축의 Error 상태를 반환합니다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiIntGetAxisErrStatus(int nCon, int nAxis, ref uint npStatus);

    //-------- DI----------------------------------------------------------------------------------------

    //0 nId => 0  ~ 31
    //1 nId => 32 ~ 63
    [DllImport("pmiMApi.dll")]
    public static extern int pmiIntSetDiEnable(int nCon, int nId, uint nMask);

    //0 nId => 0  ~ 31
    //1 nId => 32 ~ 63
    [DllImport("pmiMApi.dll")]
    public static extern int pmiIntGetDiStatus(int nCon, int nId, ref uint npStatus);


    //========================================================================================================
    //                                   Digital In/Out public static extern intS
    //========================================================================================================

    //디지털 입력 디바이스로부터 32개 채널의 상태를 32비트값으로 반환한다.
    //nId           : 0   - CH0 ~ CH31
    //              : 1   - CH32 ~ CH63
    [DllImport("pmiMApi.dll")]
    public static extern int pmiDiGetData(int nCon, int nId, ref uint npData);

    //디지털 입력의 한 채널당 ON/OFF 상태를 반환한다.
    //nBit         : 0  ~ 31  - CH0 ~ CH31
    //             : 32 ~ 63  - CH32 ~ CH63
    [DllImport("pmiMApi.dll")]
    public static extern int pmiDiGetBit(int nCon, int nBit, ref int npData);

    //디지털 출력 디바이스로부터 32개 채널의 상태를 32비트값으로 설정한다.
    //nId           : 0   - CH0  ~ CH31
    //              : 1   - CH32 ~ CH63
    [DllImport("pmiMApi.dll")]
    public static extern int pmiDoSetData(int nCon, int nId, uint nData);

    //디지털 출력 디바이스로부터 32개 채널의 상태를 32비트값으로 반환한다.
    //nId           : 0   - CH0 ~ CH31
    //              : 1   - CH32 ~ CH63
    [DllImport("pmiMApi.dll")]
    public static extern int pmiDoGetData(int nCon, int nId, ref uint npData);


    //디지털 출력의 한 채널당 ON/OFF 상태를 설정한다.
    //nBit         : 0  ~ 31  - CH0 ~ CH31
    //             : 32 ~ 63  - CH32 ~ CH63
    [DllImport("pmiMApi.dll")]
    public static extern int pmiDoSetBit(int nCon, int nBit, int nData);

    //디지털 출력의 한 채널당 ON/OFF 상태를 반환한다.
    //nBit         : 0  ~ 31  - CH0  ~ CH31
    //             : 32 ~ 63  - CH32 ~ CH63
    [DllImport("pmiMApi.dll")]
    public static extern int pmiDoGetBit(int nCon, int nBit, ref int npData);

    //디지털 입력의 한 그룹당 필터 사용유무를 설정한다.
    //nId           :  0    - CH0  ~ CH15
    //              :  1    - CH16 ~ CH31
    //              :  2    - CH32 ~ CH47
    //              :  3    - CH48 ~ CH63
    [DllImport("pmiMApi.dll")]
    public static extern int pmiDiSetFilterEnable(int nCon, int nId, int nEnable);

    //디지털 입력의 한 그룹당 필터 사용유무를 반환한다.
    //nId           :  0    - CH0  ~ CH15
    //              :  1    - CH16 ~ CH31
    //              :  2    - CH32 ~ CH47
    //              :  3    - CH48 ~ CH63
    [DllImport("pmiMApi.dll")]
    public static extern int pmiDiGetFilterEnable(int nCon, int nId, ref int npEnable);

    //디지털 입력의 한 그룹당 필터 시간를 설정한다.
    //nId           :  0    - CH0  ~ CH15
    //              :  1    - CH16 ~ CH31
    //              :  2    - CH32 ~ CH47
    //              :  3    - CH48 ~ CH63
    //nTime         :  0x00 ~ 0x0F  - 필터 시간
    [DllImport("pmiMApi.dll")]
    public static extern int pmiDiSetFilterTime(int nCon, int nId, int nTime);

    //디지털 입력의 한 그룹당 필터 시간를 반환한다.
    //nId           :  0    - CH0 ~ CH15
    //              :  1    - CH16 ~ CH31
    //              :  2    - CH32 ~ CH47
    //              :  3    - CH48 ~ CH63
    [DllImport("pmiMApi.dll")]
    public static extern int pmiDiGetFilterTime(int nCon, int nId, ref int npTime);

    //========================================================================================================
    //                                   Motion Correction Control
    //========================================================================================================

    //백래쉬(Backlash) 또는 슬립(Slip)에 대한 보정을 설정한다.
    //nCorrMode        : emCORR_DIS  0    보정 기능을 비활성
    //                 : emCORR_BACK 1    보정모드를 백래쉬 보정 모드
    //                 : emCORR_SLIP 2    보정모드를 슬립 보정 모드
    //dCorrPos         : 0 ~ 4095            보정 펄스의 수
    //dCorrVel         : 0 ~ 6553500         보정펄스의 출력 주파수
    //nCtrMask         : BIT0    1           - 보정펄스 출력시에 지령위치 Counter가 동작
    //                 : BIT1    1           - 보정펄스 출력시에 피드백위치 Counter가 동작
    //                 : BIT2    1           - 보정펄스 출력시에 편차 Counter가 동작
    //                 : BIT3    1           - 보정펄스 출력시에 범용 Counter가 동작
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetBacklashComps(int nCon, int nAxis, int nCorrMode, double dCorrPos, double dCorrVel, int nCtrMask);

    //백래쉬(Backlash) 또는 슬립(Slip)에 대한 보정 설정을 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetBacklashComps(int nCon, int nAxis, ref int npCorrMode, ref double dpCorrPos, ref double dpCorrVel, ref int npCtrMask);

    //동작 완료 직후에 진동 제어를 설정한다.
    //dRevTime     :  0 ~ 65535   반전 동작 시간 설정
    //dForTime     :  0 ~ 65535   정전 동작 시간 설정
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetSuppressVibration(int nCon, int nAxis, double dRevTime, double dForTime);

    //동작 완료 직후에 진동 제어를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetSuppressVibration(int nCon, int nAxis, ref double dpRevTime, ref double dpForTime);

    //====================== DEBUG-LOGGING FUNCTIONS ==============================================//
    //Log 파일을 저장 합니다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiDLogSetFile(string szFilename);

    //Log 파일을 읽어 옵니다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiDLogGetFile(ref string szFilename);

    //Log 저장 Level을 설정합니다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiDLogSetLevel(int nLevel);

    //Log 저장 Level을 반환합니다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiDLogGetLevel(ref int npLevel);

    //Log 저장 활성화를 설정합니다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiDLogSetEnable(int nEnable);

    //Log 저장 활성화를 반환합니다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiDLogGetEnable(ref int npEnable);

    //Error 코드를 반환합니다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiErrGetCode(int nCon, ref int npCode);

    //Error 코드의 내용을 반환합니다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiErrGetString(int nCon, int nCode, ref string szpStr);

    //지정 축의 Error 코드를 반환합니다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiErrAxGetCode(int nCon, int nAxis, ref int npCode);

    //========================================================================================================
    //                                   GENERAL public static extern intS
    //========================================================================================================

    //시뮬레이션 모드 활성화 여부를 설정한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxSetSimulEnable(int nCon, int nAxis, int nEnable);

    //시뮬레이션 모드 활성화 여부를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiAxGetSimulEnable(int nCon, int nAxis, ref int npEnable);

    //+el.-el.sd.org. alm.inp
    //0 <= nTime <= 15
    /*설정 입력 신호의 레벨	신호 지연 시간
	CLK=16MHz時）	설정 입력 신호의 레벨	신호 지연 시간（CLK=16MHz時）
	PALF3	PALF2	PALF1	PALF0				PALF3	PALF2	PALF1	PALF0	
	Low		Low		Low		Low		1 μsec	    Hi		Low		Low		Low		0.256 msec
	Low		Low		Low		Hi		2 μsec	    Hi		Low		Low		Hi		0.512 msec
	Low		Low		Hi		Low		4 μsec		Hi		Low		Hi		Low		1.02 msec(default)
	Low		Low		Hi		Hi		8 μsec		Hi		Low		Hi		Hi		2.05 msec
	Low		Hi		Low		Low		16 μsec	Hi		Hi		Low		Low		4.10 msec
	Low		Hi		Low		Hi		32 μsec	Hi		Hi		Low		Hi		8.19 msec
	Low		Hi		Hi		Low		64 μsec	Hi		Hi		Hi		Low		16.4 msec
	Low		Hi		Hi		Hi		128 μsec	Hi		Hi		Hi		Hi		32.8 msec
    */
    [DllImport("pmiMApi.dll")]
    public static extern int pmiGnSetFilterTime(int nCon, int nTime);
    [DllImport("pmiMApi.dll")]
    public static extern int pmiGnGetFilterTime(int nCon, ref int npTime);

    //비상 정지 신호 Active 레벨를 설정한다..
    [DllImport("pmiMApi.dll")]
    public static extern int pmiGnSetEmgLevel(int nCon, int nLevel);

    //비상 정지 신호 Active 레벨를 반환한다..
    [DllImport("pmiMApi.dll")]
    public static extern int pmiGnGetEmgLevel(int nCon, ref int npLevel);

    //모든 모션 동작을 감속 없이 긴급 정지하는 함수이다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiGnSetEStop(int nCon);

    //제어기 축수를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiGnGetAxesNum(int nCon, ref int npNAxesNum);

    //디지털 입출력 갯수를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiGnGetDioNum(int nCon, ref int npNDiChNum, ref int npNDoChNum);

    //피드백 속도 확인 및 주기 설정한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiGnSetCheckActVel(int nEnable, int nInterval);

    //피드백 속도 확인 및 주기 반환한다..
    [DllImport("pmiMApi.dll")]
    public static extern int pmiGnGetCheckActVel(ref int npEnable, ref int npInterval);

    //해당 Controller의 베이스 모델명을 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiConGetModel(int nCon, ref int npModel);

    //특주 Controller의 베이스 모델명을 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiConGetModelEx(int nCon, ref int npModel);

    //해당 Controller의 Firmware 버전를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiConGetFwVersion(int nCon, ref int npVer);

    //해당 Controller의 Hardware 버전를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiConGetHwVersion(int nCon, ref int npVer);

    //MAPI 버전를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiConGetMApiVersion(int nCon, ref int npVer);

    //보드의 LED ON/OFF 합니다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiConSetCheckOn(int nCon, int nOn);

    //보드의 LED 상태를 반환한다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiConGetCheckOn(int nCon, ref int npOn);

    //레지스터 상태를 읽어 옵니다.
    [DllImport("pmiMApi.dll")]
    public static extern int pmiGnGetRegister(int nCon, int nAxis, int nRNo, ref int npValue);

    //
    [DllImport("pmiMApi.dll")]
    public static extern int pmiGnGetString(int nCon, int nAxis, int nStrID, ref string szpBuffer);

    //동시 시작 H/W 
    [DllImport("pmiMApi.dll")]
    public static extern int pmiGnSetStaSignal(int nCon, int nEnable);

    //동시 시작 H/W 
    [DllImport("pmiMApi.dll")]
    public static extern int pmiGnGetStaSignal(int nCon, ref int npEnable);

    //동시 정지 H/W
    [DllImport("pmiMApi.dll")]
    public static extern int pmiGnSetStpSignal(int nCon, int nEnable);

    //동시 정지 H/W
    [DllImport("pmiMApi.dll")]
    public static extern int pmiGnGetStpSignal(int nCon, ref int npEnable);

    //========================================================================================================
    //                                   Motion-NET FUNCTIONS
    //========================================================================================================

    //통신 초기화
    [DllImport("pmiMApi.dll")]
    public static extern int pmiNetSysComm(int nCon);

    [DllImport("pmiMApi.dll")]
    public static extern int pmiNetReset(int nCon);

    //사이클릭 통신
    [DllImport("pmiMApi.dll")]
    public static extern int pmiNetCyclicBegin(int nCon);

    [DllImport("pmiMApi.dll")]
    public static extern int pmiNetCyclicEnd(int nCon);

    //통신 속도
    // 0 : 2.5 Mbps
    // 1 : 5 Mbps
    // 2 : 10 Mbps
    // 3 : 20 Mbps
    [DllImport("pmiMApi.dll")]
    public static extern int pmiNetSetCommSpeed(int nCon, int nCommSpeed);

    [DllImport("pmiMApi.dll")]
    public static extern int pmiNetGetCommSpeed(int nCon, ref int npCommSpeed);

    //통신 Digital In/Out FUNCTIONS
    [DllImport("pmiMApi.dll")]
    public static extern int pmiNetDiGetData(int nCon, int nStNo, ref uint npData);

    [DllImport("pmiMApi.dll")]
    public static extern int pmiNetDiGetBit(int nCon, int nStNo, int nBit, ref uint npData);

    [DllImport("pmiMApi.dll")]
    public static extern int pmiNetDoSetData(int nCon, int nStNo, uint nData);

    [DllImport("pmiMApi.dll")]
    public static extern int pmiNetDoGetData(int nCon, int nStNo, ref uint npData);

    [DllImport("pmiMApi.dll")]
    public static extern int pmiNetDoSetBit(int nCon, int nStNo, int nBit, uint nData);

    [DllImport("pmiMApi.dll")]
    public static extern int pmiNetDoGetBit(int nCon, int nStNo, int nBit, ref uint npData);

    //통신 에러 상태
    [DllImport("pmiMApi.dll")]
    public static extern int pmiNetGetCommErrNum(int nCon, ref int npErrNum);

    [DllImport("pmiMApi.dll")]
    public static extern int pmiNetCommErrNumClear(int nCon);

    [DllImport("pmiMApi.dll")]
    public static extern int pmiNetGetCyclicErrFlag(int nCon, int nStNo, ref int npFlag);

    [DllImport("pmiMApi.dll")]
    public static extern int pmiNetCyclicErrFlagClear(int nCon, int nStNo);

    // 사이클릭 통신 에러 상태
    // 1 || 0  비트
    // 0    0  통신 종료
    // 0    1  사이클릭 통신 중
    // 1    0  사이클릭 통신 종료. 에러 해제 안한 상태
    // 1    1  사이클릭 통신 중 에러 발생
    [DllImport("pmiMApi.dll")]
    public static extern int pmiNetGetCyclicStatus(int nCon, ref uint npStatus);

    [DllImport("pmiMApi.dll")]
    public static extern int pmiNetGetCyclicSpeed(int nCon, ref int npTime);

    [DllImport("pmiMApi.dll")]
    public static extern int pmiNetGetSlaveTotal(int nCon, ref int npSlvNum);

    //디바이스 정보
    // 2 || 1 || 0  비트
    // 0    0    0  : 32점 출력 전용
    // 0    1    0  : 16점 출력 16점 입력 
    // 1    1    1  : 32점 입력 전용
    // 3            비트
    // 0            : I/O 전용
    // 1            : Motion 전용
    // 4            비트
    // 0            : 사용 안함
    // 1            : 사용
    [DllImport("pmiMApi.dll")]
    public static extern int pmiNetGetSlaveInfo(int nCon, int nStNo, ref uint npStatus);

    [DllImport("pmiMApi.dll")]
    public static extern int pmiNetGetSlaveDioNum(int nCon, int nStNo, ref int npDiNum, ref int npDoNum);
}