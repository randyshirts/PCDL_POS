using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RocketPos.Common.Foundation;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RocketPos.Tests.PosTests
{
    [TestClass]
    public class ObservableObjectsTests
    {
        [TestMethod]
        public void OnPropertyChangeEventHandlerIsRaised()
        {
            var obj = new StubObservableEvent();

            bool raised = false;
            obj.PropertyChanged += (sender, e) =>
                {
                    Assert.IsTrue(e.PropertyName == "ChangedProperty");
                    raised = true;
                };

            obj.ChangedProperty = "Some Value";

            if(!raised)
            {
                Assert.Fail("OnPopertyChange was never invoked");
            }
        }
    }

    class StubObservableEvent : ViewModel
    {
        private string changedProperty;

        public String ChangedProperty
        {
            get
            {
                return changedProperty;
            }

            set
            {
                changedProperty = value;
                OnPropertyChanged();
            }

        }
    }
}
