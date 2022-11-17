using UniScanC.Enums;

namespace UniScanC.Module
{
    // 두께 측정기
    public class ThicknessModuleState : ModuleState
    {
        public ThicknessModuleState() : base()
        {
            ModuleStateType = EModuleStateType.Thickness;
        }

        public override string ToString()
        {
            return "Thickness";
        }
    }
}
