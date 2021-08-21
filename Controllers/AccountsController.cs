using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sharing_API.Data;
using Sharing_API.Interface;
using Sharing_API.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Sharing_API.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;

namespace Sharing_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountsController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            DataContext context, ITokenService tokenService,
            IConfiguration config, IUserRepository userRepository)
        {
            _context = context;
            _tokenService = tokenService;
            _config = config;
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Regiset(RegisterDto registerDto)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email)) return BadRequest("Email is exist.");

            if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.UserName)) return BadRequest("User Name is exist");

            if(!ModelState.IsValid) return BadRequest("The length of the password should be more than 4");
            
            var user = new AppUser()
            {
                Email = registerDto.Email.ToLower(),
                UserName = registerDto.UserName
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            
            return new UserDto
            {
                Email = user.Email,
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
            
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized("Invalid Email Address");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized(user.Email);

            return new UserDto
            {
                Email = user.Email,
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }
        [HttpPost("resetpw")]
        public async Task<ActionResult> ResetPassword(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized("Invalid Email Address");

            
            // generate a paasword
            string password = GeneratePassword();
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetPassResult = await _userManager.ResetPasswordAsync(user, token, password);
            if (!resetPassResult.Succeeded) return Unauthorized(resetPassResult.Errors);

            // Send it by send in blue API
            Configuration.Default.AddApiKey("api-key", _config["SendinblueKey"]);

            var apiInstance = new TransactionalEmailsApi();
            SendSmtpEmailSender sender = new SendSmtpEmailSender
            {
                Name = "Sharing",
                Email = "leekunhui@gmail.com"
            };
            List<SendSmtpEmailTo> tos = new List<SendSmtpEmailTo>();
            tos.Add(new SendSmtpEmailTo(loginDto.Email));
            var sendSmtpEmail = new SendSmtpEmail {
                Subject = "Reset Password",
                Sender = sender,
                To = tos,
                TextContent = password
            };

            CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);

            // update password
            //using var hmac = new HMACSHA512(user.PasswordSalt);
            //var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            //user.PasswordHash = computedHash;

           
            return NoContent();
        }
        

        private string GeneratePassword()
        {
            List<char> password = new List<char>();
            Random rd = new Random();
            int charValue;
            for(int i=0; i < 8; i++)
            {
                charValue = rd.Next(48,122);
                password.Add((char)charValue);
            }

            return new string(password.ToArray());
        }
    }

    
}
