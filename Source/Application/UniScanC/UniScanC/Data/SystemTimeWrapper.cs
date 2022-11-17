using System.Runtime.InteropServices;

namespace UniScanC.Data
{
    public static class SystemTimeWrapper
    {
        [DllImport("kernel32.dll")]
        public static extern bool SetLocalTime(ref SYSTEMTIME time);

        [DllImport("kernel32.dll")]
        public static extern bool GetLocalTime(out SYSTEMTIME time);

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct SYSTEMTIME
        {
            public short year;
            public short month;
            public short dayOfWeek;
            public short day;
            public short hour;
            public short minute;
            public short second;
            public short milliseconds;
        }
    }
}
