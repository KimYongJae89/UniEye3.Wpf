using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Data.Forms
{
    public class ChangeFiducialCommand : ICommand
    {
        private Probe probe;
        private bool useFiducialProbe;

        public ChangeFiducialCommand(Probe probe, bool useFiducialProbe)
        {
            this.probe = probe;
            this.useFiducialProbe = useFiducialProbe;
        }

        void ICommand.Execute()
        {
            //if (useFiducialProbe)
            //    probe.Target.SetFiducialProbe(probe);
            //else
            //    probe.Target.DeselectFiducialProbe(probe.Id);
        }

        void ICommand.Redo()
        {
            //if (useFiducialProbe)
            //    probe.Target.SelectFiducialProbe(probe.Id);
            //else
            //    probe.Target.DeselectFiducialProbe(probe.Id);
        }

        void ICommand.Undo()
        {
            //if (useFiducialProbe)
            //    probe.Target.DeselectFiducialProbe(probe.Id);
            //else
            //    probe.Target.SelectFiducialProbe(probe.Id);
        }
    }
}
