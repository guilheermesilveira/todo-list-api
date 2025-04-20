using AutoMapper;
using Microsoft.AspNetCore.Http;
using TodoList.Application.Contracts.Services;
using TodoList.Application.DTOs.Assignment;
using TodoList.Application.DTOs.AssignmentList;
using TodoList.Application.DTOs.Paged;
using TodoList.Application.Extension;
using TodoList.Application.Notifications;
using TodoList.Application.Validations;
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
        if (!await ValidationsToCreate(dto))
            return null;

        var assignmentList = _mapper.Map<AssignmentList>(dto);
        assignmentList.UserId = _httpContextAccessor.GetUserId() ?? 0;
        _assignmentListRepository.Create(assignmentList);

        return await CommitChanges() ? _mapper.Map<AssignmentListDto>(assignmentList) : null;
    }

    public async Task<AssignmentListDto?> Update(int id, UpdateAssignmentListDto dto)
    {
        if (!await ValidationsToUpdate(id, dto))
            return null;

        var assignmentList = await _assignmentListRepository.GetById(id, _httpContextAccessor.GetUserId());
        assignmentList!.Name = dto.Name;
        _assignmentListRepository.Update(assignmentList);

        return await CommitChanges() ? _mapper.Map<AssignmentListDto>(assignmentList) : null;
    }

    public async Task Delete(int id)
    {
        var assignmentList = await _assignmentListRepository.GetById(id, _httpContextAccessor.GetUserId());
        if (assignmentList == null)
        {
            _notificator.HandleNotFoundResource();
            return;
        }

        if (assignmentList.Assignments.Any(assignment => !assignment.Concluded))
        {
            _notificator.Handle("Unable to delete a list with pending tasks");
            return;
        }

        _assignmentListRepository.Delete(assignmentList);
        await CommitChanges();
    }

    public async Task<AssignmentListDto?> GetById(int id)
    {
        var assignmentList = await _assignmentListRepository.GetById(id, _httpContextAccessor.GetUserId());
        if (assignmentList != null)
            return _mapper.Map<AssignmentListDto>(assignmentList);

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
            _notificator.Handle("The two IDs for the task list must be the same");
            return null;
        }

        var assignmentList = await _assignmentListRepository.GetById(id, _httpContextAccessor.GetUserId());
        if (assignmentList == null)
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

    private async Task<bool> ValidationsToCreate(CreateAssignmentListDto dto)
    {
        var assignmentList = _mapper.Map<AssignmentList>(dto);
        var validator = new AssignmentListValidator();

        var validationResult = await validator.ValidateAsync(assignmentList);
        if (!validationResult.IsValid)
        {
            _notificator.Handle(validationResult.Errors);
            return false;
        }

        return true;
    }

    private async Task<bool> ValidationsToUpdate(int id, UpdateAssignmentListDto dto)
    {
        if (id != dto.Id)
        {
            _notificator.Handle("The ID given to the URL must be the same as the ID given in the JSON");
            return false;
        }

        var assignmentListExist = await _assignmentListRepository.GetById(id, _httpContextAccessor.GetUserId());
        if (assignmentListExist == null)
        {
            _notificator.HandleNotFoundResource();
            return false;
        }

        var assignmentList = _mapper.Map<AssignmentList>(dto);
        var validator = new AssignmentListValidator();

        var validationResult = await validator.ValidateAsync(assignmentList);
        if (!validationResult.IsValid)
        {
            _notificator.Handle(validationResult.Errors);
            return false;
        }

        return true;
    }

    private async Task<bool> CommitChanges()
    {
        if (await _assignmentListRepository.UnitOfWork.Commit())
            return true;

        _notificator.Handle("An error occurred while saving changes");
        return false;
    }
}