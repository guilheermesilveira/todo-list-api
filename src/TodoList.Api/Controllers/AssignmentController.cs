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
    [SwaggerOperation(Summary = "Cria uma nova tarefa.", Tags = new[] { "Tarefas" })]
    [ProducesResponseType(typeof(AssignmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] CreateAssignmentDto dto)
    {
        var createAssignment = await _assignmentService.Create(dto);
        return CustomResponse(createAssignment);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Atualiza uma tarefa existente.", Tags = new[] { "Tarefas" })]
    [ProducesResponseType(typeof(AssignmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAssignmentDto dto)
    {
        var updateAssignment = await _assignmentService.Update(id, dto);
        return CustomResponse(updateAssignment);
    }

    [HttpPatch("Concluir/{id}")]
    [SwaggerOperation(Summary = "Marca uma tarefa como concluída.", Tags = new[] { "Tarefas" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkConclude(int id)
    {
        await _assignmentService.MarkConclude(id);
        return CustomResponse();
    }

    [HttpPatch("Pendente/{id}")]
    [SwaggerOperation(Summary = "Marca uma tarefa como pendente.", Tags = new[] { "Tarefas" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkNotConclude(int id)
    {
        await _assignmentService.MarkNotConclude(id);
        return CustomResponse();
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Deleta uma tarefa.", Tags = new[] { "Tarefas" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _assignmentService.Delete(id);
        return CustomResponse();
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Obtém uma tarefa pelo id.", Tags = new[] { "Tarefas" })]
    [ProducesResponseType(typeof(AssignmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var getAssignment = await _assignmentService.GetById(id);
        return CustomResponse(getAssignment);
    }

    [HttpGet("Buscar")]
    [SwaggerOperation(Summary = "Busca tarefas.", Tags = new[] { "Tarefas" })]
    [ProducesResponseType(typeof(PagedDto<AssignmentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<PagedDto<AssignmentDto>> Search([FromQuery] SearchAssignmentDto dto)
    {
        return await _assignmentService.Search(dto);
    }
}