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
    public class TeachingAideScenarioTests : FunctionalTest
    {
        [TestMethod]
        public void AddNewTeachingAideIsPersisted()
        {

            using (var bc = new BusinessContext())
            {
                var teachingAide = new TeachingAide
                {
                    Title = "TeachingAide",
                    EAN = "1234567890123",
                    Manufacturer = "Bill Nye",
                    TeachingAideImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_TeachingAide = new List<Item>{
                        new Item
                        {
                            Id = 1,
                            ItemType = "TeachingAide",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewTeachingAide(teachingAide);

                bool exists = bc.DataContext.TeachingAides.Any(c => c.Id == teachingAide.Id);

                Assert.IsTrue(exists);
            }
        }
        
        [TestMethod]
        public void AddNewTeachingAideIsPersistedWhenOptionalFieldsAreNull()
        {

            using (var bc = new BusinessContext())
            {
                var teachingAide = new TeachingAide
                {
                    Title = "TeachingAide",
                    Manufacturer = null,
                    Items_TeachingAide = new List<Item>{
                        new Item
                        {
                            Id = 1,
                            ItemType = "TeachingAide",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewTeachingAide(teachingAide);

                bool exists = bc.DataContext.TeachingAides.Any(c => c.Id == teachingAide.Id);

                Assert.IsTrue(exists);
            }
        }

        [TestMethod]
        public void GetAllTeachingAidesReturnsListOfAllDatabaseEntries()
        {
            using (var bc = new BusinessContext())
            {
                var teachingAide = new TeachingAide
                {
                    Title = "TeachingAide",
                    EAN = "1234567890123",
                    Manufacturer = "Bill Nye",
                    TeachingAideImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_TeachingAide = new List<Item>{
                        new Item
                        {
                            Id = 1,
                            ItemType = "TeachingAide",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewTeachingAide(teachingAide);

                var teachingAide2 = new TeachingAide
                {
                    Title = "TeachingAide2",
                    EAN = "1234567891123",
                    Manufacturer = "Bill Wye",
                    TeachingAideImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_TeachingAide = new List<Item>{
                        new Item
                        {
                            Id = 2,
                            ItemType = "TeachingAide",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewTeachingAide(teachingAide2);

                var TeachingAideList = bc.GetAllTeachingAides();

                bool exists = TeachingAideList.Any(c => c.Id == teachingAide.Id);
                bool exists2 = TeachingAideList.Any(c => c.Id == teachingAide2.Id);

                Assert.IsTrue(exists);
                Assert.IsTrue(exists2);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNewTeachingAideThrowsExceptionIfIdAlreadyExists()
        {
            using (var bc = new BusinessContext())
            {
                var teachingAide = new TeachingAide
                    {
                        Title = "TeachingAide",
                        EAN = "1234567890123",
                        Manufacturer = "Bill Nye",
                        TeachingAideImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                        Items_TeachingAide = new List<Item>{
                            new Item
                            {
                                Id = 1,
                                ItemType = "TeachingAide",
                                ListedPrice = 3.00,
                                ListedDate = DateTime.Parse("12/25/2014"),
                                Subject = "Math K-2",
                                Status = "Shelved",
                                Condition = "Good"
                            }
                        }
                    };

                var teachingAide2 = new TeachingAide
                {
                    Title = "TeachingAide",
                    EAN = "1234567890123",
                    Manufacturer = "Bill Vye",
                    TeachingAideImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_TeachingAide = new List<Item>{
                            new Item
                            {
                                Id = 1,
                                ItemType = "TeachingAide",
                                ListedPrice = 3.00,
                                ListedDate = DateTime.Parse("12/25/2014"),
                                Subject = "Math K-2",
                                Status = "Shelved",
                                Condition = "Good"
                            }
                        }

                };
            
                bc.AddNewTeachingAide(teachingAide);
                bc.AddNewTeachingAide(teachingAide2);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNewTeachingAideThrowsExceptionIfMoreThanOneIdIsAdded()
        {
            using (var bc = new BusinessContext())
            {
                var teachingAide = new TeachingAide
                {
                    Title = "TeachingAide",
                    EAN = "1234567890123",
                    Manufacturer = "Bill Nye",
                    TeachingAideImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_TeachingAide = new List<Item>{
                        new Item
                        {
                            Id = 1,
                            ItemType = "TeachingAide",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        },
                        new Item
                        {
                            Id = 2,
                            ItemType = "TeachingAide",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewTeachingAide(teachingAide);
            }
        }

        [TestMethod]
        public void GetItemByIsbnReturnsCorrectDatabaseEntry()
        {
            using (var bc = new BusinessContext())
            {
                var teachingAide = new TeachingAide
                {
                    Title = "TeachingAide",
                    EAN = "1234567890123",
                    Manufacturer = "Bill Nye",
                    TeachingAideImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_TeachingAide = new List<Item>{
                        new Item
                        {
                            Id = 1,
                            ItemType = "TeachingAide",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewTeachingAide(teachingAide);

                TeachingAide TestTeachingAide = bc.GetTeachingAideByTitle(teachingAide.Title);

                bool isThere = TestTeachingAide.Id == teachingAide.Id;

                Assert.IsTrue(isThere);

            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNewTeachingAideThrowsExceptionIfTitleIsNull()
        {
            using (var bc = new BusinessContext())
            {
                var teachingAide = new TeachingAide
                {
                    Title = null,
                    EAN = "1234567890123",
                    Manufacturer = "Bill Nye",
                    Items_TeachingAide = new List<Item>{
                        new Item
                        {
                            Id = 1,
                            ItemType = "TeachingAide",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewTeachingAide(teachingAide);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNewTeachingAideThrowsExceptionIfTitleIsEmpty()
        {
            using (var bc = new BusinessContext())
            {
                var teachingAide = new TeachingAide
                {
                    Title = "",
                    EAN = "1234567890123",
                    Manufacturer = "Bill Nye",
                    Items_TeachingAide = new List<Item>{
                        new Item
                        {
                            Id = 1,
                            ItemType = "TeachingAide",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewTeachingAide(teachingAide);
            }
        }

        [TestMethod]
        public void AddNewTeachingAideUpdatesTeachingAideTableIfEanAlreadyExists()
        {
            using (var bc = new BusinessContext())
            {
                var teachingAide = new TeachingAide
                {
                    Title = "TeachingAide",
                    EAN = "1234567890123",
                    Manufacturer = "Bill Nye",
                    TeachingAideImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_TeachingAide = new List<Item>{
                        new Item
                        {
                            //Id = 1,
                            ItemType = "TeachingAide",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };
                bc.AddNewTeachingAide(teachingAide);

                var teachingAide2 = new TeachingAide
                {
                    Title = "TeachingAide",
                    EAN = "1234567890123",
                    Manufacturer = "Bill Nye",
                    TeachingAideImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_TeachingAide = new List<Item>
                    {
                        new Item
                        {
                            //Id = 2,
                            ItemType = "TeachingAide",
                            ListedPrice = 5.00,
                            ListedDate = DateTime.Parse("12/23/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                
                bc.AddNewTeachingAide(teachingAide2);

                List<TeachingAide> results = bc.GetAllTeachingAides();

                Assert.IsFalse(results.Count(b => b.EAN == teachingAide.EAN) != 1);
                Assert.IsTrue(results.FirstOrDefault(b => b.Id == teachingAide.Id).Items_TeachingAide.Count == 2);
            }
        }

        [TestMethod]
        [ExpectedException (typeof(ArgumentNullException))]
        public void AddNewTeachingAideThrowsExceptionIfTeachingAideIsNotNull()
        {
            using (var bc = new BusinessContext())
            {
                TeachingAide testTeachingAide = new TeachingAide();

                var teachingAide = new TeachingAide
                {
                    Title = "TeachingAide",
                    EAN = "1234567890123",
                    Manufacturer = "Bill Nye",
                    Items_TeachingAide = new List<Item>{
                        new Item
                        {
                            ItemType = "TeachingAide",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good",
                            TeachingAide = testTeachingAide
                        }
                    }
                };
                bc.AddNewTeachingAide(teachingAide);
            }
        }

    }
}
