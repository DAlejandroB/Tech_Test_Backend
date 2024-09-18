using MongoDB.Driver;
using Tech_Test_Backend.Data;
using Tech_Test_Backend.Models;

namespace Tech_Test_Backend.Services
{
    public interface IProvidersService
    {
        Task<Provider> GetProviderByIdAsync(Guid providerId);
        Task<IReadOnlyList<Provider>> GetAllProvidersAsync();
        Task<int> GetProviderCountAsync();
        Task CreateProviderAsync(Provider provider);
        Task UpdateProviderAsync(Guid providerId, Provider provider);
        Task DeleteProviderAsync(Guid providerId);

    }
    public class ProvidersService : IProvidersService
    {
        private readonly IMongoCollection<Provider> _providers;

        public ProvidersService(MongoDbContext database)
        {
            _providers = database.Providers;
        }

        public async Task<Provider> GetProviderByIdAsync(Guid providerId)
        {
            return await _providers.Find(p => p.Id == providerId).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<Provider>> GetAllProvidersAsync()
        {
            return await _providers.Find(_ => true).ToListAsync();
        }

        public async Task<int> GetProviderCountAsync()
        {
            return (int)await _providers.CountDocumentsAsync(_ => true);
        }

        public async Task CreateProviderAsync(Provider provider)
        {
            await _providers.InsertOneAsync(provider);
        }

        public async Task UpdateProviderAsync(Guid providerId, Provider provider)
        {
            await _providers.ReplaceOneAsync(p => p.Id == providerId, provider);
        }

        public async Task DeleteProviderAsync(Guid providerId)
        {
            await _providers.DeleteOneAsync(p => p.Id == providerId);
        }
    }

}
