using Domain.Entities.Abstract;
using Domain.Enums;

namespace Domain.Entities.Concrete
{
    public class Leave : IBaseEntity
    {
        public int Id { get; set; }
        public int RequirementOfExperience { get; set; }
        public LeaveType Type { get; set; }
        public string Description { get; set; }
        public int MaxNumberOfDays { get; set; }
        public LeaveGender Gender { get; set; }

        public ICollection<LeaveAppUser>? LeaveAppUsers { get; set; }
        public Status Status { get; set; }
    }
}
