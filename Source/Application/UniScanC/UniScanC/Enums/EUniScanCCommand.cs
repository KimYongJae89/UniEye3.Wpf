namespace UniScanC.Enums
{
    public enum EUniScanCCommand
    {
        Unknown,                        // 에러, 불분명

        GetState,                       // 상태를 주고 받기 위한 명령 (안쓰임)
        SetTime,                        // CM과 IM 시간을 맞춰주기 위한 명령

        OpenModel,                      // 모델 열기
        CloseModel,                     // 모델 닫기

        EnterWait,                      // 검사 대기 상태로 전환. LOT_NO
        ExitWait,                       // 검사 대기 상태를 해제
        SkipMode,                       // 검사 스킵 모드로 전환 (안쓰임)

        LightCalibrationStart,          // CM : 조명 캘리브레이션 시작 -> IM : ACK
        LightCalibrationTopGrab,        // CM : 상부 조명 변경 후 Grab 지시 -> IM : 변경된 조명 값 계산
        LightCalibrationBottomGrab,     // CM : 하부 조명 변경 후 Grab 지시 -> IM : 변경된 조명 값 계산
        LightCalibrationFinish,         // CM : 조명 캘리브레이션 종료 -> IM : 조명 캘리브레이션 값 파라메터에 ACK

        TeachGrab,                      // 티칭 : 이미지 그랩
        TeachInspect,                   // 티칭 : 검사 완료

        Alarm,                          // 오류 상황
    }
}
