using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sharing_API.Models
{
    public class Sharing
    {
        [Key]
        public int Id { get; set; }
        public int Status { get; set; }
        // -1: failed/deleted, 0: unsaved, 1: saved, 2: published, 3: achieved 4: completed
        public string ProductDescription { get; set; }
        public string PortionDescription { get; set; }
        public string HowToShare { get; set; }
        public double PortionPrice { get; set; }
        public int PortionQty { get; set; }
        public int AvailableQty { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime SavedDate { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime AchievedDate { get; set; }
        public int KeepQty { get; set; } // How many protions are kept for the sharing user
        public string PhotoUrl { get; set; }
        public string PhotoPublicId { get; set; }

        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
