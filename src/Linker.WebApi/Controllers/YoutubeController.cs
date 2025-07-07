namespace Linker.WebApi.Controllers
{
    using System;
    using System.Net;
    using System.Security.Claims;
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
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using Swashbuckle.AspNetCore.Filters;

    /// <summary>
    /// The API controller for <see cref="Youtube"/>.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public sealed class YoutubeController : ControllerBase, IYoutubeController
    {
        private readonly IYoutubeRepository repository;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// Initializes a new instance of the <see cref="YoutubeController"/> class.
        /// </summary>
        /// <param name="repository">The repository for <see cref="Youtube"/>.</param>
        /// <param name="mapper">The mapper instance.</param>
        /// <param name="context">The HTTP context accessor.</param>
        public YoutubeController(
            IYoutubeRepository repository,
            IMapper mapper,
            IHttpContextAccessor context)
        {
            this.repository = EnsureArg.IsNotNull(repository, nameof(repository));
            this.mapper = EnsureArg.IsNotNull(mapper, nameof(mapper));
            this.context = EnsureArg.IsNotNull(context, nameof(context));
        }

        /// <summary>
        /// Gets the current user's ID.
        /// </summary>
        public string UserId =>
            this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        /// <inheritdoc/>
        [HttpPost("", Name = "CreateYoutube")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Youtube link created")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateYoutubeRequest request)
        {
            EnsureArg.IsNotNull(request, nameof(request));

            var channel = this.mapper.Map<Youtube>(request);
            channel.CreatedBy = this.UserId;

            await this.repository
                .AddAsync(channel, CancellationToken.None)
                .ConfigureAwait(false);

            return this.Created();
        }

        #region Privilege

        /// <inheritdoc/>
        [RoleAuthorize(Role.Administrator)]
        [HttpDelete("{id:guid}", Name = "DeleteYoutube")]
        [SwaggerResponse((int)HttpStatusCode.NoContent, "Youtube successfully deleted.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Youtube not found.")]
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
        [HttpGet("", Name = "GetAllYoutubes")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Retrieved all youtubes.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(YoutubeResponseCollectionExample))]
        [ProducesResponseType(typeof(YoutubeResponseCollectionExample), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllAsync()
        {
            var channels = await this.repository
                .GetAllAsync(CancellationToken.None)
                .ConfigureAwait(false);

            var results = channels.Select(this.mapper.Map<YoutubeApiModel>)
                .ToList();

            return this.Ok(results);
        }

        /// <inheritdoc/>
        [RoleAuthorize(Role.Administrator)]
        [HttpGet("{id:guid}", Name = "GetYoutube")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Youtube not found.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Retrieved youtube.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(YoutubeResponseExample))]
        [ProducesResponseType(typeof(YoutubeResponseExample), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                var youtube = await this.repository
                    .GetByIdAsync(id.ToString(), CancellationToken.None)
                    .ConfigureAwait(false);

                var result = this.mapper.Map<YoutubeApiModel>(youtube);

                return this.Ok(result);
            }
            catch (InvalidOperationException)
            {
                return this.NotFound();
            }
        }

        /// <inheritdoc/>
        [RoleAuthorize(Role.Administrator)]
        [HttpPut("{id:guid}", Name = "UpdateYoutube")]
        [SwaggerResponse((int)HttpStatusCode.NoContent, "Youtube updated.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Youtube not found.")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateYoutubeRequest request)
        {
            EnsureArg.IsNotNull(request, nameof(request));

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
            }
            catch (InvalidOperationException)
            {
                return this.NotFound();
            }

            var channel = this.mapper.Map<Youtube>(request);
            channel.Id = id.ToString();

            await this.repository
                .UpdateAsync(channel, CancellationToken.None)
                .ConfigureAwait(false);

            return this.NoContent();
        }

        #endregion

        #region User

        /// <inheritdoc/>
        [AccountAuthorize]
        [HttpDelete("byuser/{userId:guid}/{linkId:guid}", Name = "DeleteYoutubeByUser")]
        [SwaggerResponse((int)HttpStatusCode.NoContent, "Youtube successfully deleted.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Youtube not found.")]
        public async Task<IActionResult> DeleteByUserAsync(Guid userId, Guid linkId)
        {
            try
            {
                var existingLink = await this.repository
                    .GetByIdAsync(linkId.ToString(), CancellationToken.None)
                    .ConfigureAwait(false);

                if (!existingLink.CreatedBy.Equals(userId.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    return this.Unauthorized();
                }

                await this.repository
                    .RemoveAsync(linkId.ToString(), CancellationToken.None)
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
        [HttpGet("byuser/{userId:guid}", Name = "GetAllYoutubesByUser")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Retrieved all youtubes.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(YoutubeResponseCollectionExample))]
        [ProducesResponseType(typeof(YoutubeResponseCollectionExample), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllByUserAsync(Guid userId)
        {
            var results = await this.repository
                .GetAllByUserAsync(userId.ToString(), CancellationToken.None)
                .ConfigureAwait(false);

            return this.Ok(results);
        }

        /// <inheritdoc/>
        [AccountAuthorize]
        [HttpGet("byuser/{userId:guid}/{linkId:guid}", Name = "GetYoutubeByUser")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Youtube not found.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Retrieved youtube.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(YoutubeResponseExample))]
        [ProducesResponseType(typeof(YoutubeResponseExample), (int)HttpStatusCode.OK)]
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
        [HttpPut("byuser/{userId:guid}/{linkId:guid}", Name = "UpdateYoutubeByUser")]
        [SwaggerResponse((int)HttpStatusCode.NoContent, "Youtube updated.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Youtube not found.")]
        public async Task<IActionResult> UpdateByUserAsync(
            [FromRoute] Guid userId,
            [FromRoute] Guid linkId,
            [FromBody] UpdateYoutubeRequest request)
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

                var youtube = this.mapper.Map<Youtube>(request);
                youtube.Id = linkId.ToString();

                await this.repository
                    .UpdateAsync(youtube, CancellationToken.None)
                    .ConfigureAwait(false);

                return this.NoContent();
            }
            catch (InvalidOperationException)
            {
                return this.NotFound();
            }
        }
        #endregion
    }
}
