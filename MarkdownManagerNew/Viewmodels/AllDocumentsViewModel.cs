using MarkdownManagerNew.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarkdownManagerNew.Viewmodels
{
    public class AllDocumentsViewModel
    {
        public ApplicationUser CurrentUser { get; set; }
        public List<Document> Documents { get; set; }
        public List<int> DocumentWithEditRightsById { get; set; }
    }
}