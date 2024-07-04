using Maew123.Models;

namespace Maew123.Web.Services.Contracts
{
    public interface IOrderService
    {
        event Action SalesChanged;
        List<CartsDto> Sale { get; set; }
        //SaleFilterResultDto AdminSale { get; set; }

        Task<ServiceResponse<int>> Checkout(CartDto cart);

        Task GetSaleHistory();
        Task GetPaymentRequest();
        Task<bool> AnnotateByAdmin(CartsDto cart);
        Task<bool> Payment(CartsDto cart);
        Task<bool> CancelOrder(CartsDto cart);
        Task GetAlreadyPayment();
        Task GetWaitForSent();
        Task GetAlreadySent();
        Task GetAnnotatedOrder();
        Task<int> GetAnnotatedCount();
        Task GetAllSaleForUser();

        //AdminParts
        Task<SaleFilterResultDto> GetAllSalesByStatus(SaleFilterParam saleFilterParam);
        Task<List<CartsDto>> GetAllSaleForReport();
        Task<bool> ConfirmByAdmin(CartsDto cartsDto);
        Task<bool> AlreadySentByAdmin(CartsDto cartsDto);
        Task<bool> CancelByAdmin(CartsDto cartsDto);

        Task<CartsDto> GetCartsBySaleId(int saleId);
    }
}
