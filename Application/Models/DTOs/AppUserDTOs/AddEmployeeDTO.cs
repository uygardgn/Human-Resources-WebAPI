using Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.DTOs.AppUserDTOs
{
    public class AddEmployeeDTO
    {
        public string FirstName { get; set; }
        public string? SecondFirstName { get; set; }
        public string LastName { get; set; }
        public string? SecondLastName { get; set; }
        public string Tc { get; set; }
        public decimal Salary { get; set; }
        public DateTime BirthDate { get; set; }
        public string PlaceOfBirth { get; set; }
        public DateTime DateOfRecruitment { get; set; }
        public string Title { get; set; }
        public int DepartmentId { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ImagePath { get; set; }

        public Status Status { get; set; }

        public string Gender { get; set; }

        public int CompanyId { get; set; }
    }
}
