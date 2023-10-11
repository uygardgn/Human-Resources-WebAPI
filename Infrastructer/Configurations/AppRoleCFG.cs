using Domain.Entities.Concrete;
using Domain.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructer.Configurations
{
    public class AppRoleCFG : BaseEntityCFG<AppRole>
    {
        public override void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.HasData(
               new AppRole { Id = 1, Name = "SiteManager", NormalizedName = "SITEMANAGER", Status = Status.Active },
               new AppRole { Id = 2, Name = "CompanyManager", NormalizedName = "COMPANYMANAGER", Status = Status.Active },
               new AppRole { Id = 3, Name = "Employee", NormalizedName = "EMPLOYEE", Status = Status.Active }
               );
            base.Configure(builder);
        }
    }
}
