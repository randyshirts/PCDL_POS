using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RocketPos.Data.DataLayer;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RocketPos.Data.DataLayer.Entities;
using RocketPos.Data.TransactionalLayer;

namespace RocketPos.Tests.DataTests
{
    [TestClass]
    public abstract class FunctionalTest
    {
        //create a database for each test run
        [TestInitialize]
        public virtual void TestInitialize()
        {

            using (var db = new DataContext())
            {
                if (db.Database.Exists())
                    db.Database.Delete();

                db.Database.Create();

                
            }

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


                var book1 = new Book
                {
                    Title = "Good Book",
                    ISBN = "1234567899",
                    Author = "Bill Who",
                    Binding = "Hardcover",
                    Items_Books = new List<Item>{
                        new Item
                        {
                            ItemType = "Book",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/24/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good",
                            Consignor = consignor,
                            ConsignorId = consignor.Id

                        }
                    }
                };

                bc.AddNewBook(book1);

                var book2 = new Book
                {
                    Title = "Okay Book",
                    ISBN = "1234567800",
                    Author = "Bill Nye",
                    Items_Books = new List<Item>{
                        new Item
                        {
                            ItemType = "Book",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good",
                            Consignor = consignor,
                            ConsignorId = consignor.Id
                        }
                    }
                };

                bc.AddNewBook(book2);

                var book3 = new Book
                {
                    Title = "Bad Book",
                    ISBN = "1234567890",
                    Author = "Bill Nye",
                    Items_Books = new List<Item>{
                        new Item
                        {
                            ItemType = "Book",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good",
                            Consignor = consignor,
                            ConsignorId = consignor.Id
                        }
                    }
                };

                bc.AddNewBook(book3);


                List<Item> ItemsList = new List<Item>();
                ItemsList = bc.GetAllItems();

                //Set SalePrice amount
                ItemsList.ElementAtOrDefault(0).SalePrice = 3.00;
                ItemsList.ElementAtOrDefault(1).SalePrice = 3.00;
                ItemsList.ElementAtOrDefault(2).SalePrice = 3.00;


                List<Item> ItemsA = new List<Item>();
                ItemsA.Add(ItemsList.ElementAtOrDefault(0));
                ItemsA.Add(ItemsList.ElementAtOrDefault(1));
                ItemsA.Add(ItemsList.ElementAtOrDefault(2));

                List<Item> ItemsB = new List<Item>();
                ItemsB.Add(ItemsList.ElementAtOrDefault(0));
                ItemsB.Add(ItemsList.ElementAtOrDefault(1));
                ItemsB.Add(ItemsList.ElementAtOrDefault(2));

                List<Item> ItemsC = new List<Item>();
                ItemsC.Add(ItemsList.ElementAtOrDefault(0));
                ItemsC.Add(ItemsList.ElementAtOrDefault(1));
                ItemsC.Add(ItemsList.ElementAtOrDefault(2));

                ItemSaleTransaction T1 = new ItemSaleTransaction()
                {
                    CreditTransaction_ItemSale = new CreditTransaction() {
                    TransactionDate = DateTime.Now,
                    TransactionTotal = 5.00,
                    LocalSalesTaxTotal = .05,
                    StateSalesTaxTotal = .30,
                    CountySalesTaxTotal = .05,
                    DiscountTotal = 0,
                    },

                    Items_ItemSaleTransaction = ItemsA
                };

                ItemSaleTransaction T2 = new ItemSaleTransaction()
                {
                    CreditTransaction_ItemSale = new CreditTransaction()
                    {
                        TransactionDate = DateTime.Now,
                        TransactionTotal = 6.00,
                        LocalSalesTaxTotal = .05,
                        StateSalesTaxTotal = .30,
                        CountySalesTaxTotal = .05,
                        DiscountTotal = 0,
                    },

                    Items_ItemSaleTransaction = ItemsB
                };

                ItemSaleTransaction T3 = new ItemSaleTransaction()
                {
                    CreditTransaction_ItemSale = new CreditTransaction()
                    {
                        TransactionDate = DateTime.Now,
                        TransactionTotal = 7.00,
                        LocalSalesTaxTotal = .05,
                        StateSalesTaxTotal = .30,
                        CountySalesTaxTotal = .05,
                        DiscountTotal = 0,
                    },

                    Items_ItemSaleTransaction = ItemsC
                };

                bc.AddNewItemSaleTransaction(T1);
                bc.AddNewItemSaleTransaction(T2);
                bc.AddNewItemSaleTransaction(T3);
            }

        }

        //Delete the temporary database after each test
        [TestCleanup]
        public virtual void TestCleanup()
        {
            using (var db = new DataContext())
                if (db.Database.Exists())
                    db.Database.Delete();

        }


    }
}
