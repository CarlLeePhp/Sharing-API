using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sharing_API.Models
{
    public class Interest
    {
        [Key]
        public int Id { get; set; }
        public int AppUserId { get; set; }

        public Category Category { get; set; }
        public int CategoryId { get; set; }
    }
}
