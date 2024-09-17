using MongoDB.Driver;

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
    }
}
