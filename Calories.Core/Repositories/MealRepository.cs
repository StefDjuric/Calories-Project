using AutoMapper;
using AutoMapper.QueryableExtensions;
using Calories.Core.DTOs;
using Calories.Core.Interfaces;
using Calories.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calories.Core.Repositories
{
    public class MealRepository(CaloriesDbContext dbContext, IMapper mapper) : IMealRepository
    {
        private readonly CaloriesDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;

        public async Task AddMealAsync(Meal meal)
        {
            await _dbContext.Meals.AddAsync(meal);
        }

        public void DeleteMeal(Meal meal)
        {
            _dbContext.Meals.Remove(meal);
        }

        public async Task<IEnumerable<MealDTO>> GetAllMealsAsync()
        {
            return await _dbContext.Meals
                .ProjectTo<MealDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<MealDTO?> GetMealByIdAsync(int id)
        {
            return await _dbContext.Meals
                .Where(x => x.Id == id)
                .ProjectTo<MealDTO>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<MealDTO>> GetUserMealsByUserIdAsync(string userId)
        {
            return await _dbContext.Meals
                .Where(x => x.UserId == userId)
                .ProjectTo<MealDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
