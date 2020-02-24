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

        public async Task<ResultDto<List<User>>> GetUsersAsync()
        {
            var result = new ResultDto<List<User>>();
            try
            {
                result.Data = await userRepository.GetUsersAsync();
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving Users. EX: {ex}");
                result.ErrorMessage = $"Error retrieving Users. EX: {ex}";
                result.Data = null;
                result.ResultStatus = Contracts.Enums.ResultStatus.Error;
            }
            return result;
        }

        public async Task<ResultDto<User>> GetUserAsync(Guid userId)
        {
            var result = new ResultDto<User>();
            try
            {
                result.Data = await userRepository.GetUserAsync(userId);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving Users. EX: {ex}");
                result.ErrorMessage = $"Error retrieving Users. EX: {ex}";
                result.Data = null;
                result.ResultStatus = Contracts.Enums.ResultStatus.Error;
            }
            return result;
        }

        public async Task<ResultDto<string>> PostUserAsync(UserDto userDto)
        {
            var result = new ResultDto<string>();
            try
            {
                var user = new User
                {
                    Email = userDto.Email,
                    TenantGuid = userDto.TenantGuid,
                    TenantType = userDto.TenantType,
                    ConnectionId = userDto.ConnectionId
                };
                await userRepository.AddUserAsyc(user);
                result.Data = "Succeeded";
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving Users. EX: {ex}");
                result.ErrorMessage = $"Error retrieving Users. EX: {ex}";
                result.ResultStatus = Contracts.Enums.ResultStatus.Error;
            }
            return result;
        }

        public async Task<ResultDto<string>> PutUserAsync(UserDto userDto, Guid userId)
        {
            var result = new ResultDto<string>();
            try
            {
                var user = new User
                {
                    Id = userId,
                    Email = userDto.Email,
                    TenantGuid = userDto.TenantGuid,
                    TenantType = userDto.TenantType,
                    ConnectionId = userDto.ConnectionId
                };
                result.Data = CheckIfUserExists(await userRepository.UpdateUserAsync(user));
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving Users. EX: {ex}");
                result.ErrorMessage = $"Error retrieving Users. EX: {ex}";
                result.ResultStatus = Contracts.Enums.ResultStatus.Error;
            }
            return result;
        }

        public async Task<ResultDto<string>> RemoveUserAsync(Guid userId)
        {
            var result = new ResultDto<string>();
            try
            {
                result.Data = CheckIfUserExists(await userRepository.DeleteUserAsync(userId));
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving Users. EX: {ex}");
                result.ErrorMessage = $"Error retrieving Users. EX: {ex}";
                result.ResultStatus = Contracts.Enums.ResultStatus.Error;
            }
            return result;
        }

        private string CheckIfUserExists(bool value)
        {
            return value ? "Succeeded" : "Invalid user id";
        }
    }
}
