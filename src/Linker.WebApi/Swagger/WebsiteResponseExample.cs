namespace Linker.WebApi.Swagger
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using Linker.Core.Models;
    using Linker.WebApi.ApiModels;
    using Swashbuckle.AspNetCore.Filters;

    /// <summary>
    /// The example for the Website response.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class WebsiteResponseExample : IExamplesProvider<WebsiteApiModel>
    {
        /// <inheritdoc/>
        public WebsiteApiModel GetExamples()
        {
            return new WebsiteApiModel
            {
                Id = GetRandomId(),
                Name = "TypeScript satifies Operator",
                Url = "https://www.freecodecamp.org/news/typescript-satisfies-operator/",
                Category = Category.Programming.ToString(),
                Description = "In TypeScript, the satisfies operator is a very useful tool. It was introduced in TypeScript v4.9 as an effective way to ensure type safety.",
                Tags = new[]
                {
                    "programming", "typescript", "operator",
                },
                IsMultilingual = false,
                IsSubdomain = false,
                Domain = "freecodecamp.org",
                Language = Language.English.ToString(),
                Rating = Rating.R.ToString(),
                Aesthetics = Aesthetics.Clean.ToString(),
                LastVisitAt = DateTime.Now,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                CreatedBy = GetRandomId(),
            };
        }

        private static string GetRandomId()
        {
            return Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
        }
    }
}
