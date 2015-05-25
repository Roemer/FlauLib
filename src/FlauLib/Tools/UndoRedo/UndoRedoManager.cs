using System.Collections.Generic;
using System.Linq;

namespace FlauLib.Tools.UndoRedo
{
    public class UndoRedoManager
    {
        private readonly Stack<IUndoRedoCommand> _undoCommandStack;
        private readonly Stack<IUndoRedoCommand> _redoCommandStack;
        private readonly object _lockObject = new object();

        private bool _executeInProgress = false;
        private bool _pushInProgress = false;
        private bool _undoInProgress = false;
        private bool _redoInProgress = false;

        public UndoRedoManager()
        {
            _undoCommandStack = new Stack<IUndoRedoCommand>();
            _redoCommandStack = new Stack<IUndoRedoCommand>();
        }

        /// <summary>
        /// Executes the action and pushes it to the appropriate stack
        /// </summary>
        public void Execute(IUndoRedoCommand command)
        {
            lock (_lockObject)
            {
                _executeInProgress = true;
                command.Execute();
                Push(command);
                _executeInProgress = false;
            }
        }

        /// <summary>
        /// Pushes the command to the appropriate stack
        /// </summary>
        public void Push(IUndoRedoCommand command)
        {
            lock (_lockObject)
            {
                _pushInProgress = true;
                if (!_undoInProgress && !_redoInProgress)
                {
                    // Adding a new item to the stack, clear the redo stack first
                    _redoCommandStack.Clear();
                }
                // Now, add the item to the appropriate stack ...
                // ... which is the redo-stack when an undo is in progress
                if (_undoInProgress)
                {
                    _redoCommandStack.Push(command);
                }
                else
                {
                    // ... or just to the normal undo-stack otherwise
                    _undoCommandStack.Push(command);
                }
                _pushInProgress = false;
            }
        }

        /// <summary>
        /// Undo the last command from the stack
        /// </summary>
        public void Undo()
        {
            lock (_lockObject)
            {
                if (_undoCommandStack.Any())
                {
                    _undoInProgress = true;
                    var command = _undoCommandStack.Pop();
                    command.Undo();
                    // Only re-add the command to the redo stack if this flag is not set
                    if (!command.SkipInverse)
                    {
                        _redoCommandStack.Push(command);
                    }
                    _undoInProgress = false;
                }
            }
        }

        /// <summary>
        /// Redo the last command from the stack
        /// </summary>
        public void Redo()
        {
            lock (_lockObject)
            {
                if (_redoCommandStack.Any())
                {
                    _redoInProgress = true;
                    var command = _redoCommandStack.Pop();
                    command.Execute();
                    // Only re-add the command to the undo stack if this flag is not set
                    if (!command.SkipInverse)
                    {
                        _undoCommandStack.Push(command);
                    }
                    _redoInProgress = false;
                }
            }
        }

        public void Clear(bool undoAll = false)
        {
            lock (_lockObject)
            {
                if (undoAll)
                {
                    foreach (var undo in _undoCommandStack)
                    {
                        undo.Undo();
                    }
                }
                _undoCommandStack.Clear();
                _redoCommandStack.Clear();
                _executeInProgress = false;
                _pushInProgress = false;
                _undoInProgress = false;
                _redoInProgress = false;
            }
        }
    }
}