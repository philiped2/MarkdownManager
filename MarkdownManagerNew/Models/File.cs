using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MarkdownManagerNew.Models
{
    public class File
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Namn")]
        public string Filename { get; set; }
        [Display(Name = "Beskrivning")]
        public string Description { get; set; }
        [Display(Name = "Filtyp")]
        public string ContentType { get; set; }
        public byte Data { get; set; }

        //public string CreatorId { get; set; }
        
        public string CreatorID { get; set; }
        [Required]
        [Display(Name = "Skapare")]
        [ForeignKey("CreatorID")]
        public virtual ApplicationUser Creator { get; set; }

        //public int DocumentId { get; set; }
        
        public int DocumentID { get; set; }
        [Required]
        [Display(Name = "Dokument")]
        [ForeignKey("DocumentID")]
        public virtual Document Document { get; set; }
    }
}