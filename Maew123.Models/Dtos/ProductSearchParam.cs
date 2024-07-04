using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maew123.Models.Dtos
{
    public class ProductSearchParam
    {
        public int Currentpage { get; set; } = 1;
        public string? searchText { get; set; }
        public int? filterCata {  get; set; } = null;
        public int? filterType { get; set; } = null;
        public decimal? minPrice { get; set; }
        public decimal? maxPrice { get; set; }
    }
}
