﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MarkdownManagerNew.Models
{
    public class UserGroupRight
    {
        [Key]
        public int ID { get; set; }

        //public bool CanEdit { get; set; }
        public bool IsGroupAdmin { get; set; }
        
        //public string GroupName { get; set; }

        public int GroupId { get; set; }
        [ForeignKey("GroupId")]
        public virtual Group group { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser user { get; set; }
    }
}