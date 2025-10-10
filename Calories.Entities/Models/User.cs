using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calories.Entities.Models
{
    public class User : IdentityUser
    {
        public int ExpectedCaloriesPerDay { get; set; } = 2000;
        public virtual List<Meal> Meals { get; set; } = new List<Meal>();
    }
}
