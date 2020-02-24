using SignalR.Contracts.DTOs;
using SignalR.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalR.Contracts.Interfaces.Domain
{
    public interface IUserService
    {
        Task<ResultDto<List<User>>> GetUsersAsync();
        Task<ResultDto<User>> GetUserAsync(Guid userId);
        Task<ResultDto<string>> PostUserAsync(UserDto userDto);
        Task<ResultDto<string>> PutUserAsync(UserDto userDto, Guid userId);
        Task<ResultDto<string>> RemoveUserAsync(Guid userId);
    }
}
