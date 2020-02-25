using SignalR.Contracts.DTOs;
using System.Threading.Tasks;

namespace SignalR.Contracts.Interfaces.Domain
{
    public interface IUserService
    {
        Task<ResultDto> SaveUserAsync(UserDto userDto);
    }
}
