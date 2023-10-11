using Application.Models.DTOs.CompanyDTOs;
using Application.Services.Abstract;
using AutoMapper;
using Domain.Entities.Concrete;
using Domain.Enums;
using Domain.Repositories;

namespace Application.Services.Concrete
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository companyRepository;
        private readonly IMapper mapper;

        public CompanyService(ICompanyRepository companyRepository, IMapper mapper)
        {
            this.companyRepository = companyRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Şirket oluşturabilmek için client'tan gelen modeli yakalayan method.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task CreateCompany(AddCompanyDTO model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("Model is null.");
            }

            Company company = new Company();

            mapper.Map(model, company);

            await companyRepository.CreateAsync(company);
        }

        /// <summary>
        /// Şirket detaylarını içeren bir model döndürür.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<CompanyDetailDTO> GetCompanyDetails(int id)
        {
            if (id > 0)
            {
                CompanyDetailDTO detailModel = new CompanyDetailDTO();

                Company company = await companyRepository.GetFilteredFirstOrDefaultAsync(
                    x => x,
                    x => x.Id == id
                    );

                mapper.Map(company, detailModel);

                return detailModel;
            }

            throw new ArgumentException("Id can not be zero.");
        }

        /// <summary>
        /// Şiket listesini döndürür.
        /// </summary>
        /// <returns></returns>
        public async Task<List<CompanyDTO>> GetCompanyList()
        {
            List<CompanyDTO> modelList = new List<CompanyDTO>();

            List<Company> companies = await companyRepository.GetFilteredListAsync(
                x => x,
                x => x.Id > 0
                );

            mapper.Map(companies, modelList);

            return modelList;
        }

        /// <summary>
        /// Aktif durumdaki şirketlerin listesini döndüren method.
        /// </summary>
        /// <returns></returns>
        public async Task<List<CompanyDTO>> GetActiveCompanies()
        {
            List<CompanyDTO> modelList = new List<CompanyDTO>();

            List<Company> companies = await companyRepository.GetFilteredListAsync(
               x => x,
               x => x.Id > 0
               );
            foreach ( Company company in companies)
            {
                if(company.ContratEndDate<DateTime.Now || company.ContratStartDate > DateTime.Now)
                {
                    company.Status = Status.Passive;
                }
                else
                {
                    company.Status = Status.Active;
                }
                await companyRepository.UpdateAsync(company);
            }

            List<Company> activeCompanies = await companyRepository.GetFilteredListAsync(
                x => x,
                x => x.Status == Status.Active // Sadece aktif olanları seç
            );

            mapper.Map(activeCompanies, modelList);

            return modelList;
        }

        /// <summary>
        /// Pasif durumdaki şirketlerin listesini döndüren method.
        /// </summary>
        /// <returns></returns>
        public async Task<List<CompanyDTO>> GetPassiveCompanies()
        {
            List<CompanyDTO> modelList = new List<CompanyDTO>();

            List<Company> companies = await companyRepository.GetFilteredListAsync(
              x => x,
              x => x.Id > 0
              );
            foreach (Company company in companies)
            {
                if (company.ContratEndDate < DateTime.Now || company.ContratStartDate > DateTime.Now)
                {
                    company.Status = Status.Passive;
                }
                else
                {
                    company.Status = Status.Active;
                }
                await companyRepository.UpdateAsync(company);
            }

            List<Company> passiveCompanies = await companyRepository.GetFilteredListAsync(
                x => x,
                x => x.Status == Status.Passive // Sadece pasif olanları seç
            );

            mapper.Map(passiveCompanies, modelList);

            return modelList;
        }
    }
}
