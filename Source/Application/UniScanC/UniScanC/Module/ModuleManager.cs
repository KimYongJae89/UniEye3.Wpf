using System.Collections.Generic;
using UniScanC.Enums;

namespace UniScanC.Module
{
    public class ModuleManager
    {
        private static ModuleManager _instance = null;
        public static ModuleManager Instance => _instance ?? (_instance = new ModuleManager());

        public List<ModuleState> ModuleStateList { get; private set; } = new List<ModuleState>();

        public ModuleState AddInspectModuleState(InspectModuleInfo moduleInfo)
        {
            int moduleIndex = moduleInfo.ModuleNo;
            ModuleState moduleState = GetModuleState(EModuleStateType.Inspect, moduleIndex);

            if (moduleState == null)
            {
                var inspectModuleState = new InspectModuleState();
                inspectModuleState.ModuleIndex = moduleIndex;
                inspectModuleState.StartPos = moduleInfo.StartPos;
                moduleState = inspectModuleState;
                ModuleStateList.Add(moduleState);
            }

            return moduleState;
        }

        public ModuleState AddThicknessModuleState()
        {
            ModuleState moduleState = GetModuleState(EModuleStateType.Thickness);

            if (moduleState == null)
            {
                var thicknessModuleState = new ThicknessModuleState();
                moduleState = thicknessModuleState;
                ModuleStateList.Add(moduleState);
            }

            return moduleState;
        }

        public ModuleState AddGlossModuleState()
        {
            ModuleState moduleState = GetModuleState(EModuleStateType.Gloss);

            if (moduleState == null)
            {
                var thicknessModuleState = new GlossModuleState();
                moduleState = thicknessModuleState;
                ModuleStateList.Add(moduleState);
            }

            return moduleState;
        }

        public ModuleState GetModuleState(EModuleStateType type, int moduleIndex = -1)
        {
            ModuleState moduleState = null;

            switch (type)
            {
                case EModuleStateType.Inspect:
                    moduleState = ModuleStateList.Find(x => x is InspectModuleState && ((InspectModuleState)x).ModuleIndex == moduleIndex);
                    break;
                case EModuleStateType.Thickness:
                    moduleState = ModuleStateList.Find(x => x is ThicknessModuleState);
                    break;
                case EModuleStateType.Gloss:
                    moduleState = ModuleStateList.Find(x => x is GlossModuleState);
                    break;
            }

            return moduleState;
        }
    }
}
