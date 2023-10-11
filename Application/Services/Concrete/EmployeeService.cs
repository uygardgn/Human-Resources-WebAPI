using Application.Models.DTOs.AppUserDTOs;
using Application.Models.DTOs.SiteManagerDTOs;
using Application.Models.Utilities;
using Application.Services.Abstract;
using AutoMapper;
using Domain.Entities.Concrete;
using Domain.Enums;
using Domain.Repositories;
using Infrastructer.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.Concrete
{
    public class EmployeeService : IEmployeeService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IDepartmentRepository departmentRepository;
        private readonly ICompanyRepository companyRepository;
        private readonly IMapper mapper;
        private readonly IAppUserRepository appUserRepository;

        public EmployeeService(UserManager<AppUser> userManager, IDepartmentRepository departmentRepository, ICompanyRepository companyRepository, IMapper mapper, IAppUserRepository appUserRepository)
        {
            this.userManager = userManager;
            this.departmentRepository = departmentRepository;
            this.companyRepository = companyRepository;
            this.mapper = mapper;
            this.appUserRepository = appUserRepository;
        }

        /// <summary>
        /// Aldığı şirket Id'sine göre o şirkette çalışan ve rolü "Employee" olan tüm Userları döndürür.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<UserDTO>> GetAllByCompany(int id)
        {
            List<UserDTO> result = new List<UserDTO>();
            List<Company> companies = await companyRepository.GetAllAsync();
            List<Department> departments = await departmentRepository.GetAllAsync();

            var employeeList = await userManager.GetUsersInRoleAsync("Employee");
            foreach (AppUser appUser in employeeList)
            {
                if (appUser.DateOfRecruitment > DateTime.Now || appUser.TerminationDate < DateTime.Now)
                {
                    appUser.Status = Status.Passive;
                    await userManager.UpdateAsync(appUser);
                }
            }
            var list = employeeList.Where(x => x.CompanyId == id && x.Status == Status.Active).ToList();

            mapper.Map(list, result);

            for (int i = 0; i < list.Count; i++)
            {
                result[i].CompanyName = companies.Find(x => x.Id == list[i].CompanyId).CompanyName;
                result[i].DepartmentName = departments.Find(x => x.Id == list[i].DepartmentId).DepartmentName;
            }
            return result;
        }

        public async Task<bool> AddEmployee(AddEmployeeDTO model)
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
            var employees = await appUserRepository.GetFilteredListAsync(
               x => x,
               x => x.UserName == user.UserName
               );
            if (employees.Any())
            {
                user.UserName += (employees.Count() + 1).ToString();
                user.NormalizedUserName += (employees.Count() + 1).ToString();
            }
            var creationResult = await userManager.CreateAsync(user);

            if (creationResult.Succeeded)
            {
                var roleResult = await userManager.AddToRoleAsync(user, "Employee");

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

        public async Task<bool> EndOfContract(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            user.TerminationDate = DateTime.Now;
            user.Status = Status.Passive;
            var result = await userManager.UpdateAsync(user);
            return result.Succeeded;

        }
    }
}
