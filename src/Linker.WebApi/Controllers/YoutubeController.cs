﻿namespace Linker.WebApi.Controllers
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
    /// The API controller for <see cref="Youtube"/>.
    /// </summary>
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

        /// <inheritdoc/>
        [HttpPost("", Name = "CreateYoutube")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateYoutubeRequest request)
        {
            EnsureArg.IsNotNull(request, nameof(request));

            var userId = this.context.HttpContext?.User
                .FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

            if (userId != request.CreatedBy)
            {
                return this.Forbid();
            }

            var channel = this.mapper.Map<Youtube>(request);

            await this.repository
                .AddAsync(channel, CancellationToken.None)
                .ConfigureAwait(false);

            return this.Created();
        }

        /// <inheritdoc/>
        [HttpDelete("{id:guid}", Name = "DeleteYoutube")]
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
        [HttpGet("", Name = "GetAllYoutubes")]
        public async Task<IActionResult> GetAllAsync()
        {
            var results = await this.repository
                .GetAllAsync(CancellationToken.None)
                .ConfigureAwait(false);

            return this.Ok(results);
        }

        /// <inheritdoc/>
        [HttpGet("{id:guid}", Name = "GetYoutube")]
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
        [HttpPut("{id:guid}", Name = "UpdateYoutube")]
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
                    return this.Forbid();
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
    }
}
