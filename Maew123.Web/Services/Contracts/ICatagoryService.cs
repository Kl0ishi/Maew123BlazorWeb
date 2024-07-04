using Maew123.Models;

namespace Maew123.Web.Services.Contracts
{
    public interface ICatagoryService
    {
        List<ProductCatagory> Catagories { get; set; }
        Task GetCatagories();

        Task<ServiceResponse<ProductCatagory>> GetCatagory(int catagoryid);

        Task<ServiceResponse<ProductCatagory>> UpdateCatagory(ProductCatagory catagory);

        Task<ProductCatagory> CreateCatagory(ProductCatagory catagory);
        Task DeleteCatagory(int catagoryid);
    }
}
