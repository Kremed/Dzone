using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace Dzone.Mobile.Views.AuthViews;

public partial class ForggetPasswordView : ContentPage
{
    public IAuthService authService;

    public ForggetPasswordView(IAuthService authService)
    {
        InitializeComponent();
        this.authService = authService;
    }

    private async void BtnSendrestPasswordOtp_Clicked(object sender, EventArgs e)
    {
        try
        {
            //if (string.IsNullOrEmpty(TxtEmail.Text))
            //{
            //    await Toast.Make("الرجاء ادخال البريد الألكتروني, اعادة المحاولة").Show();
            //    return;
            //}

            ForgetPasswordRequest contract = new()
            {
                email = TxtEmail.Text
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



            var result = await authService.ForgotPassword(contract);

            if (result.IsError)
            {
                await Toast.Make(result.FirstError.Description!).Show();
                return;
            }

            var parameters = new Dictionary<string, object>
            {
                { "email", contract.email },
                { "otpType", "RestPassword" },
                { "title", $"لقد تم أرسال رمز تحقق لمرة واحدة الي بريدكـ الألكتروني : {contract.email}, الرجاء التحقق من بريدك الالكتروني وتـأكيد رمز التحقق OTP في الحقل المخصص لتفعيل حسابكـ"}
            };

            await Shell.Current.GoToAsync($"../{nameof(ConfirmOtpView)}", parameters);
        }
        catch (Exception)
        {
            throw;
        }
    }
}