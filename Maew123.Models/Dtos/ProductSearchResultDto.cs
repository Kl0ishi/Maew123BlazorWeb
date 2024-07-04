
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maew123.Models.Dtos
{
    //อันนี้ไม่เอาแล้ว ใช้code เก่าแทน(เอา book reserve)
    public class ProductSearchResultDto
    {
        public int CurrentPage { get; set; }

        public int PageCount { get; set; }

        public int PageSize { get; set; }

        public int StartPage { get; set; }

        public int EndPage { get; set; }

        public int TotalPages { get; set; }

        public int TotalItems { get; set; }
        public List<NewProductDto>? Products { get; set; }
        
    }
}
