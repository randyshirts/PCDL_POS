using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RocketPos.Data.DataLayer;

namespace RocketPos.Tests.InvTests
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
