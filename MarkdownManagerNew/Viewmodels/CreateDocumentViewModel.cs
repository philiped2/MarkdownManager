using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MarkdownManagerNew.Models;

namespace MarkdownManagerNew.Viewmodels
{
    public class CreateDocumentViewModel
    {
        public CreateDocumentViewModel()
        {
            CheckboxUsers = new List<CheckBoxListItem>();
            CheckboxGroups = new List<CheckBoxListItem>();
        }

        [Display(Name = "Beskrivning")]
        public string Description { get; set; }
        [Display(Name = "Titel")]
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Markdown")]
        public string Markdown { get; set; }

        public ICollection<CheckBoxListItem> CheckboxUsers { get; set; }
        public ICollection<CheckBoxListItem> CheckboxGroups { get; set; }
    }
}