using System.ComponentModel.DataAnnotations;

namespace Dzone.Mobile.Views.AuthViews;

public partial class RegisterView : ContentPage
{
    public IAuthService authService;

    public RegisterView(IAuthService authService)
    {
        InitializeComponent();
        this.authService = authService;
    }

    private async void BtnRegister_Clicked(object sender, EventArgs e)
    {
        try
        {
            //if (string.IsNullOrEmpty(TxtEmail.Text) ||
            //    string.IsNullOrEmpty(TxtPassword.Text) ||
            //    string.IsNullOrEmpty(TxtUserName.Text) ||
            //    string.IsNullOrEmpty(TxtPhone.Text))
            //{
            //    await Toast.Make("الرجاء تعبئة وملاء جميع الحقول.").Show();
            //    return;
            //}

            RegisterContract contract = new()
            {
                email = TxtEmail.Text,
                name = TxtUserName.Text,
                phoneNumber = TxtPhone.Text,
                password = TxtPassword.Text,
                UserType = "AppUser",
            };

            // إنشاء قائمة لتخزين نتائج التحقق من صحة البيانات.
            // تحتوي كل نتيجة على معلومات حول الأخطاء التي قد تحدث أثناء التحقق.
            var validationResults = new List<ValidationResult>();

            // إنشاء سياق للتحقق من صحة الكائن (contract).
            // يتم استخدام هذا السياق لتحديد الكائن الذي يتم التحقق من صحته وإعداداته.
            var context = new ValidationContext(contract);

            // محاولة التحقق من صحة كائن العقد باستخدام السياق وقائمة نتائج التحقق.
            // المعامل الأخير (true) يُمكّن التحقق من خصائص الكائن الفرعية أيضًا.
            bool isValid = Validator.TryValidateObject(contract, context, validationResults, true);

            if (!isValid)
            {
                string errorMessage = validationResults.FirstOrDefault()?.ErrorMessage!;

                await Toast.Make(errorMessage).Show();

                return;
            }

            var result = await authService.CreateUserAsync(contract);

            if (result.IsError)
            {
                await Toast.Make(result.FirstError.Description).Show();
                return;
            }
            else
            {
                var parameters = new Dictionary<string, object>
                {
                    { "username", contract.name },
                    { "email", contract.email },
                    { "otpType", "ConfirmEmail" },
                    { "title", $"لقد تم أرسال رمز تحقق لمرة واحدة الي بريدكـ الألكتروني : {contract.email}, الرجاء التحقق من بريدك الالكتروني وتـأكيد رمز التحقق OTP في الحقل المخصص لتفعيل حسابكـ" }
                };

                await Shell.Current.GoToAsync($"../{nameof(ConfirmOtpView)}", parameters);
                //await Shell.Current.GoToAsync($"{nameof(ConfirmOtpView)}?username={contract.name}");
            }
        }
        catch (Exception ex)
        {
            await Toast.Make(ex.Message).Show();
        }
    }

    private async void BtnResetPassword_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"../{nameof(ForggetPasswordView)}");
    }

    private async void BtnLogin_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.Navigation.PopModalAsync();
        //await Shell.Current.GoToAsync($"{nameof(LoginView)}");
    }
}