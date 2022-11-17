using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DynMvp.Devices.MotionController
{
    public class MotionVirtual : Motion
    {
        private float[] position;

        public MotionVirtual(string name) : base(MotionType.Virtual, name)
        {
        }

        public MotionVirtual(MotionType motionType, string name) : base(motionType, name)
        {
        }

        public override bool Initialize(MotionInfo motionInfo)
        {
            NumAxis = motionInfo.NumAxis;
            position = new float[NumAxis];

            return true;
        }

        public override void Release()
        {
            base.Release();
        }

        public override bool CanSyncMotion()
        {
            return false;
        }

        public override void TurnOnServo(int axisNo, bool bOnOff)
        {

        }

        public override float GetCommandPos(int axisNo)
        {
            return position[axisNo];
        }

        public override float GetActualPos(int axisNo)
        {
            if (position != null && position.Count() > axisNo)
            {
                return position[axisNo];
            }
            return 0;
        }

        public override void SetPosition(int axisNo, float position)
        {
            this.position[axisNo] = position;
        }

        public override bool StartHomeMove(int axisNo, HomeParam homeSpeed)
        {
            position[axisNo] = 0;
            return true;
        }

        public override bool StartMove(int axisNo, float position, MovingParam movingParam)
        {
            this.position[axisNo] = position;
            return true;
        }

        public override bool StartRelativeMove(int axisNo, float offset, MovingParam movingParam)
        {
            if (position == null || position.Count() <= axisNo)
            {
                return false;
            }

            position[axisNo] += offset;

            return true;
        }

        public override bool ContinuousMove(int axisNo, MovingParam movingParam, bool negative)
        {
            return true;
        }

        public override void StopMove(int axisNo)
        {

        }

        public override void EmergencyStop(int axisNo)
        {
        }

        public override bool IsMoveDone(int axisNo)
        {
            return true;
        }

        public override bool IsAmpFault(int axisNo)
        {
            return false;
        }

        public override bool IsHomeOn(int axisNo)
        {
            return position[axisNo] == 0;
        }

        public override bool IsHomeDone(int axisNo)
        {
            return position[axisNo] == 0;
        }

        public override bool IsPositiveOn(int axisNo)
        {
            return false;
        }

        public override bool IsNegativeOn(int axisNo)
        {
            return false;
        }

        public override MotionStatus GetMotionStatus(int axisNo)
        {
            return new MotionStatus();
        }

        public override bool IsServoOn(int axisNo)
        {
            return true;
        }

        public override bool ResetAlarm(int axisNo)
        {
            return true;
        }

        public override bool ResetAlarmOn(int axisNo, bool isOn)
        {
            return true;
        }

        public override bool IsEmgStop(int axisNo)
        {
            return false;
        }
    }
}
