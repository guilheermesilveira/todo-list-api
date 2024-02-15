using AutoMapper;
using Microsoft.AspNetCore.Http;
using TodoList.Application.Contracts.Services;
using TodoList.Application.DTOs.Assignment;
using TodoList.Application.DTOs.Paged;
using TodoList.Application.Extension;
using TodoList.Application.Notifications;
using TodoList.Application.Validations.Assignment;
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
        if (!await ValidationsToCreateAssignment(dto))
            return null;

        var createAssignment = _mapper.Map<Assignment>(dto);
        createAssignment.UserId = _httpContextAccessor.GetUserId() ?? 0;

        _assignmentRepository.Create(createAssignment);
        return await CommitChanges() ? _mapper.Map<AssignmentDto>(createAssignment) : null;
    }

    public async Task<AssignmentDto?> Update(int id, UpdateAssignmentDto dto)
    {
        if (!await ValidationsToUpdateAssignment(id, dto))
            return null;

        var updateAssignment = await _assignmentRepository.GetById(id, _httpContextAccessor.GetUserId());
        MappingToUpdateAssignment(updateAssignment!, dto);

        _assignmentRepository.Update(updateAssignment!);
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
        var deleteAssignment = await _assignmentRepository.GetById(id, _httpContextAccessor.GetUserId());
        if (deleteAssignment == null)
        {
            _notificator.HandleNotFoundResource();
            return;
        }

        _assignmentRepository.Delete(deleteAssignment);
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

    private async Task<bool> ValidationsToCreateAssignment(CreateAssignmentDto dto)
    {
        var assignment = _mapper.Map<Assignment>(dto);
        var assignmentValidator = new ValidatorToCreateAssignment();

        var validationResult = await assignmentValidator.ValidateAsync(assignment);
        if (!validationResult.IsValid)
        {
            _notificator.Handle(validationResult.Errors);
            return false;
        }

        var existingAssignmentList = await _assignmentListRepository.FirstOrDefault(x =>
            x.Id == dto.AssignmentListId);
        if (existingAssignmentList == null)
        {
            _notificator.Handle("Não existe uma lista de tarefas com o id informado.");
            return false;
        }

        var existingAssignment = await _assignmentRepository.FirstOrDefault(x =>
            x.Description == dto.Description && x.AssignmentListId == dto.AssignmentListId);
        if (existingAssignment != null)
        {
            _notificator.Handle("Já existe uma tarefa cadastrada com essa descrição nessa lista.");
            return false;
        }

        return true;
    }

    private async Task<bool> ValidationsToUpdateAssignment(int id, UpdateAssignmentDto dto)
    {
        var existingAssignment = await _assignmentRepository.GetById(id, _httpContextAccessor.GetUserId());
        if (existingAssignment == null)
        {
            _notificator.HandleNotFoundResource();
            return false;
        }

        var assignment = _mapper.Map<Assignment>(dto);
        var assignmentValidator = new ValidatorToUpdateAssignment();

        var validationResult = await assignmentValidator.ValidateAsync(assignment);
        if (!validationResult.IsValid)
        {
            _notificator.Handle(validationResult.Errors);
            return false;
        }

        var existingAssignmentList = await _assignmentListRepository.FirstOrDefault(x =>
            x.Id == dto.AssignmentListId);
        if (existingAssignmentList == null)
        {
            _notificator.Handle("Não existe uma lista de tarefas com o id informado.");
            return false;
        }

        if (!string.IsNullOrEmpty(dto.Description))
        {
            var existingAssignmentByDescription = await _assignmentRepository.FirstOrDefault(x =>
                x.Description == dto.Description && x.AssignmentListId == dto.AssignmentListId);
            if (existingAssignmentByDescription != null)
            {
                _notificator.Handle("Já existe uma tarefa cadastrada com essa descrição nessa lista.");
                return false;
            }
        }

        return true;
    }

    private void MappingToUpdateAssignment(Assignment assignment, UpdateAssignmentDto dto)
    {
        if (!string.IsNullOrEmpty(dto.Description))
            assignment.Description = dto.Description;

        if (dto.Deadline.HasValue)
            assignment.Deadline = dto.Deadline;
    }

    private async Task<bool> CommitChanges()
    {
        if (await _assignmentRepository.UnitOfWork.Commit())
            return true;

        _notificator.Handle("Ocorreu um erro ao salvar as alterações.");
        return false;
    }
}