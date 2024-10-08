namespace Dzone.Shared.Contracts.Authentication;

public class CreateLocationContract
{
    [Required(ErrorMessage = "الرجاء كتابة اسم العنوان, حقل الاسم مطلوب")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "الرجاء كتابة وصف العنوان او اقرب نقطة دالة.")]
    public string Discrption { get; set; } = null!;

    [Required(ErrorMessage = "لايمكن انشاء موقع جديد بدون خط الطول : Latitude")]
    public string Latitude { get; set; } = null!;

    [Required(ErrorMessage = "لايمكن انشاء موقع جديد بدون خط العرض: Longitude")]
    public string Longitude { get; set; } = null!;
}
