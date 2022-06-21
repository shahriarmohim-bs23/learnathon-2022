using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace backend_task3.Models
{
    public class UserDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]

        public string? id { get; set; }
        [Required,UniqueName]
        public string Username { get; set; } = null!;


        [Required,UniqueEmail]
        public string Email { get; set; } = null!;

        [Required]
        [Minimamage(18)]

        public string? Birthday { get; set; }
    }
}
