using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using Tech_Test_Backend.Models;

namespace Tech_Test_Backend.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        public MongoDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
            _database = client.GetDatabase("Tech_Test_BackendDB");
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<IdentityRole<Guid>> Roles => _database.GetCollection<IdentityRole<Guid>>("Roles");
        public IMongoCollection<Provider> Providers => _database.GetCollection<Provider>("Providers");
    }
}
