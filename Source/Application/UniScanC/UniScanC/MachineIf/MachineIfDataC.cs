using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base;
using UniEye.Base.MachineInterface;

namespace UniScanC.MachineIf
{
    public abstract class MachineIfDataC : UniEye.Base.MachineInterface.MachineIfDataBase
    {
        // PLC -> Controller
        public bool GET_READY_MACHINE;
        public bool GET_START_MACHINE;
        public bool GET_START_COATING;
        public bool GET_START_THICKNESS;
        public bool GET_START_GLOSS;

        public bool GET_AUTO_MODEL_COATING;
        public bool GET_AUTO_MODEL_THICKNESS;
        public bool GET_AUTO_MODEL_GLOSS;

        public float GET_TARGET_WIDTH;
        public float GET_TARGET_SPEED;
        public float GET_PRESENT_SPEED;
        public float GET_TARGET_POSITION;
        public float GET_PRESENT_POSITION;
        public float GET_TARGET_THICKNESS;
        public float GET_DEFECT_NG_COUNT;

        public string GET_LOT;
        public string GET_MODEL;
        public string GET_WORKER;
        public string GET_PASTE;

        public MachineIfDataC()
        {
            Reset();
        }

        public virtual void CopyFrom(MachineIfDataBase machineIfData)
        {
            if (machineIfData is MachineIfDataC machineIfDataC)
            {
                GET_READY_MACHINE = machineIfDataC.GET_READY_MACHINE;
                GET_START_MACHINE = machineIfDataC.GET_START_MACHINE;
                GET_START_COATING = machineIfDataC.GET_START_COATING;
                GET_START_THICKNESS = machineIfDataC.GET_START_THICKNESS;
                GET_START_GLOSS = machineIfDataC.GET_START_GLOSS;

                GET_AUTO_MODEL_COATING = machineIfDataC.GET_AUTO_MODEL_COATING;
                GET_AUTO_MODEL_THICKNESS = machineIfDataC.GET_AUTO_MODEL_THICKNESS;
                GET_AUTO_MODEL_GLOSS = machineIfDataC.GET_AUTO_MODEL_GLOSS;

                GET_TARGET_WIDTH = machineIfDataC.GET_TARGET_WIDTH;
                GET_TARGET_SPEED = machineIfDataC.GET_TARGET_SPEED;
                GET_PRESENT_SPEED = machineIfDataC.GET_PRESENT_SPEED;
                GET_TARGET_POSITION = machineIfDataC.GET_TARGET_POSITION;
                GET_PRESENT_POSITION = machineIfDataC.GET_PRESENT_POSITION;
                GET_TARGET_THICKNESS = machineIfDataC.GET_TARGET_THICKNESS;
                GET_DEFECT_NG_COUNT = machineIfDataC.GET_DEFECT_NG_COUNT;

                GET_LOT = machineIfDataC.GET_LOT;
                GET_MODEL = machineIfDataC.GET_MODEL;
                GET_WORKER = machineIfDataC.GET_WORKER;
                GET_PASTE = machineIfDataC.GET_PASTE;
            }
        }

        public abstract MachineIfDataBase Clone();

        public virtual void Reset()
        {
            // Printer -> Controller
            GET_READY_MACHINE = false;
            GET_START_MACHINE = false;
            GET_START_COATING = false;
            GET_START_THICKNESS = false;
            GET_START_GLOSS = false;

            GET_AUTO_MODEL_COATING = false;
            GET_AUTO_MODEL_THICKNESS = false;
            GET_AUTO_MODEL_GLOSS = false;

            GET_TARGET_WIDTH = 0;
            GET_TARGET_SPEED = 0;
            GET_PRESENT_SPEED = 0;
            GET_TARGET_POSITION = 0;
            GET_PRESENT_POSITION = 0;
            GET_TARGET_THICKNESS = 0;
            GET_DEFECT_NG_COUNT = 0;

            GET_LOT = "";
            GET_MODEL = "";
            GET_WORKER = "";
            GET_PASTE = "";
        }
    }
}
