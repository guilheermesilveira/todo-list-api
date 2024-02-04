using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TodoList.API.Responses;
using TodoList.Application.Contracts.Services;
using TodoList.Application.DTOs.Assignment;
using TodoList.Application.DTOs.AssignmentList;
using TodoList.Application.DTOs.Paged;
using TodoList.Application.Notifications;

namespace TodoList.API.Controllers;

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
    [SwaggerOperation("Create a to-do list")]
    [ProducesResponseType(typeof(AssignmentListDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateAssignmentListDto dto)
    {
        var createAssignmentList = await _assignmentListService.Create(dto);
        return CustomResponse(createAssignmentList);
    }

    [HttpPut("{id}")]
    [SwaggerOperation("Update a to-do list")]
    [ProducesResponseType(typeof(AssignmentListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAssignmentListDto inputModel)
    {
        var updateAssignmentList = await _assignmentListService.Update(id, inputModel);
        return CustomResponse(updateAssignmentList);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation("Delete a to-do list")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _assignmentListService.Delete(id);
        return CustomResponse();
    }

    [HttpGet("{id}")]
    [SwaggerOperation("Get by id a to-do list")]
    [ProducesResponseType(typeof(AssignmentListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var getAssignmentList = await _assignmentListService.GetById(id);
        return CustomResponse(getAssignmentList);
    }

    [HttpGet("SearchLists")]
    [SwaggerOperation("Search to-do lists")]
    [ProducesResponseType(typeof(PagedDto<AssignmentListDto>), StatusCodes.Status200OK)]
    public async Task<PagedDto<AssignmentListDto>> Search([FromQuery] SearchAssignmentListDto dto)
    {
        return await _assignmentListService.Search(dto);
    }

    [HttpGet("SearchTasks/{id}")]
    [SwaggerOperation("Search for tasks in a to-do list")]
    [ProducesResponseType(typeof(IEnumerable<AssignmentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SearchAssignments(int id, [FromQuery] SearchAssignmentDto dto)
    {
        var getAssignments = await _assignmentListService.SearchAssignments(id, dto);
        return CustomResponse(getAssignments);
    }
}