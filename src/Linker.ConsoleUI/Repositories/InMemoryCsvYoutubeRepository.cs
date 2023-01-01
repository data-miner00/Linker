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
    /// An in-memory Youtube repository using csv as data source.
    /// </summary>
    public class InMemoryCsvYoutubeRepository : ICsvYoutubeRepository
    {
        private readonly string pathToData;

        private List<Youtube> channels;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryCsvYoutubeRepository"/> class.
        /// </summary>
        public InMemoryCsvYoutubeRepository()
        {
            var filePath = ConfigurationResolver.GetConfig("YoutubeCsvPath");
            this.pathToData = Path.Combine(Environment.CurrentDirectory, filePath);

            this.channels = CsvHelper.Load<CsvYoutube, Youtube, YoutubeClassMap>(this.pathToData, this.CsvModelToModel);
        }

        /// <inheritdoc/>
        public void Add(Youtube item)
        {
            var randomId = Guid.NewGuid().ToString();
            item.Id = randomId;
            this.channels.Add(item);
        }

        /// <inheritdoc/>
        public int Commit()
        {
            return CsvHelper.Save(this.pathToData, this.ModelToCsvModel, this.channels);
        }

        /// <inheritdoc/>
        public Youtube CsvModelToModel(CsvYoutube csvModel)
        {
            return new Youtube
            {
                Id = csvModel.Id,
                Url = csvModel.Url,
                Category = (Category)csvModel.Category,
                Description = csvModel.Description,
                Tags = csvModel.Tags.Split('|'),
                Language = (Language)csvModel.Language,
                LastVisitAt = csvModel.LastVisitAt,
                CreatedAt = csvModel.CreatedAt,
                ModifiedAt = csvModel.ModifiedAt,
                Name = csvModel.Name,
                Youtuber = csvModel.Youtuber,
                Country = csvModel.Country,
            };
        }

        /// <inheritdoc/>
        public IEnumerable<Youtube> GetAll()
        {
            return from channel in this.channels
                   orderby channel.CreatedAt
                   select channel;
        }

        /// <inheritdoc/>
        public Youtube GetById(string id)
        {
            var link = from channel in this.channels
                       where channel.Id == id
                       select channel;

            return link.FirstOrDefault();
        }

        /// <inheritdoc/>
        public CsvYoutube ModelToCsvModel(Youtube model)
        {
            return new CsvYoutube
            {
                Id = model.Id,
                Url = model.Url,
                Category = (int)model.Category,
                Description = model.Description,
                Tags = string.Join('|', model.Tags),
                Language = (int)model.Language,
                LastVisitAt = model.LastVisitAt,
                CreatedAt = model.CreatedAt,
                ModifiedAt = model.ModifiedAt,
                Name = model.Name,
                Youtuber = model.Youtuber,
                Country = model.Country,
            };
        }

        /// <inheritdoc/>
        public void Remove(string id)
        {
            this.channels = (from channel in this.channels
                             where channel.Id != id
                             select channel).ToList();
        }

        /// <inheritdoc/>
        public void Update(Youtube item)
        {
            var link = this.channels.FirstOrDefault(channel => channel.Id == item.Id);

            if (link == null)
            {
                throw new InvalidOperationException("Cannot find the link with id");
            }

            link.Url = item.Url ?? link.Url;
            link.Description = item.Description ?? link.Description;
            link.Category = item.Category;
            link.Tags = item.Tags ?? link.Tags;
            link.Language = item.Language;
            link.Youtuber = item.Youtuber ?? link.Youtuber;
            link.Country = item.Country ?? link.Country;
            link.Name = item.Name ?? link.Name;

            link.ModifiedAt = DateTime.Now;
        }
    }
}
