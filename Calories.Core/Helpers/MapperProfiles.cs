using AutoMapper;
using Calories.Core.DTOs;
using Calories.Entities.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calories.Core.Helpers
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles() 
        {
            CreateMap<Meal, MealDTO>()
                .ForMember(dto => dto.UserName, opt => opt.MapFrom(m => m.User.UserName))
                .ReverseMap()
                .ForMember(m => m.User, opt => opt.Ignore());
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<RegisterDTO, User>().ReverseMap();
            CreateMap<LoginDTO, User>().ReverseMap();
        }
    }
}
