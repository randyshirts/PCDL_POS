namespace DataModel.Data.TransactionalLayer.MigrationsSql
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStartDateToMembers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "StartDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Members", "StartDate");
        }
    }
}
