namespace Dzone.Mobile
{
    public partial class MainPage : ContentPage
    {
        public IProductsService productsService;

        public List<Category> Categories = new List<Category>();

        public MainPage(IProductsService productsService)
        {
            InitializeComponent();

            this.productsService = productsService;

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                var result = await productsService.GetStartData();
                //if (result.IsError)
                //    Categories = result.Value;

                result.Value.FirstOrDefault()!.IsActive = false;
                //CategoryCollectionView.ItemsSource = result.Value;
            });
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(RegisterView)}");
        }
    }
}