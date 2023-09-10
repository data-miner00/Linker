namespace Linker.Core.Controllers
{
    using Linker.Core.ApiModels;
    using Linker.Core.Models;

    /// <summary>
    /// The controller abstraction for <see cref="Website"/>.
    /// </summary>
    public interface IWebsiteController : ILinkController<CreateWebsiteRequest, UpdateWebsiteRequest>
    {
    }
}
