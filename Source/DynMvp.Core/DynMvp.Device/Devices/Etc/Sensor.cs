using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Devices
{
    public class Sensor
    {
        private Stopwatch timer = new Stopwatch();
        private bool initialized = false;
        private bool curValue = false;
        private bool filterValue = false;
        public int DelayTimeMs { get; set; } = 500;

        public Sensor()
        {
            initialized = false;
            curValue = false;
            filterValue = false;
        }

        public void UpdateEmg(bool value)
        {
            if (initialized == false)
            {
                filterValue = curValue = value;
                initialized = true;
            }
            else if (value != curValue)
            {
                curValue = value;
                timer.Start();
            }
            else if (timer.ElapsedMilliseconds > 300)
            {
                filterValue = curValue;
                if (timer.IsRunning == true)
                {
                    timer.Stop();
                }
            }
        }

        public void Update(bool value)
        {
            if (initialized == false)
            {
                filterValue = curValue = value;
                initialized = true;
                return;
            }

            if (curValue != value)
            {
                curValue = value;
                timer.Start();
            }

            if (value == true)
            {
                filterValue = value;
                if (timer.IsRunning == true)
                {
                    timer.Stop();
                }
            }
            else if (timer.ElapsedMilliseconds > DelayTimeMs || DelayTimeMs == 0)
            {
                filterValue = curValue;
                if (timer.IsRunning == false)
                {
                    timer.Stop();
                }
            }
        }

        public bool IsSignalOn()
        {
            return (filterValue == true);
        }
    }
}
