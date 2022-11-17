using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Data.Forms
{
    public class ChangeParameterCommand : ICommand
    {
        private Algorithm algorithm;
        private AlgorithmParam oldParam;
        private AlgorithmParam newParam;

        public ChangeParameterCommand(Algorithm algorithm, AlgorithmParam oldParam, AlgorithmParam newParam)
        {
            this.algorithm = algorithm;
            this.oldParam = oldParam;
            this.newParam = newParam;
        }

        void ICommand.Execute()
        {
            algorithm.Param = newParam;
        }

        void ICommand.Undo()
        {
            algorithm.Param = oldParam;
        }

        void ICommand.Redo()
        {
            algorithm.Param = newParam;
        }
    }
}
