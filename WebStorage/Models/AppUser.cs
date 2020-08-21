using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace WebStorage.Models
{
    public class AppUser : IdentityUser
    {
        public ICollection<UserLink> Links { get; set; }
    }
}
