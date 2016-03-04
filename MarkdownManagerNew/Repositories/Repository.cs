using MarkdownManagerNew.Models;
using MarkdownManagerNew.Viewmodels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public ApplicationUser GetUser (string userId)
        {
            ApplicationUser query = dbContext.Users
                .Where(x => x.Id == userId).Single();

            return query;
        }

        public void CreateGroup(ApplicationUser user, string name, string description)
        {
            // ändra parameters till: ApplicationUser user, List<ApplicationUser> groupMembers, List<Document> documents, string name, string description
            var newGroup = new Group();
            //newGroup.CreatorID = user.Id;
            //newGroup.Users = groupMembers;
            //newGroup.Documents = documents;
            newGroup.Creator = user;
            newGroup.Name = name;
            newGroup.Description = description;

            dbContext.Groups.Add(newGroup);
            dbContext.SaveChanges();
        }

        public CreateGroupViewModel ListUsersToCreateGroup(CreateGroupViewModel viewmodel)
        {
            var users = userManager.Users;

            foreach (ApplicationUser user in users)
            {
                viewmodel.Users.Add(user);
            }

            foreach (Group group in dbContext.Groups)
            {
                viewmodel.Groups.Add(group);
            }

            return viewmodel;
        }

    }
}