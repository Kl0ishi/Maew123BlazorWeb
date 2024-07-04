using Maew123.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maew123.Models.Dtos
{
    public class UserLogDetailsDto
    {
        public List<Token> Tokens { get; set; } = new List<Token>();
        public string Username { get; set; } = string.Empty;
    }
}
