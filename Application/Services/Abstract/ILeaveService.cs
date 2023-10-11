using Domain.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Abstract
{
    public interface ILeaveService
    {
        Task<List<Leave>> GetAllByGenderAsync(int gender, int experience);
    }
}
