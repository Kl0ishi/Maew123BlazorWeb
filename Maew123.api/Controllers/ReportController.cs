using Maew123.Api.Services;
using Maew123.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Maew123.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            this._reportService = reportService;
        }

        [HttpGet("[action]"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetYearlyReportCsv()
        {
            var csvBytes = await _reportService.GenerateYearlyReportCsvAsync();
            return File(csvBytes, "text/csv", "yearly_report.csv");
        }

        [HttpGet("daily-report"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDailyReportCsv()
        {
            var csvBytes = await _reportService.GenerateDailyReportCsvAsync();
            return File(csvBytes, "text/csv", "daily_report.csv");
        }

        [HttpGet("weekly-report"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetWeeklyReportCsv()
        {
            var csvBytes = await _reportService.GenerateWeeklyReportCsvAsync();
            return File(csvBytes, "text/csv", "weekly_report.csv");
        }

        [HttpGet("monthly-report"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetMonthlyReportCsv()
        {
            var csvBytes = await _reportService.GenerateMonthlyReportCsvAsync();
            return File(csvBytes, "text/csv", "monthly_report.csv");
        }

        [HttpGet("stock-report"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetStockReportCsv()
        {
            var csvBytes = await _reportService.GenerateStockReportCsvAsync();
            return File(csvBytes, "text/csv", "stock_report.csv");
        }

        [HttpGet("[action]/{saleId}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> SaleReportPdf(int saleId)
        {
            var pdfBytes = await _reportService.GenerateSalePdf(saleId);
            return File(pdfBytes, "application/pdf", "printable_report.pdf");
        }

        [HttpGet("[action]/{saleId}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeliveryPdf(int saleId)
        {
            var pdfBytes = await _reportService.GenerateDeliveryPdf(saleId);
            return File(pdfBytes, "application/pdf", "printable_report.pdf");
        }

        [HttpGet("[action]"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GenerateDailyReportPdf()
        {
            var pdfBytes = await _reportService.GenerateDailyReportPdfAsync();
            return File(pdfBytes, "application/pdf", "printable_report.pdf");
        }

        [HttpGet("[action]"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GenerateWeeklyReportPdf()
        {
            var pdfBytes = await _reportService.GenerateWeeklyReportPdfAsync();
            return File(pdfBytes, "application/pdf", "printable_report.pdf");
        }

        [HttpGet("[action]"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GenerateMonthlyReportPdf()
        {
            var pdfBytes = await _reportService.GenerateMonthlyReportPdfAsync();
            return File(pdfBytes, "application/pdf", "printable_report.pdf");
        }

        [HttpGet("[action]"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GenerateYearlyReportPdf()
        {
            var pdfBytes = await _reportService.GenerateYearlyReportPdfAsync();
            return File(pdfBytes, "application/pdf", "printable_report.pdf");
        }

        [HttpGet("[action]"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GenerateStockReportPdf()
        {
            var pdfBytes = await _reportService.GenerateStockReportPdfAsync();
            return File(pdfBytes, "application/pdf", "printable_report.pdf");
        }
        

        [HttpGet("[action]/{saleId}"), Authorize]
        public async Task<IActionResult> SaleReceiptPdf(int saleId)
        {

            var pdfBytes = await _reportService.GenerateReceiptPdf(saleId);
            return File(pdfBytes, "application/pdf", "printable_report.pdf");
        }
    }
}
