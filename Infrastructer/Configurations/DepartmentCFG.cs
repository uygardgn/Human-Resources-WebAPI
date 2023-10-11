using Domain.Entities.Concrete;
using Domain.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructer.Configurations
{
    public class DepartmentCFG:BaseEntityCFG<Department>
    {
        public override void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasData(
             new Department { Id=1, DepartmentName= "Supply Chain" , Status=Status.Active},   
             new Department { Id=2, DepartmentName= "IT", Status = Status.Active },
             new Department { Id=3, DepartmentName= "Human Resources", Status = Status.Active },  
             new Department { Id=4, DepartmentName= "Finance", Status = Status.Active }  
                );
            base.Configure(builder);
            
        }
    }
}
