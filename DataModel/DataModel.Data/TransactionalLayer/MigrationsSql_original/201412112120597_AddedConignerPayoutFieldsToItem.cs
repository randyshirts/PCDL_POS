namespace RocketPos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedConignerPayoutFieldsToItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "ConsignorPmt", c => c.Double());
            AddColumn("dbo.Items", "CashPayout", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "CashPayout");
            DropColumn("dbo.Items", "ConsignorPmt");
        }
    }
}
