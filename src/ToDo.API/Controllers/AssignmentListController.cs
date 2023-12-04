using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ToDo.API.Responses;
using ToDo.Application.Contracts;
using ToDo.Application.DTOs.Assignment;
using ToDo.Application.DTOs.AssignmentList;
using ToDo.Application.DTOs.Paged;
using ToDo.Application.Notifications;

namespace ToDo.API.Controllers;

[Authorize]
[Route("assignmentList")]
public class AssignmentListController : MainController
{
    private readonly IAssignmentListService _assignmentListService;

    public AssignmentListController(
        INotificator notificator,
        IAssignmentListService assignmentListService) 
        : base(notificator)
    {
        _assignmentListService = assignmentListService;
    }
    
    [HttpPost("create")]
    [SwaggerOperation(Summary = "Create a to-do list")]
    [ProducesResponseType(typeof(AssignmentListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateAssignmentListDto dto)
    {
        var createAssignmentList = await _assignmentListService.Create(dto);
        return CreatedResponse("", createAssignmentList);
    }

    [HttpPut("update/{id}")]
    [SwaggerOperation(Summary = "Update a to-do list")]
    [ProducesResponseType(typeof(AssignmentListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAssignmentListDto inputModel)
    {
        var updateAssignmentList = await _assignmentListService.Update(id, inputModel);
        return OkResponse(updateAssignmentList);
    }

    [HttpDelete("delete/{id}")]
    [SwaggerOperation(Summary = "Delete a to-do list")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(NotFoundResult) ,StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _assignmentListService.Delete(id);
        return NoContentResponse();
    }

    [HttpGet("get-by-id/{id}")]
    [SwaggerOperation(Summary = "Get by ID a to-do list")]
    [ProducesResponseType(typeof(AssignmentListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var getAssignmentList = await _assignmentListService.GetById(id);
        return OkResponse(getAssignmentList);
    }
    
    [HttpGet("search")]
    [SwaggerOperation(Summary = "Search to-do lists")]
    [ProducesResponseType(typeof(PagedDto<AssignmentListDto>), StatusCodes.Status200OK)]
    public async Task<PagedDto<AssignmentListDto>> Search([FromQuery] SearchAssignmentListDto dto)
    {
        return await _assignmentListService.Search(dto);
    }
    
    [HttpGet("search-tasks/{id}")]
    [SwaggerOperation(Summary = "Search for tasks in a to-do list")]
    [ProducesResponseType(typeof(IEnumerable<AssignmentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SearchAssignments(int id, [FromQuery] SearchAssignmentDto dto)
    {
        var getAssignment = await _assignmentListService.SearchAssignments(id, dto);
        return OkResponse(getAssignment);
    }
}