using System.Collections.Generic;
using FlauLib.Tools.UndoRedo;
using FlauLib.Tools.UndoRedo.Commands;

namespace FlauLib.Tests.UndoRedo
{
    public interface ITestObject
    {
        string Name { get; set; }
        List<int> Numbers { get; set; }
        void AddNumber(int newNumber);
        void RemoveNumber(int numberToRemove);
        void Redo();
        void Undo();
    }

    public class SkipInverseTestObject : UndoRedoObject, ITestObject
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                var oldValue = _name;
                _name = value;
                UndoRedoManager.Push(new SingleCommand(() => Name = oldValue) { SkipInverse = true });
            }
        }

        public List<int> Numbers { get; set; }

        public SkipInverseTestObject()
        {
            Numbers = new List<int>();
        }

        public void AddNumber(int newNumber)
        {
            Numbers.Add(newNumber);
            UndoRedoManager.Push(new SingleCommand(() => RemoveNumber(newNumber)) { SkipInverse = true });
        }

        public void RemoveNumber(int numberToRemove)
        {
            Numbers.Remove(numberToRemove);
            UndoRedoManager.Push(new SingleCommand(() => AddNumber(numberToRemove)) { SkipInverse = true });
        }
    }

    public class AutoUndoRedoTestObject : UndoRedoObject, ITestObject
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                var oldValue = _name;
                UndoRedoManager.Execute(new UndoCommand(() => InternalSetName(value), () => InternalSetName(oldValue)));
            }
        }

        public List<int> Numbers { get; set; }

        public AutoUndoRedoTestObject()
        {
            Numbers = new List<int>();
        }

        public void AddNumber(int newNumber)
        {
            UndoRedoManager.Execute(new UndoCommand(() => InternalAddNumber(newNumber), () => InternalRemoveNumber(newNumber)));
        }

        public void RemoveNumber(int numberToRemove)
        {
            UndoRedoManager.Execute(new UndoCommand(() => InternalRemoveNumber(numberToRemove), () => InternalAddNumber(numberToRemove)));
        }

        private void InternalAddNumber(int number)
        {
            Numbers.Add(number);
        }

        private void InternalRemoveNumber(int number)
        {
            Numbers.Remove(number);
        }

        private void InternalSetName(string value)
        {
            _name = value;
            // Additional things like: NotifyPropertyChanged("Name");
        }
    }
}