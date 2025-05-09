﻿using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TodoList.Api.Responses;
using TodoList.Application.Contracts.Services;
using TodoList.Application.DTOs.Auth;
using TodoList.Application.DTOs.User;
using TodoList.Application.Notifications;

namespace TodoList.Api.Controllers;

public class AuthController : MainController
{
    private readonly IAuthService _authService;

    public AuthController(INotificator notificator, IAuthService authService) : base(notificator)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [SwaggerOperation(Summary = "Register a new user", Tags = new[] { "Authentication" })]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        var user = await _authService.Register(dto);
        return CustomResponse(user);
    }

    [HttpPost("login")]
    [SwaggerOperation(Summary = "Auth", Tags = new[] { "Authentication" })]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var token = await _authService.Login(dto);
        return CustomResponse(token);
    }
}