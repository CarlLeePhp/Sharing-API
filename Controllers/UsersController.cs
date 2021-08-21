using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Sharing_API.Models;
using Sharing_API.Data;
using Microsoft.EntityFrameworkCore;
using Sharing_API.Interface;
using Sharing_API.DTOs;
using AutoMapper;
using System.Security.Claims;

namespace Sharing_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPhotoService _photoService;
        public UsersController(IUserRepository userRepository, IPhotoService photoService)
        {
            _userRepository = userRepository;
            _photoService = photoService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await _userRepository.GetMembersAsync();
            return Ok(users);
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<MemberDto>> GetUser(string email)
        {
            return await _userRepository.GetMemberAsync(email);
        }

        [HttpGet("id/{id}")]
        public async Task<ActionResult<MemberDto>> GetUserById(int id)
        {
            return await _userRepository.GetMemberByIdAsync(id);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepository.GetUserByEmailAsync(email);

            // _mapper.Map(memberUpdateDto, user);
            user.Street = memberUpdateDto.Street;
            user.City = memberUpdateDto.City;
            user.Gender = memberUpdateDto.Gender;

            _userRepository.Update(user);

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update user");
        }
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepository.GetUserByEmailAsync(email);
            // if there is a photo, delete it
            // do not neet to update the database, it will be done later
            if (user.PhotoPublicId != null)
            {
                var deleteResult = await _photoService.DeletePhotoAsync(user.PhotoPublicId);
                if (deleteResult.Error != null) return BadRequest(deleteResult.Error.Message);
            }

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new PhotoDto
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            user.PhotoUrl = result.SecureUrl.AbsoluteUri;
            user.PhotoPublicId = result.PublicId;

            _userRepository.Update(user);

            if(await _userRepository.SaveAllAsync())
            return Ok(photo);

            return BadRequest("Upload Failed");
        }
        
    }
}
