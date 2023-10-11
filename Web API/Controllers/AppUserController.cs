using Application.Models.DTOs.AppUserDTOs;
using Application.Models.DTOs.SiteManagerDTOs;
using Application.Services.Abstract;
using Domain.Entities.Concrete;
using Infrastructer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        private readonly IAppUserService appUserService;
        private readonly IDepartmentService departmentService;

        public AppUserController(IAppUserService appUserService, IDepartmentService departmentService)
        {
            this.appUserService = appUserService;
            this.departmentService = departmentService;
        }

        //For User Details Page
        [HttpGet("detailspage/{id}")]
        public async Task<IActionResult> GetUserDetail(int id)
        {
            UserDetailDTO userDetailDTO = await appUserService.GetUserDetail(id);
            if (userDetailDTO == null)
            {
                return NotFound();
            }
            return Ok(userDetailDTO);
        }

        //For User MainPage
        [HttpGet("mainpage/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            UserDTO userDTO = await appUserService.GetUser(id);
            if (userDTO == null)
            {
                return NotFound();
            }
            return Ok(userDTO);
        }

        //For User Update Page
        [HttpGet("updatepage/{id}")]
        public async Task<IActionResult> GetUpdateModel(int id)
        {
            UpdateUserDTO updateUserDTO = await appUserService.GetUpdateModel(id);
            if (updateUserDTO == null)
            {
                return NotFound();
            }
            return Ok(updateUserDTO);
        }

        //To Update User
        [HttpPut("updateuser")]
        public async Task<IActionResult> UpdateUser([FromBody] ModelForUpdateDTO model)
        {
            if (model == null)
            {
                return BadRequest("Model is not valid.");
            }
            bool isSucceed = await appUserService.UpdateUser(model);
            if (isSucceed)
            {
                return Ok("User has been updated successfully");
            }
            return BadRequest("An error occurred.");
        }

        //To List Active Departments
        [HttpGet("getdepartments")]
        public async Task<IActionResult> GetActiveDepartments()
        {
            var list = await departmentService.GetActiveDepartments();
            if (!list.Any())
            {
                return BadRequest();
            }

            return Ok(list);
        }
    }
}
