using Microsoft.AspNetCore.Identity;

namespace Ab_105_Pronia.Models
{
    public class User:IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }

    }
}
