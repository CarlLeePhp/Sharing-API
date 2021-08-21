using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Sharing_API.Models
{
    public class Joining
    {
        [Key]
        public int Id { get; set; }
        
        
        public int JoinQty { get; set; }
        public int Status { get; set; } // 1 - completed, 0 - not completed

        public int JoinUserId { get; set; }
        public Sharing Sharing { get; set; }
        public int SharingId { get; set; }
    }
}
