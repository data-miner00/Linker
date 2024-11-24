namespace Linker.WebApi.Swagger
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using Linker.Core.Models;
    using Linker.WebApi.ApiModels;
    using Swashbuckle.AspNetCore.Filters;

    /// <summary>
    /// The example for the Youtube response.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class YoutubeResponseExample : IExamplesProvider<YoutubeApiModel>
    {
        /// <inheritdoc/>
        public YoutubeApiModel GetExamples()
        {
            return new YoutubeApiModel
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
            };
        }

        private static string GetRandomId()
        {
            return Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
        }
    }
}
