namespace Linker.WebApi.Swagger
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using Linker.Core.Models;
    using Swashbuckle.AspNetCore.Filters;

    /// <summary>
    /// The example for the Article response.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class ArticleResponseExample : IExamplesProvider<Article>
    {
        /// <inheritdoc/>
        public Article GetExamples()
        {
            return new Article
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
            };
        }

        private static string GetRandomId()
        {
            return Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
        }
    }
}
