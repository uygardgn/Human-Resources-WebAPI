using Domain.Entities.Concrete;
using Domain.Repositories;
using Infrastructer.Configurations;
using Infrastructer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructer.Repositories
{
    public class LeaveAppUserRepository : BaseRepository<LeaveAppUser>, ILeaveAppUserRepository
    {
        public LeaveAppUserRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<DateTime> GetEndDate(DateTime startDate, int numberOfRequestedDays)
        {
            int numberOfSundays = 0;
            while (numberOfRequestedDays > 0)
            {
                startDate = startDate.AddDays(1);
                if (startDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    numberOfSundays++;
                }
                numberOfRequestedDays--;
            }

            DateTime endDate = startDate.AddDays(numberOfRequestedDays + numberOfSundays);
            return endDate;
        }
    }
}
