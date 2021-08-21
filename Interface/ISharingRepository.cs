using Sharing_API.DTOs;
using Sharing_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sharing_API.Interface
{
    public interface ISharingRepository
    {
        void Update(Sharing sharing);
        void CreateSharing(Sharing sharing);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<SharingDto>> GetSharingsAsync();
        Task<IEnumerable<SharingDto>> GetSharingsByStatus(int status);
        Task<IEnumerable<SharingDto>> GetSharingsByUser(int userId);
        Task<IEnumerable<SharingDto>> SearchSharings(int categoryId, string keyword);
        Task<SharingDto> GetSharingById(int id);
    }
}
