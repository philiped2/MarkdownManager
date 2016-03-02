using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MarkdownManagerNew.Models
{
    public class Group
    {
        public Group()
        {
            DateCreated = DateTime.Now;
            ChangeLog = new List<string>();
            ChangeLog.Add(DateCreated + " - Grupp skapad");

            Users = new List<ApplicationUser>();

            Documents = new List<Document>();
        }

        [Key]
        public int Id { get; set; }

        [Display(Name = "Medlemmar")] //FKEY
        public virtual ICollection<ApplicationUser> Users { get; set; }

        [Display(Name = "Dokument")] //FKEY
        public virtual ICollection<Document> Documents { get; set; }

        [Display(Name = "Beskrivning")]
        public string Description { get; set; }

        [Display(Name = "Namn")]
        public string Name { get; set; }

        public string CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        [Display(Name = "Skapare")]
        public virtual string Creator { get; set; }

        [Display(Name = "Datum skapad")]
        public Nullable<DateTime> DateCreated { get; set; }

        [Display(Name = "Senast ändrad")]
        public Nullable<DateTime> LastChanged { get; set; }

        [Display(Name = "Ändringslogg")]
        public ICollection<string> ChangeLog { get; set; }
    }
}