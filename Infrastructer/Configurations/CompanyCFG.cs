using Domain.Entities.Concrete;
using Domain.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructer.Configurations
{
    public class CompanyCFG : BaseEntityCFG<Company>
    {
        public override void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasData(
                new Company
                {
                    Id = 1,
                    CompanyName = "Organize İşler",
                    CompantTitle = "Organize İşler Ticaret LTD.",
                    Mersis = "0123456789900015",
                    TaxNumber = "1234567899",
                    TaxOffice = "Ankara",
                    ImagePath = "ed1eb8e4-64c9-48d6-902b-ac85b967a180woman.jpg",
                    PhoneNumber = "05555555555",
                    Address = "Ankara/Çankaya",
                    Email = "orgize@gmail.com",
                    NumberOfEmployees = 4,
                    DateOfEstablishment = DateTime.Now,
                    ContratStartDate = DateTime.Now,
                    ContratEndDate = DateTime.Now.AddYears(1),
                    Status = Status.Active
                });
            base.Configure(builder);
        }
    }
}
