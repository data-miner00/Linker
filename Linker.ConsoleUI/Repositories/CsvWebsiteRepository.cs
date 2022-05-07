﻿namespace Linker.ConsoleUI.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using CsvHelper;
    using Linker.ConsoleUI.Helpers;
    using Linker.Core.CsvModels;
    using Linker.Core.Models;
    using Linker.Core.Repositories;
    using static System.Globalization.CultureInfo;

    public sealed class CsvWebsiteRepository : IWebsiteRepository
    {
        static readonly string pathToData = Path.Combine(Environment.CurrentDirectory, "data.csv");

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
                MainLanguage = (Language)csvWebsite.MainLanguage,
                IsSubdomain = csvWebsite.IsSubdomain,
                IsMultilingual = csvWebsite.IsMultilingual,
                LastVisitAt = csvWebsite.LastVisitAt,
                CreatedAt = csvWebsite.CreatedAt,
                ModifiedAt = csvWebsite.ModifiedAt,
            };
        }

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
                MainLanguage = (int)website.MainLanguage,
                IsSubdomain = website.IsSubdomain,
                IsMultilingual = website.IsMultilingual,
                LastVisitAt = website.LastVisitAt,
                CreatedAt = website.CreatedAt,
                ModifiedAt = website.ModifiedAt,
            };
        }

        private List<Website> websites;

        public CsvWebsiteRepository()
        {
            this.websites = CsvLoader.Load<CsvWebsite, Website, WebsiteClassMap>(pathToData, CsvWebsiteToWebsite);
        }

        public void Add(Website item)
        {
            var randomId = Guid.NewGuid().ToString();
            item.Id = randomId;
            this.websites.Add(item);
        }

        public IEnumerable<Website> GetAll()
        {
            return from l in websites
                   orderby l.CreatedAt
                   select l;
        }

        public Website GetById(string id)
        {
            var link = from l in websites
                       orderby l.CreatedAt
                       where l.Id == id
                       select l;

            return link.FirstOrDefault();
        }

        public void Remove(string id)
        {
            this.websites = (from l in websites
                         where l.Id != id
                         select l).ToList();
        }

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
            _link.MainLanguage = item.MainLanguage;

            _link.IsSubdomain = item.IsSubdomain;
            _link.IsMultilingual = item.IsMultilingual;

            _link.ModifiedAt = DateTime.Now;
        }

        public int Commit()
        {
            try
            {
                using var streamWriter = new StreamWriter(pathToData);
                using var csvWriter = new CsvWriter(streamWriter, InvariantCulture);

                var csvLinks = websites.ConvertAll(
                    new Converter<Website, CsvWebsite>(WebsiteToCsvWebsite));

                csvWriter.WriteRecords(csvLinks);

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                return -1;
            }
        }
    }
}