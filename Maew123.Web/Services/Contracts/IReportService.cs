namespace Maew123.Web.Services.Contracts
{
    public interface IReportService
    {
        Task GetYearlyReportCsv();
        Task GetDailyReportCsv();
        Task GetWeeklyReportCsv();
        Task GetMonthlyReportCsv();
        Task GetStockReportCsv();
        Task SaleReportPdf(int saleId);
        Task DeliveryPdf(int saleId, string? saleCode);
        Task SaleReceiptPdf(int saleId);

        Task GetYearlyReportPdf();
        Task GetMonthlyReportPdf();
        Task GetWeeklyReportPdf();
        Task GetDailyReportPdf();

        Task GetStockReportPdf();
    }
}
