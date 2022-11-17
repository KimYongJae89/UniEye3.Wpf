using DynMvp.Base;
using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniEye.Base.Data
{
    public class ImState
    {
        public bool IsConnected { get; set; } = false;
        public bool CmdDone { get; set; } = false;
        public int ModuleIndex { get; set; }
        public OpMode OpMode { get; set; }
        public string AlarmMessage { get; set; }
    }

    public class ImStateManager
    {
        private List<ImState> imStateList = new List<ImState>();
        public List<ImState> ImStateList { get => imStateList; }

        public ImState AddImState(int moduleIndex)
        {
            ImState imState = imStateList.Find(x => x.ModuleIndex == moduleIndex);
            if (imState == null)
            {
                imState = new ImState();
                imState.ModuleIndex = moduleIndex;
                imStateList.Add(imState);
            }

            return imState;
        }

        public ImState GetImState(int moduleIndex)
        {
            return imStateList.Find(x => x.ModuleIndex == moduleIndex);
        }
    }
}
