using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MarkdownManagerNew.Models;
using System.Data.Entity;

namespace MarkdownManagerNew.Viewmodels
{
    public class CreateGroupViewModel
    {
        //private ApplicationDbContext dbContext;

        public List<ApplicationUser> Users { get; set; }
        public List<Group> Groups { get; set; }


        //public CreateGroupViewModel()
        //{
        //    List<ApplicationUser> Users = dbContext.Users.ToList();
        //    List<Group> Groups = dbContext.Groups.ToList();
        //}
    }
}