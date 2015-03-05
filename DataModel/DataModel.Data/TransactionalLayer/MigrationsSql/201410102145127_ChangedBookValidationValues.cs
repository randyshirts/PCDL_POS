namespace RocketPos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedBookValidationValues : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Books", "Title", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.Games", "Title", c => c.String(nullable: false, maxLength: 150));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Games", "Title", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Books", "Title", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
