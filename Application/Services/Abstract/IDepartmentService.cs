using Application.Models.DTOs.DepartmentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Abstract
{
    public interface IDepartmentService
    {
        Task<List<DepartmentDTO>> GetActiveDepartments();
    }
}
