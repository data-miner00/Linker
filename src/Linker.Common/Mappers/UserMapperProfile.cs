namespace Linker.Common.Mappers;

using AutoMapper;
using Linker.Core.ApiModels;
using Linker.Core.Models;
using System;

/// <summary>
/// The mapper profile for mapping <see cref="User"/> related models.
/// </summary>
public sealed class UserMapperProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserMapperProfile"/> class.
    /// </summary>
    public UserMapperProfile()
    {
        this.ConfigureMapFromPostRequestToUser();
        this.ConfigureMapFromUserToPostResponse();
    }

    private void ConfigureMapFromPostRequestToUser()
    {
        this.CreateMap<CreateUserRequest, User>(MemberList.Source)
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => Role.User))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Status.Active))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.Now));
    }

    private void ConfigureMapFromUserToPostResponse()
    {
        this.CreateMap<User, CreateUserResponse>(MemberList.Destination);
    }
}
