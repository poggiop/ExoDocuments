using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Documents.Models
{
    public class PageViewModel
    {

        public IHtmlString Content { get; set; }

        public string ActualPath { get; set; }
    }
}