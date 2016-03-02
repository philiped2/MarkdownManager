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
        public int Id { get; set; }
        [Display(Name = "Namn")]
        public string Filename { get; set; }
        [Display(Name = "Beskrivning")]
        public string Description { get; set; }
        [Display(Name = "Filtyp")]
        public string ContentType { get; set; }
        public byte Data { get; set; }

        //public string CreatorId { get; set; }
        //[ForeignKey("CreatorId")]
        [Display(Name = "Skapare")]
        public virtual ApplicationUser Creator { get; set; }

        //public int DocumentId { get; set; }
        //[ForeignKey("DocumentId")]
        [Display(Name = "Dokument")]
        public virtual Document Document { get; set; }
    }
}