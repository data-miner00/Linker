namespace Linker.WebApi.Mappers;

using AutoMapper;
using Linker.Core.ApiModels;
using Linker.Core.Models;

public sealed class WorkspaceMapperProfile : Profile
{
    public WorkspaceMapperProfile()
    {
        this.ConfigureMapFromPostRequestToWorkspace();
        this.ConfigureMapFromPutRequestToWorkspace();
        this.ConfigureMapFromMembershipPostRequestToMembership();
    }

    public void ConfigureMapFromPostRequestToWorkspace()
    {
        this.CreateMap<CreateWorkspaceRequest, Workspace>(MemberList.Source)
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.Now));
    }

    public void ConfigureMapFromPutRequestToWorkspace()
    {
        this.CreateMap<UpdateWorkspaceRequest, Workspace>(MemberList.Source);
    }

    public void ConfigureMapFromMembershipPostRequestToMembership()
    {
        this.CreateMap<CreateWorkspaceMembershipRequest, WorkspaceMembership>(MemberList.Source)
            .ForMember(dest => dest.WorkspaceId, opt => opt.MapFrom(src => src.WorkspaceId.ToString()))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.ToString()))
            .ForMember(dest => dest.WorkspaceRole, opt => opt.MapFrom(src => src.WorkspaceRole.ToString()));
    }
}
