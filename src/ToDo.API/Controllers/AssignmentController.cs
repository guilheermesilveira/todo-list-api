using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ToDo.Application.Contracts;
using ToDo.Application.DTOs.Assignment;
using ToDo.Application.DTOs.Paged;
using ToDo.Application.Notifications;

namespace ToDo.API.Controllers;

[Authorize]
[Route("assignment")]
public class AssignmentController : MainController
{
    private readonly IAssignmentService _assignmentService;

    public AssignmentController(INotificator notificator, IAssignmentService assignmentService) : base(notificator)
    {
        _assignmentService = assignmentService;
    }
    
    [HttpPost("create")]
    [SwaggerOperation(Summary = "Create a task")]
    [ProducesResponseType(typeof(AssignmentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateAssignmentDto dto)
    {
        var createAssignment = await _assignmentService.Create(dto);
        return CreatedResponse("", createAssignment);
    }
    
    [HttpPut("update/{id}")]
    [SwaggerOperation(Summary = "Update a task")]
    [ProducesResponseType(typeof(AssignmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAssignmentDto dto)
    {
        var updateAssignment = await _assignmentService.Update(id, dto);
        return OkResponse(updateAssignment);
    }
    
    [HttpDelete("delete/{id}")]
    [SwaggerOperation(Summary = "Delete a task")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _assignmentService.Delete(id);
        return NoContentResponse();
    }
    
    [HttpPatch("conclude/{id}")]
    [SwaggerOperation(Summary = "Conclud a task")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkConcluded(int id)
    {
        await _assignmentService.MarkConcluded(id);
        return NoContentResponse();
    }

    [HttpPatch("unconclude/{id}")]
    [SwaggerOperation(Summary = "Desconclud a task")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkDesconcluded(int id)
    { 
        await _assignmentService.MarkDesconcluded(id);
        return NoContentResponse();
    }
    
    [HttpGet("get-by-id/{id}")]
    [SwaggerOperation(Summary = "Get by ID a task")]
    [ProducesResponseType(typeof(AssignmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var getAssignment = await _assignmentService.GetById(id);
        return OkResponse(getAssignment);
    }

    [HttpGet("search")]
    [SwaggerOperation(Summary = "Search tasks")]
    [ProducesResponseType(typeof(PagedDto<AssignmentDto>), StatusCodes.Status200OK)]
    public async Task<PagedDto<AssignmentDto>> Search([FromQuery] SearchAssignmentDto dto)
    {
        return await _assignmentService.Search(dto);
    }
}