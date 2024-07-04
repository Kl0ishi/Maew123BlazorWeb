using Maew123.Models;

namespace Maew123.Web.Services.Contracts
{
    public interface ITypeService
    {
        List<ProductType> ProductTypes { get; set; }
        Task getProductTypes();

        Task<ServiceResponse<ProductType>> GetProductType(int producttypeid);

        Task<ServiceResponse<ProductType>> UpdateProductType(ProductType producttype);

        Task<ProductType> CreateProductType(ProductType producttype);
        Task DeleteProductType(int producttypeid);
    }
}
