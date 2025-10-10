using AutoMapper;
using Calories.Core.DTOs;
using Calories.Entities.Models;
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
            CreateMap<Meal, MealDTO>().ReverseMap();
            
        }
    }
}
