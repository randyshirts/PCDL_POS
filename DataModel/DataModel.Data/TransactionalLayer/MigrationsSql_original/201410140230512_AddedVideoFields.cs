namespace RocketPos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedVideoFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Video", "Publisher", c => c.String(maxLength: 100));
            AddColumn("dbo.Video", "VideoFormat", c => c.String(nullable: false, maxLength: 7));
            AddColumn("dbo.Video", "AudienceRating", c => c.String(maxLength: 20));
            DropColumn("dbo.Video", "Manufacturer");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Video", "Manufacturer", c => c.String(maxLength: 100));
            DropColumn("dbo.Video", "AudienceRating");
            DropColumn("dbo.Video", "VideoFormat");
            DropColumn("dbo.Video", "Publisher");
        }
    }
}
