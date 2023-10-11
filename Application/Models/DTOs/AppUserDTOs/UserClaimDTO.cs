using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.DTOs.SiteManagerDTOs
{
    public class UserClaimDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string ImagePath { get; set; }
    }
}
