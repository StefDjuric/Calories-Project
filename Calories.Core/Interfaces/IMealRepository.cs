using Calories.Core.DTOs;
using Calories.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calories.Core.Interfaces
{
    public interface IMealRepository
    {
        public Task<IEnumerable<MealDTO>> GetAllMealsAsync();
        public Task<MealDTO?> GetMealByIdAsync(int id);
        public Task<IEnumerable<MealDTO>> GetUserMealsByUserIdAsync(string userId);
        public Task AddMealAsync(Meal meal);
        public void DeleteMeal(Meal meal);
        public Task<bool> SaveChangesAsync();
        public Task<Meal?> GetMealEntityByIdAsync(int id);
    }
}
