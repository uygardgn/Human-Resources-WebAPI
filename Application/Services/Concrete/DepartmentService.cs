using Application.Models.DTOs.DepartmentDTOs;
using Application.Services.Abstract;
using AutoMapper;
using Domain.Enums;
using Domain.Repositories;

namespace Application.Services.Concrete
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository departmentRepository;
        private readonly IMapper mapper;

        public DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            this.departmentRepository = departmentRepository;
            this.mapper = mapper;
        }

        public async Task<List<DepartmentDTO>> GetActiveDepartments()
        {
            var list = await departmentRepository.GetFilteredListAsync(
                x => x,
                x => x.Status == Status.Active
                );

            List<DepartmentDTO> dtoList = new List<DepartmentDTO>();

            mapper.Map(list, dtoList);

            return dtoList;
        }
    }
}
