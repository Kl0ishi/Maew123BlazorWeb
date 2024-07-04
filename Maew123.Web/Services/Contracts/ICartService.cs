namespace Maew123.Web.Services.Contracts
{
    public interface ICartService
    {
        event Action OnChange;
        Task AddToCart(ItemQuantityDto cartItem);
        Task<List<ItemQuantityDto>> GetCartItems();
        Task<List<CartDetailsDto>> GetCartProducts();
        Task RemoveProductFromCart(int productId);
        Task RemoveCart();
        Task RemoveCheckedItemsFromCart(List<CartDetailsDto> checkedProducts);
        Task UpdateQuantity(CartDetailsDto product);
        Task<(bool success, string message)> AddCompareProduct(ItemQuantityDto compareitem);

        Task<List<ItemQuantityDto>> GetCompareItems();

        Task<List<CartDetailsDto>> GetCompareProducts();

        Task RemoveProductFromComparing(int productId);

    }
}
