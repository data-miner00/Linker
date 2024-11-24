namespace Linker.WebApi.Swagger
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using Linker.Core.Models;
    using Linker.WebApi.ApiModels;
    using Swashbuckle.AspNetCore.Filters;

    /// <summary>
    /// The example for the Youtube collection response.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class YoutubeResponseCollectionExample : IExamplesProvider<IEnumerable<YoutubeApiModel>>
    {
        /// <inheritdoc/>
        public IEnumerable<YoutubeApiModel> GetExamples()
        {
            return new[]
            {
                new YoutubeApiModel
                {
                    Id = GetRandomId(),
                    Name = "TypeScript satifies Operator",
                    Url = "https://www.freecodecamp.org/news/typescript-satisfies-operator/",
                    Youtuber = "Satyam Tripathi",
                    Category = Category.Programming.ToString(),
                    Description = "In TypeScript, the satisfies operator is a very useful tool. It was introduced in TypeScript v4.9 as an effective way to ensure type safety.",
                    Tags = new[]
                    {
                        "programming", "typescript", "operator",
                    },
                    Country = "United States",
                    Language = Language.English.ToString(),
                    Rating = Rating.R.ToString(),
                    LastVisitAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    CreatedBy = GetRandomId(),
                },
                new YoutubeApiModel
                {
                    Id = GetRandomId(),
                    Name = "Rust traits are better interfaces",
                    Url = "https://catalin-tech.com/traits-vs-interfaces/",
                    Youtuber = "Catalin",
                    Category = nameof(Category.Programming),
                    Description = "In this article, Catalin discusses about the perks of the Rust traits compared to interfaces of other programming languages.",
                    Tags = new[]
                    {
                        "programming", "rust", "traits", "interfaces", "oo",
                    },
                    Country = "Austria",
                    Language = nameof(Language.English),
                    Rating = nameof(Rating.NC17),
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
