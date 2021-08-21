using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Sharing_API.Models
{
    public class AppUser: IdentityUser<int>
    {
        public bool Gender { get; set; }
        
        public string Street { get; set; }
        public string City { get; set; }

        public string PhotoUrl { get; set; }
        public string PhotoPublicId { get; set; }


        public ICollection<Interest> Interests { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }

    }
}
