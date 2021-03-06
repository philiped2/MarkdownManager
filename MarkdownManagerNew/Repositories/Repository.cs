﻿using MarkdownManagerNew.Models;
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

        public List<Document> GetAuthorisedUserDocuments(ApplicationUser user) //Fixa denna med lamba-linq sedan
        {
            List<Document> query = new List<Document>();

            List<Document> dbDocuments = dbContext.Documents.Where(x => x.IsArchived == false).ToList();

            foreach (Document doc in dbDocuments)                  // usern får en rätt userright i sin lista men metoden kastar ej in doc i query
            {
                if (user.UserDocumentRights.Any(x => x.DocumentId == doc.ID) ||
                    dbContext.UserGroupRights.Any(x => x.UserId == user.Id &&
                        dbContext.GroupDocumentRights.Any(g => g.GroupId == x.GroupId &&
                            g.DocumentId == doc.ID)))
                {
                    query.Add(doc);
                }
                
            }
            return query;

        }

        public List<DocumentListModel> GetAuthorisedUserDocumentsByKeyword(string keyword, ApplicationUser user) //Fixa denna med lamba-linq sedan
        {
            List<Document> query = new List<Document>();

            List<Document> dbDocuments = dbContext.Documents.Where(x => x.IsArchived == false && x.Name.Contains(keyword) || x.IsArchived == false && x.Description.Contains(keyword) || x.IsArchived == false && x.Tags.Any(t=>t.Label.Contains(keyword))).ToList();

            foreach (Document doc in dbDocuments)                  // usern får en rätt userright i sin lista men metoden kastar ej in doc i query
            {
                if (user.UserDocumentRights.Any(x => x.DocumentId == doc.ID) ||
                    dbContext.UserGroupRights.Any(x => x.UserId == user.Id &&
                        dbContext.GroupDocumentRights.Any(g => g.GroupId == x.GroupId &&
                            g.DocumentId == doc.ID)))
                {
                    query.Add(doc);
                }

             }

            var fixedquery = query.Select(d => new DocumentListModel
            {
                ID = d.ID,
                Name = d.Name,
                Description = d.Description,
                DateCreated = d.DateCreated.ToString(),
                LastChanged = d.LastChanged.ToString(),
                Creator = d.Creator.FirstName
            }).ToList();

            return fixedquery;

        }

        public List<Document> Get10LatestAuthorisedUserDocuments(ApplicationUser user) //Fixa denna med lamba-linq sedan
        {
            List<Document> query = new List<Document>();

            List<Document> dbDocuments = dbContext.Documents.Where(x => x.IsArchived == false).ToList();

            /*foreach (Document doc in dbDocuments.OrderBy(d=>d.DateCreated)) */                 // usern får en rätt userright i sin lista men metoden kastar ej in doc i query

            if (dbDocuments.Count<10)
            {
                for (int i = 0; i < dbDocuments.Count; i++)
                {
                    if (dbDocuments[i] != null)
                    {
                        if (user.UserDocumentRights.Any(x => x.DocumentId == dbDocuments[i].ID) ||
                        dbContext.UserGroupRights.Any(x => x.UserId == user.Id &&
                            dbContext.GroupDocumentRights.Any(g => g.GroupId == x.GroupId &&
                                g.DocumentId == dbDocuments[i].ID)))
                        {
                            query.Add(dbDocuments[i]);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    if (dbDocuments[i] != null)
                    {
                        if (user.UserDocumentRights.Any(x => x.DocumentId == dbDocuments[i].ID) ||
                        dbContext.UserGroupRights.Any(x => x.UserId == user.Id &&
                            dbContext.GroupDocumentRights.Any(g => g.GroupId == x.GroupId &&
                                g.DocumentId == dbDocuments[i].ID)))
                        {
                            query.Add(dbDocuments[i]);
                            i++;
                        }
                    }
                }
            }

            return query;

        }

        public List<int> ListUsersGroupDocumentRights(ApplicationUser user, List<int> usersGroupDocumentRightsById)
        {
            List<Document> dbDocuments = dbContext.Documents.Where(x => x.IsArchived == false).ToList();


            foreach (Document doc in dbDocuments)                  
            {
                if (user.UserDocumentRights.Any(x => x.DocumentId == doc.ID && x.CanWrite == true) ||
                    dbContext.UserGroupRights.Any(x => x.UserId == user.Id &&
                        dbContext.GroupDocumentRights.Any(g => g.GroupId == x.GroupId &&
                            g.DocumentId == doc.ID)))
                {
                    //int id;
                    //id = doc.ID;
                    usersGroupDocumentRightsById.Add(doc.ID);
                }
                
            }
            
            return usersGroupDocumentRightsById;
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

            //foreach (Group group in user.Groups)
            //{
            //    userGroups.Add(group);
            //}

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
        //public void CreateGroup(CreateGroupViewModel viewmodel, ApplicationUser creator)
        //{
        //    var groupToAdd = new Group { CreatorID = creator.Id };

        //    //// ändra parameters till: ApplicationUser user, List<ApplicationUser> groupMembers, List<Document> documents, string name, string description
        //    //var users = userManager.Users;
        //    groupToAdd.Description = viewmodel.Description;
        //    groupToAdd.Name = viewmodel.Name;
        //    //GroupRight userGroupRights = new GroupRight();
        //    ApplicationUser theUser;

        //    foreach (var user in viewmodel.CheckBoxUsers.Where(x => x.IsChecked == true))
        //    {
        //        GroupRight userGroupRights = new GroupRight();
        //        //userGroupRights.GroupId = groupToAdd.ID;  //bortkommenterad för att låsa FKEY som länk ist
        //        //userGroupRights.GroupName = groupToAdd.Name; //bortkommenterad för att låsa FKEY som länk ist
        //        //userGroupRights.group = groupToAdd;
        //        ApplicationUser groupUser = dbContext.Users.Where(x => x.Id == user.ID).Single();
        //        //groupToAdd.Users.Add(groupUser);

        //        if (user.IsGroupAdmin == true)
        //        {
        //            userGroupRights.IsGroupAdmin = true;
        //        }

        //        if (user.CanEdit == true)
        //        {
        //            userGroupRights.IsGroupAdmin = true;
        //        }

        //        //userGroupRights.GroupId = groupToAdd.ID;
        //        theUser = groupUser;
        //        theUser.GroupRights.Add(userGroupRights);
        //        groupToAdd.Users.Add(theUser);
        //        //dbContext.SaveChanges();
        //    }

        //    foreach (var document in viewmodel.CheckBoxDocuments.Where(x => x.IsChecked == true))
            //    {
        //        Document groupDocument = dbContext.Documents.Where(x => x.ID == document.ID).Single();
        //        groupToAdd.Documents.Add(groupDocument);
            //    }

        //    //var newGroup = new Group();
        //    ////newGroup.CreatorID = user.Id;

        //    //foreach (string member in members)
        //    //{
        //    //    if (dbContext.Users.Where(u => u.Id == member))
        //    //    {
        //    //        newGroup.Users.Add(member);
        //    //    }
            
        //    //}
        //    ////newGroup.Users = groupMembers;
        //    ////newGroup.Documents = documents;
        //    //newGroup.Creator = user;
        //    //newGroup.Name = name;
        //    //newGroup.Description = description;
            
        //    //userGroupRights.GroupId = groupToAdd.ID;


        //    dbContext.Groups.Add(groupToAdd);
        //    dbContext.SaveChanges();

        //}

        //public void GetGroupToEdit(Group group)
        //{
        //    Group dbGroup = dbContext.Groups.Where(x => x.ID == group.ID);

        //}

        //public void EditGroup2(EditGroupViewModel viewmodel)
        //{
        //    //Group group = viewmodel.GroupToEdit;
        //    Group group = dbContext.Groups.Where(x => x.ID == viewmodel.GroupToEdit.ID).Single();
        //    group.Description = viewmodel.Description;
        //    group.Name = viewmodel.Name;

        //    //ApplicationUser theUser;

        //    foreach (var user in viewmodel.CheckBoxUsers.Where(x => x.IsChecked == true))
        //    {

        //        ApplicationUser updatedGroupUser = dbContext.Users.Where(x => x.Id == user.ID).Single();
        //        //ApplicationUser groupUser = group.Users.Where(x => x.Id == user.ID).Single();
        //        GroupRight userGroupRights;

        //        //ApplicationUser NotUpdatedGroupUser = group.Users.Where(x => x.Id == updatedGroupUser.Id).Single();

        //        if (updatedGroupUser.GroupRights.Any(x => x.GroupId == group.ID)) //ändrat från GroupName för att låsa FKEY som länk ist
        //        {
        //            userGroupRights = updatedGroupUser.GroupRights.Where(x => x.GroupId == group.ID).Single(); //ändrat från GroupName för att låsa FKEY som länk ist
        //            //updatedGroupUser = group.Users.Where(x => x.Id == updatedGroupUser.Id).Single();
        //        }

        //        else
        //        {
        //            userGroupRights = new GroupRight();
        //            userGroupRights.GroupId = group.ID; //ändrat från GroupName för att låsa FKEY som länk ist
        //            updatedGroupUser.GroupRights.Add(userGroupRights);
        //            group.Users.Add(updatedGroupUser);
        //        }
                

        //        if (user.IsGroupAdmin == true)
        //        {
        //            userGroupRights.IsGroupAdmin = true;
        //        }

        //        if (user.CanEdit == true)
        //        {
        //            userGroupRights.IsGroupAdmin = true;
        //        }

        //        if (user.UserToDelete == true)
        //        {
        //            //ApplicationUser theUser = dbContext.Users.Where(x => x.Id == user.ID).Single();
        //            group.Users.Remove(updatedGroupUser);
        //            //for (int i = 0; i < group.Users.Count; i++)
        //            //{
        //            //    if (group.Users[i] == user)
        //            //}
        //        }



                
 

        //        //if (!group.Users.Any(x => x.Id == updatedGroupUser.Id))
        //        //{
        //        //    //NotUpdatedGroupUser = updatedGroupUser;
        //        //    group.Users.Add(updatedGroupUser);
        //        //}

        //        //else
        //        //{
        //        //    NotUpdatedGroupUser = new ApplicationUser();
        //        //}

                

        //        //theUser = groupUser;
                
        //    }

        //    foreach (var document in viewmodel.CheckBoxDocuments.Where(x => x.IsChecked == true))
        //    {
        //        Document groupDocument = dbContext.Documents.Where(x => x.ID == document.ID).Single();

        //        if (!group.Documents.Any(x => x.ID == groupDocument.ID))
        //        {
        //            group.Documents.Add(groupDocument);
        //        }

        //        if (document.DocumentToDelete == true)
        //        {
        //            //ApplicationUser theUser = dbContext.Users.Where(x => x.Id == user.ID).Single();
        //            group.Documents.Remove(groupDocument);
        //            //for (int i = 0; i < group.Users.Count; i++)
        //            //{
        //            //    if (group.Users[i] == user)
        //            //}
        //        }

        //    }



        //    //group.Description = viewmodel.Description;
        //    //group.Name = viewmodel.Name;
        //    dbContext.SaveChanges();
        //}



        //public void EditGroup(EditGroupViewModel viewmodel)
        //{
        //    Group group = viewmodel.GroupToEdit;
        //    group.Description = viewmodel.Description;
        //    group.Name = viewmodel.Name;
        //    //GroupRight userGroupRights = new GroupRight();
        //    ApplicationUser theUser;

        //    foreach (var user in viewmodel.CheckBoxUsers)
        //    {
        //        //GroupRight userGroupRights = new GroupRight();
        //        //userGroupRights.GroupId = groupToAdd.ID;  // group har inget ID förens denna sparas i databasen...!
        //        //userGroupRights.GroupName = groupToAdd.Name;
        //        //userGroupRights.group = groupToAdd;
        //        ApplicationUser groupUser = dbContext.Users.Where(x => x.Id == user.ID).Single();
        //        GroupRight userGroupRights = groupUser.GroupRights.Where(x => x.GroupId == group.ID).Single();
        //        //groupToAdd.Users.Add(groupUser);

        //        if (user.IsGroupAdmin == true)
        //        {
        //            userGroupRights.IsGroupAdmin = true;
        //        }

        //        if (user.CanEdit == true)
        //        {
        //            userGroupRights.IsGroupAdmin = true;
        //        }

        //        //userGroupRights.GroupId = groupToAdd.ID;
        //        theUser = groupUser;
        //        //theUser.GroupRights.Add(userGroupRights);
        //        //groupToAdd.Users.Add(theUser);
        //        //dbContext.SaveChanges();
        //    }

        //    //foreach (var document in viewmodel.CheckBoxDocuments.Where(x => x.IsChecked == true))
        //    //{
        //    //    Document groupDocument = dbContext.Documents.Where(x => x.ID == document.ID).Single();
        //    //    groupToAdd.Documents.Add(groupDocument);
        //    //}

        //    //var newGroup = new Group();
        //    ////newGroup.CreatorID = user.Id;

        //    //foreach (string member in members)
        //    //{
        //    //    if (dbContext.Users.Where(u => u.Id == member))
        //    //    {
        //    //        newGroup.Users.Add(member);
        //    //    }

        //    //}
        //    ////newGroup.Users = groupMembers;
        //    ////newGroup.Documents = documents;
        //    //newGroup.Creator = user;
        //    //newGroup.Name = name;
        //    //newGroup.Description = description;

        //    //userGroupRights.GroupId = groupToAdd.ID;


        //    //dbContext.Groups.Add(groupToAdd);
        //    dbContext.SaveChanges();
        //}

        public void CreateUser(CreateUserViewModel viewmodel, ApplicationUser creator)
        {
            var userToAdd = new ApplicationUser { UserName = viewmodel.UserName, FirstName = viewmodel.FirstName, LastName = viewmodel.LastName, Email = viewmodel.MailAdress };

            //foreach (var group in viewmodel.Groups.Where(x => x.IsChecked == true))
            //{
            //    Group userGroup = dbContext.Groups.Where(x => x.ID == group.ID).Single();
            //    userToAdd.Groups.Add(userGroup);

            //}

            //foreach (var document in viewmodel.Documents.Where(x => x.IsChecked == true))
            //{
            //    Document userDocument = dbContext.Documents.Where(x => x.ID == document.ID).Single();
            //    userToAdd.Documents.Add(userDocument);
            //}

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

        //public void CreateDocument(CreateDocumentViewModel viewmodel, ApplicationUser creator)
        //{
        //    var documentToAdd = new Document { CreatorID = creator.Id, Name = viewmodel.Name, Description = viewmodel.Description, Markdown = viewmodel.Markdown };

        //    foreach (var user in viewmodel.CheckboxUsers.Where(x => x.IsChecked == true))
        //    {
        //        ApplicationUser documentUser = dbContext.Users.Where(x => x.Id == user.ID).Single();
        //        documentToAdd.Users.Add(documentUser);

        //    }

        //    foreach (var group in viewmodel.CheckboxGroups.Where(x => x.IsChecked == true))
        //    {
        //        Group documentGroup = dbContext.Groups.Where(x => x.ID == group.ID).Single();
        //        documentToAdd.Groups.Add(documentGroup);

        //    }

        //    foreach (var tag in viewmodel.CheckboxTags.Where(x => x.IsChecked == true))
        //    {
        //        Tag documentTag = dbContext.Tags.Where(x => x.ID == tag.ID).Single();
        //        documentToAdd.Tags.Add(documentTag);

        //    }

        //    foreach (var file in viewmodel.Files)
        //    {
        //        documentToAdd.Files.Add(file);

        //    }

        //    dbContext.Documents.Add(documentToAdd);
        //    dbContext.SaveChanges();
        //}

        public Document CreateDocument2(string name, string description, string markdown, List<string> tags, List<UserListModel> users, List<GroupListModel> groups, ApplicationUser currentUser)
        {
            Document document = new Document() { Name = name, Description = description, Markdown = markdown, CreatorID = currentUser.Id, };
            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    //If tag exists (check label.lower)
                    //get tag and add to document.tags
                    if (TagExistCheck(tag))
                    {
                        //just add the tag
                        document.Tags.Add(GetTagByLabel(tag));
                    }

                    //Else create new tag, get it and add to document.tags

                    else
                    {
                        //create the tag and then add it

                        CreateTagByLabel(tag);
                        document.Tags.Add(GetTagByLabel(tag));
                    }

                }
            }

            document = AddDocumentToDb(document);

            if (users != null)
            {
                foreach (var user in users)
                {
                    var userToAdd = GetUserByID(user.ID);
                    UserDocumentRight right = new UserDocumentRight();
                    if (user.Rights == "ReadWrite")
                    {
                        right.CanWrite = true;
                    }
                    else if (user.Rights == "Read")
                    {
                        right.CanWrite = false;
                    }
                    right.DocumentId = document.ID;
                    right.UserId = userToAdd.Id;
                    userToAdd.UserDocumentRights.Add(right);
                    dbContext.Entry(userToAdd).State = EntityState.Modified;
                    dbContext.SaveChanges();
                }
            }
            

            UserDocumentRight creatorRight = new UserDocumentRight();
            creatorRight.DocumentId = document.ID;
            creatorRight.CanWrite = true;
            creatorRight.UserId = currentUser.Id;
            document.DateCreated = DateTime.Now;
            document.LastChanged = document.DateCreated;
            currentUser.UserDocumentRights.Add(creatorRight);
            dbContext.Entry(currentUser).State = EntityState.Modified;
            dbContext.SaveChanges();

            if (groups != null)
            {
                foreach (var group in groups)
                {
                    var groupToAdd = GetGroupByID(group.ID);
                    GroupDocumentRight right = new GroupDocumentRight();
                    if (group.Rights == "ReadWrite")
                    {
                        right.CanWrite = true;
                    }
                    else if (group.Rights == "Read")
                    {
                        right.CanWrite = false;
                    }
                    right.DocumentId = document.ID;
                    right.GroupId = groupToAdd.ID;
                    groupToAdd.GroupDocumentRights.Add(right);
                    dbContext.Entry(groupToAdd).State = EntityState.Modified;
                    dbContext.SaveChanges();
                }
            }
            

            dbContext.Entry(document).State = EntityState.Modified;
            //dbContext.SaveChanges();
            return document;
        }

        private Document AddDocumentToDb(Document document)
        {
            dbContext.Documents.Add(document);
            dbContext.SaveChanges();
            return document;
        }

        private Group AddGroupToDb(Group group)
        {
            dbContext.Groups.Add(group);
            dbContext.SaveChanges();
            return group;
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

            foreach (var file in fileArray)
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
                .Select(t => t.Label).ToList();
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
            document.TimeArchived = DateTime.Now;
            dbContext.Entry(dbDocument).CurrentValues.SetValues(document);
            dbContext.SaveChanges();
        }

        public void RestoreArchivedDocument(Document document)
        {
            Document dbDocument = dbContext.Documents.Where(x => x.ID == document.ID).Single();
            document.IsArchived = false;
            document.TimeArchived = null;
            dbContext.Entry(dbDocument).CurrentValues.SetValues(document);
            dbContext.SaveChanges();
        }

        public List<UserListModel> GetUsersByName(string keyword, string currentUserID)
        {
            var userRole = dbContext.Roles
                .Where(r => r.Name == "Admin").Single();

            var query = dbContext.Users
                .Where(u => u.FirstName.Contains(keyword) && u.Id != currentUserID && u.Roles.Any(r => r.RoleId != userRole.Id) || u.LastName.Contains(keyword) && u.Id != currentUserID && u.Roles.Any(r => r.RoleId != userRole.Id))
                .Select(u => new UserListModel
                {
                    FullName = u.FirstName + " " + u.LastName,
                    ID = u.Id,
                    Rights = "Read"
                })
                .ToList();
            return query;
        }

        public List<GroupListModel> GetAuthGroupsByName(string keyword, ApplicationUser currentUser)
        {
            //var query = dbContext.Groups
            //    .Where(g => g.Users.Any(u => u.Id == currentUser.Id) && g.Name.Contains(keyword) || g.Users.Any(u => u.Id == currentUser.Id) && g.Description.Contains(keyword))
            //    .Select(g => new GroupListModel
            //    {
            //        Name = g.Name,
            //        ID = g.ID,
            //        Description = g.Description,
            //        Rights = "Read",
            //        Users = g.Users.Select(u => u.FirstName + " " + u.LastName).ToList()
            //    })
            //    .ToList();
            //var query = new List<GroupListModel>();

            //var query = dbContext.Groups.ToList()
            //    .Select(g => new GroupListModel
            //    {
            //        Name = g.Name,
            //        ID = g.ID,
            //        Description = g.Description,
            //        Rights = "Read",
            //        Users = null
            //    })
            //    .ToList();

            //var query = dbContext.Groups.Where(y => userGroupRights.Any(z => z.GroupId == y.ID))

            //    .Select(g => new GroupListModel
            //    {
            //        Name = g.Name,
            //        ID = g.ID,
            //        Description = g.Description,
            //        Rights = "Read",
            //        Users = null
            //    })
            //    .ToList();
            List<Group> selectedGroups = new List<Group>();
            List<Group> groups = dbContext.Groups.Where(g => g.Name.Contains(keyword)).ToList();

            foreach (Group group in groups) //Ger tydligen inte rätt grupper
            {
                if (currentUser.UserGroupRights.Any(x => x.GroupId == group.ID))
                {
                    selectedGroups.Add(group);
                }

            }

            var query = selectedGroups.Select(g => new GroupListModel
                {
                    Name = g.Name,
                    ID = g.ID,
                    Description = g.Description,
                    Rights = "Read",
                    Users = null
                })
                .ToList();
                
            return query;
        }

        public bool TagExistCheck(string tag)
        {
            var check = dbContext.Tags
                .Any(t => t.Label.ToLower() == tag.ToLower());

            if (check)
            {
                return true;
            }
            
            else
            {
                return false;
            }
            
        }

        public Tag GetTagByLabel(string tag)
        {
            var query = dbContext.Tags
                .Where(t => t.Label.ToLower() == tag.ToLower()).Single();
            return query;
        }

        public void CreateTagByLabel(string tag)
        {
            var tagToAdd = new Tag { Label = tag };
            dbContext.Tags.Add(tagToAdd);
            dbContext.SaveChanges();
        }

        public ApplicationUser GetUserByID(string id)
        {
            var query = userStore
                .Users
                .Where(u => u.Id == id)
                .Single();
            return query;

        }

        public Group GetGroupByID(int id)
        {
            var query = dbContext.Groups
                .Where(g => g.ID == id).Single();
            return query;
        }

        public void CreateGroup(string name, string description, List<UserListModel> users, ApplicationUser currentUser)
        {
            Group group = new Group() { Name = name, Description = description, CreatorID = currentUser.Id, };

            group = AddGroupToDb(group);

            if (users != null)
            {
                foreach (var user in users)
                {
                    var userToAdd = GetUserByID(user.ID);
                    UserGroupRight right = new UserGroupRight();
                    if (user.Rights == "ReadWrite")
                    {
                        right.IsGroupAdmin = true;
                    }
                    else if (user.Rights == "Read")
                    {
                        right.IsGroupAdmin = false;
                    }

                    right.GroupId = group.ID;
                    right.UserId = userToAdd.Id;
                    userToAdd.UserGroupRights.Add(right);
                    dbContext.Entry(userToAdd).State = EntityState.Modified;
                    dbContext.SaveChanges();
                }
            }

            UserGroupRight creatorRight = new UserGroupRight();
            creatorRight.GroupId = group.ID;
            creatorRight.UserId = currentUser.Id;
            creatorRight.IsGroupAdmin = true;
            currentUser.UserGroupRights.Add(creatorRight);
            dbContext.Entry(currentUser).State = EntityState.Modified;
            dbContext.SaveChanges();

            dbContext.Entry(group).State = EntityState.Modified;
        }

        public Document GetDocumentById(int ID, ApplicationUser currentUser)
        {
            var query = dbContext.Documents.Where(d => d.ID == ID).Single();

            if (currentUser.UserDocumentRights.Any(r => r.DocumentId == ID))
            {
                return query;
            }
            else
            {
                return null;
            }
        }

        public List<UserListModel> getDocumentUserDocumentRights(int ID)
        {
            List<UserListModel> userList = new List<UserListModel>();
            var result = dbContext.UserDocumentRights.Where(r => r.DocumentId == ID).ToList();
            foreach (var right in result)
            { 
                UserListModel modelToAdd = new UserListModel() { FullName = right.user.FirstName + " " + right.user.LastName, ID = right.UserId };
                if (right.CanWrite == true)
                {
                    modelToAdd.Rights = "ReadWrite";
                }
                else if (right.CanWrite == false)
                {
                    modelToAdd.Rights = "Read";
                }
                userList.Add(modelToAdd);
            }
            return userList;
        }

        public List<GroupListModel> getDocumentGroupDocumentRights(int ID)
        {
            List<GroupListModel> userList = new List<GroupListModel>();
            var result = dbContext.GroupDocumentRights.Where(r => r.DocumentId == ID).ToList();

            foreach (var right in result)
            {
                GroupListModel modelToAdd = new GroupListModel() { Name = right.group.Name, Description = right.group.Description, ID = right.GroupId, Users = null };
                if (right.CanWrite == true)
                {
                    modelToAdd.Rights = "ReadWrite";
                }
                else if (right.CanWrite == false)
                {
                    modelToAdd.Rights = "Read";
                }
                userList.Add(modelToAdd);
            }
            return userList;
        }

        public void DeleteOldArchivedDocuments()
        {
            
            DeleteArchivedDocumentTimeSetting settings = dbContext.DeleteArchivedDocumentTimeSetting.First();
            if (settings.Activated == true)
            {
                List<Document> archivedDocuments = dbContext.Documents.Where(d => d.IsArchived == true).ToList();

                if (settings.TimeUnit == "days")
                {
                    foreach (Document document in archivedDocuments)
                    {
                        if (DateTime.Now >= document.TimeArchived.Value.AddDays(settings.TimeValue))
                        {
                            dbContext.Documents.Remove(document);
                        }

                    }
                }

                if (settings.TimeUnit == "weeks")
                {
                    foreach (Document document in archivedDocuments)
                    {
                        if (DateTime.Now >= document.TimeArchived.Value.AddDays(settings.TimeValue * 7))
                        {
                            dbContext.Documents.Remove(document);
                        }

                    }
                }

                if (settings.TimeUnit == "months")
                {
                    foreach (Document document in archivedDocuments)
                    {
                        if (DateTime.Now >= document.TimeArchived.Value.AddMonths(settings.TimeValue))
                        {
                            dbContext.Documents.Remove(document);
                        }

                    }
                }

                dbContext.SaveChanges();
            }
            
        }

        public void EditDocument(int Id, string name, string description, string markdown, List<string> tags, List<UserListModel> users, List<GroupListModel> groups, ApplicationUser applicationUser)
        {
            Document documentToChange = dbContext.Documents.Where(d => d.ID == Id).Single();
            documentToChange.LastChanged = DateTime.Now;

            if (documentToChange.Description != description)
            {
                documentToChange.Description = description;
            }

            if (documentToChange.Markdown != markdown)
            {
                documentToChange.Markdown = markdown;
            }

            if (documentToChange.Name != name)
            {
                documentToChange.Name = name;
            }

            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    //If tag exists (check label.lower)
                    //get tag and add to document.tags
                    if (TagExistCheck(tag))
                    {
                        //just add the tag
                        documentToChange.Tags.Add(GetTagByLabel(tag));
                    }

                    //Else create new tag, get it and add to document.tags

                    else
                    {
                        //create the tag and then add it

                        CreateTagByLabel(tag);
                        documentToChange.Tags.Add(GetTagByLabel(tag));
                    }

                }
            }

            if (users != null)
            {
                foreach (var user in users)
                {
                    UserDocumentRight right;

                    var userToAdd = GetUserByID(user.ID);
                    if (!dbContext.UserDocumentRights.Any(x => x.user.Id == user.ID && x.DocumentId == documentToChange.ID))
                    {
                        right = new UserDocumentRight();
                        userToAdd.UserDocumentRights.Add(right);
                    }

                    else
                    {
                        right = dbContext.UserDocumentRights.Where(x => x.user.Id == user.ID && x.DocumentId == documentToChange.ID).Single();
                    }
                    
                    if (user.Rights == "ReadWrite")
                    {
                        right.CanWrite = true;
                    }
                    else if (user.Rights == "Read")
                    {
                        right.CanWrite = false;
                    }
                    right.DocumentId = documentToChange.ID;
                    right.UserId = userToAdd.Id;
                    //if (!dbContext.UserDocumentRights.Any(r => r.DocumentId == documentToChange.ID && r.UserId == userToAdd.Id))
                    //{
                    //    dbContext.UserDocumentRights.Add(right);
                    //}
                    dbContext.SaveChanges();
                }
            }


            //UserDocumentRight creatorRight = new UserDocumentRight();
            //creatorRight.DocumentId = documentToChange.ID;
            //creatorRight.CanWrite = true;
            //creatorRight.UserId = documentToChange.Id;
            //documentToChange.UserDocumentRights.Add(creatorRight);
            dbContext.Entry(documentToChange).State = EntityState.Modified;
            dbContext.SaveChanges();

            //if (groups != null)
            //{
            //    foreach (var group in groups)
            //    {
            //        var groupToAdd = GetGroupByID(group.ID);
            //        GroupDocumentRight right = new GroupDocumentRight();
            //        if (group.Rights == "ReadWrite")
            //        {
            //            right.CanWrite = true;
            //        }
            //        else if (group.Rights == "Read")
            //        {
            //            right.CanWrite = false;
            //        }
            //        right.DocumentId = document.ID;
            //        right.GroupId = groupToAdd.ID;
            //        groupToAdd.GroupDocumentRights.Add(right);
            //        dbContext.Entry(groupToAdd).State = EntityState.Modified;
            //        dbContext.SaveChanges();
            //    }
            //}


            //dbContext.Entry(document).State = EntityState.Modified;

            //////
            
            //if (documentToChange.Name != name)
            //{
            //    documentToChange.Name = name;
            //}

            //if (documentToChange.Description != description)
            //{
            //    documentToChange.Description = description;
            //}

            //if (documentToChange.Markdown != markdown)
            //{
            //    documentToChange.Markdown = markdown;
            //}

            //foreach (var tag in tags)
            //{
            //    if (!documentToChange.Tags.Any(t=>t.Label == tag))
            //    {
            //        if(dbContext.Tags.Any(t=>t.Label == tag))
            //        {
            //            documentToChange.Tags.Add(dbContext.Tags.Where(t=>t.Label == tag).Single());
            //        }
            //        else
            //        {
            //            documentToChange.Tags.Add(new Tag() { Label = tag, ID = documentToChange.ID });
            //        }
                    
            //    }
            //}

            //foreach (var user in users)
            //{
            //    if (user.Rights=="Read")
            //    {

            //    }

            //    else if (user.Rights == "ReadWrite")
            //    {

            //    }

            //    if(!dbContext.UserDocumentRights
            //        .Any
            //        (right=>right.DocumentId == documentToChange.ID &&right.UserId == user.ID &&right.CanWrite == true))
            //    {

            //    }
            //    else if (!dbContext.UserDocumentRights
            //        .Any
            //        (right => right.DocumentId == documentToChange.ID && right.UserId == user.ID && right.CanWrite == false))
            //    {

            //    }
            //}

        }

        public DeleteArchivedDocumentTimeSetting GetDocumentDeleteTimeSettings()
        {
            return dbContext.DeleteArchivedDocumentTimeSetting.First();
        }

        public void SetDocumentDeleteTimeSettings(bool activated, int timeValue, string timeUnit)
        {
            var setting = dbContext.DeleteArchivedDocumentTimeSetting.First();
            setting.Activated = activated;
            setting.TimeValue = timeValue;
            setting.TimeUnit = timeUnit;
            dbContext.Entry(setting).State = EntityState.Modified;
            dbContext.SaveChanges();
        }
    }
}