using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DynMvp.Devices.MotionController
{
    public class MotionHandler
    {
        private List<Motion> motionList = new List<Motion>();

        public IEnumerator<Motion> GetEnumerator()
        {
            return motionList.GetEnumerator();
        }

        public void Add(Motion motion)
        {
            motionList.Add(motion);
        }

        public void Initialize(MotionInfoList motionInfoList, bool isVirtual)
        {
            foreach (MotionInfo motionInfo in motionInfoList)
            {
                Motion motion = MotionFactory.Create(motionInfo, isVirtual);
                if (motion != null)
                {
                    Add(motion);
                }
            }
        }

        public void Release()
        {
            foreach (Motion motion in this)
            {
                motion.Release();
            }
        }

        public Motion GetMotion(string name)
        {
            return motionList.Find(x => x.Name == name);
        }

        public Motion GetMotion(int index)
        {
            return motionList[index];
        }

        public void StopMove()
        {
            foreach (Motion motion in this)
            {
                motion.StopMove();
            }

            Thread.Sleep(1000);
        }

        public void EmergencyStop()
        {
            foreach (Motion motion in this)
            {
                motion.EmergencyStop();
            }

            Thread.Sleep(1000);
        }

        public void ResetAlarm()
        {
            foreach (Motion motion in this)
            {
                motion.ResetAlarm();
            }
        }

        public void TurnOnServo(bool bOnOff)
        {
            foreach (Motion motion in this)
            {
                motion.TurnOnServo(!bOnOff);
            }

            Thread.Sleep(1000);
        }
    }
}
