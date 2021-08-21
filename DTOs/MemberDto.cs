using System.Collections.Generic;

namespace Sharing_API.DTOs
{
    public class MemberDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public bool Gender { get; set; }
        public string PhotoUrl { get; set; }
        public string PhotoPublicId { get; set; }

        public ICollection<InterestDto> Interests { get; set; }
    }
}
