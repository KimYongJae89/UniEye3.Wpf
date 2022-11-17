using System.Timers;
using WPF.UniScanCM.Override;

namespace WPF.UniScanCM.Service
{
    public static class AliveService
    {
        private static DeviceManager DeviceManager => DeviceManager.Instance() as DeviceManager;
        private static Timer AliveCheckTimer { get; set; }
        public static bool Heart { get; private set; } = true;

        // 점등을 하는 경우 쓰는 함수
        public static void StartAliveCheckTimer(int interval, bool isIO = true)
        {
            AliveCheckTimer = new Timer();
            AliveCheckTimer.Interval = interval;
            if (isIO)
            {
                AliveCheckTimer.Elapsed += IOAliveCheckTimer_Elapsed;
            }
            else
            {
                AliveCheckTimer.Elapsed += PLCAliveCheckTimer_Elapsed;
            }

            AliveCheckTimer.Start();
        }

        public static void StopAliveCheckTimer()
        {
            if (AliveCheckTimer != null)
            {
                AliveCheckTimer.Stop();
            }
        }

        // 점등을 안하는 경우 쓰는 함수
        public static void StartAliveCheckIO()
        {
            DeviceManager.SendIOHeartSignal(true);
        }

        // 점등을 안하는 경우 쓰는 함수
        public static void StopAliveCheckIO()
        {
            DeviceManager.SendIOHeartSignal(false);
        }

        private static void IOAliveCheckTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            DeviceManager.SendIOHeartSignal(Heart);
            Heart = !Heart;
        }

        private static void PLCAliveCheckTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            DeviceManager.SendPLCHeartSignal(Heart);
            Heart = !Heart;
        }
    }
}
