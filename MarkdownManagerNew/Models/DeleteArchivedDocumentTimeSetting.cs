using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MarkdownManagerNew.Models
{
    public class DeleteArchivedDocumentTimeSetting
    {
        [Key]
        public int ID { get; set; }
        public bool Activated { get; set; }
        public int TimeValue { get; set; }
        public string TimeUnit { get; set; }
    }
}