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
            IUsersService _usersService,
            ITokenService _tokenService,
            IPasswordHasher _passwordHasher,
            IRedisCacheService _redisCacheService,
            IEmailClient _emailClient
            )
        {
            this._usersService = _usersService;
            this._tokenService = _tokenService;
            this._passwordHasher = _passwordHasher;
            this._redisCacheService = _redisCacheService;
            this._emailClient = _emailClient;
        }
        [HttpPost("Register")]
        public async Task<ActionResult<ResponseModel>> Register([FromBody] SignUpDto request)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseModel(IsSuccess: false, Data: ModelState));
            if(string.IsNullOrWhiteSpace(request.Password)) return BadRequest(new ResponseModel(IsSuccess: false, Message: "Password is required"));
            if(string.IsNullOrWhiteSpace(request.FullName)) return BadRequest(new ResponseModel(IsSuccess: false, Message: "FullName is required"));
            var existingUser = await _usersService.GetUserByEmailAsync(request.Email);
            if (existingUser is not null) return BadRequest(new ResponseModel(IsSuccess: false, Message: "User with this email already exists"));


            if (string.IsNullOrWhiteSpace(request.OTP)) return BadRequest(new ResponseModel(IsSuccess: false, Message: "OTP is Required"));
            if (!int.TryParse(request.OTP, out _) || request.OTP.Length != 6) return Ok(new ResponseModel(IsSuccess: false, Message: "Invalid OTP format"));
            string otp = _redisCacheService.GetString(request.Email.ToLower());
            if (!string.Equals(otp, request.OTP)) return Ok(new ResponseModel(IsSuccess: false, Message: "Invalid OTP"));


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

        [HttpPost("ValidateSignUpOTP")]
        public async Task<ActionResult<ResponseModel>> ValidateSignUpOTPRegister2([FromBody] SignUpDto request)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseModel(IsSuccess: false, Data: ModelState));
            var existingUser = await _usersService.GetUserByEmailAsync(request.Email);
            if (existingUser is not null) return BadRequest(new ResponseModel(IsSuccess: false, Message: "User with this email already exists"));

            int secureId = RandomNumberGenerator.GetInt32(100000, 999999);
            bool isOTPSucess = _redisCacheService.SetString(request.Email.ToLower(), secureId.ToString());
            bool isEmailSucess = await _emailClient.SendEmail(toEmail: request.Email.ToLower(), htmlEmailBody: $"<h1>OTP is : {secureId.ToString()}</h1>", fallbackemailBody: $"OTP is :{secureId.ToString()}", emailSubject: "SignUp OTP");
            Task.WhenAll();
            string returnMessage = isOTPSucess && isEmailSucess ? "OTP sent please enter the opt in register endpoind" : "OTP faild please try again";
            return Ok(new ResponseModel(IsSuccess: true, Message: returnMessage));

        }

        [HttpPost("Login")]
        public async Task<ActionResult<ResponseModel>> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _usersService.GetUserByEmailAsync(request.Email);

            if (user is null)
                return Unauthorized(new ResponseModel(IsSuccess: false, Message: "Invalid email"));

            // Verify password
            if (!_passwordHasher.VerifyPassword(request.Password, user.Password))
                return Unauthorized(new ResponseModel(IsSuccess: false, Message: "Invalid password"));

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
