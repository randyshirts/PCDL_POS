namespace RocketPos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedTansactionFieldsAndSeed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "SalePrice", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "SalePrice");
        }
    }
}
