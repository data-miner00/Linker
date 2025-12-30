namespace Linker.WebApi.Controllers
{
    using System.Diagnostics.Metrics;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using AutoMapper;
    using EnsureThat;
    using Linker.Core.ApiModels;
    using Linker.Core.Controllers;
    using Linker.Core.Models;
    using Linker.Core.Repositories;
    using Linker.WebApi.Filters;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http.Timeouts;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The controller that handles everything related to authentication.
    /// </summary>
    [ApiController]
    public sealed class AuthController : ControllerBase, IAuthController
    {
        private static readonly string AuthScheme = "cookie";
        private readonly IUserRepository repository;
        private readonly IMeterFactory meterFactory;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="repository">The user repository.</param>
        /// <param name="meterFactory">The meter factory.</param>
        /// <param name="mapper">The mapper.</param>
        public AuthController(
            IUserRepository repository,
            IMeterFactory meterFactory,
            IMapper mapper)
        {
            this.repository = EnsureArg.IsNotNull(repository, nameof(repository));
            this.meterFactory = EnsureArg.IsNotNull(meterFactory, nameof(meterFactory));
            this.mapper = EnsureArg.IsNotNull(mapper, nameof(mapper));
        }

        /// <inheritdoc/>
        [HttpPost("/login", Name = "Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(LocalInformerFilter))]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var meter = this.meterFactory.Create("Linker");
            var instrument = meter.CreateCounter<int>("login_counter");
            instrument.Add(1);

            try
            {
                var user = await this.repository
                    .GetByUsernameAndPasswordAsync(request.Username, request.Password, cancellationToken)
                    .ConfigureAwait(false);

                var claims = new Claim[]
                {
                    new(ClaimTypes.NameIdentifier, user.Id),
                    new(ClaimTypes.Name, user.Username),
                    new(ClaimTypes.Role, user.Role.ToString()),
                    new(ClaimTypes.DateOfBirth, user.DateOfBirth.ToShortDateString()),
                };
                var claimsIdentity = new ClaimsIdentity(claims, AuthScheme);
                var principal = new ClaimsPrincipal(claimsIdentity);

                return this.SignIn(principal, AuthScheme);
            }
            catch (InvalidOperationException)
            {
                return this.Unauthorized("Invalid username or password.");
            }
        }

        /// <inheritdoc/>
        [HttpPost("/register", Name = "RegisterUser")]
        public async Task<IActionResult> RegisterAsync([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = this.mapper.Map<User>(request);

            await this.repository.AddAsync(user, cancellationToken)
                .ConfigureAwait(false);

            var response = this.mapper.Map<CreateUserResponse>(user);

            return this.Created("/login", response);
        }

        /// <inheritdoc/>
        [HttpPost("/logout", Name = "Logout")]
        [Authorize]
        public Task<IActionResult> LogoutAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(this.SignOut() as IActionResult);
        }

        [HttpGet("/is_authenticated")]
        [Authorize("minimum_role")]
        [RequestTimeout("MoreThanTenSeconds")]
        public async Task<IActionResult> IsAuthenticated()
        {
            await Task.Delay(TimeSpan.FromSeconds(8));

            var user = this.HttpContext.User;
            return this.Ok(user.Identity?.IsAuthenticated);
        }

        [HttpGet("/is_old")]
        [MinimumAgeAuthorize(18)]
        public IActionResult IsOldEnough()
        {
            return this.Ok("ok");
        }
    }
}
