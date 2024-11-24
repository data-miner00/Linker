namespace Linker.WebApi.Controllers
{
    using System;
    using System.Security.Claims;
    using AutoMapper;
    using EnsureThat;
    using Linker.Core.ApiModels;
    using Linker.Core.Controllers;
    using Linker.Core.Models;
    using Linker.Core.Repositories;
    using Linker.WebApi.ApiModels;
    using Linker.WebApi.Filters;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The API controller for <see cref="Website"/>.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WebsiteController : ControllerBase, IWebsiteController
    {
        private readonly IWebsiteRepository repository;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebsiteController"/> class.
        /// </summary>
        /// <param name="repository">The repository for <see cref="Website"/>.</param>
        /// <param name="mapper">The mapper instance.</param>
        /// <param name="context">The HTTP context accessor.</param>
        public WebsiteController(
            IWebsiteRepository repository,
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
        [HttpPost("", Name = "CreateWebsite")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateWebsiteRequest request)
        {
            EnsureArg.IsNotNull(request, nameof(request));

            var website = this.mapper.Map<Website>(request);
            website.CreatedBy = this.UserId;

            await this.repository
                .AddAsync(website, CancellationToken.None)
                .ConfigureAwait(false);

            return this.Created();
        }

        #region Privilege

        /// <inheritdoc/>
        [RoleAuthorize(Role.Administrator)]
        [HttpGet("{id:guid}", Name = "GetWebsite")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                var website = await this.repository
                    .GetByIdAsync(id.ToString(), CancellationToken.None)
                    .ConfigureAwait(false);

                var result = this.mapper.Map<WebsiteApiModel>(website);

                return this.Ok(result);
            }
            catch (InvalidOperationException)
            {
                return this.NotFound();
            }
        }

        /// <inheritdoc/>
        [RoleAuthorize(Role.Administrator)]
        [HttpGet("", Name = "GetAllWebsites")]
        public async Task<IActionResult> GetAllAsync()
        {
            var websites = await this.repository
                .GetAllAsync(CancellationToken.None)
                .ConfigureAwait(false);

            var results = websites.Select(this.mapper.Map<WebsiteApiModel>)
                .ToList();

            return this.Ok(results);
        }

        /// <inheritdoc/>
        [RoleAuthorize(Role.Administrator)]
        [HttpPut("{id:guid}", Name = "UpdateWebsite")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateWebsiteRequest request)
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
                    return this.Forbid();
                }
            }
            catch (InvalidOperationException)
            {
                return this.NotFound();
            }

            var website = this.mapper.Map<Website>(request);
            website.Id = id.ToString();

            await this.repository
                .UpdateAsync(website, CancellationToken.None)
                .ConfigureAwait(false);

            return this.NoContent();
        }

        /// <inheritdoc/>
        [RoleAuthorize(Role.Administrator)]
        [HttpDelete("{id:guid}", Name = "DeleteWebsite")]
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
        #endregion

        #region User

        /// <inheritdoc/>
        [AccountAuthorize]
        [HttpGet("byuser/{userId:guid}", Name = "GetAllWebsitesByUser")]
        public async Task<IActionResult> GetAllByUserAsync(Guid userId)
        {
            var results = await this.repository
                .GetAllByUserAsync(userId.ToString(), CancellationToken.None)
                .ConfigureAwait(false);

            return this.Ok(results);
        }

        /// <inheritdoc/>
        [AccountAuthorize]
        [HttpGet("byuser/{userId:guid}/{linkId:guid}", Name = "GetWebsiteByUser")]
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
        [HttpPut("/byuser/{userId:guid}/{linkId:guid}", Name = "UpdateWebsiteByUser")]
        public async Task<IActionResult> UpdateByUserAsync(
            [FromRoute] Guid userId,
            [FromRoute] Guid linkId,
            [FromBody] UpdateWebsiteRequest request)
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

                var website = this.mapper.Map<Website>(request);
                website.Id = linkId.ToString();

                await this.repository
                    .UpdateAsync(website, CancellationToken.None)
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
        [HttpDelete("/byuser/{userId:guid}/{linkId:guid}", Name = "DeleteWebsiteByUser")]
        public async Task<IActionResult> DeleteByUserAsync(
            [FromRoute] Guid userId,
            [FromRoute] Guid linkId)
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
        #endregion
    }
}
