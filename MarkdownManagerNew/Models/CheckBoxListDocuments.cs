using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarkdownManagerNew.Models
{
    public class CheckBoxListDocuments
    {
        public int ID { get; set; }
        public string Display { get; set; }
        public bool IsChecked { get; set; }
    }
}