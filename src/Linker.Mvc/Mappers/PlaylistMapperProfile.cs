namespace Linker.Mvc.Mappers;

using AutoMapper;
using Linker.Core.V2.ApiModels;
using Linker.Core.V2.Models;

/// <summary>
/// The mapper profile for mapping <see cref="Playlist"/> related models.
/// </summary>
public sealed class PlaylistMapperProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PlaylistMapperProfile"/> class.
    /// </summary>
    public PlaylistMapperProfile()
    {
        this.ConfigureMapFromPutRequestToPlaylist();
    }

    private void ConfigureMapFromPutRequestToPlaylist()
    {
        this.CreateMap<UpdatePlaylistRequest, Playlist>(MemberList.Source)
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.Now));
    }
}
