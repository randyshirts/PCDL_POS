namespace RocketPos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAdditionalFieldsToBooks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "Binding", c => c.String(maxLength: 25));
            AddColumn("dbo.Books", "NumberOfPages", c => c.Int());
            AddColumn("dbo.Books", "PublicationDate", c => c.DateTime());
            AddColumn("dbo.Books", "TradeInValue", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "TradeInValue");
            DropColumn("dbo.Books", "PublicationDate");
            DropColumn("dbo.Books", "NumberOfPages");
            DropColumn("dbo.Books", "Binding");
        }
    }
}
