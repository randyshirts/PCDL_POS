namespace RocketPos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedLocationToSubject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "Subject", c => c.String(maxLength: 30));
            DropColumn("dbo.Items", "Location");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Items", "Location", c => c.String(maxLength: 4));
            DropColumn("dbo.Items", "Subject");
        }
    }
}
