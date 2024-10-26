namespace Dzone.Mobile.Services;

public class CartService : ICartService
{
    //Configration
    //===============================================================
    public ISqliteSevice SqliteSevice { get; }
    public ISQLiteAsyncConnection DbConnection { get; set; }

    public CartService(ISqliteSevice SqliteSevice)
    {
        this.SqliteSevice = SqliteSevice;
        DbConnection = SqliteSevice.CreatConnection();
    }


    //Implementation
    //===============================================================
    public async Task<ErrorOr<bool>> AddItemToCart(Product product, string note, int quantity)
    {
        try
        {
            var SameItem = await DbConnection.Table<CartLocalTbl>()
                                             .Where(item => item.productId == product.Id )
                                             .FirstOrDefaultAsync();

            if (SameItem is not null )
            {
                SameItem.quantity = SameItem.quantity + 1;

                SameItem.note = note;

                await DbConnection.UpdateAsync(SameItem);

                return true;
            }

            CartLocalTbl cartItem = new()
            {
                //TO DO => Selected Color and Size !!
                productId = product.Id,
                name = product.Name,
                image = product.FirstImage,
                price = product.SellPrice,
                quantity = quantity,
                note = note,
            };

            await DbConnection.InsertAsync(cartItem);

            return true;

        }
        catch (Exception ex)
        {
            return Error.Unexpected(description: ex.Message);
        }
    }

    public async Task<ErrorOr<bool>> DeleteAllCartItems()
    {
        try
        {
            await DbConnection.DeleteAllAsync<CartLocalTbl>();

            return true;
        }
        catch (Exception ex)
        {
            return Error.Unexpected(description: ex.Message);
        }
    }

    public async Task<List<CartLocalTbl>> GetCartItems()
    {
        return await DbConnection.Table<CartLocalTbl>().ToListAsync();
    }

    public async Task<ErrorOr<bool>> IncreaseItemQuantity(int ID)
    {
        try
        {
            var SelectedItem = await DbConnection.Table<CartLocalTbl>()
                                             .Where(item => item.id == ID)
                                             .FirstOrDefaultAsync();

            if (SelectedItem is null)
                return Error.NotFound(description: "العنصر المطلوب غير موجود");

            SelectedItem.quantity = SelectedItem.quantity + 1;

            await DbConnection.UpdateAsync(SelectedItem);

            return true;


        }
        catch (Exception ex)
        {
            return Error.Unexpected(description: ex.Message);
        }
    }
    public async Task<ErrorOr<bool>> DecreaseItemQuantity(int ID)
    {
        try
        {
            var SelectedItem = await DbConnection.Table<CartLocalTbl>()
                                             .Where(item => item.id == ID)
                                             .FirstOrDefaultAsync();

            if (SelectedItem is null)
                return Error.NotFound(description: "العنصر المطلوب غير موجود");

            if (SelectedItem.quantity <= 1)
            {
                await DbConnection.DeleteAsync(SelectedItem);
                return true;
            }

            SelectedItem.quantity = SelectedItem.quantity - 1;

            await DbConnection.UpdateAsync(SelectedItem);

            return true;

        }
        catch (Exception ex)
        {
            return Error.Unexpected(description: ex.Message);
        }
    }
    public async Task<ErrorOr<bool>> RemoveItemFromCart(int ID)
    {
        try
        {
            var SelectedItem = await DbConnection.Table<CartLocalTbl>()
                                             .Where(item => item.id == ID)
                                             .FirstOrDefaultAsync();

            if (SelectedItem is null)
                return Error.NotFound(description: "العنصر المطلوب غير موجود");

            await DbConnection.DeleteAsync(SelectedItem);

            return true;

        }
        catch (Exception ex)
        {
            return Error.Unexpected(description: ex.Message);
        }
    }
}
