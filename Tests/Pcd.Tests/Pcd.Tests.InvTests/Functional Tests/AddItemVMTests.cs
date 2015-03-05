using System;
using Inventory.ViewModels.AddItem.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RocketPos.Common.Foundation;
using RocketPos.Data;

namespace RocketPos.Tests.InvTests
{
    [TestClass]
    public class AddItemVMTests
    {
        [TestMethod]
        public void NoValidationErrorWhenMeetsAllRequirements()
        {
            var viewModel = new AddItemVm
            {
                //CustomerName = "Life of Fred"
            };

            Assert.IsNull(viewModel["CustomerName"]);

        }

        [TestMethod]
        public void ValidationErrorWhenBarcodeNameIsNotProvided()
        {
            var viewModel = new AddItemVm
            {
                //CustomerName = null
            };

            Assert.IsNotNull(viewModel["CustomerName"]);
        }

        [TestMethod]
        public void AddItemCommandCannotExecuteWhenItemTypeIsNotValid()
        {
            var viewModel = new AddItemVm
            {
                ItemType = null,
                Subject = "Math K-5",
                Status = "Shelved",
                Condition = "Good",
                ListedPrice = 5,
                ListedDate = DateTime.Parse("12/25/2014")
            };

            Assert.IsFalse(viewModel.AddItemCommand.CanExecute(null));
        }

        [TestMethod]
        public void AddItemCommandCannotExecuteWhenSubjectIsNotValid()
        {
            var viewModel = new AddItemVm
            {
                ItemType = "Book",
                Subject = null,
                Status = "Shelved",
                Condition = "Good",
                ListedPrice = 5,
                ListedDate = DateTime.Parse("12/25/2014")
            };

            Assert.IsFalse(viewModel.AddItemCommand.CanExecute(null));
        }

        [TestMethod]
        public void AddItemCommandCannotExecuteWhenStatusIsNotValid()
        {
            var viewModel = new AddItemVm
            {
                ItemType = "Book",
                Subject = "A2",
                Status = null,
                Condition = "Good",
                ListedPrice = 5,
                ListedDate = DateTime.Parse("12/25/2014")
            };

            Assert.IsFalse(viewModel.AddItemCommand.CanExecute(null));
        }

        [TestMethod]
        public void AddItemCommandCannotExecuteWhenConditionIsNotValid()
        {
            var viewModel = new AddItemVm
            {
                ItemType = "Book",
                Subject = "Math K-5",
                Status = "Shelved",
                Condition = null,
                ListedPrice = 5,
                ListedDate = DateTime.Parse("12/25/2014")
            };

            Assert.IsFalse(viewModel.AddItemCommand.CanExecute(null));
        }

        [TestMethod]
        public void AddItemCommandAddsItemToItemsCollection()
        {
            var viewModel = new AddItemVm
            {
                ItemType = "Book",
                Subject = "Math K-5",
                Status = "Shelved",
                Condition = "Good",
                ListedPrice = 5,
                ListedDate = DateTime.Parse("12/25/2014")
            };

            viewModel.AddItemCommand.Execute();

            Assert.IsTrue(viewModel.Items.Count == 1);
        }




    }
}
