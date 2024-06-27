namespace Linker.Mvc.Mappers;

using System;
using AutoMapper;
using Linker.Common;
using Linker.Core.V2.ApiModels;
using Linker.Core.V2.Models;

/// <summary>
/// The mapper profile for mapping <see cref="Link"/> related models.
/// </summary>
public class LinkMapperProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LinkMapperProfile"/> class.
    /// </summary>
    public LinkMapperProfile()
    {
        this.ConfigureMapFromPostRequestToProfile();
        this.ConfigureMapFromPutRequestToProfile();
    }

    /// <summary>
    /// Configure the mappings from <see cref="CreateLinkRequest"/> to <see cref="Link"/>.
    /// </summary>
    public void ConfigureMapFromPostRequestToProfile()
    {
        this.CreateMap<CreateLinkRequest, Link>(MemberList.Source)
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Domain, opt => opt.MapFrom(src => UrlParser.ExtractDomainLite(src.Url)));
    }

    /// <summary>
    /// Configure the mappings from <see cref="UpdateLinkRequest"/> to <see cref="Link"/>.
    /// </summary>
    public void ConfigureMapFromPutRequestToProfile()
    {
        this.CreateMap<UpdateLinkRequest, Link>(MemberList.Source)
            .ForMember(dest => dest.Domain, opt => opt.MapFrom(src => UrlParser.ExtractDomainLite(src.Url)));
    }
}
