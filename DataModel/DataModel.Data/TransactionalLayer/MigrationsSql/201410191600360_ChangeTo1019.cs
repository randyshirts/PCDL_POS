namespace RocketPos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTo1019 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "BookImage", c => c.String(maxLength: 200));
            AddColumn("dbo.Games", "GameImage", c => c.String(maxLength: 300));
            AddColumn("dbo.Other", "OtherImage", c => c.String(maxLength: 300));
            AddColumn("dbo.TeachingAide", "TeachingAideImage", c => c.String(maxLength: 300));
            AddColumn("dbo.Video", "VideoImage", c => c.String(maxLength: 300));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Video", "VideoImage");
            DropColumn("dbo.TeachingAide", "TeachingAideImage");
            DropColumn("dbo.Other", "OtherImage");
            DropColumn("dbo.Games", "GameImage");
            DropColumn("dbo.Books", "BookImage");
        }
    }
}
