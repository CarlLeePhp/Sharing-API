using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sharing_API.DTOs
{
    public class InterestDto
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public int CategoryId { get; set; }
        public CategoryDto Category { get; set; }
    }
}
