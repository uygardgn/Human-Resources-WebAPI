using Domain.Entities.Concrete;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructer.Configurations
{
    internal class LeaveCFG : IEntityTypeConfiguration<Leave>
    {
        public void Configure(EntityTypeBuilder<Leave> builder)
        {
            builder.HasData(
                new Leave
                {
                    Id = 1,
                    Type = LeaveType.AnnualLeave,
                    Description = "14 days per year for 1-5 years of experience",   
                    MaxNumberOfDays = 14,
                    Status = Status.Active,
                    Gender = LeaveGender.Unisex,
                    RequirementOfExperience=2
                    
                },
                new Leave
                {
                    Id = 2,
                    Type = LeaveType.AnnualLeave,
                    Description = "20 days per year for 5-15 years of experience",
                    MaxNumberOfDays = 20,
                    Status = Status.Active,
                    Gender = LeaveGender.Unisex,
                    RequirementOfExperience=3
                },
                new Leave
                {
                    Id = 3,
                    Type = LeaveType.AnnualLeave,
                    Description = "24 days per year for 15+ years of experience",
                    MaxNumberOfDays = 24,
                    Status = Status.Active,
                    Gender = LeaveGender.Unisex,
                    RequirementOfExperience = 4
                    
                },
                new Leave
                {
                    Id = 4,
                    Type = LeaveType.MaternityLeave,
                    Description = "8 weeks before birth and 8 weeks after birth",
                    MaxNumberOfDays = 120,
                    Status = Status.Active,
                    Gender = LeaveGender.Female,
                    RequirementOfExperience = 1
                },
                new Leave
                {
                    Id = 5,
                    Type = LeaveType.PaternityLeave,
                    Description = "5 days after birth",
                    MaxNumberOfDays = 5,
                    Status = Status.Active,
                    Gender = LeaveGender.Male,
                    RequirementOfExperience=1
                },
                new Leave
                {
                    Id = 6,
                    Type = LeaveType.MarriageLeave,
                    Description = "3 days including wedding",
                    MaxNumberOfDays = 3,
                    Status = Status.Active,
                    Gender = LeaveGender.Unisex,
                    RequirementOfExperience = 1
                },
                new Leave
                {
                    Id = 7,
                    Type = LeaveType.SicknessLeave,
                    Description = "Ask a permission to your manager",
                    MaxNumberOfDays = 365,
                    Status = Status.Active,
                    Gender = LeaveGender.Unisex,
                    RequirementOfExperience = 1
                },
                new Leave
                {
                    Id = 8,
                    Type = LeaveType.DeathLeave,
                    Description = "3 days including funeral",
                    MaxNumberOfDays = 3,
                    Status = Status.Active,
                    Gender = LeaveGender.Unisex,
                    RequirementOfExperience = 1
                }
                );
        }
    }
}
