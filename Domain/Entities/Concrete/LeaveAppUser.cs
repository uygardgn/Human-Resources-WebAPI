using Domain.Entities.Abstract;
using Domain.Enums;

namespace Domain.Entities.Concrete
{
    public class LeaveAppUser : IBaseEntity
    {
        public int Id { get; set; }
        public int NumberOfRequestedDays { get; set; }
        public LeaveStatus LeaveStatus { get; set; } = LeaveStatus.Waiting;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DateOfRequest { get; set; } = DateTime.Now;
        public DateTime? DateofResponse { get; set; }

        public int? LeaveId { get; set; }
        public Leave? Leave { get; set; }

        public int? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public Status Status { get; set; } = Status.Active;
    }
}
