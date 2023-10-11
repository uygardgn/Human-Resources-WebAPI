using Application.Models.DTOs.AppUserDTOs;
using Application.Models.DTOs.SiteManagerDTOs;
using Application.Models.Utilities;
using Application.Services.Abstract;
using AutoMapper;
using Domain.Entities.Concrete;
using Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Domain.Enums;

namespace Application.Services.Concrete
{
    public class CompanyManagerService : ICompanyManagerService
    {
        private readonly IMapper mapper;
        private readonly ICompanyRepository companyRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly UserManager<AppUser> userManager;
        private readonly IAppUserRepository appUserRepository;

        public CompanyManagerService(IMapper mapper, ICompanyRepository companyRepository, IDepartmentRepository departmentRepository, UserManager<AppUser> userManager,IAppUserRepository appUserRepository)
        {
            this.mapper = mapper;
            this.companyRepository = companyRepository;
            this.departmentRepository = departmentRepository;
            this.userManager = userManager;
            this.appUserRepository = appUserRepository;
        }

        /// <summary>
        /// Gelen DTO'dan aldığı verileri kullanarak sisteme "CompanyManager" rolünde yeni bir kullanıcı ekler.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddCompanyManager(AddEmployeeDTO model)
        {
            Company company = await companyRepository.GetFilteredFirstOrDefaultAsync(
            x => x,
            x => x.Id == model.CompanyId
                );

            Department department = await departmentRepository.GetFilteredFirstOrDefaultAsync(
                x => x,
                x => x.Id == model.DepartmentId
            );

            AppUser user = new AppUser();

            mapper.Map(model, user);

            string username = $"{model.FirstName}{model.LastName}";

            user.UserName = Normalizer.convertCharacters(username).ToLower();
            user.Email = Normalizer.convertCharacters(user.Email);
            user.NormalizedUserName = Normalizer.Normalize(user.UserName);
            user.NormalizedEmail = Normalizer.Normalize(user.Email);

            var managers = await appUserRepository.GetFilteredListAsync(
                  x => x,
                  x => x.UserName == user.UserName
                  );
            if (managers.Any())
            {
                user.UserName += (managers.Count() + 1).ToString();
                user.NormalizedUserName += (managers.Count() + 1).ToString();
            }

            if (model.Gender == "Male")
                user.Gender = Gender.Male;
            else
                user.Gender = Gender.Female;

            var creationResult = await userManager.CreateAsync(user);

            if (creationResult.Succeeded)
            {
                var roleResult = await userManager.AddToRoleAsync(user, "CompanyManager");

                if (roleResult.Succeeded)
                {
                    if (user.Company == null && user.Department == null)
                    {
                        user.Company = company;
                        user.Department = department;

                        var modificationResult = await userManager.UpdateAsync(user);

                        return modificationResult.Succeeded;
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gelen companyId'ye göre o şirketteki şirket yöneticilerinin listesini dönen method.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<UserDTO>> GetAllByCompany(int id)
        {
            List<UserDTO> result = new List<UserDTO>();
            List<Company> companies = await companyRepository.GetAllAsync();
            List<Department> departments = await departmentRepository.GetAllAsync();

            var managerList = await userManager.GetUsersInRoleAsync("CompanyManager");
            foreach (AppUser appUser in managerList)
            {
                if (appUser.DateOfRecruitment > DateTime.Now || appUser.TerminationDate < DateTime.Now)
                {
                    appUser.Status = Status.Passive;
                    await userManager.UpdateAsync(appUser);
                }
            }
            var list = managerList.Where(x => x.CompanyId == id && x.Status == Status.Active).ToList();

            mapper.Map(list, result);

            for (int i = 0; i < list.Count; i++)
            {
                result[i].CompanyName = companies.Find(x => x.Id == list[i].CompanyId).CompanyName;
                result[i].DepartmentName = departments.Find(x => x.Id == list[i].DepartmentId).DepartmentName;
            }
            return result;
        }
    }
}
