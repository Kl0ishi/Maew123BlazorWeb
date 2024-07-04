using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maew123.Models.Dtos
{
    public class LoginDb
    {
        public int UserId { get; set; }

        public string? Email { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? Salt { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public int? RoleId { get; set; }

        public string RoleName { get; set; } = null!;
    }
}
