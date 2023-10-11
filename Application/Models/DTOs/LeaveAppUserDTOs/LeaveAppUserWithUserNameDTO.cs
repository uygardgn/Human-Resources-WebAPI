using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.DTOs.LeaveAppUserDTOs
{
    public class LeaveAppUserWithUserNameDTO
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public int NumberOfRequestedDays { get; set; }
        public string LeaveStatus { get; set; }
        public string LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DateOfRequest { get; set; }
        public DateTime? DateofResponse { get; set; }
    }
}
