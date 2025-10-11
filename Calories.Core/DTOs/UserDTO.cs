using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calories.Core.DTOs
{
    public class UserDTO
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public int? ExpectedCaloriesPerDay { get; set; }
        public string? CurrentPassword { get; set; }
        public string? UpdatedPassword { get; set; }

    }
}
