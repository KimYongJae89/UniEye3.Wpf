using UniScanC.Enums;

namespace UniScanC.Module
{
    // 두께 측정기
    public class GlossModuleState : ModuleState
    {
        public GlossModuleState() : base()
        {
            ModuleStateType = EModuleStateType.Gloss;
        }

        public override string ToString()
        {
            return "Gloss";
        }
    }
}
