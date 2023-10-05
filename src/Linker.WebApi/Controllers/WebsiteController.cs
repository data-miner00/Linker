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
    /// The API controller for <see cref="Website"/>.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class WebsiteController : ControllerBase, IWebsiteController
    {
        private readonly IWebsiteRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebsiteController"/> class.
        /// </summary>
        /// <param name="repository">The repository for <see cref="Website"/>.</param>
        public WebsiteController(IWebsiteRepository repository)
        {
            this.repository = EnsureArg.IsNotNull(repository, nameof(repository));
        }

        /// <inheritdoc/>
        [HttpGet("{id:guid}", Name = "GetWebsite")]
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
        [HttpGet("", Name = "GetAllWebsites")]
        public async Task<IActionResult> GetAllAsync()
        {
            var results = await this.repository.GetAllAsync().ConfigureAwait(false);
            return this.Ok(results);
        }

        /// <inheritdoc/>
        [HttpPost("", Name = "CreateWebsite")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateWebsiteRequest request)
        {
            var website = new Website
            {
                Id = Guid.NewGuid().ToString(),
                Url = request.Url,
                Name = request.Name,
                Category = request.Category,
                Description = request.Description,
                Tags = request.Tags,
                Language = request.Language,
                LastVisitAt = DateTime.Now,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                Domain = UrlParser.ExtractDomainLite(request.Url),
                Aesthetics = request.Aesthetics,
                IsSubdomain = request.IsSubdomain,
                IsMultilingual = request.IsMultilingual,
            };

            await this.repository.AddAsync(website).ConfigureAwait(false);

            return this.CreatedAtAction(
                actionName: nameof(this.GetByIdAsync),
                routeValues: new { website.Id },
                value: request);
        }

        /// <inheritdoc/>
        [HttpPut("{id:guid}", Name = "UpdateWebsite")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateWebsiteRequest request)
        {
            var website = new Website
            {
                Id = id.ToString(),
                Url = request.Url,
                Name = request.Name,
                Category = request.Category,
                Description = request.Description,
                Language = request.Language,
                Domain = UrlParser.ExtractDomainLite(request.Url),
                Aesthetics = request.Aesthetics,
                IsSubdomain = request.IsSubdomain,
                IsMultilingual = request.IsMultilingual,
            };

            try
            {
                await this.repository.UpdateAsync(website).ConfigureAwait(false);
            }
            catch (InvalidOperationException)
            {
                return this.NotFound();
            }

            return this.NoContent();
        }

        /// <inheritdoc/>
        [HttpDelete("{id:guid}", Name = "DeleteWebsite")]
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
    }
}
