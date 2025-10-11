using Calories.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calories.Core.Interfaces
{
    public interface IUserRepository
    {
        public Task<IEnumerable<UserDTO>> GetAllUsersAsync();
    }
}
