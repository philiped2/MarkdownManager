using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MarkdownManagerNew.Models
{
    public class GroupDocumentRight
    {
        public bool CanWrite { get; set; }
        //public bool CanDelete { get; set; }

        public int ID { get; set; }

        public int DocumentId { get; set; }
        [ForeignKey("DocumentId")]
        public virtual Document document { get; set; }


        public int GroupId { get; set; }
        [ForeignKey("GroupId")]
        public virtual Group group { get; set; }
    }
}