using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Base
{
    public interface ICommand
    {
        void Execute();
        void Undo();
        void Redo();
    }

    public class CommandManager
    {
        private List<ICommand> undoCommandList = new List<ICommand>();
        private List<ICommand> redoCommandList = new List<ICommand>();

        public void Execute(ICommand command)
        {
            command.Execute();
            redoCommandList.Clear();
            undoCommandList.Add(command);
        }

        public void Undo()
        {
            if (undoCommandList.Count <= 0)
            {
                return;
            }

            ICommand command = undoCommandList.Last();
            command.Undo();
            redoCommandList.Add(command);
            undoCommandList.Remove(command);
        }

        public void Redo()
        {
            if (redoCommandList.Count <= 0)
            {
                return;
            }

            ICommand command = redoCommandList.Last();
            command.Redo();
            undoCommandList.Add(command);
            redoCommandList.Remove(command);
        }
    }
}
