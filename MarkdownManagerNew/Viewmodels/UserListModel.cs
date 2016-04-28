using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarkdownManagerNew.Viewmodels
{
    public class UserListModel
    {
        public string ID { get; set; }

        public string FullName { get; set; }

        public string Rights { get; set; }
        //public bool Read { get; set; }
        //public bool ReadWrite { get; set; }
        //public bool Delete { get; set; }
    }
}