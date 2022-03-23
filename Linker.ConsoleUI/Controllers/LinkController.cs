using Linker.Core.Controllers;
using Linker.Core.Models;
using Linker.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Linker.ConsoleUI.Extensions;

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
            Console.WriteLine("List of collected links.");

            RenderLinks(this.linkRepository.GetAll());

            static void RenderLinks(IEnumerable<Link> links)
            {
                var displayTemplate = "|| {0} || {1} || {2} || {3} || {4} || {5} ||";
                var indexPad = 6;
                var idPad = 38;
                var namePad = 20;
                var urlPad = 30;
                var tagsPad = 20;
                var modifiedPad = 23;

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
            var id = PromptForInput("Enter the ID of the link: ");

            var link = this.linkRepository.GetById(id);

            var dividerLength = 100;
            var labelPad = 15;
            var contentPad = 83;
            
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
            string MockGetGuid() => Guid.NewGuid().ToString();

            Console.WriteLine("======================================");
            Console.WriteLine("==============Update==================");

            this.DisplayAllLinks();

            Console.WriteLine("Please select the ID of the link to be updated: ");
            int index;
            int.TryParse(Console.ReadLine(), out index);

            Console.WriteLine("Insert the details that needed to be changed. Hit Enter to skip.");
            var name = PromptForInput("Name: ");
            var url = PromptForInput("Url: ");
            var description = PromptForInput("Description: ");
            var _tags = PromptForInput("Tags (comma seperated): ");
            var tags = _tags.Split(",");

            var link = new Link()
            {
                Id = MockGetGuid(),
                Name = name,
                Url = url,
                Description = description,
                Tags = tags,
            };

            this.linkRepository.Update(link);

            Console.WriteLine("Successfully updated link!");
        }

        public void InsertLink()
        {
            Console.WriteLine("======================================");
            Console.WriteLine("===============Insert=================");

            var name = PromptForInput("Name: ");
            var url = PromptForInput("Url: ");
            var description = PromptForInput("Description: ");
            var _tags = PromptForInput("Tags (pipe seperated): ");
            var tags = _tags.Split(",");

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

        private string PromptForInput(string prompt)
        {

            Console.Write(prompt);
            var input = Console.ReadLine();

            return input;
        }

        public void Save()
        {
            this.linkRepository.Commit();
        }
    }
}
