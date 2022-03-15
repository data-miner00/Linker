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
            Console.WriteLine("======================================");
            Console.WriteLine("=============Display All==============");

            Console.WriteLine("List of collected links.");
            RenderLinks(this.linkRepository.GetAll());

            void RenderLinks(IEnumerable<Link> links)
            {
                Console.WriteLine("Index.\tName\t\tUrl\t\tDescription\t\t\tTags\t\t\tModified Date");
                foreach (var (link, index) in links.WithIndex())
                {
                    Console.WriteLine("{0}.\t{1}\t\t{2}\t\t{3}\t\t\t{4}\t\t\t{5}", index, link.Name, link.Url, link.Description, string.Join(", ", link.Tags), link.ModifiedAt);
                }
            }
        }

        public void DisplaySingleLink()
        {
            string MockGetGuid() => Guid.NewGuid().ToString();
            Console.WriteLine("======================================");
            Console.WriteLine("============Display One===============");

            var id = PromptForInput("Enter the ID of the link: ");

            var link = this.linkRepository.GetById(MockGetGuid());

            Console.WriteLine("====================");
            Console.WriteLine("{0} - {1}", link.Name, link.Url);
            Console.WriteLine("====================");
            Console.WriteLine(link.Description);
            Console.WriteLine("--------------------");
            Console.WriteLine("Tags: ", string.Join(", ", link.Tags));
            Console.WriteLine("\n\n----- End of display -----");
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

            var newLink = new Link()
            {
                Name = name,
                Url = url,
                Description = description,
                Tags = tags,
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
