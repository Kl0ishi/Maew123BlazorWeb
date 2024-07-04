using Maew123.Models.Models;
using Maew123.Web.Services.Contracts;
using Microsoft.JSInterop;
using System.Net.Http;
using static System.Net.WebRequestMethods;

namespace Maew123.Web.Services
{
    public class ReportService : IReportService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _jSRuntime;

        public ReportService(HttpClient http, IJSRuntime JSRuntime)
        {
            this._http = http;
            _jSRuntime = JSRuntime;
        }

        public async Task GetYearlyReportCsv()
        {
            var response = await _http.GetAsync("api/Report/GetYearlyReportCsv");
            await DownloadReport(response, "yearly_report.csv");
        }

        public async Task GetDailyReportCsv()
        {
            var response = await _http.GetAsync("api/Report/daily-report");
            await DownloadReport(response, "daily_report.csv");
        }

        public async Task GetWeeklyReportCsv()
        {
            var response = await _http.GetAsync("api/Report/weekly-report");
            await DownloadReport(response, "weekly_report.csv");
        }

        public async Task GetMonthlyReportCsv()
        {
            var response = await _http.GetAsync("api/Report/monthly-report");
            await DownloadReport(response, "monthly_report.csv");
        }

        public async Task GetStockReportCsv()
        {
            var response = await _http.GetAsync("api/Report/stock-report");
            await DownloadReport(response, "stock_report.csv");
        }

        public async Task SaleReportPdf(int saleId)
        {
            var response = await _http.GetAsync($"api/Report/SaleReportPdf/{saleId}");
            await DownloadReport(response, $"SaleReport-{saleId}.pdf");
        }

        public async Task DeliveryPdf(int saleId, string? saleCode)
        {
            try
            {
                var response = await _http.GetAsync($"api/Report/DeliveryPdf/{saleId}");

                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as bytes
                    var pdfBytes = await response.Content.ReadAsByteArrayAsync();

                    // Save the PDF file
                    var fileName = $"{saleCode}.pdf";
                    var contentType = "application/pdf";
                    await _jSRuntime.InvokeVoidAsync("saveAsFile", fileName, contentType, pdfBytes);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        public async Task SaleReceiptPdf(int saleId)
        {
            var response = await _http.GetAsync($"api/Report/SaleReceiptPdf/{saleId}");
            await DownloadReport(response, $"Receive-{saleId}.pdf");
        }


        public async Task GetYearlyReportPdf()
        {
            var response = await _http.GetAsync($"api/Report/GenerateYearlyReportPdf");
            await DownloadReport(response, $"YearlyReport.pdf");
        }

        public async Task GetMonthlyReportPdf()
        {
            var response = await _http.GetAsync($"api/Report/GenerateMonthlyReportPdf");
            await DownloadReport(response, $"MonthlyReport.pdf");
        }

        public async Task GetWeeklyReportPdf()
        {
            var response = await _http.GetAsync($"api/Report/GenerateWeeklyReportPdf");
            await DownloadReport(response, $"WeeklyReport.pdf");
        }

        public async Task GetDailyReportPdf()
        {
            var response = await _http.GetAsync($"api/Report/GenerateDailyReportPdf");
            await DownloadReport(response, $"DailyReport.pdf");
        }

        public async Task GetStockReportPdf()
        {
            var response = await _http.GetAsync($"api/Report/GenerateStockReportPdf");
            await DownloadReport(response, $"StockReport.pdf");
        }

        private async Task DownloadReport(HttpResponseMessage response, string fileName)
        {
            if (response.IsSuccessStatusCode)
            {
                var bytes = await response.Content.ReadAsByteArrayAsync();
                await _jSRuntime.InvokeVoidAsync("saveAsFile", fileName, "application/octet-stream", bytes);
            }
            else
            {
                
            }
        }
    }
}
