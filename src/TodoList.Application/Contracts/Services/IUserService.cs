using TodoList.Application.DTOs.User;

namespace TodoList.Application.Contracts.Services;

public interface IUserService
{
    Task<UserDto?> Register(RegisterUserDto dto);
    Task<UserTokenDto?> Login(UserLoginDto dto);
}