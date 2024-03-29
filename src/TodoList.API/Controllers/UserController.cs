﻿using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TodoList.API.Responses;
using TodoList.Application.Contracts.Services;
using TodoList.Application.DTOs.User;
using TodoList.Application.Notifications;

namespace TodoList.API.Controllers;

public class UserController : MainController
{
    private readonly IUserService _userService;

    public UserController(INotificator notificator, IUserService userService) : base(notificator)
    {
        _userService = userService;
    }

    [HttpPost("Register")]
    [SwaggerOperation(Summary = "Register a user", Tags = new[] { "User - Auth" })]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        var registerUser = await _userService.Register(dto);
        return CustomResponse(registerUser);
    }

    [HttpPost("Login")]
    [SwaggerOperation(Summary = "User login", Tags = new[] { "User - Auth" })]
    [ProducesResponseType(typeof(UserTokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
    {
        var token = await _userService.Login(dto);
        return CustomResponse(token);
    }
}