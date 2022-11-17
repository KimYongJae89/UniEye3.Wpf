using UniScanC.Enums;

namespace UniScanC.Module
{
    // 비젼 검사기
    public class InspectModuleState : ModuleState
    {
        private int moduleIndex;
        public int ModuleIndex
        {
            get => moduleIndex;
            set => Set(ref moduleIndex, value);
        }

        private double startPos;
        public double StartPos
        {
            get => startPos;
            set => Set(ref startPos, value);
        }

        public InspectModuleState() : base()
        {
            ModuleStateType = EModuleStateType.Inspect;
        }

        public override string ToString()
        {
            return string.Format("Inspect/{0}", ModuleIndex);
        }
    }
}
