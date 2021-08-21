using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Sharing_API.DTOs;
using Sharing_API.Interface;
using Sharing_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sharing_API.Data
{
    public class SharingRepository: ISharingRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public SharingRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SharingDto>> GetSharingsAsync()
        {
            return await _context.Sharings
                .Where(s => s.Status > 0)
                .ProjectTo<SharingDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IEnumerable<SharingDto>> SearchSharings(int categoryId, string keyword)
        {
            if (categoryId == 0)
            {
                return await _context.Sharings
                    .ProjectTo<SharingDto>(_mapper.ConfigurationProvider)
                    .Where(s => s.Status == 2)
                    .Where(s => s.ProductDescription.Contains(keyword)).ToArrayAsync();
            }
            return await _context.Sharings
                .ProjectTo<SharingDto>(_mapper.ConfigurationProvider)
                .Where(s => s.CategoryId == categoryId)
                .Where(s => s.Status == 2)
                .Where(s => s.ProductDescription.Contains(keyword)).ToArrayAsync();
        }
        public async Task<IEnumerable<SharingDto>> GetSharingsByStatus(int status)
        {
            return await _context.Sharings.Where(s => s.Status == status)
                .ProjectTo<SharingDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
        public async Task<SharingDto> GetSharingById(int id)
        {
            return await _context.Sharings.Where(s => s.Id == id)
                .ProjectTo<SharingDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<SharingDto>> GetSharingsByUser(int userId)
        {
            return await _context.Sharings.Where(s => s.AppUserId == userId)
                .ProjectTo<SharingDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
        public void CreateSharing(Sharing sharing)
        {
            _context.Sharings.Add(sharing);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(Sharing sharing)
        {
            _context.Entry(sharing).State = EntityState.Modified;
        }
    }
}
