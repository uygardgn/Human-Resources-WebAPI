using Domain.Entities.Concrete;
using Microsoft.AspNetCore.Identity;

namespace Application.Models.Utilities
{
    public class ForLogin
    {
        public static async void AddSuperUserAsync(UserManager<AppUser> userManager)
        {
            AppUser user = new AppUser
            {
                FirstName = "Gamze",
                LastName = "Altınelli",
                ImagePath = "",
                BirthDate = new DateTime(1995, 5, 5),
                PlaceOfBirth = "Bursa",
                Tc = "11111111111",
                DateOfRecruitment = new DateTime(2023, 1, 10),
                Title = "Educator",
                DepartmentId = 1,
                UserName = "gamzealtinelli",
                NormalizedUserName = "GAMZEALTINELLI",
                Email = "gamze.altınelli@bilgeadam.com",
                NormalizedEmail = "GAMZE.ALTINELLI@BILGEADAM.COM",
                Address = "İstanbul",
                PhoneNumber = "+90(505) 55 55"
            };

            var createResult = await userManager.CreateAsync(user, "Admin123.");
            if(createResult.Succeeded)
            {
                var roleResult = await userManager.AddToRoleAsync(user, "SiteManager");
                if(!roleResult.Succeeded)
                {
                    throw new Exception("Hatanız var(Role)");
                }
            }
            else
            {
                throw new Exception("Hatanız var(Create)");
            }
        }
    }
}
