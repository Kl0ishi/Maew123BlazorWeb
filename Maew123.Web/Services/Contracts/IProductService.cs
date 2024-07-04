using Maew123.Models.Models;
using Maew123.Models;

namespace Maew123.Web.Services.Contracts
{
    public interface IProductService
    {
        event Action ProductsChanged;
        Task<ProductSearchResultDto> LoadAdminProductIndex(ProductSearchParam param);
        List<NewProductDto> Products { get; set; }
        List<NewProductDto> BestSellerProducts { get; set; }
        List<ProductDto> AdminProducts { get; set; }
        string Message { get; set; }

        string LastSearchText { get; set; }
        Task GetProducts(string? categoryUrl = null);
        Task GetBestSellerProducts();
        Task GetProductsByTypeUnderCate(int typeId);
        Task<ServiceResponse<NewProductDto>> GetProduct(int productId);

        Task<List<string>> GetProductSearchSuggestions(string searchText);
        Task SearchProducts(string searchText);
        Task GetAdminProducts();
        Task<ProductDto> CreateProduct(ProductDto product);
        Task<ProductDto> UpdateProduct(ProductDto product);
        Task<int> UpdateStock(StocksDto stockDto);
        Task ResetPromotion();
        Task DeleteProduct(int productId);

        Task<List<DecreasedProductsDto>> GetDecreasedProducts();
    }
}
