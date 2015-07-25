namespace DataModel.Data.TransactionalLayer.MigrationsSql
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StoreCreditTransactionChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StoreCreditTransactions", "TransactionType", c => c.String());
            AddColumn("dbo.CreditTransactions", "StoreCreditTransactionId", c => c.Int());
            DropColumn("dbo.CreditTransactions", "StoreId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CreditTransactions", "StoreId", c => c.Int());
            DropColumn("dbo.CreditTransactions", "StoreCreditTransactionId");
            DropColumn("dbo.StoreCreditTransactions", "TransactionType");
        }
    }
}
