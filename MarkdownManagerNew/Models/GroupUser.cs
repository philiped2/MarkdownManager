//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Web;

//namespace MarkdownManagerNew.Models
//{
//    public class GroupUser
//    {
        

//        [Key]
//        public int GroupUserId { get; set; }

//        public ApplicationUser User { get; set; }
//        public bool CanWrite { get; set; }

//        public int ID { get; set;}

//        [ForeignKey("ID")]
//        public virtual Group group { get; set; }

//    }
//}