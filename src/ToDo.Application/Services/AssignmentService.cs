using AutoMapper;
using Microsoft.AspNetCore.Http;
using ToDo.Application.Contracts;
using ToDo.Application.DTOs.Assignment;
using ToDo.Application.DTOs.Paged;
using ToDo.Application.Extension;
using ToDo.Application.Notifications;
using ToDo.Domain.Contracts.Repositories;
using ToDo.Domain.Filter;
using ToDo.Domain.Models;

namespace ToDo.Application.Services;

public class AssignmentService : BaseService, IAssignmentService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IAssignmentListRepository _assignmentListRepository;

    public AssignmentService(
        IMapper mapper,
        INotificator notificator,
        IHttpContextAccessor httpContextAccessor,
        IAssignmentRepository assignmentRepository,
        IAssignmentListRepository assignmentListRepository) 
        : base(mapper, notificator)
    {
        _httpContextAccessor = httpContextAccessor;
        _assignmentRepository = assignmentRepository;
        _assignmentListRepository = assignmentListRepository;
    }
    
    public async Task<AssignmentDto?> Create(CreateAssignmentDto dto)
    {
        var assignment = Mapper.Map<Assignment>(dto);
        assignment.UserId = _httpContextAccessor.GetUserId() ?? 0;

        if (!await Validate(assignment)) 
            return null;

        _assignmentRepository.Create(assignment);

        if (await _assignmentRepository.UnitOfWork.Commit()) 
            return Mapper.Map<AssignmentDto>(assignment);
    
        Notificator.Handle("Não foi possível criar a tarefa.");
        return null;
    }
    
    public async Task<AssignmentDto?> Update(int id, UpdateAssignmentDto dto)
    {
        if (id != dto.Id)
        {
            Notificator.Handle("Os IDs não conferem.");
            return null;
        }

        var getAssignment = await _assignmentRepository.GetById(id, _httpContextAccessor.GetUserId());
        if (getAssignment == null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }

        Mapper.Map(dto, getAssignment);

        if (!await Validate(getAssignment)) 
            return null;
        
        _assignmentRepository.Update(getAssignment);

        if (await _assignmentRepository.UnitOfWork.Commit()) 
            return Mapper.Map<AssignmentDto>(getAssignment);

        Notificator.Handle("Não foi possível atualizar a tarefa.");
        return null;
    }

    public async Task Delete(int id)
    {
        var getAssignment = await _assignmentRepository.GetById(id, _httpContextAccessor.GetUserId());
        if (getAssignment == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        _assignmentRepository.Delete(getAssignment);

        if (!await _assignmentRepository.UnitOfWork.Commit()) 
            Notificator.Handle("Não foi possível deletar a tarefa.");
    }
    
    public async Task MarkConcluded(int id)
    {
        var getAssignment = await _assignmentRepository.GetById(id, _httpContextAccessor.GetUserId());
        if (getAssignment == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        getAssignment.SetConcluded();

        _assignmentRepository.Update(getAssignment);

        if (!await _assignmentRepository.UnitOfWork.Commit()) 
            Notificator.Handle("Não foi possível marcar a tarefa como concluída.");
    }

    public async Task MarkDesconcluded(int id)
    {
        var getAssignment = await _assignmentRepository.GetById(id, _httpContextAccessor.GetUserId());
        if (getAssignment == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        getAssignment.SetUnconcluded();

        _assignmentRepository.Update(getAssignment);

        if (!await _assignmentRepository.UnitOfWork.Commit()) 
            Notificator.Handle("Não foi possível marcar a tarefa como não concluída.");
    }
    
    public async Task<AssignmentDto?> GetById(int id)
    {
        var getAssignment = await _assignmentRepository.GetById(id, _httpContextAccessor.GetUserId());
        if (getAssignment != null) 
            return Mapper.Map<AssignmentDto>(getAssignment);

        Notificator.HandleNotFoundResource();
        return null;
    }
    
    public async Task<PagedDto<AssignmentDto>> Search(SearchAssignmentDto dto)
    {
        var filter = Mapper.Map<AssignmentFilter>(dto);
        
        var result = await _assignmentRepository.Search(_httpContextAccessor.GetUserId(), dto.AssignmentListId, filter, 
            dto.PerPage, dto.Page);

        return new PagedDto<AssignmentDto>
        {
            Items = Mapper.Map<List<AssignmentDto>>(result.Items),
            Total = result.Total,
            Page = result.Page,
            PerPage = result.PerPage,
            PageCount = result.PageCount
        };
    }
    
    private async Task<bool> Validate(Assignment assignment)
    {
        if (!assignment.Validate(out var validationResult)) 
            Notificator.Handle(validationResult.Errors);
        
        var getAssignmentList = await _assignmentListRepository.FirstOrDefault(x => 
            x.Id == assignment.AssignmentListId);
        
        if (getAssignmentList == null) 
            Notificator.Handle("Não existe essa lista de tarefas.");
        
        var getAssignment = await _assignmentRepository.FirstOrDefault(x => 
            x.Description == assignment.Description && x.AssignmentListId == assignment.AssignmentListId);

        if (getAssignment != null) 
            Notificator.Handle("Já existe uma tarefa cadastrada com essa descrição nessa lista.");

        return !Notificator.HasNotification;
    }
}