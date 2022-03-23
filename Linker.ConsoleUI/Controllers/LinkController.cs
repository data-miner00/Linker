using Linker.Core.Controllers;
using Linker.Core.Models;
using Linker.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Linker.ConsoleUI.Extensions;
using System.Linq;
using System.Reflection;

namespace Linker.ConsoleUI.Controllers
{
    public sealed class LinkController : ILinkController
    {
        private readonly ILinkRepository linkRepository;

        public LinkController(ILinkRepository linkRepository)
        {
            this.linkRepository = linkRepository;
        }

        public void DisplayAllLinks()
        {
            Console.Clear();
            Console.WriteLine("List of collected links.");

            RenderLinks(this.linkRepository.GetAll());

            static void RenderLinks(IEnumerable<Link> links)
            {
                const string displayTemplate = "|| {0} || {1} || {2} || {3} || {4} || {5} ||";
                const int indexPad = 6;
                const int idPad = 38;
                const int namePad = 20;
                const int urlPad = 30;
                const int tagsPad = 20;
                const int modifiedPad = 23;

                Console.WriteLine("".PadRight(163, '='));
                Console.WriteLine(
                    displayTemplate,
                    "Index".PadRight(indexPad),
                    "Id".PadRight(idPad),
                    "Name".PadRight(namePad),
                    "Url".PadRight(urlPad),
                    "Tags".PadRight(tagsPad),
                    "Modified Date".PadRight(modifiedPad));
                Console.WriteLine("".PadRight(163, '='));

                foreach (var (link, index) in links.WithIndex())
                {
                    Console.WriteLine(
                        displayTemplate,
                        (index + 1).ToString().PadRight(indexPad),
                        link.Id.PadRight(idPad),
                        link.Name.TruncateWithEllipsis(namePad - 3).PadRight(namePad),
                        link.Url.TruncateWithEllipsis(urlPad - 3).PadRight(urlPad),
                        string.Join(", ", link.Tags).TruncateWithEllipsis(tagsPad - 3).PadRight(tagsPad),
                        link.ModifiedAt.ToString().PadRight(modifiedPad));
                }

                Console.WriteLine("".PadRight(163, '='));
            }
        }

        public void DisplaySingleLink()
        {
            Console.Clear();
            var id = PromptForInput("Enter the ID of the link: ", "");

            var link = this.linkRepository.GetById(id);

            const int dividerLength = 100;
            const int labelPad = 15;
            const int contentPad = 83;
            
            Console.WriteLine("|{0}|", "".PadRight(dividerLength, '='));
            Console.WriteLine("|{0}|", "".PadRight(dividerLength, ' '));
            Console.WriteLine("|{0}|", link.Name.PadSides(dividerLength));
            Console.WriteLine("|{0}|", "".PadRight(dividerLength, ' '));
            Console.WriteLine("|{0}|", "".PadRight(dividerLength, '='));
            Console.WriteLine("|{0}: {1}|", "Id".PadRight(labelPad), link.Id.PadRight(contentPad));
            Console.WriteLine("|{0}|", "".PadRight(dividerLength, '-'));
            Console.WriteLine("|{0}: {1}|", "Url".PadRight(labelPad), link.Url.PadRight(contentPad));
            Console.WriteLine("|{0}|", "".PadRight(dividerLength, '-'));
            Console.WriteLine("|{0}: {1}|", "Language".PadRight(labelPad), link.MainLanguage.ToString().PadRight(contentPad));
            Console.WriteLine("|{0}|", "".PadRight(dividerLength, '-'));
            Console.WriteLine("|{0}: {1}|", "Description".PadRight(labelPad), link.Description.PadRight(contentPad));
            Console.WriteLine("|{0}|", "".PadRight(dividerLength, '-'));
            Console.WriteLine("|{0}: {1}|", "Tags".PadRight(labelPad), string.Join(", ", link.Tags).PadRight(contentPad));
            Console.WriteLine("|{0}|", "".PadRight(dividerLength, '-'));
            Console.WriteLine("|{0}: {1}| {2}: {3}|", "Created".PadRight(labelPad), link.CreatedAt.ToString().PadRight(30), "Modified".PadRight(labelPad), link.ModifiedAt.ToString().PadRight(34));
            Console.WriteLine("|{0}|", "".PadRight(dividerLength, '='));
        }


        public void UpdateLink()
        {
            Console.Clear();

            this.DisplayAllLinks();

            var id = PromptForInput("Please select the ID of the link to be updated: ", "");

            Console.WriteLine("\nInsert the details that needed to be changed. Hit Enter to skip.");

            const string labelTemplate = "{0}: ";
            const int labelPad = 13;

            var name = PromptForInput(labelTemplate, "Name".PadRight(labelPad)).ReturnNullIfEmpty();
            var url = PromptForInput(labelTemplate, "Url".PadRight(labelPad)).ReturnNullIfEmpty();
            var description = PromptForInput(labelTemplate, "Description".PadRight(labelPad)).ReturnNullIfEmpty();

            var _tags = PromptForInput(labelTemplate, "Tags".PadRight(labelPad));
            var tags = string.IsNullOrEmpty(_tags) ? null : _tags.Split(",").Select(tag => tag.Trim());

            var link = new Link()
            {
                Id = id,
                Name = name,
                Url = url,
                Description = description,
                Tags = tags,
            };

            try
            {
                this.linkRepository.Update(link);
                Console.WriteLine("Successfully updated link!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed update link! Error: {0}", ex.Message);
            }
        }

        public void InsertLink()
        {
            Console.Clear();

            const string labelTemplate = "{0}: ";
            const int labelPad = 13;

            var name = PromptForInput(labelTemplate, "Name".PadRight(labelPad));
            var url = PromptForInput(labelTemplate, "Url".PadRight(labelPad));
            var description = PromptForInput(labelTemplate, "Description".PadRight(labelPad));

            var _tags = PromptForInput(labelTemplate, "Tags".PadRight(labelPad));
            var tags = _tags.Split(",").Select(tag => tag.Trim());

            var now = DateTime.Now;

            var newLink = new Link()
            {
                Name = name,
                Url = url,
                Description = description,
                Tags = tags,
                CreatedAt = now,
                ModifiedAt = now,
            };

            this.linkRepository.Add(newLink);

            Console.WriteLine("Successfully added new link!");
        }

        public void RemoveLink()
        {
            string MockGetGuid() => Guid.NewGuid().ToString();

            Console.WriteLine("======================================");
            Console.WriteLine("================Remove================");

            var _id = PromptForInput("Select an ID to remove: ");
            int id;

            int.TryParse(_id, out id);

            this.linkRepository.Remove(MockGetGuid());

            Console.WriteLine("Successfully removed link");
        }

        private static string PromptForInput(params object[] prompt)
        {
            var consoleWrite = typeof(Console)
                .GetMethods()
                .Where(x => x.Name == "Write" && x.IsStatic)
                .FirstOrDefault();

            consoleWrite.Invoke(null, prompt);

            var input = Console.ReadLine();
            return input;
        }

        public void Save()
        {
            this.linkRepository.Commit();
        }
    }
}
