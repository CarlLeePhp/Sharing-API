using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sharing_API.DTOs
{
    public class SharingDto
    {
        public int Id { get; set; }
        public int Status { get; set; }
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
        public int KeepQty { get; set; }
        public string PhotoUrl { get; set; }
        public string PhotoPublicId { get; set; }

        public int AppUserId { get; set; }
        public int CategoryId { get; set; }
        public CategoryDto Category { get; set; }
    }
}
