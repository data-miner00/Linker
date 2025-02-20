namespace Linker.Cli.Handlers;

using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Integrations;
using Linker.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal sealed class UpdateListCommandHandler : ICommandHandler
{
    private readonly IRepository<UrlList> repository;

    public UpdateListCommandHandler(IRepository<UrlList> repository)
    {
        this.repository = Guard.ThrowIfNull(repository);
    }

    public async Task HandleAsync(object commandArguments)
    {
        if (commandArguments is UpdateListCommandArguments args)
        {
            var originalList = await this.repository.GetByIdAsync(args.Id);

            if (args.Name is not null)
            {
                originalList.Name = args.Name;
            }

            if (args.Description is not null)
            {
                originalList.Description = args.Description;
            }

            await this.repository.UpdateAsync(originalList);

            return;
        }

        throw new NotImplementedException();
    }
}
