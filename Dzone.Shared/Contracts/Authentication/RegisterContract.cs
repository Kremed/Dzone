using System.ComponentModel.DataAnnotations;

namespace Dzone.Shared.Contracts.Authentication;

public class RegisterContract
{
    [Required(ErrorMessage = "الرجاء كتابة أسم المستخدم")]
    [StringLength(100, ErrorMessage = "الحد الاقصي لعدد حروف الاسم 100 حرف.")]
    public string name { get; set; } = null!;

    [Required(ErrorMessage = "الرجاء كتابة بريد المستخدم")]
    [EmailAddress(ErrorMessage = "الرجاء التحقق من بريد المستخدم.")]
    public string email { get; set; } = null!;

    [Required(ErrorMessage = "الرجاء كتابة كلمة المرور")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "يجب ان تتكون كلمة المرور من 6 خانات علي الاقل")]
    [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessage = "كلمة المرور يجب ان تحتوي علي حروف كبيرة وصغيرة وارقام")]
    public string password { get; set; } = null!;

    [Required(ErrorMessage = "الرحاء كتابة رقم الهاتف")]
    [Phone(ErrorMessage = "رقم الهاتف غير صالح, الرجاء كتابة رقم هاتف بالصورة الصحيحة.")]
    [RegularExpression(@"^\+218(91|92|93|94|95)\d{7}$", ErrorMessage = "رقم هاتف غير صالح. يجب ان يكون الرقم مثل: +21891XXXXXXX, +21892XXXXXXX, +21893XXXXXXX, +21894XXXXXXX, or +21895XXXXXXX.")]
    public string phoneNumber { get; set; } = null!;

    public string UserType { get; set; } = null!;
}
