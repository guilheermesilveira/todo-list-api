using TodoList.Application.DTOs.Auth;
using TodoList.Application.DTOs.User;

namespace TodoList.Application.Contracts.Services;

public interface IAuthService
{
    Task<UserDto?> Register(RegisterUserDto dto);
    Task<TokenDto?> Login(LoginDto dto);
}