using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;

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
            if (string.IsNullOrEmpty(TxtEmail.Text) ||
                string.IsNullOrEmpty(TxtPassword.Text) ||
                string.IsNullOrEmpty(TxtUserName.Text) ||
                string.IsNullOrEmpty(TxtPhone.Text))
            {
                await Toast.Make("الرجاء تعبئة وملاء جميع الحقول.").Show();
                return;
            }

            RegisterContract contract = new()
            {
                email = TxtEmail.Text,
                name = TxtUserName.Text,
                phoneNumber = TxtPhone.Text,
                password = TxtPassword.Text,
                UserType = "AppUser",
            };

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