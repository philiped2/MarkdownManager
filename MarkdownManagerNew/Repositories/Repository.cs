using MarkdownManagerNew.Models;
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

        public List<Document> GetUserDocuments(ApplicationUser user)
        {
            var documentsByUserRights = user.Documents.ToList();

            List<Document> documentsByGroupRights = user.Groups.Select(g => g.Documents);

            var mergedList = documentsByUserRights.Union(new List<Document>()); //Mergea dessa
            
            List<Document> query = new List<Document>();
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

    }
}