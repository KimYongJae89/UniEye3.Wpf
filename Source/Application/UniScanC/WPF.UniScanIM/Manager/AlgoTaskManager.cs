using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UniEye.Base.Config;
using UniScanC.Algorithm;
using UniScanC.Algorithm.Base;
using UniScanC.AlgoTask;
using UniScanC.Data;
using UniScanC.Models;
using UniScanC.Struct;
using WPF.UniScanIM.Override;

namespace WPF.UniScanIM.Manager
{
    public class AlgoTaskManager
    {
        public event OnInspectCompletedDelegate OnInspectCompleted;

        public int MinBufferCnt => algoTasks.Length * 2 + algoTasks.Sum(f => f.UnitAlgorithmBase.RequiredBufferCount);
        public int MultipleReservedBuffer => 2;

        public bool IsRunning { get; private set; } = false;
        public ModuleInfo ModuleInfo { get; private set; }

        private int DebugContextSaveCnt { get; set; } = 0;

        private List<DefectGroup> onBorderDefectGroupList;
        private InspectBufferPool bufferPool;
        private IAlgoTask[] algoTasks;
        private ILink[] resultLinks;

        public AlgoTaskManager(ModuleInfo moduleInfo, AlgoTaskManagerSetting setting, VisionModel visionModel)
        {
            LogHelper.Debug(LoggerType.Grab, $"AlgoTaskManager::AlgoTaskManager - ModuleID: {moduleInfo.ModuleNo}, BufferWidth: {moduleInfo.BufferWidth}, BufferHeight: {moduleInfo.BufferHeight}");
            onBorderDefectGroupList = new List<DefectGroup>();
            bufferPool = new InspectBufferPool(moduleInfo.BufferSize);
            ModuleInfo = moduleInfo;
            algoTasks = setting.ParamList.Select((f, i) =>
            {
                if (f is IAlgorithmBaseParam)
                {
                    ((IAlgorithmBaseParam)f).SetVisionModel(visionModel);
                }

                INode algorithm = f.BuildNode(moduleInfo);
                IAlgoTask algoTask = AlgoTaskFactory.Create(i, algorithm, bufferPool);

                ILink[] dstLinks = setting.LinkList.FindAll(g => g.IsDestination(algoTask)).ToArray();
                algoTask.AddLink(dstLinks);

                algoTask.OnInspected += OnInspected;
                return algoTask;
            }).ToArray();

            resultLinks = setting.LinkList.FindAll(f => f.IsResult()).ToArray();
        }

        public bool Enqueue((InspectBufferSet, InspectResult) item)
        {
            InspectBufferSet inspectBufferSet = item.Item1;

            // Build BufferSet Struct
            Array.ForEach(algoTasks, f =>
            {
                Type type = f.UnitAlgorithmBase.GetOutputType();
                var outputs = (IResultBufferItem)Activator.CreateInstance(type);
                inspectBufferSet.SetTaskResult(f.Name, outputs);
            });

            // Get Buffer From Pool
            if (!inspectBufferSet.Request(bufferPool))
            {
                LogHelper.Error(LoggerType.Inspection, $"AlgoTaskManager::Enqueue - Request fail.");
                return false;
            }

            if (!algoTasks[0].Enqueue(item))
            {
                inspectBufferSet.ReturnAll(bufferPool);
                return false;
            }

            return true;
        }

        public void Start()
        {
            bufferPool.Alloc(MinBufferCnt * MultipleReservedBuffer, MinBufferCnt, OperationConfig.Instance().ImagingLibrary);
            Array.ForEach(algoTasks, f => f.Start());
            IsRunning = true;
            DebugContextSaveCnt = 0;
        }

        public void Stop()
        {
            Array.ForEach(algoTasks, f => f.Stop());
            bufferPool.Dispose();
            IsRunning = false;
            DebugContextSaveCnt = 0;
        }

        public byte[] RequestBytesBuffer()
        {
            return bufferPool.RequestBytesBuffer();
        }

        public void ReturnBytesBuffer(byte[] bytes)
        {
            bufferPool.ReturnBytesBuffer(bytes);
        }

        private void OnInspected(IAlgoTask algoTask, (InspectBufferSet, InspectResult) qItem, bool success)
        {
            // 다음 단계 실행 조건:
            // 이전단계 실행 성공 and 다음 단계 있음 and 다음 단계 실행 중.
            // 다음 단계 실행 안하면 완료로 빠짐.
            IAlgoTask nextTask = null;
            if (success)
            // 직전 단계 실행 성공.
            {
                int idx = Array.IndexOf(algoTasks, algoTask);
                if (idx < algoTasks.Length - 1)
                // 다음 단계 존재.
                {
                    nextTask = algoTasks[idx + 1];
                    if (!nextTask.IsRunning || nextTask.IsStopRequested)
                    // 다음 단계 실행 불가시 취소.
                    {
                        nextTask = null;
                        success = false;
                    }
                }
            }

            bool done = false;
            if (nextTask != null) // 다음 단계 존재 -> 실행
            {
                done = nextTask.Enqueue(qItem);
            }

            if (!done) // 다음 단계 없음 or 실패 -> 완료
            {
                InspectComplete(qItem.Item1, qItem.Item2, success);
            }
        }

        public void WaitAllCompleted()
        {
            Array.ForEach(algoTasks, f => f.Wait());
        }

        private void UpdateInspectResult(InspectBufferSet inspectBufferSet, InspectResult inspectResult, bool success)
        {
            LogHelper.Debug(LoggerType.Inspection, $"AlgoTaskManager::UpdateInspectResult - Success is {success}");
            if (!success)
            {
                inspectResult.Judgment = Judgment.Skip;
            }

            Array.ForEach(resultLinks, f =>
            {
                (Type, object) data = f.GetData(inspectBufferSet);
                f.SetData(inspectResult, data.Item1, data.Item2);
            });
        }

        private void InspectComplete(InspectBufferSet inspectBufferSet, InspectResult inspectResult, bool success)
        {
            if (inspectBufferSet != null)
            {
                if (success)
                {
                    if (inspectResult.FrameIndex >= 0 && Override.SystemConfig.Instance.MergeBorderDefects)
                    {
                        MergeDefects(inspectBufferSet); // 불량 합체.
                    }
                }

                UpdateInspectResult(inspectBufferSet, inspectResult, success);
            }

            inspectResult.UpdateJudgement();
            inspectResult.EndTime = inspectResult.InspectEndTime.ToString("yyyy-MM-dd HH:mm:ss");

            LogHelper.Debug(LoggerType.Inspection, $"AlgoTaskManager::InspectComplete - FrameNo: {inspectResult.FrameIndex}" +
                $"/ Time Span: {inspectResult.InspectTimeSpan.TotalMilliseconds}ms ");

            // 디버그 저장 모드이면 버퍼 내용을 저장함
            if (SystemConfig.Instance.IsSaveDebugData && (SystemConfig.Instance.IsSaveDebugDataCount > DebugContextSaveCnt))
            {
                string debugPath = Path.Combine(PathConfig.Instance().Temp, ModuleInfo.Topic);
                if (!Directory.Exists(debugPath))
                {
                    Directory.CreateDirectory(debugPath);
                }

                inspectBufferSet?.Save(debugPath);

                string path = Path.Combine(PathConfig.Instance().Temp, "Completed");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (inspectResult.DefectImageData != null)
                {
                    UniScanC.Inspect.BmpImaging.SaveBitmapSource(inspectResult.DefectImageData, Path.Combine(path, $"{inspectResult.ModuleNo}_{inspectResult.FrameIndex}.bmp"));
                }

                ++DebugContextSaveCnt;
            }

            if (SystemConfig.Instance.IsSaveDebugDataCount == DebugContextSaveCnt)
            {
                SystemConfig.Instance.IsSaveDebugData = false;
                DebugContextSaveCnt = 0;
            }

            inspectBufferSet?.ReturnAll(bufferPool);

            Task.Run(() => OnInspectCompleted?.Invoke(inspectResult));
        }

        private void MergeDefects(InspectBufferSet inspectBufferSet)
        {
            // 합체 할 조각들 구하기 - 이미지 경계에 닿은 불량들.
            List<Defect> onBorderDefectList = inspectBufferSet.DefectList.FindAll(f => f.IsOnBorder).FindAll(x => x.DefectTypeName != "COLOR");

            // 조각을 사용하여 불량 합체
            Defect[] overframeDefectList = GetOverframeDefects(onBorderDefectList);

            // 합체된 불량을 이번 프레임의 결과로 출력.
            inspectBufferSet.DefectList.AddRange(overframeDefectList);

            // 합체에 사용된 조각을 이번 프레임 결과로 출력할 것인가??
            //inspectBufferSet.DefectList.RemoveAll(f => onBorderDefectList.Contains(f));
        }

        private Defect[] GetOverframeDefects(List<Defect> onBorderDefectList)
        {
            // 현 결과들 중 프레임 상단에 접한 불량 (1)
            List<Defect> currentUpperDefectList = onBorderDefectList.FindAll(f => f.IsOnUpperBorder);

            // 현 결과들 중 프레임 하단에'만' 접한 불량 (2)
            var currentLowerDefectList = onBorderDefectList.Except(currentUpperDefectList).ToList();

            // (1)과 붙어있는 기존 불량 찾기.
            var joinDefectGroupList = currentUpperDefectList.ToDictionary(f => f, f => onBorderDefectGroupList.FindAll(g =>
                {
                    // 전 결과의 오른쪽에 접한 길이
                    float r = g.First().TouchTailRect.Right - f.TouchHeadRect.Left;

                    // 전 결과의 오른쪽에 접한 길이
                    float l = f.TouchHeadRect.Right - g.First().TouchTailRect.Left;

                    // 접한 길이가 0보다 크다. -> 붙어있다.
                    return (r > 0 && l > 0);
                }).ToArray());

            // (1)과 찾은 불량을 머지하여 DefectGroup 생성.
            var defectGroupTemp = joinDefectGroupList.Select(f =>
            {
                var dg = new DefectGroup(f.Key);
                if (f.Value.Length > 0)
                {
                    dg.AddRange(f.Value);
                    onBorderDefectGroupList.RemoveAll(g => f.Value.Contains(g)); // 기존 보관 목록 중 연결되지 않은 것만 남김.
                }
                return dg;
            }).ToList();

            var defectGroupList = new List<DefectGroup>();
            while (defectGroupTemp.Count > 0)
            {
                DefectGroup defectGroup = defectGroupTemp[0];
                defectGroupTemp.RemoveAt(0);

                List<DefectGroup> ll = defectGroupTemp.FindAll(f => f.IsIntersect(defectGroup));
                defectGroupTemp.RemoveAll(f => ll.Contains(f));
                defectGroup.AddRange(ll);

                defectGroupList.Add(defectGroup);
            }

            // 생성된 DefectGroup 중 현재 프레임 하단에 붙은것과 붙지 않은 것으로 분리.
            var groupList = defectGroupList.GroupBy(f => f.First().IsOnLowerBorder).ToList();

            // 현재 프레임 하단에 붙지 않은 불량과, 기존 보관 불량 중 연결이 끊어진 것은 내보내기.
            var noTouchedList = groupList.FindAll(f => !f.Key).SelectMany(f => f).ToList();
            noTouchedList.AddRange(onBorderDefectGroupList);
            Defect[] defects = noTouchedList.ConvertAll(f => f.ToDefect()).ToArray();

            // 바닥에 붙은 불량과, (2)는 보관.
            var touchedBottomList = groupList.FindAll(f => f.Key).SelectMany(f => f).ToList();
            touchedBottomList.AddRange(currentLowerDefectList.ConvertAll(f => new DefectGroup(f)));
            onBorderDefectGroupList.Clear();
            onBorderDefectGroupList.AddRange(touchedBottomList);

            return defects;
        }
    }
}
