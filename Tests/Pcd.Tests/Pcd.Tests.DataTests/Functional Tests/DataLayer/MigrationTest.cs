using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity.Migrations;
using RocketPos.Data.Migrations;
using System.Linq;
using System.Collections.Generic;

namespace RocketPos.Tests.DataTests.MigrationTest
{
    [TestClass]
    public class MigrationTest
    {
        [TestMethod]
        public void RunAllMigrations()
        {
            var configuration = new Configuration();
            var migrator = new DbMigrator(configuration);
            // back to 0
            migrator.Update("0");
            // up to current
            migrator.Update();

            //Test current migration to check if migration changes were successfully made
            //using (var bc = new BusinessContext())
            //{
            //    var item = new Item
            //    {
            //        ItemType = "Book",
            //        Location = "A2",
            //        Status = "Shelved",
            //        Condition = "Good",
            //        ListedPrice = 5,
            //        ListedDate = DateTime.Parse("12/25/2014"),
            //        Book = new Book
            //        {
            //            Title = "Don't Start That Again",
            //            Author = "Not him",
            //            ISBN = "1234567898"
            //        }
            //    };

            //    bc.AddNewItem(item);
            //    List<Item> results = bc.GetAllItems();
            //    Assert.IsTrue(results.Any(r => r.Id == item.Id));
            //}

            // back to 0
            migrator.Update("0");
        }
     }
            
}
