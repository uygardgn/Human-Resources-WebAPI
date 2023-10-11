using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.DTOs.SignInDTOs
{
    public class ResetPasswordDTO
    {
        public int Id { get; set; }
        public int ConformationNumber { get; set; }
        public string Password { get; set; }
    }
}
