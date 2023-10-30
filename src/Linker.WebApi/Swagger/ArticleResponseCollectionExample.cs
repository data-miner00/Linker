namespace Linker.WebApi.Swagger
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using Linker.Core.Models;
    using Swashbuckle.AspNetCore.Filters;

    /// <summary>
    /// The example for the Article collection response.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class ArticleResponseCollectionExample : IExamplesProvider<IEnumerable<Article>>
    {
        /// <inheritdoc/>
        public IEnumerable<Article> GetExamples()
        {
            return new[]
            {
                new Article
                {
                    Id = GetRandomId(),
                    Title = "TypeScript satifies Operator",
                    Url = "https://www.freecodecamp.org/news/typescript-satisfies-operator/",
                    Author = "Satyam Tripathi",
                    Category = Category.Programming,
                    Description = "In TypeScript, the satisfies operator is a very useful tool. It was introduced in TypeScript v4.9 as an effective way to ensure type safety.",
                    Tags = new[]
                    {
                        "programming", "typescript", "operator",
                    },
                    WatchLater = false,
                    Year = 2023,
                    Domain = "freecodecamp.org",
                    Language = Language.English,
                    Grammar = Grammar.Average,
                    LastVisitAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                },
                new Article
                {
                    Id = GetRandomId(),
                    Title = "Rust traits are better interfaces",
                    Url = "https://catalin-tech.com/traits-vs-interfaces/",
                    Author = "Cătălin",
                    Category = Category.Programming,
                    Description = "In this article, Catalin discusses about the perks of the Rust traits compared to interfaces of other programming languages.",
                    Tags = new[]
                    {
                        "programming", "rust", "traits", "interfaces", "oo",
                    },
                    WatchLater = true,
                    Year = 2023,
                    Domain = "catalin-tech.com",
                    Language = Language.English,
                    Grammar = Grammar.Average,
                    LastVisitAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                },
            };
        }

        private static string GetRandomId()
        {
            return Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
        }
    }
}
