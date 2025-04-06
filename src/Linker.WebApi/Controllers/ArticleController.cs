namespace Linker.WebApi.Controllers
{
    using System;
    using System.Net;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using AutoMapper;
    using EnsureThat;
    using Linker.Core.ApiModels;
    using Linker.Core.Controllers;
    using Linker.Core.Models;
    using Linker.Core.Repositories;
    using Linker.WebApi.ApiModels;
    using Linker.WebApi.Filters;
    using Linker.WebApi.Swagger;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http.Timeouts;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using Swashbuckle.AspNetCore.Filters;

    /// <summary>
    /// The API controller for <see cref="Article"/>.
    /// </summary>
    [Authorize]
    [Produces("application/json")]
    [RequestTimeout(Timeout.Infinite)]
    [Route("api/[controller]")]
    [ApiController]
    public sealed class ArticleController : ControllerBase, IArticleController
    {
        private readonly IArticleRepository repository;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleController"/> class.
        /// </summary>
        /// <param name="repository">The repository for <see cref="Article"/>.</param>
        /// <param name="mapper">The mapper instance.</param>
        /// <param name="context">The HTTP context accessor.</param>
        public ArticleController(
            IArticleRepository repository,
            IMapper mapper,
            IHttpContextAccessor context)
        {
            this.repository = EnsureArg.IsNotNull(repository, nameof(repository));
            this.mapper = EnsureArg.IsNotNull(mapper, nameof(mapper));
            this.context = EnsureArg.IsNotNull(context, nameof(context));
        }

        /// <inheritdoc/>
        [HttpPost("", Name = "CreateArticle")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Article created")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateArticleRequest request)
        {
            EnsureArg.IsNotNull(request, nameof(request));

            var userId = this.context.HttpContext?.User
                .FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

            var article = this.mapper.Map<Article>(request);
            article.CreatedBy = userId;

            await this.repository
                .AddAsync(article, CancellationToken.None)
                .ConfigureAwait(false);

            return this.Created();
        }

        #region Privilege

        /// <inheritdoc/>
        [RoleAuthorize(Role.Administrator)]
        [HttpDelete("{id:guid}", Name = "DeleteArticle")]
        [SwaggerResponse((int)HttpStatusCode.NoContent, "Article successfully deleted.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Article not found.")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                await this.repository
                    .RemoveAsync(id.ToString(), CancellationToken.None)
                    .ConfigureAwait(false);
            }
            catch (InvalidOperationException)
            {
                return this.NotFound();
            }

            return this.NoContent();
        }

        /// <inheritdoc/>
        [RoleAuthorize(Role.Administrator)]
        [HttpGet("", Name = "GetAllArticles")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Retrieved all articles.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ArticleResponseCollectionExample))]
        [ProducesResponseType(typeof(ArticleResponseCollectionExample), (int)HttpStatusCode.OK)]
        [ScopedInformerFilter("GetAllArticles Endpoint")]
        public async Task<IActionResult> GetAllAsync()
        {
            var articles = await this.repository
                .GetAllAsync(CancellationToken.None)
                .ConfigureAwait(false);

            var results = articles.Select(this.mapper.Map<ArticleApiModel>)
                .ToList();

            return this.Ok(results);
        }

        /// <inheritdoc/>
        [RoleAuthorize(Role.Administrator)]
        [HttpGet("{id:guid}", Name = "GetArticle")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Article not found.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Retrieved article by user.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ArticleResponseExample))]
        [ProducesResponseType(typeof(ArticleResponseExample), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                var article = await this.repository
                    .GetByIdAsync(id.ToString(), CancellationToken.None)
                    .ConfigureAwait(false);

                var result = this.mapper.Map<ArticleApiModel>(article);

                return this.Ok(result);
            }
            catch (InvalidOperationException)
            {
                return this.NotFound();
            }
        }

        /// <inheritdoc/>
        [RoleAuthorize(Role.Administrator)]
        [HttpPut("{id:guid}", Name = "UpdateArticle")]
        [SwaggerResponse((int)HttpStatusCode.NoContent, "Article updated.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Article not found.")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateArticleRequest request)
        {
            EnsureArg.IsNotNull(request, nameof(request));

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var userId = this.context.HttpContext?.User
                .FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

            try
            {
                var existing = await this.repository
                    .GetByIdAsync(id.ToString(), CancellationToken.None)
                    .ConfigureAwait(false);

                if (userId != existing.CreatedBy)
                {
                    return this.Unauthorized();
                }

                var article = this.mapper.Map<Article>(request);
                article.Id = id.ToString();

                await this.repository
                    .UpdateAsync(article, CancellationToken.None)
                    .ConfigureAwait(false);

                return this.NoContent();
            }
            catch (InvalidOperationException)
            {
                return this.NotFound();
            }
        }
        #endregion

        #region User

        /// <inheritdoc/>
        [AccountAuthorize]
        [HttpGet("byuser/{userId:guid}", Name = "GetAllArticlesByUser")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Retrieved all articles by user.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ArticleResponseCollectionExample))]
        [ProducesResponseType(typeof(ArticleResponseCollectionExample), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllByUserAsync(Guid userId)
        {
            var results = await this.repository
                .GetAllByUserAsync(userId.ToString(), CancellationToken.None)
                .ConfigureAwait(false);

            return this.Ok(results);
        }

        /// <inheritdoc/>
        [AccountAuthorize]
        [HttpGet("byuser/{userId:guid}/{linkId:guid}", Name = "GetArticleByUser")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Article not found.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Retrieved article.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ArticleResponseExample))]
        [ProducesResponseType(typeof(ArticleResponseExample), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByUserAsync(Guid userId, Guid linkId)
        {
            try
            {
                var result = await this.repository
                    .GetByUserAsync(userId.ToString(), linkId.ToString(), CancellationToken.None)
                    .ConfigureAwait(false);

                return this.Ok(result);
            }
            catch (InvalidOperationException)
            {
                return this.NotFound();
            }
        }

        /// <inheritdoc/>
        [AccountAuthorize]
        [HttpPut("/byuser/{userId:guid}/{linkId:guid}", Name = "UpdateArticleByUser")]
        [SwaggerResponse((int)HttpStatusCode.NoContent, "Article updated.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Article not found.")]
        public async Task<IActionResult> UpdateByUserAsync(
            [FromRoute] Guid userId,
            [FromRoute] Guid linkId,
            [FromBody] UpdateArticleRequest request)
        {
            EnsureArg.IsNotNull(request, nameof(request));

            try
            {
                var strUserId = userId.ToString();
                var existing = await this.repository
                    .GetByIdAsync(linkId.ToString(), CancellationToken.None)
                    .ConfigureAwait(false);

                if (strUserId != existing.CreatedBy)
                {
                    return this.Unauthorized();
                }

                var article = this.mapper.Map<Article>(request);
                article.Id = linkId.ToString();

                await this.repository
                    .UpdateAsync(article, CancellationToken.None)
                    .ConfigureAwait(false);

                return this.NoContent();
            }
            catch (InvalidOperationException)
            {
                return this.NotFound();
            }
        }

        /// <inheritdoc/>
        [AccountAuthorize]
        [HttpDelete("/byuser/{userId:guid}/{linkId:guid}", Name = "DeleteArticleByUser")]
        [SwaggerResponse((int)HttpStatusCode.NoContent, "Article successfully deleted.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Article not found.")]
        public async Task<IActionResult> DeleteByUserAsync(
            [FromRoute] Guid userId,
            [FromRoute] Guid linkId)
        {
            try
            {
                var existingLink = await this.repository
                    .GetByIdAsync(linkId.ToString(), CancellationToken.None)
                    .ConfigureAwait(false);

                if (!existingLink.CreatedBy.Equals(userId.ToString()))
                {
                    return this.Unauthorized();
                }

                await this.repository
                    .RemoveAsync(linkId.ToString(), CancellationToken.None)
                    .ConfigureAwait(false);
            }
            catch (InvalidOperationException)
            {
                return this.NotFound();
            }

            return this.NoContent();
        }
        #endregion
    }
}
