using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sharing_API.Data;
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
    public class CommentsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserRepository _userRepository;
        public CommentsController(DataContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<ActionResult> CreateComment(Comment comment)
        {
            // 1. find the user who submite the comment from the system
            // 2. add a comment
            // 3. find and update the status of the joining as commented [3]
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return Unauthorized();
            }
            comment.JoinUserId = user.Id;
            _context.Comment.Add(comment);
            if (await _context.SaveChangesAsync() == 0) return BadRequest("Create Comment Failed");

            Joining joining = _context.Joinings.Where(j => j.SharingId == comment.SharingId)
                .Where(j => j.JoinUserId == user.Id).FirstOrDefault();
            if (joining == null) return BadRequest("Invalid Sharing Information");
            joining.Status = 2;
            _context.Entry(joining).State = EntityState.Modified;
            if (await _context.SaveChangesAsync() > 0)
                return NoContent();


            return BadRequest("Create Comment Failed");
        }

        [HttpGet("sharing/{id}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetBySharing(int id)
        {
            var comments = await _context.Comment.Where(c => c.SharingId == id).AsNoTracking().ToArrayAsync();
            return Ok(comments);
        }
    }
}
