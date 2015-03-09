namespace DataModel.Data.TransactionalLayer.MigrationsMySql
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedPhoneNumberMapping : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PhoneNumbers", "CellPhoneNumber", c => c.String(maxLength: 10, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PhoneNumbers", "CellPhoneNumber", c => c.String(nullable: false, maxLength: 10, storeType: "nvarchar"));
        }
    }
}
