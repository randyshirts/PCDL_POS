namespace RocketPos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMapping : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Books", "Title", c => c.String(nullable: false, maxLength: 40));
            AlterColumn("dbo.Books", "ISBN", c => c.String(nullable: false, maxLength: 13));
            AlterColumn("dbo.Books", "Author", c => c.String(nullable: false, maxLength: 40));
            AlterColumn("dbo.Items", "ItemType", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Items", "Location", c => c.String(nullable: false, maxLength: 4));
            AlterColumn("dbo.Items", "Status", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Items", "Condition", c => c.String(nullable: false, maxLength: 25));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Items", "Condition", c => c.String());
            AlterColumn("dbo.Items", "Status", c => c.String());
            AlterColumn("dbo.Items", "Location", c => c.String());
            AlterColumn("dbo.Items", "ItemType", c => c.String());
            AlterColumn("dbo.Books", "Author", c => c.String());
            AlterColumn("dbo.Books", "ISBN", c => c.String());
            AlterColumn("dbo.Books", "Title", c => c.String());
        }
    }
}
