namespace DataModel.Data.TransactionalLayer.MigrationsMySql
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedBarcodeLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Items", "Barcode", c => c.String(nullable: false, maxLength: 15, fixedLength: true, storeType: "nchar"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Items", "Barcode", c => c.String(nullable: false, maxLength: 13, fixedLength: true, storeType: "nchar"));
        }
    }
}
