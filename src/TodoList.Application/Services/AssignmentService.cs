using AutoMapper;
using Microsoft.AspNetCore.Http;
using TodoList.Application.Contracts.Services;
using TodoList.Application.DTOs.Assignment;
using TodoList.Application.DTOs.Paged;
using TodoList.Application.Extension;
using TodoList.Application.Notifications;
using TodoList.Application.Validations;
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
        if (!await ValidationsToCreate(dto))
            return null;

        var assignment = _mapper.Map<Assignment>(dto);
        assignment.UserId = _httpContextAccessor.GetUserId() ?? 0;
        _assignmentRepository.Create(assignment);

        return await CommitChanges() ? _mapper.Map<AssignmentDto>(assignment) : null;
    }

    public async Task<AssignmentDto?> Update(int id, UpdateAssignmentDto dto)
    {
        if (!await ValidationsToUpdate(id, dto))
            return null;

        var assignment = await _assignmentRepository.GetById(id, _httpContextAccessor.GetUserId());
        assignment!.Description = dto.Description;
        if (dto.Deadline.HasValue)
            assignment.Deadline = dto.Deadline;
        assignment.AssignmentListId = dto.AssignmentListId;
        _assignmentRepository.Update(assignment);

        return await CommitChanges() ? _mapper.Map<AssignmentDto>(assignment) : null;
    }

    public async Task MarkConclude(int id)
    {
        var assignment = await _assignmentRepository.GetById(id, _httpContextAccessor.GetUserId());
        if (assignment == null)
        {
            _notificator.HandleNotFoundResource();
            return;
        }

        assignment.SetConclude();
        _assignmentRepository.Update(assignment);

        await CommitChanges();
    }

    public async Task MarkNotConclude(int id)
    {
        var assignment = await _assignmentRepository.GetById(id, _httpContextAccessor.GetUserId());
        if (assignment == null)
        {
            _notificator.HandleNotFoundResource();
            return;
        }

        assignment.SetNotConclude();
        _assignmentRepository.Update(assignment);

        await CommitChanges();
    }

    public async Task Delete(int id)
    {
        var assignment = await _assignmentRepository.GetById(id, _httpContextAccessor.GetUserId());
        if (assignment == null)
        {
            _notificator.HandleNotFoundResource();
            return;
        }

        _assignmentRepository.Delete(assignment);
        await CommitChanges();
    }

    public async Task<AssignmentDto?> GetById(int id)
    {
        var assignment = await _assignmentRepository.GetById(id, _httpContextAccessor.GetUserId());
        if (assignment != null)
            return _mapper.Map<AssignmentDto>(assignment);

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

    private async Task<bool> ValidationsToCreate(CreateAssignmentDto dto)
    {
        var assignment = _mapper.Map<Assignment>(dto);
        var validator = new AssignmentValidator();

        var validationResult = await validator.ValidateAsync(assignment);
        if (!validationResult.IsValid)
        {
            _notificator.Handle(validationResult.Errors);
            return false;
        }

        var assignmentList = await _assignmentListRepository.FirstOrDefault(list => list.Id == dto.AssignmentListId);
        if (assignmentList == null)
        {
            _notificator.Handle("There is no task list with the given id");
            return false;
        }

        return true;
    }

    private async Task<bool> ValidationsToUpdate(int id, UpdateAssignmentDto dto)
    {
        if (id != dto.Id)
        {
            _notificator.Handle("The ID given to the URL must be the same as the ID given in the JSON");
            return false;
        }

        var assignmentExist = await _assignmentRepository.GetById(id, _httpContextAccessor.GetUserId());
        if (assignmentExist == null)
        {
            _notificator.HandleNotFoundResource();
            return false;
        }

        var assignment = _mapper.Map<Assignment>(dto);
        var validator = new AssignmentValidator();

        var validationResult = await validator.ValidateAsync(assignment);
        if (!validationResult.IsValid)
        {
            _notificator.Handle(validationResult.Errors);
            return false;
        }

        var assignmentList = await _assignmentListRepository.FirstOrDefault(list => list.Id == dto.AssignmentListId);
        if (assignmentList == null)
        {
            _notificator.Handle("There is no task list with the given id");
            return false;
        }

        return true;
    }

    private async Task<bool> CommitChanges()
    {
        if (await _assignmentRepository.UnitOfWork.Commit())
            return true;

        _notificator.Handle("An error occurred while saving changes");
        return false;
    }
}