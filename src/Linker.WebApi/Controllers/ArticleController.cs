namespace Linker.WebApi.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using EnsureThat;
    using Linker.Common;
    using Linker.Core.ApiModels;
    using Linker.Core.Controllers;
    using Linker.Core.Models;
    using Linker.Core.Repositories;
    using Linker.WebApi.Swagger;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using Swashbuckle.AspNetCore.Filters;

    /// <summary>
    /// The API controller for <see cref="Article"/>.
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public sealed class ArticleController : ControllerBase, IArticleController
    {
        private readonly IArticleRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleController"/> class.
        /// </summary>
        /// <param name="repository">The repository for <see cref="Article"/>.</param>
        public ArticleController(IArticleRepository repository)
        {
            this.repository = EnsureArg.IsNotNull(repository, nameof(repository));
        }

        /// <inheritdoc/>
        [HttpPost("", Name = "CreateArticle")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Article created")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateArticleRequest request)
        {
            var article = new Article
            {
                Id = Guid.NewGuid().ToString(),
                Url = request.Url,
                Title = request.Title,
                Category = request.Category,
                Description = request.Description,
                Tags = request.Tags,
                Language = request.Language,
                LastVisitAt = DateTime.Now,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                Domain = UrlParser.ExtractDomainLite(request.Url),
                Grammar = request.Grammar,
                Year = request.Year,
                Author = request.Author,
                CreatedBy = request.CreatedBy,
            };

            await this.repository.AddAsync(article).ConfigureAwait(false);

            return this.CreatedAtAction(
                actionName: nameof(this.GetByIdAsync),
                routeValues: new { article.Id },
                value: request);
        }

        /// <inheritdoc/>
        [HttpDelete("{id:guid}", Name = "DeleteArticle")]
        [SwaggerResponse((int)HttpStatusCode.NoContent, "Article successfully deleted.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Article not found.")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                await this.repository.RemoveAsync(id.ToString()).ConfigureAwait(false);
            }
            catch (InvalidOperationException)
            {
                return this.NotFound();
            }

            return this.NoContent();
        }

        /// <inheritdoc/>
        [HttpGet("", Name = "GetAllArticles")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Retrieved all articles.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ArticleResponseCollectionExample))]
        [ProducesResponseType(typeof(ArticleResponseCollectionExample), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllAsync()
        {
            var results = await this.repository.GetAllAsync().ConfigureAwait(false);
            return this.Ok(results);
        }

        /// <inheritdoc/>
        [HttpGet("{id:guid}", Name = "GetArticle")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Article not found.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Retrieved article.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ArticleResponseExample))]
        [ProducesResponseType(typeof(ArticleResponseExample), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                var result = await this.repository.GetByIdAsync(id.ToString()).ConfigureAwait(false);
                return this.Ok(result);
            }
            catch (InvalidOperationException)
            {
                return this.NotFound();
            }
        }

        /// <inheritdoc/>
        [HttpPut("{id:guid}", Name = "UpdateArticle")]
        [SwaggerResponse((int)HttpStatusCode.NoContent, "Article updated.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Article not found.")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateArticleRequest request)
        {
            var article = new Article
            {
                Id = id.ToString(),
                Url = request.Url,
                Title = request.Title,
                Category = request.Category,
                Description = request.Description,
                Language = request.Language,
                Grammar = request.Grammar,
                WatchLater = request.WatchLater,
                Domain = UrlParser.ExtractDomainLite(request.Url),
                Author = request.Author,
                Year = request.Year,
            };

            try
            {
                await this.repository.UpdateAsync(article).ConfigureAwait(false);
            }
            catch (InvalidOperationException)
            {
                return this.NotFound();
            }

            return this.NoContent();
        }
    }
}
