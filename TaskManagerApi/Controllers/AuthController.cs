using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using System.Security.Cryptography;
using System.Security.Permissions;
using TaskManager.CacheService;
using TaskManager.Email;
using TaskManagerApi.Application.Dtos;
using TaskManagerApi.Application.Interfaces;
using TaskManagerApi.Domain.Entities;
using TaskManagerApi.Models;
using TaskManagerApi.Security.Services.Classes;
using TaskManagerApi.Security.Services.Interfaces;

namespace TaskManagerApi.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRedisCacheService _redisCacheService;
        private readonly IEmailClient _emailClient;

        public AuthController(
            IUsersService usersService,
            ITokenService tokenService,
            IPasswordHasher passwordHasher,
            IRedisCacheService redisCacheService,
            IEmailClient emailClient
            )
        {
            _usersService = usersService;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _redisCacheService = redisCacheService;
            _emailClient = emailClient;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ResponseModel>> Register([FromBody] RegisterRequestDto request)
        {
            if (await _usersService.GetUserByEmailAsync(request.Email) is not null) 
                return BadRequest(new ResponseModel(IsSuccess: false, Message: "User with this email already exists"));

            string? otp = _redisCacheService.GetString(request.Email.ToLower());
            if (!string.Equals(otp, request.OTP)) return BadRequest(new ResponseModel(IsSuccess: false, Message: "Invalid OTP"));

            var user = new SignUpDto
            {
                Email = request.Email,
                FullName = request.FullName,
                Password = _passwordHasher.HashPassword(request.Password)
            };
            
            var savedUser = await _usersService.CreateTaskAsync(user);
            _redisCacheService.RemoveKey(request.Email.ToLower());

            var token = _tokenService.GenerateToken(savedUser.UserId, user.Email);
            return Ok(new ResponseModel(IsSuccess: true, Data: new AuthDto
            {
                Token = token,
                Email = user.Email,
                FullName = user.FullName
            }));
        }

        [HttpPost("RequestSignUpOTP")]
        public async Task<ActionResult<ResponseModel>> RequestSignUpOTP([FromBody] RequestOtpDto request)
        {
            var existingUser = await _usersService.GetUserByEmailAsync(request.Email);
            if (existingUser is not null) return Conflict(new ResponseModel(IsSuccess: false, Message: "User with this email already exists"));

            int secureId = RandomNumberGenerator.GetInt32(100000, 999999);
            bool isOTPSucess = _redisCacheService.SetString(request.Email.ToLower(), secureId.ToString());
            bool isEmailSucess = await _emailClient.SendEmail(toEmail: request.Email.ToLower(), htmlEmailBody: $"<h1>OTP is : {secureId.ToString()}</h1>", fallbackemailBody: $"OTP is :{secureId.ToString()}", emailSubject: "SignUp OTP");
            
            string returnMessage = isOTPSucess && isEmailSucess ? "OTP sent please enter the opt in register endpoind" : "OTP faild please try again";
           
            if (isOTPSucess && isEmailSucess)
                return Ok(new ResponseModel(IsSuccess: true, Message: returnMessage));
                
            return BadRequest(new ResponseModel(IsSuccess: false, Message: returnMessage));
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ResponseModel>> Login([FromBody] LoginRequestDto request)
        {
            var user = await _usersService.GetUserByEmailAsync(request.Email);

            if (user is null || !_passwordHasher.VerifyPassword(request.Password, user.Password))
                return Unauthorized(new ResponseModel(IsSuccess: false, Message: "Invalid email or password"));

            // Generate token
            var token = _tokenService.GenerateToken(user.UserId, user.Email);
            
            return Ok(new ResponseModel(IsSuccess: true, Data: new AuthDto
            {
                Token = token,
                Email = user.Email,
                FullName = user.FullName
            }));
        }
        [HttpGet("Health")]
        public IActionResult Health()
        {
            return Ok("Task Manager API is Online");
        }
    }
}
