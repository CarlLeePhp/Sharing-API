using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public class SharingsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserRepository _userRepository;
        private readonly ISharingRepository _sharingRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        public SharingsController(DataContext contex, IUserRepository userRepository,
            ISharingRepository sharingRepository, IMapper mapper, IPhotoService photoService)
        {
            _context = contex;
            _userRepository = userRepository;
            _sharingRepository = sharingRepository;
            _mapper = mapper;
            _photoService = photoService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<SharingDto>>> GetSharings()
        {
            var sharings = await _sharingRepository.GetSharingsAsync();
            return Ok(sharings);
        }

        [HttpGet("search/{categoryId}/{keyword}")]
        public async Task<ActionResult<IEnumerable<SharingDto>>> SearchSharings(int categoryId = 0, string keyword = "")
        {
            var sharings = await _sharingRepository.SearchSharings(categoryId, keyword);
            return Ok(sharings);
        }

        [HttpGet("id/{id}")]
        public async Task<ActionResult<Sharing>> GetSharing(int id)
        {
            SharingDto sharing = await _sharingRepository.GetSharingById(id);
            return Ok(sharing);
        }
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<SharingDto>>> GetSharingsByStatus(int status)
        {
            var sharings = await _sharingRepository.GetSharingsByStatus(status);
            return Ok(sharings);
        }

        [HttpGet("user/{email}")]
        public async Task<ActionResult<IEnumerable<SharingDto>>> GetSharingsByUser(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return BadRequest("Invailied user");
            }
            var sharings = await _sharingRepository.GetSharingsByUser(user.Id);
            return Ok(sharings);
        }
        [HttpGet("interest/{email}")]
        public async Task<ActionResult<IEnumerable<SharingDto>>> GetSharingByInterests(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return BadRequest("Invailied user");
            }
            // find all interests
            var interests =await _context.Interest
                
                .Where(i => i.AppUserId == user.Id)
                .ProjectTo<InterestDto>(_mapper.ConfigurationProvider)
                .ToArrayAsync();
            
            List<int> categoryIds = new List<int>();
            List<SharingDto> sharings = new List<SharingDto>();
            for(int i=0; i < interests.Length; i++)
            {
                IEnumerable<SharingDto> sharingDtos = await _context.Sharings
                    .Where(s => s.CategoryId == interests[i].CategoryId)
                    .Where(s => s.Status == 2)
                    .ProjectTo<SharingDto>(_mapper.ConfigurationProvider)
                    .ToArrayAsync();
                if (sharingDtos.Count() > 0)
                {
                    foreach(SharingDto sharingDto in sharingDtos)
                    {
                        sharings.Add(sharingDto);
                    }
                }
                
            }

            return Ok(sharings);
            
        }
        
        [HttpPost]
        public async Task<ActionResult> CreateSharing(SharingDto sharingDto)
        {
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return BadRequest("Invailied user");
            }
            Sharing sharing = new Sharing();
            

            _mapper.Map(sharingDto, sharing);

            sharing.AppUserId = user.Id;
            sharing.Status = 1;
            sharing.SavedDate = DateTime.Now;
            _sharingRepository.CreateSharing(sharing);

            if(await _sharingRepository.SaveAllAsync())
            {
                return NoContent();
            }
            return BadRequest("Create sharing failed");
        }
        [HttpPut]
        public async Task<ActionResult> UpdateSharing(SharingDto sharingDto)
        {
            Sharing sharing = new Sharing();
            
            _mapper.Map(sharingDto, sharing);
            if (sharing.Status == 1) // saved
            {
                sharing.SavedDate = DateTime.Now;
            }
            else if (sharing.Status == 2) // published
            {
                sharing.PublishedDate = DateTime.Now;
            }
            _sharingRepository.Update(sharing);
            if (await _sharingRepository.SaveAllAsync())
            {
                return NoContent();
            }
            return BadRequest("Update sharing failed");
        }
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto([FromForm]IFormFile file, [FromForm]int sharingId)
        {
            if (file == null) return BadRequest("File cannot be null");
            Sharing sharing = await _context.Sharings.FindAsync(sharingId);
            // if there is a photo, delete it
            // do not neet to update the database, it will be done later
            if (sharing.PhotoPublicId != null && sharing.PhotoPublicId !="")
            {
                var deleteResult = await _photoService.DeletePhotoAsync(sharing.PhotoPublicId);
                if (deleteResult.Error != null) return BadRequest(deleteResult.Error.Message);
            }

            var result = await _photoService.AddPhotoAsync(file);
            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new PhotoDto
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };
            sharing.PhotoUrl = photo.Url;
            sharing.PhotoPublicId = photo.PublicId;
            _context.Entry(sharing).State = EntityState.Modified;

            if (await _context.SaveChangesAsync() > 0)
                return Ok(photo);

            return BadRequest("Upload Failed");
        }

    }
}

