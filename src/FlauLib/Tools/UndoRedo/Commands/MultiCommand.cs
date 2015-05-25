using System.Collections.Generic;

namespace FlauLib.Tools.UndoRedo.Commands
{
    /// <summary>
    /// Command to bundle multiple commands in one
    /// </summary>
    public class MultiCommand : IUndoRedoCommand
    {
        private readonly List<IUndoRedoCommand> _commands;

        public MultiCommand()
        {
            _commands = new List<IUndoRedoCommand>();
        }

        public void Execute()
        {
            foreach (var command in _commands)
            {
                command.Execute();
            }
        }

        public void Undo()
        {
            foreach (var command in _commands)
            {
                command.Undo();
            }
        }

        public bool SkipInverse { get; set; }

        public void AddCommand(IUndoRedoCommand command)
        {
            _commands.Add(command);
        }
    }
}