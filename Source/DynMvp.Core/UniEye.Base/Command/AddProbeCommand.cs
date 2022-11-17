using DynMvp.Base;
using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniEye.Base.Command
{
    public class AddProbeCommand : ICommand
    {
        private Target target;
        private Probe probe;
        private PositionAligner positionAligner;

        public AddProbeCommand(Target target, Probe probe, PositionAligner positionAligner)
        {
            this.target = target;
            this.probe = probe;
            this.positionAligner = positionAligner;
        }

        void ICommand.Execute()
        {
            target.AddProbe(probe);
            target.UpdateRegion();
        }

        void ICommand.Redo()
        {
            target.AddProbe(probe);
            target.UpdateRegion();
        }

        void ICommand.Undo()
        {
            target.RemoveProbe(probe);
            target.UpdateRegion();
        }
    }
}
