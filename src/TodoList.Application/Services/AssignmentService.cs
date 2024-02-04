using AutoMapper;
using Microsoft.AspNetCore.Http;
using TodoList.Application.Contracts.Services;
using TodoList.Application.DTOs.Assignment;
using TodoList.Application.DTOs.Paged;
using TodoList.Application.Extension;
using TodoList.Application.Notifications;
using TodoList.Domain.Contracts.Repositories;
using TodoList.Domain.Filter;
using TodoList.Domain.Models;

namespace TodoList.Application.Services;

public class AssignmentService : IAssignmentService
{
    private readonly IMapper _mapper;
    private readonly INotificator _notificator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAssignmentListRepository _assignmentListRepository;
    private readonly IAssignmentRepository _assignmentRepository;

    public AssignmentService(IMapper mapper, INotificator notificator, IHttpContextAccessor httpContextAccessor,
        IAssignmentListRepository assignmentListRepository, IAssignmentRepository assignmentRepository)
    {
        _mapper = mapper;
        _notificator = notificator;
        _httpContextAccessor = httpContextAccessor;
        _assignmentListRepository = assignmentListRepository;
        _assignmentRepository = assignmentRepository;
    }

    public async Task<AssignmentDto?> Create(CreateAssignmentDto dto)
    {
        if (!dto.Validate(out var validationResult))
        {
            _notificator.Handle(validationResult.Errors);
            return null;
        }

        var getAssignmentList = await _assignmentListRepository.FirstOrDefault(x =>
            x.Id == dto.AssignmentListId);
        if (getAssignmentList == null)
            _notificator.Handle("Não existe uma lista de tarefas com o id informado.");

        var getAssignment = await _assignmentRepository.FirstOrDefault(x =>
            x.Description == dto.Description && x.AssignmentListId == dto.AssignmentListId);
        if (getAssignment != null)
            _notificator.Handle("Já existe uma tarefa cadastrada com essa descrição nessa lista.");

        var createAssignment = _mapper.Map<Assignment>(dto);
        createAssignment.UserId = _httpContextAccessor.GetUserId() ?? 0;

        _assignmentRepository.Create(createAssignment);
        return await CommitChanges() ? _mapper.Map<AssignmentDto>(createAssignment) : null;
    }

    public async Task<AssignmentDto?> Update(int id, UpdateAssignmentDto dto)
    {
        var getAssignmentById = await _assignmentRepository.GetById(id, _httpContextAccessor.GetUserId());
        if (getAssignmentById == null)
        {
            _notificator.HandleNotFoundResource();
            return null;
        }

        if (!dto.Validate(out var validationResult))
        {
            _notificator.Handle(validationResult.Errors);
            return null;
        }

        var getAssignmentList = await _assignmentListRepository.FirstOrDefault(x =>
            x.Id == dto.AssignmentListId);
        if (getAssignmentList == null)
            _notificator.Handle("O id informado para a lista de tarefas não existe.");

        var getAssignment = await _assignmentRepository.FirstOrDefault(x =>
            x.Description == dto.Description && x.AssignmentListId == dto.AssignmentListId);
        if (getAssignment != null)
            _notificator.Handle("Já existe uma tarefa cadastrada com essa descrição nessa lista.");

        var updateAssignment = _mapper.Map<Assignment>(dto);

        _assignmentRepository.Update(updateAssignment);
        return await CommitChanges() ? _mapper.Map<AssignmentDto>(updateAssignment) : null;
    }

    public async Task MarkConclude(int id)
    {
        var getAssignment = await _assignmentRepository.GetById(id, _httpContextAccessor.GetUserId());
        if (getAssignment == null)
        {
            _notificator.HandleNotFoundResource();
            return;
        }

        getAssignment.SetConclude();

        _assignmentRepository.Update(getAssignment);
        await CommitChanges();
    }

    public async Task MarkNotConclude(int id)
    {
        var getAssignment = await _assignmentRepository.GetById(id, _httpContextAccessor.GetUserId());
        if (getAssignment == null)
        {
            _notificator.HandleNotFoundResource();
            return;
        }

        getAssignment.SetNotConclude();

        _assignmentRepository.Update(getAssignment);
        await CommitChanges();
    }

    public async Task Delete(int id)
    {
        var getAssignment = await _assignmentRepository.GetById(id, _httpContextAccessor.GetUserId());
        if (getAssignment == null)
        {
            _notificator.HandleNotFoundResource();
            return;
        }

        _assignmentRepository.Delete(getAssignment);
        await CommitChanges();
    }

    public async Task<AssignmentDto?> GetById(int id)
    {
        var getAssignment = await _assignmentRepository.GetById(id, _httpContextAccessor.GetUserId());
        if (getAssignment != null)
            return _mapper.Map<AssignmentDto>(getAssignment);

        _notificator.HandleNotFoundResource();
        return null;
    }

    public async Task<PagedDto<AssignmentDto>> Search(SearchAssignmentDto dto)
    {
        var filter = _mapper.Map<AssignmentFilter>(dto);

        var result = await _assignmentRepository.Search(_httpContextAccessor.GetUserId(), dto.AssignmentListId, filter,
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