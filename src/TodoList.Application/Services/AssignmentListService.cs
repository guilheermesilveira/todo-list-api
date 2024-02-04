using AutoMapper;
using Microsoft.AspNetCore.Http;
using TodoList.Application.Contracts.Services;
using TodoList.Application.DTOs.Assignment;
using TodoList.Application.DTOs.AssignmentList;
using TodoList.Application.DTOs.Paged;
using TodoList.Application.Extension;
using TodoList.Application.Notifications;
using TodoList.Domain.Contracts.Repositories;
using TodoList.Domain.Filter;
using TodoList.Domain.Models;

namespace TodoList.Application.Services;

public class AssignmentListService : IAssignmentListService
{
    private readonly IMapper _mapper;
    private readonly INotificator _notificator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IAssignmentListRepository _assignmentListRepository;

    public AssignmentListService(IMapper mapper, INotificator notificator, IHttpContextAccessor httpContextAccessor,
        IAssignmentRepository assignmentRepository, IAssignmentListRepository assignmentListRepository)
    {
        _mapper = mapper;
        _notificator = notificator;
        _httpContextAccessor = httpContextAccessor;
        _assignmentRepository = assignmentRepository;
        _assignmentListRepository = assignmentListRepository;
    }

    public async Task<AssignmentListDto?> Create(CreateAssignmentListDto dto)
    {
        if (!dto.Validate(out var validationResult))
        {
            _notificator.Handle(validationResult.Errors);
            return null;
        }

        var getAssignmentList = await _assignmentListRepository.FirstOrDefault(x => x.Name == dto.Name);
        if (getAssignmentList != null)
            _notificator.Handle("Já existe uma lista de tarefas com esse nome.");

        var createAssignmentList = _mapper.Map<AssignmentList>(dto);
        createAssignmentList.UserId = _httpContextAccessor.GetUserId() ?? 0;

        _assignmentListRepository.Create(createAssignmentList);
        return await CommitChanges() ? _mapper.Map<AssignmentListDto>(createAssignmentList) : null;
    }

    public async Task<AssignmentListDto?> Update(int id, UpdateAssignmentListDto dto)
    {
        var getAssignmentListById = await _assignmentListRepository.GetById(id, _httpContextAccessor.GetUserId());
        if (getAssignmentListById == null)
        {
            _notificator.HandleNotFoundResource();
            return null;
        }

        if (!dto.Validate(out var validationResult))
        {
            _notificator.Handle(validationResult.Errors);
            return null;
        }

        var getAssignmentListByName = await _assignmentListRepository.FirstOrDefault(x => x.Name == dto.Name);
        if (getAssignmentListByName != null)
            _notificator.Handle("Já existe uma lista de tarefas com esse nome.");

        var updateAssignmentList = _mapper.Map<AssignmentList>(dto);

        _assignmentListRepository.Update(updateAssignmentList);
        return await CommitChanges() ? _mapper.Map<AssignmentListDto>(updateAssignmentList) : null;
    }

    public async Task Delete(int id)
    {
        var getAssignmentList = await _assignmentListRepository.GetById(id, _httpContextAccessor.GetUserId());
        if (getAssignmentList == null)
        {
            _notificator.HandleNotFoundResource();
            return;
        }

        if (getAssignmentList.Assignments.Any(x => !x.Concluded))
        {
            _notificator.Handle("Não é possível deletar uma lista com tarefas não concluídas.");
            return;
        }

        _assignmentListRepository.Delete(getAssignmentList);
        await CommitChanges();
    }

    public async Task<AssignmentListDto?> GetById(int id)
    {
        var getAssignmentList = await _assignmentListRepository.GetById(id, _httpContextAccessor.GetUserId());
        if (getAssignmentList != null)
            return _mapper.Map<AssignmentListDto>(getAssignmentList);

        _notificator.HandleNotFoundResource();
        return null;
    }

    public async Task<PagedDto<AssignmentListDto>> Search(SearchAssignmentListDto dto)
    {
        var result = await _assignmentListRepository.Search(_httpContextAccessor.GetUserId(), dto.Name, dto.PerPage,
            dto.Page);

        return new PagedDto<AssignmentListDto>
        {
            Items = _mapper.Map<List<AssignmentListDto>>(result.Items),
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
            _notificator.Handle("Os dois IDs referente a lista de tarefas precisam ser iguais.");
            return null;
        }

        var getAssignmentList = await _assignmentListRepository.GetById(id, _httpContextAccessor.GetUserId());
        if (getAssignmentList == null)
        {
            _notificator.HandleNotFoundResource();
            return null;
        }

        var filter = _mapper.Map<AssignmentFilter>(dto);

        var result = await _assignmentRepository.Search(_httpContextAccessor.GetUserId(), id, filter,
            dto.PerPage, dto.Page);

        return new PagedDto<AssignmentDto>
        {
            Items = _mapper.Map<List<AssignmentDto>>(result.Items),
            Total = result.Total,
            Page = result.Page,
            PerPage = result.PerPage,
            PageCount = result.PageCount
        };
    }

    private async Task<bool> CommitChanges()
    {
        if (await _assignmentListRepository.UnitOfWork.Commit())
            return true;

        _notificator.Handle("Ocorreu um erro ao salvar as alterações.");
        return false;
    }
}