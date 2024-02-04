using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TodoList.Application.Configurations;
using TodoList.Application.Contracts.Services;
using TodoList.Application.DTOs.User;
using TodoList.Application.Notifications;
using TodoList.Domain.Contracts.Repositories;
using TodoList.Domain.Models;

namespace TodoList.Application.Services;

public class UserService : IUserService
{
    private readonly AppSettings _appSettings;
    private readonly IMapper _mapper;
    private readonly INotificator _notificator;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IUserRepository _userRepository;

    public UserService(IOptions<AppSettings> appSettings, IMapper mapper, INotificator notificator,
        IPasswordHasher<User> passwordHasher, IUserRepository userRepository)
    {
        _appSettings = appSettings.Value;
        _mapper = mapper;
        _notificator = notificator;
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
    }

    public async Task<UserDto?> Create(CreateUserDto dto)
    {
        if (!dto.Validate(out var validationResult))
        {
            _notificator.Handle(validationResult.Errors);
            return null;
        }

        var getUser = await _userRepository.GetByEmail(dto.Email);
        if (getUser != null)
        {
            _notificator.Handle("Já existe um usuário cadastrado com o email informado.");
            return null;
        }

        var createUser = _mapper.Map<User>(dto);
        createUser.Password = _passwordHasher.HashPassword(createUser, dto.Password);

        _userRepository.Create(createUser);

        if (await _userRepository.UnitOfWork.Commit())
            return _mapper.Map<UserDto>(createUser);

        _notificator.Handle("Não foi possível cadastrar o usuário.");
        return null;
    }

    public async Task<UserTokenDto?> Login(UserLoginDto dto)
    {
        if (!dto.Validate(out var validationResult))
        {
            _notificator.Handle(validationResult.Errors);
            return null;
        }

        var user = await _userRepository.GetByEmail(dto.Email);
        if (user == null)
        {
            _notificator.Handle("Email e/ou senha incorretos.");
            return null;
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);
        if (result == PasswordVerificationResult.Success)
            return GenerateToken(user);

        _notificator.Handle("Email e/ou senha incorretos.");
        return null;
    }

    private UserTokenDto GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.Role, "User"),
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
            }),
            Issuer = _appSettings.Issuer,
            Audience = _appSettings.Audience,
            Expires = DateTime.UtcNow.AddHours(_appSettings.TokenExpiration),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.Secret)),
                    SecurityAlgorithms.HmacSha256Signature)
        });

        var encodedToken = tokenHandler.WriteToken(token);

        return new UserTokenDto
        {
            Token = encodedToken
        };
    }
}