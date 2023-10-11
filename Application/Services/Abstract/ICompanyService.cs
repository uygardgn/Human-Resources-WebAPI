using Application.Models.DTOs.CompanyDTOs;
using Application.Models.DTOs.SiteManagerDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Abstract
{
    public interface ICompanyService
    {
        Task<CompanyDetailDTO> GetCompanyDetails(int id);
        Task CreateCompany(AddCompanyDTO model);
        Task<List<CompanyDTO>> GetActiveCompanies();
        Task<List<CompanyDTO>> GetPassiveCompanies();
    }
}
