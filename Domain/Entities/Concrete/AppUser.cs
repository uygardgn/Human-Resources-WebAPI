using Domain.Entities.Abstract;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Concrete
{
    public class AppUser : IdentityUser<int>, IBaseEntity
    {
        public string FirstName { get; set; }
        public string? SecondFirstName { get; set; }
        public string LastName { get; set; }
        public string? SecondLastName { get; set; }
        public string Tc { get; set; }
        public decimal? Salary { get; set; }
        public DateTime BirthDate { get; set; }
        public string PlaceOfBirth { get; set; }
        public DateTime DateOfRecruitment { get; set; }
        public DateTime? TerminationDate { get; set; }
        public string Title { get; set; }

        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        public string Address { get; set; }
        public string ImagePath { get; set; }
        public int? ConformationNumber { get; set; }
        public Status Status { get; set; }

        public Gender? Gender { get; set; }

        public int? CompanyId { get; set; }
        public Company? Company { get; set; }

        public ICollection<LeaveAppUser>? LeaveAppUsers { get; set; }
    }
}
