using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarkdownManagerNew.Viewmodels
{
    public class GroupListModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Rights { get; set; }
        public List<String> Users { get; set; }
    }
}