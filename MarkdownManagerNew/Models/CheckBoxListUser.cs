using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarkdownManagerNew.Models
{
    public class CheckBoxListUser
    {
        public string ID { get; set; }
        public string Display { get; set; }
        public bool IsChecked { get; set; }
        public bool UserToDelete { get; set; }

        public bool CanEdit { get; set; }
        public bool IsGroupAdmin { get; set; }

        //public int ID { get; set; }

        public Group group { get; set; }
    }
}