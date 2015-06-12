namespace DataModel.Data.TransactionalLayer.MigrationsSql
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedMembersTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "DateAdded", c => c.DateTime(nullable: false));
            AddColumn("dbo.Members", "RenewDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Members", "MemberType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Members", "MemberType");
            DropColumn("dbo.Members", "RenewDate");
            DropColumn("dbo.Members", "DateAdded");
        }
    }
}
