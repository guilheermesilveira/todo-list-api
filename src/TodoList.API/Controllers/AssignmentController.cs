using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TodoList.Application.Contracts.Services;
using TodoList.Application.DTOs.Assignment;
using TodoList.Application.DTOs.Paged;
using TodoList.Application.Notifications;

namespace TodoList.API.Controllers;

[Authorize]
public class AssignmentController : MainController
{
    private readonly IAssignmentService _assignmentService;

    public AssignmentController(INotificator notificator, IAssignmentService assignmentService) : base(notificator)
    {
        _assignmentService = assignmentService;
    }

    [HttpPost]
    [SwaggerOperation("Create a task")]
    [ProducesResponseType(typeof(AssignmentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateAssignmentDto dto)
    {
        var createAssignment = await _assignmentService.Create(dto);
        return CustomResponse(createAssignment);
    }

    [HttpPut("{id}")]
    [SwaggerOperation("Update a task")]
    [ProducesResponseType(typeof(AssignmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAssignmentDto dto)
    {
        var updateAssignment = await _assignmentService.Update(id, dto);
        return CustomResponse(updateAssignment);
    }

    [HttpPatch("Conclude/{id}")]
    [SwaggerOperation("Conclude a task")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkConclude(int id)
    {
        await _assignmentService.MarkConclude(id);
        return CustomResponse();
    }

    [HttpPatch("NotConclude/{id}")]
    [SwaggerOperation("Not conclude a task")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkNotConclude(int id)
    {
        await _assignmentService.MarkNotConclude(id);
        return CustomResponse();
    }

    [HttpDelete("{id}")]
    [SwaggerOperation("Delete a task")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _assignmentService.Delete(id);
        return CustomResponse();
    }

    [HttpGet("{id}")]
    [SwaggerOperation("Get by id a task")]
    [ProducesResponseType(typeof(AssignmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var getAssignment = await _assignmentService.GetById(id);
        return CustomResponse(getAssignment);
    }

    [HttpGet]
    [SwaggerOperation("Search tasks")]
    [ProducesResponseType(typeof(PagedDto<AssignmentDto>), StatusCodes.Status200OK)]
    public async Task<PagedDto<AssignmentDto>> Search([FromQuery] SearchAssignmentDto dto)
    {
        return await _assignmentService.Search(dto);
    }
}