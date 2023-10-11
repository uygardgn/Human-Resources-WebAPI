using Application.Models.DTOs.LeaveAppUserDTOs;
using Application.Services.Abstract;
using AutoMapper;
using Domain.Entities.Concrete;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Concrete
{
    public class LeaveAppUserService : ILeaveAppUserService
    {
        private readonly ILeaveAppUserRepository leaveAppUserRepository;
        private readonly IMapper mapper;
        private readonly ILeaveRepository leaveRepository;
        private readonly UserManager<AppUser> userManager;
        private readonly IAppUserRepository appUserRepository;

        public LeaveAppUserService(ILeaveAppUserRepository leaveAppUserRepository, IMapper mapper, ILeaveRepository leaveRepository, UserManager<AppUser> userManager, IAppUserRepository appUserRepository)
        {
            this.leaveAppUserRepository = leaveAppUserRepository;
            this.mapper = mapper;
            this.leaveRepository = leaveRepository;
            this.userManager = userManager;
            this.appUserRepository = appUserRepository;
        }

        public async Task<List<LeaveAppUserDTO>> GetApprovedFilteredAsync(int userId, int leaveId)
        {
            List<LeaveAppUserDTO> result = new List<LeaveAppUserDTO>();

            var list = await leaveAppUserRepository.GetFilteredListAsync(
                x => x,
                x => x.AppUserId == userId && x.LeaveId == leaveId && x.LeaveStatus == LeaveStatus.Approved
                );

            mapper.Map(list, result);

            return result;
        }

        public async Task<int> GetNumberOfTotalRequestedDays(List<LeaveAppUserDTO> list)
        {
            int result = 0;
            foreach (var item in list)
            {
                result += item.NumberOfRequestedDays;
            }
            return result;
        }

        public async Task<List<LeaveAppUserDTO>> GetAllFilteredAsync(int userId)
        {
            List<LeaveAppUserDTO> result = new List<LeaveAppUserDTO>();

            var list = await leaveAppUserRepository.GetFilteredListAsync(
                x => x,
                x => x.AppUserId == userId
                );

            mapper.Map(list, result);

            return result;
        }

        public async Task<List<LeaveAppUserDTO>> GetAllByUser(int userId)
        {
            List<LeaveAppUserDTO> result = new List<LeaveAppUserDTO>();

            var requests = await leaveAppUserRepository.GetFilteredListAsync(
                x => x,
                x => x.AppUserId == userId
                );
            mapper.Map(requests, result);
            //for(int i=0; i<requests.Count; i++) 
            //{
            //    result[i].Id = requests[i].Id;
            //    result[i].NumberOfRequestedDays = requests[i].NumberOfRequestedDays;
            //    result[i].StartDate = requests[i].StartDate;
            //    result[i].EndDate = requests[i].EndDate;
            //    result[i].DateOfRequest = requests[i].DateOfRequest;
            //    result[i].DateofResponse =(DateTime)requests[i].DateofResponse;

            //}
            var leaves = await leaveRepository.GetAllAsync();

            for (int i = 0; i < result.Count; i++)
            {
                result[i].LeaveStatus = requests[i].LeaveStatus.ToString();

                result[i].LeaveType = leaves.FirstOrDefault(x => x.Id == requests[i].LeaveId).Type.ToString();
            }

            return result;
        }

        public async Task<bool> CheckLeaveStatus(List<LeaveAppUserDTO> list)
        {
            foreach (var item in list)
            {
                if (item.LeaveStatus == LeaveStatus.Waiting.ToString())
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> DeleteLeave(int id)
        {
            var user = await leaveAppUserRepository.GetFilteredFirstOrDefaultAsync(
                x => x,
                x => x.Id == id
                );
            if (user != null)
            {
                bool control = await leaveAppUserRepository.DeleteAsync(user);

                return control;
            }
            return false;
        }

        public async Task<bool> CreateRequest(CreateLeaveRequestDTO model)
        {
            LeaveAppUser leaveRequest = new LeaveAppUser();

            Leave leave = await leaveRepository.GetFilteredFirstOrDefaultAsync(
                x => x,
                x => x.Id == model.LeaveId
                );

            AppUser user = await userManager.FindByIdAsync(model.UserId.ToString());

            mapper.Map(model, leaveRequest);
            leaveRequest.Leave = leave;
            leaveRequest.AppUser = user;
            leaveRequest.LeaveId = leave.Id;
            leaveRequest.AppUserId = user.Id;
            leaveRequest.EndDate = await leaveAppUserRepository.GetEndDate(leaveRequest.StartDate, leaveRequest.NumberOfRequestedDays);

            bool isSucceed1 = await leaveAppUserRepository.CreateAsync(leaveRequest);

            if (isSucceed1)
            {
                user.LeaveAppUsers.Add(leaveRequest);
                var isSucceed2 = await userManager.UpdateAsync(user);
                if (isSucceed2.Succeeded)
                {
                    leave.LeaveAppUsers.Add(leaveRequest);
                    var isSucceed3 = await leaveRepository.UpdateAsync(leave);
                    if (isSucceed3 > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public async Task<List<LeaveAppUserWithUserNameDTO>> GetAllApprovedByCompany(int companyId)
        {
            List<LeaveAppUser> leaveRequestList = new List<LeaveAppUser>();
            List<LeaveAppUserWithUserNameDTO> result = new List<LeaveAppUserWithUserNameDTO>();


            List<AppUser> userList = await appUserRepository.GetFilteredListAsync(
                x => x,
                x => x.CompanyId == companyId,
                include: query => query.Include(x => x.LeaveAppUsers)
                );

            var leaves = await leaveRepository.GetAllAsync();

            for (int i = 0; i < userList.Count; i++)
            {
                LeaveAppUserWithUserNameDTO request = new LeaveAppUserWithUserNameDTO();
                var requests = await leaveAppUserRepository.GetFilteredListAsync(
                x => x,
                x => x.AppUserId == userList[i].Id && x.LeaveStatus == LeaveStatus.Approved
                );
                if (requests.Any())
                {
                    for (int j = 0; j < requests.Count; j++)
                    {
                        mapper.Map(requests[j], request);

                        request.LeaveStatus = requests[j].LeaveStatus.ToString();

                        request.LeaveType = leaves.FirstOrDefault(x => x.Id == requests[j].LeaveId).Type.ToString();
                        var employee = userList.Where(x => x.LeaveAppUsers.Any(y => y.Id == requests[j].Id)).FirstOrDefault();
                        request.EmployeeName = employee.FirstName + " " + employee.LastName;
                        result.Add(request);
                    }
                }
            }
            return result;
        }
        public async Task<List<LeaveAppUserWithUserNameDTO>> GetAllDeniedByCompany(int companyId)
        {
            List<LeaveAppUser> leaveRequestList = new List<LeaveAppUser>();
            List<LeaveAppUserWithUserNameDTO> result = new List<LeaveAppUserWithUserNameDTO>();


            List<AppUser> userList = await appUserRepository.GetFilteredListAsync(
                x => x,
                x => x.CompanyId == companyId,
                include: query => query.Include(x => x.LeaveAppUsers)
                );

            var leaves = await leaveRepository.GetAllAsync();

            for (int i = 0; i < userList.Count; i++)
            {
                LeaveAppUserWithUserNameDTO request = new LeaveAppUserWithUserNameDTO();
                var requests = await leaveAppUserRepository.GetFilteredListAsync(
                x => x,
                x => x.AppUserId == userList[i].Id && x.LeaveStatus == LeaveStatus.Denied
                );
                if (requests.Any())
                {
                    for (int j = 0; j < requests.Count; j++)
                    {
                        mapper.Map(requests[j], request);

                        request.LeaveStatus = requests[j].LeaveStatus.ToString();

                        request.LeaveType = leaves.FirstOrDefault(x => x.Id == requests[j].LeaveId).Type.ToString();
                        var employee = userList.Where(x => x.LeaveAppUsers.Any(y => y.Id == requests[j].Id)).FirstOrDefault();
                        request.EmployeeName = employee.FirstName + " " + employee.LastName;
                        result.Add(request);
                    }
                }
            }
            return result;
        }

        public async Task<List<LeaveAppUserWithUserNameDTO>> GetAllWaitingByCompany(int companyId)
        {
            List<LeaveAppUser> leaveRequestList = new List<LeaveAppUser>();
            List<LeaveAppUserWithUserNameDTO> result = new List<LeaveAppUserWithUserNameDTO>();

            List<AppUser> userList = await appUserRepository.GetFilteredListAsync(
                x => x,
                x => x.CompanyId == companyId,
                include: query => query.Include(x => x.LeaveAppUsers)
                );

            var leaves = await leaveRepository.GetAllAsync();

            for (int i = 0; i < userList.Count; i++)
            {
                LeaveAppUserWithUserNameDTO request = new LeaveAppUserWithUserNameDTO();
                var requests = await leaveAppUserRepository.GetFilteredListAsync(
                x => x,
                x => x.AppUserId == userList[i].Id && x.LeaveStatus == LeaveStatus.Waiting
                );
                if (requests.Any())
                {
                    for (int j = 0; j < requests.Count; j++)
                    {
                        mapper.Map(requests[j], request);

                        request.LeaveStatus = requests[j].LeaveStatus.ToString();

                        request.LeaveType = leaves.FirstOrDefault(x => x.Id == requests[j].LeaveId).Type.ToString();
                        var employee = userList.Where(x => x.LeaveAppUsers.Any(y => y.Id == requests[j].Id)).FirstOrDefault();
                        request.EmployeeName = employee.FirstName + " " + employee.LastName;
                        result.Add(request);
                    }
                }
            }
            return result;
        }

        public async Task<string> ApproveRequest(int id)
        {
            var request = await leaveAppUserRepository.GetFilteredFirstOrDefaultAsync(
                  x => x,
                  x => x.Id == id
                  );

            request.LeaveStatus = LeaveStatus.Approved;
            request.DateofResponse = DateTime.Now;
            int effectedRows = await leaveAppUserRepository.UpdateAsync(request);
            if (effectedRows > 0)
            {
                await SendEmailToEmployee((int)request.AppUserId);
                return "Request approval has been succeeded.";
            }
            else
            {
                return "Request approval has been failed.";
            }
        }

        public async Task<string> DeniedRequest(int id)
        {
            var request = await leaveAppUserRepository.GetFilteredFirstOrDefaultAsync(
                  x => x,
                  x => x.Id == id
                  );

            request.LeaveStatus = LeaveStatus.Denied;
            request.DateofResponse = DateTime.Now;
            int effectedRows = await leaveAppUserRepository.UpdateAsync(request);
            if (effectedRows > 0)
            {
                await SendEmailToEmployee((int)request.AppUserId);
                return "Request denied has been succeeded.";
            }
            else
            {
                return "Request denied has been failed.";
            }
        }

        public async Task SendEmailToEmployee(int id)
        {
            var user = await appUserRepository.GetDefaultAsync(x => x.Id == id);
            var requests = await leaveAppUserRepository.GetFilteredListAsync(
                   x => x,
                   x => x.AppUserId == id,
                   x => x.OrderByDescending(x => x.Id)
                   );
            if (user != null)
            {

                MailMessage mail = new MailMessage();
                mail.IsBodyHtml = true;
                mail.To.Add(user.Email);
                mail.From = new MailAddress("ofluerdemoflu@gmail.com", "İzin Talebi", System.Text.Encoding.UTF8);
                mail.Subject = "İzin Talebi";


                if (requests[0].LeaveStatus == LeaveStatus.Approved)
                {

                    mail.Body = $"<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\">\r\n" +
                    "    <tr>\r\n" +
                    "        <td style=\"text-align: center; padding: 30px;\">\r\n" +
                    "            <h1>İzin Onaylandı</h1>\r\n" +
                    "            <p>Merhaba,</p>\r\n" +
                    $"            <p>İzniniz başarıyla onaylandı.</p>\r\n" +
                    "            <p>İyi günler dileriz!</p>\r\n" +
                    "        </td>\r\n" +
                    "    </tr>\r\n" +
                    "</table>";
                }
                else
                {
                    mail.Body = $"<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\">\r\n" +
                      "    <tr>\r\n" +
                      "        <td style=\"text-align: center; padding: 30px;\">\r\n" +
                      "            <h1>İzin Reddedildi</h1>\r\n" +
                      "            <p>Merhaba,</p>\r\n" +
                      $"            <p>Üzgünüz, izniniz reddedildi.</p>\r\n" +
                      "            <p>İyi günler dileriz!</p>\r\n" +
                      "        </td>\r\n" +
                      "    </tr>\r\n" +
                      "</table>";
                }

                mail.IsBodyHtml = true;
                SmtpClient smp = new SmtpClient();
                smp.Credentials = new NetworkCredential("ofluerdemoflu@gmail.com", "klxriioqbobpnyel");
                smp.Port = 587;
                smp.Host = "smtp.gmail.com";
                smp.EnableSsl = true;
                smp.Send(mail);

            }
        }

        public async Task SendEmailToCompanyManager(int id)
        {
            var user = await appUserRepository.GetDefaultAsync(x => x.Id == id);
            var allManagers = await userManager.GetUsersInRoleAsync("CompanyManager");
            var manager = allManagers.Where(x => x.CompanyId == user.CompanyId).ToList();
            var requests = await leaveAppUserRepository.GetFilteredListAsync(
                x => x,
                x => x.AppUserId == user.Id,
                x => x.OrderByDescending(x => x.Id)
                );
            if (requests.Any())
            {
                foreach (var item in manager)
                {
                    MailMessage mail = new MailMessage();
                    mail.IsBodyHtml = true;
                    mail.To.Add(item.Email);
                    mail.From = new MailAddress("ofluerdemoflu@gmail.com", "İzin Talebi", System.Text.Encoding.UTF8);
                    mail.Subject = "İzin Talebi";

                    mail.Body = $"<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\">\r\n" +
                     "    <tr>\r\n" +
                     "        <td style=\"text-align: center; padding: 30px;\">\r\n" +
                     "            <h1>İzin Talebi Oluşturuldu</h1>\r\n" +
                     "            <p>Merhaba,</p>\r\n" +
                     "            <p>Personel tarafından bir izin talebi oluşturuldu.</p>\r\n" +
                     $"            <p>Talep Eden Personel: {user.FirstName + " " + user.LastName}</p>\r\n" +
                     $"            <p>Talep Tarihi: {requests[0].DateOfRequest}</p>\r\n" +
                     $"            <p>İzin Başlangıç Tarihi: {requests[0].StartDate}</p>\r\n" +
                     $"            <p>İzin Bitiş Tarihi: {requests[0].EndDate}</p>\r\n" +
                     "            <p>İyi günler dileriz!</p>\r\n" +
                     "        </td>\r\n" +
                     "    </tr>\r\n" +
                     "</table>";

                    mail.IsBodyHtml = true;
                    SmtpClient smp = new SmtpClient();
                    smp.Credentials = new NetworkCredential("ofluerdemoflu@gmail.com", "klxriioqbobpnyel");
                    smp.Port = 587;
                    smp.Host = "smtp.gmail.com";
                    smp.EnableSsl = true;
                    smp.Send(mail);
                }
            }
        }
    }
}


