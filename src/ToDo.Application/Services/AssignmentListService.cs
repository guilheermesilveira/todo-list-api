using AutoMapper;
using Microsoft.AspNetCore.Http;
using ToDo.Application.Contracts;
using ToDo.Application.DTOs.Assignment;
using ToDo.Application.DTOs.AssignmentList;
using ToDo.Application.DTOs.Paged;
using ToDo.Application.Extension;
using ToDo.Application.Notifications;
using ToDo.Domain.Contracts.Repositories;
using ToDo.Domain.Filter;
using ToDo.Domain.Models;

namespace ToDo.Application.Services;

public class AssignmentListService : BaseService, IAssignmentListService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAssignmentListRepository _assignmentListRepository;
    private readonly IAssignmentRepository _assignmentRepository;

    public AssignmentListService(
        IMapper mapper,
        INotificator notificator,
        IHttpContextAccessor httpContextAccessor,
        IAssignmentListRepository assignmentListRepository,
        IAssignmentRepository assignmentRepository) 
        : base(mapper, notificator)
    {
        _httpContextAccessor = httpContextAccessor;
        _assignmentListRepository = assignmentListRepository;
        _assignmentRepository = assignmentRepository;
    }
    
    public async Task<AssignmentListDto?> Create(CreateAssignmentListDto dto)
    {
        var assignmentList = Mapper.Map<AssignmentList>(dto);
        assignmentList.UserId = _httpContextAccessor.GetUserId() ?? 0;

        if (!await Validate(assignmentList)) 
            return null;

        _assignmentListRepository.Create(assignmentList);

        if (await _assignmentListRepository.UnitOfWork.Commit()) 
            return Mapper.Map<AssignmentListDto>(assignmentList);
        
        Notificator.Handle("Não foi possível criar a lista de tarefas.");
        return null;
    }

    public async Task<AssignmentListDto?> Update(int id, UpdateAssignmentListDto dto)
    {
        if (id != dto.Id)
        {
            Notificator.Handle("Os IDs não conferem.");
            return null;
        }

        var getAssignmentList = await _assignmentListRepository.GetById(id, _httpContextAccessor.GetUserId());
        if (getAssignmentList == null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }

        Mapper.Map(dto, getAssignmentList);

        if (!await Validate(getAssignmentList)) 
            return null;
        
        _assignmentListRepository.Update(getAssignmentList);

        if (await _assignmentListRepository.UnitOfWork.Commit()) 
            return Mapper.Map<AssignmentListDto>(getAssignmentList);
        
        Notificator.Handle("Não foi possível atualizar a lista de tarefas.");
        return null;
    }

    public async Task Delete(int id)
    {
        var getAssignmentList = await _assignmentListRepository.GetById(id, _httpContextAccessor.GetUserId());
        if (getAssignmentList == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        if (getAssignmentList.Assignments.Any(x => !x.Concluded))
        {
            Notificator.Handle("Não é possível deletar uma lista com tarefas não concluídas.");
            return;
        }

        _assignmentListRepository.Delete(getAssignmentList);

        if (!await _assignmentListRepository.UnitOfWork.Commit()) 
            Notificator.Handle("Não foi possível deletar a lista de tarefas.");
    }
    
    public async Task<AssignmentListDto?> GetById(int id)
    {
        var getAssignmentList = await _assignmentListRepository.GetById(id, _httpContextAccessor.GetUserId());
        if (getAssignmentList != null) 
            return Mapper.Map<AssignmentListDto>(getAssignmentList);
        
        Notificator.HandleNotFoundResource();
        return null;
    }
    
    public async Task<PagedDto<AssignmentListDto>> Search(SearchAssignmentListDto dto)
    {
        var result = await _assignmentListRepository.Search(_httpContextAccessor.GetUserId(), dto.Name, dto.PerPage, 
            dto.Page);

        return new PagedDto<AssignmentListDto>
        {
            Items = Mapper.Map<List<AssignmentListDto>>(result.Items),
            Total = result.Total,
            Page = result.Page,
            PerPage = result.PerPage,
            PageCount = result.PageCount
        };
    }

    public async Task<PagedDto<AssignmentDto>?> SearchAssignments(int id, SearchAssignmentDto dto)
    {
        if (id != dto.AssignmentListId)
        {
            Notificator.Handle("Os IDs não conferem.");
            return null;
        }
        
        var httpAccessor = _httpContextAccessor.GetUserId();
        var filter = Mapper.Map<AssignmentFilter>(dto);

        var getAssignmentList = await _assignmentListRepository.GetById(id, httpAccessor);
        if (getAssignmentList == null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }

        var result = await _assignmentRepository.Search(httpAccessor, id, filter, dto.PerPage, dto.Page);

        return new PagedDto<AssignmentDto>
        {
            Items = Mapper.Map<List<AssignmentDto>>(result.Items),
            Total = result.Total,
            Page = result.Page,
            PerPage = result.PerPage,
            PageCount = result.PageCount
        };
    }

    private async Task<bool> Validate(AssignmentList assignmentList)
    {
        if (!assignmentList.Validate(out var validationResult)) 
            Notificator.Handle(validationResult.Errors);
        
        var getAssignmentList = await _assignmentListRepository.FirstOrDefault(x =>
            x.Name == assignmentList.Name);

        if (getAssignmentList != null) 
            Notificator.Handle("Já existe uma lista de tarefas com esse nome.");

        return !Notificator.HasNotification;
    }
}