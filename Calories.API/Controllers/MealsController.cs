using AutoMapper;
using Calories.Core.DTOs;
using Calories.Core.Interfaces;
using Calories.Entities.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Calories.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealsController(IMealRepository mealRepository, UserManager<User> userManager,
        IMapper mapper) : ControllerBase
    {
        private readonly IMealRepository _mealRepository = mealRepository;
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<User> _userManager = userManager;

        [HttpGet("admin/for-user/{userId}")] // api/<Meals>/admin/for-user/id
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<List<MealDTO>>> AdminGetAllMealsForUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return NotFound();

            var userMeals = await _mealRepository.GetUserMealsByUserIdAsync(userId);

            return Ok(userMeals);
        }

        [HttpGet("for-user")] // api/<Meals>/for-user
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<ActionResult<List<MealDTO>>> GetAllMealsForUser(
            [FromQuery]DateOnly? fromDate,
            [FromQuery]DateOnly? toDate,
            [FromQuery]TimeOnly? fromTime,
            [FromQuery]TimeOnly? toTime)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? ""; 
            var userMeals = await _mealRepository.GetUserMealsByUserIdAsync(userId);

            if(fromDate != null)
            {
                userMeals = userMeals.Where(m => m.MealDate >= fromDate);
            }
            if(toDate != null)
            {
                userMeals = userMeals.Where(m => m.MealDate <= toDate);
            }
            if(fromTime != null)
            {
                userMeals = userMeals.Where(m => m.MealTime >= fromTime);
            }
            if(toTime != null)
            {
                userMeals = userMeals.Where(m => m.MealTime <= toTime);
            }

            return Ok(userMeals);
        }

        [HttpGet("admin")] // api/<Meals>/admin
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<List<MealDTO>>> GetAllMeals()
        {
            var meals = await _mealRepository.GetAllMealsAsync();
            return Ok(meals);
        }

        [HttpGet("{id:int}", Name = "GetMealById")] // api/<Meals>/id
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User, Admin")]
        public async Task<ActionResult<MealDTO?>> GetMealById(int id)
        {
            var meal = await _mealRepository.GetMealByIdAsync(id);
            if(meal == null) return NotFound();

            if (User.IsInRole("User") && meal.UserId != User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                return Forbid();

            return Ok(meal);
        }

        [HttpPost] // api/<Meals>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<ActionResult<MealDTO>> UserCreateMeal(MealDTO mealDTO)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            if (mealDTO.MealCalories <= 0) return BadRequest("Meal can not have 0 or less calories");
            var meal = _mapper.Map<Meal>(mealDTO);
            meal.UserId = userId;
            meal.MealTime = mealDTO.MealTime ?? TimeOnly.FromDateTime(DateTime.Now);
            meal.MealDate = mealDTO.MealDate ?? DateOnly.FromDateTime(DateTime.Now);

            await _mealRepository.AddMealAsync(meal);

            var createdMealDto = _mapper.Map<MealDTO>(meal);

            if (!await _mealRepository.SaveChangesAsync()) return BadRequest("Could not create meal.");
            return CreatedAtRoute("GetMealById", new { id = createdMealDto.Id }, createdMealDto);
        }

        [HttpPost("admin/{userId}")] // api<Meals>/admin/userId
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<MealDTO>> AdminCreateMeal([FromRoute] string userId, [FromBody]MealDTO mealDto)
        {
            if (mealDto.MealCalories <= 0) return BadRequest("Meal can not have 0 or less calories");

            var meal = _mapper.Map<Meal>(mealDto);
            meal.UserId = userId;
            meal.MealTime = mealDto.MealTime ?? TimeOnly.FromDateTime(DateTime.Now);
            meal.MealDate = mealDto.MealDate ?? DateOnly.FromDateTime(DateTime.Now);

            await _mealRepository.AddMealAsync(meal);

            var createdMealDto = _mapper.Map<MealDTO>(meal);

            if (!await _mealRepository.SaveChangesAsync()) return BadRequest("Could not create meal.");
            return CreatedAtRoute("GetMealById", new { id = createdMealDto.Id }, createdMealDto);
        }

        [HttpPut("{id:int}")] // api/<Meals>/id
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User, Admin")]
        public async Task<ActionResult> EditMeal([FromRoute]int id, [FromBody]MealDTO mealDto)
        {
            var meal = await _mealRepository.GetMealEntityByIdAsync(id);

            if (meal == null) return NotFound();

            if (User.IsInRole("User") && meal.UserId != User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                return Forbid();

            if (mealDto.MealDate.HasValue) meal.MealDate = mealDto.MealDate.Value;
            if (mealDto.MealTime.HasValue) meal.MealTime = mealDto.MealTime.Value;
            if (!string.IsNullOrEmpty(mealDto.MealDescription)) meal.MealDescription = mealDto.MealDescription;
            if (mealDto.MealCalories.HasValue)
            {
                if (mealDto.MealCalories <= 0) return BadRequest("Meal cannot have 0 or less calories.");
                meal.MealCalories = mealDto.MealCalories.Value;
            }

            if (!await _mealRepository.SaveChangesAsync()) return BadRequest("Could not update meal.");

            return NoContent();
        }

        [HttpDelete("{id:int}")] // api/<Meals>/id
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User, Admin")]
        public async Task<ActionResult> DeleteMeal(int id)
        {
            var meal = await _mealRepository.GetMealByIdAsync(id);
            if(meal == null) return NotFound();

            if (User.IsInRole("User") && meal.UserId != User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                return Forbid();

            _mealRepository.DeleteMeal(_mapper.Map<Meal>(meal));
            if (!await _mealRepository.SaveChangesAsync()) return BadRequest("Could not delete meal.");

            return NoContent();
        }
    }
}
