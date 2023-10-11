using Application.Models.DTOs.AppUserDTOs;
using Application.Models.DTOs.CompanyDTOs;
using Application.Models.DTOs.DepartmentDTOs;
using Application.Models.DTOs.SiteManagerDTOs;
using Application.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteManagerController : ControllerBase
    {
        private readonly ICompanyService companyService;
        private readonly ICompanyManagerService companyManagerService;
        private readonly IAppUserService appUserService;
        private readonly IDepartmentService departmentService;

        public SiteManagerController(ICompanyService companyService, ICompanyManagerService companyManagerService, IAppUserService appUserService, IDepartmentService departmentService)
        {
            this.companyService = companyService;
            this.companyManagerService = companyManagerService;
            this.appUserService = appUserService;
            this.departmentService = departmentService;
        }

        //To List Active Companies
        [HttpGet("getcompaniesactive")]
        public async Task<IActionResult> GetActiveCompanies()
        {
            List<CompanyDTO> list;

            list = await companyService.GetActiveCompanies();

            if (!list.Any())
            {
                return NotFound();
            }
            return Ok(list);
        }

        //To List Passive Companies
        [HttpGet("getcompaniespassive")]
        public async Task<IActionResult> GetPassiveCompanies()
        {
            List<CompanyDTO> list;

            list = await companyService.GetPassiveCompanies();

            if (!list.Any())
            {
                return BadRequest();
            }
            return Ok(list);
        }

        //To Add Company
        [HttpPost("addcompany")]
        public async Task<IActionResult> AddCompany([FromBody] AddCompanyDTO model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            await companyService.CreateCompany(model);
            return Ok();
        }

        //To Add Company Manager
        [HttpPost("addcompanymanager")]
        public async Task<IActionResult> AddCompanyManager(AddEmployeeDTO model)
        {
            if(model == null)
            {
                return BadRequest();
            }
            bool control = await appUserService.CheckIfUserExists(model.Tc);  

            if (!control)
            {
                bool result = await companyManagerService.AddCompanyManager(model);
                if (result)
                {
                    await appUserService.SendEmail(model.Email);
                    return Ok(result);
                }
            }
            return BadRequest();
        }

        //To List Company Managers By Company
        [HttpGet("managersbycompany/{id}")]
        public async Task<IActionResult> GetCompanyManagersByCompany(int id)
        {
            if (id != 0)
            {
                List<UserDTO> list = await companyManagerService.GetAllByCompany(id);
                return Ok(list);
            }
            return BadRequest();
        }
    }
}
