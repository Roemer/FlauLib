using System;

namespace FlauLib.Tools.UndoRedo.Commands.Additions
{
    /// <summary>
    /// Command to set / unset values
    /// </summary>
    public class ValueUndoCommand<T> : IUndoRedoCommand
    {
        private readonly T _newValue;
        private readonly T _undoValue;
        private readonly Action<T> _performAction;

        public ValueUndoCommand(T newValue, T undoValue, Action<T> performAction)
        {
            _newValue = newValue;
            _undoValue = undoValue;
            _performAction = performAction;
        }

        public void Execute()
        {
            _performAction(_newValue);
        }

        public void Undo()
        {
            _performAction(_undoValue);
        }

        public bool SkipInverse { get; set; }
    }
}