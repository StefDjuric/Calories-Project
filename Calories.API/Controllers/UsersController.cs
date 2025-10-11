using AutoMapper;
using Calories.Core.DTOs;
using Calories.Core.Interfaces;
using Calories.Entities.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Calories.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(UserManager<User> userManager, IUserRepository userRepository,
        IMapper mapper) : ControllerBase
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;

        [HttpGet("admin/{userId}", Name = "GetUserById")] // api/<Users>/admin/id
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User Manager, Admin")]
        public async Task<ActionResult<UserDTO?>> GetUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if(user == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<UserDTO>(user));
        }

        [HttpGet("admin")] // api/<Users>/admin
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User Manager, Admin")]
        public async Task<ActionResult<List<UserDTO>>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost("admin")] // api/<Users>/admin
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User Manager, Admin")]
        public async Task<ActionResult<UserDTO>> CreateUser(RegisterDTO registerDTO)
        {
            var existingUser = await _userManager.Users.
                AnyAsync(u => u.NormalizedUserName == registerDTO.UserName.ToUpper() || u.NormalizedEmail == registerDTO.Email.ToUpper());

            if (existingUser) return BadRequest("Your information is not valid.");
            var user = _mapper.Map<User>(registerDTO);

            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest($"Could not register user: {errors}");
            }
            await _userManager.AddToRoleAsync(user, registerDTO.Role);

            user.ExpectedCaloriesPerDay = 2000;
            await _userManager.UpdateAsync(user);

            var userDto = _mapper.Map<UserDTO>(user);

            return CreatedAtRoute("GetUserById", new { userId = user.Id }, userDto);
        }

        [HttpPut("admin/{userId}")] // api/<Users>/admin/userId
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User Manager, Admin")]
        public async Task<ActionResult> AdminUpdateUser(UserDTO userDto, [FromRoute]string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return NotFound();
            
            user.UserName = userDto.UserName ?? user.UserName;
            user.Email = userDto.Email ?? user.Email;
            user.ExpectedCaloriesPerDay = userDto.ExpectedCaloriesPerDay ?? user.ExpectedCaloriesPerDay;

            if (userDto.UpdatedPassword != null)
            {
                var removeResult = await _userManager.RemovePasswordAsync(user);
                if (!removeResult.Succeeded) return BadRequest(removeResult.Errors);

                var addResult = await _userManager.AddPasswordAsync(user, userDto.UpdatedPassword);
                if(!addResult.Succeeded) return BadRequest(addResult.Errors);
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded) return BadRequest(result.Errors);

            return NoContent();
        }

        [HttpPut] // api/<Users>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<ActionResult> EditUserData(UserDTO userDto)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var user = await _userManager.FindByIdAsync(userId);

            if(user == null) return NotFound("User not found");

            user.UserName = userDto.UserName ?? user.UserName;
            user.Email = userDto.Email ?? user.Email;
            user.ExpectedCaloriesPerDay = userDto.ExpectedCaloriesPerDay ?? user.ExpectedCaloriesPerDay;

            if (userDto.CurrentPassword != null && userDto.UpdatedPassword != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, userDto.CurrentPassword, userDto.UpdatedPassword);
                if (!result.Succeeded) return BadRequest(result.Errors);
            }

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded) return BadRequest(updateResult.Errors);

            return NoContent();
        }

        [HttpDelete("admin/{userId}")] // api/<Users>/admin/userId
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User Manager, Admin")]
        public async Task<ActionResult> AdminDeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null) return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return NoContent();
        }
    }
}
