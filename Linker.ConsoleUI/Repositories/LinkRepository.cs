using System;
using System.Collections.Generic;
using Linker.Core.Models;
using Linker.Core.Repositories;
using System.Linq;
using System.Text.RegularExpressions;

namespace Linker.ConsoleUI.Repositories
{
    public class LinkRepository : ILinkRepository
    {
        private IList<Link> links;

        public LinkRepository()
        {
            links = new List<Link>
            {
                new Link
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Google",
                    Url = "https://www.google.com",
                    Domain = "www.google.com",
                    Description = "The world's famous search engine",
                    Tags = new List<string> { "search", "famous" },
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                },
                new Link
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Google",
                    Url = "https://www.google.com",
                    Domain = "www.google.com",
                    Description = "The world's famous search engine",
                    Tags = new List<string> { "search", "famous" },
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                },
                new Link
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Google",
                    Url = "https://www.google.com",
                    Domain = "www.google.com",
                    Description = "The world's famous search engine",
                    Tags = new List<string> { "search", "famous" },
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                },
            };
        }

        public void Add(Link link)
        {
            link.Id = Guid.NewGuid().ToString();
            link.Domain = ExtractDomainFromUrl(link.Url);
            link.CreatedAt = DateTime.Now;
            link.ModifiedAt = DateTime.Now;

            links.Add(link);
        }

        public IEnumerable<Link> GetAll()
        {
            return from l in links
                   orderby l.CreatedAt
                   select l;
        }

        public Link GetById(string id)
        {

            var link = links.FirstOrDefault(x => x.Id == id);
            return link;
        }

        public void Remove(string id)
        {
            var link = links.FirstOrDefault(x => x.Id == id);

            if (link != null)
            {
                links.Remove(link);
            }
        }

        public void Update(Link updatedLink)
        {
            var link = links.FirstOrDefault(x => x.Id == updatedLink.Id);
            
            if (link != null)
            {
                link.Name = updatedLink.Name ?? link.Name;
                link.Url = updatedLink.Url ?? link.Url;
                link.Domain = updatedLink.Url != "" ? ExtractDomainFromUrl(updatedLink.Url) : link.Domain;
                link.Description = updatedLink.Description ?? link.Description;
                link.Tags = updatedLink.Tags ?? link.Tags;
                link.ModifiedAt = DateTime.Now;
            }
        }

        public int Commit()
        {
            return 0;
        }

        private bool IsExists(string id)
        {
            return GetById(id) != null;
        }

        private string ExtractDomainFromUrl(string url)
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
