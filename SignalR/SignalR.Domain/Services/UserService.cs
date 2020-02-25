using Microsoft.Extensions.Logging;
using SignalR.Contracts.DTOs;
using SignalR.Contracts.Entities;
using SignalR.Contracts.Interfaces.Domain;
using SignalR.Contracts.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalR.Domain.Services
{
    public class UserService: IUserService
    {
        private readonly ILogger logger;
        private readonly IUserRepository userRepository;

        public UserService(ILogger<UserService> logger, IUserRepository userRepository)
        {
            this.logger = logger;
            this.userRepository = userRepository;
        }

        public async Task<ResultDto> SaveUserAsync(UserDto userDto)
        {
            var result = new ResultDto();
            try
            {
                if(userDto.TenantGuid == null || userDto.TenantGuid == Guid.Empty || string.IsNullOrEmpty(userDto.Email))
                {
                    logger.LogError($"Invalid arguments on method {nameof(SaveUserAsync)}");
                    result.ErrorMessage = $"Invalid arguments on method {nameof(SaveUserAsync)}";
                    result.ResultStatus = Contracts.Enums.ResultStatus.ArgumentsInvalid;
                    return result;
                }
                var user = await userRepository.GetUserByTenantGuidAndEmailAsync(userDto.TenantGuid, userDto.Email);
                if(user == null)
                {
                    logger.LogInformation($"User not found {nameof(SaveUserAsync)}");
                    await userRepository.AddUserAsyc(new User
                    {
                        Email = userDto.Email,
                        TenantGuid = userDto.TenantGuid,
                        TenantType = userDto.TenantType,
                        ConnectionId = userDto.ConnectionId
                    });
                    logger.LogInformation($"User added {nameof(SaveUserAsync)}");
                }
                else
                {
                    logger.LogInformation($"User already exists {nameof(SaveUserAsync)}");
                    user.ConnectionId = userDto.ConnectionId;
                    await userRepository.UpdateUserAsync(user);
                    logger.LogInformation($"User updated {nameof(SaveUserAsync)}");
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error saving user. EX: {ex}");
                result.ErrorMessage = $"Error saving user. EX: {ex}";
                result.ResultStatus = Contracts.Enums.ResultStatus.Error;
            }
            return result;
        }
    }
}
