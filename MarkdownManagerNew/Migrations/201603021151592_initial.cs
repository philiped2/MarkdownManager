namespace MarkdownManagerNew.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Name = c.String(),
                        Markdown = c.String(),
                        DateCreated = c.DateTime(),
                        LastChanged = c.DateTime(),
                        CreatorID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ApplicationUsers", t => t.CreatorID, cascadeDelete: true)
                .Index(t => t.CreatorID);
            
            CreateTable(
                "dbo.ApplicationUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        MailAdress = c.String(),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
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
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Filename = c.String(),
                        Description = c.String(),
                        ContentType = c.String(),
                        Data = c.Byte(nullable: false),
                        CreatorID = c.String(nullable: false, maxLength: 128),
                        DocumentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ApplicationUsers", t => t.CreatorID, cascadeDelete: true)
                .ForeignKey("dbo.Documents", t => t.DocumentID, cascadeDelete: false)
                .Index(t => t.CreatorID)
                .Index(t => t.DocumentID);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Name = c.String(),
                        CreatorID = c.String(nullable: false, maxLength: 128),
                        DateCreated = c.DateTime(),
                        LastChanged = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ApplicationUsers", t => t.CreatorID, cascadeDelete: true)
                .Index(t => t.CreatorID);
            
            CreateTable(
                "dbo.IdentityUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.IdentityUserRoles",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        IdentityRole_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.IdentityRoles", t => t.IdentityRole_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.IdentityRole_Id);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Label = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.IdentityRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GroupDocuments",
                c => new
                    {
                        Group_ID = c.Int(nullable: false),
                        Document_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Group_ID, t.Document_ID })
                .ForeignKey("dbo.Groups", t => t.Group_ID, cascadeDelete: true)
                .ForeignKey("dbo.Documents", t => t.Document_ID, cascadeDelete: false)
                .Index(t => t.Group_ID)
                .Index(t => t.Document_ID);
            
            CreateTable(
                "dbo.GroupApplicationUsers",
                c => new
                    {
                        Group_ID = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Group_ID, t.ApplicationUser_Id })
                .ForeignKey("dbo.Groups", t => t.Group_ID, cascadeDelete: true)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id, cascadeDelete: false)
                .Index(t => t.Group_ID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.TagDocuments",
                c => new
                    {
                        Tag_ID = c.Int(nullable: false),
                        Document_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_ID, t.Document_ID })
                .ForeignKey("dbo.Tags", t => t.Tag_ID, cascadeDelete: true)
                .ForeignKey("dbo.Documents", t => t.Document_ID, cascadeDelete: true)
                .Index(t => t.Tag_ID)
                .Index(t => t.Document_ID);
            
            CreateTable(
                "dbo.DocumentApplicationUsers",
                c => new
                    {
                        Document_ID = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Document_ID, t.ApplicationUser_Id })
                .ForeignKey("dbo.Documents", t => t.Document_ID, cascadeDelete: true)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id, cascadeDelete: false)
                .Index(t => t.Document_ID)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IdentityUserRoles", "IdentityRole_Id", "dbo.IdentityRoles");
            DropForeignKey("dbo.DocumentApplicationUsers", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.DocumentApplicationUsers", "Document_ID", "dbo.Documents");
            DropForeignKey("dbo.TagDocuments", "Document_ID", "dbo.Documents");
            DropForeignKey("dbo.TagDocuments", "Tag_ID", "dbo.Tags");
            DropForeignKey("dbo.Documents", "CreatorID", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserRoles", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserLogins", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.GroupApplicationUsers", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.GroupApplicationUsers", "Group_ID", "dbo.Groups");
            DropForeignKey("dbo.GroupDocuments", "Document_ID", "dbo.Documents");
            DropForeignKey("dbo.GroupDocuments", "Group_ID", "dbo.Groups");
            DropForeignKey("dbo.Groups", "CreatorID", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Files", "DocumentID", "dbo.Documents");
            DropForeignKey("dbo.Files", "CreatorID", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserClaims", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropIndex("dbo.DocumentApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.DocumentApplicationUsers", new[] { "Document_ID" });
            DropIndex("dbo.TagDocuments", new[] { "Document_ID" });
            DropIndex("dbo.TagDocuments", new[] { "Tag_ID" });
            DropIndex("dbo.GroupApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.GroupApplicationUsers", new[] { "Group_ID" });
            DropIndex("dbo.GroupDocuments", new[] { "Document_ID" });
            DropIndex("dbo.GroupDocuments", new[] { "Group_ID" });
            DropIndex("dbo.IdentityUserRoles", new[] { "IdentityRole_Id" });
            DropIndex("dbo.IdentityUserRoles", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserLogins", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Groups", new[] { "CreatorID" });
            DropIndex("dbo.Files", new[] { "DocumentID" });
            DropIndex("dbo.Files", new[] { "CreatorID" });
            DropIndex("dbo.IdentityUserClaims", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Documents", new[] { "CreatorID" });
            DropTable("dbo.DocumentApplicationUsers");
            DropTable("dbo.TagDocuments");
            DropTable("dbo.GroupApplicationUsers");
            DropTable("dbo.GroupDocuments");
            DropTable("dbo.IdentityRoles");
            DropTable("dbo.Tags");
            DropTable("dbo.IdentityUserRoles");
            DropTable("dbo.IdentityUserLogins");
            DropTable("dbo.Groups");
            DropTable("dbo.Files");
            DropTable("dbo.IdentityUserClaims");
            DropTable("dbo.ApplicationUsers");
            DropTable("dbo.Documents");
        }
    }
}
