namespace Maew123.Api.Repositories.Contracts
{
    public interface IProductTypeRepository
    {
        Task<ProductType> GetProductType(int id);
        Task<List<ProductType>> GetProductTypes();
        Task<ProductType> CreateProductType(ProductType productType);
        Task<ProductType> UpdateProductType(ProductType productType);
        Task<bool> DeleteProductType(int id, string updateBy);
    }
}
