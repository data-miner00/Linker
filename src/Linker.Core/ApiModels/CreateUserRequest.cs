namespace Linker.Core.ApiModels
{
    /// <summary>
    /// Represents the create user request.
    /// </summary>
    /// <param name="Username">The username.</param>
    /// <param name="Password">The password.</param>
    public record CreateUserRequest(string Username, string Password);
}
