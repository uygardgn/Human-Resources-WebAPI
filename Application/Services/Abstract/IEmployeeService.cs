using Application.Models.DTOs.AppUserDTOs;
using Application.Models.DTOs.SiteManagerDTOs;

namespace Application.Services.Abstract
{
    public interface IEmployeeService
    {
        Task<bool> AddEmployee(AddEmployeeDTO model);
        Task<List<UserDTO>> GetAllByCompany(int id);
        Task<bool> EndOfContract(int id);
    }
}
