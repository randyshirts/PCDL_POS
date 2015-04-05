namespace RocketPos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetConsignorAsRequiredForItems : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Items", new[] { "ConsignorId" });
            AlterColumn("dbo.Items", "ConsignorId", c => c.Int(nullable: false));
            CreateIndex("dbo.Items", "ConsignorId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Items", new[] { "ConsignorId" });
            AlterColumn("dbo.Items", "ConsignorId", c => c.Int());
            CreateIndex("dbo.Items", "ConsignorId");
        }
    }
}
