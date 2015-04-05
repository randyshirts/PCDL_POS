namespace RocketPos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedConsignorToItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "ConsignorId", c => c.Int());
            CreateIndex("dbo.Items", "ConsignorId");
            AddForeignKey("dbo.Items", "ConsignorId", "dbo.Consignors", "PersonId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "ConsignorId", "dbo.Consignors");
            DropIndex("dbo.Items", new[] { "ConsignorId" });
            DropColumn("dbo.Items", "ConsignorId");
        }
    }
}
