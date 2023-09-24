namespace Linker.Core.Controllers
{
    using Linker.Core.ApiModels;
    using Linker.Core.Models;

    /// <summary>
    /// The controller abstraction for <see cref="Youtube"/>.
    /// </summary>
    public interface IYoutubeController : ILinkController<CreateYoutubeRequest, UpdateYoutubeRequest>
    {
    }
}
