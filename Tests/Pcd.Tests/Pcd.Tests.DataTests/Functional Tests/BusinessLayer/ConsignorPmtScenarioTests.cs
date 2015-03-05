using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RocketPos.Data.DataLayer.Entities;
using RocketPos.Data.TransactionalLayer;

namespace RocketPos.Tests.DataTests.Functional_Tests
{
    [TestClass]
    public class ConsignorPmtScenarioTests : FunctionalTest
    {
        [TestMethod]
        public void AddNewConsignorPmtIsPersisted()
        {
                
                using (var bc = new BusinessContext())
                {
                    var itemsList = bc.GetAllItems();

                    var itemsA = new List<Item>
                    {
                        itemsList.ElementAtOrDefault(0),
                        itemsList.ElementAtOrDefault(1),
                        itemsList.ElementAtOrDefault(2)
                    };

                    var consignor = bc.GetConsignorByName("Bob", "Jones");


                    var consignorPmt = new ConsignorPmt
                    {
                        //Consignor Info
                        Consignor_ConsignorPmt = consignor,
                        ConsignorId = consignor.Id,

                        //DebitTransaction Info
                        DebitTransaction_ConsignorPmt = new DebitTransaction
                        {
                            DebitTotal = 10,
                            DebitTransactionDate = DateTime.Now
                        },

                        //Items info
                        Items_ConsignorPmt = itemsA,

                    };

                    bc.AddNewConsignorPmt(consignorPmt);

                var exists = bc.DataContext.Consignors.Find(1).ConsignorPmts_Consignor.ElementAtOrDefault(0).DebitTransaction_ConsignorPmt.DebitTransactionDate.Date 
                                                    == consignorPmt.DebitTransaction_ConsignorPmt.DebitTransactionDate.Date;

                Assert.IsTrue(exists);
            }
        }

        

    }
}
