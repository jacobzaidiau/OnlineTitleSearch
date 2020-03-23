using OnlineTitleSearch.IModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineTitleSearch.Models
{
    public class Result : IResult
    {
        public Search Search { get; set; }
        public Domain Domain { get; set; }

        [Key, Column(Order = 1)]
        public int SearchId { get; set; }

        [Key, Column(Order = 2)]
        public int DomainId { get; set; }

        public int ResultIndex { get; set; }
    }
}