namespace Linker.Core.ApiModels
{
    using System.ComponentModel.DataAnnotations;
    using Linker.Core.Models;

    public sealed class UpdateWebsiteRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public Category Category { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public Language Language { get; set; }

        [Required]
        public Aesthetics Aesthetics { get; set; }

        [Required]
        public bool IsSubdomain { get; set; }

        [Required]
        public bool IsMultilingual { get; set; }
    }
}
