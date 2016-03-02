using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MarkdownManagerNew.Models
{
    public class Document
    {
        public Document()
        {
            DateCreated = DateTime.Now;
            ChangeLog = new List<string>();
            ChangeLog.Add(DateCreated + " - Dokument skapat");

            Users = new List<ApplicationUser>();

            Groups = new List<Group>();

            Tags = new List<Tag>();
        }

        //[Key]
        public int Id { get; set; }

        [Display(Name = "Beskrivning")]
        public string Description { get; set; }
        [Display(Name = "Titel")]
        public string Name { get; set; }
        [Display(Name = "Markdown")]
        public string Markdown { get; set; }

        [Display(Name = "Datum skapad")]
        public Nullable<DateTime> DateCreated { get; set; }
        [Display(Name = "Datum ändrad")]
        public Nullable<DateTime> LastChanged { get; set; }
        [Display(Name = "Ändringslogg")]
        public List<string> ChangeLog { get; set; }

        [Display(Name = "Grupper")]
        public virtual ICollection<Group> Groups { get; set; }
        [Display(Name = "Taggar")]
        public virtual ICollection<Tag> Tags { get; set; }
        [Display(Name = "Gruppmedlemmar")]
        public virtual ICollection<ApplicationUser> Users { get; set; }

        public string CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        [Display(Name = "Skapare")]
        public virtual ApplicationUser Creator { get; set; }
    }
}