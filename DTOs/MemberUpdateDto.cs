using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sharing_API.DTOs
{
    public class MemberUpdateDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public bool Gender { get; set; }
    }
}
