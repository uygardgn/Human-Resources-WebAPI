using Application.Models.DTOs.SignInDTOs;
using Application.Models.DTOs.SiteManagerDTOs;
using Application.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SıgnInController : ControllerBase
    {
        private readonly IAppUserService appUserService;

        public SıgnInController(IAppUserService appUserService)
        {
            this.appUserService = appUserService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> UserLogin([FromBody] LoginDTO model)
        {
            if (model == null)
            {
                return BadRequest("An error occurred.");
            }
            UserClaimDTO claimModel = await appUserService.GetLoginModel(model);
            if (claimModel.Id == 0)
            {
                return BadRequest("User not found.");
            }
            return Ok(claimModel);
        }

        [HttpPut("changepassword/{id}")]
        public async Task<IActionResult> ChangePassword(int id, ChangePasswordDTO model)
        {
            var result = await appUserService.ChangePassword(id, model);

            if (result)
            {
                return Ok(new { Message = "Password changed successfully." });
            }

            return BadRequest(new { Message = "Password change failed." });
        }

        [HttpPost("sendmail")]
        public async Task<IActionResult> SendMail(EmailDTO model)
        {
            await appUserService.SendEmail(model.Email);
            return Ok(new { message = "Successful" });
        }

        [HttpPut("resetpassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {
            var check = await appUserService.ResetPassword(model);
            if (check)
            {
                return Ok(check);
            }
            return NotFound(check);
        }
    }
}
