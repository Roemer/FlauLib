using System;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace FlauLib.Tests.UndoRedo
{
    [TestFixture]
    public class UndoRedoTests
    {
        [Test]
        public void SkipInverseTest()
        {
            var testObject = new SkipInverseTestObject();
            RunTestObjectTest(testObject);
        }

        [Test]
        public void AutoUndoRedoTest()
        {
            var testObject = new AutoUndoRedoTestObject();
            RunTestObjectTest(testObject);
        }

        private void RunTestObjectTest(ITestObject testObject)
        {
            // String tests
            testObject.Name = "Hello";
            testObject.Name = "World";
            testObject.Name = "!!!";
            Assert.Equals("!!!", testObject.Name);
            testObject.Undo();
            Assert.Equals("World", testObject.Name);
            testObject.Undo();
            Assert.Equals("Hello", testObject.Name);
            testObject.Redo();
            Assert.Equals("World", testObject.Name);
            testObject.Redo();
            Assert.Equals("!!!", testObject.Name);

            // List tests
            testObject.AddNumber(1);
            testObject.AddNumber(2);
            testObject.AddNumber(3);
            CollectionAssert.Equals(new[] { 1, 2, 3 }, testObject.Numbers);
            testObject.Undo();
            CollectionAssert.Equals(new[] { 1, 2 }, testObject.Numbers);
            testObject.Undo();
            CollectionAssert.Equals(new[] { 1 }, testObject.Numbers);
            testObject.Redo();
            CollectionAssert.Equals(new[] { 1, 2 }, testObject.Numbers);
            testObject.AddNumber(6);
            CollectionAssert.Equals(new[] { 1, 2, 6 }, testObject.Numbers);
            testObject.RemoveNumber(6);
            CollectionAssert.Equals(new[] { 1, 2 }, testObject.Numbers);
            testObject.Undo();
            CollectionAssert.Equals(new[] { 1, 2, 6 }, testObject.Numbers);
            testObject.Undo();
            CollectionAssert.Equals(new[] { 1, 2 }, testObject.Numbers);
        }
    }
}