using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarkdownManagerNew.Models
{
    public class CheckBoxListItem
    {
        public string ID { get; set; }
        public string Display { get; set; }
        public bool IsChecked { get; set; }
    }
}