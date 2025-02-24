namespace Linker.Cli.Handlers;

using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Integrations;
using Linker.Common.Helpers;
using System;
using System.Threading.Tasks;

internal sealed class GetListCommandHandler : ICommandHandler
{
    private readonly IRepository<UrlList> repository;

    public GetListCommandHandler(IRepository<UrlList> repository)
    {
        this.repository = Guard.ThrowIfNull(repository);
    }

    public async Task HandleAsync(object commandArguments)
    {
        if (commandArguments is GetListCommandArguments args)
        {
            var list = await this.repository.GetByIdAsync(args.Id);

            if (list == null)
            {
                Console.WriteLine($"The link with ID {args.Id} cannot be found.");
                return;
            }

            bool[] flags = [
                args.Links,
                args.Name,
                args.Description,
            ];

            var isFlagsProvided = Array.Exists(flags, x => x);

            if (!isFlagsProvided || args.Name)
            {
                Console.WriteLine("Name: " + list.Name);
            }

            if (!isFlagsProvided || args.Description)
            {
                Console.WriteLine("Description: " + list.Description);
            }

            if (!isFlagsProvided || args.Links)
            {
                Console.WriteLine("Links:");

                foreach (var link in list.Links)
                {
                    Console.WriteLine($"{link.Id}. {link.Url} - {link.Name}");
                }
            }

            return;
        }

        throw new ArgumentException("Invalid args");
    }
}
