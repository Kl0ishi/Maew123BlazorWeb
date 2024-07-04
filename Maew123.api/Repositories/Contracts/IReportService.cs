namespace Maew123.Api.Repositories.Contracts
{
    public interface IReportService
    {
        Task<byte[]> GenerateYearlyReportCsvAsync();
        Task<byte[]> GenerateDailyReportCsvAsync();
        Task<byte[]> GenerateWeeklyReportCsvAsync();
        Task<byte[]> GenerateMonthlyReportCsvAsync();
        Task<byte[]> GenerateStockReportCsvAsync();
        Task<byte[]> GenerateSalePdf(int saleId);
        Task<byte[]> GenerateDeliveryPdf(int saleId);

        Task<byte[]> GenerateDailyReportPdfAsync();
        Task<byte[]> GenerateWeeklyReportPdfAsync();
        Task<byte[]> GenerateMonthlyReportPdfAsync();
        Task<byte[]> GenerateYearlyReportPdfAsync();

        Task<byte[]> GenerateStockReportPdfAsync();

        Task<byte[]> GenerateReceiptPdf(int saleId);
    }
}
