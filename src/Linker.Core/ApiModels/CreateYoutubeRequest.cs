namespace Linker.Core.ApiModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Linker.Core.Models;

    public sealed class CreateYoutubeRequest
    {
        [Required]
        public string Url { get; set; }

        [Required]
        public Category Category { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public Language Language { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Youtuber { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public IEnumerable<string> Tags { get; set; }
    }
}
