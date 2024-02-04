using TodoList.Application.DTOs.User;

namespace TodoList.Application.Contracts.Services;

public interface IUserService
{
    Task<UserDto?> Create(CreateUserDto dto);
    Task<UserTokenDto?> Login(UserLoginDto dto);
}