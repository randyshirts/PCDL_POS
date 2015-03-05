using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using RocketPos.Data.DataLayer.Entities;
using RocketPos.Data.TransactionalLayer;

namespace RocketPos.Tests.DataTests.Functional_Tests
{
    [TestClass]
    public class OtherScenarioTests : FunctionalTest
    {
        [TestMethod]
        public void AddNewOtherIsPersisted()
        {

            using (var bc = new BusinessContext())
            {
                var other = new Other
                {
                    Title = "Other",
                    EAN = "1234567890123",
                    Manufacturer = "Bill Nye",
                    OtherImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_Other = new List<Item>{
                        new Item
                        {
                            Id = 1,
                            ItemType = "Other",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewOther(other);

                bool exists = bc.DataContext.Others.Any(c => c.Id == other.Id);

                Assert.IsTrue(exists);
            }
        }
        
        [TestMethod]
        public void AddNewOtherIsPersistedWhenOptionalFieldsAreNull()
        {

            using (var bc = new BusinessContext())
            {
                var other = new Other
                {
                    Title = "Other",
                    Manufacturer = null,
                    Items_Other = new List<Item>{
                        new Item
                        {
                            Id = 1,
                            ItemType = "Other",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewOther(other);

                bool exists = bc.DataContext.Others.Any(c => c.Id == other.Id);

                Assert.IsTrue(exists);
            }
        }

        [TestMethod]
        public void GetAllOtherReturnsListOfAllDatabaseEntries()
        {
            using (var bc = new BusinessContext())
            {
                var other = new Other
                {
                    Title = "Other",
                    EAN = "1234567890123",
                    Manufacturer = "Bill Nye",
                    OtherImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_Other = new List<Item>{
                        new Item
                        {
                            Id = 1,
                            ItemType = "Other",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewOther(other);

                var other2 = new Other
                {
                    Title = "Other2",
                    EAN = "1234567891123",
                    Manufacturer = "Bill Wye",
                    OtherImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_Other = new List<Item>{
                        new Item
                        {
                            Id = 2,
                            ItemType = "Other",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewOther(other2);

                var OtherList = bc.GetAllOthers();

                bool exists = OtherList.Any(c => c.Id == other.Id);
                bool exists2 = OtherList.Any(c => c.Id == other2.Id);

                Assert.IsTrue(exists);
                Assert.IsTrue(exists2);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNewOtherThrowsExceptionIfIdAlreadyExists()
        {
            using (var bc = new BusinessContext())
            {
                var other = new Other
                    {
                        Title = "Other",
                        EAN = "1234567890123",
                        Manufacturer = "Bill Nye",
                        OtherImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                        Items_Other = new List<Item>{
                            new Item
                            {
                                Id = 1,
                                ItemType = "Other",
                                ListedPrice = 3.00,
                                ListedDate = DateTime.Parse("12/25/2014"),
                                Subject = "Math K-2",
                                Status = "Shelved",
                                Condition = "Good"
                            }
                        }
                    };

                var other2 = new Other
                {
                    Title = "Other",
                    EAN = "1234567890123",
                    Manufacturer = "Bill Vye",
                    OtherImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_Other = new List<Item>{
                            new Item
                            {
                                Id = 1,
                                ItemType = "Other",
                                ListedPrice = 3.00,
                                ListedDate = DateTime.Parse("12/25/2014"),
                                Subject = "Math K-2",
                                Status = "Shelved",
                                Condition = "Good"
                            }
                        }

                };
            
                bc.AddNewOther(other);
                bc.AddNewOther(other2);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNewOtherThrowsExceptionIfMoreThanOneIdIsAdded()
        {
            using (var bc = new BusinessContext())
            {
                var other = new Other
                {
                    Title = "Other",
                    EAN = "1234567890123",
                    Manufacturer = "Bill Nye",
                    OtherImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_Other = new List<Item>{
                        new Item
                        {
                            Id = 1,
                            ItemType = "Other",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        },
                        new Item
                        {
                            Id = 2,
                            ItemType = "Other",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewOther(other);
            }
        }

        [TestMethod]
        public void GetItemByIsbnReturnsCorrectDatabaseEntry()
        {
            using (var bc = new BusinessContext())
            {
                var other = new Other
                {
                    Title = "Other",
                    EAN = "1234567890123",
                    Manufacturer = "Bill Nye",
                    OtherImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_Other = new List<Item>{
                        new Item
                        {
                            Id = 1,
                            ItemType = "Other",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewOther(other);

                Other TestOther = bc.GetOtherByTitle(other.Title);

                bool isThere = TestOther.Id == other.Id;

                Assert.IsTrue(isThere);

            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNewOtherThrowsExceptionIfTitleIsNull()
        {
            using (var bc = new BusinessContext())
            {
                var other = new Other
                {
                    Title = null,
                    EAN = "1234567890123",
                    Manufacturer = "Bill Nye",
                    Items_Other = new List<Item>{
                        new Item
                        {
                            Id = 1,
                            ItemType = "Other",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewOther(other);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNewOtherThrowsExceptionIfTitleIsEmpty()
        {
            using (var bc = new BusinessContext())
            {
                var other = new Other
                {
                    Title = "",
                    EAN = "1234567890123",
                    Manufacturer = "Bill Nye",
                    Items_Other = new List<Item>{
                        new Item
                        {
                            Id = 1,
                            ItemType = "Other",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewOther(other);
            }
        }

        [TestMethod]
        public void AddNewOtherUpdatesOtherTableIfEanAlreadyExists()
        {
            using (var bc = new BusinessContext())
            {
                var other = new Other
                {
                    Title = "Other",
                    EAN = "1234567890123",
                    Manufacturer = "Bill Nye",
                    OtherImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_Other = new List<Item>{
                        new Item
                        {
                            //Id = 1,
                            ItemType = "Other",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };
                bc.AddNewOther(other);

                var other2 = new Other
                {
                    Title = "Other",
                    EAN = "1234567890123",
                    Manufacturer = "Bill Nye",
                    OtherImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_Other = new List<Item>
                    {
                        new Item
                        {
                            //Id = 2,
                            ItemType = "Other",
                            ListedPrice = 5.00,
                            ListedDate = DateTime.Parse("12/23/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                
                bc.AddNewOther(other2);

                List<Other> results = bc.GetAllOthers();

                Assert.IsFalse(results.Count(b => b.EAN == other.EAN) != 1);
                Assert.IsTrue(results.FirstOrDefault(b => b.Id == other.Id).Items_Other.Count == 2);
            }
        }

        [TestMethod]
        [ExpectedException (typeof(ArgumentNullException))]
        public void AddNewOtherThrowsExceptionIfOtherIsNotNull()
        {
            using (var bc = new BusinessContext())
            {
                Other testOther = new Other();

                var other = new Other
                {
                    Title = "Other",
                    EAN = "1234567890123",
                    Manufacturer = "Bill Nye",
                    Items_Other = new List<Item>{
                        new Item
                        {
                            ItemType = "Other",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good",
                            Other = testOther
                        }
                    }
                };
                bc.AddNewOther(other);
            }
        }

    }
}
