namespace RocketPos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTransactionTable : DbMigration
    {
        public override void Up()
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
            CreateIndex("dbo.Items", "TransactionId");
            AddForeignKey("dbo.Items", "TransactionId", "dbo.Transactions", "TransactionId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "TransactionId", "dbo.Transactions");
            DropIndex("dbo.Items", new[] { "TransactionId" });
            DropColumn("dbo.Items", "TransactionId");
            DropTable("dbo.Transactions");
        }
    }
}
