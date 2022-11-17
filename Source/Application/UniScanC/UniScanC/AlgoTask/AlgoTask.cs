using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using UniScanC.Algorithm;
using UniScanC.Algorithm.Base;
using UniScanC.Data;
using UniScanC.Struct;

namespace UniScanC.AlgoTask
{
    public delegate void OnInspectedDelegate(IAlgoTask inspectUnit, (InspectBufferSet, InspectResult) qItem, bool success);

    public interface IAlgoTask
    {
        event OnInspectedDelegate OnInspected;

        string Name { get; }
        int Index { get; }
        bool IsStopRequested { get; }
        bool IsRunning { get; }

        IAlgorithmBase UnitAlgorithmBase { get; }

        void AddLink(ILink[] links);

        bool Enqueue((InspectBufferSet, InspectResult) item);
        void Start();
        void Stop();
        void Wait();
    }

    public static class AlgoTaskFactory
    {
        public static IAlgoTask Create(int index, INode algorithm, InspectBufferPool bufferPool)
        {
            Type inputType = algorithm.GetInputType();
            Type outputType = algorithm.GetOutputType();

            Type contextType = typeof(AlgoTask<,>).MakeGenericType(inputType, outputType);
            var algoTask = (IAlgoTask)Activator.CreateInstance(contextType, index, algorithm, bufferPool);
            return algoTask;
        }
    }

    internal class AlgoTask<Tin, Tout> : IAlgoTask
        where Tin : InputOutputs, new()
        where Tout : InputOutputs, IResultBufferItem, TupleElement, new()
    {
        public event OnInspectedDelegate OnInspected;
        public string Name => unitAlgorithmBase.Param.Name;

        public int Index { get; }

        public bool IsStopRequested => threadHandler == null ? true : threadHandler.RequestStop;

        public bool IsRunning { get; private set; }

        public bool IsProcessing { get; private set; }

        public IAlgorithmBase UnitAlgorithmBase => unitAlgorithmBase;

        private AlgorithmBase<Tin, Tout> unitAlgorithmBase;
        private List<ILink> linkList;
        private InspectBufferPool bufferPool;
        private ThreadHandler threadHandler;
        private AlgoImage[] workingBuffers;
        private ConcurrentQueue<(InspectBufferSet, InspectResult)> queue;

        public AlgoTask(int index, AlgorithmBase<Tin, Tout> unitAlgorithmBase, InspectBufferPool bufferPool)
        {
            Index = index;
            this.unitAlgorithmBase = unitAlgorithmBase;
            this.bufferPool = bufferPool;

            linkList = new List<ILink>();
            IsRunning = false;
        }

        public void AddLink(ILink[] links)
        {
            //Tout tout = new Tout();
            //Array.ForEach(links, f =>
            //{
            //    if (f is LinkS)
            //        f.DstPort = tout.GetPropIndex(((LinkS)f).DstPortName);
            //});
            linkList.AddRange(links);
        }

        public void Start()
        {
            LogHelper.Debug(LoggerType.Inspection, $"InspectUnit::Start - Name: {Name}");
            workingBuffers = unitAlgorithmBase.BuildBuffer();

            queue = new ConcurrentQueue<(InspectBufferSet, InspectResult)>();
            threadHandler = new ThreadHandler(Name, new System.Threading.Thread(ThreadProc), false);
            threadHandler.Start();
        }

        public void Stop()
        {
            LogHelper.Debug(LoggerType.Inspection, $"InspectUnit::Stop - Name: {Name}");
            threadHandler.Stop();

            while (queue.TryDequeue(out (InspectBufferSet, InspectResult) qItem))
            {
                qItem.Item1.ReturnAll(bufferPool);
            }

            if (workingBuffers != null)
            {
                Array.ForEach(workingBuffers, f => f.Dispose());
            }

            workingBuffers = null;
        }

        public void Wait()
        {
            while (IsProcessing || queue.Count > 0)
            {
                Thread.Sleep(10);
            }
        }

        public bool Enqueue((InspectBufferSet, InspectResult) item)
        {
            LogHelper.Debug(LoggerType.Inspection, $"AlgoTask[{Name}]::Enqueue - FrameNo: {item.Item1.FrameNo}, Q Size: {queue.Count}");

            bool enq = IsRunning && !IsStopRequested;
            if (enq)
            {
                queue.Enqueue(item);
            }

            return enq;
        }

        private Tin GetInputs(InspectBufferSet inspectBufferSet)
        {
            var inputs = new Tin();
            (string, Type)[] nameTypes = inputs.GetPropNameTypes();

            linkList.ForEach(f =>
            {
                (Type, object) data = f.GetData(inspectBufferSet);
                f.SetData(inputs, data.Item1, data.Item2);
            });

            //for (int i = 0; i < nameTypes.Length; i++)
            //{
            //    ILink link = this.linkList.Find(f => (f.IsDestination(this) && (f.DstPort == i));
            //    if (link != null)
            //    {
            //        object data = link.GetData(inspectBufferSet);
            //        inputs.SetValue(i, data);
            //    }
            //}

            return inputs;
        }

        // Output을 요청할 때 버퍼에서 가져오기...?
        private Tout GetOutputs(InspectBufferSet inspectBufferSet)
        {
            return (Tout)inspectBufferSet.GetTaskResult(Name);
        }

        private void SetOutputResult(Tout outputResult, InspectBufferSet inspectBufferSet)
        {
            inspectBufferSet.SetTaskResult(Name, outputResult);
        }

        private void ThreadProc()
        {
            IsRunning = true;
            var inspectStopWatch = new Stopwatch();
            var waitStopWatch = new Stopwatch();

            while (!threadHandler.RequestStop || queue.Count > 0)
            {
                if (!queue.TryDequeue(out (InspectBufferSet, InspectResult) qItem))
                {
                    IsProcessing = false;
                    Thread.Sleep(10);
                    continue;
                }

                (InspectBufferSet inspectBufferSet, InspectResult inspectResult) = qItem;

                LogHelper.Debug(LoggerType.Inspection, $"AlgoTask::ThreadProc({Name}) Start - FrameIndex: {qItem.Item2.FrameIndex}");
                waitStopWatch.Stop();
                inspectStopWatch.Restart();
                bool isSuccess = false;
                try
                {
                    IsProcessing = true;
                    Tin inputParam = GetInputs(inspectBufferSet);
                    Tout outputResult = GetOutputs(inspectBufferSet);
                    isSuccess = unitAlgorithmBase.Run(inputParam, ref outputResult, workingBuffers);

                    if (isSuccess)
                    {
                        SetOutputResult(outputResult, inspectBufferSet);
                    }
                }
                catch (Exception ex)
                {
                    do
                    {
                        LogHelper.Error(LoggerType.Inspection, $"AlgoTask::ThreadProc({Name}) - {ex.GetType().Name}: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
                    } while ((ex = ex.InnerException) != null);
                }
                finally
                {
                    inspectStopWatch.Stop();
                    inspectResult.InspectEndTime = DateTime.Now;
                    double loadFactor = inspectStopWatch.Elapsed.TotalMilliseconds / (inspectStopWatch.Elapsed.TotalMilliseconds + waitStopWatch.Elapsed.TotalMilliseconds) * 100f;
                    inspectResult.LoadFactorList.Add(Name, loadFactor);
                    LogHelper.Debug(LoggerType.Inspection, $"AlgoTask::ThreadProc({Name}) End. " +
                        $"Run:{inspectStopWatch.Elapsed.TotalMilliseconds}[ms] / Wait:{waitStopWatch.Elapsed.TotalMilliseconds}[ms] / Load Factor:{loadFactor}%");
                    OnInspected?.Invoke(this, qItem, isSuccess);
                    waitStopWatch.Restart();
                }
            }

            IsProcessing = false;
            IsRunning = false;
        }
    }
}
