using Maew123.Api.Services;
using Maew123.Api.Utilities;
using Maew123.Models;
using Maew123.Models.Dtos;
using Maew123.Models.InputedValues;
using Maew123.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Maew123.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly IStatusRepository _statusRepository;
        private readonly AuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly ISaleRepository _saleRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProductStockRepository _stockRepository;

        public SaleController(IStatusRepository statusRepository, AuthService authService, IUserRepository userRepository,
            ISaleRepository saleRepository, IWebHostEnvironment webHostEnvironment, IProductStockRepository stockRepository)
        {
            this._statusRepository = statusRepository;
            this._authService = authService;
            this._userRepository = userRepository;
            this._saleRepository = saleRepository;
            this._webHostEnvironment = webHostEnvironment;
            this._stockRepository = stockRepository;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<ServiceResponse<List<CartDetailsDto>>>> GetCart(CartDto cart)
        {
            //var user = await _userRepository.GetUserById(_authService.GetUserId());

            var Cart = new List<CartDetailsDto>();
            var ErrorMessage = string.Empty;
            var uniqueProductIds = new HashSet<int>();

            foreach (ItemQuantityDto itemQuantity in cart.Quans)
            {
                //ห้ามซ้ำกับก่อนหน้า
                if (uniqueProductIds.Contains(itemQuantity.ProductId))
                {
                    continue;
                }

                //เพิ่มจำนวนเป็นสูงสุดถ้าเกินstock
                var stock = await _stockRepository.GetStockbyProductId(itemQuantity.ProductId);
                if (itemQuantity.Quantity > stock.NumStock)
                {
                    itemQuantity.Quantity = stock.NumStock ?? 0;
                    ErrorMessage = "เนื่องจากมีสินค้าเกินจำนวนสูงสุด ระบบจึงขอทำการตัดเหลือปริมาณที่เป็นไปได้สูงสุด";
                }

                //คำนวณราคา
                int productId = itemQuantity.ProductId;
                int quantity = itemQuantity.Quantity;
                var item = await _saleRepository.GetCartDetails(productId, quantity);
                item.Proprice = await Task.Run(() => CalculatePrice.CalPromoPrice(item.Price, item.PromotionType, item.Discount, item.thresholdAmount, item.orderAmountDiscount, quantity));
                item.TotalPrice = item.Price * quantity;
                if (item.Proprice != null)
                    item.TotalPrice = item.Proprice * quantity;
                Cart.Add(item);
                uniqueProductIds.Add(productId);
            }

            var response = new ServiceResponse<List<CartDetailsDto>>
            {
                Data = Cart,
                Success = true,
                Message = ErrorMessage
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<ServiceResponse<List<string>>>> GetDeliveryMethod()
        {
            //await Task.Delay(100);
            return Ok(new ServiceResponse<List<string>>
            {
                Data = DropdownForProduct.DeliveryMethod,
                Success = true,
            });

        }

        [HttpPost]
        [Route("[action]"), Authorize]
        public async Task<ActionResult<ServiceResponse<int>>> Checkout([FromBody] CartDto cart)
        {
            try
            {
                var user = await _userRepository.GetUserById(_authService.GetUserId());

                var Result = new ServiceResponse<int>();

                if (!await CheckStockAvailability(cart.Quans))
                {
                    Result.Data = 0; Result.Success = false;
                    Result.Message = "จำนวนสินค้าเกินจำนวนสูงสุดในคลัง";
                    return BadRequest(Result);
                }

                if (cart.AddressId == null || cart.AddressId == 0)
                {
                    Result.Data = 0; Result.Success = false;
                    Result.Message = "กรุณาระบุข้อมูลสถานที่จัดส่ง";
                    return BadRequest(Result);
                }

                var Sale = new Sale
                {
                    UserId = user.UserId,
                    SaleCode = await CreateCartNumber(),
                    SaleNum = cart.Quans.Count,
                    SaleTotal = 0,
                    SaleDiscount = 0,
                    OrderDate = DateTime.Now,
                    StatusId = 1,
                    AddressSnapshotId = cart.AddressId??0
                };

                //ใส่Idใหม่ให้ sale
                var address = await _userRepository.GetAddressByAddressId(Sale.AddressSnapshotId);
                AddressSaleSnapshot addressSaleSnapshot = new AddressSaleSnapshot
                {
                    UserId = address.UserId,
                    FirstName = address.FirstName,
                    LastName = address.LastName,
                    AddressName = address.addressName,
                    Street = address.Street,
                    City = address.City,
                    State = address.State,
                    Zip = address.Zip,
                    Country = address.Country,
                    Phone = address.Phone,
                };
                var newAddressSnapId = await _userRepository.InsertAddressSaleSnapshot(addressSaleSnapshot);
                Sale.AddressSnapshotId = newAddressSnapId;

                var saleId = await _saleRepository.CreateSaleId(Sale);

                var SaleItems = new List<SaleItem>();

                foreach (ItemQuantityDto itemQuantity in cart.Quans)
                {
                    int productId = itemQuantity.ProductId;
                    int quantity = itemQuantity.Quantity;
                    var item = await _saleRepository.GetCartDetails(productId, quantity);
                    item.Proprice = await Task.Run(() => CalculatePrice.CalPromoPrice(item.Price, item.PromotionType, item.Discount, item.thresholdAmount, item.orderAmountDiscount, quantity));
                    item.TotalPrice = item.Price * quantity;

                    if (item.Proprice != 0)
                    {
                        item.TotalPrice = item.Proprice * quantity;
                        cart.SaleDiscount = Convert.ToInt32((item.Price - item.Proprice) * quantity);
                    }
                    else
                    {
                        cart.SaleDiscount = 0;
                    }

                    Sale.SaleTotal += item.TotalPrice;
                    Sale.SaleDiscount += cart.SaleDiscount;
                    SaleItems.Add(new SaleItem
                    {
                        SaleId = saleId,
                        ProductId = item.ProductId,
                        TotalPrice = item.TotalPrice,
                        Qty = quantity,
                        PromotionId = item.PromotionId,
                    });
                }

                foreach (SaleItem saleitem in SaleItems)
                {
                    await _saleRepository.SaveSaleItems(saleitem);
                    var stock = await _stockRepository.GetStockbyProductId(saleitem.ProductId);
                    stock.NumStock -= saleitem.Qty;
                    await _stockRepository.UpdateDecreaseStock(stock);
                }

                await _saleRepository.UpdateSale(Sale);

                Result.Data = saleId;
                Result.Success = true;
                Result.Message = "สั่งซื้อสำเร็จแล้ว";

                return Result;
            }
            catch (Exception ex)
            {
                return BadRequest($"เกิดข้อผิดพลาด: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("[action]"), Authorize]
        public async Task<ActionResult<ServiceResponse<int>>> GetAnnotatedCount()
        {
            var user = await _userRepository.GetUserById(_authService.GetUserId());

            var annotatedCount = await _saleRepository.GetAnnotatedCount(user.UserId);
            return Ok(new ServiceResponse<int>
            {
                Data = annotatedCount,
                Success = true
            });
        }

        [HttpGet]
        [Route("[action]"), Authorize]
        public async Task<ActionResult<ServiceResponse<List<CartsDto>>>> GetAllSaleById()
        {
            var user = await _userRepository.GetUserById(_authService.GetUserId());

            var saleHistory = await _saleRepository.GetAllSaleById(user.UserId);

            var salesToProcess = saleHistory.Where(sale => sale.StatusId == 1).ToList();;
            foreach (var sale in salesToProcess)
            {
                if (await CheckAndCancelOrderIfExpired(sale))
                {
                    sale.StatusId = 8;
                    sale.StatusName = "ไม่ชำระตามกำหนด";
                }
            }


            var result = new ServiceResponse<List<CartsDto>>
            {
                Data = saleHistory,
                Success = true
            };
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]"), Authorize]
        public async Task<ActionResult<ServiceResponse<List<CartsDto>>>> GetSaleHistory()
        {
            var user = await _userRepository.GetUserById(_authService.GetUserId());

            var saleHistory = await _saleRepository.GetSalesHistory(user.UserId);
            var result = new ServiceResponse<List<CartsDto>>
            {
                Data = saleHistory,
                Success = true
            };
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]"), Authorize]
        public async Task<ActionResult<ServiceResponse<List<CartsDto>>>> GetPaymentRequest()
        {
            var user = await _userRepository.GetUserById(_authService.GetUserId());

            var saleHistory = await _saleRepository.GetPaymentRequest(user.UserId);

            var salesToProcess = saleHistory.Where(sale => sale.StatusId == 1).ToList();;
            foreach (var sale in salesToProcess)
            {
                if (await CheckAndCancelOrderIfExpired(sale))
                {
                    sale.StatusId = 8;
                    sale.StatusName = "ไม่ชำระตามกำหนด";
                }
            }


            var result = new ServiceResponse<List<CartsDto>>
            {
                Data = saleHistory,
                Success = true
            };
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]"), Authorize]
        public async Task<ActionResult<ServiceResponse<List<CartsDto>>>> GetAlreadyPayment()
        {
            var user = await _userRepository.GetUserById(_authService.GetUserId());

            var saleHistory = await _saleRepository.GetAlreadyPayment(user.UserId);
            var result = new ServiceResponse<List<CartsDto>>
            {
                Data = saleHistory,
                Success = true
            };
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]"), Authorize]
        public async Task<ActionResult<ServiceResponse<List<CartsDto>>>> GetAnnotatedOrder()
        {
            var user = await _userRepository.GetUserById(_authService.GetUserId());

            var saleHistory = await _saleRepository.GetAnnotatedOrder(user.UserId);
            var result = new ServiceResponse<List<CartsDto>>
            {
                Data = saleHistory,
                Success = true
            };
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]"), Authorize]
        public async Task<ActionResult<ServiceResponse<List<CartsDto>>>> GetAlreadySent()
        {
            var user = await _userRepository.GetUserById(_authService.GetUserId());

            var saleHistory = await _saleRepository.GetAlreadySent(user.UserId);
            var result = new ServiceResponse<List<CartsDto>>
            {
                Data = saleHistory,
                Success = true
            };
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]"), Authorize]
        public async Task<ActionResult<ServiceResponse<List<CartsDto>>>> GetWaitForSent()
        {
            var user = await _userRepository.GetUserById(_authService.GetUserId());

            var saleHistory = await _saleRepository.GetWaitForSent(user.UserId);
            var result = new ServiceResponse<List<CartsDto>>
            {
                Data = saleHistory,
                Success = true
            };
            return Ok(result);
        }

        [HttpPost]
        [Route("[action]"), Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> Payment(CartsDto cartsDto)
        {
            var user = await _userRepository.GetUserById(_authService.GetUserId());

            var ImageUrl = UploadImage(cartsDto);
            if (ImageUrl != null)
            {
                cartsDto.ImgPath = ImageUrl;

                var result = new ServiceResponse<bool>
                {
                    Data = await _saleRepository.Payment(cartsDto),
                    Success = true
                };
                return result;
            }
            return BadRequest("เกิดข้อผิดพลาด");
        }

        [HttpPost]
        [Route("[action]"), Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> CancelOrder([FromBody] CartsDto cartsDto)
        {
            var user = await _userRepository.GetUserById(_authService.GetUserId());

            var result = new ServiceResponse<bool> { Success = false };

            var saleDb = await _saleRepository.GetSaleBySaleId(cartsDto.SaleId);
            if (saleDb.StatusId != 1)
                return BadRequest(result);

            if (await UpdateStock(cartsDto))
            {
                result.Data = await _saleRepository.CancelOrder(cartsDto);
                result.Success = true;
            }

            return result;
        }


        [HttpGet]
        [Route("[action]"), Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<ServiceResponse<SaleFilterResultDto>>> GetAllSalesByStatus([FromQuery] SaleFilterParam saleFilterParam)
        {
            try
            {
                var user = await _userRepository.GetUserById(_authService.GetUserId());
                var result = new ServiceResponse<SaleFilterResultDto>();

                if (saleFilterParam.Currentpage == null || saleFilterParam.Currentpage == 0)
                {
                    saleFilterParam.Currentpage = 1;
                }

                int maxRowsPerPage = 12;

                // Check and adjust StatusIds based on specified conditions
                if (saleFilterParam.StatusIds.Contains(5))
                {
                    if (!saleFilterParam.StatusIds.Contains(8))
                    {
                        saleFilterParam.StatusIds.Add(8); // Add 8 if it's not already in the list
                    }
                }
                else
                {
                    saleFilterParam.StatusIds.Remove(8); // Remove 8 if it's in the list but 5 is not
                }
                if (saleFilterParam.Month == 0)
                    saleFilterParam.Month = null;
                

                var allSales = await _saleRepository.GetAllSalesByStatus(saleFilterParam.StatusIds, saleFilterParam.Year, saleFilterParam.Month);
                result.Success = true;
                result.Message = "Fumo Fumo";

                //just check none of them is not out of pending time
                var salesToProcess = allSales.Where(sale => sale.StatusId == 1).ToList();
                foreach (var sale in salesToProcess)
                {
                    if (await CheckAndCancelOrderIfExpired(sale))
                    {
                        sale.StatusId = 8;
                        sale.StatusName = "ไม่ชำระตามกำหนด";
                    }
                }


                if (!string.IsNullOrEmpty(saleFilterParam.OrderBy))
                {
                    allSales = SortSales(allSales, saleFilterParam.OrderBy, saleFilterParam.SortDirection);
                }
                else
                {
                    allSales = allSales.OrderByDescending(x => x.SaleId).ToList();
                }

                // Calculate pagination
                int totalCount = allSales.Count;
                int totalPages = (int)Math.Ceiling((decimal)totalCount / (decimal)maxRowsPerPage);
                int startPage = saleFilterParam.Currentpage - 5;
                int endPage = saleFilterParam.Currentpage + 4;

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

                result.Data = new SaleFilterResultDto();

                result.Data.Carts = allSales
                    .Skip((saleFilterParam.Currentpage - 1) * maxRowsPerPage)
                    .Take(maxRowsPerPage)
                .ToList();

                result.Data.PageCount = totalCount;
                result.Data.CurrentPage = saleFilterParam.Currentpage;
                result.Data.PageSize = maxRowsPerPage;
                result.Data.TotalPages = totalPages;
                result.Data.StartPage = startPage;
                result.Data.EndPage = endPage;

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return BadRequest(new ServiceResponse<SaleFilterResultDto>
                {
                    Success = false,
                    Message = $"Error occurred while processing the request: {ex.Message}",
                });
            }
        }

        [HttpGet]
        [Route("[action]"), Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<ServiceResponse<List<CartsDto>>>> GetAllSalesForReport()
        {
            var user = await _userRepository.GetUserById(_authService.GetUserId());

            var result = new ServiceResponse<List<CartsDto>>
            {
                Data = await _saleRepository.GetAllSalesForReport(),
                Success = true
            };

            return result;
        }

        [HttpPost]
        [Route("[action]"), Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<ServiceResponse<bool>>> ConfirmByAdmin([FromBody] CartsDto cartsDto)
        {
            var user = await _userRepository.GetUserById(_authService.GetUserId());

            var result = new ServiceResponse<bool>
            {
                Data = await _saleRepository.ConfirmByAdmin(cartsDto),
                Success = true
            };

            return result;
        }

        [HttpPost]
        [Route("[action]"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<bool>>> AnnotateByAdmin([FromBody] CartsDto cartsDto)
        {
            var user = await _userRepository.GetUserById(_authService.GetUserId());

            var result = new ServiceResponse<bool> { Success = false };

            var saleDb = await _saleRepository.GetSaleBySaleId(cartsDto.SaleId);

            if (saleDb.StatusId == 4 || saleDb.StatusId == 5 || saleDb.StatusId == 6)
                return BadRequest(result);

            if (await UpdateStock(cartsDto))
            {
                result.Data = await _saleRepository.AnnotateByAdmin(cartsDto);
                result.Success = true;
            }

            return result;
        }

        [HttpPost]
        [Route("[action]"), Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<ServiceResponse<bool>>> AlreadySentByAdmin(CartsDto cartsDto)
        {
            var user = await _userRepository.GetUserById(_authService.GetUserId());

            if (cartsDto.ParcelNumber == null || cartsDto.ParcelNumber.IsNullOrEmpty())
            {
                return new ServiceResponse<bool> { Data = false, Success = false };
            }

            cartsDto.ParcelNumber = cartsDto.ParcelNumber;
            cartsDto.ParcelTypeNo = cartsDto.ParcelTypeNo;

            var result = new ServiceResponse<bool>
            {
                Data = await _saleRepository.AlreadySentByAdmin(cartsDto),
                Success = true
            };

            return result;
        }

        [HttpPost]
        [Route("[action]"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<bool>>> CancelByAdmin([FromBody] CartsDto cartsDto)
        {
            var user = await _userRepository.GetUserById(_authService.GetUserId());

            var result = new ServiceResponse<bool> { Success = false };

            var saleDb = await _saleRepository.GetSaleBySaleId(cartsDto.SaleId);

            if (saleDb.StatusId == 4 || saleDb.StatusId == 5 || saleDb.StatusId == 6)
                return BadRequest(result);

            if (await UpdateStock(cartsDto))
            {
                result.Data = await _saleRepository.CancelByAdmin(cartsDto);
                result.Success = true;
            }

            return result;
        }

        [HttpGet]
        [Route("[action]/{SaleId}"), Authorize]
        public async Task<ActionResult<ServiceResponse<CartsDto>>> GetCartsById(int SaleId)
        {
            var user = await _userRepository.GetUserById(_authService.GetUserId());

            try
            {
                var Carts = await _saleRepository.GetCartsById(SaleId);
                if (Carts != null)
                {
                    if (Carts.UserId == user.UserId || user.RoleId == 6 || user.RoleId == 7)
                    {
                        var result = new ServiceResponse<CartsDto>
                        {
                            Data = Carts,
                            Success = true
                        };
                        return Ok(result);
                    }
                }
                var failResult = new ServiceResponse<CartsDto>
                {
                    Data = new CartsDto(),
                    Success = false,
                    Message = "เหลี่ยม"
                };
                return Ok(failResult);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<string> CreateCartNumber()
        {
            var currentDate = DateTime.Now;
            string newOrder = await GenerateNumber();
            return newOrder;
        }

        private async Task<string> GenerateNumber()
        {
            var strPrefix = "NO";

            var currentDate = DateTime.Now;
            var currentYear = currentDate.Year;
            var currentMonth = currentDate.Month;

            var LatestGenerateNumber = await _saleRepository.FindLatestGenerateNumber(currentYear, currentMonth);

            if (LatestGenerateNumber == null)
            {
                LatestGenerateNumber = new GenerateNumber()
                {
                    Year = currentYear,
                    Month = currentMonth,
                    Sequence = 1
                };
                await _saleRepository.NewNumber(LatestGenerateNumber);
            }
            else
            {
                LatestGenerateNumber.Sequence++;
                await _saleRepository.UpdateNumber(LatestGenerateNumber);
            }

            var saleCode = $"{strPrefix}-{currentYear}-{currentMonth.ToString("D2")}-{LatestGenerateNumber.Sequence}";
            return saleCode;
        }

        private string UploadImage(CartsDto obj)
        {
            string uniqueFileName = string.Empty;
            if (obj.ImgPath != null)
            {
                try
                {
                    string base64ImageDataWithoutPrefix = obj.Base64ImageData!.Replace("data:image/png;base64,", "");

                    byte[] fileBytes = Convert.FromBase64String(base64ImageDataWithoutPrefix);

                    string rootPath = _webHostEnvironment.ContentRootPath;
                    string uploadFolder = Path.Combine(rootPath, "ZStores/Images/Payment");

                    string fileExtension = GetFileExtensionFromBase64(obj.Base64ImageData!);

                    uniqueFileName = Guid.NewGuid().ToString() + fileExtension;

                    string filePath = Path.Combine(uploadFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        fileStream.Write(fileBytes, 0, fileBytes.Length);
                    }
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

        private async Task<bool> CheckStockAvailability(List<ItemQuantityDto> cartItems)
        {
            foreach (ItemQuantityDto itemQuantity in cartItems)
            {
                int productId = itemQuantity.ProductId;
                int quantity = itemQuantity.Quantity;

                var productStock = await _stockRepository.GetStockbyProductId(productId);

                if (productStock == null)
                {
                    return false;
                }

                if (quantity > productStock.NumStock || quantity == 0)
                {
                    return false;
                }

            }

            return true;
        }

        private async Task<bool> UpdateStock(CartsDto SaleItems)
        {
            bool success = true;
            foreach (CartDetailsDto saleItem in SaleItems.CartDetails)
            {
                var stock = await _stockRepository.GetStockbyProductId(saleItem.ProductId);

                if (stock != null)
                {
                    stock.NumStock += saleItem.Quantity;
                    await _stockRepository.UpdateDecreaseStock(stock);
                }
                else
                {
                    success = false;
                }
            }
            return success;
        }

        private List<CartsDto> SortSales(List<CartsDto> sales, string orderBy, string sortDirection)
        {
            if(sortDirection != "asc" || sortDirection != "desc")
                sortDirection = "desc";
            switch (orderBy.ToLower())
            {
                case "saledate":
                    if (sortDirection.ToLower() == "asc")
                    {
                        return sales.OrderBy(s => s.OrderDate).ToList();
                    }
                    else if (sortDirection.ToLower() == "desc")
                    {
                        return sales.OrderByDescending(s => s.OrderDate).ToList();
                    }
                    break;
                case "status":
                    if (sortDirection.ToLower() == "asc")
                    {
                        return sales.OrderBy(s => s.StatusName).ToList();
                    }
                    else if (sortDirection.ToLower() == "desc")
                    {
                        return sales.OrderByDescending(s => s.StatusName).ToList();
                    }
                    break;
                case "user":
                    if (sortDirection.ToLower() == "asc")
                    {
                        return sales.OrderBy(s => s.UserId).ToList();
                    }
                    else if (sortDirection.ToLower() == "desc")
                    {
                        return sales.OrderByDescending(s => s.UserId).ToList();
                    }
                    break;
                    // Add more cases for other properties to sort by
            }

            return sales;
        }

        private async Task<bool> CheckAndCancelOrderIfExpired(CartsDto sale)
        {
            if (DateTime.Now - sale.OrderDate > TimeSpan.FromHours(24))
            {
                if(await _saleRepository.CheckAndCancelOrderIfExpired(sale))
                {
                    await UpdateStock(sale);
                }
                return true;
            }
            return false;
        }

        //private async Task<int> CheckStockAvailability(ItemQuantityDto cartItem)
        //{
        //    int productId = cartItem.ProductId;
        //    int quantity = cartItem.Quantity;

        //    var productStock = await _stockRepository.GetStockbyProductId(productId);

        //    if (productStock == null)
        //    {
        //        return false;
        //    }

        //    if (quantity > productStock.NumStock)
        //    {
        //        return false;
        //    }

        //    return true;
        //}
    }

}