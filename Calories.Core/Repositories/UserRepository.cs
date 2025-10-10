using AutoMapper;
using AutoMapper.QueryableExtensions;
using Calories.Core.DTOs;
using Calories.Core.Interfaces;
using Calories.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calories.Core.Repositories
{
    public class UserRepository(UserManager<User> userManager, IMapper mapper) : IUserRepository
    {
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<User> _userManager = userManager;
        public async Task<IEnumerable<UserDTO>> GetAllUsers()
        {
            return await _userManager.Users
                .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
