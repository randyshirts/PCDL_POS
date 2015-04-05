namespace RocketPos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedGamesMapping : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Items", name: "Game_GameId", newName: "GameId");
            RenameIndex(table: "dbo.Items", name: "IX_Game_GameId", newName: "IX_GameId");
            AlterColumn("dbo.Games", "Title", c => c.String(nullable: false, maxLength: 40));
            AlterColumn("dbo.Games", "Manufacturer", c => c.String(maxLength: 40));
            DropColumn("dbo.Games", "ISBN");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Games", "ISBN", c => c.String());
            AlterColumn("dbo.Games", "Manufacturer", c => c.String());
            AlterColumn("dbo.Games", "Title", c => c.String());
            RenameIndex(table: "dbo.Items", name: "IX_GameId", newName: "IX_Game_GameId");
            RenameColumn(table: "dbo.Items", name: "GameId", newName: "Game_GameId");
        }
    }
}
