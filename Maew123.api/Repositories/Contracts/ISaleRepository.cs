namespace Maew123.Api.Repositories.Contracts
{
    public interface ISaleRepository
    {
        Task<CartDetailsDto> GetCartDetails(int productId, int quantity);
        //ย้ายไปฝั่งหน้าบ้าน
        //Task<CartDto> AddSaleItem(CartDto cartsDto);
        //Task<CartDto> RemoveSaleItem(CartDto cartsDto);

        Task<int> CreateSaleId(Sale sale);
        Task SaveSaleItems(SaleItem saleItem);
        Task UpdateSale(Sale sale);

        //Client and Admin
        Task<int> GetAnnotatedCount(int userId);
        Task<List<CartsDto>> GetAllSaleById(int Userid);
        Task<List<CartsDto>> GetSalesHistory(int Userid);
        Task<List<CartsDto>> GetPaymentRequest(int Userid);
        Task<List<CartsDto>> GetWaitForSent(int userId);
        Task<List<CartsDto>> GetAlreadyPayment(int Userid);
        Task<List<CartsDto>> GetAnnotatedOrder(int userId);
        Task<List<CartsDto>> GetAlreadySent(int Userid);
        Task<bool> Payment(CartsDto carts);
        Task<bool> CancelOrder(CartsDto carts);

        //Task<CartDetailsDto> GetSaleDetails(int SaleId);

        //Admin Part
        Task<List<CartsDto>> GetAllSalesByStatus(List<int> StatusIds, int? year, int? month);
        Task<List<CartsDto>> GetAllSalesForReport();
        Task<bool> ConfirmByAdmin(CartsDto carts);
        Task<bool> AnnotateByAdmin(CartsDto carts);
        Task<bool> AlreadySentByAdmin(CartsDto carts);
        Task<bool> CancelByAdmin(CartsDto carts);
        Task<CartsDto> GetCartsById(int SaleId);

        Task<GenerateNumber> FindLatestGenerateNumber(int year, int month);
        Task<bool> NewNumber(GenerateNumber generateNumber);
        Task<bool> UpdateNumber(GenerateNumber generateNumber);
        Task<bool> CheckAndCancelOrderIfExpired(CartsDto carts);

        Task<Sale> GetSaleBySaleId(int saleId);
        Task<List<SaleItem>> GetSaleItemsBySaleId(int saleId);

        Task<int> GetCount();
        Task<List<SaleItem>> GetAllSaleItemForMostSale();
    }
}
