using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;
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
    public class ConsignorScenarioTests : FunctionalTest
    {
        [TestMethod]
        public void AddNewConsignorIsPersisted()
        {
                
                using (var bc = new BusinessContext())
                {
                    Consignor consignor = new Consignor
                    {
                        DateAdded = DateTime.Now,
                        Id = 1,
                        Consignor_Person = new Person
                        {
                            FirstName = "Bob",
                            LastName = "Jones",

                            EmailAddresses = new Collection<Email>
                            {
                                new Email
                                {
                                    EmailAddress = "bj@ala.edu"
                                }
                            },

                            MailingAddresses = new Collection<MailingAddress> 
                            { 
                                new MailingAddress
                                {
                                    MailingAddress1 = "100 My Street",
                                    MailingAddress2 = null,
                                    City = "My City",
                                    State = "AL",
                                    ZipCode = "12345"
                                }
                            },
                            PhoneNumbers = new PhoneNumber
                                {
                                    CellPhoneNumber = "1234567890",
                                    HomePhoneNumber = null,
                                    AltPhoneNumber = null,
                                    //PhoneNumber_Person = person
                                },
                        },
                    };

                    bc.AddNewConsignor(consignor);

                bool exists = bc.DataContext.Persons.Find(1).Consignor.DateAdded.Date == consignor.DateAdded.Date;

                Assert.IsTrue(exists);
            }
        }

        //        [TestMethod]
        //        public void AddNewBookIsPersistedWhenOptionalFieldsAreNull()
        //        {

        //            using (var bc = new BusinessContext())
        //            {
        //                var book = new Book
        //                {
        //                    Title = "Book",
        //                    ISBN = "1234567890",
        //                    Author = "Bill Nye",
        //                    Items_Books = new List<Item>{
        //                        new Item
        //                        {
        //                            Id = 1,
        //                            ItemType = "Book",
        //                            ListedPrice = 3.00,
        //                            ListedDate = DateTime.Parse("12/25/2014"),
        //                            Location = "F2",
        //                            Status = "Shelved",
        //                            Condition = "Good"
        //                        }
        //                    }
        //                };

        //                bc.AddNewBook(book);

        //                bool exists = bc.DataContext.Books.Any(c => c.Id == book.Id);

        //                Assert.IsTrue(exists);
        //            }
        //        }

        //        [TestMethod]
        //        public void GetAllBooksReturnsListOfAllDatabaseEntries()
        //        {
        //            using (var bc = new BusinessContext())
        //            {
        //                var book = new Book
        //                {
        //                    Title = "Book",
        //                    ISBN = "1234567890",
        //                    Author = "Bill Nye",
        //                    Binding = "Hardcover",
        //                    NumberOfPages = 100,
        //                    PublicationDate = DateTime.Now,
        //                    TradeInValue = 1.00,
        //                    BookImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
        //                    Items_Books = new List<Item>{
        //                        new Item
        //                        {
        //                            Id = 1,
        //                            ItemType = "Book",
        //                            ListedPrice = 3.00,
        //                            ListedDate = DateTime.Parse("12/25/2014"),
        //                            Location = "F2",
        //                            Status = "Shelved",
        //                            Condition = "Good"
        //                        }
        //                    }
        //                };

        //                bc.AddNewBook(book);

        //                var book2 = new Book
        //                {
        //                    Title = "Book2",
        //                    ISBN = "1234567891",
        //                    Author = "Bill Wye",
        //                    Binding = "Hardcover",
        //                    NumberOfPages = 100,
        //                    PublicationDate = DateTime.Now,
        //                    TradeInValue = 2.00,
        //                    BookImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
        //                    Items_Books = new List<Item>{
        //                        new Item
        //                        {
        //                            Id = 2,
        //                            ItemType = "Book",
        //                            ListedPrice = 3.00,
        //                            ListedDate = DateTime.Parse("12/25/2014"),
        //                            Location = "F2",
        //                            Status = "Shelved",
        //                            Condition = "Good"
        //                        }
        //                    }
        //                };

        //                bc.AddNewBook(book2);

        //                var BookList = bc.GetAllBooks();

        //                bool exists = BookList.Any(c => c.Id == book.Id);
        //                bool exists2 = BookList.Any(c => c.Id == book2.Id);

        //                Assert.IsTrue(exists);
        //                Assert.IsTrue(exists2);
        //            }
        //        }

        //        [TestMethod]
        //        [ExpectedException(typeof(ArgumentException))]
        //        public void AddNewBookThrowsExceptionIfIdAlreadyExists()
        //        {
        //            using (var bc = new BusinessContext())
        //            {
        //                var book = new Book
        //                    {
        //                        Title = "Book",
        //                        ISBN = "1234567890",
        //                        Author = "Bill Nye",
        //                        Binding = "Hardcover",
        //                        NumberOfPages = 100,
        //                        PublicationDate = DateTime.Now,
        //                        TradeInValue = 1.00,
        //                        BookImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
        //                        Items_Books = new List<Item>{
        //                            new Item
        //                            {
        //                                Id = 1,
        //                                ItemType = "Book",
        //                                ListedPrice = 3.00,
        //                                ListedDate = DateTime.Parse("12/25/2014"),
        //                                Location = "F2",
        //                                Status = "Shelved",
        //                                Condition = "Good"
        //                            }
        //                        }
        //                    };

        //                var book2 = new Book
        //                {
        //                    Title = "Book2",
        //                    ISBN = "1234567890",
        //                    Author = "Bill Vye",
        //                    Binding = "Hardcover",
        //                    NumberOfPages = 100,
        //                    PublicationDate = DateTime.Now,
        //                    TradeInValue = 1.00,
        //                    BookImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
        //                    Items_Books = new List<Item>{
        //                            new Item
        //                            {
        //                                Id = 1,
        //                                ItemType = "Book",
        //                                ListedPrice = 3.00,
        //                                ListedDate = DateTime.Parse("12/25/2014"),
        //                                Location = "F2",
        //                                Status = "Shelved",
        //                                Condition = "Good"
        //                            }
        //                        }

        //                };

        //                bc.AddNewBook(book);
        //                bc.AddNewBook(book2);
        //            }
        //        }

        //        [TestMethod]
        //        [ExpectedException(typeof(ArgumentException))]
        //        public void AddNewBookThrowsExceptionIfMoreThanOneIdIsAdded()
        //        {
        //            using (var bc = new BusinessContext())
        //            {
        //                var book = new Book
        //                {
        //                    Title = "Book",
        //                    ISBN = "1234567890",
        //                    Author = "Bill Nye",
        //                    Binding = "Hardcover",
        //                    NumberOfPages = 100,
        //                    PublicationDate = DateTime.Now,
        //                    TradeInValue = 1.00,
        //                    BookImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
        //                    Items_Books = new List<Item>{
        //                        new Item
        //                        {
        //                            Id = 1,
        //                            ItemType = "Book",
        //                            ListedPrice = 3.00,
        //                            ListedDate = DateTime.Parse("12/25/2014"),
        //                            Location = "F2",
        //                            Status = "Shelved",
        //                            Condition = "Good"
        //                        },
        //                        new Item
        //                        {
        //                            Id = 2,
        //                            ItemType = "Book",
        //                            ListedPrice = 3.00,
        //                            ListedDate = DateTime.Parse("12/25/2014"),
        //                            Location = "F2",
        //                            Status = "Shelved",
        //                            Condition = "Good"
        //                        }
        //                    }
        //                };

        //                bc.AddNewBook(book);
        //            }
        //        }

        //        [TestMethod]
        //        public void GetItemByIsbnReturnsCorrectDatabaseEntry()
        //        {
        //            using (var bc = new BusinessContext())
        //            {
        //                var book = new Book
        //                {
        //                    Title = "Book",
        //                    ISBN = "1234567890",
        //                    Author = "Bill Nye",
        //                    Binding = "Hardcover",
        //                    NumberOfPages = 100,
        //                    PublicationDate = DateTime.Now,
        //                    TradeInValue = 1.00,
        //                    BookImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
        //                    Items_Books = new List<Item>{
        //                        new Item
        //                        {
        //                            Id = 1,
        //                            ItemType = "Book",
        //                            ListedPrice = 3.00,
        //                            ListedDate = DateTime.Parse("12/25/2014"),
        //                            Location = "F2",
        //                            Status = "Shelved",
        //                            Condition = "Good"
        //                        }
        //                    }
        //                };

        //                bc.AddNewBook(book);

        //                Book TestBook = bc.GetBookByIsbn(book.ISBN);

        //                bool isThere = TestBook.Id == book.Id;

        //                Assert.IsTrue(isThere);

        //            }
        //        }

        //        [TestMethod]
        //        [ExpectedException(typeof(ArgumentNullException))]
        //        public void AddNewBookThrowsExceptionIfTitleIsNull()
        //        {
        //            using (var bc = new BusinessContext())
        //            {
        //                var book = new Book
        //                {
        //                    Title = null,
        //                    ISBN = "1234567890",
        //                    Author = "Bill Nye",
        //                    Items_Books = new List<Item>{
        //                        new Item
        //                        {
        //                            Id = 1,
        //                            ItemType = "Book",
        //                            ListedPrice = 3.00,
        //                            ListedDate = DateTime.Parse("12/25/2014"),
        //                            Location = "F2",
        //                            Status = "Shelved",
        //                            Condition = "Good"
        //                        }
        //                    }
        //                };

        //                bc.AddNewBook(book);
        //            }
        //        }

        //        [TestMethod]
        //        [ExpectedException(typeof(ArgumentException))]
        //        public void AddNewBookThrowsExceptionIfTitleIsEmpty()
        //        {
        //            using (var bc = new BusinessContext())
        //            {
        //                var book = new Book
        //                {
        //                    Title = "",
        //                    ISBN = "1234567890",
        //                    Author = "Bill Nye",
        //                    Items_Books = new List<Item>{
        //                        new Item
        //                        {
        //                            Id = 1,
        //                            ItemType = "Book",
        //                            ListedPrice = 3.00,
        //                            ListedDate = DateTime.Parse("12/25/2014"),
        //                            Location = "F2",
        //                            Status = "Shelved",
        //                            Condition = "Good"
        //                        }
        //                    }
        //                };

        //                bc.AddNewBook(book);
        //            }
        //        }

        //        [TestMethod]
        //        [ExpectedException(typeof(ArgumentNullException))]
        //        public void AddNewBookThrowsExceptionIfIsbnIsNull()
        //        {
        //            using (var bc = new BusinessContext())
        //            {
        //                var book = new Book
        //                {
        //                    Title = "Book",
        //                    ISBN = null,
        //                    Author = "Bill Nye",
        //                    Items_Books = new List<Item>{
        //                        new Item
        //                        {
        //                            Id = 1,
        //                            ItemType = "Book",
        //                            ListedPrice = 3.00,
        //                            ListedDate = DateTime.Parse("12/25/2014"),
        //                            Location = "F2",
        //                            Status = "Shelved",
        //                            Condition = "Good"
        //                        }
        //                    }
        //                };

        //                bc.AddNewBook(book);
        //            }
        //        }

        //        [TestMethod]
        //        [ExpectedException(typeof(ArgumentException))]
        //        public void AddNewBookThrowsExceptionIfIsbnIsEmpty()
        //        {
        //            using (var bc = new BusinessContext())
        //            {
        //                var book = new Book
        //                {
        //                    Title = "Book",
        //                    ISBN = "",
        //                    Author = "Bill Nye",
        //                    Binding = "Hardcover",
        //                    NumberOfPages = 100,
        //                    PublicationDate = DateTime.Now,
        //                    TradeInValue = 1.00,
        //                    BookImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
        //                    Items_Books = new List<Item>{
        //                        new Item
        //                        {
        //                            Id = 1,
        //                            ItemType = "Book",
        //                            ListedPrice = 3.00,
        //                            ListedDate = DateTime.Parse("12/25/2014"),
        //                            Location = "F2",
        //                            Status = "Shelved",
        //                            Condition = "Good"
        //                        }
        //                    }
        //                };

        //                bc.AddNewBook(book);
        //            }
        //        }

        //        [TestMethod]
        //        public void AddNewBookUpdatesBookTableIfIsbnAlreadyExists()
        //        {
        //            using (var bc = new BusinessContext())
        //            {
        //                var book = new Book
        //                {
        //                    Title = "Book",
        //                    ISBN = "1234567890",
        //                    Author = "Bill Nye",
        //                    Binding = "Hardcover",
        //                    NumberOfPages = 100,
        //                    PublicationDate = DateTime.Now,
        //                    TradeInValue = 1.00,
        //                    BookImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
        //                    Items_Books = new List<Item>{
        //                        new Item
        //                        {
        //                            //Id = 1,
        //                            ItemType = "Book",
        //                            ListedPrice = 3.00,
        //                            ListedDate = DateTime.Parse("12/25/2014"),
        //                            Location = "F2",
        //                            Status = "Shelved",
        //                            Condition = "Good"
        //                        }
        //                    }
        //                };
        //                bc.AddNewBook(book);

        //                var book2 = new Book
        //                {
        //                    Title = "Book",
        //                    ISBN = "1234567890",
        //                    Author = "Bill Nye",
        //                    Binding = "Hardcover",
        //                    NumberOfPages = 100,
        //                    PublicationDate = DateTime.Now,
        //                    TradeInValue = 1.00,
        //                    BookImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
        //                    Items_Books = new List<Item>
        //                    {
        //                        new Item
        //                        {
        //                            //Id = 2,
        //                            ItemType = "Book",
        //                            ListedPrice = 5.00,
        //                            ListedDate = DateTime.Parse("12/23/2014"),
        //                            Location = "F2",
        //                            Status = "Shelved",
        //                            Condition = "Good"
        //                        }
        //                    }
        //                };


        //                bc.AddNewBook(book2);

        //                List<Book> results = bc.GetAllBooks();

        //                Assert.IsFalse(results.Count(b => b.ISBN == book.ISBN) != 1);
        //                Assert.IsTrue(results.FirstOrDefault(b => b.Id == book.Id).Items_Books.Count == 2);
        //            }
        //        }

        //        [TestMethod]
        //        [ExpectedException (typeof(ArgumentNullException))]
        //        public void AddNewBookThrowsExceptionIfBookIsNotNull()
        //        {
        //            using (var bc = new BusinessContext())
        //            {
        //                Book testBook = new Book();

        //                var book = new Book
        //                {
        //                    Title = "Book",
        //                    ISBN = "1234567890",
        //                    Author = "Bill Nye",
        //                    Items_Books = new List<Item>{
        //                        new Item
        //                        {
        //                            //Id = 1,
        //                            ItemType = "Book",
        //                            ListedPrice = 3.00,
        //                            ListedDate = DateTime.Parse("12/25/2014"),
        //                            Location = "F2",
        //                            Status = "Shelved",
        //                            Condition = "Good",
        //                            Book = testBook
        //                        }
        //                    }
        //                };
        //                bc.AddNewBook(book);
        //            }
        //        }

        //        [TestMethod]
        //        public void UpdateBookSuccessfullyUpdatesBookRecord()
        //        {
        //            using (var bc = new BusinessContext())
        //            {

        //                //First Add Book
        //                Book book = new Book
        //                {
        //                    Title = "Book",
        //                    ISBN = "1234567890",
        //                    Author = "Bill Nye",
        //                    Binding = "Hardcover",
        //                    NumberOfPages = 100,
        //                    PublicationDate = DateTime.Now,
        //                    TradeInValue = 1.00,
        //                    BookImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
        //                    Items_Books = new List<Item>{
        //                        new Item
        //                        {
        //                            //Id = 1,
        //                            ItemType = "Book",
        //                            ListedPrice = 3.00,
        //                            ListedDate = DateTime.Parse("12/25/2014"),
        //                            Location = "F2",
        //                            Status = "Shelved",
        //                            Condition = "Good",
        //                        }
        //                    }
        //                };
        //                string BookISBN = book.ISBN;
        //                int? NumberOfPages = book.NumberOfPages;
        //                double? TradeInValue = book.TradeInValue;
        //                DateTime? PublicationDate = book.PublicationDate;
        //                bc.AddNewBook(book);


        //                //Then update book
        //                Book book2 = bc.GetBookByIsbn(book.ISBN);

        //                book2.ISBN = "1234567899";
        //                book2.NumberOfPages = 90;
        //                book2.PublicationDate = DateTime.Now;
        //                book2.TradeInValue = 7;

        //                bc.UpdateBook(book2);

        //                //Get original record from DB - ISBN shouldn't be there
        //                var testBook = bc.GetBookByIsbn(BookISBN);
        //                Assert.IsNull(testBook);

        //                //Get new record from DB - see if they have changed
        //                testBook = bc.GetBookByIsbn(book2.ISBN);
        //                Assert.IsTrue(book.Title == testBook.Title);
        //                Assert.IsFalse(TradeInValue == testBook.TradeInValue);
        //                Assert.IsFalse(NumberOfPages == testBook.NumberOfPages);
        //                Assert.IsFalse(PublicationDate == testBook.PublicationDate);
        //            }
        //        }

    }
}
