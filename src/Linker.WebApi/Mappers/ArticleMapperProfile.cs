namespace Linker.WebApi.Mappers;

using System;
using AutoMapper;
using Linker.Common;
using Linker.Core.ApiModels;
using Linker.Core.Models;
using Linker.WebApi.ApiModels;

/// <summary>
/// The mapper profile for mapping <see cref="Article"/> related models.
/// </summary>
public class ArticleMapperProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ArticleMapperProfile"/> class.
    /// </summary>
    public ArticleMapperProfile()
    {
        this.ConfigureMapFromPostRequestToArticle();
        this.ConfigureMapFromPutRequestToArticle();
        this.ConfigureMapFromArticleToArticleApiModel();
    }

    /// <summary>
    /// Configure the mappings from <see cref="CreateArticleRequest"/> to <see cref="Article"/>.
    /// </summary>
    public void ConfigureMapFromPostRequestToArticle()
    {
        this.CreateMap<CreateArticleRequest, Article>(MemberList.Source)
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
            .ForMember(dest => dest.LastVisitAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Domain, opt => opt.MapFrom(src => UrlParser.ExtractDomainLite(src.Url)));
    }

    /// <summary>
    /// Configure the mappings from <see cref="UpdateArticleRequest"/> to <see cref="Article"/>.
    /// </summary>
    public void ConfigureMapFromPutRequestToArticle()
    {
        this.CreateMap<UpdateArticleRequest, Article>(MemberList.Source)
            .ForMember(dest => dest.Domain, opt => opt.MapFrom(src => UrlParser.ExtractDomainLite(src.Url)));
    }

    /// <summary>
    /// Configure the mappings from <see cref="Article"/> to <see cref="ArticleApiModel"/>.
    /// </summary>
    public void ConfigureMapFromArticleToArticleApiModel()
    {
        this.CreateMap<Article, ArticleApiModel>(MemberList.Source)
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()))
            .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Language.ToString()))
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating.ToString()))
            .ForMember(dest => dest.Grammar, opt => opt.MapFrom(src => src.Grammar.ToString()));
    }
}
