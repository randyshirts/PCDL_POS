namespace RocketPos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedRequiredFields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Books", "Author", c => c.String(maxLength: 40));
            AlterColumn("dbo.Items", "Location", c => c.String(maxLength: 4));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Items", "Location", c => c.String(nullable: false, maxLength: 4));
            AlterColumn("dbo.Books", "Author", c => c.String(nullable: false, maxLength: 40));
        }
    }
}
