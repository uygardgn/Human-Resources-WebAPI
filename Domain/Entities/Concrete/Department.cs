using Domain.Entities.Abstract;
using Domain.Enums;

namespace Domain.Entities.Concrete
{
    public class Department : IBaseEntity
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public Status Status { get; set; }
        public ICollection<AppUser>? Employees { get; set; }
    }
}
