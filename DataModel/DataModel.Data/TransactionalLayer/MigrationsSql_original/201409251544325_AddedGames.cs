namespace RocketPos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedGames : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        GameId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        ISBN = c.String(),
                        Manufacturer = c.String(),
                    })
                .PrimaryKey(t => t.GameId);
            
            AddColumn("dbo.Items", "Game_GameId", c => c.Int());
            CreateIndex("dbo.Items", "Game_GameId");
            AddForeignKey("dbo.Items", "Game_GameId", "dbo.Games", "GameId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "Game_GameId", "dbo.Games");
            DropIndex("dbo.Items", new[] { "Game_GameId" });
            DropColumn("dbo.Items", "Game_GameId");
            DropTable("dbo.Games");
        }
    }
}
