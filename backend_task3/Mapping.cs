using AutoMapper;
using backend_task3.Models;

namespace backend_task3
{
    public class Mapping:Profile
    {
      public Mapping()
        {
            CreateMap<User, UserDto>();
        }
    }
}
