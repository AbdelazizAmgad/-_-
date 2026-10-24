using Microsoft.AspNetCore.Identity;

namespace CinemaProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}