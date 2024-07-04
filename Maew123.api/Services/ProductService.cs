using Maew123.Api.Models;

using Maew123.Models;

using Microsoft.EntityFrameworkCore;

namespace Maew123.Api.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductStockRepository _pStockRepository;
        private readonly IPromotionRepository _promotionRepository;
        private readonly ISaleRepository _saleRepository;

        public ProductService(IProductRepository productRepository, IProductStockRepository pStockRepository, IPromotionRepository promotionRepository, ISaleRepository saleRepository)
        {
            _productRepository = productRepository;
            _pStockRepository = pStockRepository;
            _promotionRepository = promotionRepository;
            this._saleRepository = saleRepository;
        }

        public async Task<ServiceResponse<NewProductDto>> GetProduct(int productId)
        {
            var response = new ServiceResponse<NewProductDto>();
            var product = await _productRepository.GetProductAsync(productId);
            if (product == null)
            {
                response.Success = false;
                response.Message = "Sorry, but this product does not exist.";
            }
            else
            {
                response.Data = product;
                response.Success = true;
            }

            return response;
        }

        public async Task<ServiceResponse<List<ProductDto>>> GetProducts()
        {
            var response = new ServiceResponse<List<ProductDto>>
            {
                Data = await _productRepository.GetProductsAsync(),
                Success = true
            };
            return response;
        }

        public async Task<ServiceResponse<List<NewProductDto>>> GetProductsByCategory(string categoryUrl)
        {
            var response = new ServiceResponse<List<NewProductDto>>
            {
                Data = await _productRepository.GetProductsByCategoryAsync(categoryUrl),
                Success = true
            };

            return response;
        }

        public async Task<ServiceResponse<List<NewProductDto>>> GetProductsByType(int TypeId)
        {
            var response = new ServiceResponse<List<NewProductDto>>
            {
                Data = await _productRepository.GetProductsByTypeAsync(TypeId),
                Success = true
            };

            return response;
        }

        public async Task<ServiceResponse<List<string>>> GetProductSuggestions(string searchText)
        {
            var products = await _productRepository.FindProductsBySearchText(searchText);

            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var product in products)
            {
                if (product.ProductName.Contains(searchText, StringComparison.OrdinalIgnoreCase)) //หาว่าtext สั้นๆนั้น ใกล้เคียงกับtext เต็มอันไหนของproduct.Tileมั่ง
                {
                    result.Add(product.ProductName);
                }

                if (product.Description != null)
                {
                    var punctuation = product.Description.Where(char.IsPunctuation)
                        .Distinct().ToArray(); //ตัดเว้นวรรคออก แล้วเอาแต่ละคำเก็บเป็นarray
                    var words = product.Description.Split()
                        .Select(s => s.Trim(punctuation));

                    foreach (var word in words)
                    {
                        if (word.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                            && !result.Contains(word))
                        {
                            result.Add(word);
                        }
                    }
                }

                // Check for matches in category name
                if (product.ProductCatagoryName != null && product.ProductCatagoryName.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(product.ProductCatagoryName);
                }

                // Check for matches in product type name
                if (product.ProductTypeName != null && product.ProductTypeName.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(product.ProductTypeName);
                }
            }

            return new ServiceResponse<List<string>> { Data = result.ToList(), Success = true };
        }

        public async Task<ServiceResponse<List<NewProductDto>>> GetFeaturedProducts()
        {
            var response = new ServiceResponse<List<NewProductDto>>
            {
                Data = await _productRepository.GetFeaturedProducts(),
                Success = true
            };

            return response;
        }

        public async Task<ServiceResponse<List<NewProductDto>>> GetBestSellerProducts()
        {
            var allSaleItems = await _saleRepository.GetAllSaleItemForMostSale();
            var products = await _productRepository.GetBestSellerProducts();

            var productSaleCounts = allSaleItems
                .GroupBy(saleItem => saleItem.ProductId)
                .Select(group => new { ProductId = group.Key, SaleCount = group.Count() })
                .ToList();

            // Update products with the purchased counts
            foreach (var product in products)
            {
                // Find the corresponding count of sale items for the product's ProductId
                var saleCount = productSaleCounts.FirstOrDefault(pc => pc.ProductId == product.ProductId)?.SaleCount ?? 0;

                // Set the PurchasedCount property of the product
                product.PurchasedCount = saleCount;
            }

            products = products.OrderByDescending(p => p.PurchasedCount).ToList();

            var response = new ServiceResponse<List<NewProductDto>>
            {
                Data = products,
                Success = true
            };

            return response;
        }

        //public async Task<ServiceResponse<ProductSearchResultDto>> SearchProducts(string searchText, int page)
        //{
        //    var products = await _productRepository.SearchProducts(searchText, page);
        //    var response = new ServiceResponse<ProductSearchResultDto>
        //    {
        //        Data = new ProductSearchResultDto
        //        {
        //            Products = products,
        //            CurrentPage = page,
        //            Pages = (int)products.Count,
        //        },
        //        Success = true
        //    };

        //    return response;
        //}

        public async Task<ServiceResponse<List<ProductDto>>> GetAdminProducts()
        {
            var response = new ServiceResponse<List<ProductDto>>
            {
                Data = await _productRepository.GetAdminProducts(),
                Success = true
            };

            return response;
        }

        public async Task<ServiceResponse<ProductDto>> CreateProduct(ProductDto product, string UpdateBy)
        {
            product.InsertBy = UpdateBy;
            product.InsertDate = DateTime.Now;
            product.UpdateBy = UpdateBy;
            product.UpdateDate = DateTime.Now;
            var returnProduct = await _productRepository.CreateProduct(product);
            return new ServiceResponse<ProductDto> { Data = returnProduct, Success = true };
        }

        public async Task<ServiceResponse<ProductDto>> UpdateProduct(ProductDto product, string UpdateBy)
        {
            product.UpdateBy = UpdateBy;
            var result = await _productRepository.UpdateProduct(product);
            if (result.IsFound == false)
                return new ServiceResponse<ProductDto>
                {
                    Success = false,
                    Message = "Product not found."
                };

            return new ServiceResponse<ProductDto> { Data = result, Success = true };
        }

        public async Task<ServiceResponse<bool>> DeleteProduct(int productId, string UpdateBy)
        {
            var result = await _productRepository.DeleteProduct(productId, UpdateBy);
            if (result != true)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Data = false,
                    Message = "Product not found."
                };
            }

            return new ServiceResponse<bool> { Data = true, Success = true };
        }

        public async Task<ServiceResponse<int>> UpdateStock(StocksDto stockDto)
        {
            stockDto.UpdateDate = DateTime.Now;
            var response = await _pStockRepository.UpdateStock(stockDto);
            if (response != -666)
            {
                return new ServiceResponse<int>
                {
                    Data = response,
                    Success = true,
                    Message = $"อัปเดตstockเสร็จสิ้น จำนวนคงเหลือ: {response}"
                };
            }

            return new ServiceResponse<int>
            {
                Success = false,
                Message = "อัปเดตstockล้มเหลว... สต็อกสินค้าต่ำกว่าจำนวนที่กรอกมา"
            };

        }

        public async Task<ServiceResponse<List<DecreasedProductsDto>>> GetDecreasedProductsAsync()
        {
            var decreasedProduct = await _productRepository.GetDecreasedProductsAsync();
            return new ServiceResponse<List<DecreasedProductsDto>>
            {
                Data = decreasedProduct,
                Success = true,
            };
        }

        public async Task<ServiceResponse<int>> UpdatePromotion()
        {
            var promotions = await _promotionRepository.GetPromotions();
            var products = await _productRepository.GetAdminProducts();
            foreach (var product in products)
            {
                if (!promotions.Any(p => p.PromotionId == product.PromotionId))
                {
                    product.PromotionId = null;

                    await _productRepository.UpdateProduct(product);
                }
            }

            return new ServiceResponse<int>
            {
                Data = products.Count,
                Success = true,
                Message = $"Promotion IDs updated for products.{products.Count}"
            };

        }

        public async Task UpdateAllNonStock()
        {
            await _pStockRepository.UpdateAllNonStock();
        }
    }
}
