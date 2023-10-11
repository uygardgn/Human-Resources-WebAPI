using Application.Models.DTOs.AppUserDTOs;
using Application.Models.DTOs.SiteManagerDTOs;

namespace Application.Services.Abstract
{
    public interface ICompanyManagerService
    {
        Task<bool> AddCompanyManager(AddEmployeeDTO model);
        //Task<List<UserDTO>> GetAll();
        Task<List<UserDTO>> GetAllByCompany(int id);
    }
}
