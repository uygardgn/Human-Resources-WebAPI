using Application.Models.DTOs.AppUserDTOs;
using Application.Models.DTOs.SignInDTOs;
using Application.Models.DTOs.SiteManagerDTOs;

namespace Application.Services.Abstract
{
    public interface IAppUserService
    {
        Task<UserDTO> GetUser(int id);
        Task<UserDetailDTO> GetUserDetail(int id);
        Task<UpdateUserDTO> GetUpdateModel(int id);
        Task<bool> UpdateUser(ModelForUpdateDTO model);
        Task<UserClaimDTO> GetLoginModel(LoginDTO model);
        Task<bool> ChangePassword(int id, ChangePasswordDTO model);
        Task SendEmail(string email);
        Task<bool> ResetPassword(ResetPasswordDTO model);
        Task<bool> CheckIfUserExists(string tc);
    }
}
