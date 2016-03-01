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

        public List<Document> listUserDocuments(string userid)
        {
            ApplicationUser currentuser = dbContext.Users
                .Where(x => x.Id == userid).Single();

            var documentsFromUserRights = dbContext.Documents
                .Where(d => d.Users.Contains(currentuser)).ToList();

            var documentsFromGroups = dbContext.Documents
                .TakeWhile(d => d.Groups.Any(x => currentuser.Groups.Contains(x))).ToList();

            List<Document> query = new List<Document>();

            foreach (var d in documentsFromUserRights)
            {
                query.Add(d);
            }

            foreach (var d in documentsFromGroups)
            {
                query.Add(d);
            }

            return query;

        }

        public List<Document> listAllDocuments()
        {
            List<Document> documentList = dbContext.Documents.ToList();

            return documentList;
        }

    }
}