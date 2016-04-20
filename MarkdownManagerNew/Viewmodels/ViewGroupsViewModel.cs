using MarkdownManagerNew.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarkdownManagerNew.Viewmodels
{
    public class ViewGroupsViewModel
    {
        public ApplicationUser CurrentUser { get; set; }
        public List<Group> UsersGroups { get; set; }
    }
}