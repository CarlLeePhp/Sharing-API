using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
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
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public UserRepository(DataContext context, IMapper mapper, UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<MemberDto> GetMemberAsync(string email)
        {
            
            return await _userManager.Users
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .Where(m => m.Email == email.ToLower())
                .FirstOrDefaultAsync();
        }

        public async Task<MemberDto> GetMemberByIdAsync(int id)
        {

            return await _userManager.Users
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
            return await _context.Users
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }


        public async Task<AppUser> GetUserByEmailAsync(string email)
        {
            return await _userManager.Users
                .Include(s => s.Interests)
                .SingleOrDefaultAsync(x => x.Email == email);
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users
                .Include(s => s.Interests)
                .ToListAsync();
        }

        public void AddUser(AppUser user)
        {
            _context.Users.Add(user);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public async Task<bool> UserExist(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email.ToLower()
            );
        }
    }
}
