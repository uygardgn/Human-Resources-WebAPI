using Domain.Entities.Abstract;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Concrete
{
    public class AppRole : IdentityRole<int>, IBaseEntity
    {
        public Status Status { get; set; }
    }
}


