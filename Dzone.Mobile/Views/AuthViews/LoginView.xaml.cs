namespace Dzone.Mobile.Views.AuthViews;

public partial class LoginView : ContentPage
{
    public LoginView()
    {
        InitializeComponent();
    }

    private void BtnLogin_Clicked(object sender, EventArgs e)
    {
    }

    private async void BtnResetPassword_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Application.Current!.MainPage = new AppShell();
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