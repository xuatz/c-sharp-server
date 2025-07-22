using Microsoft.AspNetCore.Identity;

namespace CSharpServer.Models
{
    public class User : IdentityUser
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }
}