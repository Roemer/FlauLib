namespace FlauLib.Tools.UndoRedo
{
    public interface IUndoRedoCommand
    {
        /// <summary>
        /// Execute the command
        /// </summary>
        void Execute();

        /// <summary>
        /// Execute the undo command
        /// </summary>
        void Undo();

        /// <summary>
        /// Boolean flag to set if the inverse command should be added or not
        /// when undo or redo is called.
        /// Should be set to true if an undo or redo method adds a command on it's own.
        /// </summary>
        bool SkipInverse { get; set; }

        /// <summary>
        /// Ignore commands which are added during an execute/undo operation
        /// </summary>
        //bool IgnoreAdditionalCommands { get; set; }
    }
}