using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SignalR.Contracts.Entities;
using SignalR.Contracts.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<SQLiteDbContext, User>, IUserRepository
    {
        private readonly ILogger logger;
        private readonly SQLiteDbContext dbContext;
        private readonly DbSet<User> user;

        public UserRepository(ILogger<UserRepository> logger, SQLiteDbContext context) : base(context)
        {
            this.logger = logger;
            dbContext = context;
            this.user = context.Set<User>();
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await GetAll().AsNoTracking().Where(u => u.IsDeleted == false).ToListAsync<User>();
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            return await dbSet.AsNoTracking().Where(u => u.Id == userId && u.IsDeleted == false).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByTenantGuidAndEmailAsync(Guid tenantGuid, string email)
        {
            return await dbSet.AsNoTracking().Where(u => u.TenantGuid == tenantGuid && u.Email == email && u.IsDeleted == false).FirstOrDefaultAsync();
        }

        public async Task AddUserAsyc(User user)
        {
            await AddAsync(user);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            var existingUser = await GetUserAsync(user.Id);
            if (existingUser == null) return false;
            await UpdateAsync(user);
            return true;
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var existingUser = await dbSet.Where(u => u.Id == userId).FirstOrDefaultAsync();
            if (existingUser == null) return false;
            existingUser.IsDeleted = true;
            await dbContext.SaveChangesAsync();
            return true;
        }

    }
}
