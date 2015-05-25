using System;
namespace FlauLib.Tools.UndoRedo.Commands
{
    /// <summary>
    /// Used for commands where the undo command is the same as the normal command
    /// </summary>
    public class SingleCommand : IUndoRedoCommand
    {
        private readonly Action _performAction;

        public SingleCommand(Action performAction)
        {
            _performAction = performAction;
        }

        public void Execute()
        {
            _performAction();
        }

        public void Undo()
        {
            _performAction();
        }

        public bool SkipInverse { get; set; }
    }
}