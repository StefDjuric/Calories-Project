using AutoMapper;
using Calories.Core.DTOs;
using Calories.Core.Interfaces;
using Calories.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Calories.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(UserManager<User> userManager, IMapper mapper, ITokenService tokenService) : ControllerBase
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IMapper _mapper = mapper;

        [HttpPost("register")] // api/<account>/register
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
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
            await _userManager.AddToRoleAsync(user, "User");

            user.ExpectedCaloriesPerDay = 2000;
            var userDto = _mapper.Map<UserDTO>(user);

            return CreatedAtRoute("GetUserById", new { userId = user.Id }, userDto);
        }

        [HttpPost("login")] // api/<account>/login
        public async Task<ActionResult<string>> Login(LoginDTO loginDto)
        {
            var existingUser = await _userManager.Users
                .SingleOrDefaultAsync(u => u.NormalizedEmail == loginDto.EmailOrUsername.ToUpper()
                || u.NormalizedUserName == loginDto.EmailOrUsername.ToUpper());

            if (existingUser == null) return BadRequest("Invalid credentials");

            var result = await _userManager.CheckPasswordAsync(existingUser, loginDto.Password);
            if(!result) return Unauthorized("Invalid credentials");

            var token = await _tokenService.CreateAccessTokenAsync(existingUser);

            return Ok(token);
        }
    }
}
