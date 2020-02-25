using SignalR.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.Contracts.Interfaces.Infrastructure
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync();
        Task<User> GetUserAsync(Guid userId);
        Task AddUserAsyc(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(Guid userId);
        Task<User> GetUserByTenantGuidAndEmailAsync(Guid tenantGuid, string email);
    }
}
