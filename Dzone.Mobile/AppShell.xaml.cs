
namespace Dzone.Mobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));

            //Register Auth Views Route =>
            Routing.RegisterRoute(nameof(LoginView), typeof(LoginView));
            Routing.RegisterRoute(nameof(RegisterView), typeof(RegisterView));
            Routing.RegisterRoute(nameof(ConfirmOtpView), typeof(ConfirmOtpView));
            Routing.RegisterRoute(nameof(ForggetPasswordView), typeof(ForggetPasswordView));

            //Register Store Views Route =>
            Routing.RegisterRoute(nameof(CartView), typeof(CartView));
            Routing.RegisterRoute(nameof(ProductsView), typeof(ProductsView));
            Routing.RegisterRoute(nameof(CheckoutView), typeof(CheckoutView));
            Routing.RegisterRoute(nameof(OrdaresArchive), typeof(OrdaresArchive));
            Routing.RegisterRoute(nameof(ProductInfoView), typeof(ProductInfoView));
        }

        //fixing App Shell FlowDiriction Issue github [https://github.com/dotnet/maui/pull/23473#issuecomment-2309531487]
        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();
            try
            {
#if ANDROID
                var platformView = Handler!.PlatformView;
                var shellFlyoutRenderer = (ShellFlyoutRenderer)platformView!;
                shellFlyoutRenderer!.LayoutDirection = Android.Views.LayoutDirection.Rtl;
#elif IOS || MACCATALYST
            FlowDirection = FlowDirection.RightToLeft;
#endif
            }
            catch
            {
            }
        }

        private async void BtnLogout_Tapped(object sender, TappedEventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync(nameof(LoginView), true);

                Shell.Current.FlyoutIsPresented = false;
            }
            catch (Exception ex)
            {
                await Toast.Make(ex.Message).Show();
            }
        }
    }
}