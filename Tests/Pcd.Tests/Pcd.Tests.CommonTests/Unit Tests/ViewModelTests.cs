using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using RocketPos.Common.Foundation;

namespace RocketPos.Tests.PosTests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ViewModelTests
    {
        public ViewModelTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        //Is our viewmodel abstract
        [TestMethod]
        public void IsAbstractBaseClass()
        {
            Type t = typeof(ViewModel);
            Assert.IsTrue(t.IsAbstract);

            // TODO: Add test logic here
            //
        }

        //Is the IDataErrorInfo interface used
        [TestMethod]
        public void IsIDataErrorInfo()
        {
            Assert.IsTrue(typeof(IDataErrorInfo).IsAssignableFrom(typeof(ViewModel)));
            
        }

        //Is the view model derived from ObservableObjects 
        [TestMethod]
        public void IsObservableObjects()
        {
            Assert.IsTrue(typeof(ViewModel).BaseType == (typeof(ObservableObjects)));
        }

        //Make sure we don't use the error property
        //[TestMethod]
        //[ExpectedException(typeof(NotSupportedException))]
        //public void IDataErrorInfo_ErrorPropertyNotSupported()
        //{
        //    var viewmodel = new StubViewModel();
        //    //Throw an exception if Error property is accessed
        //    var value = viewmodel.Error;    
        //}

        //Make sure validation returns message for the correct property
        [TestMethod]
        public void IndexerReturnsErrorMessageForRequestedInvalidProperty()
        {
            var viewModel = new StubViewModel
            {
                RequiredProperty = null,
                SomeOtherProperty = null
            };

            var msg = viewModel["SomeOtherProperty"];

            Assert.AreEqual("The SomeOtherProperty field is required.", msg);

        }
        
        private class StubViewModel : ViewModel
        {
            //Define a required property
            [Required]
            public string RequiredProperty { get; set; }

            [Required]
            public string SomeOtherProperty { get; set; }
        }

        //Check  if validation works when required property has an invalid value
        [TestMethod]
        public void IndexerPropertyValidatesPropertyNameWithInvalidValue()
        {
            var viewmodel = new StubViewModel();
            Assert.IsNotNull(viewmodel["RequiredProperty"]);
        }

        //Check  if validation works when required property has a valid value
        [TestMethod]
        public void IndexerPropertyValidatesPropertyNameWithValidValue()
        {
            var viewmodel = new StubViewModel()
            {
                RequiredProperty = "Some Value"
            };
            Assert.IsNull(viewmodel["RequiredProperty"]);
        }
    }
}
