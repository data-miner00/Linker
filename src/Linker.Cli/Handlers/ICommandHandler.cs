namespace Linker.Cli.Handlers;

using System.Threading.Tasks;

public interface ICommandHandler
{
    Task HandleAsync(object commandArguments);
}
