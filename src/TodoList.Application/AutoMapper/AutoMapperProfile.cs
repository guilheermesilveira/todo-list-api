using AutoMapper;
using TodoList.Application.DTOs.Assignment;
using TodoList.Application.DTOs.AssignmentList;
using TodoList.Application.DTOs.Auth;
using TodoList.Application.DTOs.User;
using TodoList.Domain.Filter;
using TodoList.Domain.Models;

namespace TodoList.Application.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        #region Auth

        CreateMap<RegisterUserDto, User>().ReverseMap();
        CreateMap<LoginDto, User>().ReverseMap();
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