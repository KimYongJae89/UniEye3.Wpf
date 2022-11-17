using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Unieye.WPF.Base.Helpers
{
    public static class MinidumpWriter
    {
        #region Wrapping Function

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process([In] IntPtr processHandle,
            [Out, MarshalAs(UnmanagedType.Bool)] out bool wow64Process);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetCurrentProcess();

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool OpenProcessToken(IntPtr ProcessHandle,
            uint DesiredAccess, out IntPtr TokenHandle);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool LookupPrivilegeValue(string lpSystemName, string lpName,
            out LUID lpLuid);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AdjustTokenPrivileges(
            IntPtr TokenHandle,
            [MarshalAs(UnmanagedType.Bool)] bool DisableAllPrivileges,
            ref TOKEN_PRIVILEGES NewState,
            uint Zero,
            IntPtr Null1,
            IntPtr Null2);

        [DllImport("kernel32.dll", EntryPoint = "GetCurrentThreadId", ExactSpelling = true)]
        private static extern uint GetCurrentThreadId();

        [DllImport("DbgHelp.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        private static extern bool MiniDumpWriteDump(
                            IntPtr hProcess,
                            int processId,
                            IntPtr fileHandle,
                            MiniDumpType dumpType,
                            ref MiniDumpExceptionInformation expParam,
                            IntPtr userStreamParam,
                            IntPtr callbackParam);

        #endregion

        #region Struct

        [StructLayout(LayoutKind.Sequential)]
        public struct TOKEN_PRIVILEGES
        {
            public uint PrivilegeCount;
            public LUID Luid;
            public uint Attributes;
        }

        //[StructLayout(LayoutKind.Sequential)]
        //public struct VS_FIXEDFILEINFO
        //{
        //    public UInt32 dwSignature;
        //    public UInt32 dwStrucVersion;
        //    public UInt32 dwFileVersionMS;
        //    public UInt32 dwFileVersionLS;
        //    public UInt32 dwProductVersionMS;
        //    public UInt32 dwProductVersionLS;
        //    public UInt32 dwFileFlagsMask;
        //    public UInt32 dwFileFlags;
        //    public UInt32 dwFileOS;
        //    public UInt32 dwFileType;
        //    public UInt32 dwFileSubtype;
        //    public UInt32 dwFileDateMS;
        //    public UInt32 dwFileDateLS;
        //}

        //[StructLayout(LayoutKind.Sequential)]
        //public struct MINIDUMP_MODULE_CALLBACK
        //{
        //    [MarshalAs(UnmanagedType.LPWStr)]
        //    public String FullPath;
        //    public UInt64 BaseOfImage;
        //    public UInt32 SizeOfImage;
        //    public UInt32 CheckSum;
        //    public UInt32 TimeDateStamp;
        //    public VS_FIXEDFILEINFO VersionInfo;
        //    public IntPtr CvRecord;
        //    public UInt32 SizeOfCvRecord;
        //    public IntPtr MiscRecord;
        //    public UInt32 SizeOfMiscRecord;
        //}

        //[StructLayout(LayoutKind.Explicit)]
        //public struct MINIDUMP_CALLBACK_INPUT
        //{
        //    [FieldOffset(0)]
        //    public UInt32 ProcessId;

        //    [FieldOffset(4)]
        //    public IntPtr ProcessHandle;

        //    [FieldOffset(8)]
        //    public MINIDUMP_CALLBACK_TYPE CallbackType;

        //    [FieldOffset(12)]
        //    public IntPtr Thread;
        //    [FieldOffset(12)]
        //    public IntPtr ThreadEx;
        //    [FieldOffset(12)]
        //    public MINIDUMP_MODULE_CALLBACK Module;
        //    [FieldOffset(12)]
        //    public IntPtr IncludeThread;
        //    [FieldOffset(12)]
        //    public IntPtr IncludeModule;
        //};

        //[StructLayout(LayoutKind.Explicit)]
        //public struct MINIDUMP_CALLBACK_OUTPUT
        //{
        //    [FieldOffset(0)]
        //    public MODULE_WRITE_FLAGS ModuleWriteFlags;
        //    [FieldOffset(0)]
        //    public ulong ThreadWriteFlags;
        //}

        public struct MiniDumpExceptionInformation
        {
            public uint ThreadId;
            public IntPtr ExceptionPointers;
            [MarshalAs(UnmanagedType.Bool)]
            public bool ClientPointers;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LUID
        {
            public uint LowPart;
            public int HighPart;
        }

        public struct DUMP_PARAMETER
        {
            public IntPtr Thread;
            public int ThreadId;
            public IntPtr ExPtrs;
        }

        //public struct MINIDUMP_CALLBACK_INFORMATION
        //{
        //    public MINIDUMP_CALLBACK_ROUTINE CallbackRoutine;
        //    public IntPtr CallbackParam;
        //}

        //public delegate bool MINIDUMP_CALLBACK_ROUTINE(
        //    IntPtr CallBackParam,
        //    MINIDUMP_CALLBACK_INPUT input,
        //    MINIDUMP_CALLBACK_OUTPUT output);

        #endregion

        #region Enum

        public enum ExceptionInfo
        {
            None,
            Present
        }

        public enum MiniDumpType
        {
            Normal = 0x00000000,
            WithDataSegs = 0x00000001,
            WithFullMemory = 0x00000002,
            WithHandleData = 0x00000004,
            FilterMemory = 0x00000008,
            ScanMemory = 0x00000010,
            WithUnloadedModules = 0x00000020,
            WithIndirectlyReferencedMemory = 0x00000040,
            FilterModulePaths = 0x00000080,
            WithProcessThreadData = 0x00000100,
            WithPrivateReadWriteMemory = 0x00000200,
            WithoutOptionalData = 0x00000400,
            WithFullMemoryInfo = 0x00000800,
            WithThreadInfo = 0x00001000,
            WithCodeSegs = 0x00002000,
            WithoutAuxiliaryState = 0x00004000,
            WithFullAuxiliaryState = 0x00008000,

            TiniDump = Normal,
            MiniDump = WithFullMemory | WithFullMemoryInfo,//WithIndirectlyReferencedMemory | ScanMemory,
            MidiDump = WithPrivateReadWriteMemory | WithDataSegs | WithHandleData | WithFullMemoryInfo | WithThreadInfo | WithUnloadedModules,
            MaxiDump = WithFullMemory | WithFullMemoryInfo | WithHandleData | WithThreadInfo | WithUnloadedModules,
        }

        public enum MINIDUMP_CALLBACK_TYPE
        {
            ModuleCallback,
            ThreadCallback,
            ThreadExCallback,
            IncludeThreadCallback,
            IncludeModuleCallback,
            MemoryCallback,
            CancelCallback,
            WriteKernelMinidumpCallback,
            KernelMinidumpStatusCallback,
            RemoveMemoryCallback,
            IncludeVmRegionCallback,
            IoStartCallback,
            IoWriteAllCallback,
            IoFinishCallback,
            ReadMemoryFailureCallback,
            SecondaryFlagsCallback,
            IsProcessSnapshotCallback,
            VmStartCallback,
            VmQueryCallback,
            VmPreReadCallback,
            VmPostReadCallback
        }

        public enum MODULE_WRITE_FLAGS
        {
            ModuleWriteModule,
            ModuleWriteDataSeg,
            ModuleWriteMiscRecord,
            ModuleWriteCvRecord,
            ModuleReferencedByMemory,
            ModuleWriteTlsData,
            ModuleWriteCodeSegs
        }


        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VMOperation = 0x00000008,
            VMRead = 0x00000010,
            VMWrite = 0x00000020,
            DupHandle = 0x00000040,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            Synchronize = 0x00100000
        }

        #endregion

        #region Constant

        private static uint STANDARD_RIGHTS_REQUIRED = 0x000F0000;
        private static uint STANDARD_RIGHTS_READ = 0x00020000;
        private static uint TOKEN_ASSIGN_PRIMARY = 0x0001;
        private static uint TOKEN_DUPLICATE = 0x0002;
        private static uint TOKEN_IMPERSONATE = 0x0004;
        private static uint TOKEN_QUERY = 0x0008;
        private static uint TOKEN_QUERY_SOURCE = 0x0010;
        private static uint TOKEN_ADJUST_PRIVILEGES = 0x0020;
        private static uint TOKEN_ADJUST_GROUPS = 0x0040;
        private static uint TOKEN_ADJUST_DEFAULT = 0x0080;
        private static uint TOKEN_ADJUST_SESSIONID = 0x0100;
        private static uint TOKEN_READ = (STANDARD_RIGHTS_READ | TOKEN_QUERY);
        private static uint TOKEN_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | TOKEN_ASSIGN_PRIMARY |
            TOKEN_DUPLICATE | TOKEN_IMPERSONATE | TOKEN_QUERY | TOKEN_QUERY_SOURCE |
            TOKEN_ADJUST_PRIVILEGES | TOKEN_ADJUST_GROUPS | TOKEN_ADJUST_DEFAULT |
            TOKEN_ADJUST_SESSIONID);

        public const string SE_DEBUG_NAME = "SeDebugPrivilege";

        public const uint SE_PRIVILEGE_ENABLED_BY_DEFAULT = 0x00000001;
        public const uint SE_PRIVILEGE_ENABLED = 0x00000002;
        public const uint SE_PRIVILEGE_REMOVED = 0x00000004;
        public const uint SE_PRIVILEGE_USED_FOR_ACCESS = 0x80000000;

        #endregion

        public static MiniDumpType DumpType = MiniDumpType.MiniDump;

        private static List<string> ReferenceAssemblyList = new List<string>();
        //private static bool IsWritingDump = false;
        private static Process CurrentProcess = null;
        public static void StartDumpWriter()
        {
            CurrentProcess = Process.GetCurrentProcess();

            var assem = Assembly.Load(CurrentProcess?.ProcessName);
            AssemblyName[] assemblies = assem.GetReferencedAssemblies();

            ReferenceAssemblyList.Clear();
            ReferenceAssemblyList.Add(assem.GetName().Name);
            ReferenceAssemblyList.AddRange(assemblies.Where(x => x.GetPublicKeyToken().Length == 0).Select(x => x.Name));

            // 두번 호출되지 않게 예외처리

            AppDomain.CurrentDomain.FirstChanceException -= OnFirstChanceHandler;
            AppDomain.CurrentDomain.FirstChanceException += OnFirstChanceHandler;

            // UnhandledException은 실제 예외가 발생한 구문을 벗어난 후 호출이 되기 때문에 정확한 Dump파일을 생성할 수 없다.
            Application.Current.DispatcherUnhandledException -= OnProcessUnhandledException;
            Application.Current.DispatcherUnhandledException += OnProcessUnhandledException;
        }

        private static void OnFirstChanceHandler(object sender, FirstChanceExceptionEventArgs e)
        {
            if (ReferenceAssemblyList.Exists(x => x == e.Exception.Source))
            {
                // 메인 프로세스에서 Exception이 발생하면 Dump를 한번만 기록한다.
                // 이 이벤트는 모든 Exception이 걸리기 때문에 다운되지 않는 Exception이 기록되는지 여부는 추가로 확인해야 한다.
                MakeDump(e.Exception.GetType().ToString());
                LogHelper.Error(LoggerType.Error, "FirstChanceException");
                LogHelper.Error(e.Exception.StackTrace);
            }
        }

        private static void OnProcessUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            LogHelper.Error(LoggerType.Error, "Unhandled exception");
            LogHelper.Error(e.Exception.StackTrace);

            CloseHandle(CurrentProcess.Handle);
        }

        private static void MakeDump(string exceptionName)
        {
            //IsWritingDump = true;

            string folder = string.Format(@"{0}\..\Dump", Directory.GetCurrentDirectory());
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string dumpFilePath = string.Format(@"{0}\{1}_{2}.dmp", folder, DateTime.Now.ToString("yyyyMMddHHmmssfff"), exceptionName);

            var process = Process.GetCurrentProcess();
            int processId = process.Id;

            //SetDumpPrivileges();

            using (var stream = new FileStream(dumpFilePath, FileMode.Create))
            {
                //MINIDUMP_CALLBACK_INFORMATION CallbackInfo;
                //CallbackInfo.CallbackRoutine = OnMiniDumpCallback;
                //CallbackInfo.CallbackParam = IntPtr.Zero;

                MiniDumpExceptionInformation exp;
                exp.ThreadId = GetCurrentThreadId();
                exp.ClientPointers = false;
                exp.ExceptionPointers = IntPtr.Zero;

                bool res = MiniDumpWriteDump(
                    process.Handle,
                    processId,
                    stream.SafeFileHandle.DangerousGetHandle(),
                    DumpType,
                    ref exp,
                    IntPtr.Zero,
                    IntPtr.Zero);

                int dumpError = res ? 0 : Marshal.GetLastWin32Error();
                Console.WriteLine(dumpError);
            }

            //IsWritingDump = false;
        }

        //private static bool OnMiniDumpCallback(IntPtr CallBackParam, MINIDUMP_CALLBACK_INPUT pInput, MINIDUMP_CALLBACK_OUTPUT pOutput)
        //{
        //    bool bRet = false;

        //    // Check parameters 

        //    //if (pInput == null)
        //    //    return false;

        //    //if (pOutput == null)
        //    //    return false;

        //    // Process the callbacks 

        //    switch (pInput.CallbackType)
        //    {
        //        case MINIDUMP_CALLBACK_TYPE.IncludeModuleCallback:
        //            {
        //                // Include the module into the dump 
        //                bRet = true;
        //            }
        //            break;

        //        case MINIDUMP_CALLBACK_TYPE.IncludeThreadCallback:
        //            {
        //                // Include the thread into the dump 
        //                bRet = true;
        //            }
        //            break;

        //        case MINIDUMP_CALLBACK_TYPE.ModuleCallback:
        //            {
        //                // Are data sections available for this module ? 

        //                if ((pOutput.ModuleWriteFlags & MODULE_WRITE_FLAGS.ModuleWriteDataSeg) == MODULE_WRITE_FLAGS.ModuleWriteDataSeg)
        //                {
        //                    // Yes, they are, but do we need them? 

        //                    Console.WriteLine("Excluding module data sections: {0} \n", pInput.Module.FullPath);
        //                    pOutput.ModuleWriteFlags &= (~MODULE_WRITE_FLAGS.ModuleWriteDataSeg);
        //                }

        //                bRet = true;
        //            }
        //            break;

        //        case MINIDUMP_CALLBACK_TYPE.ThreadCallback:
        //            {
        //                // Include all thread information into the minidump 
        //                bRet = true;
        //            }
        //            break;

        //        case MINIDUMP_CALLBACK_TYPE.ThreadExCallback:
        //            {
        //                // Include this information 
        //                bRet = true;
        //            }
        //            break;

        //        case MINIDUMP_CALLBACK_TYPE.MemoryCallback:
        //            {
        //                // We do not include any information here -> return FALSE 
        //                bRet = false;
        //            }
        //            break;

        //        case MINIDUMP_CALLBACK_TYPE.CancelCallback:
        //            break;
        //    }

        //    return bRet;
        //}

        private static void SetDumpPrivileges()
        {
            TOKEN_PRIVILEGES tkpPrivileges;

            if (!OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, out IntPtr hToken))
            {
                Console.WriteLine("OpenProcessToken() failed, error = {0} . SeDebugPrivilege is not available", Marshal.GetLastWin32Error());
                return;
            }

            if (!LookupPrivilegeValue(null, SE_DEBUG_NAME, out LUID luidSEDebugNameValue))
            {
                Console.WriteLine("LookupPrivilegeValue() failed, error = {0} .SeDebugPrivilege is not available", Marshal.GetLastWin32Error());
                CloseHandle(hToken);
                return;
            }

            tkpPrivileges.PrivilegeCount = 1;
            tkpPrivileges.Luid = luidSEDebugNameValue;
            tkpPrivileges.Attributes = SE_PRIVILEGE_ENABLED;

            if (!AdjustTokenPrivileges(hToken, false, ref tkpPrivileges, 0, IntPtr.Zero, IntPtr.Zero))
            {
                Console.WriteLine("LookupPrivilegeValue() failed, error = {0} .SeDebugPrivilege is not available", Marshal.GetLastWin32Error());
                return;
            }

            CloseHandle(hToken);
        }
    }
}
