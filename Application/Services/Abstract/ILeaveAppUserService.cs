using Application.Models.DTOs.LeaveAppUserDTOs;
using Domain.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Abstract
{
    public interface ILeaveAppUserService
    {
        Task<List<LeaveAppUserDTO>> GetAllFilteredAsync(int userId);
        Task<List<LeaveAppUserDTO>> GetApprovedFilteredAsync(int userId, int leaveId);
        Task<int> GetNumberOfTotalRequestedDays(List<LeaveAppUserDTO> list);
        Task<bool> CheckLeaveStatus(List<LeaveAppUserDTO> list);
        Task<bool> CreateRequest(CreateLeaveRequestDTO model);
        Task<List<LeaveAppUserWithUserNameDTO>> GetAllApprovedByCompany(int companyId);
        Task<List<LeaveAppUserWithUserNameDTO>> GetAllDeniedByCompany(int companyId);
        Task<List<LeaveAppUserWithUserNameDTO>> GetAllWaitingByCompany(int companyId);
        Task<List<LeaveAppUserDTO>> GetAllByUser(int userId);
        Task<string> ApproveRequest(int id);
        Task<string> DeniedRequest(int id);
        Task SendEmailToEmployee(int id);
        Task SendEmailToCompanyManager(int id);
        Task<bool> DeleteLeave(int id);

    }
}
