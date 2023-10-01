namespace Linker.ConsoleUI.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Linker.Core.Models;
    using Linker.Core.Repositories;

    /// <summary>
    /// Simple in-memory website repository.
    /// </summary>
    [Obsolete(message: "This class is not in use.")]
    public sealed class WebsiteRepository : IRepository<Website>, ITransactionalRepository
{
        private readonly IList<Website> links;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebsiteRepository"/> class.
        /// </summary>
        public WebsiteRepository()
        {
            // Populates 3 dummy records
            this.links = new List<Website>
            {
                new Website
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Google",
                    Url = "https://www.google.com",
                    Domain = "www.google.com",
                    Description = "The world's famous search engine",
                    Tags = new List<string> { "search", "famous" },
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                },
                new Website
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Google",
                    Url = "https://www.google.com",
                    Domain = "www.google.com",
                    Description = "The world's famous search engine",
                    Tags = new List<string> { "search", "famous" },
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                },
                new Website
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Google",
                    Url = "https://www.google.com",
                    Domain = "www.google.com",
                    Description = "The world's famous search engine",
                    Tags = new List<string> { "search", "famous" },
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                },
            };
        }

        /// <inheritdoc/>
        public void Add(Website link)
        {
            link.Id = Guid.NewGuid().ToString();
            link.Domain = ExtractDomainFromUrl(link.Url);
            link.CreatedAt = DateTime.Now;
            link.ModifiedAt = DateTime.Now;

            this.links.Add(link);
        }

        /// <inheritdoc/>
        public int Commit()
        {
            return 0;
        }

        /// <inheritdoc/>
        public IEnumerable<Website> GetAll()
        {
            return from l in this.links
                   orderby l.CreatedAt
                   select l;
        }

        /// <inheritdoc/>
        public Website GetById(string id)
        {
            var link = this.links.FirstOrDefault(x => x.Id == id);
            return link;
        }

        /// <inheritdoc/>
        public void Remove(string id)
        {
            var link = this.links.FirstOrDefault(x => x.Id == id);

            if (link != null)
            {
                this.links.Remove(link);
            }
        }

        /// <inheritdoc/>
        public void Update(Website updatedLink)
        {
            var link = this.links.FirstOrDefault(x => x.Id == updatedLink.Id);
            if (link != null)
            {
                link.Name = updatedLink.Name ?? link.Name;
                link.Url = updatedLink.Url ?? link.Url;
                link.Domain = updatedLink.Url != string.Empty ? ExtractDomainFromUrl(updatedLink.Url) : link.Domain;
                link.Description = updatedLink.Description ?? link.Description;
                link.Tags = updatedLink.Tags ?? link.Tags;
                link.ModifiedAt = DateTime.Now;
            }
        }

        private static string ExtractDomainFromUrl(string url)
        {
            var urlPattern = @"https?://(.*?)/?w*";

            Regex urlRegex = new Regex(urlPattern, RegexOptions.IgnoreCase);
            Match match = urlRegex.Match(url);

            if (!match.Success)
            {
                throw new ArgumentException("The url is invalid");
            }

            return match.Groups[1].Value;
        }
    }
}
