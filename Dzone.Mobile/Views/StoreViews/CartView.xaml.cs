namespace Dzone.Mobile.Views.StoreViews;

public partial class CartView : ContentPage
{
    public ICartService cartService { get; set; }

    public CartView(ICartService cartService)
    {
        InitializeComponent();

        this.cartService = cartService;

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await GetCartItems();
        });
    }

    protected override void OnAppearing()
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await GetCartItems();
            //await cartService.DeleteAllCartItems();
        });
    }

    public async Task GetCartItems()
    {
        var cartItems = await cartService.GetCartItems();

        CartCollectionView.ItemsSource = cartItems;

        LblcartTotal.Text = cartItems.Sum(itemTotal => itemTotal.quantity * itemTotal.price).ToString("0.##") + " دل";
    }



    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            if (e.Parameter is null || e.Parameter is not int ID)
            {
                await Toast.Make("حدث خطاء اثناء اختيار الصنف, الرجاء اعادة المحاولة").Show();
                return;
            }

            await cartService.IncreaseItemQuantity(ID);

            await GetCartItems();
        }
        catch (Exception ex)
        {
            await Toast.Make(ex.Message).Show();
        }
    }

    private async void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        try
        {
            if (e.Parameter is null || e.Parameter is not int ID)
            {
                await Toast.Make("حدث خطاء اثناء اختيار الصنف, الرجاء اعادة المحاولة").Show();
                return;
            }

            await cartService.DecreaseItemQuantity(ID);

            await GetCartItems();
        }
        catch (Exception ex)
        {
            await Toast.Make(ex.Message).Show();
        }
    }
}