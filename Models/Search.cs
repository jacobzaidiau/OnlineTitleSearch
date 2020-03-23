using OnlineTitleSearch.IModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineTitleSearch.Models
{
    public class Search : ISearch
    {
        public int SearchId { get; set; }
        public DateTime SearchDate { get; set; }
    }
}