using System;
using System.Runtime.InteropServices;

namespace Euresys
{
    internal class MultiCamException : System.Exception
    {
        public MultiCamException(string error) : base(error) { }
    }

    namespace MultiCam
    {
        /// <summary>
        /// Class to expose the MultiCam C API in .NET
        /// </summary>
        public sealed class MC
        {
            /// <summary>
            /// Native functions imported from the MultiCam C API.
            /// </summary>
            #region Native Methods
            private class NativeMethods
            {
                private NativeMethods() { }

                [DllImport("MultiCam.dll")]
                internal static extern int McOpenDriver(IntPtr instanceName);
                [DllImport("MultiCam.dll")]
                internal static extern int McCloseDriver();
                [DllImport("MultiCam.dll")]
                internal static extern int McCreate(uint modelInstance, out uint instance);
                [DllImport("MultiCam.dll")]
                internal static extern int McCreateNm(string modelName, out uint instance);
                [DllImport("MultiCam.dll")]
                internal static extern int McDelete(uint instance);
                [DllImport("MultiCam.dll")]
                internal static extern int McSetParamInt(uint instance, uint parameterId, int value);
                [DllImport("MultiCam.dll")]
                internal static extern int McSetParamNmInt(uint instance, string parameterName, int value);
                [DllImport("MultiCam.dll")]
                internal static extern int McSetParamStr(uint instance, uint parameterId, string value);
                [DllImport("MultiCam.dll")]
                internal static extern int McSetParamNmStr(uint instance, string parameterName, string value);
                [DllImport("MultiCam.dll")]
                internal static extern int McSetParamFloat(uint instance, uint parameterId, double value);
                [DllImport("MultiCam.dll")]
                internal static extern int McSetParamNmFloat(uint instance, string parameterName, double value);
                [DllImport("MultiCam.dll")]
                internal static extern int McSetParamInst(uint instance, uint parameterId, uint value);
                [DllImport("MultiCam.dll")]
                internal static extern int McSetParamNmInst(uint instance, string parameterName, uint value);
                [DllImport("MultiCam.dll")]
                internal static extern int McSetParamPtr(uint instance, uint parameterId, IntPtr value);
                [DllImport("MultiCam.dll")]
                internal static extern int McSetParamNmPtr(uint instance, string parameterName, IntPtr value);
                [DllImport("MultiCam.dll")]
                internal static extern int McSetParamInt64(uint instance, uint parameterId, long value);
                [DllImport("MultiCam.dll")]
                internal static extern int McSetParamNmInt64(uint instance, string parameterName, long value);
                [DllImport("MultiCam.dll")]
                internal static extern int McGetParamInt(uint instance, uint parameterId, out int value);
                [DllImport("MultiCam.dll")]
                internal static extern int McGetParamNmInt(uint instance, string parameterName, out int value);
                [DllImport("MultiCam.dll")]
                internal static extern int McGetParamStr(uint instance, uint parameterId, IntPtr value, uint maxLength);
                [DllImport("MultiCam.dll")]
                internal static extern int McGetParamNmStr(uint instance, string parameterName, IntPtr value, uint maxLength);
                [DllImport("MultiCam.dll")]
                internal static extern int McGetParamFloat(uint instance, uint parameterId, out double value);
                [DllImport("MultiCam.dll")]
                internal static extern int McGetParamNmFloat(uint instance, string parameterName, out double value);
                [DllImport("MultiCam.dll")]
                internal static extern int McGetParamInst(uint instance, uint parameterId, out uint value);
                [DllImport("MultiCam.dll")]
                internal static extern int McGetParamNmInst(uint instance, string parameterName, out uint value);
                [DllImport("MultiCam.dll")]
                internal static extern int McGetParamPtr(uint instance, uint parameterId, out IntPtr value);
                [DllImport("MultiCam.dll")]
                internal static extern int McGetParamNmPtr(uint instance, string parameterName, out IntPtr value);
                [DllImport("MultiCam.dll")]
                internal static extern int McGetParamInt64(uint instance, uint parameterId, out long value);
                [DllImport("MultiCam.dll")]
                internal static extern int McGetParamNmInt64(uint instance, string parameterName, out long value);
                [DllImport("MultiCam.dll")]
                internal static extern int McRegisterCallback(uint instance, CALLBACK callbackFunction, uint context);
                [DllImport("MultiCam.dll")]
                internal static extern int McWaitSignal(uint instance, int signal, uint timeout, out SIGNALINFO info);
                [DllImport("MultiCam.dll")]
                internal static extern int McGetSignalInfo(uint instance, int signal, out SIGNALINFO info);
            }
            #endregion

            #region Private Constants
            private const int MAX_VALUE_LENGTH = 1024;
            #endregion

            #region Default object instance Constants
            public const uint CONFIGURATION = 0x20000000;
            public const uint BOARD = 0xE0000000;
            public const uint CHANNEL = 0x8000FFFF;
            public const uint DEFAULT_SURFACE_HANDLE = 0x4FFFFFFF;
            #endregion

            #region Specific parameter values Constants
            public const int INFINITE = -1;
            public const int INDETERMINATE = -1;
            public const int DISABLE = 0;
            #endregion

            #region Signal handling Constants
            public const uint SignalEnable = (24 << 14);

            public const int SIG_ANY = 0;
            public const int SIG_SURFACE_PROCESSING = 1;
            public const int SIG_SURFACE_FILLED = 2;
            public const int SIG_UNRECOVERABLE_OVERRUN = 3;
            public const int SIG_FRAMETRIGGER_VIOLATION = 4;
            public const int SIG_START_EXPOSURE = 5;
            public const int SIG_END_EXPOSURE = 6;
            public const int SIG_ACQUISITION_FAILURE = 7;
            public const int SIG_CLUSTER_UNAVAILABLE = 8;
            public const int SIG_RELEASE = 9;
            public const int SIG_END_ACQUISITION_SEQUENCE = 10;
            public const int SIG_START_ACQUISITION_SEQUENCE = 11;
            public const int SIG_END_CHANNEL_ACTIVITY = 12;

            public const int SIG_GOLOW = (1 << 12);
            public const int SIG_GOHIGH = (2 << 12);
            public const int SIG_GOOPEN = (3 << 12);
            #endregion

            #region Signal handling Type Definitions
            public delegate void CALLBACK(ref SIGNALINFO signalInfo);

            [StructLayout(LayoutKind.Sequential)]
            public struct SIGNALINFO
            {
                public IntPtr Context;
                public uint Instance;
                public int Signal;
                public uint SignalInfo;
                public uint SignalContext;
            };
            #endregion

            #region Constructors
            private MC() { }
            #endregion

            #region Error handling Methods
            private static string GetErrorMessage(int errorCode)
            {
                const uint ErrorDesc = (98 << 14);
                string errorDescription;
                uint status = (uint)Math.Abs(errorCode);
                IntPtr text = Marshal.AllocHGlobal(MAX_VALUE_LENGTH + 1);
                if (NativeMethods.McGetParamStr(CONFIGURATION, ErrorDesc + status, text, MAX_VALUE_LENGTH) != 0)
                {
                    errorDescription = "Unknown error";
                }
                else
                {
                    errorDescription = Marshal.PtrToStringAnsi(text);
                }

                Marshal.FreeHGlobal(text);
                return errorDescription;
            }

            private static void ThrowOnMultiCamError(int status, string action)
            {
                if (status != 0)
                {
                    string error = action + ": " + GetErrorMessage(status);
                    throw new Euresys.MultiCamException(error);
                }
            }
            #endregion

            #region Driver connection Methods
            public static void OpenDriver()
            {
                ThrowOnMultiCamError(NativeMethods.McOpenDriver((IntPtr)null),
                    "Cannot open MultiCam driver");
            }

            public static void CloseDriver()
            {
                ThrowOnMultiCamError(NativeMethods.McCloseDriver(),
                    "Cannot close MultiCam driver");
            }
            #endregion

            #region Object creation/deletion Methods
            public static void Create(uint modelInstance, out uint instance)
            {
                ThrowOnMultiCamError(NativeMethods.McCreate(modelInstance, out instance),
                    string.Format("Cannot create '{0}' instance", modelInstance));
            }

            public static void Create(string modelName, out uint instance)
            {
                ThrowOnMultiCamError(NativeMethods.McCreateNm(modelName, out instance),
                    string.Format("Cannot create '{0}' instance", modelName));
            }

            public static void Delete(uint instance)
            {
                ThrowOnMultiCamError(NativeMethods.McDelete(instance),
                    string.Format("Cannot delete '{0}' instance", instance));
            }
            #endregion

            #region Parameter 'setter' Methods
            public static void SetParam(uint instance, uint parameterId, int value)
            {
                ThrowOnMultiCamError(NativeMethods.McSetParamInt(instance, parameterId, value),
                    string.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
            }

            public static void SetParam(uint instance, string parameterName, int value)
            {
                ThrowOnMultiCamError(NativeMethods.McSetParamNmInt(instance, parameterName, value),
                    string.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
            }

            public static void SetParam(uint instance, uint parameterId, string value)
            {
                ThrowOnMultiCamError(NativeMethods.McSetParamStr(instance, parameterId, value),
                    string.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
            }

            public static void SetParam(uint instance, string parameterName, string value)
            {
                ThrowOnMultiCamError(NativeMethods.McSetParamNmStr(instance, parameterName, value),
                    string.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
            }

            public static void SetParam(uint instance, uint parameterId, double value)
            {
                ThrowOnMultiCamError(NativeMethods.McSetParamFloat(instance, parameterId, value),
                    string.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
            }

            public static void SetParam(uint instance, string parameterName, double value)
            {
                ThrowOnMultiCamError(NativeMethods.McSetParamNmFloat(instance, parameterName, value),
                    string.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
            }

            public static void SetParam(uint instance, uint parameterId, uint value)
            {
                ThrowOnMultiCamError(NativeMethods.McSetParamInst(instance, parameterId, value),
                    string.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
            }

            public static void SetParam(uint instance, string parameterName, uint value)
            {
                ThrowOnMultiCamError(NativeMethods.McSetParamNmInst(instance, parameterName, value),
                    string.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
            }

            public static void SetParam(uint instance, uint parameterId, IntPtr value)
            {
                ThrowOnMultiCamError(NativeMethods.McSetParamPtr(instance, parameterId, value),
                    string.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
            }

            public static void SetParam(uint instance, string parameterName, IntPtr value)
            {
                ThrowOnMultiCamError(NativeMethods.McSetParamNmPtr(instance, parameterName, value),
                    string.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
            }

            public static void SetParam(uint instance, uint parameterId, long value)
            {
                ThrowOnMultiCamError(NativeMethods.McSetParamInt64(instance, parameterId, value),
                    string.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
            }

            public static void SetParam(uint instance, string parameterName, long value)
            {
                ThrowOnMultiCamError(NativeMethods.McSetParamNmInt64(instance, parameterName, value),
                    string.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
            }
            #endregion

            #region Parameter 'getter' Methods
            public static void GetParam(uint instance, uint parameterId, out int value)
            {
                ThrowOnMultiCamError(NativeMethods.McGetParamInt(instance, parameterId, out value),
                    string.Format("Cannot get parameter '{0}'", parameterId));
            }

            public static void GetParam(uint instance, string parameterName, out int value)
            {
                ThrowOnMultiCamError(NativeMethods.McGetParamNmInt(instance, parameterName, out value),
                    string.Format("Cannot get parameter '{0}'", parameterName));
            }

            public static void GetParam(uint instance, uint parameterId, out string value)
            {
                IntPtr text = Marshal.AllocHGlobal(MAX_VALUE_LENGTH + 1);
                try
                {
                    ThrowOnMultiCamError(NativeMethods.McGetParamStr(instance, parameterId, text, MAX_VALUE_LENGTH),
                        string.Format("Cannot get parameter '{0}'", parameterId));
                    value = Marshal.PtrToStringAnsi(text);
                }
                finally
                {
                    Marshal.FreeHGlobal(text);
                }
            }

            public static void GetParam(uint instance, string parameterName, out string value)
            {
                IntPtr text = Marshal.AllocHGlobal(MAX_VALUE_LENGTH + 1);
                try
                {
                    ThrowOnMultiCamError(NativeMethods.McGetParamNmStr(instance, parameterName, text, MAX_VALUE_LENGTH),
                        string.Format("Cannot get parameter '{0}'", parameterName));
                    value = Marshal.PtrToStringAnsi(text);
                }
                finally
                {
                    Marshal.FreeHGlobal(text);
                }
            }

            public static void GetParam(uint instance, uint parameterId, out double value)
            {
                ThrowOnMultiCamError(NativeMethods.McGetParamFloat(instance, parameterId, out value),
                    string.Format("Cannot get parameter '{0}'", parameterId));
            }

            public static void GetParam(uint instance, string parameterName, out double value)
            {
                ThrowOnMultiCamError(NativeMethods.McGetParamNmFloat(instance, parameterName, out value),
                    string.Format("Cannot get parameter '{0}'", parameterName));
            }

            public static void GetParam(uint instance, uint parameterId, out uint value)
            {
                ThrowOnMultiCamError(NativeMethods.McGetParamInst(instance, parameterId, out value),
                    string.Format("Cannot get parameter '{0}'", parameterId));
            }

            public static void GetParam(uint instance, string parameterName, out uint value)
            {
                ThrowOnMultiCamError(NativeMethods.McGetParamNmInst(instance, parameterName, out value),
                    string.Format("Cannot get parameter '{0}'", parameterName));
            }

            public static void GetParam(uint instance, uint parameterId, out IntPtr value)
            {
                ThrowOnMultiCamError(NativeMethods.McGetParamPtr(instance, parameterId, out value),
                    string.Format("Cannot get parameter '{0}'", parameterId));
            }

            public static void GetParam(uint instance, string parameterName, out IntPtr value)
            {
                ThrowOnMultiCamError(NativeMethods.McGetParamNmPtr(instance, parameterName, out value),
                    string.Format("Cannot get parameter '{0}'", parameterName));
            }

            public static void GetParam(uint instance, uint parameterId, out long value)
            {
                ThrowOnMultiCamError(NativeMethods.McGetParamInt64(instance, parameterId, out value),
                    string.Format("Cannot get parameter '{0}'", parameterId));
            }

            public static void GetParam(uint instance, string parameterName, out long value)
            {
                ThrowOnMultiCamError(NativeMethods.McGetParamNmInt64(instance, parameterName, out value),
                    string.Format("Cannot get parameter '{0}'", parameterName));
            }
            #endregion

            #region Signal handling Methods
            public static void RegisterCallback(uint instance, CALLBACK callbackFunction, uint context)
            {
                ThrowOnMultiCamError(NativeMethods.McRegisterCallback(instance, callbackFunction, context),
                    "Cannot register callback");
            }

            public static void WaitSignal(uint instance, int signal, uint timeout, out SIGNALINFO info)
            {
                ThrowOnMultiCamError(NativeMethods.McWaitSignal(instance, signal, timeout, out info),
                    "WaitSignal error");
            }

            public static void GetSignalInfo(uint instance, int signal, out SIGNALINFO info)
            {
                ThrowOnMultiCamError(NativeMethods.McGetSignalInfo(instance, signal, out info),
                    "Cannot get signal information");
            }
            #endregion
        }
    }
}
