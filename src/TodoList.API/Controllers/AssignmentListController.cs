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
    [SwaggerOperation(Summary = "Cria uma nova lista de tarefas.", Tags = new[] { "Lista de tarefas" })]
    [ProducesResponseType(typeof(AssignmentListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] CreateAssignmentListDto dto)
    {
        var createAssignmentList = await _assignmentListService.Create(dto);
        return CustomResponse(createAssignmentList);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Atualiza uma lista de tarefas existente.", Tags = new[] { "Lista de tarefas" })]
    [ProducesResponseType(typeof(AssignmentListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAssignmentListDto dto)
    {
        var updateAssignmentList = await _assignmentListService.Update(id, dto);
        return CustomResponse(updateAssignmentList);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Deleta uma lista de tarefas.", Tags = new[] { "Lista de tarefas" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _assignmentListService.Delete(id);
        return CustomResponse();
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Obtém uma lista de tarefas pelo id.", Tags = new[] { "Lista de tarefas" })]
    [ProducesResponseType(typeof(AssignmentListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var getAssignmentList = await _assignmentListService.GetById(id);
        return CustomResponse(getAssignmentList);
    }

    [HttpGet("Buscar")]
    [SwaggerOperation(Summary = "Busca por listas de tarefas.", Tags = new[] { "Lista de tarefas" })]
    [ProducesResponseType(typeof(PagedDto<AssignmentListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<PagedDto<AssignmentListDto>> Search([FromQuery] SearchAssignmentListDto dto)
    {
        return await _assignmentListService.Search(dto);
    }

    [HttpGet("Buscar-Tarefas/{id}")]
    [SwaggerOperation(Summary = "Busca por tarefas em uma lista específica pelo id da lista.", Tags = new[] { "Lista de tarefas" })]
    [ProducesResponseType(typeof(IEnumerable<AssignmentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SearchAssignments(int id, [FromQuery] SearchAssignmentDto dto)
    {
        var getAssignments = await _assignmentListService.SearchAssignments(id, dto);
        return CustomResponse(getAssignments);
    }
}