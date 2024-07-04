using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maew123.Models.Dtos
{
    public class UserInfoDto
    {
        public int UserId { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z0-9]{6,}$", ErrorMessage = "Username must contain at least 6 characters with at least one letter and one digit.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [RegularExpression(@"^[A-Za-zก-๙]+$", ErrorMessage = "Only Thai or English characters are allowed.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [RegularExpression(@"^[A-Za-zก-๙]+$", ErrorMessage = "Only Thai or English characters are allowed.")]
        public string? LastName { get; set; }

        //[StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Password must have at least 8 characters with one uppercase letter, one lowercase letter, and one digit.")]
        public string? Gender { get; set; }


        [StringLength(10, MinimumLength = 10)]
        public string? UserTel { get; set; }

        
        public string? UserAddress { get; set; }

        public string? InsertBy { get; set; }

        public DateTime? InsertDate { get; set; }

        public string? UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int? RoleId { get; set; }

        public string RoleName
        {
            get
            {
                return RoleId switch
                {
                    4 => "Member",
                    6 => "Employee",
                    7 => "Admin",
                    _ => "Unknown" // Default value for unknown role ids
                };
            }
        }
    }
}
