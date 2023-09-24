namespace Linker.WebApi.Controllers
{
    using System;
    using EnsureThat;
    using Linker.Core.ApiModels;
    using Linker.Core.Controllers;
    using Linker.Core.Models;
    using Linker.Core.Repositories;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The API controller for <see cref="Youtube"/>.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public sealed class YoutubeController : ControllerBase, IYoutubeController
    {
        private readonly IYoutubeRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="YoutubeController"/> class.
        /// </summary>
        /// <param name="repository">The repository for <see cref="Youtube"/>.</param>
        public YoutubeController(IYoutubeRepository repository)
        {
            this.repository = EnsureArg.IsNotNull(repository, nameof(repository));
        }

        /// <inheritdoc/>
        [HttpPost("", Name = "CreateYoutube")]
        public IActionResult Create([FromBody] CreateYoutubeRequest request)
        {
            var channel = new Youtube
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
                Youtuber = request.Youtuber,
                Country = request.Country,
            };

            this.repository.Add(channel);

            return this.CreatedAtAction(
                actionName: nameof(this.GetById),
                routeValues: new { channel.Id },
                value: request);
        }

        /// <inheritdoc/>
        [HttpDelete("{id:guid}", Name = "DeleteYoutube")]
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
        [HttpGet("", Name = "GetAllYoutubes")]
        public IActionResult GetAll()
        {
            var results = this.repository.GetAll();
            return this.Ok(results);
        }

        /// <inheritdoc/>
        [HttpGet("{id:guid}", Name = "GetYoutube")]
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
        [HttpPut("{id:guid}", Name = "UpdateYoutube")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateYoutubeRequest request)
        {
            var channel = new Youtube
            {
                Id = id.ToString(),
                Url = request.Url,
                Name = request.Name,
                Category = request.Category,
                Description = request.Description,
                Language = request.Language,
                Youtuber = request.Youtuber,
                Country = request.Country,
            };

            try
            {
                this.repository.Update(channel);
            }
            catch (InvalidOperationException)
            {
                return this.NotFound();
            }

            return this.NoContent();
        }
    }
}
