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

        public List<ApplicationUser> ListUsersToCreateGroup()
        {
            var users = userManager.Users.ToList();

            return users;
        }

    }
}