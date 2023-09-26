namespace Linker.WebApi.Controllers
{
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
        public IActionResult GetAll()
        {
            var results = this.repository.GetAll();
            return this.Ok(results);
        }

        /// <inheritdoc/>
        [HttpPost("", Name = "CreateTag")]
        public IActionResult Create([FromBody] CreateTagRequest request)
        {
            this.repository.Add(request.TagName);
            return this.NoContent();
        }

        /// <inheritdoc/>
        [HttpPost("linktag/{linkId:guid}/{tagId:guid}", Name = "CreateLinkTag")]
        public IActionResult CreateLinkTag([FromRoute] Guid linkId, [FromRoute] Guid tagId)
        {
            this.repository.AddLinkTag(linkId.ToString(), tagId.ToString());
            return this.NoContent();
        }

        /// <inheritdoc/>
        [HttpPut("{id:guid}", Name = "UpdateTag")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateTagRequest request)
        {
            this.repository.EditName(id.ToString(), request.NewName);
            return this.Ok();
        }

        /// <inheritdoc/>
        [HttpDelete("{id:guid}", Name = "DeleteTag")]
        public IActionResult Delete(Guid id)
        {
            this.repository.Delete(id.ToString());
            return this.NoContent();
        }

        /// <inheritdoc/>
        [HttpDelete("linktag/{linkId:guid}/{tagId:guid}", Name = "DeleteLinkTag")]
        public IActionResult DeleteLinkTag([FromRoute] Guid linkId, [FromRoute] Guid tagId)
        {
            this.repository.DeleteLinkTag(linkId.ToString(), tagId.ToString());
            return this.NoContent();
        }
    }
}
