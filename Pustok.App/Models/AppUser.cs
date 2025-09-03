using Microsoft.AspNetCore.Identity;

namespace Pustok.App.Models
{
    public class AppUser : IdentityUser
    {
        public string Fullname { get; set; }
    }
}
