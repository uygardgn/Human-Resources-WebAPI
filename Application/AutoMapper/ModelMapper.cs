using Application.Models.DTOs.AppUserDTOs;
using Application.Models.DTOs.CompanyDTOs;
using Application.Models.DTOs.DepartmentDTOs;
using Application.Models.DTOs.LeaveAppUserDTOs;
using Application.Models.DTOs.LeaveDTOs;
using Application.Models.DTOs.SignInDTOs;
using Application.Models.DTOs.SiteManagerDTOs;
using AutoMapper;
using Domain.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.AutoMapper
{
    internal class ModelMapper : Profile
    {
        public ModelMapper()
        {
            CreateMap<AppUser, UserDTO>().ReverseMap();
            CreateMap<AppUser, UserDetailDTO>().ReverseMap();
            CreateMap<AppUser, UpdateUserDTO>().ReverseMap();
            CreateMap<AppUser, ModelForUpdateDTO>().ReverseMap();
            CreateMap<Department, DepartmentDTO>().ReverseMap();
            CreateMap<Company, AddCompanyDTO>().ReverseMap();
            CreateMap<Company, CompanyDTO>().ReverseMap();
            CreateMap<Company, CompanyDetailDTO>().ReverseMap();
            CreateMap<AppUser,ResetPasswordDTO>().ReverseMap();
            CreateMap<AppUser,AddEmployeeDTO>().ReverseMap();
            CreateMap<Leave,LeaveDTO>().ReverseMap();
            CreateMap<LeaveAppUser,CreateLeaveRequestDTO>().ReverseMap();
            CreateMap<LeaveAppUser,LeaveAppUserDTO>().ReverseMap();
            CreateMap<LeaveAppUser,WaitingLeaveAppUserDTO>().ReverseMap();
            CreateMap<LeaveAppUser,LeaveAppUserWithUserNameDTO>().ReverseMap();
        }
    }
}
