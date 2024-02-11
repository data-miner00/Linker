namespace Linker.WebApi.Mappers;

using AutoMapper;
using Linker.Core.ApiModels;
using Linker.Core.Models;

/// <summary>
/// The mapper profile for Workspace related models.
/// </summary>
public sealed class WorkspaceMapperProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WorkspaceMapperProfile"/> class.
    /// </summary>
    public WorkspaceMapperProfile()
    {
        this.ConfigureMapFromPostRequestToWorkspace();
        this.ConfigureMapFromPutRequestToWorkspace();
        this.ConfigureMapFromMembershipPostRequestToMembership();
    }

    /// <summary>
    /// Configures the mapping for <see cref="CreateWorkspaceRequest"/> to <see cref="Workspace"/>.
    /// </summary>
    public void ConfigureMapFromPostRequestToWorkspace()
    {
        this.CreateMap<CreateWorkspaceRequest, Workspace>(MemberList.Source)
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.Now));
    }

    /// <summary>
    /// Configures the mapping for <see cref="UpdateWorkspaceRequest"/> to <see cref="Workspace"/>.
    /// </summary>
    public void ConfigureMapFromPutRequestToWorkspace()
    {
        this.CreateMap<UpdateWorkspaceRequest, Workspace>(MemberList.Source);
    }

    /// <summary>
    /// Configures the mapping for <see cref="CreateWorkspaceMembershipRequest"/> to <see cref="WorkspaceMembership"/>.
    /// </summary>
    public void ConfigureMapFromMembershipPostRequestToMembership()
    {
        this.CreateMap<CreateWorkspaceMembershipRequest, WorkspaceMembership>(MemberList.Source)
            .ForMember(dest => dest.WorkspaceId, opt => opt.MapFrom(src => src.WorkspaceId.ToString()))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.ToString()))
            .ForMember(dest => dest.WorkspaceRole, opt => opt.MapFrom(src => src.WorkspaceRole.ToString()));
    }
}
