using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MarkdownManagerNew.Models
{
    public class Tag
    {
        public Tag()
        {
            Documents = new List<Document>();
        }

        [Key]
        public int ID { get; set; }
        [Display(Name = "Tagg")]
        public string Label { get; set; }
        [Display(Name = "Dokument")]
        public virtual ICollection<Document> Documents { get; set; }
    }
}