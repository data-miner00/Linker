namespace Linker.WebApi.Controllers
{
    using System.Threading.Tasks;
    using Linker.Core.ApiModels;
    using Linker.Core.Controllers;
    using Linker.Core.Models;
    using Linker.Core.Repositories;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The controller that handles everything related to authentication.
    /// </summary>
    [ApiController]
    public sealed class AuthController : ControllerBase, IAuthController
    {
        private readonly IUserRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="repository">The user repository.</param>
        public AuthController(IUserRepository repository)
        {
            this.repository = repository;
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

                if (user.Password != request.Password)
                {
                    return this.BadRequest("Wrong password");
                }

                return this.Ok();
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
            return this.Ok(response);
        }
    }
}
