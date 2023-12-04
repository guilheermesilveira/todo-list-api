using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ToDo.API.Responses;
using ToDo.Application.Contracts;
using ToDo.Application.DTOs.User;
using ToDo.Application.Notifications;

namespace ToDo.API.Controllers;

[Route("user")]
public class UserController : MainController
{
    private readonly IUserService _userService;

    public UserController(INotificator notificator, IUserService userService) : base(notificator)
    {
        _userService = userService;
    }
    
    [HttpPost("create")]
    [SwaggerOperation(Summary = "Create a user")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
    {
        var createUser = await _userService.Create(dto);
        return CreatedResponse("", createUser);
    }

    [HttpPost("login")]
    [SwaggerOperation(Summary = "User login")]
    [ProducesResponseType(typeof(UserTokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
    {
        var token = await _userService.Login(dto);
        return OkResponse(token);
    }
}