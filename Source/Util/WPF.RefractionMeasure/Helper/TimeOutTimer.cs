using System;
using System.Timers;

namespace WPF.RefractionMeasure.Helper
{
    public class TimeOutTimer
    {
        private System.Timers.Timer timer = null;
        public bool TimeOut { get; private set; } = false;

        public TimeOutTimer()
        {
            timer = new System.Timers.Timer();
            timer.Elapsed += timer_Elapsed;
        }

        public void Start(int timeOutMs)
        {
            TimeOut = false;
            timer.Interval = timeOutMs;
            timer.Start();
        }

        public void Restart()
        {
            timer.Stop();
            TimeOut = false;
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        public void Reset()
        {
            timer.Stop();
            TimeOut = false;
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            TimeOut = true;
        }

        public void ThrowIfTimeOut()
        {
            if (TimeOut)
            {
                throw new TimeoutException();
            }
        }
    }
}
