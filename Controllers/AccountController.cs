using AutoMapper;
using E_Commerce_API.Data;
using E_Commerce_API.DTO.Identity;
using E_Commerce_API.Filters;
using E_Commerce_API.Models;
using E_Commerce_API.Service.Implementation;
using E_Commerce_API.Static;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
      public class AccountController : ControllerBase
    {

        private readonly  IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JWTService _jwtService;
        private readonly IdentityUsers _users;
        private readonly Application _context;

        public AccountController(IdentityUsers users, JWTService jwtService, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, Application context)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _users = users;
            _context = context;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO register )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var findEmail = await _userManager.FindByEmailAsync(register.Email);
            if (findEmail != null)
                return BadRequest("your email already exist");
            

            var newUser = _mapper.Map<User>(register);
            
            var result = await _userManager.CreateAsync(newUser, register.Password);
            
                

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            if (string.IsNullOrEmpty(newUser.UserName))
            {
                newUser.UserName = register.UserName;
            }
               
                await _userManager.AddToRoleAsync(newUser, "User");
                var token = await _jwtService.GenerateTokenAsync(newUser);

            return Ok(
                new
                {
                    AccessToken = token.AccessToken,
                    RefreshToken = token.RefreshToken
                });

        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
           if(!ModelState.IsValid)
                return BadRequest(ModelState);

           var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null)
                return Unauthorized("Invalid Email or password");

            var validPassword = await _userManager.CheckPasswordAsync(user, login.Password);
            if(!validPassword)
                return Unauthorized("Invalid Email or password");

            var token = await _jwtService.GenerateTokenAsync(user);

            return Ok(new
            {
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken
            });

        }


        [HttpPost("Role")]
        public async Task<IActionResult> PromoteToAdmin(string userEmail)
        {
            // دور الـ UserManager
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
                return NotFound("User not found");

            if (user.Email == User.Identity.Name)
                return BadRequest("You cannot promote your selfe");

            // شيله من اليوزر لو موجود
            if (await _userManager.IsInRoleAsync(user, "User"))
                await _userManager.RemoveFromRoleAsync(user, "User");

            // ضيفه للادمن
            if (!await _userManager.IsInRoleAsync(user, "Admin"))
                await _userManager.AddToRoleAsync(user, "Admin");

            return Ok($"User {userEmail} is now an Admin");
        }

        [HttpGet("Users")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers( CancellationToken ct = default)
        {
            var users = await _users.GetAllUsers(ct);
            var map  = _mapper.Map<List<UsersDTO>>(users);
            return Ok(map);
        }



        [HttpPut("Delete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string email, CancellationToken ct = default)
        {
            var result = await _users.DeleteUser(email, ct);

            return result switch
            {
                "Email Not Found" => NotFound("Email Not Found"),
                "Email Already Deleted" => BadRequest("Email For User is Already Deleted"),
                "Deleted Successfuly" => Ok("User Deleted Successfuly")
            };
        }



        [HttpPost("Refresh")]
        public async Task<IActionResult> RefreshToken(string refreshtoken)
        {
            var tokenRecord = await _context.RefreshToken.FirstOrDefaultAsync(a => a.Token == refreshtoken);

            if(tokenRecord == null)
                return Unauthorized ("this token invalid");

            if (tokenRecord.Expires < DateTime.UtcNow)
                return Unauthorized("this token is expired");


            if (tokenRecord.IsRevoked)
                return Unauthorized("this token is cancelled");


            var user = await _userManager.FindByIdAsync(tokenRecord.UserId);
            if (user == null)
                return NotFound("this user invalid");

            var newToken = await _jwtService.GenerateTokenAsync(user);


            return Ok(
                new
                {
                    AccessToken = newToken.AccessToken,
                    RefreshToken = newToken.RefreshToken,
                });

        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout(string refreshToken)
        {
            var accesstoken = await _context.RefreshToken.FirstOrDefaultAsync(a => a.Token == refreshToken);
            if(accesstoken != null)
            {
                _context.RefreshToken.RemoveRange(accesstoken);
                _context.SaveChanges();
            }
            return Ok("Logout Successfuly");

        }


    }
}
