using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calories.Core.DTOs
{
    public class LoginDTO
    {
        [Required]
        public required string EmailOrUsername { get; set; }
        [Required]
        public required string Password { get; set; }  
    }
}
