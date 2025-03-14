﻿namespace Linker.WebApi.Swagger
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using Linker.Core.Models;
    using Linker.WebApi.ApiModels;
    using Swashbuckle.AspNetCore.Filters;

    /// <summary>
    /// The example for the Article collection response.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class ArticleResponseCollectionExample : IExamplesProvider<IEnumerable<ArticleApiModel>>
    {
        /// <inheritdoc/>
        public IEnumerable<ArticleApiModel> GetExamples()
        {
            return new[]
            {
                new ArticleApiModel
                {
                    Id = GetRandomId(),
                    Title = "TypeScript satifies Operator",
                    Url = "https://www.freecodecamp.org/news/typescript-satisfies-operator/",
                    Author = "Satyam Tripathi",
                    Category = Category.Programming.ToString(),
                    Description = "In TypeScript, the satisfies operator is a very useful tool. It was introduced in TypeScript v4.9 as an effective way to ensure type safety.",
                    Tags = new[]
                    {
                        "programming", "typescript", "operator",
                    },
                    WatchLater = false,
                    Year = 2023,
                    Domain = "freecodecamp.org",
                    Language = Language.English.ToString(),
                    Grammar = Grammar.Average.ToString(),
                    Rating = Rating.G.ToString(),
                    LastVisitAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    CreatedBy = GetRandomId(),
                },
                new ArticleApiModel
                {
                    Id = GetRandomId(),
                    Title = "Rust traits are better interfaces",
                    Url = "https://catalin-tech.com/traits-vs-interfaces/",
                    Author = "Cătălin",
                    Category = nameof(Category.Programming),
                    Description = "In this article, Catalin discusses about the perks of the Rust traits compared to interfaces of other programming languages.",
                    Tags = new[]
                    {
                        "programming", "rust", "traits", "interfaces", "oo",
                    },
                    WatchLater = true,
                    Year = 2023,
                    Domain = "catalin-tech.com",
                    Language = nameof(Language.English),
                    Grammar = nameof(Grammar.Average),
                    Rating = nameof(Rating.G),
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
