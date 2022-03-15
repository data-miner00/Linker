﻿using Linker.Core.Models;
using Linker.Core.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using static System.Globalization.CultureInfo;
using System.Linq;
using System.Text;
using Linker.Core.CsvModels;

namespace Linker.ConsoleUI.Repositories
{
    public sealed class CsvLinkRepository : ILinkRepository
    {
        static readonly string pathToData = Path.Combine(Environment.CurrentDirectory ,"data.csv");

        public static Link CsvLinkToLink(CsvLink csvLink)
        {
            return new Link
            {
                Id = csvLink.Id,
                Name = csvLink.Name,
                Url = csvLink.Url,
                Category = (Category)csvLink.Category,
                Domain = csvLink.Domain,
                Description = csvLink.Description,
                Tags = csvLink.Tags.Split('|'),
                MainLanguage = (Language)csvLink.MainLanguage,
                IsSubdomain = csvLink.IsSubdomain,
                IsMultilingual = csvLink.IsMultilingual,
                LastVisitAt = csvLink.LastVisitAt,
                CreatedAt = csvLink.CreatedAt,
                ModifiedAt = csvLink.ModifiedAt,
            };
        }

        public static CsvLink LinkToCsvLink(Link link)
        {
            return new CsvLink
            {
                Id = link.Id,
                Name = link.Name,
                Url = link.Url,
                Category = (int)link.Category,
                Domain = link.Domain,
                Description = link.Description,
                Tags = string.Join('|', link.Tags),
                MainLanguage = (int)link.MainLanguage,
                IsSubdomain = link.IsSubdomain,
                IsMultilingual = link.IsMultilingual,
                LastVisitAt = link.LastVisitAt,
                CreatedAt = link.CreatedAt,
                ModifiedAt = link.ModifiedAt,
            };
        }

        private List<Link> links;
        private bool fileExist = true;

        public CsvLinkRepository()
        {
            if (!File.Exists(pathToData))
            {
                fileExist = false;
                links = new List<Link>();
                Console.WriteLine("No data found");
            } else
            {
                using (var streamReader = new StreamReader(pathToData))
                {
                    using (var csvReader = new CsvReader(streamReader, InvariantCulture))
                    {
                        csvReader.Context.RegisterClassMap<LinkClassMap>();
                        this.links = csvReader.GetRecords<CsvLink>().ToList().ConvertAll(
                            new Converter<CsvLink, Link>(CsvLinkToLink));
                    }
                }
            }
        }

        public void Add(Link link)
        {
            var randomId = Guid.NewGuid().ToString();
            link.Id = randomId;
            this.links.Add(link);
        }

        public IEnumerable<Link> GetAll()
        {
            return from l in links
                   orderby l.CreatedAt
                   select l;
        }

        public Link GetById(string id)
        {
            var link = from l in links
                       orderby l.CreatedAt
                       where l.Id == id
                       select l;

            return link.FirstOrDefault();
        }

        public void Remove(string id)
        {
            this.links = (List<Link>)(from l in links
                         where l.Id != id
                         select l);
        }

        public void Update(Link link)
        {
            var _link = this.links.Where(l => l.Id == link.Id).FirstOrDefault();
            
            _link.Name = link.Name;
            _link.Url = link.Url;
            _link.Category = link.Category;
            _link.Domain = link.Domain;
            _link.IsSubdomain = link.IsSubdomain;
            _link.Description = link.Description;
            _link.Tags = link.Tags;
            _link.ModifiedAt = DateTime.Now;
            _link.MainLanguage = link.MainLanguage;
            _link.IsMultilingual = link.IsMultilingual;
        }

        public int Commit()
        {
            using var streamWriter = new StreamWriter(pathToData);
            using var csvWriter = new CsvWriter(streamWriter, InvariantCulture);

            var csvLinks = links.ConvertAll(
                new Converter<Link, CsvLink>(LinkToCsvLink));

            csvWriter.WriteRecords(csvLinks);

            return 0;
        }
    }
}
