using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Base
{
    public class EtcHelper
    {
        private struct SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;
        }

        [DllImport("coredll.dll")]
        private static extern void GetSystemTime(ref SYSTEMTIME lpSystemTime);

        [DllImport("coredll.dll")]
        private static extern uint SetSystemTime(ref SYSTEMTIME lpSystemTime);

        public static void SetSystemTime(DateTime dateTime)
        {
            var systime = new SYSTEMTIME();
            GetSystemTime(ref systime);

            // Set the system clock ahead one hour.
            systime.wYear = (ushort)dateTime.Year;
            systime.wMonth = (ushort)dateTime.Month;
            systime.wDay = (ushort)dateTime.Day;
            systime.wHour = (ushort)dateTime.Hour;
            systime.wMinute = (ushort)dateTime.Minute;
            systime.wSecond = (ushort)dateTime.Second;
            SetSystemTime(ref systime);
        }
    }
}
