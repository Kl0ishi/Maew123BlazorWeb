namespace Maew123.Api.Repositories.Contracts
{
    public interface IProductCatagoryRepository
    {
        Task<ProductCatagory> GetCatagory(int id);
        Task<List<ProductCatagory>> GetCatagories();
        Task<ProductCatagory> CreateCatagory(ProductCatagory catagory);
        Task<ProductCatagory> UpdateCatagory(ProductCatagory catagory);
        Task<bool> DeleteCatagory(int id, string updateBy);

    }
}
