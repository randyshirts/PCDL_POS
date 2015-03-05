namespace RocketPos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedStoreCredit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StoreCreditPmts",
                c => new
                    {
                        DebitTransactionId = c.Int(nullable: false),
                        ConsignorId = c.Int(nullable: false),
                        StoreCreditPmtAmount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.DebitTransactionId)
                .ForeignKey("dbo.Consignors", t => t.ConsignorId)
                .ForeignKey("dbo.DebitTransactions", t => t.DebitTransactionId)
                .Index(t => t.DebitTransactionId)
                .Index(t => t.ConsignorId);
            
            CreateTable(
                "dbo.StoreCreditTransactions",
                c => new
                    {
                        CreditTransactionId = c.Int(nullable: false),
                        ConsignorId = c.Int(nullable: false),
                        StoreCreditTransactionAmount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.CreditTransactionId)
                .ForeignKey("dbo.Consignors", t => t.ConsignorId)
                .ForeignKey("dbo.CreditTransactions", t => t.CreditTransactionId)
                .Index(t => t.CreditTransactionId)
                .Index(t => t.ConsignorId);
            
            AddColumn("dbo.CreditTransactions", "StoreCreditTransactionId", c => c.Int());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StoreCreditTransactions", "CreditTransactionId", "dbo.CreditTransactions");
            DropForeignKey("dbo.StoreCreditTransactions", "ConsignorId", "dbo.Consignors");
            DropForeignKey("dbo.StoreCreditPmts", "DebitTransactionId", "dbo.DebitTransactions");
            DropForeignKey("dbo.StoreCreditPmts", "ConsignorId", "dbo.Consignors");
            DropIndex("dbo.StoreCreditTransactions", new[] { "ConsignorId" });
            DropIndex("dbo.StoreCreditTransactions", new[] { "CreditTransactionId" });
            DropIndex("dbo.StoreCreditPmts", new[] { "ConsignorId" });
            DropIndex("dbo.StoreCreditPmts", new[] { "DebitTransactionId" });
            DropColumn("dbo.CreditTransactions", "StoreCreditTransactionId");
            DropTable("dbo.StoreCreditTransactions");
            DropTable("dbo.StoreCreditPmts");
        }
    }
}
