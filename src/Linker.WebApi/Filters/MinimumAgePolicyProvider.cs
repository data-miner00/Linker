namespace Linker.WebApi.Filters;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

/// <summary>
/// The minimum age authorization policy provider.
/// </summary>
public sealed class MinimumAgePolicyProvider : IAuthorizationPolicyProvider
{
#pragma warning disable SA1310 // Field names should not contain underscore
    private const string POLICY_PREFIX = "MinimumAge";
#pragma warning restore SA1310 // Field names should not contain underscore

    /// <summary>
    /// Initializes a new instance of the <see cref="MinimumAgePolicyProvider"/> class.
    /// </summary>
    /// <param name="options">The authorization options.</param>
    public MinimumAgePolicyProvider(IOptions<AuthorizationOptions> options)
    {
        this.BackupPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }

    private DefaultAuthorizationPolicyProvider BackupPolicyProvider { get; }

    /// <inheritdoc/>
    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        var policyBuilder = new AuthorizationPolicyBuilder("cookie")
            .RequireAuthenticatedUser()
            .Build();

        return Task.FromResult(policyBuilder);
    }

    /// <inheritdoc/>
    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        return Task.FromResult<AuthorizationPolicy?>(null);
    }

    /// <inheritdoc/>
    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(POLICY_PREFIX, StringComparison.OrdinalIgnoreCase) &&
            int.TryParse(policyName.Substring(POLICY_PREFIX.Length), out var age))
        {
            var policy = new AuthorizationPolicyBuilder("cookie");
            policy.AddRequirements(new MinimumAgeRequirement(age));
            return Task.FromResult(policy.Build());
        }

        return this.BackupPolicyProvider.GetPolicyAsync(policyName);
    }
}
