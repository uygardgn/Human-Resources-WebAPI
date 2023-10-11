using Domain.Entities.Abstract;
using Domain.Enums;

namespace Domain.Entities.Concrete
{
    public class Company : IBaseEntity
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string CompantTitle { get; set; }
        public string Mersis { get; set; }
        public string TaxNumber { get; set; }
        public string TaxOffice { get; set; }
        public string ImagePath { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public int NumberOfEmployees { get; set; }
        public DateTime DateOfEstablishment { get; set; }
        public DateTime ContratStartDate { get; set; }
        public DateTime ContratEndDate { get; set; }
        public Status Status { get; set; }

        public ICollection<AppUser>? Employees { get; set; }
    }
}
