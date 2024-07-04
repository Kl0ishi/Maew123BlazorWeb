

namespace Maew123.Api.Repositories.Contracts
{
    public interface IProductStockRepository
    {
        Task<int> UpdateStock(StocksDto stockDto);
        Task<ProductStock> GetStockbyProductId(int productId);
        Task UpdateDecreaseStock(ProductStock stock);

        Task UpdateAllNonStock();

        Task<List<DecreasedProduct>> GetDecreasedProductsAsync();
    }
}
