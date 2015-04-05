namespace RocketPos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedItemFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "IsDiscountable", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Items", "Subject", c => c.String(nullable: false, maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Items", "Subject", c => c.String(maxLength: 30));
            DropColumn("dbo.Items", "IsDiscountable");
        }
    }
}
