namespace Dzone.Mobile.Views;

public partial class HomeView : ContentPage
{
    public HomeView()
    {
        InitializeComponent();
    }

    private async void BtnGoToLogin_Clicked(object sender, EventArgs e)
    {

        await Shell.Current.GoToAsync(nameof(LoginPage), false);
        //await Shell.Current.GoToAsync($"{nameof(LoginPage)}/{nameof(RegisterView)}");

    }
}