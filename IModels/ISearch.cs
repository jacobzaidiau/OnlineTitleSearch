using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineTitleSearch.IModels
{
    public interface ISearch
    {
        int SearchId { get; set; }
        DateTime SearchDate { get; set; }
    }
}
