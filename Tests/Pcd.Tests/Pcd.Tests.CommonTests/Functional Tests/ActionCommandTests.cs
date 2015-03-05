using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RocketPos.Common.Foundation;

namespace RocketPos.Tests.PosTests
{
    [TestClass]
    public class ActionCommandTests
    {
        [TestMethod]
        [ExpectedException (typeof(ArgumentNullException))]
        public void ConstructorThrowsExceptionIfActionParameterIsNull()
        {
            var command = new ActionCommand(null);
        }

        [TestMethod]
        public void ExecuteInvokesAction()
        {
            var invoked = false;

            Action<Object> action = obj => invoked = true;

            var command = new ActionCommand(action);

            command.Execute();

            Assert.IsTrue(invoked);
        }

        [TestMethod]
        public void ExecuteOverloadInvokesActionWithParameter()
        {
            var invoked = false;

            Action<Object> action = obj =>
                {
                    Assert.IsNotNull(obj);
                    invoked = true;
                };
            var command = new ActionCommand(action);

            command.Execute(new object());

            Assert.IsTrue(invoked);
        }

        [TestMethod]
        public void CanExecuteIsTrueByDefault()
        {
            var command = new ActionCommand(obj => { });
            Assert.IsTrue(command.CanExecute(null));
        }

        [TestMethod]
        public void CanExecuteOverloadExecutesTruePredicate()
        {
            Action<Object> action = obj => { };
            Predicate<Object> predicate = obj => (int)obj == 1;
            
            var command = new ActionCommand(action, predicate);
            
            Assert.IsTrue(command.CanExecute(1));
        }

        [TestMethod]
        public void CanExecuteOverloadExecutesFalsePredicate()
        {
            Action<Object> action = obj => { };
            Predicate<Object> predicate = obj => (int)obj == 1;

            var command = new ActionCommand(action, predicate);

            Assert.IsFalse(command.CanExecute(2));
        }

    }
}
