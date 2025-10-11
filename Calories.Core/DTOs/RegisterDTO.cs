using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calories.Core.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string UserName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 8)]
        public required string Password { get; set; }
        public string? Role { get; set; }
    }
}
