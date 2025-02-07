using AutoMapper;
using Data.DTOs;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            //Mapping i entities 
            CreateMap<Expense, ExpenseDto>();
            CreateMap<ExpenseDto, Expense>();
            CreateMap<Expense, CreateExpenseDto>();
            CreateMap<CreateExpenseDto, Expense>();

            CreateMap<Category, CategoryDto>();
            //CreateMap<CategoryDto, Category>();
            //CreateMap<Category, CreateCategoryDto>();
            CreateMap<CreateCategoryDto, Category>();


            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<User, CreateUserDto>();
            CreateMap<CreateUserDto, User>();
        }
    }
}
