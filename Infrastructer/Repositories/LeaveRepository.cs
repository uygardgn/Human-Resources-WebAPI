using Domain.Entities.Concrete;
using Domain.Repositories;
using Infrastructer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructer.Repositories
{
    public class LeaveRepository : BaseRepository<Leave>, ILeaveRepository
    {
        public LeaveRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
