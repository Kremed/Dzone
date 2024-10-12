using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Alerts;

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
                await Shell.Current.GoToAsync(nameof(ConfirmOtpView));
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
}