namespace Linker.WebApi.Controllers
{
    using System;
    using EnsureThat;
    using Linker.Common;
    using Linker.Core.ApiModels;
    using Linker.Core.Controllers;
    using Linker.Core.Models;
    using Linker.Core.Repositories;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The API controller for <see cref="Article"/>.
    /// </summary>
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
        public IActionResult Create([FromBody] CreateArticleRequest request)
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
            };

            this.repository.Add(article);

            return this.CreatedAtAction(
                actionName: nameof(this.GetById),
                routeValues: new { article.Id },
                value: request);
        }

        /// <inheritdoc/>
        [HttpDelete("{id:guid}", Name = "DeleteArticle")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                this.repository.Remove(id.ToString());
            }
            catch (InvalidOperationException)
            {
                return this.NotFound();
            }

            return this.NoContent();
        }

        /// <inheritdoc/>
        [HttpGet("", Name = "GetAllArticles")]
        public IActionResult GetAll()
        {
            var results = this.repository.GetAll();
            return this.Ok(results);
        }

        /// <inheritdoc/>
        [HttpGet("{id:guid}", Name = "GetArticle")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                var result = this.repository.GetById(id.ToString());
                return this.Ok(result);
            }
            catch (InvalidOperationException)
            {
                return this.NotFound();
            }
        }

        /// <inheritdoc/>
        [HttpPut("{id:guid}", Name = "UpdateArticle")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateArticleRequest request)
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
                this.repository.Update(article);
            }
            catch (InvalidOperationException)
            {
                return this.NotFound();
            }

            return this.NoContent();
        }
    }
}
