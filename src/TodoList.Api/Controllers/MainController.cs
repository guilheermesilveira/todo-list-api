using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TodoList.Api.Responses;
using TodoList.Application.Notifications;

namespace TodoList.Api.Controllers;

[ApiController]
[Route("[controller]")]
public abstract class MainController : ControllerBase
{
    private readonly INotificator _notificator;

    protected MainController(INotificator notificator)
    {
        _notificator = notificator;
    }

    protected ActionResult CustomResponse(object? result = null)
    {
        if (ValidOperation)
            return Ok(result);

        if (_notificator.IsNotFoundResource)
            return NotFound();

        return BadRequest(new BadRequestResponse
        {
            Errors = _notificator.GetNotifications().ToList()
        });
    }

    protected ActionResult CustomResponse(ModelStateDictionary modelState)
    {
        var errors = modelState.Values.SelectMany(e => e.Errors);
        foreach (var error in errors)
        {
            AddError(error.ErrorMessage);
        }

        return CustomResponse();
    }

    protected ActionResult CustomResponse(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
        {
            AddError(error.ErrorMessage);
        }

        return CustomResponse();
    }

    private bool ValidOperation => !(_notificator.HasNotification || _notificator.IsNotFoundResource);

    private void AddError(string error) => _notificator.Handle(error);
}