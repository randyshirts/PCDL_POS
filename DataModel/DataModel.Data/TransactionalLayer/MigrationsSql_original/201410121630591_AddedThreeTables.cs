namespace RocketPos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedThreeTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Audio",
                c => new
                    {
                        AudioId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 150),
                        Manufacturer = c.String(maxLength: 100),
                        EAN = c.String(maxLength: 13),
                    })
                .PrimaryKey(t => t.AudioId);
            
            CreateTable(
                "dbo.TeachingAide",
                c => new
                    {
                        TeachingAideId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 150),
                        Manufacturer = c.String(maxLength: 100),
                        EAN = c.String(maxLength: 13),
                    })
                .PrimaryKey(t => t.TeachingAideId);
            
            CreateTable(
                "dbo.Video",
                c => new
                    {
                        VideoId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 150),
                        Manufacturer = c.String(maxLength: 100),
                        EAN = c.String(maxLength: 13),
                    })
                .PrimaryKey(t => t.VideoId);
            
            AddColumn("dbo.Items", "TeachingAideId", c => c.Int());
            AddColumn("dbo.Items", "AudioId", c => c.Int());
            AddColumn("dbo.Items", "VideoId", c => c.Int());
            CreateIndex("dbo.Items", "TeachingAideId");
            CreateIndex("dbo.Items", "AudioId");
            CreateIndex("dbo.Items", "VideoId");
            AddForeignKey("dbo.Items", "AudioId", "dbo.Audio", "AudioId");
            AddForeignKey("dbo.Items", "TeachingAideId", "dbo.TeachingAide", "TeachingAideId");
            AddForeignKey("dbo.Items", "VideoId", "dbo.Video", "VideoId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "VideoId", "dbo.Video");
            DropForeignKey("dbo.Items", "TeachingAideId", "dbo.TeachingAide");
            DropForeignKey("dbo.Items", "AudioId", "dbo.Audio");
            DropIndex("dbo.Items", new[] { "VideoId" });
            DropIndex("dbo.Items", new[] { "AudioId" });
            DropIndex("dbo.Items", new[] { "TeachingAideId" });
            DropColumn("dbo.Items", "VideoId");
            DropColumn("dbo.Items", "AudioId");
            DropColumn("dbo.Items", "TeachingAideId");
            DropTable("dbo.Video");
            DropTable("dbo.TeachingAide");
            DropTable("dbo.Audio");
        }
    }
}
