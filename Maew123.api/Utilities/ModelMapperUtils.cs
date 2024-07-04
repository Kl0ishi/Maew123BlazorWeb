using Maew123.Models.Models;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Maew123.Api.Utilities
{
    public static class ModelMapperUtils
    {
        public static ProductDto MapToProductDto(this Product product)
        {
            var productDto = new ProductDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName ?? string.Empty,
                ProductStatus = product.ProductStatus ?? string.Empty,
                ProductCatagoryId = product.ProductCatagoryId,
                ProductTypeId = product.ProductTypeId,
                PromotionId = product.PromotionId,
                Price = product.Price,
                Condition = product.Condition,
                Description = product.Description,
                InsertBy = product.InsertBy,
                InsertDate = product.InsertDate,
                UpdateBy = product.UpdateBy,
                UpdateDate = product.UpdateDate,
                ImgPath = product.ImgPath,
                ProductCatagory = product.ProductCatagory,
                ProductType = product.ProductType,
                Promotion = product.Promotion,
                Featured = product.Featured.GetValueOrDefault(),
                Visible = product.Visible.GetValueOrDefault(),
                Deleted = product.Deleted.GetValueOrDefault(),
                //Editing = false,
                //IsNew = false
            };

            AssignStockInfo(product.ProductStocks, productDto);
            return productDto;

        }

        private static void AssignStockInfo(ICollection<ProductStock> productStocks, ProductDto productDto)
        {
            if (productStocks != null && productStocks.Count > 0)
            {
                var firstStock = productStocks.First();
                productDto.stockId = firstStock.ProductStockId;
                productDto.stockNum = firstStock.NumStock??0;
            }
            else
            {
                productDto.stockId = 0; // or whatever default value you want to assign
                productDto.stockNum = 0; 
            }
        }

        public static List<ProductDto> MapToProductDto(this List<Product> products)
        {
            return products.Select(product => product.MapToProductDto()).ToList();
        }

        //ProductDto to Product
        public static Product ToProduct(this ProductDto productDto)
        {
            return new Product
            {
                ProductId = productDto.ProductId,
                ProductName = productDto.ProductName ?? string.Empty,
                ProductStatus = productDto.ProductStatus ?? string.Empty,
                ProductCatagoryId = productDto.ProductCatagoryId,
                ProductTypeId = productDto.ProductTypeId,
                PromotionId = productDto.PromotionId,
                Price = productDto.Price,
                Condition = productDto.Condition,
                Description = productDto.Description,
                InsertBy = productDto.InsertBy,
                InsertDate = productDto.InsertDate,
                UpdateBy = productDto.UpdateBy,
                UpdateDate = productDto.UpdateDate,
                ImgPath = productDto.ImgPath,
                //ProductCatagory = productDto.ProductCatagory,
                //Promotion = productDto.Promotion,
                Featured = productDto.Featured,
                Visible = productDto.Visible,
                Deleted = productDto.Deleted,
            };
        }

        //UserInfo To User
        //public static User ToUser(this UserInfoDto userDto)
        //{
        //    return new User
        //    {
        //        UserId = userDto.UserId,
        //        Username = userDto.Username,
        //        FirstName = userDto.FirstName,
        //        LastName = userDto.LastName,
        //        Email = userDto.Email,
        //        UserAddress = userDto.UserAddress, //อาจไม่ใช้
        //        UserTel = userDto.UserTel,
        //        Gender = userDto.Gender,
        //        InsertDate = userDto.InsertDate,
        //        InsertBy = userDto.InsertBy,
        //        UpdateDate = DateTime.UtcNow,
        //        UpdateBy = "System",
        //        RoleId = userDto.RoleId
        //    };
        //}

        //เอาข้อมูลจากDtoยัดลงUser   ใช้กับPost
        public static User GetFromDto(this User user, UserInfoDto userInfoDto)
        {
            user.Username = userInfoDto.Username;
            user.FirstName = userInfoDto.FirstName;
            user.LastName = userInfoDto.LastName;
            user.Email = userInfoDto.Email;
            user.UserAddress = userInfoDto.UserAddress; //อาจไม่ใช้
            user.UserTel = userInfoDto.UserTel;
            user.Gender = userInfoDto.Gender;
            user.UpdateDate = DateTime.UtcNow;
            user.UpdateBy = "System";
            user.RoleId = userInfoDto.RoleId;
            return user;
        }

        //ใช้กับ Get
        public static UserInfoDto ToUserDto(this User user)
        {
            return new UserInfoDto
            {
                UserId = user.UserId,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserAddress = user.UserAddress, //อาจไม่ใช้
                UserTel = user.UserTel,
                Gender = user.Gender,
                InsertDate = user.InsertDate,
                InsertBy = user.InsertBy,
                UpdateDate = user.UpdateDate,
                UpdateBy = user.UpdateBy,
                RoleId = user.RoleId
            };
        }


        public static Promotion GetFromDto(this Promotion promotion, PromotionDto dto)
        {
            promotion.PromotionType = dto.PromotionType;
            promotion.PromotionName = dto.PromotionName;
            promotion.DiscountPer = dto.DiscountPer;
            promotion.StartDate = dto.StartDate;
            promotion.EndDate = dto.EndDate;
            promotion.UpdateDate = DateTime.Now;
            return promotion;
        }

        public static PromotionDto ToPromotionDto(this Promotion promotion)
        {
            return new PromotionDto
            {
                PromotionId = promotion.PromotionId,
                PromotionType = promotion.PromotionType,
                PromotionName = promotion.PromotionName,
                DiscountPer = promotion.DiscountPer,
                StartDate = promotion.StartDate,
                EndDate = promotion.EndDate,
                expiredInDay = (promotion.EndDate.DayNumber - (DateOnly.FromDateTime(DateTime.Now)).DayNumber)+1
            };
        }

        public static SaleReport ConvertFromCartsDto(CartsDto cartsDto)
        {
            return new SaleReport
            {
                SaleCode = cartsDto.SaleCode,
                SaleNum = cartsDto.SaleNum,
                SaleDiscount = cartsDto.SaleDiscount,
                SaleTotal = cartsDto.SaleTotal,
                OrderDate = cartsDto.OrderDate,
                StatusName = cartsDto.StatusName,
                ParcelNumber = cartsDto.ParcelNumber,
                SentDate = cartsDto.SentDate
            };
        }

        public static StockReport ConvertFromNewProductDto(NewProductDto productDto)
        {
            return new StockReport
            {
                ProductName = productDto.ProductName,
                ProductStatus = productDto.ProductStatus,
                ProductCatagoryName = productDto.ProductCatagoryName,
                ProductTypeName = productDto.ProductTypeName,
                numStock = productDto.numStock,
                PromotionName = productDto.PromotionName,
                Discount = productDto.Discount,
                Price = productDto.Price,
                Condition = productDto.Condition,
                //Description = productDto.Description,
                InsertBy = productDto.InsertBy,
                InsertDate = productDto.InsertDate,
                UpdateBy = productDto.UpdateBy,
                UpdateDate = productDto.UpdateDate,
            };
        }
    }
}
