namespace Linker.WebApi.Swagger
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using Linker.Core.Models;
    using Linker.WebApi.ApiModels;
    using Swashbuckle.AspNetCore.Filters;

    /// <summary>
    /// The example for the Website collection response.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class WebsiteResponseCollectionExample : IExamplesProvider<IEnumerable<WebsiteApiModel>>
    {
        /// <inheritdoc/>
        public IEnumerable<WebsiteApiModel> GetExamples()
        {
            return new[]
            {
                new WebsiteApiModel
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
                },
                new WebsiteApiModel
                {
                    Id = GetRandomId(),
                    Name = "Rust traits are better interfaces",
                    Url = "https://catalin-tech.com/traits-vs-interfaces/",
                    Category = nameof(Category.Programming),
                    Description = "In this article, Catalin discusses about the perks of the Rust traits compared to interfaces of other programming languages.",
                    Tags = new[]
                    {
                        "programming", "rust", "traits", "interfaces", "oo",
                    },
                    IsMultilingual = false,
                    IsSubdomain = false,
                    Domain = "catalin-tech.com",
                    Language = nameof(Language.English),
                    Aesthetics = nameof(Aesthetics.Normal),
                    LastVisitAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    CreatedBy = GetRandomId(),
                },
            };
        }

        private static string GetRandomId()
        {
            return Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
        }
    }
}
