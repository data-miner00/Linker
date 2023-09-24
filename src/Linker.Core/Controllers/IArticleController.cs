namespace Linker.Core.Controllers
{
    using Linker.Core.ApiModels;
    using Linker.Core.Models;

    /// <summary>
    /// The controller abstraction for <see cref="Article"/>.
    /// </summary>
    public interface IArticleController : ILinkController<CreateArticleRequest, UpdateArticleRequest>
    {
    }
}
