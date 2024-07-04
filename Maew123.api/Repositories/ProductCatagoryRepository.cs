using Maew123.Api.Models;


using Maew123.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Maew123.Api.Repositories
{
    public class ProductCatagoryRepository: IProductCatagoryRepository
    {
        private readonly ItshopMaew123Context _dbcontext;

        public ProductCatagoryRepository(ItshopMaew123Context dbcontext)
        {
            this._dbcontext = dbcontext;
        }

        public async Task<ProductCatagory> GetCatagory(int id)
        {
            var catagory = await _dbcontext.ProductCatagories
                .FirstOrDefaultAsync(c => c.ProductCatagoryId == id);

            return catagory;
        }

        public async Task<List<ProductCatagory>> GetCatagories()
        {
            var catagories = await _dbcontext.ProductCatagories
                .Where(c => c.Deleted != true)
                .ToListAsync();

            return catagories;
        }

        public async Task<ProductCatagory> CreateCatagory(ProductCatagory catagory)
        {
            catagory.Url = catagory.ProductCatagoryName?.ToLower();
            catagory.InsertDate = DateTime.Now;
            catagory.UpdateDate = DateTime.Now;

            _dbcontext.ProductCatagories.Add(catagory);
            await _dbcontext.SaveChangesAsync();

            return catagory;
        }

        public async Task<ProductCatagory> UpdateCatagory(ProductCatagory catagory)
        {
            var dbCatagory = await _dbcontext.ProductCatagories
                .FirstOrDefaultAsync(c => c.ProductCatagoryId == catagory.ProductCatagoryId);

            if (dbCatagory == null)
            {
                return null!;
            }

            dbCatagory.ProductCatagoryName = catagory.ProductCatagoryName;
            dbCatagory.Url = catagory.Url;
            dbCatagory.Visible = catagory.Visible;
            dbCatagory.Deleted = catagory.Deleted;
            dbCatagory.UpdateBy = catagory.UpdateBy;
            dbCatagory.UpdateDate = DateTime.Now;

            _dbcontext.ProductCatagories.Update(dbCatagory);
            await _dbcontext.SaveChangesAsync();

            return dbCatagory;
        }

        public async Task<bool> DeleteCatagory(int id, string updateBy)
        {
            var dbCatagory = await _dbcontext.ProductCatagories
                .FirstOrDefaultAsync(c => c.ProductCatagoryId==id);

            if (dbCatagory == null)
            {
                return false;
            }

            dbCatagory.UpdateBy= updateBy;
            dbCatagory.UpdateDate= DateTime.Now;
            dbCatagory.Deleted = true;

            _dbcontext.ProductCatagories.Update(dbCatagory);
            await _dbcontext.SaveChangesAsync();

            return true;
        }
        
    }
}
