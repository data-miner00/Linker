namespace Linker.WebApi.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using EnsureThat;
    using Linker.Core.ApiModels;
    using Linker.Core.Controllers;
    using Linker.Core.Models;
    using Linker.Core.Repositories;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The controller that handles everything related to authentication.
    /// </summary>
    [ApiController]
    public sealed class AuthController : ControllerBase, IAuthController
    {
        private static readonly string AuthScheme = "cookie";
        private readonly IUserRepository repository;
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="repository">The user repository.</param>
        /// <param name="context">The http context accessor.</param>
        public AuthController(IUserRepository repository, IHttpContextAccessor context)
        {
            this.repository = EnsureArg.IsNotNull(repository, nameof(repository));
            this.context = EnsureArg.IsNotNull(context, nameof(context));
        }

        /// <inheritdoc/>
        [HttpPost("/login", Name = "Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var user = await this.repository.GetByUsernameAsync(request.Username, cancellationToken)
                    .ConfigureAwait(false);

                var claims = new Claim[]
                {
                    new(ClaimTypes.NameIdentifier, user.Id),
                    new(ClaimTypes.Name, user.Username),
                    new(ClaimTypes.Role, user.Role.ToString()),
                };
                var claimsIdentity = new ClaimsIdentity(claims, AuthScheme);
                var principal = new ClaimsPrincipal(claimsIdentity);

                if (user.Password != request.Password)
                {
                    return this.BadRequest("Wrong password");
                }

                return this.SignIn(principal, AuthScheme);
            }
            catch (InvalidOperationException)
            {
                return this.BadRequest("User not found");
            }
        }

        /// <inheritdoc/>
        [HttpPost("/register", Name = "RegisterUser")]
        public async Task<IActionResult> RegisterAsync([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = request.Username,
                Password = request.Password,
                Role = Role.User,
                Status = Status.Active,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
            };

            await this.repository.AddAsync(user, cancellationToken)
                .ConfigureAwait(false);

            var response = new CreateUserResponse(user.Id, user.Username, user.Role, user.Status, user.CreatedAt, user.ModifiedAt);
            return this.Created("/login", response);
        }

        /// <inheritdoc/>
        [HttpPost("/logout", Name = "Logout")]
        [Authorize]
        public async Task<IActionResult> LogoutAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return this.SignOut();
        }

        [HttpGet("/is_authenticated")]
        [Authorize("minimum_role")]
        public IActionResult IsAuthenticated()
        {
            var result = this.context.HttpContext.User;
            return this.Ok();
        }
    }
}
