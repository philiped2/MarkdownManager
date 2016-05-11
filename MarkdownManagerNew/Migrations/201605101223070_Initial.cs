namespace MarkdownManagerNew.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DeleteArchivedDocumentTimeSettings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Activated = c.Boolean(nullable: false),
                        TimeValue = c.Int(nullable: false),
                        TimeUnit = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
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
                        IsArchived = c.Boolean(nullable: false),
                        TimeArchived = c.DateTime(),
                        CreatorID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatorID, cascadeDelete: true)
                .Index(t => t.CreatorID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
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
                        UserName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.LoginProvider, t.ProviderKey })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.UserDocumentRights",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CanWrite = c.Boolean(nullable: false),
                        DocumentId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Documents", t => t.DocumentId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.DocumentId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserGroupRights",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IsGroupAdmin = c.Boolean(nullable: false),
                        GroupId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.GroupId)
                .Index(t => t.UserId);
            
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
                .ForeignKey("dbo.AspNetUsers", t => t.CreatorID, cascadeDelete: true)
                .Index(t => t.CreatorID);
            
            CreateTable(
                "dbo.GroupDocumentRights",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CanWrite = c.Boolean(nullable: false),
                        DocumentId = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Documents", t => t.DocumentId, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: false)
                .Index(t => t.DocumentId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Label = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.TagDocuments", "Document_ID", "dbo.Documents");
            DropForeignKey("dbo.TagDocuments", "Tag_ID", "dbo.Tags");
            DropForeignKey("dbo.Documents", "CreatorID", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserGroupRights", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserGroupRights", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.GroupDocumentRights", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.GroupDocumentRights", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.Groups", "CreatorID", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserDocumentRights", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserDocumentRights", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.TagDocuments", new[] { "Document_ID" });
            DropIndex("dbo.TagDocuments", new[] { "Tag_ID" });
            DropIndex("dbo.GroupDocumentRights", new[] { "GroupId" });
            DropIndex("dbo.GroupDocumentRights", new[] { "DocumentId" });
            DropIndex("dbo.Groups", new[] { "CreatorID" });
            DropIndex("dbo.UserGroupRights", new[] { "UserId" });
            DropIndex("dbo.UserGroupRights", new[] { "GroupId" });
            DropIndex("dbo.UserDocumentRights", new[] { "UserId" });
            DropIndex("dbo.UserDocumentRights", new[] { "DocumentId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.Documents", new[] { "CreatorID" });
            DropTable("dbo.TagDocuments");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Tags");
            DropTable("dbo.GroupDocumentRights");
            DropTable("dbo.Groups");
            DropTable("dbo.UserGroupRights");
            DropTable("dbo.UserDocumentRights");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Documents");
            DropTable("dbo.DeleteArchivedDocumentTimeSettings");
        }
    }
}
