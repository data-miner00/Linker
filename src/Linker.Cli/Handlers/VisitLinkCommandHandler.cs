namespace Linker.Cli.Handlers;

using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Integrations;
using Linker.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal sealed class VisitLinkCommandHandler : ICommandHandler
{
    private readonly IRepository<Link> linkRepository;
    private readonly IRepository<Visit> visitRepository;

    public VisitLinkCommandHandler(IRepository<Link> linkRepository, IRepository<Visit> visitRepository)
    {
        this.linkRepository = Guard.ThrowIfNull(linkRepository);
        this.visitRepository = Guard.ThrowIfNull(visitRepository);
    }

    public async Task HandleAsync(object commandArguments)
    {
        if (commandArguments is VisitLinkCommandArguments vlca)
        {
            Link? linkToVisit;

            if (vlca.LinkId is not null)
            {
                linkToVisit = await this.linkRepository.GetByIdAsync(vlca.LinkId.Value);
            }
            else if (vlca.Random)
            {
                var links = (await this.linkRepository.GetAllAsync()).ToArray();

                var random = new Random();

                var randomIndex = random.Next(links.Length);

                linkToVisit = links[randomIndex];
            }
            else
            {
                throw new InvalidOperationException("Bad condition");
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = linkToVisit.Url,
                UseShellExecute = true,
            };

            Process.Start(startInfo);

            var visit = new Visit
            {
                LinkId = linkToVisit.Id,
                CreatedAt = DateTime.Now,
            };

            await this.visitRepository.AddAsync(visit);
            return;
        }

        throw new ArgumentException("Wrong args");
    }
}
