namespace Linker.Integrations.UnitTests;

public sealed class JsonStreamifierTests
{
    private readonly JsonStreamifier jsonStreamifier = new();

    [Fact]
    public async Task StreamifyAsync_GivenCollectionOfObject_ShouldMatchConvertedString()
    {
        List<User> users =
        [
            new User
            {
                Id = 1,
                Name = "Shaun",
                Age = 50,
            },
            new User
            {
                Id = 2,
                Name = "Ben",
                Age = 3,
            },
        ];

        var expectedJsonString = File.ReadAllText("data/JsonStreamifierData/StreamifyAsyncCollectionsExpectedData.json");

        var memoryStream = await this.jsonStreamifier.StreamifyAsync(users, default);

        var streamReader = new StreamReader(memoryStream);
        var actualJsonString = streamReader.ReadToEnd();

        Assert.Equal(expectedJsonString, actualJsonString);
    }

    [Fact]
    public async Task StreamifyAsync_SingleObject_ShouldMatchConvertedString()
    {
        var user = new User
        {
            Id = 1,
            Name = "Kyle",
            Age = 27,
        };

        var expectedJsonString = File.ReadAllText("data/JsonStreamifierData/StreamifyAsyncSingleObjectExpectedData.json");

        var memoryStream = await this.jsonStreamifier.StreamifyAsync(user, default);

        var streamReader = new StreamReader(memoryStream);
        var actualJsonString = streamReader.ReadToEnd();

        Assert.Equal(expectedJsonString, actualJsonString);
    }

    private sealed class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }
    }
}
