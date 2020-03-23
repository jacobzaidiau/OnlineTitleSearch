using OnlineTitleSearch.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTitleSearch.IModels
{
    public interface IResult
    {
        Search Search { get; set; }
        Domain Domain { get; set; }
        [Key, Column(Order = 1)]
        int SearchId { get; set; }
        [Key, Column(Order = 2)]
        int DomainId { get; set; }
        int ResultIndex { get; set; }
    }
}
