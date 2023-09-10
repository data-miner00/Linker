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
        public IActionResult GetById(Guid id)
        {
            var website = this.repository.GetById(id.ToString());

            return this.Ok(website);
        }

        /// <inheritdoc/>
        [HttpGet("", Name = "GetAllWebsites")]
        public IActionResult GetAll()
        {
            var websites = this.repository.GetAll();
            return this.Ok(websites);
        }

        /// <inheritdoc/>
        [HttpPost("", Name = "CreateWebsite")]
        public IActionResult Create([FromBody] CreateWebsiteRequest request)
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

            this.repository.Add(website);

            return this.CreatedAtAction(
                actionName: nameof(this.GetById),
                routeValues: new { website.Id },
                value: request);
        }

        /// <inheritdoc/>
        [HttpPut("{id:guid}", Name = "UpdateWebsite")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateWebsiteRequest request)
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

            this.repository.Update(website);

            return this.NoContent();
        }

        /// <inheritdoc/>
        [HttpDelete("{id:guid}", Name = "DeleteWebsite")]
        public IActionResult Delete(Guid id)
        {
            this.repository.Remove(id.ToString());
            return this.NoContent();
        }
    }
}
