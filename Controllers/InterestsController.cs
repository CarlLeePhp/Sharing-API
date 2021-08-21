using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sharing_API.Data;
using Sharing_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sharing_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterestsController : ControllerBase
    {
        private readonly DataContext _context;
        public InterestsController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> CreateInterest(Interest interest)
        {
            await _context.Interest.AddAsync(interest);
            if (await _context.SaveChangesAsync() > 0) return NoContent();
            return BadRequest("Interest created failed");
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInterest(int id)
        {
            Interest interest = await _context.Interest.FindAsync(id);
            _context.Interest.Remove(interest);
            if (await _context.SaveChangesAsync() > 0) return NoContent();
            return BadRequest("Interest deleted failed");
        }
    }
}
