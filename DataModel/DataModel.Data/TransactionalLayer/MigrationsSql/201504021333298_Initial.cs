namespace DataModel.Data.TransactionalLayer.MigrationsSql
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 150),
                        ISBN = c.String(nullable: false, maxLength: 13),
                        Author = c.String(maxLength: 100),
                        Binding = c.String(maxLength: 25),
                        NumberOfPages = c.Int(),
                        PublicationDate = c.DateTime(),
                        TradeInValue = c.Double(),
                        ItemImage = c.String(maxLength: 200),
                        EAN = c.String(),
                        Manufacturer = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemType = c.String(nullable: false, maxLength: 20),
                        ListedPrice = c.Double(nullable: false),
                        SalePrice = c.Double(),
                        CashPayout = c.Boolean(),
                        ListedDate = c.DateTime(nullable: false),
                        Subject = c.String(nullable: false, maxLength: 30),
                        Status = c.String(nullable: false, maxLength: 30),
                        Condition = c.String(nullable: false, maxLength: 25),
                        IsDiscountable = c.Boolean(nullable: false),
                        Description = c.String(),
                        Barcode = c.String(nullable: false, maxLength: 15, fixedLength: true),
                        BookId = c.Int(),
                        GameId = c.Int(),
                        TeachingAideId = c.Int(),
                        OtherId = c.Int(),
                        VideoId = c.Int(),
                        ConsignorId = c.Int(nullable: false),
                        ItemSaleTransactionId = c.Int(),
                        ConsignorPmtId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Books", t => t.BookId)
                .ForeignKey("dbo.Consignors", t => t.ConsignorId)
                .ForeignKey("dbo.ConsignorPmts", t => t.ConsignorPmtId)
                .ForeignKey("dbo.Games", t => t.GameId)
                .ForeignKey("dbo.ItemSaleTransactions", t => t.ItemSaleTransactionId)
                .ForeignKey("dbo.Others", t => t.OtherId)
                .ForeignKey("dbo.TeachingAides", t => t.TeachingAideId)
                .ForeignKey("dbo.Videos", t => t.VideoId)
                .Index(t => t.BookId)
                .Index(t => t.GameId)
                .Index(t => t.TeachingAideId)
                .Index(t => t.OtherId)
                .Index(t => t.VideoId)
                .Index(t => t.ConsignorId)
                .Index(t => t.ItemSaleTransactionId)
                .Index(t => t.ConsignorPmtId);
            
            CreateTable(
                "dbo.Consignors",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        DateAdded = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Persons", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Persons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 20),
                        LastName = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Emails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmailAddress = c.String(nullable: false, maxLength: 40),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MailingAddresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MailingAddress1 = c.String(nullable: false, maxLength: 40),
                        MailingAddress2 = c.String(maxLength: 40),
                        ZipCode = c.String(nullable: false, maxLength: 10),
                        City = c.String(nullable: false, maxLength: 30),
                        State = c.String(nullable: false, maxLength: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Persons", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.PhoneNumbers",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        HomePhoneNumber = c.String(maxLength: 10),
                        CellPhoneNumber = c.String(maxLength: 10),
                        AltPhoneNumber = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Persons", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        EmailConfirmationCode = c.String(),
                        PasswordResetCode = c.String(),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                        PersonUser_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Persons", t => t.PersonUser_Id)
                .Index(t => t.PersonUser_Id);
            
            CreateTable(
                "dbo.IdentityUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.IdentityUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.IdentityUserRoles",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        IdentityRole_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.IdentityRoles", t => t.IdentityRole_Id)
                .Index(t => t.UserId)
                .Index(t => t.IdentityRole_Id);
            
            CreateTable(
                "dbo.Volunteers",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Persons", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.ConsignorPmts",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        NoDiscountFee = c.Double(),
                        ConsignorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Consignors", t => t.ConsignorId)
                .ForeignKey("dbo.DebitTransactions", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.ConsignorId);
            
            CreateTable(
                "dbo.DebitTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DebitTransactionDate = c.DateTime(nullable: false),
                        DebitTotal = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StoreCreditPmts",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ConsignorId = c.Int(nullable: false),
                        StoreCreditPmtAmount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Consignors", t => t.ConsignorId)
                .ForeignKey("dbo.DebitTransactions", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.ConsignorId);
            
            CreateTable(
                "dbo.StoreCreditTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ConsignorId = c.Int(nullable: false),
                        StoreCreditTransactionAmount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Consignors", t => t.ConsignorId)
                .ForeignKey("dbo.CreditTransactions", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.ConsignorId);
            
            CreateTable(
                "dbo.CreditTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransactionDate = c.DateTime(nullable: false),
                        TransactionTotal = c.Double(nullable: false),
                        StateSalesTaxTotal = c.Double(nullable: false),
                        LocalSalesTaxTotal = c.Double(nullable: false),
                        CountySalesTaxTotal = c.Double(nullable: false),
                        DiscountTotal = c.Double(),
                        ItemSaleTransactionId = c.Int(),
                        ClassPmtTransactionId = c.Int(),
                        SpaceRentalTransactionId = c.Int(),
                        StoreId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ClassPmtTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CreditTransactions", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.ItemSaleTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CreditTransactions", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.SpaceRentalTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CreditTransactions", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 150),
                        Manufacturer = c.String(maxLength: 100),
                        EAN = c.String(maxLength: 13),
                        ItemImage = c.String(maxLength: 300),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Others",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 150),
                        Manufacturer = c.String(maxLength: 100),
                        EAN = c.String(maxLength: 13),
                        ItemImage = c.String(maxLength: 300),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TeachingAides",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 150),
                        Manufacturer = c.String(maxLength: 100),
                        EAN = c.String(maxLength: 13),
                        ItemImage = c.String(maxLength: 300),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Videos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 150),
                        Publisher = c.String(maxLength: 100),
                        EAN = c.String(maxLength: 13),
                        VideoFormat = c.String(nullable: false, maxLength: 7),
                        AudienceRating = c.String(maxLength: 40),
                        ItemImage = c.String(maxLength: 300),
                        Manufacturer = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Description = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmailAddresses_Persons",
                c => new
                    {
                        EmailAddressId = c.Int(nullable: false),
                        PersonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.EmailAddressId, t.PersonId })
                .ForeignKey("dbo.Persons", t => t.EmailAddressId, cascadeDelete: true)
                .ForeignKey("dbo.Emails", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.EmailAddressId)
                .Index(t => t.PersonId);
            
            CreateTable(
                "dbo.MailingAddresses_Persons",
                c => new
                    {
                        MailingAddressId = c.Int(nullable: false),
                        PersonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MailingAddressId, t.PersonId })
                .ForeignKey("dbo.Persons", t => t.MailingAddressId, cascadeDelete: true)
                .ForeignKey("dbo.MailingAddresses", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.MailingAddressId)
                .Index(t => t.PersonId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IdentityUserRoles", "IdentityRole_Id", "dbo.IdentityRoles");
            DropForeignKey("dbo.Items", "VideoId", "dbo.Videos");
            DropForeignKey("dbo.Items", "TeachingAideId", "dbo.TeachingAides");
            DropForeignKey("dbo.Items", "OtherId", "dbo.Others");
            DropForeignKey("dbo.Items", "ItemSaleTransactionId", "dbo.ItemSaleTransactions");
            DropForeignKey("dbo.Items", "GameId", "dbo.Games");
            DropForeignKey("dbo.Items", "ConsignorPmtId", "dbo.ConsignorPmts");
            DropForeignKey("dbo.Items", "ConsignorId", "dbo.Consignors");
            DropForeignKey("dbo.StoreCreditTransactions", "Id", "dbo.CreditTransactions");
            DropForeignKey("dbo.SpaceRentalTransactions", "Id", "dbo.CreditTransactions");
            DropForeignKey("dbo.ItemSaleTransactions", "Id", "dbo.CreditTransactions");
            DropForeignKey("dbo.ClassPmtTransactions", "Id", "dbo.CreditTransactions");
            DropForeignKey("dbo.StoreCreditTransactions", "ConsignorId", "dbo.Consignors");
            DropForeignKey("dbo.StoreCreditPmts", "Id", "dbo.DebitTransactions");
            DropForeignKey("dbo.StoreCreditPmts", "ConsignorId", "dbo.Consignors");
            DropForeignKey("dbo.ConsignorPmts", "Id", "dbo.DebitTransactions");
            DropForeignKey("dbo.ConsignorPmts", "ConsignorId", "dbo.Consignors");
            DropForeignKey("dbo.Volunteers", "Id", "dbo.Persons");
            DropForeignKey("dbo.Users", "PersonUser_Id", "dbo.Persons");
            DropForeignKey("dbo.IdentityUserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.IdentityUserLogins", "User_Id", "dbo.Users");
            DropForeignKey("dbo.IdentityUserClaims", "UserId", "dbo.Users");
            DropForeignKey("dbo.PhoneNumbers", "Id", "dbo.Persons");
            DropForeignKey("dbo.Members", "Id", "dbo.Persons");
            DropForeignKey("dbo.MailingAddresses_Persons", "PersonId", "dbo.MailingAddresses");
            DropForeignKey("dbo.MailingAddresses_Persons", "MailingAddressId", "dbo.Persons");
            DropForeignKey("dbo.EmailAddresses_Persons", "PersonId", "dbo.Emails");
            DropForeignKey("dbo.EmailAddresses_Persons", "EmailAddressId", "dbo.Persons");
            DropForeignKey("dbo.Consignors", "Id", "dbo.Persons");
            DropForeignKey("dbo.Items", "BookId", "dbo.Books");
            DropIndex("dbo.MailingAddresses_Persons", new[] { "PersonId" });
            DropIndex("dbo.MailingAddresses_Persons", new[] { "MailingAddressId" });
            DropIndex("dbo.EmailAddresses_Persons", new[] { "PersonId" });
            DropIndex("dbo.EmailAddresses_Persons", new[] { "EmailAddressId" });
            DropIndex("dbo.SpaceRentalTransactions", new[] { "Id" });
            DropIndex("dbo.ItemSaleTransactions", new[] { "Id" });
            DropIndex("dbo.ClassPmtTransactions", new[] { "Id" });
            DropIndex("dbo.StoreCreditTransactions", new[] { "ConsignorId" });
            DropIndex("dbo.StoreCreditTransactions", new[] { "Id" });
            DropIndex("dbo.StoreCreditPmts", new[] { "ConsignorId" });
            DropIndex("dbo.StoreCreditPmts", new[] { "Id" });
            DropIndex("dbo.ConsignorPmts", new[] { "ConsignorId" });
            DropIndex("dbo.ConsignorPmts", new[] { "Id" });
            DropIndex("dbo.Volunteers", new[] { "Id" });
            DropIndex("dbo.IdentityUserRoles", new[] { "IdentityRole_Id" });
            DropIndex("dbo.IdentityUserRoles", new[] { "UserId" });
            DropIndex("dbo.IdentityUserLogins", new[] { "User_Id" });
            DropIndex("dbo.IdentityUserClaims", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "PersonUser_Id" });
            DropIndex("dbo.PhoneNumbers", new[] { "Id" });
            DropIndex("dbo.Members", new[] { "Id" });
            DropIndex("dbo.Consignors", new[] { "Id" });
            DropIndex("dbo.Items", new[] { "ConsignorPmtId" });
            DropIndex("dbo.Items", new[] { "ItemSaleTransactionId" });
            DropIndex("dbo.Items", new[] { "ConsignorId" });
            DropIndex("dbo.Items", new[] { "VideoId" });
            DropIndex("dbo.Items", new[] { "OtherId" });
            DropIndex("dbo.Items", new[] { "TeachingAideId" });
            DropIndex("dbo.Items", new[] { "GameId" });
            DropIndex("dbo.Items", new[] { "BookId" });
            DropTable("dbo.MailingAddresses_Persons");
            DropTable("dbo.EmailAddresses_Persons");
            DropTable("dbo.IdentityRoles");
            DropTable("dbo.Videos");
            DropTable("dbo.TeachingAides");
            DropTable("dbo.Others");
            DropTable("dbo.Games");
            DropTable("dbo.SpaceRentalTransactions");
            DropTable("dbo.ItemSaleTransactions");
            DropTable("dbo.ClassPmtTransactions");
            DropTable("dbo.CreditTransactions");
            DropTable("dbo.StoreCreditTransactions");
            DropTable("dbo.StoreCreditPmts");
            DropTable("dbo.DebitTransactions");
            DropTable("dbo.ConsignorPmts");
            DropTable("dbo.Volunteers");
            DropTable("dbo.IdentityUserRoles");
            DropTable("dbo.IdentityUserLogins");
            DropTable("dbo.IdentityUserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.PhoneNumbers");
            DropTable("dbo.Members");
            DropTable("dbo.MailingAddresses");
            DropTable("dbo.Emails");
            DropTable("dbo.Persons");
            DropTable("dbo.Consignors");
            DropTable("dbo.Items");
            DropTable("dbo.Books");
        }
    }
}
