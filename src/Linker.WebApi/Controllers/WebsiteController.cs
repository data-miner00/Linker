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
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The API controller for <see cref="Website"/>.
    /// </summary>
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

        /// <inheritdoc/>
        [HttpGet("{id:guid}", Name = "GetWebsite")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                var result = await this.repository
                    .GetByIdAsync(id.ToString(), CancellationToken.None)
                    .ConfigureAwait(false);

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
            var results = await this.repository
                .GetAllAsync(CancellationToken.None)
                .ConfigureAwait(false);

            return this.Ok(results);
        }

        /// <inheritdoc/>
        [HttpPost("", Name = "CreateWebsite")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateWebsiteRequest request)
        {
            EnsureArg.IsNotNull(request, nameof(request));

            var website = this.mapper.Map<Website>(request);

            await this.repository
                .AddAsync(website, CancellationToken.None)
                .ConfigureAwait(false);

            return this.Created();
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        [HttpGet("byuser/{userId:guid}", Name = "GetAllWebsitesByUser")]
        public async Task<IActionResult> GetAllByUserAsync(Guid userId)
        {
            var results = await this.repository
                .GetAllByUserAsync(userId.ToString(), CancellationToken.None)
                .ConfigureAwait(false);

            return this.Ok(results);
        }

        /// <inheritdoc/>
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
    }
}
