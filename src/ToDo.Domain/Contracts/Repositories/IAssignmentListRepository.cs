﻿using ToDo.Domain.Contracts.Interfaces;
using ToDo.Domain.Models;

namespace ToDo.Domain.Contracts.Repositories;

public interface IAssignmentListRepository : IEntityRepository<AssignmentList>
{
    void Create(AssignmentList assignmentList);
    void Update(AssignmentList assignmentList);
    void Delete(AssignmentList assignmentList);
    Task<AssignmentList?> GetById(int id, int? userId);
    Task<IPagedResult<AssignmentList>> Search(int? userId, string name, int perPage = 10, int page = 1);
}