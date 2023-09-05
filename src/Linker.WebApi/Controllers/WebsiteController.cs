namespace Linker.WebApi.Controllers
{
    using System;
    using EnsureThat;
    using Linker.Common;
    using Linker.Core.ApiModels;
    using Linker.Core.Models;
    using Linker.Data.SQLite;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class WebsiteController : ControllerBase
    {
        private readonly WebsiteRepository repository;

        public WebsiteController(WebsiteRepository repository)
        {
            this.repository = EnsureArg.IsNotNull(repository, nameof(repository));
        }

        [HttpGet("{id:guid}", Name = "GetWebsite")]
        public IActionResult Get(Guid id)
        {
            var website = this.repository.GetById(id.ToString());

            return this.Ok(website);
        }

        [HttpGet("", Name = "GetAllWebsites")]
        public IActionResult GetAll()
        {
            var websites = this.repository.GetAll();
            return this.Ok(websites);
        }

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
                actionName: nameof(this.Get),
                routeValues: new { website.Id },
                value: request);
        }

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

        [HttpDelete("{id:guid}", Name = "DeleteWebsite")]
        public IActionResult Delete(Guid id)
        {
            this.repository.Remove(id.ToString());
            return this.NoContent();
        }
    }
}
