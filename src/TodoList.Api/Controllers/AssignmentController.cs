using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TodoList.Api.Responses;
using TodoList.Application.Contracts.Services;
using TodoList.Application.DTOs.Assignment;
using TodoList.Application.DTOs.Paged;
using TodoList.Application.Notifications;

namespace TodoList.Api.Controllers;

[Authorize]
public class AssignmentController : MainController
{
    private readonly IAssignmentService _assignmentService;

    public AssignmentController(INotificator notificator, IAssignmentService assignmentService) : base(notificator)
    {
        _assignmentService = assignmentService;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create a new task", Tags = new[] { "Tasks" })]
    [ProducesResponseType(typeof(AssignmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] CreateAssignmentDto dto)
    {
        var assignment = await _assignmentService.Create(dto);
        return CustomResponse(assignment);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update a task", Tags = new[] { "Tasks" })]
    [ProducesResponseType(typeof(AssignmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAssignmentDto dto)
    {
        var assignment = await _assignmentService.Update(id, dto);
        return CustomResponse(assignment);
    }

    [HttpPatch("conclude/{id}")]
    [SwaggerOperation(Summary = "Conclude a task", Tags = new[] { "Tasks" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkConclude(int id)
    {
        await _assignmentService.MarkConclude(id);
        return CustomResponse();
    }

    [HttpPatch("not-conclude/{id}")]
    [SwaggerOperation(Summary = "Not conclude a task", Tags = new[] { "Tasks" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkNotConclude(int id)
    {
        await _assignmentService.MarkNotConclude(id);
        return CustomResponse();
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete a task", Tags = new[] { "Tasks" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _assignmentService.Delete(id);
        return CustomResponse();
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get a task by id", Tags = new[] { "Tasks" })]
    [ProducesResponseType(typeof(AssignmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var assignment = await _assignmentService.GetById(id);
        return CustomResponse(assignment);
    }

    [HttpGet("search")]
    [SwaggerOperation(Summary = "Search tasks", Tags = new[] { "Tasks" })]
    [ProducesResponseType(typeof(PagedDto<AssignmentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<PagedDto<AssignmentDto>> Search([FromQuery] SearchAssignmentDto dto)
    {
        return await _assignmentService.Search(dto);
    }
}