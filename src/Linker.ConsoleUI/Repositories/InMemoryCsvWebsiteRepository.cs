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
