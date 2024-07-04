using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Maew123.Api.Services;
using Maew123.Models;

using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using Maew123.Models.Models;
using Microsoft.EntityFrameworkCore;
using Maew123.Api.Utilities;

namespace Maew123.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;

        public ProductController(ProductService productService, IUserRepository userRepository, IProductRepository productRepository)
        {
            _productService = productService;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        [HttpGet]
        [Route("[action]"), Authorize(Roles ="Admin,Employee")]
        public async Task<ActionResult<ServiceResponse<ProductSearchResultDto>>> Home([FromQuery] ProductSearchParam ProductSearchParam)
        {
            try
            {
                var Result = new ServiceResponse<ProductSearchResultDto>();

                if (ProductSearchParam.Currentpage == null || ProductSearchParam.Currentpage == 0)
                {
                    ProductSearchParam.Currentpage = 1;
                }

                int maxrowsperpage;
                maxrowsperpage = 12;

                var allProducts = await _productRepository.NewGetAdminProducts();

                await _productRepository.UpdateOutOfStockToAvailable(allProducts);

                Result.Success = true;
                Result.Message = "Fumo Fumo";

                foreach (var product in allProducts)
                {
                    product.ProPrice = await Task.Run(() => CalculatePrice.CalPromoPrice(product.Price, product.PromotionType, product.Discount, 0, 0, 0));
                }

                if (!string.IsNullOrEmpty(ProductSearchParam.searchText))
                {
                    allProducts = allProducts
                        .Where(b => b.ProductName.Contains(ProductSearchParam.searchText.Trim(), StringComparison.OrdinalIgnoreCase)
                               || b.ProductCatagoryName.Contains(ProductSearchParam.searchText.Trim(), StringComparison.OrdinalIgnoreCase)
                               || b.ProductTypeName.Contains(ProductSearchParam.searchText.Trim(), StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                if (ProductSearchParam.filterCata.HasValue && ProductSearchParam.filterCata.Value > 0)
                {
                    allProducts = allProducts
                        .Where(b => b.ProductCatagoryId == ProductSearchParam.filterCata.Value)
                        .ToList();
                }

                if (ProductSearchParam.filterType.HasValue && ProductSearchParam.filterType.Value > 0)
                {
                    allProducts = allProducts
                        .Where(b => b.ProductTypeId == ProductSearchParam.filterType.Value)
                        .ToList();
                }

                if (ProductSearchParam.minPrice.HasValue)
                {
                    allProducts = allProducts
                        .Where(b => b.ProPrice > 0 ? b.ProPrice >= ProductSearchParam.minPrice.Value : b.Price >= ProductSearchParam.minPrice.Value)
                        .ToList();
                }

                if (ProductSearchParam.maxPrice.HasValue && ProductSearchParam.maxPrice!= 0)
                {
                    allProducts = allProducts
                        .Where(b => b.ProPrice > 0 ? b.ProPrice <= ProductSearchParam.maxPrice.Value : b.Price <= ProductSearchParam.maxPrice.Value)
                        .ToList();
                }

                // Calculate pagination
                int Scount = allProducts.Count;
                int totalPages = (int)Math.Ceiling((decimal)Scount / (decimal)maxrowsperpage);
                int startPage = ProductSearchParam.Currentpage - 5;
                int endPage = ProductSearchParam.Currentpage + 4;

                if (startPage <= 0)
                {
                    endPage = endPage - (startPage - 1);
                    startPage = 1;
                }

                if (endPage > totalPages)
                {
                    endPage = totalPages;
                    if (endPage > 10)
                    {
                        startPage = endPage - 9;
                    }
                }

                Result.Data = new ProductSearchResultDto(); // Instantiate the object
                // Fetch books for the current page
                Result.Data.Products = allProducts
                    .OrderByDescending(x => x.ProductId)
                    .Skip((ProductSearchParam.Currentpage - 1) * maxrowsperpage)
                    .Take(maxrowsperpage)
                    .ToList();

                Result.Data.PageCount = Scount;
                Result.Data.CurrentPage = ProductSearchParam.Currentpage;
                Result.Data.PageSize = maxrowsperpage;
                Result.Data.TotalPages = totalPages;
                Result.Data.StartPage = startPage;
                Result.Data.EndPage = endPage;

                return Ok(Result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("[action]/{productId}")]
        public async Task<ActionResult<ServiceResponse<NewProductDto>>> GetProduct(int productId)
        {
            var result = await _productService.GetProduct(productId);
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<ServiceResponse<List<ProductDto>>>> GetProducts()
        {
            var result = await _productService.GetProducts();
            return Ok(result);
        }

        [HttpGet, Authorize(Roles = "Admin,Employee")]
        [Route("[action]")]
        public async Task<ActionResult<ServiceResponse<List<ProductDto>>>> GetAdminProducts()
        {
            var result = await _productService.GetAdminProducts();
            return Ok(result);
        }

        [HttpGet]
        [Route("category/{categoryUrl}")]
        public async Task<ActionResult<ServiceResponse<List<NewProductDto>>>> GetProductsByCategory(string categoryUrl)
        {
            var result = await _productService.GetProductsByCategory(categoryUrl);
            return Ok(result);
        }

        [HttpGet]
        [Route("type/{typeId}")]
        public async Task<ActionResult<ServiceResponse<List<NewProductDto>>>> GetProductsByType(int typeId)
        {
            var result = await _productService.GetProductsByType(typeId);
            return Ok(result);
        }

        //[HttpGet]
        //[Route("[action]")]
        //public async Task<ActionResult<ServiceResponse<ProductSearchResultDto>>> SearchProducts(string searchText, int page = 1)
        //{
        //    var result = await _productService.SearchProducts(searchText, page);
        //    return Ok(result);
        //}

        [HttpGet]
        [Route("[action]/{searchText}")]
        public async Task<ActionResult<ServiceResponse<List<string>>>> GetProductSearchSuggestions(string searchText)
        {
            var result = await _productService.GetProductSuggestions(searchText);
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<ServiceResponse<List<NewProductDto>>>> SearchProducts([FromQuery] ProductSearchParam ProductSearchParam)
        {
            try
            {
                var Result = new ServiceResponse<List<NewProductDto>>();


                var allProducts = await _productRepository.NewGetProducts();
                Result.Success = true;
                Result.Message = "Fumo Fumo";

                foreach (var product in allProducts)
                {
                    product.ProPrice = await Task.Run(() => CalculatePrice.CalPromoPrice(product.Price, product.PromotionType, product.Discount, 0, 0, 0));
                }

                if (!string.IsNullOrEmpty(ProductSearchParam.searchText))
                {
                    allProducts = allProducts
                        .Where(b => b.ProductName.Contains(ProductSearchParam.searchText.Trim(), StringComparison.OrdinalIgnoreCase)
                               || b.ProductCatagoryName.Contains(ProductSearchParam.searchText.Trim(), StringComparison.OrdinalIgnoreCase)
                               || b.ProductTypeName.Contains(ProductSearchParam.searchText.Trim(), StringComparison.OrdinalIgnoreCase)
                               || b.Description.Contains(ProductSearchParam.searchText.Trim(), StringComparison.OrdinalIgnoreCase)) //อ.นนแนะนำ ง่ายแต่โคตรเจ๋ง
                        .ToList();
                }

                if (ProductSearchParam.filterCata.HasValue && ProductSearchParam.filterCata.Value > 0)
                {
                    allProducts = allProducts
                        .Where(b => b.ProductCatagoryId == ProductSearchParam.filterCata.Value)
                        .ToList();
                }

                if (ProductSearchParam.filterType.HasValue && ProductSearchParam.filterType.Value > 0)
                {
                    allProducts = allProducts
                        .Where(b => b.ProductTypeId == ProductSearchParam.filterType.Value)
                        .ToList();
                }

                if (ProductSearchParam.minPrice.HasValue)
                {
                    allProducts = allProducts
                        .Where(b => b.ProPrice > 0 ? b.ProPrice >= ProductSearchParam.minPrice.Value : b.Price >= ProductSearchParam.minPrice.Value)
                        .ToList();
                }

                if (ProductSearchParam.maxPrice.HasValue && ProductSearchParam.maxPrice != 0)
                {
                    allProducts = allProducts
                        .Where(b => b.ProPrice > 0 ? b.ProPrice <= ProductSearchParam.maxPrice.Value : b.Price <= ProductSearchParam.maxPrice.Value)
                        .ToList();
                }

                Result.Data = allProducts;

                return Ok(Result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<ServiceResponse<List<NewProductDto>>>> GetFeaturedProducts()
        {
            var result = await _productService.GetFeaturedProducts();
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<ServiceResponse<List<NewProductDto>>>> GetBestSellerProducts()
        {
            var result = await _productService.GetBestSellerProducts();
            return Ok(result);
        }


        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<ServiceResponse<ProductDto>>> CreateProduct(ProductDto product)
        {
            try
            {
                var whoLogin = await getUser();
                var result = await _productService.CreateProduct(product, whoLogin.Username!);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        [HttpPut]
        [Route("[action]"), Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<ServiceResponse<ProductDto>>> UpdateProduct(ProductDto product)
        {
            var whoLogin = await getUser();
            var result = await _productService.UpdateProduct(product, whoLogin.Username!);
            return Ok(result);
        }

        [HttpDelete]
        [Route("[action]/{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteProduct(int id)
        {
            var whoLogin = await getUser();
            var result = await _productService.DeleteProduct(id, whoLogin.Username!);
            return Ok(result);
        }

        [HttpPost]
        [Route("[action]"), Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<ServiceResponse<int>>> UpdateStock(StocksDto stockDto)
        {
            var whoLogin = await getUser();
            stockDto.UpdateBy = whoLogin.Username!;
            var result = await _productService.UpdateStock(stockDto);
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]"), Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<ServiceResponse<int>>> UpdatePromotion()
        {
            var whoLogin = await getUser();
            var result = await _productService.UpdatePromotion();
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]"), Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<ServiceResponse<DecreasedProductsDto>>> GetDecreasedProducts()
        {
            var whoLogin = await getUser();
            var result = await _productService.GetDecreasedProductsAsync();
            return Ok(result);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Test(ZTestDto test)
        {
            await _productService.UpdateAllNonStock();
            return Ok();
        }

        private async Task<User> getUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _userRepository.GetUserById(int.Parse(userId!));
        }

        
    }
}
