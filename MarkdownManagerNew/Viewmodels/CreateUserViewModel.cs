using MarkdownManagerNew.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MarkdownManagerNew.Viewmodels
{
    public class CreateUserViewModel
    {
        public CreateUserViewModel()
        {
            Documents = new List<CheckBoxListDocuments>();
            Groups = new List<CheckBoxListGroup>();
        }

        [Display(Name = "Administratörskonto")]
        public bool admin { get; set; }

        [Display(Name = "Användarnamn")]
        public string UserName { get; set; }

        [Display(Name = "Epost")]
        public string MailAdress { get; set; }

        [Required]
        [Display(Name = "Förnamn")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Efternamn")]
        public string LastName { get; set; }

        [Display(Name = "Dokument")]
        public virtual List<CheckBoxListDocuments> Documents { get; set; }

        [Display(Name = "Grupper")]
        public virtual List<CheckBoxListGroup> Groups { get; set; }


    }
}