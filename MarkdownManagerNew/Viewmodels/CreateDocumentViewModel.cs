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
            CheckboxUsers = new List<CheckBoxListUser>();
            CheckboxGroups = new List<CheckBoxListGroup>();
        }

        [Display(Name = "Beskrivning")]
        public string Description { get; set; }
        [Display(Name = "Titel")]
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Markdown")]
        public string Markdown { get; set; }

        [Display(Name="Användare")]
        public List<CheckBoxListUser> CheckboxUsers { get; set; }
        [Display(Name = "Grupper")]
        public List<CheckBoxListGroup> CheckboxGroups { get; set; }
    }
}