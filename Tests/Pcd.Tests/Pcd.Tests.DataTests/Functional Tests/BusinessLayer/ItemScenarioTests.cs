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
    public class ItemScenarioTests : FunctionalTest
    {
       

        [TestMethod]
        public void GetAllItemsReturnsListOfAllDatabaseEntries()
        {
            using (var bc = new BusinessContext())
            {

                var book = new Book
                {
                    Title = "Book",
                    ISBN = "1234567890",
                    Author = "Bill Nye",
                    Binding = "Hardcover",
                    NumberOfPages = 100,
                    PublicationDate = DateTime.Now,
                    TradeInValue = 1.00,
                    BookImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_Books = new List<Item>{
                        new Item
                        {
                            ItemType = "Book",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewBook(book);

                var book2 = new Book
                {
                    Title = "Secret Of Life",
                    ISBN = "1234567891",
                    Author = "God",
                    Binding = "Hardcover",
                    NumberOfPages = 1,
                    PublicationDate = DateTime.Now,
                    TradeInValue = 2.00,
                    BookImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_Books = new List<Item>{
                        new Item
                        {
                            ItemType = "Book",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewBook(book2);

                var ItemList = bc.GetAllItems();

                int itemId = book.Items_Books.FirstOrDefault().Id;
                int itemId2 = book2.Items_Books.FirstOrDefault().Id;

                bool exists = ItemList.Any(c => c.Id == itemId);
                bool exists2 = ItemList.Any(c => c.Id == itemId2);

                Assert.IsTrue(exists);
                Assert.IsTrue(exists2);
            }
        }

        [TestMethod]
        public void GetItemByIdReturnsCorrectDatabaseEntry()
        {
            
            using (var bc = new BusinessContext())
            {
                var book = new Book
                {
                    Title = "Book",
                    ISBN = "1234567890",
                    Author = "Bill Nye",
                    Binding = "Hardcover",
                    NumberOfPages = 100,
                    PublicationDate = DateTime.Now,
                    TradeInValue = 1.00,
                    BookImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_Books = new List<Item>{
                        new Item
                        {
                            Id = 1,
                            ItemType = "Book",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewBook(book);
                
                int itemId = book.Items_Books.FirstOrDefault().Id;
                Item result = bc.GetItemById(itemId);

                bool isThere = result.Id == itemId;

                Assert.IsNotNull(result);
                Assert.IsTrue(isThere);
            }
        }

        [TestMethod]
        [Obsolete]
        public void AddNewItemAddsBookItemWhenPresentAndItPersists()
        {
            using (var bc = new BusinessContext())
            {
                var item = new Item
                {
                    //Id = "2014000001",
                    ItemType = "Book",
                    ListedPrice = 3.00,
                    ListedDate = DateTime.Parse("12/25/2014"),
                    Subject = "Math K-2",
                    Status = "Shelved",
                    Condition = "Good",
                    Book = new Book
                    {
                        Title = "Book of Life",
                        Author = "God",
                        ISBN = "99999999999",
                        Binding = "Hardcover",
                        NumberOfPages = 100,
                        PublicationDate = DateTime.Now,
                        TradeInValue = 1.00,
                        BookImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg"
                    }
                };

                bc.AddNewItem(item);

                Book result = bc.GetBookByIsbn(item.Book.ISBN);

                bool isThere = result.Id == item.Book.Id;

                Assert.IsNotNull(result);
                Assert.IsTrue(isThere);
            }
        }
    }
}
