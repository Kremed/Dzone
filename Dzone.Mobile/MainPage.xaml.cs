using System.Diagnostics.Contracts;

namespace Dzone.Mobile
{
    public partial class MainPage : ContentPage
    {
        public IProductsService productsService;
        public ILocalTokenService userService;

        public List<Category> Categories = new List<Category>();

        public MainPage(IProductsService productsService, ILocalTokenService userService)
        {
            InitializeComponent();

            this.userService = userService;
            this.productsService = productsService;

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                var userInfo = await userService.GetUserInformationAsync();

                if (userInfo.IsError is not true)
                    LblUserName.Text = $"مرحبـأ {userInfo.Value.name}";

                var result = await productsService.GetStartData();

                result.Value.FirstOrDefault()!.IsActive = false;

                CategoryCollectionView.ItemsSource = result.Value;
            });
        }

        private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            try
            {
                if (e.Parameter is null)
                {
                    await Toast.Make("الرجاء اختيار التصنيف المناسب").Show();
                    return;
                }
                var parameters = new Dictionary<string, object>
                {
                    { "SelectedCategory", e.Parameter!}
                };

                await Shell.Current.GoToAsync($"{nameof(ProductsView)}", parameters);
            }
            catch (Exception ex)
            {
                await Toast.Make(ex.Message).Show();
            }
        }
    }
}