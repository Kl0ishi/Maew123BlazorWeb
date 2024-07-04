using Maew123.Api.Models;

using Maew123.Api.Utilities;
using Maew123.Models;

using Maew123.Models.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Maew123.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ItshopMaew123Context _dbcontext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductRepository(ItshopMaew123Context dbcontext, IWebHostEnvironment webHostEnvironment)
        {
            _dbcontext = dbcontext;
            this._webHostEnvironment = webHostEnvironment;
        }

        //public async Task<ProductDto> GetProductAsync(int productId)
        //{
        //    var product = await _dbcontext.Products
        //        .Include(p => p.ProductCatagory)
        //            .ThenInclude(c => c.ProductTypes)
        //        .Include(p => p.ProductStocks)
        //        .Include(p => p.Promotion)
        //        .FirstOrDefaultAsync(p => p.ProductId == productId);

        //    var productDto = product.MapToProductDto();
        //    return productDto;
        //}

        public async Task<NewProductDto> GetProductAsync(int productId)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var product = await (from a in _dbcontext.Products
                                  join b in _dbcontext.ProductCatagories on a.ProductCatagoryId equals b.ProductCatagoryId
                                  join c in _dbcontext.ProductTypes on a.ProductTypeId equals c.ProductTypeId
                                  join d in _dbcontext.ProductStocks on a.ProductId equals d.ProductId
                                  join e in _dbcontext.Promotions on a.PromotionId equals e.PromotionId into promotions
                                  from e in promotions.DefaultIfEmpty()  // Use left join
                                  where a.Deleted == false && a.ProductId == productId
                                  select new NewProductDto
                                  {
                                      ProductId = a.ProductId,
                                      ProductName = a.ProductName,
                                      ProductStatus = a.ProductStatus,
                                      ProductCatagoryId = a.ProductCatagoryId,
                                      ProductCatagoryName = b.ProductCatagoryName,
                                      ProductTypeId = a.ProductTypeId,
                                      ProductTypeName = c.ProductTypeName,
                                      stockId = d.ProductStockId,
                                      numStock = d.NumStock,
                                      PromotionId = e != null && e.EndDate >= today ? a.PromotionId : null,
                                      PromotionName = e != null ? e.PromotionName : "No Promotion",
                                      PromotionType = e.PromotionType,
                                      Price = a.Price,
                                      Discount = e != null && e.EndDate >= today ? e.DiscountPer : 0, // Check if promotion is active
                                      ProPrice = CalculatePrice.CalPromoPrice(a.Price, e.PromotionType, e.DiscountPer, 0, 0, 0),
                                      Condition = a.Condition,
                                      Description = a.Description,
                                      InsertBy = a.InsertBy,
                                      InsertDate = a.InsertDate,
                                      UpdateBy = a.UpdateBy,
                                      UpdateDate = a.UpdateDate,
                                      Featured = a.Featured.GetValueOrDefault(),
                                      Visible = a.Visible.GetValueOrDefault(),
                                      Deleted = a.Deleted.GetValueOrDefault(),
                                      ImgPath = a.ImgPath,

                                  }).FirstOrDefaultAsync();

            return product;
        }

        public async Task<List<ProductDto>> GetProductsAsync()
        {
            var products = await _dbcontext.Products
                .Include(p => p.ProductCatagory)
                .Include(p => p.ProductStocks)
                .Include(p => p.Promotion)
                .ToListAsync();

            var productsDto = products.MapToProductDto();
            return productsDto;
        }

        public async Task<List<NewProductDto>> NewGetAdminProducts()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var products = await (from a in _dbcontext.Products
                                  join b in _dbcontext.ProductCatagories on a.ProductCatagoryId equals b.ProductCatagoryId
                                  join c in _dbcontext.ProductTypes on a.ProductTypeId equals c.ProductTypeId
                                  join d in _dbcontext.ProductStocks on a.ProductId equals d.ProductId
                                  join e in _dbcontext.Promotions on a.PromotionId equals e.PromotionId into promotions
                                  from e in promotions.DefaultIfEmpty()  // Use left join
                                  where a.Deleted == false
                                  select new NewProductDto
                                  {
                                      ProductId = a.ProductId,
                                      ProductName = a.ProductName,
                                      ProductStatus = a.ProductStatus,
                                      ProductCatagoryId = a.ProductCatagoryId,
                                      ProductCatagoryName = b.ProductCatagoryName,
                                      ProductTypeId = a.ProductTypeId,
                                      ProductTypeName = c.ProductTypeName,
                                      stockId = d.ProductStockId,
                                      numStock = d.NumStock,
                                      PromotionId = e != null && e.EndDate >= today ? a.PromotionId : null,
                                      PromotionName = e != null ? e.PromotionName : "No Promotion",
                                      PromotionType = e.PromotionType,
                                      Price = a.Price,
                                      Discount = e != null && e.EndDate >= today ? e.DiscountPer : 0, // Check if promotion is active
                                      ProPrice = CalculatePrice.CalPromoPrice(a.Price, e.PromotionType, e.DiscountPer, 0, 0, 0),
                                      Condition = a.Condition,
                                      Description = a.Description,
                                      InsertBy = a.InsertBy,
                                      InsertDate = a.InsertDate,
                                      UpdateBy = a.UpdateBy,
                                      UpdateDate = a.UpdateDate,
                                      Featured = a.Featured.GetValueOrDefault(),
                                      Visible = a.Visible.GetValueOrDefault(),
                                      Deleted = a.Deleted.GetValueOrDefault(),
                                      ImgPath = a.ImgPath,

                                  }).ToListAsync();

            return products;
        }

        public async Task<List<NewProductDto>> NewGetProducts()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var products = await (from a in _dbcontext.Products
                                  join b in _dbcontext.ProductCatagories on a.ProductCatagoryId equals b.ProductCatagoryId
                                  join c in _dbcontext.ProductTypes on a.ProductTypeId equals c.ProductTypeId
                                  join d in _dbcontext.ProductStocks on a.ProductId equals d.ProductId
                                  join e in _dbcontext.Promotions on a.PromotionId equals e.PromotionId into promotions
                                  from e in promotions.DefaultIfEmpty()  // Use left join
                                  where a.Visible == true && a.Deleted == false
                                  select new NewProductDto
                                  {
                                      ProductId = a.ProductId,
                                      ProductName = a.ProductName,
                                      ProductStatus = a.ProductStatus,
                                      ProductCatagoryId = a.ProductCatagoryId,
                                      ProductCatagoryName = b.ProductCatagoryName,
                                      ProductTypeId = a.ProductTypeId,
                                      ProductTypeName = c.ProductTypeName,
                                      stockId = d.ProductStockId,
                                      numStock = d.NumStock,
                                      PromotionId = e != null && e.EndDate >= today ? a.PromotionId : null,
                                      PromotionName = e != null ? e.PromotionName : "No Promotion",
                                      PromotionType = e.PromotionType,
                                      Price = a.Price,
                                      Discount = e != null && e.EndDate >= today ? e.DiscountPer : 0, // Check if promotion is active
                                      ProPrice = CalculatePrice.CalPromoPrice(a.Price, e.PromotionType, e.DiscountPer, 0, 0, 0),
                                      Condition = a.Condition,
                                      Description = a.Description,
                                      InsertBy = a.InsertBy,
                                      InsertDate = a.InsertDate,
                                      UpdateBy = a.UpdateBy,
                                      UpdateDate = a.UpdateDate,
                                      Featured = a.Featured.GetValueOrDefault(),
                                      Visible = a.Visible.GetValueOrDefault(),
                                      Deleted = a.Deleted.GetValueOrDefault(),
                                      ImgPath = a.ImgPath,

                                  }).ToListAsync();

            return products;
        }

        public async Task<List<NewProductDto>> GetFeaturedProducts()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var products = await (from a in _dbcontext.Products
                                  join b in _dbcontext.ProductCatagories on a.ProductCatagoryId equals b.ProductCatagoryId
                                  join c in _dbcontext.ProductTypes on a.ProductTypeId equals c.ProductTypeId
                                  join d in _dbcontext.ProductStocks on a.ProductId equals d.ProductId
                                  join e in _dbcontext.Promotions on a.PromotionId equals e.PromotionId into promotions
                                  from e in promotions.DefaultIfEmpty()  // Use left join
                                  where a.Featured == true && a.Visible == true && a.Deleted == false
                                  select new NewProductDto
                                  {
                                      ProductId = a.ProductId,
                                      ProductName = a.ProductName,
                                      ProductStatus = a.ProductStatus,
                                      ProductCatagoryId = a.ProductCatagoryId,
                                      ProductCatagoryName = b.ProductCatagoryName,
                                      ProductTypeId = a.ProductTypeId,
                                      ProductTypeName = c.ProductTypeName,
                                      stockId = d.ProductStockId,
                                      numStock = d.NumStock,
                                      PromotionId = e != null && e.EndDate >= today ? a.PromotionId : null,
                                      PromotionName = e != null ? e.PromotionName : "No Promotion",
                                      PromotionType = e.PromotionType,
                                      Price = a.Price,
                                      Discount = e != null && e.EndDate >= today ? e.DiscountPer : 0, // Check if promotion is active
                                      ProPrice = CalculatePrice.CalPromoPrice(a.Price, e.PromotionType, e.DiscountPer, 0, 0, 0),
                                      Condition = a.Condition,
                                      Description = a.Description,
                                      InsertBy = a.InsertBy,
                                      InsertDate = a.InsertDate,
                                      UpdateBy = a.UpdateBy,
                                      UpdateDate = a.UpdateDate,
                                      Featured = a.Featured.GetValueOrDefault(),
                                      Visible = a.Visible.GetValueOrDefault(),
                                      Deleted = a.Deleted.GetValueOrDefault(),
                                      ImgPath = a.ImgPath,

                                  }).ToListAsync();
            return products;
        }

        public async Task<List<NewProductDto>> GetBestSellerProducts()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var products = await (from a in _dbcontext.Products
                                  join b in _dbcontext.ProductCatagories on a.ProductCatagoryId equals b.ProductCatagoryId
                                  join c in _dbcontext.ProductTypes on a.ProductTypeId equals c.ProductTypeId
                                  join d in _dbcontext.ProductStocks on a.ProductId equals d.ProductId
                                  join e in _dbcontext.Promotions on a.PromotionId equals e.PromotionId into promotions
                                  from e in promotions.DefaultIfEmpty()  // Use left join
                                  where a.Featured == true && a.Visible == true && a.Deleted == false
                                  select new NewProductDto
                                  {
                                      ProductId = a.ProductId,
                                      ProductName = a.ProductName,
                                      ProductStatus = a.ProductStatus,
                                      ProductCatagoryId = a.ProductCatagoryId,
                                      ProductCatagoryName = b.ProductCatagoryName,
                                      ProductTypeId = a.ProductTypeId,
                                      ProductTypeName = c.ProductTypeName,
                                      stockId = d.ProductStockId,
                                      numStock = d.NumStock,
                                      PromotionId = e != null && e.EndDate >= today ? a.PromotionId : null,
                                      PromotionName = e != null ? e.PromotionName : "No Promotion",
                                      PromotionType = e.PromotionType,
                                      Price = a.Price,
                                      Discount = e != null && e.EndDate >= today ? e.DiscountPer : 0, // Check if promotion is active
                                      ProPrice = CalculatePrice.CalPromoPrice(a.Price, e.PromotionType, e.DiscountPer, 0, 0, 0),
                                      Condition = a.Condition,
                                      Description = a.Description,
                                      InsertBy = a.InsertBy,
                                      InsertDate = a.InsertDate,
                                      UpdateBy = a.UpdateBy,
                                      UpdateDate = a.UpdateDate,
                                      Featured = a.Featured.GetValueOrDefault(),
                                      Visible = a.Visible.GetValueOrDefault(),
                                      Deleted = a.Deleted.GetValueOrDefault(),
                                      ImgPath = a.ImgPath,
                                  }).ToListAsync();

            return products;
        }

        public async Task<List<NewProductDto>> GetProductsByCategoryAsync(string categoryUrl)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var products = await (from a in _dbcontext.Products
                                  join b in _dbcontext.ProductCatagories on a.ProductCatagoryId equals b.ProductCatagoryId
                                  join c in _dbcontext.ProductTypes on a.ProductTypeId equals c.ProductTypeId
                                  join d in _dbcontext.ProductStocks on a.ProductId equals d.ProductId
                                  join e in _dbcontext.Promotions on a.PromotionId equals e.PromotionId into promotions
                                  from e in promotions.DefaultIfEmpty()  // Use left join
                                  where b.Url.ToLower().Equals(categoryUrl.ToLower()) && a.Visible == true && a.Deleted == false
                                  select new NewProductDto
                                  {
                                      ProductId = a.ProductId,
                                      ProductName = a.ProductName,
                                      ProductStatus = a.ProductStatus,
                                      ProductCatagoryId = a.ProductCatagoryId,
                                      ProductCatagoryName = b.ProductCatagoryName,
                                      ProductTypeId = a.ProductTypeId,
                                      ProductTypeName = c.ProductTypeName,
                                      stockId = d.ProductStockId,
                                      numStock = d.NumStock,
                                      PromotionId = e != null && e.EndDate >= today ? a.PromotionId : null,
                                      PromotionName = e != null ? e.PromotionName : "No Promotion",
                                      PromotionType = e.PromotionType,
                                      Price = a.Price,
                                      Discount = e != null && e.EndDate >= today ? e.DiscountPer : 0, // Check if promotion is active
                                      ProPrice = CalculatePrice.CalPromoPrice(a.Price, e.PromotionType, e.DiscountPer, 0, 0, 0),
                                      Condition = a.Condition,
                                      Description = a.Description,
                                      InsertBy = a.InsertBy,
                                      InsertDate = a.InsertDate,
                                      UpdateBy = a.UpdateBy,
                                      UpdateDate = a.UpdateDate,
                                      Featured = a.Featured.GetValueOrDefault(),
                                      Visible = a.Visible.GetValueOrDefault(),
                                      Deleted = a.Deleted.GetValueOrDefault(),
                                      ImgPath = a.ImgPath,

                                  }).ToListAsync();

            return products;
        }

        public async Task<List<NewProductDto>> GetProductsByTypeAsync(int TypeId)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var products = await (from a in _dbcontext.Products
                                  join b in _dbcontext.ProductCatagories on a.ProductCatagoryId equals b.ProductCatagoryId
                                  join c in _dbcontext.ProductTypes on a.ProductTypeId equals c.ProductTypeId
                                  join d in _dbcontext.ProductStocks on a.ProductId equals d.ProductId
                                  join e in _dbcontext.Promotions on a.PromotionId equals e.PromotionId into promotions
                                  from e in promotions.DefaultIfEmpty()  // Use left join
                                  where a.ProductTypeId == TypeId && a.Visible == true && a.Deleted == false
                                  select new NewProductDto
                                  {
                                      ProductId = a.ProductId,
                                      ProductName = a.ProductName,
                                      ProductStatus = a.ProductStatus,
                                      ProductCatagoryId = a.ProductCatagoryId,
                                      ProductCatagoryName = b.ProductCatagoryName,
                                      ProductTypeId = a.ProductTypeId,
                                      ProductTypeName = c.ProductTypeName,
                                      stockId = d.ProductStockId,
                                      numStock = d.NumStock,
                                      PromotionId = e != null && e.EndDate >= today ? a.PromotionId : null,
                                      PromotionName = e != null ? e.PromotionName : "No Promotion",
                                      PromotionType = e.PromotionType,
                                      Price = a.Price,
                                      Discount = e != null && e.EndDate >= today ? e.DiscountPer : 0, // Check if promotion is active
                                      ProPrice = CalculatePrice.CalPromoPrice(a.Price, e.PromotionType, e.DiscountPer, 0, 0, 0),
                                      Condition = a.Condition,
                                      Description = a.Description,
                                      InsertBy = a.InsertBy,
                                      InsertDate = a.InsertDate,
                                      UpdateBy = a.UpdateBy,
                                      UpdateDate = a.UpdateDate,
                                      Featured = a.Featured.GetValueOrDefault(),
                                      Visible = a.Visible.GetValueOrDefault(),
                                      Deleted = a.Deleted.GetValueOrDefault(),
                                      ImgPath = a.ImgPath,

                                  }).ToListAsync();

            return products;
        }

        //public async Task<List<ProductDto>> SearchProducts(string searchText, int page)
        //{
        //    var pageResults = 2f;
        //    var pageCount = Math.Ceiling((await FindProductsBySearchText(searchText)).Count() / pageResults);
        //    var products = await _dbcontext.Products
        //                        .Where(p => p.ProductName.ToLower().Contains(searchText.ToLower())
        //                        ||
        //                        p.Description.ToLower().Contains(searchText.ToLower()))
        //                        .Include(p => p.ProductCatagory)
        //                        .Skip((page - 1) * (int)pageResults)
        //                        .Take((int)pageResults)
        //                        .ToListAsync();

        //    var productsDto = products.MapToProductDto();
        //    return productsDto;
        //}

        public async Task<List<NewProductDto>> FindProductsBySearchText(string searchText)
        {
            var products = await (from a in _dbcontext.Products
                                  join b in _dbcontext.ProductCatagories on a.ProductCatagoryId equals b.ProductCatagoryId
                                  join c in _dbcontext.ProductTypes on a.ProductTypeId equals c.ProductTypeId
                                  join d in _dbcontext.ProductStocks on a.ProductId equals d.ProductId
                                  join e in _dbcontext.Promotions on a.PromotionId equals e.PromotionId into promotions
                                  from e in promotions.DefaultIfEmpty()  // Use left join
                                  where a.ProductName.ToLower().Contains(searchText.ToLower()) || a.Description.ToLower().Contains(searchText.ToLower())
                                  && a.Visible == true && a.Deleted != false
                                  select new NewProductDto
                                  {
                                      ProductId = a.ProductId,
                                      ProductName = a.ProductName,
                                      ProductStatus = a.ProductStatus,
                                      ProductCatagoryId = a.ProductCatagoryId,
                                      ProductCatagoryName = b.ProductCatagoryName,
                                      ProductTypeId = a.ProductTypeId,
                                      ProductTypeName = c.ProductTypeName,
                                      stockId = d.ProductStockId,
                                      numStock = d.NumStock,
                                      //PromotionId = a.PromotionId,
                                      //PromotionName = e != null ? e.PromotionName : "No Promotion",
                                      //PromotionType = e.PromotionType,
                                      Price = a.Price,
                                      Discount = e.DiscountPer,
                                      //ProPrice = CalPromoPrice(a.Price, e.PromotionType, e.DiscountPer),
                                      Condition = a.Condition,
                                      Description = a.Description,
                                      InsertBy = a.InsertBy,
                                      InsertDate = a.InsertDate,
                                      UpdateBy = a.UpdateBy,
                                      UpdateDate = a.UpdateDate,
                                      Featured = a.Featured.GetValueOrDefault(),
                                      Visible = a.Visible.GetValueOrDefault(),
                                      Deleted = a.Deleted.GetValueOrDefault(),
                                      ImgPath = a.ImgPath,

                                  }).ToListAsync();

            return products;
        }

        public async Task<List<DecreasedProductsDto>> GetDecreasedProductsAsync()
        {
            var decreasedProducts = await (from a in _dbcontext.DecreasedProducts
                                  join b in _dbcontext.Products on a.ProductId equals b.ProductId
                                  select new DecreasedProductsDto
                                  {
                                      DecreasedProductId = a.DecreasedProductId,
                                      ProductId = a.ProductId,
                                      ProductName = b.ProductName,
                                      Condition = b.Condition,
                                      ImgPath = b.ImgPath,
                                      DecreaseDate = a.DecreaseDate,
                                      DecreaseBy = a.DecreaseBy,
                                      DecreaseQuantity = a.DecreaseQuantity,
                                      DecreaseReason = a.DecreaseReason
                                  }).ToListAsync();
            return decreasedProducts;
        }

        public async Task<List<ProductDto>> GetAdminProducts()
        {
            var productsDto = (await _dbcontext.Products
                    .Include(p => p.ProductStocks)
                    .Where(p => p.Deleted == false)
                    .ToListAsync()
                    ).MapToProductDto();


            return productsDto;
        }
        //เพิ่มอีกอันเป็น GetProductPromotion
        public Task<List<ProductDto>> GetProductsByPromotionId(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductDto> CreateProduct(ProductDto productDto)
        {
            var product = productDto.ToProduct();
            var existingProduct = await _dbcontext.Products.FirstOrDefaultAsync(p => p.ProductName == product.ProductName && p.ProductType.ProductTypeId == product.ProductTypeId);

            if (existingProduct != null)
            {
                // Update existing Stock with the provided numStock
                product.ProductId = existingProduct.ProductId;
                var existingProductStock = await _dbcontext.ProductStocks.FirstOrDefaultAsync(ps => ps.ProductId == existingProduct.ProductId);
                if (existingProductStock != null)
                {
                    existingProductStock.NumStock += productDto.numStock;
                    existingProduct.Deleted = false;

                    existingProductStock.UpdateBy = product.UpdateBy;
                    existingProductStock.UpdateDate = DateTime.Now;
                    _dbcontext.ProductStocks.Update(existingProductStock);
                }
                else
                {
                    // Create a new Stock if it doesn't exist
                    var newProductStock = new ProductStock
                    {
                        ProductId = existingProduct.ProductId,
                        NumStock = productDto.numStock,
                        InsertBy = existingProduct.InsertBy,
                        InsertDate = DateTime.Now,
                        UpdateBy = existingProduct.UpdateBy,
                        UpdateDate = DateTime.Now,
                    };
                    _dbcontext.ProductStocks.Add(newProductStock);
                }
                _dbcontext.Entry(existingProduct).State = EntityState.Detached;
            }

            else
            {
                // Create a new product and a corresponding ProductStock
                _dbcontext.Products.Add(product);
                await _dbcontext.SaveChangesAsync();

                await CreateProductStock(product, productDto.numStock);
            }

            if (productDto.Base64ImageData != null)
            {
                string uniqueFileName = UploadImage(productDto);
                product.ImgPath = uniqueFileName;
            }

            _dbcontext.Products.Update(product);
            await _dbcontext.SaveChangesAsync();

            productDto.ProductId = product.ProductId;
            return productDto;
        }

        public async Task<ProductDto> UpdateProduct(ProductDto product)
        {
            var dbProduct = await _dbcontext.Products
                .FirstOrDefaultAsync(p => p.ProductId == product.ProductId);

            if (dbProduct == null)
            {
                return new ProductDto
                {
                    IsFound = false
                };

            }

            dbProduct.ProductName = product.ProductName;
            dbProduct.ProductStatus = product.ProductStatus;
            dbProduct.Price = product.Price;
            dbProduct.Condition = product.Condition;
            dbProduct.Description = product.Description;
            dbProduct.ProductTypeId = product.ProductTypeId;
            dbProduct.ProductCatagoryId = product.ProductCatagoryId;
            dbProduct.PromotionId = product.PromotionId;
            dbProduct.Visible = product.Visible;
            dbProduct.Featured = product.Featured;
            dbProduct.UpdateBy = product.UpdateBy;
            dbProduct.UpdateDate = DateTime.Now;

            string uniqueFileName = string.Empty;
            if (product.Base64ImageData != null)
            {
                if (dbProduct.ImgPath != null)
                {
                    //แทนที่รูปอันเก่า
                    string rootPath = _webHostEnvironment.ContentRootPath;
                    string filepath = Path.Combine(rootPath, "ZStores/Images/Products/", dbProduct.ImgPath);
                    if (System.IO.File.Exists(filepath))
                    {
                        System.IO.File.Delete(filepath);
                    }
                }
                if (product.Base64ImageData != null)
                {
                    uniqueFileName = UploadImage(product);
                    dbProduct.ImgPath = uniqueFileName;
                }

            }

            await UpdateProductStock(product.ProductId, product.numStock);

            await _dbcontext.SaveChangesAsync();
            product.IsFound = true;
            return product;
        }

        public async Task<bool> DeleteProduct(int productId, string updateBy)
        {
            var dbProduct = await _dbcontext.Products.FindAsync(productId);
            if (dbProduct == null)
            {
                return false;
            }

            dbProduct.UpdateBy = updateBy;
            dbProduct.UpdateDate = DateTime.Now;
            dbProduct.Deleted = true;
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task UpdateOutOfStockToAvailable(List<NewProductDto> products)
        {
            var availableProducts = products.Where(p => p.numStock > 0).ToList();

            foreach (var product in availableProducts)
            {
                product.ProductStatus = "Available";

                // Update the product status in the database
                var dbProduct = await _dbcontext.Products.FindAsync(product.ProductId);
                if (dbProduct != null)
                {
                    dbProduct.ProductStatus = "Available";
                    _dbcontext.Products.Update(dbProduct);
                }
            }

            await _dbcontext.SaveChangesAsync();
        }

        public async Task CreateProductStock(Product product, int? numStock)
        {
            var newProductStock = new ProductStock
            {
                ProductId = product.ProductId,
                NumStock = numStock,
                InsertBy = product.InsertBy,
                InsertDate = product.InsertDate,
                UpdateBy = product.UpdateBy,
                UpdateDate = product.UpdateDate,
            };

            _dbcontext.ProductStocks.Add(newProductStock);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task UpdateProductStock(int productId, int? numStock)
        {
            var existingProductStock = await _dbcontext.ProductStocks
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (existingProductStock == null)
            {
                return;
            }
            existingProductStock.NumStock += numStock ?? 0;

            _dbcontext.ProductStocks.Update(existingProductStock);
            await _dbcontext.SaveChangesAsync();
        }

        private string UploadImage(ProductDto obj)
        {
            string uniqueFileName = string.Empty;
            if (obj.ImgPath != null)
            {
                try
                {
                    string base64ImageDataWithoutPrefix = obj.Base64ImageData!.Replace("data:image/png;base64,", "");

                    byte[] fileBytes = Convert.FromBase64String(base64ImageDataWithoutPrefix);

                    string rootPath = _webHostEnvironment.ContentRootPath;
                    string uploadFolder = Path.Combine(rootPath, "ZStores/Images/Products");

                    string fileExtension = GetFileExtensionFromBase64(obj.Base64ImageData!);

                    uniqueFileName = Guid.NewGuid().ToString() + fileExtension;

                    string filePath = Path.Combine(uploadFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        fileStream.Write(fileBytes, 0, fileBytes.Length);
                    }
                    //uniqueFileName = Guid.NewGuid().ToString() + "_" + obj.ImagePath.FileName;
                    //string filePath = Path.Combine(uploadFolder, uniqueFileName);
                    //using (var fileStream = new FileStream(filePath, FileMode.Create))
                    //{
                    //    obj.ImagePath.CopyTo(fileStream);
                    //}
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
            return uniqueFileName;
        }

        private string GetFileExtensionFromBase64(string base64Data)
        {
            string[] parts = base64Data.Split(',');
            string header = parts[0];

            // Extract the file extension from the header
            string[] headerParts = header.Split(';');
            string contentType = headerParts[0].Split(':')[1];
            string fileExtension = contentType.Split('/')[1];

            return "." + fileExtension;
        }

    }
}
