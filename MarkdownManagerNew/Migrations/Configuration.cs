namespace MarkdownManagerNew.Migrations
{
    using MarkdownManagerNew.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Text;

    internal sealed class Configuration : DbMigrationsConfiguration<MarkdownManagerNew.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MarkdownManagerNew.Models.ApplicationDbContext context)
        {

            if (System.Diagnostics.Debugger.IsAttached == false)
                System.Diagnostics.Debugger.Launch();

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            try
            {




                if (!context.Roles.Any(r => r.Name == "Admin"))
                {
                    var role = new IdentityRole { Name = "Admin" };
                    roleManager.Create(role);
                }

                if (!context.Users.Any(u => u.UserName == "Admin"))
                {
                    var user = new ApplicationUser { UserName = "Admin", FirstName = "AdminFirstName", LastName = "AdminLastName", Email = "admin@markdownmanager.com" };

                    userManager.Create(user, "AdminAdmin123");
                    userManager.AddToRole(user.Id, "Admin");
                }

                if (!context.Roles.Any(r => r.Name == "User"))
                {
                    var role = new IdentityRole { Name = "User" };
                    roleManager.Create(role);
                }



                var philip = new ApplicationUser { UserName = "phiedi", FirstName = "Philip", LastName = "Edin", Email = "philip.e@markdownmanager.com" };

                var alexander = new ApplicationUser { UserName = "aleedi", FirstName = "Alexander", LastName = "Edin", Email = "alexander.e@markdownmanager.com" };

                var per = new ApplicationUser { UserName = "perolo", FirstName = "Per", LastName = "Olofsson", Email = "per.o@markdownmanager.com" };

                var andreas = new ApplicationUser { UserName = "andjoh", FirstName = "Andreas", LastName = "Johansson", Email = "andreas.j@markdownmanager.com" };

                var anton = new ApplicationUser { UserName = "antber", FirstName = "Anton", LastName = "Berglund", Email = "anton.b@markdownmanager.com" };



                if (!context.Users.Any(u => u.UserName == philip.UserName))
                {
                    userManager.Create(philip, "Password123");
                    userManager.AddToRole(philip.Id, "User");
                }

                if (!context.Users.Any(u => u.UserName == alexander.UserName))
                {
                    userManager.Create(alexander, "Password123");
                    userManager.AddToRole(alexander.Id, "User");
                }

                if (!context.Users.Any(u => u.UserName == per.UserName))
                {
                    userManager.Create(per, "Password123");
                    userManager.AddToRole(per.Id, "User");
                }

                if (!context.Users.Any(u => u.UserName == andreas.UserName))
                {
                    userManager.Create(andreas, "Password123");
                    userManager.AddToRole(andreas.Id, "User");
                }

                if (!context.Users.Any(u => u.UserName == anton.UserName))
                {
                    userManager.Create(anton, "Password123");
                    userManager.AddToRole(anton.Id, "User");
                }

                //var group1 = new Group { Name = "Drift", Description = "Gruppbeskrivning", CreatorID = andreas.Id };
                //var group2 = new Group { Name = "Plan", Description = "Gruppbeskrivning", CreatorID = anton.Id };
                //var group3 = new Group { Name = "Praktik", Description = "Gruppbeskrivning", CreatorID = alexander.Id };

                //context.Groups.AddOrUpdate(group1);  // GROUPS
                //context.Groups.AddOrUpdate(group2);
                //context.Groups.AddOrUpdate(group3);

                context.SaveChanges();

                //var group1 = new Group { Name = "Grupp1", Description = "Den f�rsta gruppen", CreatorID = philip.Id };
                //var group2 = new Group { Name = "Grupp2", Description = "Den andra gruppen", CreatorID = william.Id };


                //var tag1a = new Tag { Label = "tag1a" };
                //var tag1b = new Tag { Label = "tag1b" };
                //var tagGeneral = new Tag { Label = "General" };

                //var tag2a = new Tag { Label = "tag2a" };
                //var tag2b = new Tag { Label = "tag2b" };

                //var tag3a = new Tag { Label = "tag3a" };
                //var tag3b = new Tag { Label = "tag3b" };

                //var tag4a = new Tag { Label = "tag4a" };
                //var tag4b = new Tag { Label = "tag4b" };

                //var document1 = new Document { Name = "Document1", CreatorID = philip.Id, Description = "F�rsta dokumentet" };
                //var document2 = new Document { Name = "Document2", CreatorID = alexander.Id, Description = "Andra dokumentet" };
                //var document3 = new Document { Name = "Document3", CreatorID = william.Id, Description = "Tredje dokumentet" };
                //var document4 = new Document { Name = "Document4", CreatorID = olle.Id, Description = "Fj�rde dokumentet" };

                if (context.DeleteArchivedDocumentTimeSetting.Count() == 0)
                {
                    DeleteArchivedDocumentTimeSetting deleteArchivedDocumentTimeSetting = new DeleteArchivedDocumentTimeSetting() { Activated = false };
                    context.DeleteArchivedDocumentTimeSetting.Add(deleteArchivedDocumentTimeSetting); //L�gger till instans f�r inst�llningen i databasen. Avaktiverad som standard
                }


                //var GroupRightTest = new GroupRight { ID = group1.ID, IsGroupAdmin = true, GroupId = group1.ID };
                //var DocumentRightTest = new DocumentRight { ID = document2.ID, CanWrite = true, document = document2 };

                //var groupTest = new Group { Name = "Testgrupp", Description = "Testing read and write rights", CreatorID = william.Id };
                //var groupUser1 = new GroupUser { User = alexander, CanWrite = true, group = groupTest};
                //var groupUser2 = new GroupUser { User = philip, CanWrite = false, group = groupTest};

                //groupTest.GroupUsers.Add(groupUser1);
                //groupTest.GroupUsers.Add(groupUser2);

                //group1.Documents.Add(document1); // GROUP --> DOCUMENTS
                //group1.Documents.Add(document2);

                //group2.Documents.Add(document3);
                //group2.Documents.Add(document4);

                //document1.Groups.Add(group1); // DOCUMENT --> GROUPS AND USERS
                //document1.Users.Add(william);

                //document2.Groups.Add(group1);
                //document2.Users.Add(olle);

                //document3.Groups.Add(group2);
                //document3.Users.Add(philip);

                //document4.Groups.Add(group2);
                //document4.Users.Add(alexander);



                //document1.Tags.Add(tag1a);  // DOCUMENT --> TAGS
                //document1.Tags.Add(tag1b);
                //document1.Tags.Add(tagGeneral);

                //document2.Tags.Add(tag2a);
                //document2.Tags.Add(tag2b);
                //document2.Tags.Add(tagGeneral);

                //document3.Tags.Add(tag3a);
                //document3.Tags.Add(tag3b);
                //document3.Tags.Add(tagGeneral);

                //document4.Tags.Add(tag4a);
                //document4.Tags.Add(tag4b);
                //document4.Tags.Add(tagGeneral);

                //tag1a.Documents.Add(document1);  // TAG --> DOCUMENT
                //tag1b.Documents.Add(document1);
                //tagGeneral.Documents.Add(document1);

                //tag2a.Documents.Add(document2);
                //tag2b.Documents.Add(document2);
                //tagGeneral.Documents.Add(document2);

                //tag3a.Documents.Add(document3);
                //tag3b.Documents.Add(document3);
                //tagGeneral.Documents.Add(document3);

                //tag4a.Documents.Add(document4);
                //tag4b.Documents.Add(document4);
                //tagGeneral.Documents.Add(document4);

                //alexander.GroupRights.Add(GroupRightTest);                                  // USER --> GROUPRIGHTS                // GROUPRIGHTS
                //alexander.DocumentRights.Add(DocumentRightTest);

                // ------------   CONTEXT ADD OR UPDATE -------------

                //context.Documents.AddOrUpdate(document1);  // // DOCUMENTS
                //context.Documents.AddOrUpdate(document2);
                //context.Documents.AddOrUpdate(document3);
                //context.Documents.AddOrUpdate(document4);

                //context.Groups.AddOrUpdate(group1);  // GROUPS
                //context.Groups.AddOrUpdate(group2);

                //context.Tags.AddOrUpdate(tag1a); // TAGS
                //context.Tags.AddOrUpdate(tag1b);

                //context.Tags.AddOrUpdate(tag2a);
                //context.Tags.AddOrUpdate(tag2b);

                //context.Tags.AddOrUpdate(tag3a);
                //context.Tags.AddOrUpdate(tag3b);

                //context.Tags.AddOrUpdate(tag4a);
                //context.Tags.AddOrUpdate(tag4b);

                //context.Tags.AddOrUpdate(tagGeneral);


                //context.Users.AddOrUpdate(alexander);
                //context.Users.AddOrUpdate(william);
                //context.Users.AddOrUpdate(olle);

                //context.GroupRights.AddOrUpdate(GroupRightTest);
                //context.DocumentRights.AddOrUpdate(DocumentRightTest);



                //context.GroupUsers.AddOrUpdate(groupUser1);
                //context.GroupUsers.AddOrUpdate(groupUser2);

                //context.SaveChanges();


            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                ); // Add the original exception as the innerException
            }
        }
    }
}






