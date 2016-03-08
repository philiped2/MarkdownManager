using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MarkdownManagerNew.Models;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace MarkdownManagerNew.Viewmodels
{
    public class CreateGroupViewModel
    {
        //private ApplicationDbContext dbContext;

        //public List<ApplicationUser> Users { get; set; }
        //public List<string> UsersToAdd {get; set;}
        [Display(Name = "Beskrivning")]
        public string Description { get; set; }
        [Display(Name = "Titel")]
        public string Name { get; set; }

        public List<CheckBoxListUser> CheckBoxUsers { get; set; }
        public List<CheckBoxListDocuments> CheckBoxDocuments { get; set; }

        public CreateGroupViewModel()
        {
            //Users = new List<ApplicationUser>();
            //UsersToAdd = new List<string>();
            CheckBoxUsers = new List<CheckBoxListUser>();
            CheckBoxDocuments = new List<CheckBoxListDocuments>();
        }
    }
}