namespace Linker.ConsoleUI.Repositories
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
    public sealed class InMemoryCsvWebsiteRepository : ICsvWebsiteRepository
    {
        private readonly string pathToData;

        /// <summary>
        /// The in memory list that holds all websites.
        /// </summary>
        private List<Website> websites;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryCsvWebsiteRepository"/> class.
        /// </summary>
        public InMemoryCsvWebsiteRepository()
        {
            var filePath = ConfigurationResolver.GetConfig("WebsiteCsvPath");
            this.pathToData = Path.Combine(Environment.CurrentDirectory, filePath);

            this.websites = CsvHelper.Load<CsvWebsite, Website, WebsiteClassMap>(this.pathToData, this.CsvModelToModel);
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
            return from l in this.websites
                   orderby l.CreatedAt
                   select l;
        }

        /// <inheritdoc/>
        public Website GetById(string id)
        {
            var link = from l in this.websites
                       orderby l.CreatedAt
                       where l.Id == id
                       select l;

            return link.FirstOrDefault();
        }

        /// <inheritdoc/>
        public void Remove(string id)
        {
            this.websites = (from l in this.websites
                         where l.Id != id
                         select l).ToList();
        }

        /// <inheritdoc/>
        public void Update(Website item)
        {
            var link = this.websites.Find(l => l.Id == item.Id)
                ?? throw new InvalidOperationException("Cannot find the link with id");

            link.Name = item.Name ?? link.Name;
            link.Url = item.Url ?? link.Url;
            link.Domain = item.Domain ?? link.Domain;
            link.Description = item.Description ?? link.Description;
            link.Tags = item.Tags ?? link.Tags;

            link.Category = item.Category;
            link.Aesthetics = item.Aesthetics;
            link.Language = item.Language;

            link.IsSubdomain = item.IsSubdomain;
            link.IsMultilingual = item.IsMultilingual;

            link.ModifiedAt = DateTime.Now;
        }

        /// <inheritdoc/>
        public int Commit()
        {
            return CsvHelper.Save(this.pathToData, this.ModelToCsvModel, this.websites);
        }

        /// <inheritdoc/>
        public Website CsvModelToModel(CsvWebsite csvModel)
        {
            return new Website
            {
                Id = csvModel.Id,
                Name = csvModel.Name,
                Url = csvModel.Url,
                Category = (Category)csvModel.Category,
                Aesthetics = (Aesthetics)csvModel.Aesthetics,
                Domain = csvModel.Domain,
                Description = csvModel.Description,
                Tags = csvModel.Tags.Split('|'),
                Language = (Language)csvModel.MainLanguage,
                IsSubdomain = csvModel.IsSubdomain,
                IsMultilingual = csvModel.IsMultilingual,
                LastVisitAt = csvModel.LastVisitAt,
                CreatedAt = csvModel.CreatedAt,
                ModifiedAt = csvModel.ModifiedAt,
            };
        }

        /// <inheritdoc/>
        public CsvWebsite ModelToCsvModel(Website model)
        {
            return new CsvWebsite
            {
                Id = model.Id,
                Name = model.Name,
                Url = model.Url,
                Category = (int)model.Category,
                Aesthetics = (int)model.Aesthetics,
                Domain = model.Domain,
                Description = model.Description,
                Tags = string.Join('|', model.Tags),
                MainLanguage = (int)model.Language,
                IsSubdomain = model.IsSubdomain,
                IsMultilingual = model.IsMultilingual,
                LastVisitAt = model.LastVisitAt,
                CreatedAt = model.CreatedAt,
                ModifiedAt = model.ModifiedAt,
            };
        }
    }
}
