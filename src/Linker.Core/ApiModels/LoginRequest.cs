namespace Linker.Core.ApiModels
{
    using System.ComponentModel.DataAnnotations;

    public sealed class LoginRequest
    {
        [Required(AllowEmptyStrings = false)]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }
    }
}
