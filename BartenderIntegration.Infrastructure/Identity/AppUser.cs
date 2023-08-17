using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BartenderIntegration.Infrastructure.Identity
{
    public class AppUser : IdentityUser
    {
        public UserProfile UserProfile { get; set; }
    }

    public class UserProfile
    {
        [Key]
        public string UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public AppUser AppUser { get; set; }
    }
}
