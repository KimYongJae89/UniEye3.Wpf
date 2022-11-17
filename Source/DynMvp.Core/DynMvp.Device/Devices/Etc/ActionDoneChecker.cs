using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DynMvp.Devices
{
    public class ActionTimeoutException : ApplicationException
    {
        public ActionTimeoutException(string msg)
            : base(msg)
        {
        }
    }

    public delegate bool IsActionError();
    public delegate bool IsActionDone(bool enableLog = true);

    public class ActionDoneChecker
    {
        public IsActionDone IsActionDone { get; set; }
        public IsActionError IsActionError { get; set; }
        public static bool StopDoneChecker { get; set; } = false;

        public bool WaitActionDone(int timeoutMilliSec, int initialWaitTime = 0)
        {
            if (timeoutMilliSec <= 0)
            {
                timeoutMilliSec = 1000;
            }

            var timeOutTimer = new TimeOutTimer();
            timeOutTimer.Start(timeoutMilliSec);

            while (timeOutTimer.TimeOut == false)
            {
                if (IsActionDone())
                {
                    return true;
                }

                if (StopDoneChecker == true)
                {
                    return true;
                }

                Thread.Sleep(10);
            }

            return false;
        }
    }
}
