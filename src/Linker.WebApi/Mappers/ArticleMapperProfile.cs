namespace Linker.WebApi.Mappers
{
    using AutoMapper;
    using Linker.Common;
    using Linker.Core.ApiModels;
    using Linker.Core.Models;

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
    }
}
