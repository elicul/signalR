using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalR.Contracts.DTOs;
using SignalR.Contracts.Entities;
using SignalR.Contracts.Interfaces.Domain;
using SignalR.Contracts.Interfaces.Hubs;
using SignalR.Contracts.Interfaces.Infrastructure;
using SignalR.Hubs;
using System;
using System.Threading.Tasks;

namespace SignalR.Domain.Services
{
    public class UserService: IUserService
    {
        private readonly ILogger logger;
        private readonly IUserRepository userRepository;
        private readonly IHubContext<NotifyHub, ITypedHubClient> hubContext;

        public UserService(ILogger<UserService> logger, IUserRepository userRepository, IHubContext<NotifyHub, ITypedHubClient> hubContext)
        {
            this.logger = logger;
            this.userRepository = userRepository;
            this.hubContext = hubContext;
        }

        public async Task<ResultDto> SaveUserAsync(UserDto userDto)
        {
            var result = new ResultDto();
            try
            {
                if(userDto.TenantGuid == null || userDto.TenantGuid == Guid.Empty || string.IsNullOrEmpty(userDto.Email))
                {
                    logger.LogError($"Invalid arguments on method {nameof(SaveUserAsync)}.");
                    result.ErrorMessage = $"Invalid arguments on method {nameof(SaveUserAsync)}.";
                    result.ResultStatus = Contracts.Enums.ResultStatus.ArgumentsInvalid;
                    return result;
                }
                var user = await userRepository.GetUserByTenantGuidAndEmailAsync(userDto.TenantGuid, userDto.Email);
                if(user == null)
                {
                    logger.LogInformation($"User not found {nameof(SaveUserAsync)}.");
                    await userRepository.AddUserAsyc(new User
                    {
                        Email = userDto.Email,
                        TenantGuid = userDto.TenantGuid,
                        TenantType = userDto.TenantType,
                        ConnectionId = userDto.ConnectionId
                    });
                    logger.LogInformation($"User added {nameof(SaveUserAsync)}.");
                }
                else
                {
                    logger.LogInformation($"User already exists {nameof(SaveUserAsync)}.");
                    user.ConnectionId = userDto.ConnectionId;
                    await userRepository.UpdateUserAsync(user);
                    logger.LogInformation($"User updated {nameof(SaveUserAsync)}.");
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

        public async Task<ResultDto<string>> SendMessageToUserAsync(string email, Guid tenantGuid, Message message)
        {
            var result = new ResultDto<string>();
            try
            {
                if (tenantGuid == null || tenantGuid == Guid.Empty || string.IsNullOrEmpty(email) || message == null 
                    || string.IsNullOrEmpty(message.Payload) || string.IsNullOrEmpty(message.Type))
                {
                    logger.LogError($"Invalid arguments on method {nameof(SendMessageToUserAsync)}.");
                    result.ErrorMessage = $"Invalid arguments on method {nameof(SendMessageToUserAsync)}.";
                    result.ResultStatus = Contracts.Enums.ResultStatus.ArgumentsInvalid;
                    return result;
                }
                var user = await userRepository.GetUserByTenantGuidAndEmailAsync(tenantGuid, email);
                if (user == null)
                {
                    logger.LogInformation($"User not found {nameof(SendMessageToUserAsync)}.");
                    result.ResultStatus = Contracts.Enums.ResultStatus.NotFound;
                    return result;
                }
                await hubContext.Clients.Client(user.ConnectionId).BroadcastMessage(message.Type, message.Payload);
                logger.LogInformation($"Message sent to {user.Email} {nameof(SendMessageToUserAsync)}");
                result.Data = $"The message has been sent!";
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
