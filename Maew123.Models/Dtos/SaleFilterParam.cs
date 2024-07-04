using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maew123.Models.Dtos
{
    public class SaleFilterParam
    {
        public int Currentpage { get; set; }

        public List<int> StatusIds { get; set; } = new List<int> { 1, 2, 3, 4, 5, 6 };
        public int? Year { get; set; } = DateTime.Now.Year;
        public int? Month { get; set; } = DateTime.Now.Month;

        public string? OrderBy { get; set; } 
        public string? SortDirection { get; set; }
    }
}
