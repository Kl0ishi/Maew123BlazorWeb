using Maew123.Api.Models;
using Maew123.Models;


namespace Maew123.Api.Repositories.Contracts
{
    public interface IProductRepository
    {
        Task<List<NewProductDto>> NewGetAdminProducts();
        Task<List<NewProductDto>> NewGetProducts();
        Task<NewProductDto> GetProductAsync(int ProductId);
        Task<List<ProductDto>> GetProductsAsync();
        Task<List<NewProductDto>> GetProductsByCategoryAsync(string categoryUrl);
        Task<List<NewProductDto>> GetProductsByTypeAsync(int TypeId);
        //Task<List<ProductDto>> SearchProducts(string searchText, int page);
        Task<List<NewProductDto>> GetFeaturedProducts();
        Task<List<NewProductDto>> GetBestSellerProducts();

        Task<List<ProductDto>> GetAdminProducts();
        Task<List<ProductDto>> GetProductsByPromotionId(int id);

        Task<ProductDto> CreateProduct(ProductDto product);
        Task<ProductDto> UpdateProduct(ProductDto product);
        Task<bool> DeleteProduct(int productId, string updateBy);
        Task UpdateOutOfStockToAvailable(List<NewProductDto> products);
        Task<List<NewProductDto>> FindProductsBySearchText(string searchText);

        Task<List<DecreasedProductsDto>> GetDecreasedProductsAsync();
    }
}
