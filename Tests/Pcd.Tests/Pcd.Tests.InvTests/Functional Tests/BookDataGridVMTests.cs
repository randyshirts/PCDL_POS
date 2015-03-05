using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RocketPos.Common.Foundation;
using System.Data.Entity;
//using System.Windows.Controls;
using System.Windows;


namespace RocketPos.Tests.InvTests
{
    //[TestClass]
    //public class BookDataGridVMTests
    //{

    //    [TestMethod]
    //    public void UpdateItemCommandCannotExecuteWhenItemTypeIsNotValid()
    //    {
    //        using (BusinessContext bc = new BusinessContext())
    //        {
    //            var book = new Book
    //            {
    //                Title = "Best Book Evah",
    //                ISBN = "1111111111",
    //                Author = "The Big Guy",
    //                Id = 1,
    //                Binding = "Hardcover",
    //                NumberOfPages = 100,
    //                PublicationDate = DateTime.Parse("10/31/2014"),
    //                TradeInValue = 3,
    //                Items_Books = new List<Item>{
    //                    new Item
    //                    {
    //                        Id = 1,
    //                        ItemType = "Book",
    //                        ListedPrice = 3.00,
    //                        ListedDate = DateTime.Parse("12/25/2014"),
    //                        Location = "F2",
    //                        Status = "Shelved",
    //                        Description = "Well well",
    //                        Condition = "Good"
    //                    }
    //                }
    //            };

    //            bc.AddNewBook(book);
    //        }


    //        var viewModel = new BookDataGridVM();

    //        DataGridCellEditEndingEventArgs e;

    //        var updatedBook = new Book
    //        {
    //            Title = "Best Book Evah",
    //            ISBN = "1111111111",
    //            Author = "The Big Guy",
    //            Id = 1,
    //            Binding = "Hardcover",
    //            NumberOfPages = null,
    //            PublicationDate = DateTime.Parse("10/31/2014"),
    //            TradeInValue = 3,
    //            Items_Books = new List<Item>{
    //                    new Item
    //                    {
    //                        Id = 1,
    //                        ItemType = "Book",
    //                        ListedPrice = 3.00,
    //                        ListedDate = DateTime.Parse("12/25/2014"),
    //                        Location = "F2",
    //                        Status = "Shelved",
    //                        Description = "Well well",
    //                        Condition = "Good"
    //                    }
    //                }
    //        };

    //        Assert.IsFalse(viewModel.DelCommand.CanExecute(null));
    //    }

}