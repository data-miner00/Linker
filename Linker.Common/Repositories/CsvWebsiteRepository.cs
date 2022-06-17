﻿namespace Linker.Common.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Linker.Common.Helpers;
    using Linker.Core.CsvModels;
    using Linker.Core.Models;
    using Linker.Core.Repositories;

    /// <summary>
    /// An in-memory website repository using csv as data source.
    /// </summary>
    public sealed class CsvWebsiteRepository : IWebsiteRepository
    {

        private readonly string pathToData;

        /// <summary>
        /// The in memory list that holds all websites.
        /// </summary>
        private List<Website> websites;

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvWebsiteRepository"/> class.
        /// </summary>
        public CsvWebsiteRepository()
        {
            var filePath = ConfigurationResolver.GetConfig("WebsiteCsvPath");
            this.pathToData = Path.Combine(Environment.CurrentDirectory, filePath);

            this.websites = CsvHelper.Load<CsvWebsite, Website, WebsiteClassMap>(this.pathToData, CsvWebsiteToWebsite);
        }

        /// <summary>
        /// Converts the CsvWebsite model to Website.
        /// </summary>
        /// <param name="csvWebsite">The <see cref="CsvWebsite"/> object.</param>
        /// <returns>The converted <see cref="Website"/> object.</returns>
        public static Website CsvWebsiteToWebsite(CsvWebsite csvWebsite)
        {
            return new Website
            {
                Id = csvWebsite.Id,
                Name = csvWebsite.Name,
                Url = csvWebsite.Url,
                Category = (Category)csvWebsite.Category,
                Aesthetics = (Aesthetics)csvWebsite.Aesthetics,
                Domain = csvWebsite.Domain,
                Description = csvWebsite.Description,
                Tags = csvWebsite.Tags.Split('|'),
                Language = (Language)csvWebsite.MainLanguage,
                IsSubdomain = csvWebsite.IsSubdomain,
                IsMultilingual = csvWebsite.IsMultilingual,
                LastVisitAt = csvWebsite.LastVisitAt,
                CreatedAt = csvWebsite.CreatedAt,
                ModifiedAt = csvWebsite.ModifiedAt,
            };
        }

        /// <summary>
        /// Converts the Website model to CsvWebsite.
        /// </summary>
        /// <param name="website">The <see cref="Website"/> object.</param>
        /// <returns>The converted <see cref="CsvWebsite"/> object.</returns>
        public static CsvWebsite WebsiteToCsvWebsite(Website website)
        {
            return new CsvWebsite
            {
                Id = website.Id,
                Name = website.Name,
                Url = website.Url,
                Category = (int)website.Category,
                Aesthetics = (int)website.Aesthetics,
                Domain = website.Domain,
                Description = website.Description,
                Tags = string.Join('|', website.Tags),
                MainLanguage = (int)website.Language,
                IsSubdomain = website.IsSubdomain,
                IsMultilingual = website.IsMultilingual,
                LastVisitAt = website.LastVisitAt,
                CreatedAt = website.CreatedAt,
                ModifiedAt = website.ModifiedAt,
            };
        }

        /// <inheritdoc/>
        public void Add(Website item)
        {
            var randomId = Guid.NewGuid().ToString();
            item.Id = randomId;
            this.websites.Add(item);
        }

        /// <inheritdoc/>
        public IEnumerable<Website> GetAll()
        {
            return from l in websites
                   orderby l.CreatedAt
                   select l;
        }

        /// <inheritdoc/>
        public Website GetById(string id)
        {
            var link = from l in websites
                       orderby l.CreatedAt
                       where l.Id == id
                       select l;

            return link.FirstOrDefault();
        }

        /// <inheritdoc/>
        public void Remove(string id)
        {
            this.websites = (from l in websites
                         where l.Id != id
                         select l).ToList();
        }

        /// <inheritdoc/>
        public void Update(Website item)
        {
            var _link = this.websites.Where(l => l.Id == item.Id).FirstOrDefault();

            if (_link == null)
            {
                throw new InvalidOperationException("Cannot find the link with id");
            }

            _link.Name = item.Name ?? _link.Name;
            _link.Url = item.Url ?? _link.Url;
            _link.Domain = item.Domain ?? _link.Domain;
            _link.Description = item.Description ?? _link.Description;
            _link.Tags = item.Tags ?? _link.Tags;

            _link.Category = item.Category;
            _link.Aesthetics = item.Aesthetics;
            _link.Language = item.Language;

            _link.IsSubdomain = item.IsSubdomain;
            _link.IsMultilingual = item.IsMultilingual;

            _link.ModifiedAt = DateTime.Now;
        }

        /// <inheritdoc/>
        public int Commit()
        {
            return CsvHelper.Save(this.pathToData, WebsiteToCsvWebsite, this.websites);
        }
    }
}
