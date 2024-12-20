﻿namespace Linker.WebApi.Swagger
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using Linker.Core.Models;
    using Linker.WebApi.ApiModels;
    using Swashbuckle.AspNetCore.Filters;

    /// <summary>
    /// The example for the Article response.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class ArticleResponseExample : IExamplesProvider<ArticleApiModel>
    {
        /// <inheritdoc/>
        public ArticleApiModel GetExamples()
        {
            return new ArticleApiModel
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
                Rating = Rating.R.ToString(),
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
