using ToDo.Application.DTOs.User;

namespace ToDo.Application.Contracts;

public interface IUserService
{
    Task<UserDto?> Create(CreateUserDto dto);
    Task<UserTokenDto?> Login(UserLoginDto dto);
}