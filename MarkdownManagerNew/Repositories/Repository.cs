using MarkdownManagerNew.Models;
using MarkdownManagerNew.Viewmodels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
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
                if (item.IsArchived == false)
                {
                    query.Add(item);
                }
            }

            foreach (var group in user.Groups)
            {
                foreach (var document in group.Documents)
                {
                    if (document.IsArchived == false)
                    {
                        query.Add(document);
                    }
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

        public List<Group> GetUserGroups(ApplicationUser user)
        {
            List<Group> userGroups = new List<Group>();

            foreach (Group group in user.Groups)
            {
                userGroups.Add(group);
            }

            return userGroups;
        }

        public List<ApplicationUser> GetAllUsers()
        {
            List<ApplicationUser> userList = dbContext.Users.ToList();

            return userList;
        }

        public ApplicationUser GetUser(string userId)
        {
            ApplicationUser query = dbContext.Users
                .Where(x => x.Id == userId).Single();

            return query;
        }

        //public CreateGroupViewModel CreateGroup(List<string> members, ApplicationUser user, string name, string description, CreateGroupViewModel viewmodel)
        public void CreateGroup(CreateGroupViewModel viewmodel, ApplicationUser creator)
        {
            var groupToAdd = new Group { CreatorID = creator.Id };

            //// ändra parameters till: ApplicationUser user, List<ApplicationUser> groupMembers, List<Document> documents, string name, string description
            //var users = userManager.Users;
            groupToAdd.Description = viewmodel.Description;
            groupToAdd.Name = viewmodel.Name;
            //GroupRight userGroupRights = new GroupRight();
            ApplicationUser theUser;

            foreach (var user in viewmodel.CheckBoxUsers.Where(x => x.IsChecked == true))
            {
                GroupRight userGroupRights = new GroupRight();
                //userGroupRights.GroupId = groupToAdd.ID;  //bortkommenterad för att låsa FKEY som länk ist
                //userGroupRights.GroupName = groupToAdd.Name; //bortkommenterad för att låsa FKEY som länk ist
                //userGroupRights.group = groupToAdd;
                ApplicationUser groupUser = dbContext.Users.Where(x => x.Id == user.ID).Single();
                //groupToAdd.Users.Add(groupUser);

                if (user.IsGroupAdmin == true)
                {
                    userGroupRights.IsGroupAdmin = true;
                }

                if (user.CanEdit == true)
                {
                    userGroupRights.IsGroupAdmin = true;
                }

                //userGroupRights.GroupId = groupToAdd.ID;
                theUser = groupUser;
                theUser.GroupRights.Add(userGroupRights);
                groupToAdd.Users.Add(theUser);
                //dbContext.SaveChanges();
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

            //userGroupRights.GroupId = groupToAdd.ID;
            
            
            dbContext.Groups.Add(groupToAdd);
            dbContext.SaveChanges();

        }

        //public void GetGroupToEdit(Group group)
        //{
        //    Group dbGroup = dbContext.Groups.Where(x => x.ID == group.ID);

        //}

        public void EditGroup2(EditGroupViewModel viewmodel)
        {
            //Group group = viewmodel.GroupToEdit;
            Group group = dbContext.Groups.Where(x => x.ID == viewmodel.GroupToEdit.ID).Single();
            group.Description = viewmodel.Description;
            group.Name = viewmodel.Name;

            //ApplicationUser theUser;

            foreach (var user in viewmodel.CheckBoxUsers.Where(x => x.IsChecked == true))
            {

                ApplicationUser updatedGroupUser = dbContext.Users.Where(x => x.Id == user.ID).Single();
                //ApplicationUser groupUser = group.Users.Where(x => x.Id == user.ID).Single();
                GroupRight userGroupRights;

                //ApplicationUser NotUpdatedGroupUser = group.Users.Where(x => x.Id == updatedGroupUser.Id).Single();

                if (updatedGroupUser.GroupRights.Any(x => x.GroupId == group.ID)) //ändrat från GroupName för att låsa FKEY som länk ist
                {
                    userGroupRights = updatedGroupUser.GroupRights.Where(x => x.GroupId == group.ID).Single(); //ändrat från GroupName för att låsa FKEY som länk ist
                    //updatedGroupUser = group.Users.Where(x => x.Id == updatedGroupUser.Id).Single();
                }

                else
                {
                    userGroupRights = new GroupRight();
                    userGroupRights.GroupId = group.ID; //ändrat från GroupName för att låsa FKEY som länk ist
                    updatedGroupUser.GroupRights.Add(userGroupRights);
                    group.Users.Add(updatedGroupUser);
                }
                

                if (user.IsGroupAdmin == true)
                {
                    userGroupRights.IsGroupAdmin = true;
                }

                if (user.CanEdit == true)
                {
                    userGroupRights.IsGroupAdmin = true;
                }

                if (user.UserToDelete == true)
                {
                    //ApplicationUser theUser = dbContext.Users.Where(x => x.Id == user.ID).Single();
                    group.Users.Remove(updatedGroupUser);
                    //for (int i = 0; i < group.Users.Count; i++)
                    //{
                    //    if (group.Users[i] == user)
                    //}
                }



                
 

                //if (!group.Users.Any(x => x.Id == updatedGroupUser.Id))
                //{
                //    //NotUpdatedGroupUser = updatedGroupUser;
                //    group.Users.Add(updatedGroupUser);
                //}

                //else
                //{
                //    NotUpdatedGroupUser = new ApplicationUser();
                //}

                

                //theUser = groupUser;
                
            }

            foreach (var document in viewmodel.CheckBoxDocuments.Where(x => x.IsChecked == true))
            {
                Document groupDocument = dbContext.Documents.Where(x => x.ID == document.ID).Single();

                if (!group.Documents.Any(x => x.ID == groupDocument.ID))
                {
                    group.Documents.Add(groupDocument);
                }

                if (document.DocumentToDelete == true)
                {
                    //ApplicationUser theUser = dbContext.Users.Where(x => x.Id == user.ID).Single();
                    group.Documents.Remove(groupDocument);
                    //for (int i = 0; i < group.Users.Count; i++)
                    //{
                    //    if (group.Users[i] == user)
                    //}
                }

            }



            //group.Description = viewmodel.Description;
            //group.Name = viewmodel.Name;
            dbContext.SaveChanges();
        }



        public void EditGroup(EditGroupViewModel viewmodel)
        {
            Group group = viewmodel.GroupToEdit;
            group.Description = viewmodel.Description;
            group.Name = viewmodel.Name;
            //GroupRight userGroupRights = new GroupRight();
            ApplicationUser theUser;

            foreach (var user in viewmodel.CheckBoxUsers)
            {
                //GroupRight userGroupRights = new GroupRight();
                //userGroupRights.GroupId = groupToAdd.ID;  // group har inget ID förens denna sparas i databasen...!
                //userGroupRights.GroupName = groupToAdd.Name;
                //userGroupRights.group = groupToAdd;
                ApplicationUser groupUser = dbContext.Users.Where(x => x.Id == user.ID).Single();
                GroupRight userGroupRights = groupUser.GroupRights.Where(x => x.GroupId == group.ID).Single();
                //groupToAdd.Users.Add(groupUser);

                if (user.IsGroupAdmin == true)
                {
                    userGroupRights.IsGroupAdmin = true;
                }

                if (user.CanEdit == true)
                {
                    userGroupRights.IsGroupAdmin = true;
                }

                //userGroupRights.GroupId = groupToAdd.ID;
                theUser = groupUser;
                //theUser.GroupRights.Add(userGroupRights);
                //groupToAdd.Users.Add(theUser);
                //dbContext.SaveChanges();
            }

            //foreach (var document in viewmodel.CheckBoxDocuments.Where(x => x.IsChecked == true))
            //{
            //    Document groupDocument = dbContext.Documents.Where(x => x.ID == document.ID).Single();
            //    groupToAdd.Documents.Add(groupDocument);
            //}

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

            //userGroupRights.GroupId = groupToAdd.ID;


            //dbContext.Groups.Add(groupToAdd);
            dbContext.SaveChanges();
        }

        public void CreateUser(CreateUserViewModel viewmodel, ApplicationUser creator)
        {
            var userToAdd = new ApplicationUser { UserName = viewmodel.UserName, FirstName = viewmodel.FirstName, LastName = viewmodel.LastName, Email = viewmodel.MailAdress };

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
            var documentToAdd = new Document { CreatorID = creator.Id, Name = viewmodel.Name, Description = viewmodel.Description, Markdown = viewmodel.Markdown };

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

            foreach (var file in viewmodel.Files)
            {
                documentToAdd.Files.Add(file);

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

        public void LogChanges(Group group, ApplicationUser user)
        {
            DateTime timeChanged = DateTime.Now;
            //ApplicationUser currentUser = GetUser();
            //Group dbGroup = dbContext.Groups.Find(group.ID);
            Group dbGroup = dbContext.Groups.Where(x => x.ID == group.ID).Single();
            group = dbGroup;
            //string newChangelog = "Användare: " + user.UserName + ", " + "Time: " + group.LastChanged;

            group.LastChanged = timeChanged;
            //group.ChangeLog.Add("Användare: " + user.UserName + ", " + "Time: " + group.LastChanged);
            //dbContext.Groups.Where(x => x.ID == dbGroup.ID).Single().ChangeLog.Add(newChangelog);

            //group.ChangeLog.Add("Användare: " + user.UserName + ", " + "Time: " + group.LastChanged);
            //dbContext.Entry(group.ChangeLog).State = EntityState.Modified;
            //dbContext.SaveChanges();

            //dbContext.Groups.Attach(group);
            string newLog = "Användare: " + user.UserName + ", " + "Time: " + group.LastChanged;
            group.ChangeLog.Add(newLog);

            dbContext.Entry(dbGroup).CurrentValues.SetValues(group);
            dbContext.SaveChanges();
        }

        public void LogDocumentChanges(Document document, ApplicationUser user)
        {
            DateTime timeChanged = DateTime.Now;
            //ApplicationUser currentUser = GetUser();
            //Group dbGroup = dbContext.Groups.Find(group.ID);
            Document dbDocument = dbContext.Documents.Where(x => x.ID == document.ID).Single();
            document = dbDocument;
            //string newChangelog = "Användare: " + user.UserName + ", " + "Time: " + group.LastChanged;

            document.LastChanged = timeChanged;
            //group.ChangeLog.Add("Användare: " + user.UserName + ", " + "Time: " + group.LastChanged);
            //dbContext.Groups.Where(x => x.ID == dbGroup.ID).Single().ChangeLog.Add(newChangelog);

            //group.ChangeLog.Add("Användare: " + user.UserName + ", " + "Time: " + group.LastChanged);
            //dbContext.Entry(group.ChangeLog).State = EntityState.Modified;
            //dbContext.SaveChanges();

            //dbContext.Groups.Attach(group);
            string newLog = "Användare: " + user.UserName + ", " + "Time: " + document.LastChanged;
            document.ChangeLog.Add(newLog);

            //dbContext.Entry(document).State = EntityState.Added;

            dbContext.Entry(dbDocument).CurrentValues.SetValues(document);

            dbContext.SaveChanges();
        }

        public List<Tag> GetAllTags()
        {
            List<Tag> tagList = dbContext.Tags.ToList();
            return tagList;
        }

        public File CreateFile(HttpPostedFileBase upload, ApplicationUser user)
        {
            var reader = new System.IO.BinaryReader(upload.InputStream);
            var newFile = new File
            {
                Filename = System.IO.Path.GetFileName(upload.FileName),
                ContentType = upload.ContentType,
                Data = reader.ReadBytes(upload.ContentLength),
                CreatorID = user.Id,
                Size = upload.ContentLength
            };
            return newFile;
        }

        public List<File> CreateFileListFromJson(string json, ApplicationUser user)
        {
            dynamic fileArray = JsonConvert.DeserializeObject(json);

            List<File> fileList = new List<File>();
            //var binaryReader = new System.IO.BinaryReader(upload.InputStream);

            foreach(var file in fileArray)
            {
                //var newFile = new File
                //{
                //    Filename = file.Filename,
                //    ContentType = file.ContentType,
                //    Data = file.Data,
                //    CreatorID = user.Id
                //};
                //fileList.Add(newFile);
                string Filename = file.Filename;
                string ContentType = file.ContentType;
                byte[] Data = System.Text.Encoding.UTF8.GetBytes(file.Data);
                //byte[] Data = file.Data;
                string CreatorID = user.Id;

            }


            return fileList;
        }


        public List<string> GetTagsByName(string tagLabel)
        {
            var query = dbContext.Tags
                .Where(t => t.Label.Contains(tagLabel))
                .Select(t=>t.Label).ToList();
            return query;
        }

        public List<string> GetUserByUserName(string userName)
        {
            var query = dbContext.Users
                .Where(t => t.UserName.Contains(userName))
                .Select(t => t.UserName).ToList();
            return query;
        }

        public void ArchiveDocument(Document document)
        {
            Document dbDocument = dbContext.Documents.Where(x => x.ID == document.ID).Single();
            document.IsArchived = true;
            dbContext.Entry(dbDocument).CurrentValues.SetValues(document);
            dbContext.SaveChanges();
        }

        public List<ListUserViewModel> GetUsersByName(string keyword)
        {
            var query = dbContext.Users
                .Where(u => u.FirstName.Contains(keyword) || u.LastName.Contains(keyword))
                .Select(u => new ListUserViewModel {
                    FullName = u.FirstName + " " + u.LastName,
                    ID = u.Id,
                    Rights = "Read"
                })
                .ToList();
            return query;
        }

        public List<ListGroupViewModel> GetAuthGroupsByName(string keyword, ApplicationUser currentUser)
        {
            var query = dbContext.Groups
                .Where(g => g.Users.Any(u => u.Id == currentUser.Id) && g.Name.Contains(keyword) || g.Users.Any(u => u.Id == currentUser.Id) && g.Description.Contains(keyword))
                .Select(g => new ListGroupViewModel
                {
                    Name = g.Name,
                    ID = g.ID,
                    Description = g.Description,
                    Rights = "Read",
                    Users = g.Users.Select(u => u.FirstName + " " + u.LastName).ToList()
                })
                .ToList();
            return query;
        }
    }
}