using RocketPos.Data.BusinessLayer;

namespace RocketPos.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.SqlServer;
    using RocketPos.Data.Business;
    using System.Collections.Generic;

    public sealed class Configuration : DbMigrationsConfiguration<RocketPos.Data.DataLayer.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(RocketPos.Data.DataLayer.DataContext context)
        {
            //This method will be called after migrating to the latest version.

            //You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //to avoid creating duplicate seed data. E.g.

            List<Item> ItemsList = new List<Item>();
            ItemsList = context.Items.ToList();

            //Set SalePrice amount
            ItemsList.ElementAtOrDefault(2).SalePrice = 3.00;
            ItemsList.ElementAtOrDefault(3).SalePrice = 3.00;
            ItemsList.ElementAtOrDefault(4).SalePrice = 3.00;
            ItemsList.ElementAtOrDefault(22).SalePrice = 3.00;
            ItemsList.ElementAtOrDefault(23).SalePrice = 3.00;
            ItemsList.ElementAtOrDefault(24).SalePrice = 3.00;
            ItemsList.ElementAtOrDefault(32).SalePrice = 3.00;
            ItemsList.ElementAtOrDefault(33).SalePrice = 3.00;
            ItemsList.ElementAtOrDefault(34).SalePrice = 3.00;


                List<Item> ItemsA = new List<Item>();
                ItemsA.Add(ItemsList.ElementAtOrDefault(2));
                ItemsA.Add(ItemsList.ElementAtOrDefault(3));
                ItemsA.Add(ItemsList.ElementAtOrDefault(4));

                List<Item> ItemsB = new List<Item>();
                ItemsB.Add(ItemsList.ElementAtOrDefault(22));
                ItemsB.Add(ItemsList.ElementAtOrDefault(23));
                ItemsB.Add(ItemsList.ElementAtOrDefault(24));

                List<Item> ItemsC = new List<Item>();
                ItemsC.Add(ItemsList.ElementAtOrDefault(32));
                ItemsC.Add(ItemsList.ElementAtOrDefault(33));
                ItemsC.Add(ItemsList.ElementAtOrDefault(34));

                context.ItemSaleTransactions.AddOrUpdate(
                    t => t.CreditTransactionId,
                
                new ItemSaleTransaction()
                {
                     Items_ItemSaleTransaction = ItemsA,
 
                     CreditTransaction_ItemSale = new CreditTransaction() {
                          TransactionDate = DateTime.Now,
                          TransactionTotal = 5.00,
                          LocalSalesTaxTotal = .045,
                          StateSalesTaxTotal = .40,
                          CountySalesTaxTotal = .05,
                          DiscountTotal = 0                      
                      },
                  },

                  new ItemSaleTransaction()
                  {
                      Items_ItemSaleTransaction = ItemsB,

                      CreditTransaction_ItemSale = new CreditTransaction()
                      {
                          TransactionDate = DateTime.Now,
                          TransactionTotal = 6.00,
                          LocalSalesTaxTotal = .045,
                          StateSalesTaxTotal = .40,
                          CountySalesTaxTotal = .05,
                          DiscountTotal = 1.0
                      },
                  },
                  new ItemSaleTransaction
                  {
                      Items_ItemSaleTransaction = ItemsC,

                      CreditTransaction_ItemSale = new CreditTransaction()
                      {
                          TransactionDate = DateTime.Now,
                          TransactionTotal = 7.00,
                          LocalSalesTaxTotal = .045,
                          StateSalesTaxTotal = .4,
                          CountySalesTaxTotal = .05,
                          DiscountTotal = 2.0
                      },
                  }

                );
            
        }
    }
}
