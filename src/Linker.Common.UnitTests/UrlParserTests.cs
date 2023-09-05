namespace Linker.Common.UnitTests
{
    using Linker.Common;
    using Xunit;

    public class UrlParserTests
    {
        [Theory]
        [InlineData("http://www.who.org", "www.who.org")]
        [InlineData("https://www.who.org", "www.who.org")]
        [InlineData("http://youtube.com", "youtube.com")]
        [InlineData("https://youtube.com/watch?param=abc", "youtube.com")]
        [InlineData("https://medium.com/tag/programming", "medium.com")]
        public void ExtractDomainLite_GivenValidUrl_ShouldExtractDomainCorrectly(
            string url,
            string expectedDomain)
        {
            var actual = UrlParser.ExtractDomainLite(url);
            Assert.Equal(expectedDomain, actual);
        }
    }
}
