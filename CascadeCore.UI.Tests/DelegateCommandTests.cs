using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CascadePass.CascadeCore.UI.Tests
{
    [TestClass]
    public class DelegateCommandTests
    {
        [TestMethod]
        public void Execute_WithParameter_ShouldCallExecuteAction()
        {
            bool wasCalled = false;
            var command = new DelegateCommand(obj => wasCalled = true);

            command.Execute(null);

            Assert.IsTrue(wasCalled);
        }

        [TestMethod]
        public void Execute_WithoutParameter_ShouldCallExecuteAction()
        {
            bool wasCalled = false;
            var command = new DelegateCommand(() => wasCalled = true);

            command.Execute(null);

            Assert.IsTrue(wasCalled);
        }

        [TestMethod]
        public void CanExecute_WithParameter_ReturnsExpectedValue()
        {
            var command = new DelegateCommand(obj => { }, obj => obj != null);

            Assert.IsTrue(command.CanExecute("Valid"));
            Assert.IsFalse(command.CanExecute(null));
        }

        [TestMethod]
        public void CanExecute_WithoutParameter_ReturnsExpectedValue()
        {
            bool allow = true;
            var command = new DelegateCommand(() => { }, () => allow);

            Assert.IsTrue(command.CanExecute(null));

            allow = false;
            Assert.IsFalse(command.CanExecute(null));
        }

    }
}
