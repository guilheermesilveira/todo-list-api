using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ToDo.API.Responses;
using ToDo.Application.Notifications;

namespace ToDo.API.Controllers;

[ApiController]
public abstract class MainController : ControllerBase
{
    private readonly INotificator _notificator;
    
    protected MainController(INotificator notificator)
    {
        _notificator = notificator;
    }
    
    protected IActionResult NoContentResponse() 
        => CustomResponse(NoContent());

    protected IActionResult CreatedResponse(string uri = "", object? result = null) 
        => CustomResponse(Created(uri, result));

    protected IActionResult OkResponse(object? result = null) 
        => CustomResponse(Ok(result));
    
    protected IActionResult CustomResponse(IActionResult objectResult)
    {
        if (ValidOperation) 
            return objectResult;
        
        if (_notificator.IsNotFoundResource) 
            return NotFound();

        var response = new BadRequestResponse(_notificator.GetNotifications().ToList());
        return BadRequest(response);
    }
    
    protected IActionResult CustomResponse(ModelStateDictionary modelState)
    {
        var errors = modelState.Values.SelectMany(e => e.Errors);
        foreach (var error in errors) 
            AddProcessingError(error.ErrorMessage);
        
        return CustomResponse(Ok(null));
    }

    protected IActionResult CustomResponse(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors) 
            AddProcessingError(error.ErrorMessage);

        return CustomResponse(Ok(null));
    }
    
    private bool ValidOperation => !(_notificator.HasNotification || _notificator.IsNotFoundResource);

    private void AddProcessingError(string error) => _notificator.Handle(error);
}