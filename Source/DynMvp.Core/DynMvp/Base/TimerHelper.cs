using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace DynMvp.Base
{
    public class TimerHelper
    {
        public static void Sleep(float sleepTimeMs)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            while (stopWatch.ElapsedMilliseconds < sleepTimeMs)
            {
                Thread.Sleep(0);
            }
        }
    }
}
