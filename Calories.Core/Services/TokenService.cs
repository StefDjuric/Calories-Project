using Calories.Core.Interfaces;
using Calories.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Calories.Core.Services
{
    public class TokenService(IConfiguration config, UserManager<User> userManager) : ITokenService
    {
        private readonly IConfiguration _config = config;
        private readonly UserManager<User> _userManager = userManager;

        public async Task<string> CreateAccessTokenAsync(User user)
        {
            var tokenKey = _config["JwtToken:TokenKey"] ?? throw new Exception("Token key is not found.");
            if (tokenKey.Length < 64) throw new Exception("Token key length is less than 64.");
            var accessTokenExpiryInHours = Convert.ToDouble(_config["JwtToken:AccessTokenExpiryInHours"] ?? throw new Exception("No accessTokenExpiryInHours found."));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, user.Id),
                new (ClaimTypes.Name, user.UserName),
                new (ClaimTypes.Email, user.Email)
            };

            var roles = await _userManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(accessTokenExpiryInHours),
                SigningCredentials = credentials,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
