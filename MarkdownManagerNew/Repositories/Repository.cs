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
            

            foreach (var user in viewmodel.CheckBoxUsers.Where(x => x.IsChecked == true))
            {
                GroupRight userGroupRights = new GroupRight();
                //userGroupRights.group = groupToAdd;
                ApplicationUser groupUser = dbContext.Users.Where(x => x.Id == user.ID).Single();
                groupToAdd.Users.Add(groupUser);

                if (user.IsGroupAdmin == true)
                {
                    userGroupRights.IsGroupAdmin = true;
                }

                if (user.CanEdit == true)
                {
                    userGroupRights.CanEdit = true;
                }

                userGroupRights.GroupId = groupToAdd.ID;
                groupUser.GroupRights.Add(userGroupRights);
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

            dbContext.Groups.Add(groupToAdd);
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

    }
}