namespace RocketPos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedBooks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        BookId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        ISBN = c.String(),
                        Author = c.String(),
                    })
                .PrimaryKey(t => t.BookId);
            
            AddColumn("dbo.Items", "BookId", c => c.Int());
            CreateIndex("dbo.Items", "BookId");
            AddForeignKey("dbo.Items", "BookId", "dbo.Books", "BookId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "BookId", "dbo.Books");
            DropIndex("dbo.Items", new[] { "BookId" });
            DropColumn("dbo.Items", "BookId");
            DropTable("dbo.Books");
        }
    }
}
