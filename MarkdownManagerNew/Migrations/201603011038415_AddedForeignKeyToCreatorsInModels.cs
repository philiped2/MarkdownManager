namespace MarkdownManagerNew.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedForeignKeyToCreatorsInModels : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationUserDocuments", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserDocuments", "Document_Id", "dbo.Documents");
            DropForeignKey("dbo.ApplicationUserGroups", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserGroups", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.Files", "DocumentId", "dbo.AspNetUsers");
            DropIndex("dbo.Files", new[] { "DocumentId" });
            DropIndex("dbo.ApplicationUserDocuments", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserDocuments", new[] { "Document_Id" });
            DropIndex("dbo.ApplicationUserGroups", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserGroups", new[] { "Group_Id" });
            AddColumn("dbo.Documents", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Groups", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "Group_Id", c => c.Int());
            AddColumn("dbo.AspNetUsers", "Document_Id", c => c.Int());
            AlterColumn("dbo.Documents", "CreatorId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Groups", "CreatorId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Files", "DocumentId", c => c.Int(nullable: false));
            CreateIndex("dbo.Documents", "CreatorId");
            CreateIndex("dbo.Documents", "ApplicationUser_Id");
            CreateIndex("dbo.AspNetUsers", "Group_Id");
            CreateIndex("dbo.AspNetUsers", "Document_Id");
            CreateIndex("dbo.Groups", "CreatorId");
            CreateIndex("dbo.Groups", "ApplicationUser_Id");
            CreateIndex("dbo.Files", "DocumentId");
            AddForeignKey("dbo.Documents", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Groups", "CreatorId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "Group_Id", "dbo.Groups", "Id");
            AddForeignKey("dbo.Groups", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Documents", "CreatorId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "Document_Id", "dbo.Documents", "Id");
            AddForeignKey("dbo.Files", "DocumentId", "dbo.Documents", "Id", cascadeDelete: true);
            DropTable("dbo.ApplicationUserDocuments");
            DropTable("dbo.ApplicationUserGroups");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ApplicationUserGroups",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Group_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Group_Id });
            
            CreateTable(
                "dbo.ApplicationUserDocuments",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Document_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Document_Id });
            
            DropForeignKey("dbo.Files", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.AspNetUsers", "Document_Id", "dbo.Documents");
            DropForeignKey("dbo.Documents", "CreatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Groups", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.Groups", "CreatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Documents", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Files", new[] { "DocumentId" });
            DropIndex("dbo.Groups", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Groups", new[] { "CreatorId" });
            DropIndex("dbo.AspNetUsers", new[] { "Document_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Group_Id" });
            DropIndex("dbo.Documents", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Documents", new[] { "CreatorId" });
            AlterColumn("dbo.Files", "DocumentId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Groups", "CreatorId", c => c.String());
            AlterColumn("dbo.Documents", "CreatorId", c => c.String());
            DropColumn("dbo.AspNetUsers", "Document_Id");
            DropColumn("dbo.AspNetUsers", "Group_Id");
            DropColumn("dbo.Groups", "ApplicationUser_Id");
            DropColumn("dbo.Documents", "ApplicationUser_Id");
            CreateIndex("dbo.ApplicationUserGroups", "Group_Id");
            CreateIndex("dbo.ApplicationUserGroups", "ApplicationUser_Id");
            CreateIndex("dbo.ApplicationUserDocuments", "Document_Id");
            CreateIndex("dbo.ApplicationUserDocuments", "ApplicationUser_Id");
            CreateIndex("dbo.Files", "DocumentId");
            AddForeignKey("dbo.Files", "DocumentId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.ApplicationUserGroups", "Group_Id", "dbo.Groups", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ApplicationUserGroups", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ApplicationUserDocuments", "Document_Id", "dbo.Documents", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ApplicationUserDocuments", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
