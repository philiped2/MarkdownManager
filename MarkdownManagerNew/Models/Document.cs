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

            Users = new List<ApplicationUser>();

            Groups = new List<Group>();

            Tags = new List<Tag>();

            Files = new List<File>();
        }

        [Key]
        public int ID { get; set; }

        [Display(Name = "Beskrivning")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Display(Name = "Titel")]
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
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
        [Display(Name = "Filer")]
        public virtual ICollection<File> Files { get; set; }

        //public string CreatorId { get; set; }
        // En onödig kommentar
        [Required]
        public string CreatorID { get; set; }
        [Display(Name = "Skapare")]
        //[ForeignKey("CreatorID")]
        public virtual ApplicationUser Creator { get; set; } //Lägg till virtual. Gör det på andra creators också
    }
}