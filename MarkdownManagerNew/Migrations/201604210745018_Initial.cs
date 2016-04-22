namespace MarkdownManagerNew.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DocumentRights",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        CanWrite = c.Boolean(nullable: false),
                        DocumentName = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Documents", t => t.ID)
                .Index(t => t.ID)
                .Index(t => t.ApplicationUser_Id);
            
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
                "dbo.Files",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Filename = c.String(),
                        ContentType = c.String(),
                        Data = c.Binary(),
                        Size = c.Int(nullable: false),
                        CreatorID = c.String(nullable: false, maxLength: 128),
                        DocumentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatorID, cascadeDelete: true)
                .ForeignKey("dbo.Documents", t => t.DocumentID, cascadeDelete: false)
                .Index(t => t.CreatorID)
                .Index(t => t.DocumentID);
            
            CreateTable(
                "dbo.GroupRights",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IsGroupAdmin = c.Boolean(nullable: false),
                        GroupId = c.Int(nullable: false),
                        GroupName = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
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
                "dbo.GroupDocuments",
                c => new
                    {
                        Group_ID = c.Int(nullable: false),
                        Document_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Group_ID, t.Document_ID })
                .ForeignKey("dbo.Groups", t => t.Group_ID, cascadeDelete: false)
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
                .ForeignKey("dbo.Groups", t => t.Group_ID, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: false)
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
                .ForeignKey("dbo.Documents", t => t.Document_ID, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: false)
                .Index(t => t.Document_ID)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.DocumentRights", "ID", "dbo.Documents");
            DropForeignKey("dbo.DocumentApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.DocumentApplicationUsers", "Document_ID", "dbo.Documents");
            DropForeignKey("dbo.TagDocuments", "Document_ID", "dbo.Documents");
            DropForeignKey("dbo.TagDocuments", "Tag_ID", "dbo.Tags");
            DropForeignKey("dbo.Documents", "CreatorID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.GroupApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.GroupApplicationUsers", "Group_ID", "dbo.Groups");
            DropForeignKey("dbo.GroupDocuments", "Document_ID", "dbo.Documents");
            DropForeignKey("dbo.GroupDocuments", "Group_ID", "dbo.Groups");
            DropForeignKey("dbo.Groups", "CreatorID", "dbo.AspNetUsers");
            DropForeignKey("dbo.GroupRights", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Files", "DocumentID", "dbo.Documents");
            DropForeignKey("dbo.Files", "CreatorID", "dbo.AspNetUsers");
            DropForeignKey("dbo.DocumentRights", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.DocumentApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.DocumentApplicationUsers", new[] { "Document_ID" });
            DropIndex("dbo.TagDocuments", new[] { "Document_ID" });
            DropIndex("dbo.TagDocuments", new[] { "Tag_ID" });
            DropIndex("dbo.GroupApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.GroupApplicationUsers", new[] { "Group_ID" });
            DropIndex("dbo.GroupDocuments", new[] { "Document_ID" });
            DropIndex("dbo.GroupDocuments", new[] { "Group_ID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.Groups", new[] { "CreatorID" });
            DropIndex("dbo.GroupRights", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Files", new[] { "DocumentID" });
            DropIndex("dbo.Files", new[] { "CreatorID" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.Documents", new[] { "CreatorID" });
            DropIndex("dbo.DocumentRights", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.DocumentRights", new[] { "ID" });
            DropTable("dbo.DocumentApplicationUsers");
            DropTable("dbo.TagDocuments");
            DropTable("dbo.GroupApplicationUsers");
            DropTable("dbo.GroupDocuments");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Tags");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Groups");
            DropTable("dbo.GroupRights");
            DropTable("dbo.Files");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Documents");
            DropTable("dbo.DocumentRights");
        }
    }
}
