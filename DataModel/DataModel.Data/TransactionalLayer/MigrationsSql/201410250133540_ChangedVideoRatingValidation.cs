namespace RocketPos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedVideoRatingValidation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Video", "AudienceRating", c => c.String(maxLength: 40));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Video", "AudienceRating", c => c.String(maxLength: 20));
        }
    }
}
