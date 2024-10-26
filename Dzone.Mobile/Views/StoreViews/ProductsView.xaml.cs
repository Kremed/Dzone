namespace Dzone.Mobile.Views.StoreViews;

[QueryProperty(nameof(CategoryAndProducts), "SelectedCategory")]
public partial class ProductsView : ContentPage
{
    public ICartService cartService { get; set; }
    public Category CategoryAndProducts { get; set; } = new();

    public ProductsView(ICartService cartService)
    {
        InitializeComponent();
        this.cartService = cartService;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        try
        {
            base.OnNavigatedTo(args);

            if (CategoryAndProducts is null)
            {
                await Shell.Current.GoToAsync("..", true);
                return;
            }

            LblCategoryName.Text = CategoryAndProducts.Name;
            LblCategoryDiscrption.Text = CategoryAndProducts.Description;
            ProductsCollectionView.ItemsSource = CategoryAndProducts.Products;

        }
        catch (Exception ex)
        {
            await Toast.Make(ex.Message).Show();
        }
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            if (e.Parameter is null ||
                e.Parameter is not Product selectedProduct)
            {
                await Toast.Make("حدث خطاء اثناء اختيار الصنف, الرجاء اعادة المحاولة").Show();
                return;
            }

            var sss = e.Parameter as Product;
            
            var result = await cartService.AddItemToCart(selectedProduct);

            if (result.IsError)
            {
                await Toast.Make(result.FirstError.Description).Show();
                return;
            }

            await Toast.Make("تم أضافة العنصر الي السلة").Show();
        }
        catch (Exception ex)
        {
            await Toast.Make(ex.Message).Show();
        }
    }
}