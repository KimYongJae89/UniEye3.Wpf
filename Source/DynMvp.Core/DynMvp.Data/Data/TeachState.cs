using DynMvp.Base;
using DynMvp.Devices.FrameGrabber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Data
{
    public class TeachState
    {
        public bool LockMove { get; set; }
        public Camera SelectedCamera { get; set; }
        public ImageD TargetGroupImage { get; set; }

        private static TeachState _instance;
        public static TeachState Instance()
        {
            if (_instance == null)
            {
                _instance = new TeachState();
            }

            return _instance;
        }
    }
}
