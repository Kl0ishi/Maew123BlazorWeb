using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maew123.Models.Dtos
{
    public class ChangePasswordRequest
    {
        private const int MinimumLength = 8;

        [Required]
        public string oldPassword {  get; set; } = string.Empty;
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Password must have at least 8 characters with one uppercase letter, one lowercase letter, and one digit.")]
        public string newPassword {  get; set; } = string.Empty;
        [Compare("newPassword", ErrorMessage = "The passwords do not match.")]
        public string confirmNewPassword { get; set; } = string.Empty;
    }
}
