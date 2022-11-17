using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DynMvp.Base
{
    public class ThreadHandler
    {
        [DllImport("kernel32")]
        private static extern int GetCurrentThreadId();

        private string name;

        protected Thread workingThread;
        public Thread WorkingThread
        {
            get => workingThread;
            set => workingThread = value;
        }

        protected bool requestStop;
        public bool RequestStop
        {
            get => requestStop;
            set => requestStop = value;
        }

        public bool IsRunning => workingThread.IsAlive;

        public ThreadHandler(string name, Thread workingThread = null, bool requestStop = false)
        {
            this.name = name;
            this.workingThread = workingThread;
            this.requestStop = requestStop;
        }

        protected void SetAffinity(int coreNo)
        {
            LogHelper.Debug(LoggerType.Function, string.Format("ThreadHandler({0})::SetAffinity", name));
            var Proc = Process.GetCurrentProcess();
            //long AffinityMask = (long)Proc.ProcessorAffinity;
            //if (coreMask <= 0)
            //    coreMask = 0xFFFF;
            //Proc.ProcessorAffinity = (IntPtr)coreMask;

            int curThreadId = GetCurrentThreadId();
            foreach (ProcessThread th in Proc.Threads)
            {
                if (curThreadId == th.Id)
                {
                    if (coreNo == -1)
                    {
                        coreNo = 0xFFFF;
                    }
                    else
                    {
                        coreNo = 0x01 << (coreNo % Environment.ProcessorCount);
                    }

                    th.ProcessorAffinity = (IntPtr)coreNo;
                    break;
                }
            }
        }

        public void Start()
        {
            LogHelper.Debug(LoggerType.Function, string.Format("ThreadHandler({0})::Start", name));
            workingThread.Start();
            ThreadManager.AddThread(this);
        }

        public void AsyncStop()
        {
            LogHelper.Debug(LoggerType.Function, string.Format("ThreadHandler({0})::AsyncStop", name));
            if (workingThread == null)
            {
                return;
            }

            requestStop = true;
        }

        public bool IsStop()
        {
            LogHelper.Debug(LoggerType.Function, string.Format("ThreadHandler({0})::WaitStop", name));
            if (workingThread == null)
            {
                return true;
            }

            return (workingThread.IsAlive == false);
        }

        public bool Stop(int timeoutMs = -1)
        {
            if (workingThread == null)
            {
                return true;
            }

            var tt = new TimeOutTimer();
            requestStop = true;

            if (timeoutMs >= 0)
            {
                tt.Start(timeoutMs);
            }

            while (workingThread.IsAlive)
            {
                Application.DoEvents();
                Thread.Sleep(10);
                if (tt.TimeOut)
                {
                    return false;
                }
            }
            ThreadManager.RemoveThread(this);

            LogHelper.Debug(LoggerType.Function, string.Format("ThreadHandler({0})::Stop", name));
            return true;
        }
    }

    public class ThreadManager
    {
        private static List<ThreadHandler> threadHandlerList = new List<ThreadHandler>();

        public static void AddThread(ThreadHandler threadHandler)
        {
            if (threadHandlerList.Contains(threadHandler) == false)
            {
                threadHandlerList.Add(threadHandler);
            }
        }

        public static void RemoveThread(ThreadHandler threadHandler)
        {
            try
            {
                threadHandlerList.Remove(threadHandler);
            }
            catch (ArgumentOutOfRangeException)
            {

            }
        }

        public static void StopAllThread()
        {
            foreach (ThreadHandler threadHandler in threadHandlerList)
            {
                threadHandler.RequestStop = true;
            }
        }

        public static bool IsAllDead()
        {
            foreach (ThreadHandler threadHandler in threadHandlerList)
            {
                if (threadHandler.WorkingThread == null)
                {
                    continue;
                }

                if (threadHandler.WorkingThread.IsAlive)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool WaitAllDead(int timeoutMs)
        {
            Thread.Sleep(10);

            for (int i = 0; i < timeoutMs / 10; i++)
            {
                if (IsAllDead())
                {
                    Debug.Write("Wait All Thread Dead\n");

                    return true;
                }

                Thread.Sleep(10);
            }

            return false;
        }
    }
}
