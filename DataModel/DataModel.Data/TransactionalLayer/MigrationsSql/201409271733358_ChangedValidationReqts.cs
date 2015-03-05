namespace RocketPos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedValidationReqts : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Books", "Title", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Books", "Author", c => c.String(maxLength: 100));
            AlterColumn("dbo.Games", "Title", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Games", "Manufacturer", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Games", "Manufacturer", c => c.String(maxLength: 40));
            AlterColumn("dbo.Games", "Title", c => c.String(nullable: false, maxLength: 40));
            AlterColumn("dbo.Books", "Author", c => c.String(maxLength: 40));
            AlterColumn("dbo.Books", "Title", c => c.String(nullable: false, maxLength: 40));
        }
    }
}
