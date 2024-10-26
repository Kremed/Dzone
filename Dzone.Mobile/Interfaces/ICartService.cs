namespace Dzone.Mobile.Interfaces;

public interface ICartService
{
    Task<ErrorOr<bool>> AddItemToCart(Product product, string note = "", int quantity = 1);
    Task<ErrorOr<bool>> RemoveItemFromCart(int ID);
    Task<List<CartLocalTbl>> GetCartItems();
    Task<ErrorOr<bool>> DeleteAllCartItems();
    Task<ErrorOr<bool>> IncreaseItemQuantity(int ID);
    Task<ErrorOr<bool>> DecreaseItemQuantity(int ID);

}
