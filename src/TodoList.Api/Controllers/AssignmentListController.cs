using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TodoList.Api.Responses;
using TodoList.Application.Contracts.Services;
using TodoList.Application.DTOs.Assignment;
using TodoList.Application.DTOs.AssignmentList;
using TodoList.Application.DTOs.Paged;
using TodoList.Application.Notifications;

namespace TodoList.Api.Controllers;

[Authorize]
public class AssignmentListController : MainController
{
    private readonly IAssignmentListService _assignmentListService;

    public AssignmentListController(INotificator notificator, IAssignmentListService assignmentListService)
        : base(notificator)
    {
        _assignmentListService = assignmentListService;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create a new list", Tags = new[] { "Lists" })]
    [ProducesResponseType(typeof(AssignmentListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] CreateAssignmentListDto dto)
    {
        var assignmentList = await _assignmentListService.Create(dto);
        return CustomResponse(assignmentList);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update a list", Tags = new[] { "Lists" })]
    [ProducesResponseType(typeof(AssignmentListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAssignmentListDto dto)
    {
        var assignmentList = await _assignmentListService.Update(id, dto);
        return CustomResponse(assignmentList);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete a list", Tags = new[] { "Lists" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _assignmentListService.Delete(id);
        return CustomResponse();
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get a list by id", Tags = new[] { "Lists" })]
    [ProducesResponseType(typeof(AssignmentListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var assignmentList = await _assignmentListService.GetById(id);
        return CustomResponse(assignmentList);
    }

    [HttpGet("search")]
    [SwaggerOperation(Summary = "Search lists", Tags = new[] { "Lists" })]
    [ProducesResponseType(typeof(PagedDto<AssignmentListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<PagedDto<AssignmentListDto>> Search([FromQuery] SearchAssignmentListDto dto)
    {
        return await _assignmentListService.Search(dto);
    }

    [HttpGet("search-tasks/{id}")]
    [SwaggerOperation(Summary = "Search for tasks in a list", Tags = new[] { "Lists" })]
    [ProducesResponseType(typeof(IEnumerable<AssignmentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SearchAssignments(int id, [FromQuery] SearchAssignmentDto dto)
    {
        var assignments = await _assignmentListService.SearchAssignments(id, dto);
        return CustomResponse(assignments);
    }
}