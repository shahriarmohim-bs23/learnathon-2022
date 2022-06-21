using backend_task3.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace backend_task3.Services
{
    public class Userservices
    {
        private readonly IMongoCollection<User> _register;
        public Userservices(IOptions<MongoDbSettings> dbsettings)
        {
            MongoClient client = new MongoClient(dbsettings.Value.Connection);
            IMongoDatabase database = client.GetDatabase(dbsettings.Value.Database);
            _register = database.GetCollection<User>(dbsettings.Value.Collection);


        }

        public IMongoCollection<User> Users()
        {
            return _register;
        }

        public async Task<User>GetEmail(string Email)
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Eq("Email", Email);
            var find = _register.Find(filter);
            var user   = find.FirstOrDefault();
            return user;

            

        }
        public async Task<int>Getcount()
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Empty;
            var find = _register.Find(filter);
            return(int) find.Count();
        }
        public async Task<List<User>> GetAsync(int? querypage,int perpage)
        {
            int page = querypage.GetValueOrDefault(1) == 0 ? 1 : querypage.GetValueOrDefault(1);

           


            FilterDefinition<User> filter = Builders<User>.Filter.Empty;
            var find = _register.Find(filter);

            return find.Skip((querypage-1)*perpage).Limit(perpage).ToList();    
        }
        public async Task CreateAsync(User newuser)
        {
            await _register.InsertOneAsync(newuser);
            return;
        }
        public async Task DeleteAsync(string id)
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Eq("_id",ObjectId.Parse(id));
            await _register.DeleteOneAsync(filter);
            return;
        }
        public async Task UpdateAsync(UserDto updateUser)
        {
            FilterDefinition<User>filter = Builders<User>.Filter.Eq("_id",ObjectId.Parse(updateUser.id));
            UpdateDefinition<User> update = Builders<User>.Update.Set("Email", updateUser.Email)
                                                                  .Set("Birthday",updateUser.Birthday);
            await _register.UpdateOneAsync(filter,update);
            return;

        }

    }
}
