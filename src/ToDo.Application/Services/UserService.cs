using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ToDo.Application.Contracts;
using ToDo.Application.DTOs.User;
using ToDo.Application.Notifications;
using ToDo.Domain.Contracts.Repositories;
using ToDo.Domain.Models;

namespace ToDo.Application.Services;

public class UserService : BaseService, IUserService
{
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;

    public UserService(
        IMapper mapper,
        INotificator notificator,
        IPasswordHasher<User> passwordHasher,
        IConfiguration configuration, 
        IUserRepository userRepository) 
        : base(mapper, notificator)
    {
        _passwordHasher = passwordHasher;
        _configuration = configuration;
        _userRepository = userRepository;
    }
    
    public async Task<UserDto?> Create(CreateUserDto dto)
    {
        if (!dto.Validate(out var validationResult))
        {
            Notificator.Handle(validationResult.Errors);
            return null;
        }

        var getUser = await _userRepository.GetByEmail(dto.Email);
        if (getUser != null)
        {
            Notificator.Handle("Já existe um usuário cadastrado com o email informado.");
            return null;
        }
        
        var user = Mapper.Map<User>(dto);

        user.Password = _passwordHasher.HashPassword(user, dto.Password);

        _userRepository.Create(user);

        if (await _userRepository.UnitOfWork.Commit()) 
            return Mapper.Map<UserDto>(user);

        Notificator.Handle("Não foi possível cadastrar o usuário.");
        return null;
    }
    
    public async Task<UserTokenDto?> Login(UserLoginDto dto)
    {
        if (!dto.Validate(out var validationResult))
        {
            Notificator.Handle(validationResult.Errors);
            return null;
        }

        var user = await _userRepository.GetByEmail(dto.Email);

        if (user != null && _passwordHasher.VerifyHashedPassword(user, user.Password,
                dto.Password) == PasswordVerificationResult.Success)
        {
            return GenerateToken(user);
        }

        Notificator.Handle("Email ou senha estão incorretos.");
        return null;
    }
    
    private UserTokenDto GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_configuration["AppSettings:Secret"] ?? string.Empty);

        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.Role, "User"),
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
            }),
            SigningCredentials = 
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Expires = 
                DateTime.UtcNow.AddHours(int.Parse(_configuration["AppSettings:ExpirationHours"] ?? string.Empty)),
            Issuer = _configuration["AppSettings:Issuer"],
            Audience = _configuration["AppSettings:ValidOn"]
        });

        var encodedToken = tokenHandler.WriteToken(token);

        return new UserTokenDto
        {
            Token = encodedToken
        };
    }
}