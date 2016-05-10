using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarkdownManagerNew.Viewmodels
{
    public class DocumentListModel
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string DateCreated { get; set; }

        public string LastChanged { get; set; }

        public string Creator { get; set; }
    }
}