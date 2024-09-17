using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using Tech_Test_Backend.Models;

namespace Tech_Test_Backend.Data;
public class MongoUserStore : IUserStore<User>, IUserPasswordStore<User>
{
    private readonly IMongoCollection<User> _userCollection;

    public MongoUserStore(MongoDbContext context)
    {
        _userCollection = context.Users;
    }

    public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
    {
        try
        {
            await _userCollection.InsertOneAsync(user, cancellationToken: cancellationToken);
            return IdentityResult.Success;
        }
        catch (Exception ex)
        {
            return IdentityResult.Failed(new IdentityError { Description = ex.Message });
        }
    }

    public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
    {
        var result = await _userCollection.DeleteOneAsync(u => u.Id == user.Id, cancellationToken);
        return result.DeletedCount > 0 ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "User not found" });
    }

    public void Dispose() {
    }

    public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        return await _userCollection.Find(u => u.Id == Guid.Parse(userId)).SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        return await _userCollection.Find(u => u.NormalizedUserName == normalizedUserName).SingleOrDefaultAsync(cancellationToken);
    }

    public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.NormalizedUserName);
    }

    public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Id.ToString());
    }

    public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.UserName);
    }

    public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
    {
        user.NormalizedUserName = normalizedName;
        return Task.CompletedTask;
    }

    public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
    {
        user.UserName = userName;
        return Task.CompletedTask;
    }

    public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
    {
        user.PasswordHash = passwordHash;
        return Task.CompletedTask;
    }

    public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.PasswordHash);
    }

    public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
    }

    public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _userCollection.ReplaceOneAsync(u => u.Id == user.Id, user, cancellationToken: cancellationToken);
            return result.ModifiedCount > 0 ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "Update failed" });
        }
        catch (Exception ex)
        {
            return IdentityResult.Failed(new IdentityError { Description = ex.Message });
        }
    }


}

