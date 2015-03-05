namespace RocketPos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedAudioToOther : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Items", "AudioId", "dbo.Audio");
            DropIndex("dbo.Items", new[] { "AudioId" });
            CreateTable(
                "dbo.Other",
                c => new
                    {
                        OtherId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 150),
                        Manufacturer = c.String(maxLength: 100),
                        EAN = c.String(maxLength: 13),
                    })
                .PrimaryKey(t => t.OtherId);
            
            AddColumn("dbo.Items", "OtherId", c => c.Int());
            CreateIndex("dbo.Items", "OtherId");
            AddForeignKey("dbo.Items", "OtherId", "dbo.Other", "OtherId");
            DropColumn("dbo.Items", "AudioId");
            DropTable("dbo.Audio");
        }
        
        public override void Down()
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
            
            AddColumn("dbo.Items", "AudioId", c => c.Int());
            DropForeignKey("dbo.Items", "OtherId", "dbo.Other");
            DropIndex("dbo.Items", new[] { "OtherId" });
            DropColumn("dbo.Items", "OtherId");
            DropTable("dbo.Other");
            CreateIndex("dbo.Items", "AudioId");
            AddForeignKey("dbo.Items", "AudioId", "dbo.Audio", "AudioId");
        }
    }
}
