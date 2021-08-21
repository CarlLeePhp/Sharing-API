using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sharing_API.Data;
using Sharing_API.DTOs;
using Sharing_API.Interface;
using Sharing_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sharing_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JoiningsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserRepository _userRepository;
        public JoiningsController(DataContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }
        [HttpGet("user/{'email'}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Joining>>> GetJoiningsByUser(string email)
        {
            var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepository.GetUserByEmailAsync(userEmail);
            if (user == null)
            {
                return BadRequest("Invailied user");
            }
            var joinings = await _context.Joinings.Where(j => j.JoinUserId == user.Id)
                .Include(j => j.Sharing).ToArrayAsync();
            return Ok(joinings);
        }


        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateJoining(Joining joining)
        {
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return BadRequest("Invailied user");
            }

            joining.JoinUserId = user.Id;
            _context.Joinings.Add(joining);
            // Modify available qty in sharing
            // update status if achieved
            Sharing sharing = await _context.Sharings.FindAsync(joining.SharingId);
            if (joining.JoinQty > sharing.AvailableQty) return BadRequest("More than available");
            sharing.AvailableQty -= joining.JoinQty;
            if (sharing.AvailableQty == 0) sharing.Status = 3;
            _context.Entry(sharing).State = EntityState.Modified;
            if ( await _context.SaveChangesAsync() > 0)
            return NoContent();

            return BadRequest("Join Failed");
        }

        [HttpPut("qty")]
        [Authorize]
        public async Task<ActionResult> updateJoining(Joining joining)
        {
            
            Joining oldJoining = await _context.Joinings.FindAsync(joining.Id);
            int qtyChange = joining.JoinQty - oldJoining.JoinQty;
            oldJoining.JoinQty = joining.JoinQty;
            _context.Entry(oldJoining).State = EntityState.Modified;
            if (await _context.SaveChangesAsync() == 0)
                return BadRequest("Joining update Failed");
            Sharing sharing = await _context.Sharings.FindAsync(joining.SharingId);
            
            if (qtyChange > sharing.AvailableQty) return BadRequest("More than available");
            sharing.AvailableQty -= qtyChange;
            if (sharing.AvailableQty == 0) sharing.Status = 3;
            
            _context.Entry(sharing).State = EntityState.Modified;
            if (await _context.SaveChangesAsync() > 0)
                return NoContent();
            
            return BadRequest("Update Failed");
        }

        [HttpPut("status")]
        [Authorize]
        public async Task<ActionResult> updateStatus(Joining joining)
        {
            Joining oldJoining = await _context.Joinings.FindAsync(joining.Id);
            oldJoining.Status = 1;
            _context.Entry(oldJoining).State = EntityState.Modified;
            if (await _context.SaveChangesAsync() == 0)
            {
                return BadRequest("Update Joining Failed");
            }
            
            Sharing sharing = await _context.Sharings.FindAsync(joining.SharingId);
            bool anyUncompleted = await _context.Joinings.Where(j => j.Status == 0 ).Where(j => j.SharingId == joining.SharingId).AnyAsync();
            if (!anyUncompleted) sharing.Status = 4;
            _context.Entry(sharing).State = EntityState.Modified;

            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }
            return BadRequest("Update Sharing Failed");
        }
        [HttpDelete]
        [Authorize]
        public async Task<ActionResult> deleteJoining(Joining joining)
        {
            _context.Joinings.Remove(joining);
            if (await _context.SaveChangesAsync() > 0)
                return NoContent();

            return BadRequest("Deteleted Failed");
        }
    }
}
