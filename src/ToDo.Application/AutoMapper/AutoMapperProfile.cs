using AutoMapper;
using ToDo.Application.DTOs.Assignment;
using ToDo.Application.DTOs.AssignmentList;
using ToDo.Application.DTOs.User;
using ToDo.Domain.Filter;
using ToDo.Domain.Models;

namespace ToDo.Application.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        #region User
        
        CreateMap<CreateUserDto, User>().ReverseMap();
        CreateMap<UserDto, User>().ReverseMap();

        #endregion
        
        #region AssignmentList
        
        CreateMap<CreateAssignmentListDto, AssignmentList>().ReverseMap();
        CreateMap<UpdateAssignmentListDto, AssignmentList>().ReverseMap();
        CreateMap<AssignmentListDto, AssignmentList>().ReverseMap();

        #endregion

        #region Assignment
        
        CreateMap<CreateAssignmentDto, Assignment>().ReverseMap();
        CreateMap<UpdateAssignmentDto, Assignment>().ReverseMap();
        CreateMap<SearchAssignmentDto, AssignmentFilter>().ReverseMap();
        CreateMap<AssignmentDto, Assignment>().ReverseMap();

        #endregion
    }
}