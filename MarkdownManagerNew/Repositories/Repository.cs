using MarkdownManagerNew.Models;
using MarkdownManagerNew.Viewmodels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace MarkdownManagerNew.Repositories
{
    public class Repository
    {
        private ApplicationDbContext dbContext;
        private RoleStore<IdentityRole> roleStore;
        private RoleManager<IdentityRole> roleManager;
        private UserStore<ApplicationUser> userStore;
        private UserManager<ApplicationUser> userManager;

        public Repository()
        {
            this.dbContext = new ApplicationDbContext();
            this.roleStore = new RoleStore<IdentityRole>(dbContext);
            this.roleManager = new RoleManager<IdentityRole>(roleStore);
            this.userStore = new UserStore<ApplicationUser>(dbContext);
            this.userManager = new UserManager<ApplicationUser>(userStore);  
        }

        public List<Document> GetUserDocuments(ApplicationUser user) //Fixa denna med lamba-linq sedan
        {
            List<Document> query = new List<Document>();
            var documentsByUserRights = user.Documents.ToList();
            
            foreach (var item in documentsByUserRights)
            {
                query.Add(item);
            }

            foreach (var group in user.Groups)
            {
                foreach (var document in group.Documents)
                {
                    query.Add(document);
                }
            }

            return query;

        }

        public List<Document> GetAllDocuments()
        {
            List<Document> documentList = dbContext.Documents.ToList();

            return documentList;
        }

        public List<Group> GetAllGroups()
        {
            List<Group> groupList = dbContext.Groups.ToList();

            return groupList;
        }

        public List<ApplicationUser> GetAllUsers()
        {
            List<ApplicationUser> userList = dbContext.Users.ToList();

            return userList;
        }

        public ApplicationUser GetUser (string userId)
        {
            ApplicationUser query = dbContext.Users
                .Where(x => x.Id == userId).Single();

            return query;
        }

        //public CreateGroupViewModel CreateGroup(List<string> members, ApplicationUser user, string name, string description, CreateGroupViewModel viewmodel)
        public void CreateGroup( CreateGroupViewModel viewmodel, ApplicationUser creator)
        {
            var groupToAdd = new Group { CreatorID = creator.Id };
            //// ändra parameters till: ApplicationUser user, List<ApplicationUser> groupMembers, List<Document> documents, string name, string description
            //var users = userManager.Users;
            groupToAdd.Description = viewmodel.Description;
            groupToAdd.Name = viewmodel.Name;

            foreach (var user in viewmodel.CheckBoxUsers.Where(x => x.IsChecked == true))
            {
                ApplicationUser groupUser = dbContext.Users.Where(x => x.Id == user.ID).Single();
                groupToAdd.Users.Add(groupUser);

            }

            foreach (var document in viewmodel.CheckBoxDocuments.Where(x => x.IsChecked == true))
            {
                Document groupDocument = dbContext.Documents.Where(x => x.ID == document.ID).Single();
                groupToAdd.Documents.Add(groupDocument);
            }

            //var newGroup = new Group();
            ////newGroup.CreatorID = user.Id;

            //foreach (string member in members)
            //{
            //    if (dbContext.Users.Where(u => u.Id == member))
            //    {
            //        newGroup.Users.Add(member);
            //    }
                
            //}
            ////newGroup.Users = groupMembers;
            ////newGroup.Documents = documents;
            //newGroup.Creator = user;
            //newGroup.Name = name;
            //newGroup.Description = description;

            dbContext.Groups.Add(groupToAdd);
            dbContext.SaveChanges();
        }

        public void CreateUser(CreateUserViewModel viewmodel, ApplicationUser creator)
        {
            var userToAdd = new ApplicationUser { UserName = viewmodel.UserName, FirstName = viewmodel.FirstName, LastName = viewmodel.LastName, Email = viewmodel.MailAdress};

            foreach (var group in viewmodel.Groups.Where(x => x.IsChecked == true))
            {
                Group userGroup = dbContext.Groups.Where(x => x.ID == group.ID).Single();
                userToAdd.Groups.Add(userGroup);

            }

            foreach (var document in viewmodel.Documents.Where(x => x.IsChecked == true))
            {
                Document userDocument = dbContext.Documents.Where(x => x.ID == document.ID).Single();
                userToAdd.Documents.Add(userDocument);
            }

            if (viewmodel.admin)
            {
                userManager.Create(userToAdd, "AdminAdmin123");
                userManager.AddToRole(userToAdd.Id, "Admin"); 
            }

            else
            {
                userManager.Create(userToAdd, "Password123");
                userManager.AddToRole(userToAdd.Id, "User");
            }
        }

        public void CreateDocument(CreateDocumentViewModel viewmodel, ApplicationUser creator)
        {
            var documentToAdd = new Document { CreatorID = creator.Id, Name = viewmodel.Name, Description = viewmodel.Description, Markdown = viewmodel.Markdown};

            foreach (var user in viewmodel.CheckboxUsers.Where(x => x.IsChecked == true))
            {
                ApplicationUser documentUser = dbContext.Users.Where(x => x.Id == user.ID).Single();
                documentToAdd.Users.Add(documentUser);

            }

            foreach (var group in viewmodel.CheckboxGroups.Where(x => x.IsChecked == true))
            {
                Group documentGroup = dbContext.Groups.Where(x => x.ID == group.ID).Single();
                documentToAdd.Groups.Add(documentGroup);

            }

            foreach (var tag in viewmodel.CheckboxTags.Where(x => x.IsChecked == true))
            {
                Tag documentTag = dbContext.Tags.Where(x => x.ID == tag.ID).Single();
                documentToAdd.Tags.Add(documentTag);

            }

            dbContext.Documents.Add(documentToAdd);
            dbContext.SaveChanges();
        }

        public List<ApplicationUser> ListUsersToCreateGroup()
        {
            var users = userManager.Users.ToList();

            return users;
        }


        public void CreateTag(Tag model)
        {
            var tagToAdd = new Tag { Label = model.Label };
            dbContext.Tags.Add(tagToAdd);
            dbContext.SaveChanges();
        }

        public List<Tag> GetAllTags()
        {
            List<Tag> tagList = dbContext.Tags.ToList();
            return tagList;
        }

    //    public File CreateFile(HttpPostedFileBase upload, ApplicationUser user)
    //    {
    //        if (upload != null && upload.ContentLength > 0)
    //        {
    //            var reader = new System.IO.BinaryReader(upload.InputStream);
    //            var newFile = new File
    //            {
    //                Filename = System.IO.Path.GetFileName(upload.FileName),
    //                Description = upload.Description
    //                ContentType = upload.ContentType,
    //                Data = reader.ReadBytes(upload.ContentLength),
    //                Creator = user,
    //                CreatorID = user.Id,
    //                DocumentID = upload.DocumentID
    //            };

    //            //var count = dbContext.Files
    //            //    .Where(f => f.FileURL == newFile.FileURL || (f.FileName == newFile.FileName && f.className == newFile.className));

    //            if (count.Count() == 0) //Om det inte redan finns en fil med samma URL
    //            {
    //                return newFile;
    //            }

    //            else
    //            {
    //                return null;
    //            }
    //        }
    //}
}