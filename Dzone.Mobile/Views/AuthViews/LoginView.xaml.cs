using Microsoft.Maui.Controls.Internals;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace Dzone.Mobile.Views.AuthViews;

public partial class LoginView : ContentPage
{
    public IAuthService authService;
    public ILocalTokenService userService;
    public LoginView(IAuthService authService, ILocalTokenService userService)
    {
        InitializeComponent();
        this.authService = authService;
        this.userService = userService;
    }

    private async void BtnLogin_Clicked(object sender, EventArgs e)
    {
        try
        {
            LoginContract contract = new()
            {
                email = TxtEmail.Text,
                password = TxtPassword.Text,
            };

            var validationResults = new List<ValidationResult>();

            var context = new ValidationContext(contract);

            bool isValid = Validator.TryValidateObject(contract, context, validationResults, true);

            if (!isValid)
            {
                string errorMessage = validationResults.FirstOrDefault()?.ErrorMessage!;

                await Toast.Make(errorMessage).Show();

                return;
            }

            var loginResult = await authService.LoginAsync(contract);
            //General.Unauthorized
            if (loginResult.IsError && loginResult.FirstError.Type == ErrorType.Unauthorized)
            {
                var parameters = new Dictionary<string, object>
                {
                    { "email", contract.email },
                    { "otpType", "ConfirmEmail" },
                    { "title", $"حسابك غير مفعل لقد قامت خوادمنا بأرسال رمز تحقق لمرة واحدة الي بريدكـ الألكتروني : {contract.email}, الرجاء التحقق من بريدك الالكتروني وتـأكيد رمز التحقق OTP في الحقل المخصص لتفعيل حسابكـ" }
                };

                await Shell.Current.GoToAsync($"../{nameof(ConfirmOtpView)}", parameters);

                await Toast.Make(loginResult.FirstError.Description).Show();

                return;
            }
            else if (loginResult.IsError)
            {
                await Toast.Make(loginResult.FirstError.Description).Show();
                return;
            }


            var saveTokenResult = await userService.UpsertJwtTokenAsync(loginResult.Value);

            if (saveTokenResult.IsError)
            {
                await Toast.Make(loginResult.FirstError.Description).Show();

                return;
            }

            var userInfo = await userService.GetUserInformationAsync();

            await Toast.Make($"مرحبـأ بعودتكـ {userInfo.Value.name}, تمت عملية تسجيل الدخول بنجاح").Show();
            
            await Shell.Current.Navigation.PopToRootAsync();
            



            //To Clear Shell stack =>

            //await Shell.Current.GoToAsync($"{nameof(RegisterView)}/{nameof(ForggetPasswordView)}/{nameof(LoginView)}", true);
            //await Shell.Current.Navigation.PopToRootAsync();

            //OR

            //var stack = Shell.Current.Navigation.NavigationStack.ToArray();
            //for (int i = stack.Length - 1; i > 0; i--)
            //{
            //    Shell.Current.Navigation.RemovePage(stack[i]);
            //}
        }
        catch (Exception ex)
        {

            await Toast.Make(ex.Message).Show();
        }
    }

    private async void BtnResetPassword_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Shell.Current.GoToAsync(nameof(ForggetPasswordView), true);
        }
        catch (Exception ex)
        {
            await Toast.Make(ex.Message).Show();
        }
    }

    private async void BtnRegister_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Shell.Current.GoToAsync(nameof(RegisterView), true);
        }
        catch (Exception ex)
        {
            await Toast.Make(ex.Message).Show();
        }
    }
}