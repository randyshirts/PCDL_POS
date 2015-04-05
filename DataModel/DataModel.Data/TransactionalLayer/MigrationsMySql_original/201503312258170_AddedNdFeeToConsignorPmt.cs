namespace DataModel.Data.TransactionalLayer.MigrationsMySql
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNdFeeToConsignorPmt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ConsignorPmts", "NoDiscountFee", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ConsignorPmts", "NoDiscountFee");
        }
    }
}
