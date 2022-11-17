using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace DynMvp.Devices.MotionController
{
    public enum AxisHandlerName
    {
        RobotStage, Converyor
    }

    public delegate void RobotEventDelegate(AxisPosition axisPosition);

    public class AxisHandler
    {
        public string Name { get; set; }
        public RobotEventDelegate OnBeginMove { get; set; }
        public RobotEventDelegate OnEndMove { get; set; }
        public RobotMapper RobotAligner { get; set; } = new RobotMapper();

        private static int motionDoneCheckIntervalMs = 10;
        public static int MotionDoneCheckIntervalMs
        {
            get => AxisHandler.motionDoneCheckIntervalMs;
            set => AxisHandler.motionDoneCheckIntervalMs = value;
        }

        private static MovingProfileType movingProfileType;
        public static MovingProfileType MovingProfileType
        {
            get => AxisHandler.movingProfileType;
            set => AxisHandler.movingProfileType = value;
        }
        public AxisPosition LastAxisPosition { get; private set; } = null;
        public List<Axis> AxisList { get; } = new List<Axis>();
        public List<Axis> UniqueAxisList { get; } = new List<Axis>();

        public Axis GetUniqueAxis(string axisName)
        {
            return UniqueAxisList.Find(x => x.Name == axisName);
        }

        public Axis this[int key]
        {
            get => AxisList[key];
            set => AxisList[key] = value;
        }

        public override string ToString()
        {
            return Name;
        }

        public AxisHandler(string name)
        {
            Name = name;
        }

        public void Clear()
        {
            AxisList.Clear();
        }

        public int NumAxis => AxisList.Count;

        public int NumUniqueAxis => UniqueAxisList.Count;

        public void AddAxis(List<Axis> axisList)
        {
            foreach (Axis axis in axisList)
            {
                AddAxis(axis);
            }
        }

        public Axis AddAxis(string name, Motion motion, int axisNo)
        {
            var axis = new Axis(name, motion, axisNo);
            AxisList.Add(axis);

            if (GetUniqueAxis(name) == null)
            {
                UniqueAxisList.Add(axis);
            }

            return axis;
        }

        public void AddAxis(Axis axis)
        {
            if (AxisList.IndexOf(axis) == -1)
            {
                AxisList.Add(axis);
            }

            if (GetUniqueAxis(axis.Name) == null)
            {
                UniqueAxisList.Add(axis);
            }
        }
        public string MotionParamFile { get; set; } = string.Empty;

        public AxisPosition CreatePosition(string name = "")
        {
            var axisPosition = new AxisPosition(UniqueAxisList.Count);
            axisPosition.Name = name;

            return axisPosition;
        }

        public void Load(MotionHandler motionHandler)
        {
            string configurationFile = string.Format(@"{0}\AxisConfig_{1}.xml", BaseConfig.Instance().ConfigPath, Name);
            if (File.Exists(configurationFile) == false)
            {
                return;
            }

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(configurationFile);

            XmlElement axisHandlerElement = xmlDocument.DocumentElement;

            int numAxis = Convert.ToInt32(XmlHelper.GetValue(axisHandlerElement, "NumAxis", "0"));

            AxisList.Clear();

            motionDoneCheckIntervalMs = Convert.ToInt32(XmlHelper.GetValue(axisHandlerElement, "MotionDoneCheckIntervalMs", "10"));

            if (numAxis == 0)
            {
                return;
            }

            int axisId = 0;

            foreach (XmlElement axisElement in axisHandlerElement)
            {
                if (axisElement.Name == "Axis")
                {
                    string axisName = XmlHelper.GetValue(axisElement, "AxisName", "");
                    string motionName = XmlHelper.GetValue(axisElement, "Motion", "");
                    int axisNo = Convert.ToInt32(XmlHelper.GetValue(axisElement, "AxisNo", "0"));

                    if (string.IsNullOrEmpty(motionName))
                    {
                        ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.InvalidSetting,
                                        ErrorLevel.Error, ErrorSection.Motion.ToString(), CommonError.InvalidSetting.ToString(), string.Format("Motion name is empty [AxisHandler : {0}]", Name));
                        continue;
                    }

                    Motion motion = motionHandler.GetMotion(motionName);

                    var axis = new Axis(axisName, motion, axisNo);
                    AxisList.Add(axis);
                    if (GetUniqueAxis(axisName) == null)
                    {
                        UniqueAxisList.Add(axis);
                    }

                    AxisList[axisId].AxisParam.Load(axisElement);
                    axisId++;

                    if (numAxis <= axisId)
                    {
                        break;
                    }
                }
            }

            RobotAligner.Load();
        }

        public void Save(string configDir)
        {
            string configurationFile = string.Format(@"{0}\AxisConfig_{1}.xml", configDir, Name);

            var xmlDocument = new XmlDocument();
            XmlElement axisHandlerElement = xmlDocument.CreateElement("", "AxisHandler", "");
            xmlDocument.AppendChild(axisHandlerElement);

            XmlHelper.SetValue(axisHandlerElement, "NumAxis", AxisList.Count.ToString());
            XmlHelper.SetValue(axisHandlerElement, "MotionDoneCheckIntervalMs", motionDoneCheckIntervalMs.ToString());

            foreach (Axis axis in AxisList)
            {
                XmlElement axisElement = axisHandlerElement.OwnerDocument.CreateElement("", "Axis", "");
                axisHandlerElement.AppendChild(axisElement);

                XmlHelper.SetValue(axisElement, "AxisName", axis.Name);
                XmlHelper.SetValue(axisElement, "Motion", axis.Motion.Name);
                XmlHelper.SetValue(axisElement, "AxisNo", axis.AxisNo.ToString());

                axis.AxisParam.Save(axisElement);
            }

            var xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            xmlWriterSettings.IndentChars = "\t";
            xmlWriterSettings.NewLineHandling = NewLineHandling.Entitize;
            xmlWriterSettings.NewLineChars = "\r\n";

            var xmlWriter = XmlWriter.Create(configurationFile, xmlWriterSettings);

            xmlDocument.Save(xmlWriter);
            xmlWriter.Flush();
            xmlWriter.Close();
        }

        public RectangleF GetWorkingRange()
        {
            Axis xAxis = GetAxis(AxisName.X.ToString());
            Axis yAxis = GetAxis(AxisName.Y.ToString());
            if (xAxis == null || yAxis == null)
            {
                return RectangleF.Empty;
            }
            return RectangleF.FromLTRB(xAxis.GetNegativePos(), yAxis.GetNegativePos(), xAxis.GetPositivePos(), yAxis.GetPositivePos());
        }

        private bool CanSyncMotion()
        {
            Motion motion = null;
            foreach (Axis axis in AxisList)
            {
                if (motion == null)
                {
                    motion = axis.Motion;
                }
                else if (motion != axis.Motion)
                {
                    return false;
                }
            }

            return motion.CanSyncMotion();
        }

        public Axis GetAxis(int axisId)
        {
            if (AxisList.Count > axisId)
            {
                return AxisList[axisId];
            }

            return null;
        }

        public Axis GetAxisByNo(int axisNo)
        {
            foreach (Axis axis in AxisList)
            {
                if (axis.AxisNo == axisNo)
                {
                    return axis;
                }
            }

            return null;
        }

        public Axis GetAxis(string name)
        {
            foreach (Axis axis in AxisList)
            {
                if (axis.Name == name)
                {
                    return axis;
                }
            }

            return null;
        }

        public bool HomeMove(string name, CancellationTokenSource cancellationTokenSource)
        {
            foreach (Axis axis in AxisList)
            {
                if (axis.Name == name)
                {
                    if (axis.HomeMove() == false)
                    {
                        StopMove();
                        return false;
                    }
                }
            }

            return true;
        }

        public bool HomeMove(int axisId)
        {
            if (AxisList[axisId].HomeMove() == false)
            {
                StopMove();
                return false;
            }

            return true;
        }

        public Task StartHomeMove(CancellationToken cancellationToken)
        {
            return Task.Run(() => HomeMove(cancellationToken));
        }

        private int GetMaxHomeOrder()
        {
            return AxisList.Max(x => x.HomeOrder);
        }

        private List<Axis> GetAxisListByHomeOrder(int homeOrder)
        {
            var homeAxisList = new List<Axis>();

            foreach (Axis axis in AxisList)
            {
                if (axis.HomeOrder == homeOrder)
                {
                    homeAxisList.Add(axis);
                }
            }

            return homeAxisList;
        }

        public bool HomeMove(string axisName = "")
        {
            return HomeMove(CancellationToken.None, axisName);
        }

        public bool HomeMove(CancellationToken cancellationToken, string axisName = "")
        {
            if (axisName == "")
            {
                for (int homeOrder = -1; homeOrder <= GetMaxHomeOrder(); homeOrder++)
                {
                    List<Axis> homeAxisList = GetAxisListByHomeOrder(homeOrder);
                    if (homeAxisList.Count > 0)
                    {
                        if (homeOrder == -1)
                        {
                            homeAxisList.ForEach(x =>
                            {
                                x.SetPosition(0);
                                x.HomeFound = true;
                            });
                        }
                        else if (HomeMove(homeAxisList, cancellationToken) == false)
                        {
                            StopMove();
                            return false;
                        }
                    }
                }
            }
            else
            {
                Axis axis = GetAxis(axisName);

                if (axis.HomeOrder == -1)
                {
                    axis.SetPosition(0);
                    axis.HomeFound = true;
                }
                else if (HomeMove(axis, cancellationToken) == false)
                {
                    StopMove();

                    return false;
                }
            }

            return true;
        }

        private bool HomeMove(List<Axis> homeMoveAxisList, CancellationToken cancellationToken)
        {
            if (WaitMoveDone(cancellationToken) == false)
            {
                return false;
            }

            OnBeginMove?.Invoke(null);

            bool waitHome = false;
            foreach (Axis axis in homeMoveAxisList)
            {
                axis.HomeFound = false;

                waitHome = true;
                if (axis.StartHomeMove() == false)
                {
                    StopMove();

                    OnEndMove?.Invoke(GetActualPos());

                    return false;
                }

                Thread.Sleep(50);
            }

            if (waitHome)
            // 모터가 모두 Home이 되어있었다면, 아래 함수에서 무한루프에 빠짐.
            {
                if (WaitHomeDone(homeMoveAxisList, cancellationToken) == false)
                {
                    OnEndMove?.Invoke(GetActualPos());

                    return false;
                }

                foreach (Axis homeMoveAxis in homeMoveAxisList)
                {
                    homeMoveAxis.HomeFound = true;
                }
            }
            Thread.Sleep(500);

            foreach (Axis axis in homeMoveAxisList)
            {
                axis.HomeFound = true;

                if (axis.AxisParam.OriginPulse != 0)
                {
                    if (axis.Move(axis.AxisParam.OriginPulse, null, true) == false)
                    {
                        return false;
                    }
                }
            }

            OnEndMove?.Invoke(GetActualPos());

            ResetState();

            return true;
        }

        private bool HomeMove(Axis homeMoveAxis, CancellationToken cancellationToken)
        {
            if (WaitMoveDone(cancellationToken) == false)
            {
                return false;
            }

            OnBeginMove?.Invoke(null);

            homeMoveAxis.HomeFound = false;

            if (homeMoveAxis.StartHomeMove() == false)
            {
                StopMove();

                return false;
            }

            Thread.Sleep(50);

            if (WaitHomeDone(cancellationToken) == false)
            {
                OnEndMove?.Invoke(GetActualPos());

                return false;
            }

            Thread.Sleep(500);

            homeMoveAxis.HomeFound = true;

            if (homeMoveAxis.AxisParam.OriginPulse != 0)
            {
                if (homeMoveAxis.Move(homeMoveAxis.AxisParam.OriginPulse, null, true) == false)
                {
                    return false;
                }
            }

            OnEndMove?.Invoke(GetActualPos());

            ResetState();

            return true;
        }

        public void ResetAlarm()
        {
            foreach (Axis axis in AxisList)
            {
                axis.ResetAlarm();
            }
        }

        public void ResetAlarmOn(bool isOn)
        {
            foreach (Axis axis in AxisList)
            {
                axis.ResetAlarmOn(isOn);
            }
        }

        public void TurnOnServo(bool bOnOff)
        {
            foreach (Axis axis in AxisList)
            {
                axis.TurnOnServo(bOnOff);
            }
        }

        public bool StartMove(string name, float position)
        {
            foreach (Axis axis in AxisList)
            {
                if (axis.Name == name)
                {
                    if (axis.StartMove(position, axis.AxisParam.MovingParam) == false)
                    {
                        StopMove();
                        return false;
                    }
                }
            }

            return true;
        }

        public bool StartMove(int axisId, float position)
        {
            OnBeginMove?.Invoke(null);

            if (AxisList[axisId].StartMove(position) == false)
            {
                StopMove();
                return false;
            }

            return true;
        }

        public bool StartMove(int[] axisId, AxisPosition position, MovingParam movingParam = null)
        {
            LastAxisPosition = position;

            position = RobotAligner.Align(position);

            OnBeginMove?.Invoke(position);

            foreach (int index in axisId)
            {
                if (AxisList[index].StartMove(position[index], movingParam) == false)
                {
                    StopMove();
                    return false;
                }
            }

            return true;
        }

        public bool Move(int axisId, float position)
        {
            return Move(axisId, position, CancellationToken.None);
        }

        public bool Move(int axisId, float position, CancellationToken cancellationToken)
        {
            if (StartMove(axisId, position) == false)
            {
                return false;
            }

            return WaitMoveDone(cancellationToken);
        }

        public bool Move(string name, float position)
        {
            return Move(name, position, CancellationToken.None);
        }

        public bool Move(string name, float position, CancellationToken cancellationToken)
        {
            if (StartMove(name, position) == false)
            {
                return false;
            }

            return WaitMoveDone(cancellationToken);
        }

        public bool Move(int[] axisIndex, AxisPosition position, MovingParam movingParam = null)
        {
            return Move(axisIndex, position, movingParam, CancellationToken.None);
        }

        public bool Move(int[] axisIndex, AxisPosition position, MovingParam movingParam, CancellationToken cancellationToken)
        {
            if (StartMove(axisIndex, position, movingParam) == false)
            {
                return false;
            }

            return WaitMoveDone(cancellationToken);
        }

        public bool Move(AxisPosition axisPosition)
        {
            return Move(axisPosition, CancellationToken.None);

        }

        public bool Move(AxisPosition axisPosition, CancellationToken cancellationToken)
        {
            if (StartMove(axisPosition) == false)
            {
                return false;
            }

            return WaitMoveDone(cancellationToken);
        }

        public void ResetPosition()
        {
            AxisPosition axisPosition = CreatePosition();
            axisPosition.ResetPosition();

            SetPosition(new AxisPosition());
        }

        public bool StartRelativeMove(string name, float position)
        {
            OnBeginMove?.Invoke(null);

            foreach (Axis axis in AxisList)
            {
                if (axis.Name == name)
                {
                    if (axis.StartRelativeMove(position) == false)
                    {
                        StopMove();
                        return false;
                    }
                }
            }

            return true;
        }

        public bool RelativeMove(string name, float position)
        {
            if (StartRelativeMove(name, position) == false)
            {
                return false;
            }

            return WaitMoveDone();
        }

        public bool RelativeMove(AxisPosition axisPosition)
        {
            return RelativeMove(axisPosition, CancellationToken.None);
        }

        public bool RelativeMove(AxisPosition axisPosition, CancellationToken cancellationToken)
        {
            if (StartRelativeMove(axisPosition) == false)
            {
                return false;
            }

            return WaitMoveDone(cancellationToken);
        }

        public bool IsMoveDone()
        {
            foreach (Axis axis in AxisList)
            {
                if (axis.IsMoveDone() == false)
                {
                    return false;
                }
            }

            return true;
        }

        public void ResetState()
        {
            foreach (Axis axis in AxisList)
            {
                axis.ResetState();
            }
        }

        public bool IsMovingTimeOut()
        {
            foreach (Axis axis in AxisList)
            {
                if (axis.IsMovingTimeOut() == true)
                {
                    return true;
                }
            }

            return false;
        }
        public bool CheckValidState(List<Axis> axisList)
        {
            foreach (Axis axis in axisList)
            {
                if (axis.CheckValidState() == false)
                {
                    return false;
                }
            }

            return true;
        }

        public bool WaitMoveDone()
        {
            return WaitMoveDone(CancellationToken.None);
        }

        public bool WaitMoveDone(CancellationToken cancellationToken)
        {
            var timeOutTimer = new TimeOutTimer();
            timeOutTimer.Start(DeviceConfig.Instance().MoveTimeoutMs);

            try
            {
                while (IsMoveDone() == false)
                {
                    Application.DoEvents();

                    if (CheckValidState(AxisList) == false)
                    {
                        ResetState();
                        StopMove();
                        return false;
                    }

                    timeOutTimer.ThrowIfTimeOut();

                    if (cancellationToken != CancellationToken.None)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                    }

                    Thread.Sleep(motionDoneCheckIntervalMs);
                }

                ResetState();

                OnEndMove?.Invoke(GetActualPos());
            }
            catch (OperationCanceledException)
            {
                ResetState();
                StopMove();
                return false;
            }
            catch (TimeoutException)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)MotionError.MovingTimeOut, ErrorLevel.Warning,
                    ErrorSection.Motion.ToString(), MotionError.MovingTimeOut.ToString(), string.Format("Axis Handler : {0}", Name));

                ResetState();
                StopMove();
                return false;
            }

            return true;
        }

        public bool WaitHomeDone(CancellationToken cancellationToken)
        {
            return WaitHomeDone(AxisList, cancellationToken);
        }

        public bool WaitHomeDone(List<Axis> axisList, CancellationToken cancellationToken)
        {
            try
            {
                var timeOutTimer = new TimeOutTimer();
                timeOutTimer.Start(DeviceConfig.Instance().OriginTimeoutMs);

                while (IsMoveDone() == false) // || IsHomeOn() == false)
                {
                    if (CheckValidState(axisList) == false)
                    {
                        ResetState();
                        StopMove();
                        return false;
                    }

                    timeOutTimer.ThrowIfTimeOut();

                    if (cancellationToken != CancellationToken.None)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                    }

                    Thread.Sleep(motionDoneCheckIntervalMs);
                }

                ResetState();

                OnEndMove?.Invoke(GetActualPos());
            }
            catch (OperationCanceledException)
            {
                ResetState();
                StopMove();

                return false;
            }
            catch (TimeoutException)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)MotionError.MovingTimeOut, ErrorLevel.Warning,
                    ErrorSection.Motion.ToString(), MotionError.MovingTimeOut.ToString(), string.Format("Axis Handler : {0}", Name));

                ResetState();
                StopMove();
                return false;
            }

            return true;
        }

        // 동기 구동이 가능한 모션 보드의 경우, 동기 구동이 되는 함수로 호출을 해 주는 것이 좋다.
        public virtual bool StartMove(AxisPosition position)
        {
            OnBeginMove?.Invoke(null);

            LastAxisPosition = position.Clone();

            position = RobotAligner.Align(position);
            for (int index = 0; index < position.NumAxis && index < AxisList.Count; index++)
            {
                string axisName = UniqueAxisList[index].Name;
                if (StartMove(axisName, position[index]) == false)
                {
                    StopMove();
                    return false;
                }
            }

            return true;
        }

        public virtual bool StartRelativeMove(AxisPosition position)
        {
            OnBeginMove?.Invoke(null);

            LastAxisPosition = position;

            for (int index = 0; index < position.NumAxis && index < AxisList.Count; index++)
            {
                string axisName = UniqueAxisList[index].Name;
                if (StartRelativeMove(axisName, position[index]) == false)
                {
                    StopMove();
                    return false;
                }
            }

            return true;
        }

        public bool StartContinuousMove(bool negative = false)
        {
            OnBeginMove?.Invoke(null);

            foreach (Axis axis in AxisList)
            {
                if (axis.ContinuousMove(null, negative) == false)
                {
                    return false;
                }
            }

            ResetState();

            return true;
        }

        public bool StartContinuousMove(int axisId, bool negative)
        {
            OnBeginMove?.Invoke(null);

            if (UniqueAxisList.Count <= axisId)
            {
                return false;
            }

            string uniqueAxisName = UniqueAxisList[axisId].Name;
            foreach (Axis axis in AxisList)
            {
                if (axis.Name == uniqueAxisName)
                {
                    if (axis.ContinuousMove(null, axis.AxisParam.Inverse ? !negative : negative) == false)
                    {
                        return false;
                    }
                }
            }

            ResetState();

            return true;
        }

        public bool StartContinuousMove(int[] axisId, bool negative, MovingParam movingParam = null)
        {
            OnBeginMove?.Invoke(null);

            foreach (Axis axis in AxisList)
            {
                for (int i = 0; i < axisId.Count(); i++)
                {
                    if (axis.AxisNo == axisId[i])
                    {
                        if (axis.ContinuousMove(null, negative) == false)
                        {
                            return false;
                        }
                    }
                }
            }

            ResetState();

            return true;
        }

        public bool StartContinuousMove(string name, bool negative)
        {
            OnBeginMove?.Invoke(null);

            foreach (Axis axis in AxisList)
            {
                if (axis.Name == name)
                {
                    if (axis.ContinuousMove(null, negative) == false)
                    {
                        return false;
                    }
                }
            }

            ResetState();

            return true;
        }


        public void StopMove()
        {
            foreach (Axis axis in AxisList)
            {
                axis.StopMove();
            }

            OnEndMove?.Invoke(GetActualPos());
        }

        public void EmergencyStop()
        {
            foreach (Axis axis in AxisList)
            {
                axis.EmergencyStop();
            }
        }

        public AxisPosition GetCommandPos()
        {
            var commandPos = new List<float>();
            foreach (Axis axis in UniqueAxisList)
            {
                commandPos.Add(axis.GetCommandPos());
            }

            var axisPosition = new AxisPosition();
            axisPosition.SetPosition(commandPos.ToArray());

            return RobotAligner.InverseAlign(axisPosition);
        }

        public float GetCommandPos(int axisId)
        {
            return UniqueAxisList[axisId].GetCommandPos();
        }

        public AxisPosition GetActualPos()
        {
            var actualPos = new List<float>();
            foreach (Axis axis in UniqueAxisList)
            {
                actualPos.Add(axis.GetActualPos());
            }

            var axisPosition = new AxisPosition();
            axisPosition.SetPosition(actualPos.ToArray());

            return RobotAligner.InverseAlign(axisPosition);
        }

        public float GetActualPos(int axisId)
        {
            return UniqueAxisList[axisId].GetActualPos();
        }

        public void SetPosition(string axisName, float position)
        {
            foreach (Axis axis in AxisList)
            {
                if (axis.Name == axisName)
                {
                    axis.SetPosition(position);
                }
            }
        }

        public void SetPosition(AxisPosition axisPosition)
        {
            AxisPosition alignPosition = RobotAligner.Align(axisPosition);
            for (int index = 0; index < alignPosition.NumAxis && index < AxisList.Count; index++)
            {
                string axisName = UniqueAxisList[index].Name;
                SetPosition(axisName, alignPosition[index]);
            }
        }

        public virtual bool IsOnError()
        {
            foreach (Axis axis in AxisList)
            {
                if (axis.IsOnError() == true)
                {
                    return true;
                }
            }

            return false;
        }

        public bool[] IsAmpFault()
        {
            bool[] ampFault = new bool[AxisList.Count()];
            foreach (Axis axis in AxisList)
            {
                ampFault[axis.AxisNo] = axis.IsAmpFault();
            }

            return ampFault;
        }

        public bool[] IsHomeOn()
        {
            bool[] homeOn = new bool[AxisList.Count()];
            foreach (Axis axis in AxisList)
            {
                homeOn[axis.AxisNo] = axis.IsHomeOn();
            }

            return homeOn;
        }

        public bool IsAllHomeOn()
        {
            foreach (Axis axis in AxisList)
            {
                if (axis.IsHomeOn() == false)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsAllHomeDone()
        {
            foreach (Axis axis in AxisList)
            {
                if (axis.IsHomeDone() == false)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsAllServoOn()
        {
            foreach (Axis axis in AxisList)
            {
                if (axis.IsServoOn() == false)
                {
                    return false;
                }
            }
            return true;
        }

        public bool[] IsPositiveOn()
        {
            bool[] positiveOn = new bool[AxisList.Count()];
            foreach (Axis axis in AxisList)
            {
                positiveOn[axis.AxisNo] = axis.IsPositiveOn();
            }

            return positiveOn;
        }

        public bool[] IsNegativeOn()
        {
            bool[] negativeOn = new bool[AxisList.Count()];
            foreach (Axis axis in AxisList)
            {
                negativeOn[axis.AxisNo] = axis.IsNegativeOn();
            }

            return negativeOn;
        }
    }
}
