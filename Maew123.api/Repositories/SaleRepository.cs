using Maew123.Api.Models;
using Maew123.Models.Models;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Maew123.Api.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly ItshopMaew123Context _dbContext;

        public SaleRepository(ItshopMaew123Context dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<CartDetailsDto> GetCartDetails(int productId, int quantity)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var saleDetails = await (from a in _dbContext.Products
                                     join b in _dbContext.Promotions on a.PromotionId equals b.PromotionId into promotions
                                     from b in promotions.DefaultIfEmpty()  // Use left join
                                     where a.ProductId == productId
                                     select new CartDetailsDto
                                     {
                                         ProductId = productId,
                                         ProductName = a.ProductName,
                                         ImageUrl = a.ImgPath,
                                         Price = a.Price,
                                         Quantity = quantity,
                                         Description = a.Description,
                                         //TotalPrice = a.Price * quantity,
                                         PromotionId = b != null && b.EndDate >= today ? a.PromotionId : null,
                                         PromotionName = b != null ? b.PromotionName : "No Promotion",
                                         PromotionType = b.PromotionType,
                                         thresholdAmount = b.thresholdAmount,
                                         orderAmountDiscount = b.orderAmountDiscount,
                                         Discount = b != null && b.EndDate >= today ? b.DiscountPer??0 : 0
                                     }).FirstOrDefaultAsync();
            return saleDetails!;
        }

        //สำหรับเผื่อมี SaleItem
        //public async Task<CartDetailsDto> GetCartDetails(int productId, int quantity)
        //{
        //    var saleDetails = await (from a in _dbContext.SaleItems
        //                             join b in _dbContext.Products on a.ProductId equals b.ProductId
        //                             join c in _dbContext.Promotions on a.PromotionId equals c.PromotionId into promotions
        //                             from promotion in promotions.DefaultIfEmpty()  // Use left join
        //                             where b.ProductId == productId
        //                             select new CartDetailsDto
        //                             {
        //                                 ProductId = productId,
        //                                 ProductName = b.ProductName,
        //                                 ImageUrl = b.ImgPath,
        //                                 Price = b.Price,
        //                                 Quantity = quantity,
        //                                 TotalPrice = b.Price * quantity,
        //                                 promotionId = b.PromotionId,
        //                                 promotionName = promotion != null ? promotion.PromotionName : "No Promotion",
        //                             }).FirstOrDefaultAsync();
        //    return saleDetails!;
        //}

        public async Task<int> CreateSaleId(Sale sale)
        {
            _dbContext.Sales.Add(sale);
            await _dbContext.SaveChangesAsync();

            return sale.SaleId;
        }

        public async Task SaveSaleItems(SaleItem saleItem)
        {
            _dbContext.SaleItems.Add(saleItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateSale(Sale sale)
        {
            _dbContext.Sales.Update(sale);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<CartsDto>> GetAllSaleById(int userId)
        {
            var sales = await (from a in _dbContext.Sales
                               join b in _dbContext.Users on a.UserId equals b.UserId
                               join c in _dbContext.Statuses on a.StatusId equals c.StatusId
                               where a.UserId == userId
                               select new CartsDto
                               {
                                   SaleId = a.SaleId,
                                   SaleCode = a.SaleCode,
                                   UserId = a.UserId,
                                   SaleNum = a.SaleNum,
                                   SaleDiscount = a.SaleDiscount ?? 0,
                                   SaleTotal = a.SaleTotal,
                                   OrderDate = a.OrderDate,
                                   StatusId = a.StatusId,
                                   StatusName = c.StatusName,
                                   Annotation = a.Annotation,
                                   ParcelNumber = a.ParcelNumber,
                                   ParcelTypeNo = a.ParcelTypeNo,
                                   CartDetails = (from si in _dbContext.SaleItems
                                                  where si.SaleId == a.SaleId
                                                  join p in _dbContext.Products on si.ProductId equals p.ProductId
                                                  join pr in _dbContext.Promotions on si.PromotionId equals pr.PromotionId into promotions
                                                  from pr in promotions.DefaultIfEmpty()
                                                  select new CartDetailsDto
                                                  {
                                                      ProductId = p.ProductId,
                                                      ProductName = p.ProductName,
                                                      ImageUrl = p.ImgPath,
                                                      //Price = p.Price,  หมายเหตุ: แสดงราคารวมหลังลดราคาแล้ว พอ!!!
                                                      Quantity = si.Qty ?? 0,
                                                      TotalPrice = si.TotalPrice,
                                                      PromotionId = pr.PromotionId,
                                                      PromotionName = pr != null ? pr.PromotionName : "No Promotion",
                                                      PromotionType = pr.PromotionType,
                                                      Discount = pr.DiscountPer ?? 0
                                                  }).ToList()
                               }).ToListAsync();

            return sales;
        }

        public async Task<int> GetAnnotatedCount(int userId)
        {
            var annotatedCount = await _dbContext.Sales
                .Where(a => a.UserId == userId && a.StatusId == 7)
                .CountAsync();

            return annotatedCount;
        }

        public async Task<List<CartsDto>> GetSalesHistory(int userId)
        {
            var sales = await (from a in _dbContext.Sales
                               join b in _dbContext.Users on a.UserId equals b.UserId
                               join c in _dbContext.Statuses on a.StatusId equals c.StatusId
                               where a.UserId == userId && a.StatusId == 4 || a.StatusId == 5 || a.StatusId == 6 || a.StatusId == 8
                               select new CartsDto
                               {
                                   SaleId = a.SaleId,
                                   SaleCode = a.SaleCode,
                                   UserId = a.UserId,
                                   SaleNum = a.SaleNum,
                                   SaleDiscount = a.SaleDiscount ?? 0,
                                   SaleTotal = a.SaleTotal,
                                   OrderDate = a.OrderDate,
                                   StatusId = a.StatusId,
                                   StatusName = c.StatusName,
                                   Annotation = a.Annotation,
                                   ParcelNumber = a.ParcelNumber,
                                   ParcelTypeNo = a.ParcelTypeNo,
                                   CartDetails = (from si in _dbContext.SaleItems
                                                  where si.SaleId == a.SaleId
                                                  join p in _dbContext.Products on si.ProductId equals p.ProductId
                                                  join pr in _dbContext.Promotions on si.PromotionId equals pr.PromotionId into promotions
                                                  from pr in promotions.DefaultIfEmpty()
                                                  select new CartDetailsDto
                                                  {
                                                      ProductId = p.ProductId,
                                                      ProductName = p.ProductName,
                                                      ImageUrl = p.ImgPath,
                                                      //Price = p.Price,  หมายเหตุ: แสดงราคารวมหลังลดราคาแล้ว พอ!!!
                                                      Quantity = si.Qty ?? 0,
                                                      TotalPrice = si.TotalPrice,
                                                      PromotionId = pr.PromotionId,
                                                      PromotionName = pr != null ? pr.PromotionName : "No Promotion",
                                                      PromotionType = pr.PromotionType,
                                                      Discount = pr.DiscountPer ?? 0
                                                  }).ToList()
                               }).ToListAsync();

            return sales;
        }

        public async Task<List<CartsDto>> GetPaymentRequest(int userId)
        {
            var sales = await (from a in _dbContext.Sales
                               join b in _dbContext.Users on a.UserId equals b.UserId
                               join c in _dbContext.Statuses on a.StatusId equals c.StatusId
                               where a.UserId == userId && a.StatusId == 1
                               select new CartsDto
                               {
                                   SaleId = a.SaleId,
                                   SaleCode = a.SaleCode,
                                   UserId = a.UserId,
                                   SaleNum = a.SaleNum,
                                   SaleDiscount = a.SaleDiscount ?? 0,
                                   SaleTotal = a.SaleTotal,
                                   OrderDate = a.OrderDate,
                                   StatusId = a.StatusId,
                                   StatusName = c.StatusName,
                                   Annotation = a.Annotation,
                                   ParcelNumber = a.ParcelNumber,
                                   ParcelTypeNo = a.ParcelTypeNo,
                                   CartDetails = (from si in _dbContext.SaleItems
                                                  where si.SaleId == a.SaleId
                                                  join p in _dbContext.Products on si.ProductId equals p.ProductId
                                                  join pr in _dbContext.Promotions on si.PromotionId equals pr.PromotionId into promotions
                                                  from pr in promotions.DefaultIfEmpty()
                                                  select new CartDetailsDto
                                                  {
                                                      ProductId = p.ProductId,
                                                      ProductName = p.ProductName,
                                                      ImageUrl = p.ImgPath,
                                                      //Price = p.Price,  หมายเหตุ: แสดงราคารวมหลังลดราคาแล้ว พอ!!!
                                                      Quantity = si.Qty ?? 0,
                                                      TotalPrice = si.TotalPrice,
                                                      PromotionId = pr.PromotionId,
                                                      PromotionName = pr != null ? pr.PromotionName : "No Promotion",
                                                      PromotionType = pr.PromotionType,
                                                      Discount = pr.DiscountPer ?? 0
                                                  }).ToList()
                               }).ToListAsync();

            return sales;
        }

        public async Task<List<CartsDto>> GetAlreadyPayment(int userId)
        {
            var sales = await (from a in _dbContext.Sales
                               join b in _dbContext.Users on a.UserId equals b.UserId
                               join c in _dbContext.Statuses on a.StatusId equals c.StatusId
                               where a.UserId == userId && a.StatusId == 2
                               select new CartsDto
                               {
                                   SaleId = a.SaleId,
                                   SaleCode = a.SaleCode,
                                   UserId = a.UserId,
                                   SaleNum = a.SaleNum,
                                   SaleDiscount = a.SaleDiscount ?? 0,
                                   SaleTotal = a.SaleTotal,
                                   OrderDate = a.OrderDate,
                                   StatusId = a.StatusId,
                                   StatusName = c.StatusName,
                                   Annotation = a.Annotation,
                                   ParcelNumber = a.ParcelNumber,
                                   ParcelTypeNo = a.ParcelTypeNo,
                                   CartDetails = (from si in _dbContext.SaleItems
                                                  where si.SaleId == a.SaleId
                                                  join p in _dbContext.Products on si.ProductId equals p.ProductId
                                                  join pr in _dbContext.Promotions on si.PromotionId equals pr.PromotionId into promotions
                                                  from pr in promotions.DefaultIfEmpty()
                                                  select new CartDetailsDto
                                                  {
                                                      ProductId = p.ProductId,
                                                      ProductName = p.ProductName,
                                                      ImageUrl = p.ImgPath,
                                                      //Price = p.Price,  หมายเหตุ: แสดงราคารวมหลังลดราคาแล้ว พอ!!!
                                                      Quantity = si.Qty ?? 0,
                                                      TotalPrice = si.TotalPrice,
                                                      PromotionId = pr.PromotionId,
                                                      PromotionName = pr != null ? pr.PromotionName : "No Promotion",
                                                      PromotionType = pr.PromotionType,
                                                      Discount = pr.DiscountPer ?? 0
                                                  }).ToList()
                               }).ToListAsync();

            return sales;
        }

        public async Task<List<CartsDto>> GetAnnotatedOrder(int userId)
        {
            var sales = await (from a in _dbContext.Sales
                               join b in _dbContext.Users on a.UserId equals b.UserId
                               join c in _dbContext.Statuses on a.StatusId equals c.StatusId
                               where a.UserId == userId && a.StatusId == 7
                               select new CartsDto
                               {
                                   SaleId = a.SaleId,
                                   SaleCode = a.SaleCode,
                                   UserId = a.UserId,
                                   SaleNum = a.SaleNum,
                                   SaleDiscount = a.SaleDiscount ?? 0,
                                   SaleTotal = a.SaleTotal,
                                   OrderDate = a.OrderDate,
                                   StatusId = a.StatusId,
                                   StatusName = c.StatusName,
                                   Annotation = a.Annotation,
                                   ParcelNumber = a.ParcelNumber,
                                   ParcelTypeNo = a.ParcelTypeNo,
                                   CartDetails = (from si in _dbContext.SaleItems
                                                  where si.SaleId == a.SaleId
                                                  join p in _dbContext.Products on si.ProductId equals p.ProductId
                                                  join pr in _dbContext.Promotions on si.PromotionId equals pr.PromotionId into promotions
                                                  from pr in promotions.DefaultIfEmpty()
                                                  select new CartDetailsDto
                                                  {
                                                      ProductId = p.ProductId,
                                                      ProductName = p.ProductName,
                                                      ImageUrl = p.ImgPath,
                                                      //Price = p.Price,  หมายเหตุ: แสดงราคารวมหลังลดราคาแล้ว พอ!!!
                                                      Quantity = si.Qty ?? 0,
                                                      TotalPrice = si.TotalPrice,
                                                      PromotionId = pr.PromotionId,
                                                      PromotionName = pr != null ? pr.PromotionName : "No Promotion",
                                                      PromotionType = pr.PromotionType,
                                                      Discount = pr.DiscountPer ?? 0
                                                  }).ToList()
                               }).ToListAsync();

            return sales;
        }

        public async Task<List<CartsDto>> GetWaitForSent(int userId)
        {
            var sales = await (from a in _dbContext.Sales
                               join b in _dbContext.Users on a.UserId equals b.UserId
                               join c in _dbContext.Statuses on a.StatusId equals c.StatusId
                               where a.UserId == userId && a.StatusId == 3
                               select new CartsDto
                               {
                                   SaleId = a.SaleId,
                                   SaleCode = a.SaleCode,
                                   UserId = a.UserId,
                                   SaleNum = a.SaleNum,
                                   SaleDiscount = a.SaleDiscount ?? 0,
                                   SaleTotal = a.SaleTotal,
                                   OrderDate = a.OrderDate,
                                   StatusId = a.StatusId,
                                   StatusName = c.StatusName,
                                   Annotation = a.Annotation,
                                   ParcelNumber = a.ParcelNumber,
                                   ParcelTypeNo = a.ParcelTypeNo,
                                   CartDetails = (from si in _dbContext.SaleItems
                                                  where si.SaleId == a.SaleId
                                                  join p in _dbContext.Products on si.ProductId equals p.ProductId
                                                  join pr in _dbContext.Promotions on si.PromotionId equals pr.PromotionId into promotions
                                                  from pr in promotions.DefaultIfEmpty()
                                                  select new CartDetailsDto
                                                  {
                                                      ProductId = p.ProductId,
                                                      ProductName = p.ProductName,
                                                      ImageUrl = p.ImgPath,
                                                      //Price = p.Price,  หมายเหตุ: แสดงราคารวมหลังลดราคาแล้ว พอ!!!
                                                      Quantity = si.Qty ?? 0,
                                                      TotalPrice = si.TotalPrice,
                                                      PromotionId = pr.PromotionId,
                                                      PromotionName = pr != null ? pr.PromotionName : "No Promotion",
                                                      PromotionType = pr.PromotionType,
                                                      Discount = pr.DiscountPer ?? 0
                                                  }).ToList()
                               }).ToListAsync();

            return sales;
        }

        public async Task<List<CartsDto>> GetAlreadySent(int userId)
        {
            var sales = await (from a in _dbContext.Sales
                               join b in _dbContext.Users on a.UserId equals b.UserId
                               join c in _dbContext.Statuses on a.StatusId equals c.StatusId
                               where a.UserId == userId && a.StatusId == 4
                               select new CartsDto
                               {
                                   SaleId = a.SaleId,
                                   SaleCode = a.SaleCode,
                                   UserId = a.UserId,
                                   SaleNum = a.SaleNum,
                                   SaleDiscount = a.SaleDiscount ?? 0,
                                   SaleTotal = a.SaleTotal,
                                   OrderDate = a.OrderDate,
                                   StatusId = a.StatusId,
                                   StatusName = c.StatusName,
                                   Annotation = a.Annotation,
                                   ParcelNumber = a.ParcelNumber,
                                   ParcelTypeNo = a.ParcelTypeNo,
                                   CartDetails = (from si in _dbContext.SaleItems
                                                  where si.SaleId == a.SaleId
                                                  join p in _dbContext.Products on si.ProductId equals p.ProductId
                                                  join pr in _dbContext.Promotions on si.PromotionId equals pr.PromotionId into promotions
                                                  from pr in promotions.DefaultIfEmpty()
                                                  select new CartDetailsDto
                                                  {
                                                      ProductId = p.ProductId,
                                                      ProductName = p.ProductName,
                                                      ImageUrl = p.ImgPath,
                                                      //Price = p.Price,  หมายเหตุ: แสดงราคารวมหลังลดราคาแล้ว พอ!!!
                                                      Quantity = si.Qty ?? 0,
                                                      TotalPrice = si.TotalPrice,
                                                      PromotionId = pr.PromotionId,
                                                      PromotionName = pr != null ? pr.PromotionName : "No Promotion",
                                                      PromotionType = pr.PromotionType,
                                                      Discount = pr.DiscountPer ?? 0
                                                  }).ToList()
                               }).ToListAsync();

            return sales;
        }

        public async Task<bool> Payment(CartsDto carts)
        {
            if (carts.ImgPath != null)
            {
                var dbSale = await _dbContext.Sales.FindAsync(carts.SaleId);
                dbSale.ImgPath = carts.ImgPath;
                dbSale.PayDate = DateTime.Now;
                dbSale.StatusId = 2;
                _dbContext.Sales.Update(dbSale);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> CancelOrder(CartsDto carts)
        {
            var dbSale = await _dbContext.Sales.FindAsync(carts.SaleId);
            if (dbSale.StatusId == 1)
            {
                dbSale.StatusId = 5;
                _dbContext.Update(dbSale);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        //public Task<CartDetailsDto> GetSaleDetails(int saleId)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<List<CartsDto>> GetAllSalesByStatus(List<int> statusIds, int? year, int? month)
        {
            var sales = await (from a in _dbContext.Sales
                               join b in _dbContext.Users on a.UserId equals b.UserId
                               join c in _dbContext.Statuses on a.StatusId equals c.StatusId
                               where statusIds.Contains(a.StatusId) &&
                                     (!year.HasValue || a.OrderDate.Year == year.Value) &&
                                     (!month.HasValue || a.OrderDate.Month == month.Value)
                               select new CartsDto
                               {
                                   SaleId = a.SaleId,
                                   SaleCode = a.SaleCode,
                                   UserId = a.UserId,
                                   UserName = b.Username,
                                   SaleNum = a.SaleNum,
                                   SaleDiscount = a.SaleDiscount ?? 0,
                                   SaleTotal = a.SaleTotal,
                                   OrderDate = a.OrderDate,
                                   StatusId = a.StatusId,
                                   StatusName = c.StatusName,
                                   Annotation = a.Annotation,
                                   AddressId = a.AddressSnapshotId,
                                   ImgPath = a.ImgPath,
                                   ParcelNumber = a.ParcelNumber,
                                   ParcelTypeNo = a.ParcelTypeNo,
                                   SentDate = a.SentDate,
                                   CartDetails = (from si in _dbContext.SaleItems
                                                  where si.SaleId == a.SaleId
                                                  join p in _dbContext.Products on si.ProductId equals p.ProductId
                                                  join pr in _dbContext.Promotions on si.PromotionId equals pr.PromotionId into promotions
                                                  from pr in promotions.DefaultIfEmpty()
                                                  select new CartDetailsDto
                                                  {
                                                      ProductId = p.ProductId,
                                                      ProductName = p.ProductName,
                                                      ImageUrl = p.ImgPath,
                                                      Quantity = si.Qty ?? 0,
                                                      TotalPrice = si.TotalPrice,
                                                      PromotionId = pr.PromotionId,
                                                      PromotionName = pr != null ? pr.PromotionName : "No Promotion",
                                                      PromotionType = pr.PromotionType,
                                                      Discount = pr.DiscountPer ?? 0
                                                  }).ToList()
                               }).ToListAsync();
            return sales;
        }

        public async Task<List<CartsDto>> GetAllSalesForReport()
        {
            var sales = await (from a in _dbContext.Sales
                               where a.StatusId == 3 || a.StatusId == 4
                               join b in _dbContext.Users on a.UserId equals b.UserId
                               join c in _dbContext.Statuses on a.StatusId equals c.StatusId
                               select new CartsDto
                               {
                                   SaleId = a.SaleId,
                                   SaleCode = a.SaleCode,
                                   UserId = a.UserId,
                                   UserName = b.Username,
                                   SaleNum = a.SaleNum,
                                   SaleDiscount = a.SaleDiscount ?? 0,
                                   SaleTotal = a.SaleTotal,
                                   OrderDate = a.OrderDate,
                                   StatusId = a.StatusId,
                                   StatusName = c.StatusName,
                                   Annotation = a.Annotation,
                                   ImgPath = a.ImgPath,
                                   ParcelNumber = a.ParcelNumber,
                                   ParcelTypeNo = a.ParcelTypeNo,
                                   SentDate = a.SentDate,
                                   CartDetails = (from si in _dbContext.SaleItems
                                                  where si.SaleId == a.SaleId
                                                  join p in _dbContext.Products on si.ProductId equals p.ProductId
                                                  join pr in _dbContext.Promotions on si.PromotionId equals pr.PromotionId into promotions
                                                  from pr in promotions.DefaultIfEmpty()
                                                  select new CartDetailsDto
                                                  {
                                                      ProductId = p.ProductId,
                                                      ProductName = p.ProductName,
                                                      ImageUrl = p.ImgPath,
                                                      Quantity = si.Qty ?? 0,
                                                      TotalPrice = si.TotalPrice,
                                                      PromotionId = pr.PromotionId,
                                                      PromotionName = pr != null ? pr.PromotionName : "No Promotion",
                                                      PromotionType = pr.PromotionType,
                                                      Discount = pr.DiscountPer ?? 0
                                                  }).ToList()
                               }).ToListAsync();
            return sales;
        }

        public async Task<bool> ConfirmByAdmin(CartsDto carts)
        {
            var dbSale = await _dbContext.Sales.FindAsync(carts.SaleId);

            dbSale.StatusId = 3;
            _dbContext.Update(dbSale);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AnnotateByAdmin(CartsDto carts)
        {
            var dbSale = await _dbContext.Sales.FindAsync(carts.SaleId);

            dbSale.StatusId = 7;
            dbSale.Annotation = carts.Annotation;
            _dbContext.Update(dbSale);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AlreadySentByAdmin(CartsDto carts)
        {
            var dbSale = await _dbContext.Sales.FindAsync(carts.SaleId);

            dbSale.ParcelNumber = carts.ParcelNumber;
            dbSale.ParcelTypeNo = carts.ParcelTypeNo;
            if(dbSale.SentDate == null)
                dbSale.SentDate = DateTime.Now;
            dbSale.StatusId = 4;
            _dbContext.Update(dbSale);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelByAdmin(CartsDto carts)
        {
            var dbSale = await _dbContext.Sales.FindAsync(carts.SaleId);

            dbSale.StatusId = 6;
            dbSale.Annotation = carts.Annotation;
            _dbContext.Update(dbSale);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<CartsDto> GetCartsById(int SaleId)
        {
            var sales = await (from a in _dbContext.Sales
                               join b in _dbContext.Users on a.UserId equals b.UserId
                               join c in _dbContext.Statuses on a.StatusId equals c.StatusId
                               where a.SaleId == SaleId
                               select new CartsDto
                               {
                                   SaleId = a.SaleId,
                                   SaleCode = a.SaleCode,
                                   UserId = a.UserId,
                                   FirstName = b.FirstName,
                                   LastName = b.LastName,
                                   UserTel = b.UserTel,
                                   SaleNum = a.SaleNum,
                                   SaleDiscount = a.SaleDiscount ?? 0,
                                   SaleTotal = a.SaleTotal,
                                   OrderDate = a.OrderDate,
                                   PayDate = a.PayDate,
                                   StatusId = a.StatusId,
                                   StatusName = c.StatusName,
                                   Annotation = a.Annotation,
                                   CartDetails = (from si in _dbContext.SaleItems
                                                  where si.SaleId == a.SaleId
                                                  join p in _dbContext.Products on si.ProductId equals p.ProductId
                                                  join pr in _dbContext.Promotions on si.PromotionId equals pr.PromotionId into promotions
                                                  from pr in promotions.DefaultIfEmpty()
                                                  select new CartDetailsDto
                                                  {
                                                      ProductId = p.ProductId,
                                                      ProductName = p.ProductName,
                                                      ImageUrl = p.ImgPath,
                                                      //Price = p.Price,  หมายเหตุ: แสดงราคารวมหลังลดราคาแล้ว พอ!!!
                                                      Quantity = si.Qty ?? 0,
                                                      TotalPrice = si.TotalPrice,
                                                      PromotionId = pr.PromotionId,
                                                      PromotionName = pr != null ? pr.PromotionName : "No Promotion",
                                                      PromotionType = pr.PromotionType,
                                                      Discount = pr.DiscountPer ?? 0
                                                  }).ToList()
                               }).FirstOrDefaultAsync();

            return sales;
        }


        public async Task<GenerateNumber> FindLatestGenerateNumber(int currentYear, int currentMonth)
        {
            var LatestGenerateNumber = await _dbContext.GenerateNumbers
                .FirstOrDefaultAsync(g => g.Year == currentYear && g.Month == currentMonth);
            return LatestGenerateNumber!;
        }

        public async Task<bool> NewNumber(GenerateNumber generateNumber)
        {
            _dbContext.GenerateNumbers.Add(generateNumber);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateNumber(GenerateNumber generateNumber)
        {
            _dbContext.GenerateNumbers.Update(generateNumber);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CheckAndCancelOrderIfExpired(CartsDto carts)
        {
            var dbSale = await _dbContext.Sales.FindAsync(carts.SaleId);
            if (dbSale.StatusId == 1)
            {
                dbSale.StatusId = 8;
                _dbContext.Update(dbSale);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }


        public async Task<Sale> GetSaleBySaleId(int saleId)
        {
            try
            {
                var sale = await _dbContext.Sales.FindAsync(saleId);
                return sale;
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public async Task<List<SaleItem>> GetSaleItemsBySaleId(int saleId)
        {
            return await _dbContext.SaleItems
                .Include(s => s.Product)
                .Where(s => s.SaleId == saleId)
                .ToListAsync();
        }

        public async Task<int> GetCount()
        {
            var MultipleTable = (from a in _dbContext.Sales
                                 join b in _dbContext.Statuses on a.StatusId equals b.StatusId
                                 select new CartsDto
                                 {
                                     SaleId = a.SaleId,
                                     SaleCode = a.SaleCode,
                                     UserId = a.UserId,
                                     SaleNum = a.SaleNum,
                                     SaleDiscount = a.SaleDiscount ?? 0,
                                     SaleTotal = a.SaleTotal,
                                     OrderDate = a.OrderDate,
                                     StatusId = a.StatusId,
                                 }
                                    ).Count();
            //await Task.Delay(100);
            return MultipleTable;
        }

        public async Task<List<SaleItem>> GetAllSaleItemForMostSale()
        {
            return await _dbContext.SaleItems.ToListAsync();
        }

    }
}
