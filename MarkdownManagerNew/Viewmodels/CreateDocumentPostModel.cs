using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarkdownManagerNew.Viewmodels
{
    public class CreateDocumentPostModel
    {
        //$scope.document = {
        //    Name: "",
        //    Description: "",
        //    Markdown: "",
        //    Tags: [],
        //    Users: [],
        //    Groups: []
        //}

        public string Name { get; set; }
        public string Description { get; set; }
        public string Markdown { get; set; }
        public string[] Tags { get; set; }
        public ListUserViewModel[] Users { get; set; }
        public ListGroupViewModel[] Groups { get; set; }
    }
}