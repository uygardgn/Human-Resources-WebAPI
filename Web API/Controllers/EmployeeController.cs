using Application.Models.DTOs.LeaveAppUserDTOs;
using Application.Models.DTOs.LeaveDTOs;
using Application.Services.Abstract;
using Application.Services.Concrete;
using AutoMapper;
using Domain.Entities.Concrete;
using Domain.Enums;
using Infrastructer.Repositories;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ILeaveService leaveService;
        private readonly ILeaveAppUserService leaveAppUserService;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;
        private readonly IEmployeeService employeeService;

        public EmployeeController(ILeaveService leaveService, ILeaveAppUserService leaveAppUserService, IMapper mapper, UserManager<AppUser> userManager, IEmployeeService employeeService)
        {
            this.leaveService = leaveService;
            this.leaveAppUserService = leaveAppUserService;
            this.mapper = mapper;
            this.userManager = userManager;
            this.employeeService = employeeService;
        }

        [HttpGet("getleavetypes/{id}")]
        public async Task<List<LeaveDTO>> GetLeaveTypes(int id)
        {
            List<LeaveDTO> leaveDTOs = new List<LeaveDTO>();
            var user = await userManager.FindByIdAsync(id.ToString());
            List<Leave> leaves;
            int experience = DateTime.Now.Year - user.DateOfRecruitment.Year;
            if (experience >= 15)
            {
                leaves = await leaveService.GetAllByGenderAsync((int)user.Gender, 4);
            }
            else if (experience >= 5 && experience < 15)
            {
                leaves = await leaveService.GetAllByGenderAsync((int)user.Gender, 3);
            }
            else if (experience >= 1 && experience < 5)
            {
                leaves = await leaveService.GetAllByGenderAsync((int)user.Gender, 2);
            }
            else
            {
                leaves = await leaveService.GetAllByGenderAsync((int)user.Gender, 1);
            }
            mapper.Map(leaves, leaveDTOs);

            for (int i = 0; i < leaveDTOs.Count; i++)
            {
                leaveDTOs[i].LeaveType = leaves[i].Type.ToString();
            }

            return leaveDTOs;
        }

        [HttpPost("createleaverequest")]
        public async Task<IActionResult> CreateLeaveRequest(CreateLeaveRequestDTO model)
        {
            var user = await userManager.FindByIdAsync(model.UserId.ToString());
            List<Leave> leaves;
            int experience = DateTime.Now.Year - user.DateOfRecruitment.Year;
            if(experience >= 15) {
                leaves= await leaveService.GetAllByGenderAsync((int)user.Gender, 4);
            }
            else if (experience >= 5 && experience < 15)
            {
                leaves = await leaveService.GetAllByGenderAsync((int)user.Gender, 3);
            }
            else if (experience>=1 && experience<5)
            {
                leaves = await leaveService.GetAllByGenderAsync((int)user.Gender, 2);
            }
            else 
            {
                leaves = await leaveService.GetAllByGenderAsync((int)user.Gender, 1);
            }
            var targetLeave = leaves.FirstOrDefault(x => x.Id == model.LeaveId);

            var leaveAppUsers = await leaveAppUserService.GetApprovedFilteredAsync(model.UserId, model.LeaveId);

            int total = await leaveAppUserService.GetNumberOfTotalRequestedDays(leaveAppUsers);

            var requests = await leaveAppUserService.GetAllFilteredAsync(model.UserId);
            bool control = await leaveAppUserService.CheckLeaveStatus(requests);

            if (control)
            {
                if (model.NumberOfRequestedDays <= (targetLeave.MaxNumberOfDays - total))
                {
                    bool isCreated = await leaveAppUserService.CreateRequest(model);

                    if (isCreated)
                    {
                        await leaveAppUserService.SendEmailToCompanyManager(model.UserId);
                        return Ok("Leave request has been created.");
                    }
                    else
                        return NotFound("An error occurred while creating.");
                }
                else
                {
                    return BadRequest($"You are not allowed to take this amount of leave. This is your maximum leave entitlement: {targetLeave.MaxNumberOfDays - total}.");
                }
            }
            return BadRequest("You currently have another leave request pending.");
        }

        [HttpGet("getleaverequests/{id}")]
        public async Task<List<LeaveAppUserDTO>> GetLeaveRequests(int id)
        {
            var result = await leaveAppUserService.GetAllByUser(id);

            return result;
        }
        [HttpDelete("deleteleave/{id}")]
        public async Task<bool> DeleteLeave(int id)
        {
            var result = await leaveAppUserService.DeleteLeave(id);
            if (result)
                return true;
            return false;
        }
        [HttpPut("endofcontract/{id}")]
        public async Task<IActionResult> EndOfContract(int id)
        {
            var result = await employeeService.EndOfContract(id);
            if (result)
                return Ok(new {message="Succeed"});

            return BadRequest(new { message = "Fail" });
        }
    }
}
