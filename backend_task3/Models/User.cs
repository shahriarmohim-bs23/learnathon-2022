using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System.ComponentModel.DataAnnotations;

namespace backend_task3.Models
{
    public class User
    {



        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }


        public string Username { get; set; } = null!;



            public string Email { get; set; } = null!;
            
           
            public string? Birthday { get; set; }

            public byte[] HashPassword { get; set; } = null!;
            
            public byte  [] SaltPassword { get; set; } = null!; 
             
            public string Refreshtoken { get; set; }   =   string.Empty;
            

           public DateTime? RefreshTokenExpires=null ;

       
    }
}
