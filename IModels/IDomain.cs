using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OnlineTitleSearch.IModels
{
    public interface IDomain
    {
        int DomainId { get; set; }
        [Required]
        string DomainUrl { get; set; }
        string DomainTitle { get; set; }
    }
}
