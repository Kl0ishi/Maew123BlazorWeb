using Maew123.Api.Models;
using Microsoft.EntityFrameworkCore;


namespace Maew123.Api.Repositories
{
    public class ProductStockRepository : IProductStockRepository
    {
        private readonly ItshopMaew123Context _dbcontext;

        public ProductStockRepository(ItshopMaew123Context dbcontext)
        {
            this._dbcontext = dbcontext;
        }

        public async Task<int> UpdateStock(StocksDto stockDto)
        {
            var stock = await _dbcontext.ProductStocks.FindAsync(stockDto.ProductStockId);
            if (stock != null)
            {
                if (stockDto.Subtract ?? false)
                {
                    if (stockDto.NumStock > stock.NumStock)
                        return -666; //หมายถึงระเบิด
                    var DecreasedProduct = new DecreasedProduct() //เพิ่มจำนวนก่อนเปลี่ยนเป็นลบ
                    {
                        ProductId = stock.ProductId,
                        DecreaseDate = stockDto.UpdateDate,
                        DecreaseQuantity = stockDto.NumStock,
                        DecreaseReason = stockDto.DecreasedReason,
                        DecreaseBy = stockDto.UpdateBy
                    };
                    _dbcontext.DecreasedProducts.Add(DecreasedProduct);

                    stockDto.NumStock = -stockDto.NumStock;
                }

                if(stock.NumStock == null)
                    stock.NumStock = 0;

                stock.NumStock += stockDto.NumStock;
                stock.UpdateBy = stockDto.UpdateBy;
                stock.UpdateDate = stockDto.UpdateDate;

                if (stock.NumStock < 0)
                    return -666;

                _dbcontext.ProductStocks.Update(stock);
                await _dbcontext.SaveChangesAsync();
                return stock.NumStock!.Value;
            }
            return -666;
        }

        public async Task<ProductStock> GetStockbyProductId(int productId)
        {
            var stock = await _dbcontext.ProductStocks.FirstOrDefaultAsync(p => p.ProductId == productId);
            return stock;
        }

        public async Task UpdateDecreaseStock(ProductStock stock)
        {
            _dbcontext.Update(stock);
            await _dbcontext.SaveChangesAsync();

            //maybe cause error, not sure
            if (stock.NumStock == 0)
            {
                var product = await _dbcontext.Products.FindAsync(stock.ProductId);
                if (product != null)
                {
                    product.ProductStatus = "Out of stock";
                    _dbcontext.Products.Update(product);
                    await _dbcontext.SaveChangesAsync();
                }
            }
        }


        //debug zone
        public async Task UpdateAllNonStock()
        {
            var productsNonStock = await (from a in _dbcontext.Products
                                          where !_dbcontext.ProductStocks.Any(b => b.ProductId == a.ProductId)
                                          select new ProductStock
                                          {
                                              NumStock = 1, 
                                              ProductId = a.ProductId,
                                              InsertBy = "System", 
                                              InsertDate = DateTime.Now, 
                                              UpdateBy = "System", 
                                              UpdateDate = DateTime.Now
                                          }).ToListAsync();

            if(productsNonStock.Count > 0)
            {
                await _dbcontext.ProductStocks.AddRangeAsync(productsNonStock);

                await _dbcontext.SaveChangesAsync();
            }

            Console.WriteLine("ไม่มีตัวไหนไม่มี Stocks");
        }

        public async Task<List<DecreasedProduct>> GetDecreasedProductsAsync()
        {
            return await _dbcontext.DecreasedProducts
                .OrderByDescending(dp => dp.DecreaseDate)
                .ToListAsync();
        }

    }
}
