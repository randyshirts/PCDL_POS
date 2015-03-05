namespace RocketPos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedEanToGames : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "EAN", c => c.String(maxLength: 13));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games", "EAN");
        }
    }
}
