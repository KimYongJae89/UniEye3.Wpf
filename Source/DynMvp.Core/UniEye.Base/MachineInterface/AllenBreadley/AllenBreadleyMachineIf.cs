using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UniEye.Base.MachineInterface.AllenBreadley
{
    public partial class AllenBreadleyMachineIf
    {
        #region 생성자
        public AllenBreadleyMachineIf(MachineIfSetting machineIfSetting, IProtocol protocol = null) : base(machineIfSetting, protocol)
        {
            var asm = Assembly.LoadFrom("ABPLC_TagCommW.dll");
            Type myStaticClassType = asm.GetExportedTypes().First(f => f.Name == "PLC");

            InitConsol = (D_InitConsol)Delegate.CreateDelegate(typeof(D_InitConsol), myStaticClassType.GetMethod("InitConsol"), true);
            DisposeConsol = (D_DisposeConsol)Delegate.CreateDelegate(typeof(D_DisposeConsol), myStaticClassType.GetMethod("DisposeConsol"), true);

            GetErrorString = (D_GetErrorString)Delegate.CreateDelegate(typeof(D_GetErrorString), myStaticClassType.GetMethod("GetErrorString"), true);

            InitPLC = (D_InitPLC)Delegate.CreateDelegate(typeof(D_InitPLC), myStaticClassType.GetMethod("InitPLC"), true);
            ReleasePLC = (D_ReleasePLC)Delegate.CreateDelegate(typeof(D_ReleasePLC), myStaticClassType.GetMethod("ReleasePLC"), true);

            Register = (D_Register)Delegate.CreateDelegate(typeof(D_Register), myStaticClassType.GetMethod("Register", new Type[] { typeof(int), typeof(string), typeof(int), typeof(int) }), true);
            RegisterStr = (D_RegisterStr)Delegate.CreateDelegate(typeof(D_RegisterStr), myStaticClassType.GetMethod("RegisterStr"), true);
            UnRegister = (D_UnRegister)Delegate.CreateDelegate(typeof(D_UnRegister), myStaticClassType.GetMethod("UnRegister"), true);

            ReadDataS08 = (D_ReadDataS08)Delegate.CreateDelegate(typeof(D_ReadDataS08), myStaticClassType.GetMethod("ReadData", new Type[] { typeof(sbyte[]), typeof(int), typeof(int), typeof(int) }), true);
            ReadDataU08 = (D_ReadDataU08)Delegate.CreateDelegate(typeof(D_ReadDataU08), myStaticClassType.GetMethod("ReadData", new Type[] { typeof(byte[]), typeof(int), typeof(int), typeof(int) }), true);
            ReadDataS16 = (D_ReadDataS16)Delegate.CreateDelegate(typeof(D_ReadDataS16), myStaticClassType.GetMethod("ReadData", new Type[] { typeof(short[]), typeof(int), typeof(int), typeof(int) }), true);
            ReadDataU16 = (D_ReadDataU16)Delegate.CreateDelegate(typeof(D_ReadDataU16), myStaticClassType.GetMethod("ReadData", new Type[] { typeof(ushort[]), typeof(int), typeof(int), typeof(int) }), true);
            ReadDataS32 = (D_ReadDataS32)Delegate.CreateDelegate(typeof(D_ReadDataS32), myStaticClassType.GetMethod("ReadData", new Type[] { typeof(int[]), typeof(int), typeof(int), typeof(int) }), true);
            ReadDataU32 = (D_ReadDataU32)Delegate.CreateDelegate(typeof(D_ReadDataU32), myStaticClassType.GetMethod("ReadData", new Type[] { typeof(uint[]), typeof(int), typeof(int), typeof(int) }), true);
            ReadDataS64 = (D_ReadDataS64)Delegate.CreateDelegate(typeof(D_ReadDataS64), myStaticClassType.GetMethod("ReadData", new Type[] { typeof(long[]), typeof(int), typeof(int), typeof(int) }), true);
            ReadDataU64 = (D_ReadDataU64)Delegate.CreateDelegate(typeof(D_ReadDataU64), myStaticClassType.GetMethod("ReadData", new Type[] { typeof(ulong[]), typeof(int), typeof(int), typeof(int) }), true);
            ReadDataR32 = (D_ReadDataR32)Delegate.CreateDelegate(typeof(D_ReadDataR32), myStaticClassType.GetMethod("ReadData", new Type[] { typeof(float[]), typeof(int), typeof(int), typeof(int) }), true);
            ReadDataR64 = (D_ReadDataR64)Delegate.CreateDelegate(typeof(D_ReadDataR64), myStaticClassType.GetMethod("ReadData", new Type[] { typeof(double[]), typeof(int), typeof(int), typeof(int) }), true);
            ReadString = (D_ReadString)Delegate.CreateDelegate(typeof(D_ReadString), myStaticClassType.GetMethod("ReadString"), true);

            SetTagValueS08 = (D_SetTagValueS08)Delegate.CreateDelegate(typeof(D_SetTagValueS08), myStaticClassType.GetMethod("SetTagValue", new Type[] { typeof(int), typeof(int), typeof(int), typeof(sbyte) }), true);
            SetTagValueU08 = (D_SetTagValueU08)Delegate.CreateDelegate(typeof(D_SetTagValueU08), myStaticClassType.GetMethod("SetTagValue", new Type[] { typeof(int), typeof(int), typeof(int), typeof(byte) }), true);
            SetTagValueS16 = (D_SetTagValueS16)Delegate.CreateDelegate(typeof(D_SetTagValueS16), myStaticClassType.GetMethod("SetTagValue", new Type[] { typeof(int), typeof(int), typeof(int), typeof(short) }), true);
            SetTagValueU16 = (D_SetTagValueU16)Delegate.CreateDelegate(typeof(D_SetTagValueU16), myStaticClassType.GetMethod("SetTagValue", new Type[] { typeof(int), typeof(int), typeof(int), typeof(ushort) }), true);
            SetTagValueS32 = (D_SetTagValueS32)Delegate.CreateDelegate(typeof(D_SetTagValueS32), myStaticClassType.GetMethod("SetTagValue", new Type[] { typeof(int), typeof(int), typeof(int), typeof(int) }), true);
            SetTagValueU32 = (D_SetTagValueU32)Delegate.CreateDelegate(typeof(D_SetTagValueU32), myStaticClassType.GetMethod("SetTagValue", new Type[] { typeof(int), typeof(int), typeof(int), typeof(uint) }), true);
            SetTagValueS64 = (D_SetTagValueS64)Delegate.CreateDelegate(typeof(D_SetTagValueS64), myStaticClassType.GetMethod("SetTagValue", new Type[] { typeof(int), typeof(int), typeof(int), typeof(long) }), true);
            SetTagValueU64 = (D_SetTagValueU64)Delegate.CreateDelegate(typeof(D_SetTagValueU64), myStaticClassType.GetMethod("SetTagValue", new Type[] { typeof(int), typeof(int), typeof(int), typeof(ulong) }), true);
            SetTagValueF32 = (D_SetTagValueF32)Delegate.CreateDelegate(typeof(D_SetTagValueF32), myStaticClassType.GetMethod("SetTagValue", new Type[] { typeof(int), typeof(int), typeof(int), typeof(float) }), true);
            SetTagValueF64 = (D_SetTagValueF64)Delegate.CreateDelegate(typeof(D_SetTagValueF64), myStaticClassType.GetMethod("SetTagValue", new Type[] { typeof(int), typeof(int), typeof(int), typeof(double) }), true);
            WriteString = (D_WriteString)Delegate.CreateDelegate(typeof(D_WriteString), myStaticClassType.GetMethod("WriteString"), true);

            WriteData = (D_WriteData)Delegate.CreateDelegate(typeof(D_WriteData), myStaticClassType.GetMethod("WriteData"), true);
        }
        #endregion


        #region 속성
        private List<string> tagNameList = new List<string>();

        private delegate IntPtr D_InitConsol();
        private delegate void D_DisposeConsol();
        private D_InitConsol InitConsol;
        private D_DisposeConsol DisposeConsol;

        private delegate string D_GetErrorString(int errCode);
        private D_GetErrorString GetErrorString;

        private delegate void D_InitPLC(string ipAddress, string cpuType, string path, int debugLevel);
        private delegate void D_ReleasePLC();
        private D_InitPLC InitPLC;
        private D_ReleasePLC ReleasePLC;

        private delegate void D_Register(int iTagIndex, string strTagName, int iDataType, int iCount);
        private delegate void D_RegisterStr(int iTagIndex, string strTagName);
        private delegate void D_UnRegister(int iTagIndex);
        private D_Register Register;
        private D_RegisterStr RegisterStr;
        private D_UnRegister UnRegister;

        private delegate void D_ReadDataS08(sbyte[] value, int iTagIndex, int iArrayCount, int iTimeout);
        private delegate void D_ReadDataU08(byte[] value, int iTagIndex, int iArrayCount, int iTimeout);
        private delegate void D_ReadDataS16(short[] value, int iTagIndex, int iArrayCount, int iTimeout);
        private delegate void D_ReadDataU16(ushort[] value, int iTagIndex, int iArrayCount, int iTimeout);
        private delegate void D_ReadDataS32(int[] value, int iTagIndex, int iArrayCount, int iTimeout);
        private delegate void D_ReadDataU32(uint[] value, int iTagIndex, int iArrayCount, int iTimeout);
        private delegate void D_ReadDataS64(long[] value, int iTagIndex, int iArrayCount, int iTimeout);
        private delegate void D_ReadDataU64(ulong[] value, int iTagIndex, int iArrayCount, int iTimeout);
        private delegate void D_ReadDataR32(float[] value, int iTagIndex, int iArrayCount, int iTimeout);
        private delegate void D_ReadDataR64(double[] value, int iTagIndex, int iArrayCount, int iTimeout);
        private delegate string D_ReadString(int iTagIndex, int iTimeout);
        private D_ReadDataS08 ReadDataS08;
        private D_ReadDataU08 ReadDataU08;
        private D_ReadDataS16 ReadDataS16;
        private D_ReadDataU16 ReadDataU16;
        private D_ReadDataS32 ReadDataS32;
        private D_ReadDataU32 ReadDataU32;
        private D_ReadDataS64 ReadDataS64;
        private D_ReadDataU64 ReadDataU64;
        private D_ReadDataR32 ReadDataR32;
        private D_ReadDataR64 ReadDataR64;
        private D_ReadString ReadString;

        private delegate void D_SetTagValueS08(int iTagIndex, int iOffset, int iDataType, sbyte value);
        private delegate void D_SetTagValueU08(int iTagIndex, int iOffset, int iDataType, byte value);
        private delegate void D_SetTagValueS16(int iTagIndex, int iOffset, int iDataType, short value);
        private delegate void D_SetTagValueU16(int iTagIndex, int iOffset, int iDataType, ushort value);
        private delegate void D_SetTagValueS32(int iTagIndex, int iOffset, int iDataType, int value);
        private delegate void D_SetTagValueU32(int iTagIndex, int iOffset, int iDataType, uint value);
        private delegate void D_SetTagValueS64(int iTagIndex, int iOffset, int iDataType, long value);
        private delegate void D_SetTagValueU64(int iTagIndex, int iOffset, int iDataType, ulong value);
        private delegate void D_SetTagValueF32(int iTagIndex, int iOffset, int iDataType, float value);
        private delegate void D_SetTagValueF64(int iTagIndex, int iOffset, int iDataType, double value);
        private delegate void D_WriteString(int iTagIndex, int iTimeout, string @string);
        private D_SetTagValueS08 SetTagValueS08;
        private D_SetTagValueU08 SetTagValueU08;
        private D_SetTagValueS16 SetTagValueS16;
        private D_SetTagValueU16 SetTagValueU16;
        private D_SetTagValueS32 SetTagValueS32;
        private D_SetTagValueU32 SetTagValueU32;
        private D_SetTagValueS64 SetTagValueS64;
        private D_SetTagValueU64 SetTagValueU64;
        private D_SetTagValueF32 SetTagValueF32;
        private D_SetTagValueF64 SetTagValueF64;
        private D_WriteString WriteString;

        private delegate void D_WriteData(int iTagIndex, int iTimeout);
        private D_WriteData WriteData;
        #endregion


        #region 메서드
        #endregion
    }

    public partial class AllenBreadleyMachineIf : MachineIf
    {
        #region 메서드
        public override void Start()
        {
            var settings = (AllenBreadleyMachineIfSetting)MachineIfSetting;
            InitPLC.Invoke(settings.TcpIpInfo.IpAddress, settings.CpuType, settings.PlcPath, 0);

            var list = settings.MachineIfItemInfoList.Dic.Values.Cast<AllenBreadleyMachineIfItemInfo>().ToList();
            list.RemoveAll(f => !f.IsValid || !f.Use);
            IEnumerable<IGrouping<string, AllenBreadleyMachineIfItemInfo>> groups = list.GroupBy(f => f.TagName);
            foreach (IGrouping<string, AllenBreadleyMachineIfItemInfo> group in groups)
            {
                tagNameList.Add(group.Key);

                int index = tagNameList.IndexOf(group.Key);
                string tagName = group.Key;
                int count = group.Max(f => f.OffsetByte4 + f.SizeByte4);
                Register(index, group.Key, sizeof(int), group.Max(f => f.OffsetByte4 + f.SizeByte4));
            }
        }

        public override void Stop()
        {
            for (int i = 0; i < tagNameList.Count; i++)
            {
                UnRegister(i);
            }

            tagNameList.Clear();

            ReleasePLC.Invoke();
        }

        public override bool IsStarted()
        {
            return true;
        }

        public override bool Send(MachineIfItemInfo itemInfo, params string[] args)
        {
            try
            {
                var allenBreadleyMachineIfProtocol = (AllenBreadleyMachineIfItemInfo)itemInfo;
                int tagId = tagNameList.IndexOf(allenBreadleyMachineIfProtocol.TagName);
                int[] resultValue;
                if (args != null && args.Length > 0)
                // 쓰기명령
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i] != null)
                        {
                            SetTagValueS32(tagId, i, sizeof(int), Convert.ToInt32(args[i], 16));
                        }
                    }
                    WriteData(tagId, itemInfo.WaitResponceMs);
                    resultValue = new int[0];
                }
                else
                // 읽기명령
                {
                    resultValue = new int[allenBreadleyMachineIfProtocol.SizeByte4];
                    ReadDataS32(resultValue, 0, allenBreadleyMachineIfProtocol.SizeByte4, itemInfo.WaitResponceMs);
                }

                if (ItemInfoResponce != null || ItemInfoResponce.IsResponced == false)
                {
                    string recivedData = string.Join("", resultValue.Select(f => f.ToString("X08")));
                    ItemInfoResponce.SetRecivedData(recivedData, true, null);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
