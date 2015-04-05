namespace RocketPos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCreditTransaction : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Items", "TransactionId", "dbo.Transactions");
            DropIndex("dbo.Items", new[] { "TransactionId" });
            CreateTable(
                "dbo.ItemSaleTransactions",
                c => new
                    {
                        CreditTransactionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CreditTransactionId)
                .ForeignKey("dbo.CreditTransactions", t => t.CreditTransactionId)
                .Index(t => t.CreditTransactionId);
            
            CreateTable(
                "dbo.CreditTransactions",
                c => new
                    {
                        CreditTransactionId = c.Int(nullable: false, identity: true),
                        TransactionDate = c.DateTime(nullable: false),
                        TransactionTotal = c.Double(nullable: false),
                        StateSalesTaxTotal = c.Double(nullable: false),
                        LocalSalesTaxTotal = c.Double(nullable: false),
                        CountySalesTaxTotal = c.Double(nullable: false),
                        DiscountTotal = c.Double(),
                        ItemSaleTransactionId = c.Int(),
                        ClassPmtTransactionId = c.Int(),
                        SpaceRentalTransactionId = c.Int(),
                    })
                .PrimaryKey(t => t.CreditTransactionId);
            
            CreateTable(
                "dbo.ClassPmtTransactions",
                c => new
                    {
                        CreditTransactionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CreditTransactionId)
                .ForeignKey("dbo.CreditTransactions", t => t.CreditTransactionId)
                .Index(t => t.CreditTransactionId);
            
            CreateTable(
                "dbo.SpaceRentalTransactions",
                c => new
                    {
                        CreditTransactionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CreditTransactionId)
                .ForeignKey("dbo.CreditTransactions", t => t.CreditTransactionId)
                .Index(t => t.CreditTransactionId);
            
            AddColumn("dbo.Items", "ItemSaleTransactionId", c => c.Int());
            CreateIndex("dbo.Items", "ItemSaleTransactionId");
            AddForeignKey("dbo.Items", "ItemSaleTransactionId", "dbo.ItemSaleTransactions", "CreditTransactionId");
            DropColumn("dbo.Items", "TransactionId");
            DropTable("dbo.Transactions");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        TransactionId = c.Int(nullable: false, identity: true),
                        TransactionDate = c.DateTime(nullable: false),
                        SaleTotal = c.Double(nullable: false),
                        StateSalesTaxTotal = c.Double(nullable: false),
                        LocalSalesTaxTotal = c.Double(nullable: false),
                        DiscountTotal = c.Double(),
                    })
                .PrimaryKey(t => t.TransactionId);
            
            AddColumn("dbo.Items", "TransactionId", c => c.Int());
            DropForeignKey("dbo.Items", "ItemSaleTransactionId", "dbo.ItemSaleTransactions");
            DropForeignKey("dbo.SpaceRentalTransactions", "CreditTransactionId", "dbo.CreditTransactions");
            DropForeignKey("dbo.ItemSaleTransactions", "CreditTransactionId", "dbo.CreditTransactions");
            DropForeignKey("dbo.ClassPmtTransactions", "CreditTransactionId", "dbo.CreditTransactions");
            DropIndex("dbo.SpaceRentalTransactions", new[] { "CreditTransactionId" });
            DropIndex("dbo.ClassPmtTransactions", new[] { "CreditTransactionId" });
            DropIndex("dbo.ItemSaleTransactions", new[] { "CreditTransactionId" });
            DropIndex("dbo.Items", new[] { "ItemSaleTransactionId" });
            DropColumn("dbo.Items", "ItemSaleTransactionId");
            DropTable("dbo.SpaceRentalTransactions");
            DropTable("dbo.ClassPmtTransactions");
            DropTable("dbo.CreditTransactions");
            DropTable("dbo.ItemSaleTransactions");
            CreateIndex("dbo.Items", "TransactionId");
            AddForeignKey("dbo.Items", "TransactionId", "dbo.Transactions", "TransactionId");
        }
    }
}
