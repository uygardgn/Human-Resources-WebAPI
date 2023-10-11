namespace Application.Models.DTOs.LeaveAppUserDTOs
{
    public class CreateLeaveRequestDTO
    {
        public int LeaveId { get; set; }
        public int UserId { get; set; }
        public int NumberOfRequestedDays { get; set; }
        public DateTime StartDate { get; set; }
    }
}
