using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace DynMvp.Devices.MotionController
{
    public enum AxisName
    {
        X, Y, Z, R
    }

    public enum Direction
    {
        Positive = 1, Negative = -1
    }

    public class AxisParam
    {
        public bool UseServo { get; set; }
        public bool IsStep { get; set; }
        public int MovingDoneWaitTimeMs { get; set; } = 30000;
        public int HomingDoneWaitTimeMs { get; set; } = 40000;
        public float PositiveLimit { get; set; }
        public float NegativeLimit { get; set; }
        public HomeParam HomeSpeed { get; set; } = new HomeParam();
        public MovingParam MovingParam { get; set; } = new MovingParam();
        public MovingParam JogParam { get; set; } = new MovingParam();
        public float OriginPulse { get; set; } = 0;
        public float MicronPerPulse { get; set; } = 1;
        public bool Inverse { get; set; } = false;

        public bool IsValidPosition(float position)
        {
            return position > NegativeLimit && position < PositiveLimit;
        }

        public void Load(XmlElement axisParamElement)
        {
            if (axisParamElement == null)
            {
                return;
            }

            UseServo = Convert.ToBoolean(XmlHelper.GetValue(axisParamElement, "UseServo", UseServo.ToString()));
            IsStep = Convert.ToBoolean(XmlHelper.GetValue(axisParamElement, "IsStep", IsStep.ToString()));
            PositiveLimit = Convert.ToSingle(XmlHelper.GetValue(axisParamElement, "PositiveLimit", PositiveLimit.ToString()));
            NegativeLimit = Convert.ToSingle(XmlHelper.GetValue(axisParamElement, "NegativeLimit", NegativeLimit.ToString()));
            MovingDoneWaitTimeMs = Convert.ToInt32(XmlHelper.GetValue(axisParamElement, "MovingDoneWaitTime", MovingDoneWaitTimeMs.ToString()));
            HomingDoneWaitTimeMs = Convert.ToInt32(XmlHelper.GetValue(axisParamElement, "HomingDoneWaitTime", HomingDoneWaitTimeMs.ToString()));

            Inverse = Convert.ToBoolean(XmlHelper.GetValue(axisParamElement, "Inverse", Inverse.ToString()));
            MicronPerPulse = Convert.ToSingle(XmlHelper.GetValue(axisParamElement, "MicronPerPulse", MicronPerPulse.ToString()));
            OriginPulse = Convert.ToSingle(XmlHelper.GetValue(axisParamElement, "OriginPulse", OriginPulse.ToString()));

            XmlElement homeSpeedElement = axisParamElement["HomeSpeed"];
            if (homeSpeedElement != null)
            {
                HomeSpeed.Load(homeSpeedElement);
            }

            XmlElement movingParamElement = axisParamElement["MovingParam"];
            if (movingParamElement != null)
            {
                MovingParam.Load(movingParamElement);
            }

            XmlElement jogParamElement = axisParamElement["JogParam"];
            if (jogParamElement != null)
            {
                JogParam.Load(jogParamElement);
            }
        }

        public void Save(XmlElement axisParamElement)
        {
            if (axisParamElement == null)
            {
                return;
            }

            XmlHelper.SetValue(axisParamElement, "UseServo", UseServo.ToString());
            XmlHelper.SetValue(axisParamElement, "IsStep", IsStep.ToString());
            XmlHelper.SetValue(axisParamElement, "PositiveLimit", PositiveLimit.ToString());
            XmlHelper.SetValue(axisParamElement, "NegativeLimit", NegativeLimit.ToString());
            XmlHelper.SetValue(axisParamElement, "MovingDoneWaitTime", MovingDoneWaitTimeMs.ToString());
            XmlHelper.SetValue(axisParamElement, "HomingDoneWaitTime", HomingDoneWaitTimeMs.ToString());

            XmlHelper.SetValue(axisParamElement, "Inverse", Inverse.ToString());
            XmlHelper.SetValue(axisParamElement, "MicronPerPulse", MicronPerPulse.ToString());
            XmlHelper.SetValue(axisParamElement, "OriginPulse", OriginPulse.ToString());

            XmlElement homeSpeedElement = axisParamElement.OwnerDocument.CreateElement("", "HomeSpeed", "");
            axisParamElement.AppendChild(homeSpeedElement);
            HomeSpeed.Save(homeSpeedElement);

            XmlElement movingParamElement = axisParamElement.OwnerDocument.CreateElement("", "MovingParam", "");
            axisParamElement.AppendChild(movingParamElement);
            MovingParam.Save(movingParamElement);

            XmlElement jogParamElement = axisParamElement.OwnerDocument.CreateElement("", "JogParam", "");
            axisParamElement.AppendChild(jogParamElement);
            JogParam.Save(jogParamElement);
        }
    }

    public class Axis
    {
        public string Name { get; private set; }
        public Motion Motion { get; private set; }
        public int AxisNo { get; private set; }
        public bool HomeFound { get; set; }
        public AxisParam AxisParam { get; } = new AxisParam();

        private bool onHomeMoving = false;
        private TimeOutTimer timeOutTimer = new TimeOutTimer();
        public int HomeOrder { get; set; } = -1;
        public bool SuppressHomeFound { get; set; } = false;

        public Axis(string name, Motion motion, int axisNo)
        {
            Update(name, motion, axisNo);
        }

        public void Update(string name, Motion motion, int axisNo)
        {
            Name = name;
            Motion = motion;
            AxisNo = axisNo;
        }

        public override string ToString()
        {
            return Name;
        }

        private bool IsInWorkingRegion(float pulse)
        {
            // 값이 설정되어 있지 않으면 두 값 모두 '0'
            if (AxisParam.NegativeLimit < AxisParam.PositiveLimit)
            {
                if (AxisParam.NegativeLimit > pulse)
                {
                    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)MotionError.NegLimit, ErrorLevel.Warning,
                        ErrorSection.Motion.ToString(), MotionError.NegLimit.ToString(), string.Format("Axis No = {0}", AxisNo.ToString()));
                    return false;
                }

                if (pulse > AxisParam.PositiveLimit)
                {
                    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)MotionError.PosLimit, ErrorLevel.Warning,
                        ErrorSection.Motion.ToString(), MotionError.PosLimit.ToString(), string.Format("Axis No = {0}", AxisNo.ToString()));
                    return false;
                }
            }

            //float curPulse = GetActualPulse();
            //if (motion.IsPositiveOn(axisNo) && curPulse < pulse)
            //{
            //    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)MotionError.PosLimit, ErrorLevel.Warning, String.Format("Axis No = {0}", axisNo.ToString()));
            //    return false;
            //}

            //if (motion.IsNegativeOn(axisNo) && curPulse > pulse)
            //{
            //    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)MotionError.PosLimit, ErrorLevel.Warning, String.Format("Axis No = {0}", axisNo.ToString()));
            //    return false;
            //}

            return true;
        }

        public bool CheckValidState()
        {
            if (ErrorManager.Instance().IsAlarmed())
            {
                return false;
            }

            if (SuppressHomeFound == false && HomeFound == false && onHomeMoving == false)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)MotionError.HomeFound, ErrorLevel.Error,
                        ErrorSection.Motion.ToString(), MotionError.HomeFound.ToString(), string.Format("Axis No = {0}", AxisNo.ToString()));
                return false;
            }

            if (Motion.IsAmpFault(AxisNo) == true)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)MotionError.AmpFault, ErrorLevel.Error,
                    ErrorSection.Motion.ToString(), MotionError.AmpFault.ToString(), string.Format("Axis No = {0}", AxisNo.ToString()));
                return false;
            }

            if (Motion.IsServoOn(AxisNo) == false)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)MotionError.ServoOff, ErrorLevel.Error,
                    ErrorSection.Motion.ToString(), MotionError.ServoOff.ToString(), string.Format("Axis No = {0}", AxisNo.ToString()));
                return false;
            }

            //if (onHomeMoving == true)
            //{
            //    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)MotionError.Moving, ErrorLevel.Error,
            //  ErrorSection.Motion.ToString(), MotionError.Moving.ToString(), String.Format("Axis No {0} is searching home", axisNo.ToString()));
            //    return false;
            //}

            //    if (onHomeMoving == false)
            //{
            //    if (motion.IsPositiveOn(axisNo) == true)
            //    {
            //        ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)MotionError.PosLimit, ErrorLevel.Error,
            //            ErrorSection.Motion.ToString(), MotionError.PosLimit.ToString(), String.Format("Axis No = {0}", axisNo.ToString()));
            //        return false;
            //    }

            //    if (motion.IsNegativeOn(axisNo) == true)
            //    {
            //        ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)MotionError.NegLimit, ErrorLevel.Error,
            //            ErrorSection.Motion.ToString(), MotionError.NegLimit.ToString(), String.Format("Axis No = {0}", axisNo.ToString()));
            //        return false;
            //    }
            //}

            //if (IsMovingTimeOut() == true)
            //{
            //    if (onHomeMoving == true)
            //        ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)MotionError.HomingTimeOut, ErrorLevel.Warning, 
            //            ErrorSection.Motion.ToString(), MotionError.HomingTimeOut.ToString(), String.Format("Axis No = {0}", axisNo.ToString()));
            //    else
            //        ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)MotionError.MovingTimeOut, ErrorLevel.Warning,
            //            ErrorSection.Motion.ToString(), MotionError.MovingTimeOut.ToString(), String.Format("Axis No = {0}", axisNo.ToString()));

            //    ResetState();

            //    return false;
            //}

            return true;
        }

        public bool StartMove(float position, MovingParam movingParam = null, bool rawMove = false)
        {
            if (CheckValidState() == false)
            {
                return false;
            }

            float pulse;
            if (rawMove == false)
            {
                pulse = ToPulse(position);
            }
            else
            {
                pulse = position;
            }

            if (!IsInWorkingRegion(pulse))
            {
                return false;
            }

            LogHelper.Debug(LoggerType.Motion, string.Format("StartMove : Axis Id {0} / Position {1} / Pulse {2} ", AxisNo, position, pulse));

            bool result;
            if (movingParam != null)
            {
                result = Motion.StartMove(AxisNo, pulse, movingParam);
            }
            else
            {
                result = Motion.StartMove(AxisNo, pulse, AxisParam.MovingParam);
            }

            if (result == true)
            {
                timeOutTimer.Start(AxisParam.MovingDoneWaitTimeMs);
            }

            return result;
        }

        public bool Move(float position, MovingParam movingParam = null, bool rawMove = false)
        {
            LogHelper.Debug(LoggerType.Motion, string.Format("Move : Axis Id {0} / Position {1} ", AxisNo, position));

            if (StartMove(position, movingParam, rawMove) == false)
            {
                return false;
            }

            return WaitMoveDone();
        }

        public bool StartRelativeMove(float offset, MovingParam movingParam = null)
        {
            if (CheckValidState() == false)
            {
                return false;
            }

            float position = GetActualPos() + offset;

            if (!IsInWorkingRegion(ToPulse(position)))
            {
                return false;
            }

            float offsetPulse = ToOffsetPulse(offset);

            LogHelper.Debug(LoggerType.Motion, string.Format("StartRelativeMove : Axis Id {0} / Offset {1} / Offset Pulse {2} ", AxisNo, offset, offsetPulse));

            bool result;
            if (movingParam != null)
            {
                result = Motion.StartRelativeMove(AxisNo, offsetPulse, movingParam);
            }
            else
            {
                result = Motion.StartRelativeMove(AxisNo, offsetPulse, AxisParam.MovingParam);
            }

            if (result == true)
            {
                timeOutTimer.Start(AxisParam.MovingDoneWaitTimeMs);
            }

            return result;
        }

        public bool RelativeMove(float offset, MovingParam movingParam = null)
        {
            LogHelper.Debug(LoggerType.Motion, string.Format("RelativeMove : Axis Id {0} / Offset {1} ", AxisNo, offset));

            if (StartRelativeMove(offset, movingParam) == false)
            {
                return false;
            }

            return WaitMoveDone();
        }

        public bool EjectHome(int axisNo)
        {
            if (Motion.MotionType == MotionType.Virtual)
            {
                return true;
            }

            if (IsMoveDone() == false)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)MotionError.HomingTimeOut, ErrorLevel.Warning,
                    ErrorSection.Motion.ToString(), MotionError.HomingTimeOut.ToString(), string.Format("Eject Home : Axis No = {0}", axisNo.ToString()));
                return false;
            }

            if (CheckValidState() == false)
            {
                return false;
            }

            if (ContinuousMove(AxisParam.HomeSpeed.FineSpeed) == false)
            {
                return false;
            }

            timeOutTimer.Start(AxisParam.MovingDoneWaitTimeMs);

            while (Motion.IsHomeOn(axisNo))
            {
                Application.DoEvents();

                if (timeOutTimer.TimeOut)
                {
                    StopMove();
                    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)MotionError.HomingTimeOut, ErrorLevel.Warning,
                                ErrorSection.Motion.ToString(), MotionError.HomingTimeOut.ToString(), string.Format("Eject Home : Axis No = {0}", axisNo.ToString()));
                    return false;
                }

                if (CheckValidState() == false)
                {
                    StopMove();
                    return false;
                }

                Thread.Sleep(AxisHandler.MotionDoneCheckIntervalMs);
            }

            LogHelper.Debug(LoggerType.Motion, string.Format("Eject Home Elapsed Time : {0} ms", AxisParam.MovingDoneWaitTimeMs));

            ResetState();

            return true;
        }

        public void ResetAlarm()
        {
            Motion.ResetAlarm(AxisNo);
        }

        public void ResetAlarmOn(bool isOn)
        {
            Motion.ResetAlarmOn(AxisNo, isOn);
        }

        public void StopMove()
        {
            ResetState();

            Motion.StopMove(AxisNo);
        }

        public void EmergencyStop()
        {
            ResetState();

            Motion.EmergencyStop(AxisNo);
        }

        public bool StartHomeMove()
        {
            onHomeMoving = true;
            timeOutTimer.Start(AxisParam.HomingDoneWaitTimeMs);

            return Motion.StartHomeMove(AxisNo, AxisParam.HomeSpeed);
        }

        public bool HomeMove()
        {
            if (StartHomeMove() == false)
            {
                return false;
            }

            return WaitHomeDone();
        }

        public bool IsOnError()
        {
            return Motion.IsAmpFault(AxisNo);
        }

        public bool IsOnEmgStop()
        {
            return Motion.IsEmgStop(AxisNo);
        }

        internal float GetNegativePos()
        {
            if (AxisParam.Inverse)
            {
                return ToPosition(AxisParam.PositiveLimit);
            }
            else
            {
                return ToPosition(AxisParam.NegativeLimit);
            }
        }

        internal float GetPositivePos()
        {
            if (AxisParam.Inverse)
            {
                return ToPosition(AxisParam.NegativeLimit);
            }
            else
            {
                return ToPosition(AxisParam.PositiveLimit);
            }
        }

        public bool ContinuousMove(MovingParam movingParam = null, bool negative = false)
        {
            if (CheckValidState() == false)
            {
                return false;
            }

            bool result;
            if (movingParam != null)
            {
                result = Motion.ContinuousMove(AxisNo, movingParam, negative);
            }
            else
            {
                result = Motion.ContinuousMove(AxisNo, AxisParam.JogParam, negative);
            }

            return result;
        }

        public void TurnOnServo(bool bOnOff)
        {
            if (AxisParam.IsStep)
            {
                bOnOff = !bOnOff;
            }

            Motion.TurnOnServo(AxisNo, bOnOff);
        }

        public bool IsMoveDone()
        {
            return Motion.IsMoveDone(AxisNo);
        }

        public bool IsMovingTimeOut()
        {
            return (timeOutTimer.TimeOut);
        }

        public bool WaitMoveDone()
        {
            while (Motion.IsMoveDone(AxisNo) == false)
            {
                Application.DoEvents();

                if (CheckValidState() == false)
                {
                    StopMove();
                    return false;
                }

                Thread.Sleep(AxisHandler.MotionDoneCheckIntervalMs);
            }

            ResetState();

            return true;
        }

        public bool WaitHomeDone()
        {
            while (/*motion.IsHomeDone(axisNo) == false ||*/ Motion.IsMoveDone(AxisNo) == false)
            {
                Application.DoEvents();

                if (CheckValidState() == false)
                {
                    ResetState();
                    StopMove();
                    return false;
                }

                Thread.Sleep(AxisHandler.MotionDoneCheckIntervalMs);
            }

            HomeFound = true;

            ResetState();
            IsHomeOn();
            return true;
        }

        public void ResetState()
        {
            onHomeMoving = false;
            timeOutTimer.Reset();
        }

        public bool IsAmpFault()
        {
            return Motion.IsAmpFault(AxisNo);
        }

        public bool IsServoOn()
        {
            return Motion.IsServoOn(AxisNo);
        }

        public bool IsHomeOn()
        {
            return Motion.IsHomeOn(AxisNo);
        }

        public bool IsHomeDone()
        {
            return Motion.IsHomeDone(AxisNo);
        }

        public bool IsPositiveOn()
        {
            return Motion.IsPositiveOn(AxisNo);
        }

        public bool IsNegativeOn()
        {
            return Motion.IsNegativeOn(AxisNo);
        }

        public float GetCommandPos()
        {
            float pulse = Motion.GetCommandPos(AxisNo);
            return ToPosition(pulse);
        }

        public float GetActualPos()
        {
            float pulse = Motion.GetActualPos(AxisNo);
            return ToPosition(pulse);
        }

        public float GetCommandPulse()
        {
            return Motion.GetCommandPos(AxisNo);
        }

        public float GetActualPulse()
        {
            return Motion.GetActualPos(AxisNo);
        }

        public void SetPosition(float position)
        {
            float pulse = ToPulse(position);
            Motion.SetPosition(AxisNo, pulse);
        }

        public void SetPulse(float pulse)
        {
            Motion.SetPosition(AxisNo, pulse);
        }

        public MotionStatus GetMotionStatus()
        {
            return Motion.GetMotionStatus(AxisNo);
        }

        public float ToPulse(float position)
        {
            return Convert.ToSingle(Math.Round(position / AxisParam.MicronPerPulse * (AxisParam.Inverse ? -1 : 1) + AxisParam.OriginPulse));
        }

        public float ToOffsetPulse(float position)
        {
            return Convert.ToSingle(Math.Round(position / AxisParam.MicronPerPulse * (AxisParam.Inverse ? -1 : 1)));
        }

        public float ToPosition(float pulse)
        {
            return Convert.ToSingle(Math.Round((pulse - AxisParam.OriginPulse) * AxisParam.MicronPerPulse * (AxisParam.Inverse ? -1 : 1)));
        }
    }
}
