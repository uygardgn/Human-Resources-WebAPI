using Domain.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface ILeaveAppUserRepository : IBaseRepository<LeaveAppUser>
    {
        Task<DateTime> GetEndDate(DateTime startDate, int numberOfRequestedDays);
    }
}
