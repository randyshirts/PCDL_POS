namespace RocketPos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedBarcodeToItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "Barcode", c => c.String(nullable: false, maxLength: 13));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "Barcode");
        }
    }
}
