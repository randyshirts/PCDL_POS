namespace RocketPos.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPersonTables : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Other", newName: "Others");
            RenameTable(name: "dbo.TeachingAide", newName: "TeachingAides");
            RenameTable(name: "dbo.Video", newName: "Videos");
            CreateTable(
                "dbo.Consignors",
                c => new
                    {
                        PersonId = c.Int(nullable: false),
                        DateAdded = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PersonId)
                .ForeignKey("dbo.Persons", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.PersonId);
            
            CreateTable(
                "dbo.Persons",
                c => new
                    {
                        PersonId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 20),
                        LastName = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.PersonId);
            
            CreateTable(
                "dbo.Emails",
                c => new
                    {
                        EmailId = c.Int(nullable: false, identity: true),
                        EmailAddress = c.String(nullable: false, maxLength: 40),
                    })
                .PrimaryKey(t => t.EmailId);
            
            CreateTable(
                "dbo.MailingAddresses",
                c => new
                    {
                        MailingAddressId = c.Int(nullable: false, identity: true),
                        MailingAddress1 = c.String(nullable: false, maxLength: 40),
                        MailingAddress2 = c.String(maxLength: 40),
                        ZipCode = c.String(nullable: false, maxLength: 10),
                        City = c.String(nullable: false, maxLength: 30),
                        State = c.String(nullable: false, maxLength: 2),
                    })
                .PrimaryKey(t => t.MailingAddressId);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        PersonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PersonId)
                .ForeignKey("dbo.Persons", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.PersonId);
            
            CreateTable(
                "dbo.PhoneNumbers",
                c => new
                    {
                        PersonId = c.Int(nullable: false),
                        HomePhoneNumber = c.String(maxLength: 10),
                        CellPhoneNumber = c.String(nullable: false, maxLength: 10),
                        AltPhoneNumber = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.PersonId)
                .ForeignKey("dbo.Persons", t => t.PersonId)
                .Index(t => t.PersonId);
            
            CreateTable(
                "dbo.Volunteers",
                c => new
                    {
                        PersonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PersonId)
                .ForeignKey("dbo.Persons", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.PersonId);
            
            CreateTable(
                "dbo.EmailAddresses_Persons",
                c => new
                    {
                        PersonId = c.Int(nullable: false),
                        EmailAddressId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PersonId, t.EmailAddressId })
                .ForeignKey("dbo.Persons", t => t.PersonId, cascadeDelete: true)
                .ForeignKey("dbo.Emails", t => t.EmailAddressId, cascadeDelete: true)
                .Index(t => t.PersonId)
                .Index(t => t.EmailAddressId);
            
            CreateTable(
                "dbo.MailingAddresses_Persons",
                c => new
                    {
                        PersonId = c.Int(nullable: false),
                        MailingAddressId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PersonId, t.MailingAddressId })
                .ForeignKey("dbo.Persons", t => t.PersonId, cascadeDelete: true)
                .ForeignKey("dbo.MailingAddresses", t => t.MailingAddressId, cascadeDelete: true)
                .Index(t => t.PersonId)
                .Index(t => t.MailingAddressId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Volunteers", "PersonId", "dbo.Persons");
            DropForeignKey("dbo.PhoneNumbers", "PersonId", "dbo.Persons");
            DropForeignKey("dbo.Members", "PersonId", "dbo.Persons");
            DropForeignKey("dbo.MailingAddresses_Persons", "MailingAddressId", "dbo.MailingAddresses");
            DropForeignKey("dbo.MailingAddresses_Persons", "PersonId", "dbo.Persons");
            DropForeignKey("dbo.EmailAddresses_Persons", "EmailAddressId", "dbo.Emails");
            DropForeignKey("dbo.EmailAddresses_Persons", "PersonId", "dbo.Persons");
            DropForeignKey("dbo.Consignors", "PersonId", "dbo.Persons");
            DropIndex("dbo.MailingAddresses_Persons", new[] { "MailingAddressId" });
            DropIndex("dbo.MailingAddresses_Persons", new[] { "PersonId" });
            DropIndex("dbo.EmailAddresses_Persons", new[] { "EmailAddressId" });
            DropIndex("dbo.EmailAddresses_Persons", new[] { "PersonId" });
            DropIndex("dbo.Volunteers", new[] { "PersonId" });
            DropIndex("dbo.PhoneNumbers", new[] { "PersonId" });
            DropIndex("dbo.Members", new[] { "PersonId" });
            DropIndex("dbo.Consignors", new[] { "PersonId" });
            DropTable("dbo.MailingAddresses_Persons");
            DropTable("dbo.EmailAddresses_Persons");
            DropTable("dbo.Volunteers");
            DropTable("dbo.PhoneNumbers");
            DropTable("dbo.Members");
            DropTable("dbo.MailingAddresses");
            DropTable("dbo.Emails");
            DropTable("dbo.Persons");
            DropTable("dbo.Consignors");
            RenameTable(name: "dbo.Videos", newName: "Video");
            RenameTable(name: "dbo.TeachingAides", newName: "TeachingAide");
            RenameTable(name: "dbo.Others", newName: "Other");
        }
    }
}
