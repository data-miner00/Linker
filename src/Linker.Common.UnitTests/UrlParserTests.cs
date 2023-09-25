namespace Linker.Common.UnitTests
{
    using Linker.Common;
    using Xunit;

    public sealed class UrlParserTests
    {
        [Theory]
        [InlineData("http://www.who.org", "www.who.org")]
        [InlineData("https://www.who.org", "www.who.org")]
        [InlineData("http://youtube.com", "youtube.com")]
        [InlineData("https://youtube.com/watch?param=abc", "youtube.com")]
        [InlineData("https://medium.com/tag/programming", "medium.com")]
        [InlineData("https://disquiet.io/@mrshin/makerlog/%EC%95%B1%EC%8A%A4%ED%86%A0%EC%96%B4-1%EC%9C%84-%EB%91%90-%EB%8B%AC-%EB%A7%8C%EC%97%90-dau%EA%B0%80-10%ED%86%A0%EB%A7%89%EC%9D%B4-%EB%82%98%EB%B2%84%EB%A0%B8%EB%8B%A4", "disquiet.io")]
        public void ExtractDomainLite_GivenValidUrl_ShouldExtractDomainCorrectly(
            string url,
            string expectedDomain)
        {
            var actual = UrlParser.ExtractDomainLite(url);
            Assert.Equal(expectedDomain, actual);
        }
    }
}
