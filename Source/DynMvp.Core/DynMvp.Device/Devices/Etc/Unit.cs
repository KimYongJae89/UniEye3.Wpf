using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.Dio;
using DynMvp.Devices.MotionController;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace DynMvp.Devices
{
    public class UnitState
    {
        public enum EOpMode
        {
            Manual, Auto, SemiAuto, Pass, DryRun
        }

        public enum ERunMode
        {
            Start, Stop, Run, CycleEnd, Error, Reset
        }
        public EOpMode OpMode { get; set; }
        public ERunMode RunMode { get; set; }
    }

    public enum UnitError
    {
        MoveTo
    }

    public abstract class UnitManager
    {
        protected UnitState unitState = new UnitState();
        public UnitState UnitState
        {
            get => unitState;
            set => unitState = value;
        }

        protected List<Unit> unitList = new List<Unit>();
        public List<Unit> UnitList
        {
            get => unitList;
            set => unitList = value;
        }

        private string lastFileName;
        private CancellationTokenSource runProcessCancellationTokenSource;

        public virtual void Start()
        {
            Started = true;

            try
            {
                runProcessCancellationTokenSource = new CancellationTokenSource();
                ActionDoneChecker.StopDoneChecker = false;

                foreach (Unit unit in unitList)
                {
                    unit.Start(runProcessCancellationTokenSource.Token);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        public void Stop()
        {
            Started = false;

            if (runProcessCancellationTokenSource != null)
            {
                runProcessCancellationTokenSource.Cancel();
            }

            ActionDoneChecker.StopDoneChecker = true;

            foreach (Unit unit in unitList)
            {
                unit.Stop();
            }

            ActionDoneChecker.StopDoneChecker = false;
        }
        public bool InitDone { get; set; } = false;
        public bool Started { get; set; } = false;

        public void Init(bool initData, bool initMachine, CancellationTokenSource cancellationTokenSource)
        {
            InitDone = false;

            var progressForm = new SimpleProgressForm("Initialize...");
            progressForm.Show(() =>
            {
                int maxInitOrder = unitList.Max(x => x.InitOrder);
                for (int i = 0; i <= maxInitOrder; i++)
                {
                    foreach (Unit unit in unitList)
                    {
                        unit.Init(i, initData, initMachine, cancellationTokenSource.Token);
                    }

                    var timeOutTimer = new TimeOutTimer();
                    timeOutTimer.Start(200000); // 15000 : 원래 지정되어 있던 타임아웃

                    while (IsInitialized() == false)
                    {
                        if (timeOutTimer.TimeOut)
                        {
                            InitDone = true;
                            ErrorManager.Instance().Report((int)ErrorSection.Machine, (int)MachineError.InitTimeOut, ErrorLevel.Error,
                                ErrorSection.Machine.ToString(), MachineError.InitTimeOut.ToString(), "Init Timout");

                            cancellationTokenSource.Cancel();

                            return;
                        }
                        Thread.Sleep(100);
                    }
                }
                InitDone = true;
            }, cancellationTokenSource);
        }

        private bool IsInitialized()
        {
            foreach (Unit unit in unitList)
            {
                if (unit.IsInitalized() == false)
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsStopped()
        {
            foreach (Unit unit in unitList)
            {
                if (unit.IsStopped == false)
                {
                    return false;
                }
            }

            return true;
        }

        public void Link()
        {
            foreach (Unit unit in unitList)
            {
                unit.Link(this);
            }
        }

        public virtual void LoadData(XmlElement element)
        {
            foreach (Unit unit in unitList)
            {
                unit.LoadData(element);
            }
        }

        public virtual void SaveData(XmlElement element)
        {
            foreach (Unit unit in unitList)
            {
                unit.SaveData(element);
            }
        }

        public void LoadData(string fileName)
        {
            if (File.Exists(fileName) == false)
            {
                throw new Exception();
            }

            var stateDocument = new XmlDocument();
            stateDocument.Load(fileName);

            XmlElement stateElement = stateDocument.DocumentElement;

            LoadData(stateDocument.DocumentElement);

            lastFileName = fileName;
        }

        public void SaveData(string fileName = "")
        {
            if (fileName == "")
            {
                fileName = lastFileName;
            }

            var xmlDocument = new XmlDocument();

            XmlElement stateElement = xmlDocument.CreateElement("", "State", "");
            xmlDocument.AppendChild(stateElement);

            SaveData(stateElement);

            var xmlSettings = new XmlWriterSettings();
            xmlSettings.Indent = true;
            xmlSettings.IndentChars = "\t";
            xmlSettings.NewLineHandling = NewLineHandling.Entitize;
            xmlSettings.NewLineChars = "\r\n";

            var xmlWriter = XmlWriter.Create(fileName, xmlSettings);

            xmlDocument.Save(xmlWriter);
            xmlWriter.Flush();
            xmlWriter.Close();

            lastFileName = fileName;
        }
    }

    public delegate void StateChangedDelegate(string srcName);

    public abstract class Unit
    {
        public string Name { get; set; }

        protected bool onInitializing = false;
        public bool OnInitializing => onInitializing;

        protected CancellationToken initCancellationToken;
        protected CancellationToken runProcessCancellationToken;

        public StateChangedDelegate StateChanged;
        private Task task;

        public readonly object ioLock = new object();

        protected UnitState unitState;
        protected string stateFileName;
        private object stateFileLock = new object();

        protected DigitalIoHandler digitalIoHandler;

        protected int errorSection;
        protected string errorSectionName;
        public int InitOrder { get; set; }
        public bool Enabled { get; set; } = true;

        protected AxisHandler axisHandler;
        public AxisHandler AxisHandler
        {
            get => axisHandler;
            set => axisHandler = value;
        }

        protected Task initMachineTask;

        protected bool isError = false;
        public bool IsError
        {
            get => isError;
            set => isError = value;
        }

        protected bool isStopped = true;
        public bool IsStopped
        {
            get => isStopped;
            set => isStopped = value;
        }

        public abstract string GetStepName();

        private System.Windows.Forms.Timer stateUpdateTimer = null;

        public Unit()
        {
            stateUpdateTimer = new System.Windows.Forms.Timer();
            stateUpdateTimer.Interval = 100;
            stateUpdateTimer.Tick += new EventHandler(StateUpdateTimer_Tick);
            stateUpdateTimer.Start();
        }

        private void StateUpdateTimer_Tick(object sender, EventArgs e)
        {
            stateUpdateTimer.Stop();
            StateChanged?.Invoke(Name);

            stateUpdateTimer.Start();
        }

        public void Start(CancellationToken runProcessCancellationToken)
        {
            if (task != null)
            {
                MessageForm.Show(null, "Task is not finished. Please, check the stop process.");
                return;
            }

            unitState.RunMode = UnitState.ERunMode.Start;
            this.runProcessCancellationToken = runProcessCancellationToken;


            task = new Task(new Action(UnitProcess), runProcessCancellationToken);
            task.Start();

            isStopped = false;
        }

        public void Stop()
        {
            unitState.RunMode = UnitState.ERunMode.Stop;

            try
            {
                task?.Wait();
            }
            catch (AggregateException)
            {
            }

            task = null;
        }

        public void Reset()
        {
            unitState.RunMode = UnitState.ERunMode.Reset;
            task.Wait();
        }

        public void UnitProcess()
        {
            LogHelper.Debug(LoggerType.Device, "[UnitProcess] Unit Process Start. Name = " + Name);

            try
            {
                bool stopFlag = false;

                while (stopFlag == false)
                {
                    switch (unitState.OpMode)
                    {
                        case UnitState.EOpMode.Manual:
                            UnitManualProcess();
                            break;
                        case UnitState.EOpMode.Pass:
                        case UnitState.EOpMode.DryRun:
                        case UnitState.EOpMode.Auto:
                            switch (unitState.RunMode)
                            {
                                case UnitState.ERunMode.Stop:
                                    StopRunProcess();
                                    runProcessCancellationToken.ThrowIfCancellationRequested();
                                    break;
                                case UnitState.ERunMode.Start:
                                    StartRunProcess();
                                    unitState.RunMode = UnitState.ERunMode.Run;
                                    break;
                                case UnitState.ERunMode.Error:
                                    StopRunProcess();
                                    runProcessCancellationToken.ThrowIfCancellationRequested();
                                    break;
                                case UnitState.ERunMode.Run:
                                    if (Enabled == false || ErrorManager.Instance().IsError())
                                    {
                                        break;
                                    }

                                    runProcessCancellationToken.ThrowIfCancellationRequested();

                                    if (RunProcess() == false)
                                    {
                                        LogHelper.Debug(LoggerType.Device, string.Format("[UnitProcess] Run Process Cancelled. Name = {0} , Step = {1}", Name, GetStepName()));

                                        //if (axisHandler.IsMovingTimeOut() == true)
                                        //{
                                        //    axisHandler.StopMove();
                                        //    ErrorReport(errorSection, (int)UnitError.MoveTo, ErrorLevel.Warning, Name, UnitError.MoveTo.ToString(), String.Format("Unit : {0} / Move to {1}", Name, axisHandler.LastAxisPosition.Name));
                                        //}

                                        unitState.RunMode = UnitState.ERunMode.Error;
                                    }

                                    runProcessCancellationToken.ThrowIfCancellationRequested();

                                    if (IsStateChanged())
                                    {
                                        SaveLastState();

                                        StateChanged?.Invoke(Name);
                                    }
                                    runProcessCancellationToken.ThrowIfCancellationRequested();

                                    break;
                                case UnitState.ERunMode.Reset:
                                    ResetRunProcess();
                                    break;
                                default:
                                    break;
                            }
                            break;
                    }

                    runProcessCancellationToken.ThrowIfCancellationRequested();

                    Thread.Sleep(10);
                }
            }
            catch (OperationCanceledException)
            {
                StopRunProcess();
                LogHelper.Debug(LoggerType.Device, "[UnitProcess] Unit Process Cancelled. Name = " + Name);

                task = null;
            }

            isStopped = true;
        }

        public abstract void GetPositionList(List<AxisPosition> positionList);
        public abstract void SetPositionList(List<AxisPosition> positionList);

        public void LoadLastState()
        {
            if (File.Exists(stateFileName) == false)
            {
                return;
            }

            try
            {
                var stateDocument = new XmlDocument();
                stateDocument.Load(stateFileName);

                XmlElement stateElement = stateDocument.DocumentElement;

                LoadLastState(stateDocument.DocumentElement);
            }
            catch (XmlException)
            {
                File.Delete(stateFileName);
            }
        }

        public void SaveLastState()
        {
            var xmlDocument = new XmlDocument();

            XmlElement stateElement = xmlDocument.CreateElement("", "State", "");
            xmlDocument.AppendChild(stateElement);

            SaveLastState(stateElement);

            var xmlSettings = new XmlWriterSettings();
            xmlSettings.Indent = true;
            xmlSettings.IndentChars = "\t";
            xmlSettings.NewLineHandling = NewLineHandling.Entitize;
            xmlSettings.NewLineChars = "\r\n";

            lock (stateFileLock)
            {
                var xmlWriter = XmlWriter.Create(stateFileName, xmlSettings);

                xmlDocument.Save(xmlWriter);
                xmlWriter.Flush();
                xmlWriter.Close();
            }
        }

        protected void ErrorReport(int errorSection, int errorType, ErrorLevel errorLevel, string sectionStr, string errorStr, string message, string reasonMsg = "", string solutionMsg = "")
        {
            ErrorManager.Instance().Report(errorSection, errorType, errorLevel, sectionStr, errorStr, message, reasonMsg, solutionMsg);
        }

        public abstract void Init(int orderNo, bool initData, bool initMachine, CancellationToken cancellationToken);

        public virtual bool IsInitalized()
        {
            if (initMachineTask != null)
            {
                if (initMachineTask.IsCompleted == false)
                {
                    return false;
                }

                initMachineTask = null;
            }

            return onInitializing == false;
        }

        public abstract void Link(UnitManager unitManager);

        public abstract bool IsStateChanged();
        public abstract void LoadLastState(XmlElement stateElement);
        public abstract void SaveLastState(XmlElement stateElement);
        public abstract void LoadData(XmlElement element);
        public abstract void SaveData(XmlElement element);

        public abstract void UnitManualProcess();
        public abstract void StartRunProcess();
        public abstract void StopRunProcess();
        public abstract void ResetRunProcess();
        public abstract bool RunProcess();
    }
}
