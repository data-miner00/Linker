namespace Linker.WebApi.Controllers
{
    using System.Data.SQLite;
    using System.Threading.Tasks;
    using EnsureThat;
    using Linker.Core.ApiModels;
    using Linker.Core.Controllers;
    using Linker.Core.Models;
    using Linker.Core.Repositories;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The API controller for <see cref="Tag"/>.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public sealed class TagController : ControllerBase, ITagController
    {
        private readonly ITagRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagController"/> class.
        /// </summary>
        /// <param name="repository">The repository for <see cref="Tag"/>.</param>
        public TagController(ITagRepository repository)
        {
            this.repository = EnsureArg.IsNotNull(repository, nameof(repository));
        }

        /// <inheritdoc/>
        [HttpGet("", Name = "GetAllTags")]
        public async Task<IActionResult> GetAllAsync()
        {
            var results = await this.repository.GetAllAsync().ConfigureAwait(false);
            return this.Ok(results);
        }

        /// <inheritdoc/>
        [HttpGet("query", Name = "GetBy")]
        public async Task<IActionResult> GetByAsync([FromQuery] string? id, [FromQuery] string? name)
        {
            try
            {
                Tag tag;

                if (!string.IsNullOrEmpty(id))
                {
                    tag = await this.repository.GetByAsync("Id", id).ConfigureAwait(false);
                }
                else if (!string.IsNullOrEmpty(name))
                {
                    tag = await this.repository.GetByAsync("Name", name).ConfigureAwait(false);
                }
                else
                {
                    return this.BadRequest("Either Id or Name must be specified");
                }

                return this.Ok(tag);
            }
            catch (InvalidOperationException)
            {
                return this.NotFound();
            }
        }

        /// <inheritdoc/>
        [HttpPost("", Name = "CreateTag")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateTagRequest request)
        {
            try
            {
                await this.repository.AddAsync(request.TagName).ConfigureAwait(false);
                return this.NoContent();
            }
            catch (SQLiteException ex) when (ex.Message.Equals("constraint failed\r\nUNIQUE constraint failed: Tags.Name"))
            {
                return this.BadRequest("The tag with the same name already exists.");
            }
        }

        /// <inheritdoc/>
        [HttpPost("linktag/{linkId:guid}/{tagId:guid}", Name = "CreateLinkTag")]
        public async Task<IActionResult> CreateLinkTagAsync([FromRoute] Guid linkId, [FromRoute] Guid tagId)
        {
            await this.repository.AddLinkTagAsync(linkId.ToString(), tagId.ToString()).ConfigureAwait(false);
            return this.NoContent();
        }

        /// <inheritdoc/>
        [HttpPut("{id:guid}", Name = "UpdateTag")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateTagRequest request)
        {
            await this.repository.EditNameAsync(id.ToString(), request.NewName).ConfigureAwait(false);
            return this.Ok();
        }

        /// <inheritdoc/>
        [HttpDelete("{id:guid}", Name = "DeleteTag")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await this.repository.DeleteAsync(id.ToString()).ConfigureAwait(false);
            return this.NoContent();
        }

        /// <inheritdoc/>
        [HttpDelete("linktag/{linkId:guid}/{tagId:guid}", Name = "DeleteLinkTag")]
        public async Task<IActionResult> DeleteLinkTagAsync([FromRoute] Guid linkId, [FromRoute] Guid tagId)
        {
            await this.repository.DeleteLinkTagAsync(linkId.ToString(), tagId.ToString()).ConfigureAwait(false);
            return this.NoContent();
        }
    }
}
