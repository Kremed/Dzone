namespace Dzone.Shared.Contracts.Authentication
{
    public class LoginContract
    {
        [Required(ErrorMessage = "الرجاء كتابة البريد الألكتروني")]
        [EmailAddress(ErrorMessage = "الرجاء التحقق من بريد المستخدم.")]
        public string email { get; set; } = null!;

        [Required(ErrorMessage = "الرجاء كتابة كلمة المرور")]
        public string password { get; set; } = null!;
    }
}
