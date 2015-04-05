namespace RocketPos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedConsignorPmtAndDebitTrans : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Items", "ConsignorPmt");
            DropForeignKey("dbo.Consignors", "PersonId", "dbo.Persons");
            DropForeignKey("dbo.Members", "PersonId", "dbo.Persons");
            DropForeignKey("dbo.Volunteers", "PersonId", "dbo.Persons");
            CreateTable(
                "dbo.ConsignorPmts",
                c => new
                    {
                        DebitTransactionId = c.Int(nullable: false),
                        ConsignorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DebitTransactionId)
                .ForeignKey("dbo.Consignors", t => t.ConsignorId)
                .ForeignKey("dbo.DebitTransactions", t => t.DebitTransactionId)
                .Index(t => t.DebitTransactionId)
                .Index(t => t.ConsignorId);
            
            CreateTable(
                "dbo.DebitTransactions",
                c => new
                    {
                        DebitTransactionId = c.Int(nullable: false, identity: true),
                        DebitTransactionDate = c.DateTime(nullable: false),
                        DebitTotal = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.DebitTransactionId);
            
            AddColumn("dbo.Items", "ConsignorPmtId", c => c.Int());
            CreateIndex("dbo.Items", "ConsignorPmtId");
            AddForeignKey("dbo.Items", "ConsignorPmtId", "dbo.ConsignorPmts", "DebitTransactionId");
            AddForeignKey("dbo.Consignors", "PersonId", "dbo.Persons", "PersonId");
            AddForeignKey("dbo.Members", "PersonId", "dbo.Persons", "PersonId");
            AddForeignKey("dbo.Volunteers", "PersonId", "dbo.Persons", "PersonId");
           
        }
        
        public override void Down()
        {
            AddColumn("dbo.Items", "ConsignorPmt", c => c.Double());
            DropForeignKey("dbo.Volunteers", "PersonId", "dbo.Persons");
            DropForeignKey("dbo.Members", "PersonId", "dbo.Persons");
            DropForeignKey("dbo.Consignors", "PersonId", "dbo.Persons");
            DropForeignKey("dbo.Items", "ConsignorPmtId", "dbo.ConsignorPmts");
            DropForeignKey("dbo.ConsignorPmts", "DebitTransactionId", "dbo.DebitTransactions");
            DropForeignKey("dbo.ConsignorPmts", "ConsignorId", "dbo.Consignors");
            DropIndex("dbo.ConsignorPmts", new[] { "ConsignorId" });
            DropIndex("dbo.ConsignorPmts", new[] { "DebitTransactionId" });
            DropIndex("dbo.Items", new[] { "ConsignorPmtId" });
            DropColumn("dbo.Items", "ConsignorPmtId");
            DropTable("dbo.DebitTransactions");
            DropTable("dbo.ConsignorPmts");
            AddForeignKey("dbo.Volunteers", "PersonId", "dbo.Persons", "PersonId", cascadeDelete: true);
            AddForeignKey("dbo.Members", "PersonId", "dbo.Persons", "PersonId", cascadeDelete: true);
            AddForeignKey("dbo.Consignors", "PersonId", "dbo.Persons", "PersonId", cascadeDelete: true);
        }
    }
}
