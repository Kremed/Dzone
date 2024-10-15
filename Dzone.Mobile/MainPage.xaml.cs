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
    }
}