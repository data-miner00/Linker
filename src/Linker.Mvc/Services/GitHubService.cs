namespace Linker.Mvc.Services;

using Linker.Common.Helpers;

public sealed class GitHubService
{
    private readonly HttpClient client;

    public GitHubService(HttpClient client)
    {
        this.client = Guard.ThrowIfNull(client);
    }

    public async Task<object?> GetByUsernameAsync(string username)
    {
        var content = await this.client.GetFromJsonAsync<object>($"users/{username}");
        return content;
    }
}
