using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calories.Entities.Models
{
    public class Meal
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public DateOnly MealDate { get; set; }
        public TimeOnly MealTime { get; set; }
        public required string MealDescription { get; set; }
        public double MealCalories { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
