using DynMvp.Base;
using DynMvp.InspectData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base.Data;
using UniScanC.Data;
using WPF.UniScanCM.Override;

namespace WPF.UniScanCM.Events
{
    public delegate void UpdateResultDelegate(IEnumerable<ProductResult> productResults, CancellationTokenSource taskCancelToken = null);
    public delegate void VoidDelegate();

    public class InspectEventListener
    {
        private static InspectEventListener _instance;
        public static InspectEventListener Instance => (_instance ?? (_instance = new InspectEventListener()));

        public UpdateResultDelegate UpdateResultDelegate { get; set; }

        // 불량 번호 재 부여를 위한 변수
        private int DefectNo { get; set; } = 1;

        // 최근에 업데이트 된 데이터만 읽어오기 위한 변수
        private Dictionary<string, int> VisionFrameindexDic { get; set; } = new Dictionary<string, int>();
        private int ThicknessTraverseIndex { get; set; } = -1;
        private int GlossTraverseIndex { get; set; } = -1;

        private InspectDataImporter VisionDataImporter { get; set; }
        private Task VisionTask { get; set; }

        private ThicknessDataImporter ThicknessDataImporter { get; set; }
        private Task ThicknessTask { get; set; }

        private GlossDataImporter GlossDataImporter { get; set; }
        private Task GlossTask { get; set; }

        private CancellationTokenSource TaskCancelToken { get; set; }

        public InspectEventListener()
        {
            SystemConfig config = SystemConfig.Instance;

            if (config.UseInspectModule)
            {
                VisionDataImporter = new InspectDataImporter();
                VisionDataImporter.SetDataBaseInfo(config.DatabaseIpAddress, config.DatabaseName, config.DatabaseUserName, config.DatabasePassword);
            }
            if (config.UseThicknessModule)
            {
                ThicknessDataImporter = new ThicknessDataImporter();
                ThicknessDataImporter.SetDataBaseInfo(config.DatabaseIpAddress, config.DatabaseName, config.DatabaseUserName, config.DatabasePassword);
            }
            if (config.UseGlossModule)
            {
                GlossDataImporter = new GlossDataImporter();
                GlossDataImporter.SetDataBaseInfo(config.DatabaseIpAddress, config.DatabaseName, config.DatabaseUserName, config.DatabasePassword);
            }
        }

        public void StartResultReader(string lotName, int startFrameNo = 0)
        {
            LogHelper.Debug(DynMvp.Base.LoggerType.Inspection, "InspectEventListener::StartResultReader - Start");

            // 데이터 초기화를 위해서 Null 보냄
            UpdateResultDelegate(null);
            TaskCancelToken = new CancellationTokenSource();

            DefectNo = 0;

            VisionFrameindexDic.Clear();
            foreach (UniScanC.Module.InspectModuleInfo im in SystemConfig.Instance.ImModuleList)
            {
                VisionFrameindexDic.Add(im.ModuleTopic, startFrameNo);
            }
            ThicknessTraverseIndex = startFrameNo;
            GlossTraverseIndex = startFrameNo;

            SystemConfig config = SystemConfig.Instance;
            if (config.UseInspectModule)
            {
                if (VisionTask == null)
                {
                    VisionTask = ReadVisionResultProc(lotName, TaskCancelToken);
                }
            }

            if (config.UseThicknessModule)
            {
                if (ThicknessTask == null)
                {
                    ThicknessTask = ReadThicknessResultProc(lotName, TaskCancelToken);
                }
            }

            if (config.UseGlossModule)
            {
                if (GlossTask == null)
                {
                    GlossTask = ReadGlossResultProc(lotName, TaskCancelToken);
                }
            }

            LogHelper.Debug(DynMvp.Base.LoggerType.Inspection, "InspectEventListener::StartResultReader - End");
        }

        public async Task StopResultReader()
        {
            await Task.Run(() =>
            {
                TaskCancelToken?.Cancel();

                SystemConfig config = SystemConfig.Instance;
                while (config.UseInspectModule && VisionTask != null)
                {
                    Thread.Sleep(100);
                }
                while (config.UseThicknessModule && ThicknessTask != null)
                {
                    Thread.Sleep(100);
                }
                while (config.UseGlossModule && GlossTask != null)
                {
                    Thread.Sleep(100);
                }
            });
        }

        private Task ReadVisionResultProc(string lotName, CancellationTokenSource cancellation)
        {
            LogHelper.Debug(LoggerType.Inspection, "InspectEventListener::ReadVisionResultProc - Start");
            return Task.Factory.StartNew(() =>
            {
                //int index = 0; // Test Code
                while (SystemState.Instance().OpState != OpState.Idle)
                {
                    try
                    {
                        if (TaskCancelToken.IsCancellationRequested)
                        {
                            VisionTask = null;
                            break;
                        }

                        // Test Code
                        //List<InspectResult> results = new List<InspectResult>();
                        //InspectResult result = new InspectResult();
                        //result.RepeatCount = index;
                        //Defect defect = new Defect();
                        //defect.DefectNo = index++;
                        //result.AddDefect(defect);
                        //results.Add(result);

                        List<InspectResult> results = ReadVisionResult(lotName);
                        if (results != null && results.Count() > 0)
                        {
                            UpdateResultDelegate(results, TaskCancelToken);
                            Thread.Sleep(SystemConfig.Instance.UIUpdateDelay);
                        }
                        else
                        {
                            Thread.Sleep(100);
                        }
                    }
                    catch (Exception e)
                    {
                        LogHelper.Debug(LoggerType.Comm, "InspectEventListener::ReadVisionResultProc - Error :" + e.Message);
                    }
                }
                VisionTask = null;
            }, TaskCreationOptions.LongRunning);
        }

        private List<InspectResult> ReadVisionResult(string lotName)
        {
            LogHelper.Debug(LoggerType.Inspection, "InspectEventListener::ReadVisionResult - Start");
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            var resultList = new List<InspectResult>();
            var filterdFrameDatas = new List<Dictionary<string, object>>();

            foreach (UniScanC.Module.InspectModuleInfo im in SystemConfig.Instance.ImModuleList)
            {
                int lastRowIndex = VisionFrameindexDic[im.ModuleTopic];
                int updateRowIndex = VisionDataImporter.ImportModuleMaxRowIndex(lotName, im.ModuleNo.ToString());
                if (updateRowIndex == -1 || updateRowIndex <= lastRowIndex)
                {
                    continue;
                }
                else
                {
                    filterdFrameDatas.AddRange(VisionDataImporter.ImportUpdatedModuleFrameData(lotName, im.ModuleNo.ToString(), lastRowIndex, updateRowIndex));
                    VisionFrameindexDic[im.ModuleTopic] = updateRowIndex;
                }
            }

            //var lastRowIndex = VisionFrameIndex + 1;

            //// 마지막 데이터 이후로 추가된 데이터가 있는지 확인
            //// 업데이트가 없다면 비어있는 리스트 리턴
            //// updateRowIndex는 RowCount 값과 동일
            //var updateRowIndex = VisionDataImporter.ImportMaxRowIndex(lotName);
            //if (updateRowIndex == -1 || updateRowIndex <= lastRowIndex)
            //{
            //    return resultList;
            //}
            //// 마지막 번호부터 업데이트된 번호까지 데이터를 읽음
            //totalFrameDatas = VisionDataImporter.ImportUpdatedFrameData(lotName, lastRowIndex, updateRowIndex);
            ////System.Diagnostics.Debug.WriteLine($"LastFrame: {lastFrameIndex} / UpdatedFrame: {updateFrameIndex}");
            //for (var i = lastRowIndex / imCount; i <= updateRowIndex / imCount; i++)
            //{
            //    //IM들의 데이터가 모두다 모인 리스트만 모아서 필터링 진행
            //    var pair = totalFrameDatas.FindAll(x => Convert.ToInt32(x["frame_index"]) == i);
            //    if (pair.Count < imCount)
            //    {
            //        FrameTimeOutCount++;
            //        if (FrameTimeOutCount > FrameTimeOutMaxCount)
            //        {
            //            VisionFrameIndex++;
            //        }
            //        break;
            //    }
            //    else
            //    {
            //        FrameTimeOutCount = 0;
            //        VisionFrameIndex = i;
            //        filterdFrameDatas.AddRange(pair);
            //    }
            //}

            if (filterdFrameDatas.Count == 0)
            {
                LogHelper.Debug(LoggerType.Inspection, $"InspectEventListener::ReadVisionResult - filterdFrameDatas Count is 0");
                LogHelper.Debug(LoggerType.Inspection, $"InspectEventListener::ReadVisionResult - End [{sw.ElapsedMilliseconds}ms]");
                sw.Stop();
                return resultList;
            }
            else
            {
                resultList = InspectResult.Parse(VisionDataImporter, filterdFrameDatas, DefectNo + 1, true, true);
            }

            foreach (InspectResult result in resultList)
            {
                DefectNo += result.DefectList.Count;
            }

            LogHelper.Debug(LoggerType.Inspection, $"InspectEventListener::ReadVisionResult - End [{sw.ElapsedMilliseconds}ms]");
            sw.Stop();

            return resultList;
        }

        private Task ReadThicknessResultProc(string lotName, CancellationTokenSource cancellation)
        {
            LogHelper.Debug(LoggerType.Inspection, "InspectEventListener::ReadThicknessResultProc - Start");
            return Task.Factory.StartNew(() =>
            {
                while (SystemState.Instance().OpState != OpState.Idle)
                {
                    try
                    {
                        if (TaskCancelToken.IsCancellationRequested)
                        {
                            ThicknessTask = null;
                            break;
                        }

                        List<ThicknessResult> results = ReadThicknessResult(lotName);
                        if (results != null && results.Count() > 0)
                        {
                            UpdateResultDelegate(results, TaskCancelToken);
                            Thread.Sleep(SystemConfig.Instance.UIUpdateDelay);
                        }
                    }
                    catch (Exception e)
                    {
                        LogHelper.Debug(LoggerType.Comm, "InspectEventListener::ReadThicknessResultProc - Error :" + e.Message);
                    }
                }
                ThicknessTask = null;
            }, TaskCreationOptions.LongRunning);
        }

        private List<ThicknessResult> ReadThicknessResult(string lotName)
        {
            var resultList = new List<ThicknessResult>();
            List<Dictionary<string, object>> totalTraverseDatas = null;
            int lastRowIndex = ThicknessTraverseIndex;

            // 마지막 데이터 이후로 추가된 데이터가 있는지 확인
            // 업데이트가 없다면 비어있는 리스트 리턴
            int updateRowIndex = ThicknessDataImporter.ImportMaxRowIndex(lotName);
            if (updateRowIndex == -1 || updateRowIndex <= lastRowIndex)
            {
                return resultList;
            }
            else
            {
                ThicknessTraverseIndex = updateRowIndex;
            }
            //마지막 번호부터 업데이트된 번호까지 데이터를 읽음
            totalTraverseDatas = ThicknessDataImporter.ImportUpdatedTraverseData(lotName, lastRowIndex, updateRowIndex);
            // 두께 데이터로 만들어서 리턴
            resultList = ThicknessResult.Parse(ThicknessDataImporter, totalTraverseDatas);

            return resultList;
        }

        private Task ReadGlossResultProc(string lotName, CancellationTokenSource cancellation)
        {
            LogHelper.Debug(LoggerType.Inspection, "InspectEventListener::ReadGlossResultProc - Start");
            return Task.Factory.StartNew(() =>
            {
                while (SystemState.Instance().OpState != OpState.Idle)
                {
                    try
                    {
                        if (TaskCancelToken.IsCancellationRequested)
                        {
                            GlossTask = null;
                            break;
                        }

                        List<GlossResult> results = ReadGlossResult(lotName);
                        if (results != null && results.Count() > 0)
                        {
                            UpdateResultDelegate(results, TaskCancelToken);
                            Thread.Sleep(SystemConfig.Instance.UIUpdateDelay);
                        }
                    }
                    catch (Exception e)
                    {
                        LogHelper.Debug(LoggerType.Comm, "InspectEventListener::ReadGlossResultProc - Error :" + e.Message);
                    }
                }
                GlossTask = null;
            }, TaskCreationOptions.LongRunning);
        }

        private List<GlossResult> ReadGlossResult(string lotName)
        {
            var resultList = new List<GlossResult>();
            List<Dictionary<string, object>> totalTraverseDatas = null;
            int lastRowIndex = GlossTraverseIndex;

            // 마지막 데이터 이후로 추가된 데이터가 있는지 확인
            // 업데이트가 없다면 비어있는 리스트 리턴
            int updateRowIndex = GlossDataImporter.ImportMaxRowIndex(lotName);
            if (updateRowIndex == -1 || updateRowIndex <= lastRowIndex)
            {
                return resultList;
            }
            else
            {
                GlossTraverseIndex = updateRowIndex;
            }
            //마지막 번호부터 업데이트된 번호까지 데이터를 읽음
            totalTraverseDatas = GlossDataImporter.ImportUpdatedTraverseData(lotName, lastRowIndex, updateRowIndex);
            // 광택도 데이터로 만들어서 리턴
            resultList = GlossResult.Parse(GlossDataImporter, totalTraverseDatas);

            return resultList;
        }
    }
}
