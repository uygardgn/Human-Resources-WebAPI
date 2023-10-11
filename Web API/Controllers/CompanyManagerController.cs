using Application.Models.DTOs.AppUserDTOs;
using Application.Models.DTOs.SiteManagerDTOs;
using Application.Services.Abstract;
using Application.Services.Concrete;
using Domain.Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyManagerController : ControllerBase
    {
        private readonly IAppUserService appUserService;
        private readonly IEmployeeService employeeService;
        private readonly ICompanyService companyService;
        private readonly ILeaveAppUserService leaveAppUserService;
        private readonly UserManager<AppUser> userManager;

        public CompanyManagerController(IAppUserService appUserService, IEmployeeService employeeService, ICompanyService companyService, ILeaveAppUserService leaveAppUserService, UserManager<AppUser> userManager)
        {
            this.appUserService = appUserService;
            this.employeeService = employeeService;
            this.companyService = companyService;
            this.leaveAppUserService = leaveAppUserService;
            this.userManager = userManager;
        }

        [HttpGet("getpersonelsbycompany/{id}")]
        public async Task<IActionResult> GetPersonelsByCompany(int id)
        {
            if (id != 0)
            {
                List<UserDTO> list = await employeeService.GetAllByCompany(id);
                return Ok(list);
            }
            return BadRequest();
        }

        //For Company Details Page
        [HttpGet("getcompanydetails/{id}")]
        public async Task<IActionResult> GetCompanyDetails(int id)
        {
            var detailModel = await companyService.GetCompanyDetails(id);
            if (detailModel == null)
            {
                return BadRequest();
            }
            return Ok(detailModel);
        }

        [HttpPost("addemployee")]
        public async Task<IActionResult> AddEmployee(AddEmployeeDTO model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            bool control = await appUserService.CheckIfUserExists(model.Tc);

            if (!control)
            {
                bool result = await employeeService.AddEmployee(model);
                if (result)
                {
                    await appUserService.SendEmail(model.Email);
                    return Ok(result);
                }
            }
            return BadRequest();
        }

        [HttpGet("getemployeesbycompany/{id}")]
        public async Task<IActionResult> GetAllEmployeesByCompany(int id)
        {
            var list = await employeeService.GetAllByCompany(id);
            if (!list.Any())
            {
                return BadRequest();
            }
            return Ok(list);
        }

        [HttpGet("getallapprovedrequests/{id}")]
        public async Task<IActionResult> GetAllApprovedRequests(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            var result = await leaveAppUserService.GetAllApprovedByCompany((int)user.CompanyId);
            var list=result.OrderByDescending(result=>result.Id).ToList();  
            if (result.Any())
                return Ok(list);
            else 
                return BadRequest();
        }

        [HttpGet("getalldeniedrequests/{id}")]
        public async Task<IActionResult> GetAllDeniedRequests(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            var result = await leaveAppUserService.GetAllDeniedByCompany((int)user.CompanyId);
            var list = result.OrderByDescending(result => result.Id).ToList();
            if (result.Any())
                return Ok(list);
            else
                return BadRequest();
        }

        [HttpGet("getallwaitingrequests/{id}")]
        public async Task<IActionResult> GetAllWaitingRequests(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            var result = await leaveAppUserService.GetAllWaitingByCompany((int)user.CompanyId);
            var list = result.OrderByDescending(result => result.Id).ToList();
            if (result.Any())
                return Ok(list);
            else
                return BadRequest();
        }

        /// <summary>
        /// id => leaveappuserid
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpPut("approverequest/{id}")]
        public async Task<IActionResult> ApproveRequest(int id)
        {
            var result = await leaveAppUserService.ApproveRequest(id);

            if (result != null)
                return Ok(result);
            else
                return BadRequest();
        }

        [HttpPut("approvedenied/{id}")]
        public async Task<IActionResult> DeniedRequest(int id)
        {
            var result = await leaveAppUserService.DeniedRequest(id);

            if (result != null)
                return Ok(result);
            else
                return BadRequest();
        }
    }
}
