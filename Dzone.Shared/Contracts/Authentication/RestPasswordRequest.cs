namespace Dzone.Shared.Contracts.Authentication
{
    public class RestPasswordRequest
    {
        [Required(ErrorMessage = "البريد الألكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الالكتروني الذي تم أرساله غير مطابق, الرجاء أرسال بريد الكتروني صحيح.")]
        public string email { get; set; }

        [Required(ErrorMessage = "رمز التحقق مطلوب OTP")]
        [Range(10000, 99999, ErrorMessage = "رمز التاكيد يجب ان يكون رقم من 5 خانات.")]
        public int otpCode { get; set; }

        [Required(ErrorMessage = "كلمة المرور الجديدة مطلوبة")]
        public string newPassword { get; set; }
    }
}
