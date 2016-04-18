using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MarkdownManagerNew.Models;
using System.Web.Helpers;

namespace MarkdownManagerNew.Viewmodels
{
    public class CreateDocumentViewModel2
    {
        public CreateDocumentViewModel2()
        {
            Tags = new List<string>();
        }

        [Display(Name = "Beskrivning")]
        public string Description { get; set; }
        [Display(Name = "Titel")]
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Markdown")]
        public string Markdown { get; set; }
        HttpPostedFileBase FileToAdd { get; set; }

        [Display(Name = "Taggar")]
        public List<string> Tags { get; set; }
    }
}