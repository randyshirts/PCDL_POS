using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RocketPos.Data.DataLayer.Entities;
using RocketPos.Data.TransactionalLayer;

namespace rocketPos.Tests.DataTests
{
    [TestClass]
    public class BusinessContextTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNewItem_ThrowsExceptionWhenItemTypeIsNull()
        {
            using (var bc = new BusinessContext())
            {
                var item = new Item
                {
                    ItemType    = null,
                    ListedPrice = 3.00,
                    ListedDate = DateTime.Parse("12/25/2014"),
                    Subject = "Math K-2",
                    Status = "Shelved",
                    Condition = "Good"
                };
                bc.AddNewItem(item);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNewItem_ThrowsExceptionWhenItemTypeIsEmpty()
        {
            using (var bc = new BusinessContext())
            {
                var item = new Item
                {
                    ItemType = "",
                    ListedPrice = 3.00,
                    ListedDate = DateTime.Parse("12/25/2014"),
                    Subject = "Math K-2",
                    Status = "Shelved",
                    Condition = "Good"
                };
                bc.AddNewItem(item);
            }
        }
    }
}
