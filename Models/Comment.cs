using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sharing_API.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public int SharingRate { get; set; }
        public string SharingComment { get; set; }
        public int SharerRate { get; set; }
        public string SharerComment { get; set; }
        public int JoinUserId { get; set; } // AppUserId
        public int SharingId { get; set; }
    }
}
