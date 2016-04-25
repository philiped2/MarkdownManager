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

            Users = new List<ApplicationUser>();

            Documents = new List<Document>();
        }

        [Key]
        public int ID { get; set; }

        [Display(Name = "Medlemmar")] //FKEY
        public virtual ICollection<ApplicationUser> Users { get; set; }

        //[Display(Name = "Gruppmedlemmar")]
        //public virtual ICollection<GroupUser> GroupUsers { get; set; }

        [Display(Name = "Dokument")] //FKEY
        public virtual ICollection<Document> Documents { get; set; }

        [Display(Name = "Beskrivning")]
        public string Description { get; set; }

        [Display(Name = "Namn")]
        public string Name { get; set; }

        //public string CreatorId { get; set; }
        [Required]
        public string CreatorID { get; set; }
        [Display(Name = "Skapare")]
        //[ForeignKey("CreatorID")]
        public ApplicationUser Creator { get; set; }

        [Display(Name = "Datum skapad")]
        public Nullable<DateTime> DateCreated { get; set; }

        [Display(Name = "Senast ändrad")]
        public Nullable<DateTime> LastChanged { get; set; }

        [Display(Name = "Ändringslogg")]
        public ICollection<string> ChangeLog { get; set; }

        public virtual List<DocumentRight> DocumentRights { get; set; }
    }
}