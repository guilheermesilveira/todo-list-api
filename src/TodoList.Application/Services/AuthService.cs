using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TodoList.Application.Configurations;
using TodoList.Application.Contracts.Services;
using TodoList.Application.DTOs.Auth;
using TodoList.Application.DTOs.User;
using TodoList.Application.Notifications;
using TodoList.Application.Validations.Auth;
using TodoList.Domain.Contracts.Repositories;
using TodoList.Domain.Models;

namespace TodoList.Application.Services;

public class AuthService : IAuthService
{
    private readonly IMapper _mapper;
    private readonly INotificator _notificator;
    private readonly JwtSettings _jwtSettings;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IUserRepository _userRepository;

    public AuthService(IMapper mapper, INotificator notificator, IOptions<JwtSettings> jwtSettings,
        IPasswordHasher<User> passwordHasher, IUserRepository userRepository)
    {
        _mapper = mapper;
        _notificator = notificator;
        _jwtSettings = jwtSettings.Value;
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
    }

    public async Task<UserDto?> Register(RegisterUserDto dto)
    {
        if (!await ValidationsToRegisterUser(dto))
            return null;

        var registerUser = _mapper.Map<User>(dto);
        registerUser.Password = _passwordHasher.HashPassword(registerUser, dto.Password);

        _userRepository.Register(registerUser);
        return await CommitChanges() ? _mapper.Map<UserDto>(registerUser) : null;
    }

    public async Task<TokenDto?> Login(LoginDto dto)
    {
        if (!await ValidationsForLogin(dto))
            return null;

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

    private async Task<bool> ValidationsToRegisterUser(RegisterUserDto dto)
    {
        var user = _mapper.Map<User>(dto);
        var userValidator = new RegistrationValidator();

        var validationResult = await userValidator.ValidateAsync(user);
        if (!validationResult.IsValid)
        {
            _notificator.Handle(validationResult.Errors);
            return false;
        }

        var existingUserByEmail = await _userRepository.GetByEmail(user.Email);
        if (existingUserByEmail != null)
        {
            _notificator.Handle("Já existe um usuário cadastrado com o email informado.");
            return false;
        }

        return true;
    }

    private async Task<bool> ValidationsForLogin(LoginDto dto)
    {
        var user = _mapper.Map<User>(dto);
        var userValidator = new LoginValidator();

        var validationResult = await userValidator.ValidateAsync(user);
        if (!validationResult.IsValid)
        {
            _notificator.Handle(validationResult.Errors);
            return false;
        }

        return true;
    }

    private TokenDto GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.Role, "User"),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddHours(_jwtSettings.HoursToExpire),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        });

        var encodedToken = tokenHandler.WriteToken(token);

        return new TokenDto
        {
            Token = encodedToken
        };
    }

    private async Task<bool> CommitChanges()
    {
        if (await _userRepository.UnitOfWork.Commit())
            return true;

        _notificator.Handle("Ocorreu um erro ao salvar as alterações.");
        return false;
    }
}