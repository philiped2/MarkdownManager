using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MarkdownManagerNew.Models
{
    public class DocumentRight
    {
        public bool CanWrite { get; set; }
        public bool CanDelete { get; set; }

        public int ID { get; set; }
        public string DocumentName { get; set; }

        [ForeignKey("ID")]
        public virtual Document document { get; set; }
    }
}