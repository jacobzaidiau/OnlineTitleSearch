using OnlineTitleSearch.IModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineTitleSearch.Models
{
    public class Domain : IDomain
    {
        public int DomainId { get; set; }
        [Required]
        public string DomainUrl { get; set; }
        public string DomainTitle { get; set; }
    }
}