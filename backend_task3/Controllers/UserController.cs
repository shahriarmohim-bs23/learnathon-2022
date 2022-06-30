using AutoMapper;
using backend_task3.Models;
using backend_task3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace backend_task3.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowSpecificOrigins")]
   
    public class UserController : Controller
    {
        private readonly Userservices _mongoservice;
        private readonly IMapper _mapper;

        public UserController(Userservices mongoservice, IMapper mapper)
        {
            _mongoservice = mongoservice;
            _mapper = mapper;

        }

        public static User user = new User();
        [HttpGet,Authorize]

        public async Task<ActionResult> Get(int querypage, int perpage)
        {
            int totalcount = await _mongoservice.Getcount();
            Pagination pagination = new Pagination(perpage, querypage, totalcount);

            List<UserDto> Getuser = _mapper.Map<List<User>, List<UserDto>>(await _mongoservice.GetAsync(querypage, perpage));
            var camelcaseformatter = new JsonSerializerSettings();
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pagination, camelcaseformatter));
            return Ok(Getuser);



        }
        [HttpPut,Authorize]
        public async Task<ActionResult> Update(UserDto updateuser)
        {
            var user = await _mongoservice.GetEmail(updateuser.Email);
            if (user == null ||user.id ==updateuser.id)
            {
                await _mongoservice.UpdateAsync(updateuser);
                return NoContent();
            }
            return BadRequest("Email is already taken");
        }


        [HttpDelete,Authorize]
        public async Task<ActionResult> Delete(string id)
        {
            await _mongoservice.DeleteAsync(id);
            return NoContent();
        }
    }
}
