#nullable enable
namespace Linker.Core.ApiModels
{
    using Linker.Core.Models;

    public sealed class Data<T>
        where T : Link
    {
        public T Item { get; set; }
    }

    public sealed class WebApiResponse<T>
        where T : Link
    {
        public string Status { get; set; }

        public string? Error { get; set; }

        public Data<T>? Data { get; set; }
    }
}
