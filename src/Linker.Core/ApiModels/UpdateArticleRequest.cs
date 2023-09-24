namespace Linker.Core.ApiModels
{
    using System.ComponentModel.DataAnnotations;
    using Linker.Core.Models;

    public sealed class UpdateArticleRequest
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public Category Category { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public Language Language { get; set; }

        [Required]
        public Aesthetics Aesthetics { get; set; }

        [Required]
        public Grammar Grammar { get; set; }

        [Range(1970, 2100)]
        public int Year { get; set; }

        [Required]
        public bool WatchLater { get; set; }
    }
}
