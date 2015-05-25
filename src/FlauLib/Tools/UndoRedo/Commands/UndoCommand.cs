using System;
namespace FlauLib.Tools.UndoRedo.Commands
{
    /// <summary>
    /// Default command with separate do and undo action
    /// </summary>
    public class UndoCommand : IUndoRedoCommand
    {
        private readonly Action _doAction;
        private readonly Action _undoAction;

        public UndoCommand(Action doAction, Action undoAction)
        {
            _doAction = doAction;
            _undoAction = undoAction;
        }

        public void Execute()
        {
            _doAction();
        }

        public void Undo()
        {
            _undoAction();
        }

        public bool SkipInverse { get; set; }
    }
}