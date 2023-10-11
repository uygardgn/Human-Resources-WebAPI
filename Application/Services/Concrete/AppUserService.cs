using Application.Models.DTOs.AppUserDTOs;
using Application.Models.DTOs.SignInDTOs;
using Application.Models.DTOs.SiteManagerDTOs;
using Application.Services.Abstract;
using AutoMapper;
using Domain.Entities.Concrete;
using Domain.Enums;
using Domain.Repositories;
using Infrastructer.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

namespace Application.Services.Concrete
{
    public class AppUserService : IAppUserService
    {
        private readonly IAppUserRepository appUserRepository;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;
        private readonly IDepartmentRepository departmentRepository;
        private readonly ICompanyRepository companyRepository;

        public AppUserService(IAppUserRepository appUserRepository, IMapper mapper, UserManager<AppUser> userManager, IDepartmentRepository departmentRepository, ICompanyRepository companyRepository)
        {
            this.appUserRepository = appUserRepository;
            this.mapper = mapper;
            this.userManager = userManager;
            this.departmentRepository = departmentRepository;
            this.companyRepository = companyRepository;
        }

        /// <summary>
        /// Update Page'te görünecek olan bilgileri Clien side'a 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UpdateUserDTO> GetUpdateModel(int id)
        {
            UpdateUserDTO model = new UpdateUserDTO();
            var user = await appUserRepository.GetDefaultAsync(x => x.Id == id);
            mapper.Map(user, model);
            return model;
        }

        /// <summary>
        /// Id'sine göre ilgili User'ı döndüren method.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserDTO> GetUser(int id)
        {
            UserDTO model = new UserDTO();
            var user = await appUserRepository.GetDefaultAsync(x => x.Id == id);
            List<Department> departments = await departmentRepository.GetAllAsync();
            List<Company> companies = await companyRepository.GetAllAsync();

            mapper.Map(user, model);
            model.DepartmentName = departments.FirstOrDefault(x => x.Id == user.DepartmentId).DepartmentName;
            if (user.CompanyId==null)
            {
                return model;
            }
            model.CompanyName = companies.FirstOrDefault(x => x.Id == user.CompanyId).CompanyName;
            return model;
        }

        /// <summary>
        /// Gelen modelin içindeki Id'ye göre ilgili User'ın confirmation number'ını modeldeki ile karşılaştırır, eğer doğruysa şifresini  modeldeki şifre ile değiştirir.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> ResetPassword(ResetPasswordDTO model)
        {
            var user = await appUserRepository.GetDefaultAsync(x => x.Id == model.Id);

            bool isMatch=await userManager.CheckPasswordAsync(user, model.Password);

            if (user.ConformationNumber == model.ConformationNumber&&isMatch==false)
            {
                user.PasswordHash = userManager.PasswordHasher.HashPassword(user, model.Password);
                user.ConformationNumber = null;
                await userManager.UpdateAsync(user);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Id'ye göre ilgili User'ın detaylarını içeren modeli döndüren method.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserDetailDTO> GetUserDetail(int id)
        {
            List<Company> companies = await companyRepository.GetAllAsync();
            UserDetailDTO model = new UserDetailDTO();
            List<Department> departments = await departmentRepository.GetAllAsync();
            var user = await appUserRepository.GetDefaultAsync(x => x.Id == id);
            mapper.Map(user, model);

            model.DepartmentName = departments.FirstOrDefault(x => x.Id == user.DepartmentId).DepartmentName;

            model.Status = user.Status;
            if (user.CompanyId == null)
            {
                return model;
            }
            model.CompanyName = companies.FirstOrDefault(x => x.Id == user.CompanyId).CompanyName;
            return model;
        }

        /// <summary>
        /// Clienttan gelen Update modelin içindeki Id'ye göre ilgili User'ı yine modelin içindeki biligilere göre update eden method.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateUser(ModelForUpdateDTO model)
        {
            var user = await appUserRepository.GetDefaultAsync(x => x.Id == model.Id);
            if (user != null)
            {
                if (model.ImagePath == "none")
                {
                    model.ImagePath = user.ImagePath;
                }
                mapper.Map(model, user);
                int effectedRows = await appUserRepository.UpdateAsync(user);
                if (effectedRows > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Clienttan gelen modelin içindeki userName'e göre ilgili User'ı çeken ve eğer şifresi modeldeki şifre ile aynı ise Claim yapısı için bir UserClaimDTO döndüren method.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<UserClaimDTO> GetLoginModel(LoginDTO model)
        {
            LoginDTO loginModel = new LoginDTO();
            UserClaimDTO claimModel = new UserClaimDTO();
            var user = await appUserRepository.GetDefaultAsync(x => x.UserName == model.UserName);
            bool isMatched = await userManager.CheckPasswordAsync(user, model.Password);
            if (user != null && isMatched && user.Status == Status.Active)
            {
                claimModel.UserName = model.UserName;
                claimModel.Id = user.Id;
                claimModel.ImagePath = user.ImagePath;
                var role = await userManager.GetRolesAsync(user);
                claimModel.Role = role[0];
            }
            return claimModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> ChangePassword(int id, ChangePasswordDTO model)
        {
            var user = await appUserRepository.GetDefaultAsync(x => x.Id == id);
            if (user != null)
            {
                var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                if (result.Succeeded)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Client tarafından gelen email adresine bir şifre güncelleme linki gönderen method.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task SendEmail(string email)
        {
            var user = await appUserRepository.GetDefaultAsync(x => x.Email == email);
            if (user != null)
            {
                Random rnd = new Random();
                int conformationNumber = rnd.Next(1000, 9999);
                user.ConformationNumber = conformationNumber;
                await appUserRepository.UpdateAsync(user);

                MailMessage mail = new MailMessage();
                mail.IsBodyHtml = true;
                mail.To.Add(user.Email);
                mail.From = new MailAddress("[MAIL ADDRESS]", "Şifre Güncelleme", System.Text.Encoding.UTF8);
                mail.Subject = "Şifre Güncelleme Talebi";
                if(user.PasswordHash == null)
                {
                    mail.Body = $"<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\">\r\n        <tr>\r\n            <td style=\"text-align: center; padding: 30px;\">\r\n                <h1>Şifre Güncelleme</h1>\r\n                <p>Merhaba,</p>\r\n<p>Kullanıcı adınız: {user.UserName}</p>\r\n<p>Doğrulama Kodu: {user.ConformationNumber}</p>\r\n                <p>Şifrenizi güncellemek için aşağıdaki bağlantıyı kullanabilirsiniz:</p>\r\n                <p><a href=\"https://humanresourceswebapp.azurewebsites.net/resetpassword/{user.Id}\" target=\"_blank\" style=\"background-color: #007BFF; color: #fff; padding: 10px 20px; text-decoration: none; border-radius: 5px;\">Şifreyi Güncelle</a></p>\r\n<p>İyi günler dileriz!</p>\r\n            </td>\r\n        </tr>\r\n    </table>";
                }
                else
                {
                    mail.Body = $"<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\">\r\n        <tr>\r\n            <td style=\"text-align: center; padding: 30px;\">\r\n                <h1>Şifre Güncelleme</h1>\r\n                <p>Merhaba,</p>\r\n<p>Doğrulama Kodu: {user.ConformationNumber}</p>\r\n                <p>Şifrenizi güncellemek için aşağıdaki bağlantıyı kullanabilirsiniz:</p>\r\n                <p><a href=\"https://humanresourceswebapp.azurewebsites.net/resetpassword/{user.Id}\" target=\"_blank\" style=\"background-color: #007BFF; color: #fff; padding: 10px 20px; text-decoration: none; border-radius: 5px;\">Şifreyi Güncelle</a></p>\r\n<p>İyi günler dileriz!</p>\r\n            </td>\r\n        </tr>\r\n    </table>";
                }
                mail.IsBodyHtml = true;
                SmtpClient smp = new SmtpClient();
                smp.Credentials = new NetworkCredential("[MAIL ADDRESS]", "[KEY]");
                smp.Port = 587;
                smp.Host = "smtp.gmail.com";
                smp.EnableSsl = true;
                smp.Send(mail);
            }
        }

        public async Task<bool> CheckIfUserExists(string tc)
        {
            if(!string.IsNullOrEmpty(tc))
            {
                var test = await appUserRepository.GetFilteredFirstOrDefaultAsync(
                    x => x,
                    x => x.Tc == tc
                    );

                if (test == null)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
