using AutoMapper;
using backend_task3.Models;
using backend_task3.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;

using System.Security.Cryptography;


namespace backend_task3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowSpecificOrigins")]
    public class RegisterController : ControllerBase
    {
        private readonly Userservices _mongoservice;
       
        public RegisterController(Userservices mongoservice)
        {
            _mongoservice = mongoservice;
           
              
        }
        
        public static  User user = new User();
        
        [HttpPost]
        public async Task<ActionResult> Post(Register register)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            CreatePasswordHash(register.Password, out byte[] Passwordhash,out byte[] Passwordsalt);
            user.id = ObjectId.GenerateNewId().ToString();
            user.Username = register.Username;
            user.Email = register.Email;
            user.Birthday = register.Birthday;
            user.HashPassword = Passwordhash;
            user.SaltPassword = Passwordsalt;
            await _mongoservice.CreateAsync(user);

            return StatusCode(200); ;

        }
        
       
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

       
       
    }
}
