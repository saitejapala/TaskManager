using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Permissions;
using TaskManagerApi.Application.Dtos;
using TaskManagerApi.Application.Interfaces;
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

        public AuthController(
            IUsersService _usersService,
            ITokenService _tokenService,
            IPasswordHasher _passwordHasher
            )
        {
            this._usersService = _usersService;
            this._tokenService = _tokenService;
            this._passwordHasher = _passwordHasher;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ResponseModel>> Register([FromBody] SignUpDto request)
        {
            if(!ModelState.IsValid) return BadRequest(new ResponseModel(IsSuccess: false, Data: ModelState));
            var existingUser = await _usersService.GetUserByEmailAsync(request.Email);
            if (existingUser is not null) return BadRequest(new ResponseModel(IsSuccess: false, Message: "User with this email already exists"));

            var user = new SignUpDto
            {
                Email = request.Email,
                FullName = request.FullName,
                Password = _passwordHasher.HashPassword(request.Password)
            };
            await _usersService.CreateTaskAsync(user);
            var savedUser = await _usersService.GetUserByEmailAsync(request.Email) ?? new SignUpDto();

            var token = _tokenService.GenerateToken(savedUser.UserId, user.Email);

            return Ok(new ResponseModel(IsSuccess: true, Data: new AuthDto
            {
                Token = token,
                Email = user.Email,
                FullName = user.FullName
            }));

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
