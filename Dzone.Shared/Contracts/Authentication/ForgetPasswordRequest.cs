using System.ComponentModel.DataAnnotations;

namespace Dzone.Shared.Contracts.Authentication
{
    public class ForgetPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }
    }
}
