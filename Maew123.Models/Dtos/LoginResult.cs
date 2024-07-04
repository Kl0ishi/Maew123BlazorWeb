using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maew123.Models.Dtos
{
    public class LoginResult
    {
        public string? Token { get; set; }
        public DateTime Expired { get; set; }
        public bool Result { get; set; }
        public List<string>? Errors { get; set; }
    }
}
