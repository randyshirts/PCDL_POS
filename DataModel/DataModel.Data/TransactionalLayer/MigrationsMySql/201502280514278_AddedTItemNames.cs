namespace RocketPos.Data.TransactionalLayer.MigrationsMySql
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTItemNames : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "ItemImage", c => c.String(maxLength: 200, storeType: "nvarchar"));
            AddColumn("dbo.Books", "EAN", c => c.String(unicode: false));
            AddColumn("dbo.Books", "Manufacturer", c => c.String(unicode: false));
            AddColumn("dbo.Games", "ItemImage", c => c.String(maxLength: 300, storeType: "nvarchar"));
            AddColumn("dbo.Others", "ItemImage", c => c.String(maxLength: 300, storeType: "nvarchar"));
            AddColumn("dbo.TeachingAides", "ItemImage", c => c.String(maxLength: 300, storeType: "nvarchar"));
            AddColumn("dbo.Videos", "ItemImage", c => c.String(maxLength: 300, storeType: "nvarchar"));
            AddColumn("dbo.Videos", "Manufacturer", c => c.String(unicode: false));
            DropColumn("dbo.Books", "BookImage");
            DropColumn("dbo.Games", "GameImage");
            DropColumn("dbo.Others", "OtherImage");
            DropColumn("dbo.TeachingAides", "TeachingAideImage");
            DropColumn("dbo.Videos", "VideoImage");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Videos", "VideoImage", c => c.String(maxLength: 300, storeType: "nvarchar"));
            AddColumn("dbo.TeachingAides", "TeachingAideImage", c => c.String(maxLength: 300, storeType: "nvarchar"));
            AddColumn("dbo.Others", "OtherImage", c => c.String(maxLength: 300, storeType: "nvarchar"));
            AddColumn("dbo.Games", "GameImage", c => c.String(maxLength: 300, storeType: "nvarchar"));
            AddColumn("dbo.Books", "BookImage", c => c.String(maxLength: 200, storeType: "nvarchar"));
            DropColumn("dbo.Videos", "Manufacturer");
            DropColumn("dbo.Videos", "ItemImage");
            DropColumn("dbo.TeachingAides", "ItemImage");
            DropColumn("dbo.Others", "ItemImage");
            DropColumn("dbo.Games", "ItemImage");
            DropColumn("dbo.Books", "Manufacturer");
            DropColumn("dbo.Books", "EAN");
            DropColumn("dbo.Books", "ItemImage");
        }
    }
}
