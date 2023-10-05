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
        public async Task<IActionResult> CreateAsync([FromBody] CreateYoutubeRequest request)
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

            await this.repository.AddAsync(channel).ConfigureAwait(false);

            return this.CreatedAtAction(
                actionName: nameof(this.GetByIdAsync),
                routeValues: new { channel.Id },
                value: request);
        }

        /// <inheritdoc/>
        [HttpDelete("{id:guid}", Name = "DeleteYoutube")]
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
        [HttpGet("", Name = "GetAllYoutubes")]
        public async Task<IActionResult> GetAllAsync()
        {
            var results = await this.repository.GetAllAsync().ConfigureAwait(false);
            return this.Ok(results);
        }

        /// <inheritdoc/>
        [HttpGet("{id:guid}", Name = "GetYoutube")]
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
        [HttpPut("{id:guid}", Name = "UpdateYoutube")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateYoutubeRequest request)
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
                await this.repository.UpdateAsync(channel).ConfigureAwait(false);
            }
            catch (InvalidOperationException)
            {
                return this.NotFound();
            }

            return this.NoContent();
        }
    }
}
