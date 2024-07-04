using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maew123.Models.Dtos
{
    public class UserLogDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}
