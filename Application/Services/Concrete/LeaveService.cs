using Application.Services.Abstract;
using Domain.Entities.Concrete;
using Domain.Enums;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Concrete
{
    public class LeaveService : ILeaveService
    {
        private readonly ILeaveRepository leaveRepository;

        public LeaveService(ILeaveRepository leaveRepository)
        {
            this.leaveRepository = leaveRepository;
        }

        public async Task<List<Leave>> GetAllByGenderAsync(int gender, int experience)
        {

            var list= await leaveRepository.GetFilteredListAsync(
                x => x,
                x => x.Gender == (LeaveGender)gender || x.Gender == LeaveGender.Unisex
                );

            return list.Where(x=>x.RequirementOfExperience==(int)RequirementOfExperience.Experience1 || x.RequirementOfExperience==experience).ToList();
           
        }
    }
}
