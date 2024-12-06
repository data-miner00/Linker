namespace Linker.Integrations.UnitTests;

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Serialization;

public sealed class XmlStreamifierTests
{
    private readonly XmlStreamifier xmlStreamifier = new();

    [Fact]
    public async Task StreamifyAsync_GivenCollectionsOfObject_ShouldMatchConvertedString()
    {
        List<User> users =
        [
            new User
            {
                Id = 1,
                Name = "Shaun",
                Email = "shaun@gmail.com",
                Password = "password",
            },
            new User
            {
                Id = 2,
                Name = "Ben",
                Email = "ben@yahoo.com",
                Password = "password",
            },
        ];

        var expectedXmlString = File.ReadAllText("data/XmlStreamifierData/StreamifyAsyncCollectionsExpectedData.xml");

        var memoryStream = await this.xmlStreamifier.StreamifyAsync(users, default);

        var streamReader = new StreamReader(memoryStream);
        var actualXmlString = streamReader.ReadToEnd();

        Assert.Equal(expectedXmlString, actualXmlString);
    }

    [Fact]
    public async Task StreamifyAsync_SingleObject_ShouldMatchConvertedString()
    {
        var user = new User
        {
            Id = 1,
            Name = "Kyle",
            Email = "kyle@cmail.com",
        };

        var expectedXmlString = File.ReadAllText("data/XmlStreamifierData/StreamifyAsyncSingleObjectExpectedData.xml");

        var memoryStream = await this.xmlStreamifier.StreamifyAsync(user, default);

        var streamReader = new StreamReader(memoryStream);
        var actualXmlString = streamReader.ReadToEnd();

        Assert.Equal(expectedXmlString, actualXmlString);
    }

    [XmlRoot(Namespace = "http://example.com")]
    public sealed class User
    {
        public int Id { get; set; }

        [XmlElement("FullName")]
        public string Name { get; set; }

        public string Email { get; set; }

        [XmlIgnore]
        public string Password { get; set; }
    }
}
