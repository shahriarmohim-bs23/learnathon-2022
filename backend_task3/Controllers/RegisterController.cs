using AutoMapper;
using backend_task3.Models;
using backend_task3.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
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
    public class RegisterController : ControllerBase
    {
        private readonly Userservices _mongoservice;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;    
        
        public RegisterController(Userservices mongoservice,IMapper mapper, IConfiguration configuration)
        {
            _mongoservice = mongoservice;
            _mapper = mapper;
            _configuration = configuration;    
        }
        
        public static  User user = new User();
        [HttpGet]
       
        public async Task<ActionResult> Get(int  querypage,int perpage)
        {
            int totalcount =await _mongoservice.Getcount();
            Pagination pagination = new Pagination(perpage,querypage,totalcount);
            
            List<UserDto> Getuser = _mapper.Map<List<User>, List<UserDto>>(await _mongoservice.GetAsync(querypage,perpage));
            var camelcaseformatter = new JsonSerializerSettings();
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pagination,camelcaseformatter));
            return Ok(Getuser);



       }
        [HttpPost]
        public async Task<ActionResult> Post(Register register)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            CreatePasswordHash(register.Password, out byte[] Passwordhash,out byte[] Passwordsalt);
            user.Username = register.Username;
            user.Email = register.Email;
            user.Birthday = register.Birthday;
            user.HashPassword = Passwordhash;
            user.SaltPassword = Passwordsalt;
            await _mongoservice.CreateAsync(user);

            return StatusCode(200); ;

        }
        
        [HttpPut]
        public async Task<ActionResult> Update(UserDto updateuser)
        {
            await _mongoservice.UpdateAsync(updateuser);
            return NoContent();
        }


        [HttpDelete]
        public async Task<ActionResult>Delete(string id)
        {
            await _mongoservice.DeleteAsync(id); 
            return NoContent(); 
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
