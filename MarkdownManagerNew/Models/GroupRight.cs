using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MarkdownManagerNew.Models
{
    public class GroupRight
    {
        [Key]
        public int ID { get; set; }

        //public bool CanEdit { get; set; }
        public bool IsGroupAdmin { get; set; }

        public int GroupId { get; set; }
        public string GroupName { get; set; }

        //[ForeignKey("GroupId")]
        //public virtual Group group { get; set; }
    }
}