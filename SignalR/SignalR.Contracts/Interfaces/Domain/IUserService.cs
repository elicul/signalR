using SignalR.Contracts.DTOs;
using SignalR.Contracts.Entities;
using System;
using System.Threading.Tasks;

namespace SignalR.Contracts.Interfaces.Domain
{
    public interface IUserService
    {
        Task<ResultDto> SaveUserAsync(UserDto userDto);
        Task<ResultDto<string>> SendMessageToUserAsync(string email, Guid tenantGuid, Message message);
        Task<ResultDto<string>> SendMessageToTenantUsersAsync(Guid tenantGuid, Message message);
    }
}
