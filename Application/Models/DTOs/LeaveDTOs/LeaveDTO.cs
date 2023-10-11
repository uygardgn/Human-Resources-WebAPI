using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.DTOs.LeaveDTOs
{
    public class LeaveDTO
    {
        public int Id { get; set; }
        public string LeaveType { get; set; }
        public int MaxNumberOfDays { get; set; }
        public string Description { get; set; }
    }
}
