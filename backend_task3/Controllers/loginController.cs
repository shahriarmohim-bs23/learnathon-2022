using Microsoft.AspNetCore.Mvc;

using backend_task3.Models;
using backend_task3.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace backend_task3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowSpecificOrigins")]
    public class loginController : Controller
    {
        private readonly Userservices _mongoservice;
       
        private readonly IConfiguration _configuration;
        public loginController(Userservices mongoservice,  IConfiguration configuration)
        {
            _mongoservice = mongoservice;
          
            _configuration = configuration;
        }
        public static User user = new User();

        [HttpPost]

        public async Task<IActionResult> Login(loginDto user1)
        {
            user = await _mongoservice.GetEmail(user1.Email);

            if (user == null)
            {
                return BadRequest("Email Not Found");
            }
            if (!VerifyPasswordHash(user1.Password, user.HashPassword, user.SaltPassword))
            {
                return BadRequest("Wrong Password");
            }
            

            string token = Createtoken(user);

            var refreshtoken = GenerateRefreshToken();
            // user.Refreshtoken = refreshtoken.Token;
            //  user.RefreshTokenExpires=refreshtoken.Expires;   
            return Ok(new 
            {
                Token = token,
                RefreshToken = refreshtoken.Token,
                RefreshTokenExpires = refreshtoken.Expires
            });
           
        }

        private Refreshtoken GenerateRefreshToken()
        {
            var refreshToken = new Refreshtoken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.UtcNow.AddDays(7)
               
            };

            return refreshToken;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        private string Createtoken(User user)
        {
            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString("MMM ddd dd yyyy HH:mm:ss tt")),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("id", user.id),

            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
