using Maew123.Api.Models;

using Microsoft.EntityFrameworkCore;

namespace Maew123.Api.Repositories
{
    public class ProductTypeRepository : IProductTypeRepository
    {
        private readonly ItshopMaew123Context _dbcontext;

        public ProductTypeRepository(ItshopMaew123Context dbcontext)
        {
            this._dbcontext = dbcontext;
        }

        public async Task<ProductType> GetProductType(int id)
        {
            var productType = await _dbcontext.ProductTypes
                .Include(t => t.ProductCategory)
                .FirstOrDefaultAsync(t => t.ProductTypeId == id);

            return productType;
        }

        public async Task<List<ProductType>> GetProductTypes()
        {
            var productTypes = await _dbcontext.ProductTypes
                .Where(t => t.ProductTypeStatus != false)
                .ToListAsync();

            return productTypes;
        }

        public async Task<ProductType> CreateProductType(ProductType productType)
        {
            productType.ProductTypeStatus = true;
            productType.InsertDate = DateTime.Now;
            productType.UpdateDate = DateTime.Now;

            productType.ProductCategory = null;
            _dbcontext.ProductTypes.Add(productType);
            await _dbcontext.SaveChangesAsync();

            return productType;
        }

        public async Task<ProductType> UpdateProductType(ProductType productType)
        {
            var dbProductType = await _dbcontext.ProductTypes
                .FirstOrDefaultAsync(t => t.ProductTypeId == productType.ProductTypeId);

            if (dbProductType == null)
            {
                return null;
            }

            dbProductType.ProductTypeName = productType.ProductTypeName;
            dbProductType.ProductTypeStatus = productType.ProductTypeStatus;
            dbProductType.UpdateBy = productType.UpdateBy;
            dbProductType.UpdateDate = DateTime.Now;
            dbProductType.ProductCategoryId = productType.ProductCategoryId;

            _dbcontext.ProductTypes.Update(dbProductType);
            await _dbcontext.SaveChangesAsync();

            return dbProductType;
        }

        public async Task<bool> DeleteProductType(int id, string updateBy)
        {
            var dbProductType = await _dbcontext.ProductTypes
                .FirstOrDefaultAsync(t => t.ProductTypeId == id);

            if (dbProductType == null)
            {
                return false;
            }

            dbProductType.ProductTypeStatus = false;
            dbProductType.UpdateBy = updateBy;
            dbProductType.UpdateDate = DateTime.Now;

            _dbcontext.ProductTypes.Update(dbProductType);
            await _dbcontext.SaveChangesAsync();

            return true;
        }

    }
}
