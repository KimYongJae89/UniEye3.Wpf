using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Vision;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UniScanC.Data;
using WPF.UniScanIM.Override;

namespace WPF.UniScanIM.Manager
{
    public class ResultSyncData
    {
        public DateTime Time { get; set; }
        public bool IsNG { get; set; }
        public int ScanNo { get; set; }
        public int ModuleNo { get; set; }

        public ResultSyncData(DateTime time, bool isDefect, int scanNo, int moduleNo)
        {
            Time = time;
            IsNG = isDefect;
            ScanNo = scanNo;
            ModuleNo = moduleNo;
        }
    }

    public static class IMDefectSignalManager
    {
        private static CancellationTokenSource TokenSource { get; set; }
        // Queue 에서 데이터를 빼서 신호를 보내주는 스레드
        private static Thread DefectSignalThread { get; set; }

        // 나중에 검사한 결과가 먼저 들어올 수 있기 때문에 scanNo 값을 키로 받고 나중에 정렬을 해서 사용한다.
        public static Dictionary<int, ConcurrentDictionary<int, ResultSyncData>> ResultListDic { get; set; } = new Dictionary<int, ConcurrentDictionary<int, ResultSyncData>>();
        // 신호를 보내기 직전 값을 지니고 있는 큐
        private static Dictionary<int, ConcurrentQueue<ResultSyncData>> ResultSyncDataQueueDic { get; set; } = new Dictionary<int, ConcurrentQueue<ResultSyncData>>();

        // 한 파형을 보내는 테스크
        private static Dictionary<int, Task> PulseTaskDic { get; set; } = new Dictionary<int, Task>();
        // 파형을 준 마지막 시간 (파형 중복 방지를 위한 변수)
        private static Dictionary<int, DateTime> LastSignalTimeDic { get; set; } = new Dictionary<int, DateTime>();

        public static void AddJudgment(ModuleInfo moduleInfo, InspectResult inspectResult)
        {
            var markingTime = new DateTime(inspectResult.InspectStartTime.Ticks);
            if (inspectResult.DefectList.Count > 0)
            {
                // 검사 완료 시점에서 DefectPos는 Pixel좌표를 사용하므로 해상도 값을 곱해준다.
                float minYPositionM = inspectResult.DefectList.Min(defect => defect.DefectPos.Y) * moduleInfo.ResolutionHeight / 1000000f;
                markingTime += TimeSpan.FromSeconds(minYPositionM / (SystemConfig.Instance.LineSpeed / 60.0));
            }

            AddJudgment(markingTime, inspectResult.Judgment == Judgment.NG ? true : false, inspectResult.FrameIndex, inspectResult.ModuleNo);
        }

        private static void AddJudgment(DateTime imGrabbedTime, bool isDefect, int scanNo, int moduleNo)
        {
            Task.Run(() =>
            {
                if (ResultListDic.ContainsKey(moduleNo))
                {
                    ResultListDic[moduleNo].TryAdd(scanNo, new ResultSyncData(imGrabbedTime, isDefect, scanNo, moduleNo));
                    CheckImResult(moduleNo);
                }

                return true;
            });
        }

        private static void CheckImResult(int moduleNo)
        {
            if (ResultListDic[moduleNo].Count == 0)
            {
                return;
            }

            IOrderedEnumerable<KeyValuePair<int, ResultSyncData>> dic = ResultListDic[moduleNo].OrderBy(x => x.Key);
            int firstScanNo = dic.First().Key;
            ResultListDic[moduleNo].TryRemove(firstScanNo, out ResultSyncData data);

            // 하나라도 NG 라면 true => 1 => NG
            if (data != null && data.IsNG == true)
            {
                double signalDistanceM = SystemConfig.Instance.SignalDistanceM;
                if (signalDistanceM != 0)
                {
                    float lineSpeedPerMinute = SystemConfig.Instance.LineSpeed;
                    //var xData = new double[3] { 10.0, 25.0, 110.0 };
                    //var yData = new double[3] { 0, 0, 0 };
                    //var polyFit = new PolyFit<double>(xData, yData, 1);
                    //var additonalDistance = (polyFit.Coeff[1] * lineSpeedPerMinute) + (polyFit.Coeff[0]);

                    //      거리        = 시간
                    // 분당 속도 / 60초
                    //var HalfofPageSec = ((SystemConfig.Instance.InspectionFrameLengthmm / 2.0) / (SystemConfig.Instance.LineSpeed * 1000.0 / 60.0));
                    data.Time += TimeSpan.FromSeconds(((signalDistanceM/* + additonalDistance*/) / (lineSpeedPerMinute / 60.0))/* - HalfofPageSec*/);
                }

                double signalTimeDelayMs = SystemConfig.Instance.SignalTimeDelayMs;
                if (signalTimeDelayMs != 0)
                {
                    data.Time += TimeSpan.FromMilliseconds(signalTimeDelayMs);
                }

                ResultSyncDataQueueDic[data.ModuleNo].Enqueue(new ResultSyncData(data.Time, data.IsNG, data.ScanNo, data.ModuleNo));
            }
        }

        // 검사 시작 시 동시 시작
        public static void StartResultSyncTask()
        {
            LastSignalTimeDic.Clear();
            PulseTaskDic.Clear();
            ResultSyncDataQueueDic.Clear();
            ResultListDic.Clear();

            foreach (ModuleInfo module in SystemConfig.Instance.ModuleList.ToList())
            {
                ResultListDic.Add(module.ModuleNo, new ConcurrentDictionary<int, ResultSyncData>());
                ResultSyncDataQueueDic.Add(module.ModuleNo, new ConcurrentQueue<ResultSyncData>());
                PulseTaskDic.Add(module.ModuleNo, null);
                LastSignalTimeDic.Add(module.ModuleNo, DateTime.Now);
            }

            DefectSignalThread = new Thread(StartResultSyncAction);
            DefectSignalThread.IsBackground = true;
            DefectSignalThread.Priority = ThreadPriority.Highest;
            DefectSignalThread.Start();
        }

        private static void StartResultSyncAction()
        {
            TokenSource = new CancellationTokenSource();

            var resultSyncDataDic = new Dictionary<int, ResultSyncData>();
            foreach (ModuleInfo module in SystemConfig.Instance.ModuleList.ToList())
            {
                resultSyncDataDic.Add(module.ModuleNo, null);
            }

            SystemConfig config = SystemConfig.Instance;
            // 중복 동작을 막기위한 시간 딜레이 값
            double nonOverlayTimeMs = 0;
            switch (config.DefectSignalNonOverlayMode)
            {
                case DefectSignalNonOverlayMode.Time: nonOverlayTimeMs += config.SignalNonOverlayTimeMs; break;
                case DefectSignalNonOverlayMode.Distance: nonOverlayTimeMs += config.SignalNonOverlayDistanceM / config.LineSpeed * 60 * 1000; break;
                default: nonOverlayTimeMs += config.SignalNonOverlayTimeMs; break;
            }

            while (!TokenSource.IsCancellationRequested)
            {
                foreach (ModuleInfo module in SystemConfig.Instance.ModuleList.ToList())
                {
                    if (resultSyncDataDic[module.ModuleNo] == null)
                    {
                        ResultSyncDataQueueDic[module.ModuleNo].TryDequeue(out ResultSyncData resultSyncData);
                        resultSyncDataDic[module.ModuleNo] = resultSyncData;
                    }

                    if (resultSyncDataDic[module.ModuleNo] != null && DateTime.Now >= resultSyncDataDic[module.ModuleNo].Time)
                    {
                        // 마지막 끝나는 시점의 신호보다 현재 신호가 더 나중 신호면 진행한다.
                        if (PulseTaskDic[module.ModuleNo] == null && resultSyncDataDic[module.ModuleNo].Time >= LastSignalTimeDic[module.ModuleNo])
                        {
                            // 중복이 안되도록 신호가 끝나는 시점을 가지고 있는다.
                            LastSignalTimeDic[module.ModuleNo] = resultSyncDataDic[module.ModuleNo].Time.AddMilliseconds(nonOverlayTimeMs);
                            PulseTaskDic[module.ModuleNo] = Task.Run(() =>
                            {
                                // IO 신호 주기
                                var deviceManager = Override.DeviceManager.Instance() as Override.DeviceManager;
                                deviceManager.SendDefectSignal(module.ModuleNo, true);
                                Thread.Sleep(config.SignalDuration);
                                deviceManager.SendDefectSignal(module.ModuleNo, false);
                                PulseTaskDic[module.ModuleNo] = null;
                            });
                        }

                        resultSyncDataDic[module.ModuleNo] = null;
                    }
                }

                Thread.Sleep(10);
            }
        }

        public static void StopResultSyncTask()
        {
            TokenSource?.Cancel();

            foreach (ModuleInfo module in SystemConfig.Instance.ModuleList.ToList())
            {
                if (ResultSyncDataQueueDic[module.ModuleNo] != null)
                {
                    while (ResultSyncDataQueueDic[module.ModuleNo].Count > 0)
                    {
                        ResultSyncDataQueueDic[module.ModuleNo].TryDequeue(out ResultSyncData temp);
                    }
                }

                if (ResultListDic[module.ModuleNo] != null)
                {
                    ResultListDic[module.ModuleNo].Clear();
                }
            }

            LastSignalTimeDic.Clear();
            PulseTaskDic.Clear();
            ResultSyncDataQueueDic.Clear();
            ResultListDic.Clear();
        }
    }
}
