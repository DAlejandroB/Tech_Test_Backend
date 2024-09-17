using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using Tech_Test_Backend.Models;

namespace Tech_Test_Backend.Data
{
    public class MongoRoleStore : IRoleStore<IdentityRole<Guid>>
    {
        private readonly IMongoCollection<IdentityRole<Guid>> _roleCollection;
        public MongoRoleStore(MongoDbContext context)
        {
            _roleCollection = context.Roles;
        }

        public async Task<IdentityResult> CreateAsync(IdentityRole<Guid> role, CancellationToken cancellationToken)
        {
            try
            {
                await _roleCollection.InsertOneAsync(role, cancellationToken: cancellationToken);
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = ex.Message });
            }
        }

        public async Task<IdentityResult> DeleteAsync(IdentityRole<Guid> role, CancellationToken cancellationToken)
        {
            var result = await _roleCollection.DeleteOneAsync(r => r.Id == role.Id, cancellationToken);
            return result.DeletedCount > 0 ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "Role not found" });
        }

        public void Dispose() { /* Dispose resources if necessary */ }

        public async Task<IdentityRole<Guid>> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            return await _roleCollection.Find(r => r.Id == Guid.Parse(roleId)).SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<IdentityRole<Guid>> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return await _roleCollection.Find(r => r.Name == normalizedRoleName).SingleOrDefaultAsync(cancellationToken);
        }

        public Task<string> GetNormalizedRoleNameAsync(IdentityRole<Guid> role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task<string> GetRoleIdAsync(IdentityRole<Guid> role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(IdentityRole<Guid> role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(IdentityRole<Guid> role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(IdentityRole<Guid> role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.CompletedTask;
        }
        public async Task<IdentityResult> UpdateAsync(IdentityRole<Guid> role, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _roleCollection.ReplaceOneAsync(
                    r => r.Id == role.Id,
                    role,
                    new ReplaceOptions { IsUpsert = false },
                    cancellationToken
                );

                if (result.MatchedCount > 0)
                {
                    return IdentityResult.Success;
                }
                else
                {
                    return IdentityResult.Failed(new IdentityError { Description = "Role not found" });
                }
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = ex.Message });
            }
        }

    }
}
