namespace Linker.WebApi.Mappers;

using System;
using AutoMapper;
using Linker.Common;
using Linker.Core.ApiModels;
using Linker.Core.Models;

/// <summary>
/// The mapper profile for mapping <see cref="Website"/> related models.
/// </summary>
public sealed class WebsiteMapperProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WebsiteMapperProfile"/> class.
    /// </summary>
    public WebsiteMapperProfile()
    {
        this.ConfigureMapFromPostRequestToWebsite();
        this.ConfigureMapFromPutRequestToWebsite();
    }

    /// <summary>
    /// Configure the mappings from <see cref="CreateWebsiteRequest"/> to <see cref="Website"/>.
    /// </summary>
    public void ConfigureMapFromPostRequestToWebsite()
    {
        this.CreateMap<CreateWebsiteRequest, Website>(MemberList.Source)
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
            .ForMember(dest => dest.LastVisitAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Domain, opt => opt.MapFrom(src => UrlParser.ExtractDomainLite(src.Url)));
    }

    /// <summary>
    /// Configure the mappings from <see cref="UpdateWebsiteRequest"/> to <see cref="Website"/>.
    /// </summary>
    public void ConfigureMapFromPutRequestToWebsite()
    {
        this.CreateMap<UpdateWebsiteRequest, Website>(MemberList.Source)
            .ForMember(dest => dest.Domain, opt => opt.MapFrom(src => UrlParser.ExtractDomainLite(src.Url)));
    }
}
