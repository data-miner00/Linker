namespace Linker.WebApi.Mappers;

using System;
using AutoMapper;
using Linker.Core.ApiModels;
using Linker.Core.Models;
using Linker.WebApi.ApiModels;

/// <summary>
/// The mapper profile for mapping <see cref="Youtube"/> related models.
/// </summary>
public sealed class YoutubeMapperProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="YoutubeMapperProfile"/> class.
    /// </summary>
    public YoutubeMapperProfile()
    {
        this.ConfigureMapFromPostRequestToYoutube();
        this.ConfigureMapFromPutRequestToYoutube();
        this.ConfigureMapFromYoutubeToYoutubeApiModel();
    }

    /// <summary>
    /// Configure the mappings from <see cref="CreateYoutubeRequest"/> to <see cref="Youtube"/>.
    /// </summary>
    public void ConfigureMapFromPostRequestToYoutube()
    {
        this.CreateMap<CreateYoutubeRequest, Youtube>(MemberList.Source)
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
            .ForMember(dest => dest.LastVisitAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.Now));
    }

    /// <summary>
    /// Configure the mappings from <see cref="UpdateYoutubeRequest"/> to <see cref="Youtube"/>.
    /// </summary>
    public void ConfigureMapFromPutRequestToYoutube()
    {
        this.CreateMap<UpdateYoutubeRequest, Youtube>(MemberList.Source);
    }

    /// <summary>
    /// Configure the mappings from <see cref="Youtube"/> to <see cref="YoutubeApiModel"/>.
    /// </summary>
    public void ConfigureMapFromYoutubeToYoutubeApiModel()
    {
        this.CreateMap<Youtube, YoutubeApiModel>(MemberList.Source)
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()))
            .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Language.ToString()))
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating.ToString()));
    }
}
