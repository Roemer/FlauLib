using System.ComponentModel;

namespace FlauLib.Tools.UndoRedo
{
    public class UndoRedoObject : IEditableObject
    {
         public UndoRedoManager UndoRedoManager { get; private set; }

         protected UndoRedoObject()
        {
            UndoRedoManager = new UndoRedoManager();
        }

        public void BeginEdit()
        {
            UndoRedoManager.Clear();
        }

        public void EndEdit()
        {
            UndoRedoManager.Clear();
        }

        public void CancelEdit()
        {
            UndoRedoManager.Clear(true);
        }

        public void Redo()
        {
            UndoRedoManager.Redo();
        }

        public void Undo()
        {
            UndoRedoManager.Undo();
        }
    }
}