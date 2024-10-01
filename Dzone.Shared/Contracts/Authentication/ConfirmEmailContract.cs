using System.ComponentModel.DataAnnotations;

namespace Dzone.Shared.Contracts.Authentication
{
    public class ConfirmEmailContract
    {
        [Required(ErrorMessage = "Required OTP code")]
        
        public int code { get; set; }


        [Required(ErrorMessage = "Required Email")]
        [EmailAddress(ErrorMessage = "not valid Email Adress")]
        public string email { get; set; }
    }
}
