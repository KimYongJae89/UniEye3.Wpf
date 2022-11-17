using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniScanC.MachineIf;

namespace WPF.UniScanCM.MachineIf
{
    internal enum MachineIfItemSet { SET_VISION_STATE };

    public class MachineIfDataCM : UniScanC.MachineIf.MachineIfDataC
    {
        #region 필드
        // Controller -> Printer
        public bool SET_VISION_COATING_INSP_READY;
        public bool SET_VISION_COATING_INSP_RUNNING;
        public bool SET_VISION_COATING_INSP_ERROR;

        public bool SET_VISION_COATING_INSP_NG_PINHOLE;
        public bool SET_VISION_COATING_INSP_NG_DUST;

        public int SET_VISION_COATING_INSP_CNT_ALL;
        public int SET_VISION_COATING_INSP_CNT_PINHOLE;
        public int SET_VISION_COATING_INSP_CNT_DUST;

        public int SET_VISION_COATING_INSP_TOTAL_CNT_ALL;
        public int SET_VISION_COATING_INSP_TOTAL_CNT_PINHOLE;
        public int SET_VISION_COATING_INSP_TOTAL_CNT_DUST;
        #endregion


        #region 생성자
        public MachineIfDataCM() : base() { }
        #endregion


        #region 속성
        #endregion


        #region 메서드
        public override void CopyFrom(MachineIfDataBase machineIfData)
        {
            base.CopyFrom(machineIfData);
            if (machineIfData is MachineIfDataCM machineIfDataCM)
            {
                SET_VISION_COATING_INSP_READY = machineIfDataCM.SET_VISION_COATING_INSP_READY;
                SET_VISION_COATING_INSP_RUNNING = machineIfDataCM.SET_VISION_COATING_INSP_RUNNING;
                SET_VISION_COATING_INSP_ERROR = machineIfDataCM.SET_VISION_COATING_INSP_ERROR;

                SET_VISION_COATING_INSP_NG_PINHOLE = machineIfDataCM.SET_VISION_COATING_INSP_NG_PINHOLE;
                SET_VISION_COATING_INSP_NG_DUST = machineIfDataCM.SET_VISION_COATING_INSP_NG_DUST;

                SET_VISION_COATING_INSP_CNT_ALL = machineIfDataCM.SET_VISION_COATING_INSP_CNT_ALL;
                SET_VISION_COATING_INSP_CNT_PINHOLE = machineIfDataCM.SET_VISION_COATING_INSP_CNT_PINHOLE;
                SET_VISION_COATING_INSP_CNT_DUST = machineIfDataCM.SET_VISION_COATING_INSP_CNT_DUST;

                SET_VISION_COATING_INSP_TOTAL_CNT_ALL = machineIfDataCM.SET_VISION_COATING_INSP_TOTAL_CNT_ALL;
                SET_VISION_COATING_INSP_TOTAL_CNT_PINHOLE = machineIfDataCM.SET_VISION_COATING_INSP_TOTAL_CNT_PINHOLE;
                SET_VISION_COATING_INSP_TOTAL_CNT_DUST = machineIfDataCM.SET_VISION_COATING_INSP_TOTAL_CNT_DUST;
            }
        }

        public override MachineIfDataBase Clone()
        {
            var machineIfDataCM = new MachineIfDataCM();
            machineIfDataCM.CopyFrom(this);
            return machineIfDataCM as MachineIfDataBase;
        }

        public void SetInspCnt(/*ProductionG productionG*/)
        {
            //this.SET_VISION_GRAVURE_INSP_CNT_ALL = productionG.Done;
            //this.SET_VISION_GRAVURE_INSP_CNT_NG = productionG.Ng;
            //this.SET_VISION_GRAVURE_INSP_CNT_PINHOLE = productionG.PinHolePatternNum;
            //this.SET_VISION_COATING_INSP_CNT_DUST = productionG.DielectricPatternNum;
        }

        public override void Reset()
        {
            base.Reset();

            // Controller -> Printer
            SET_VISION_COATING_INSP_READY = false;
            SET_VISION_COATING_INSP_RUNNING = false;
            SET_VISION_COATING_INSP_ERROR = false;

            SET_VISION_COATING_INSP_NG_PINHOLE = false;
            SET_VISION_COATING_INSP_NG_DUST = false;

            SET_VISION_COATING_INSP_CNT_ALL = 0;
            SET_VISION_COATING_INSP_CNT_PINHOLE = 0;
            SET_VISION_COATING_INSP_CNT_DUST = 0;

            SET_VISION_COATING_INSP_TOTAL_CNT_ALL = 0;
            SET_VISION_COATING_INSP_TOTAL_CNT_PINHOLE = 0;
            SET_VISION_COATING_INSP_TOTAL_CNT_DUST = 0;
        }

        public void ClearVisionNG()
        {
            SET_VISION_COATING_INSP_ERROR = false;  // 0(false)이면 정상. 1(true)이면 불량.
            SET_VISION_COATING_INSP_NG_PINHOLE = false;
            SET_VISION_COATING_INSP_NG_DUST = false;
        }
        #endregion
    }
}
