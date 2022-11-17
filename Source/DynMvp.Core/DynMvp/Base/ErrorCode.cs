using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Base
{
    public enum ErrorSection
    {
        Initialize = 0,
        Release = 100,
        Environment = 200,      // File Load

        Safety = 300,
        Interlocked = 400,
        Motion = 600,
        Grabber = 700,
        DigitalIo = 800,
        DAQ = 900,
        Light = 1000,
        Machine = 1100,

        Teach = 1500,
        Inspect = 1600,
        Process = 1700,

        ExternalIF = 2000,

        NextErrorSection = 5000
    }

    public enum ErrorSubSection
    {
        CommonReason = 0,
        SpecificReason = 50
    }

    public enum CommonError
    {
        InvalidType = ErrorSubSection.CommonReason,
        InvalidState,
        InvalidSetting,
        FailToCreate,
        FailToInitialize,
        FailToRelease,
        FailToReadValue,
        FailToReadParamFile,
        FailToReadParam,
        FailToWriteValue,
        FailToWriteParam,
        FailToExecute
    }

    public enum ErrorLevel
    {
        Fatal,  // 프로그램의 재 시동이 필요한 오류
        Error,  // 현재의 동작을 정지해야 할 오류
        Warning // 비 정상적인 시점에 명령이 호출되는 등의 오류. 동작의 사전 점검 등에서 발생
    }

    public enum SafetyError
    {
        DoorOpen = ErrorSubSection.CommonReason,
        EmergencySwitch,
        AreaSensor,
        ServoOff,
    }

    public enum MachineError
    {
        CylinderInjection,
        CylinderEjection,
        ConveyorMovingTimeOut,
        CantDetectPart,
        InitTimeOut,
        AirPressure,
        VaccumOn,
        Light,
        Serial
    }

    public enum ExternalIfError
    {
        CommunicationTimeout = ErrorSubSection.CommonReason,
        InvalidRemoteIp
    }

    public enum InspectError
    {
        DefectOccrued = ErrorSubSection.CommonReason,
        FiducialError,
        FiducialLengthError
    }

    //public enum ErrorCode
    //{
    //    InitMotion = 0,
    //    InitDIO,
    //    InitGrabber,
    //    InitCamera,
    //    InitSerialPort,
    //    InitDaq,
    //    InitDepthScanner,

    //    ReleaseMotion = 100,
    //    ReleaseDIO,
    //    ReleaseGrabber,
    //    ReleaseCamera,
    //    ReleaseSerialPort,
    //    ReleaseDaq,
    //    ReleaseDepthScanner,

    //    // Data
    //    LoadConfigFileError = 200,
    //    LoadCameraCalibFileError,
    //    LoadRobotCalibFileError,
    //    LoadModelFileError,

    //    // Motion
    //    HomeMoving = 600,
    //    Moving,
    //    AxisPosLimit,
    //    AxisNegLimit,
    //    AxisAmpFault,

    //    // Grabber
    //    GrabTimeout = 700,

    //    // DigitalIO
    //    DigitalIo_InvalidType = ErrorSection.DigitalIo,
    //    DigitalIo_Instance,

    //    // Teach
    //    Teach,

    //    // External I/F
    //    CommunicationTimeout = 1000,
    //}
}
