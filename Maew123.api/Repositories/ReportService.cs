using CsvHelper.Configuration;
using DinkToPdf.Contracts;
using DinkToPdf;
using System.Globalization;
using CsvHelper;
using System.Text;
using Maew123.Api.Utilities;
using Maew123.Models.Dtos;
using Maew123.Models.Models;
using Maew123.Api.Services;

namespace Maew123.Api.Repositories
{
    public class ReportService : IReportService
    {
        private readonly IConverter _pdfConverter;
        private readonly ISaleRepository _saleRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly AuthService _authService;

        public ReportService(IConverter pdfConverter, ISaleRepository saleRepository, IProductRepository productRepository, IUserRepository userRepository, AuthService authService)
        {
            _pdfConverter = pdfConverter;
            _saleRepository = saleRepository;
            this._productRepository = productRepository;
            this._userRepository = userRepository;
            this._authService = authService;
        }

        public async Task<byte[]> GenerateYearlyReportCsvAsync()
        {
            var startOfYear = new DateTime(DateTime.Today.Year, 1, 1);
            var endOfYear = startOfYear.AddYears(1).AddDays(-1);


            var sales = await _saleRepository.GetAllSalesForReport();
            var byDate = sales.Where(s => s.OrderDate.Date >= startOfYear && s.OrderDate.Date <= endOfYear)
            .ToList();

            List<SaleReport> saleReports = new List<SaleReport>();

            foreach (var cartsDto in byDate)
            {
                SaleReport saleReport = ModelMapperUtils.ConvertFromCartsDto(cartsDto);
                saleReports.Add(saleReport);
            }

            return await GenerateCsvReport(saleReports);
        }

        public async Task<byte[]> GenerateDailyReportCsvAsync()
        {
            var today = DateTime.Today;

            var sales = await _saleRepository.GetAllSalesForReport();
            var byDate = sales.Where(s => s.OrderDate.Date == today).ToList();

            List<SaleReport> saleReports = new List<SaleReport>();

            foreach (var cartsDto in byDate)
            {
                SaleReport saleReport = ModelMapperUtils.ConvertFromCartsDto(cartsDto);
                saleReports.Add(saleReport);
            }

            return await GenerateCsvReport(saleReports);
        }

        public async Task<byte[]> GenerateWeeklyReportCsvAsync()
        {
            var startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(6);

            var sales = await _saleRepository.GetAllSalesForReport();
            var byDate = sales.Where(s => s.OrderDate.Date >= startOfWeek && s.OrderDate.Date <= endOfWeek).ToList();

            List<SaleReport> saleReports = new List<SaleReport>();

            foreach (var cartsDto in byDate)
            {
                SaleReport saleReport = ModelMapperUtils.ConvertFromCartsDto(cartsDto);
                saleReports.Add(saleReport);
            }

            return await GenerateCsvReport(saleReports);
        }

        public async Task<byte[]> GenerateMonthlyReportCsvAsync()
        {
            var startOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var sales = await _saleRepository.GetAllSalesForReport();
            var byDate = sales.Where(s => s.OrderDate.Date >= startOfMonth && s.OrderDate.Date <= endOfMonth).ToList();

            List<SaleReport> saleReports = new List<SaleReport>();

            foreach (var cartsDto in byDate)
            {
                SaleReport saleReport = ModelMapperUtils.ConvertFromCartsDto(cartsDto);
                saleReports.Add(saleReport);
            }

            return await GenerateCsvReport(saleReports);
        }

        public async Task<byte[]> GenerateStockReportCsvAsync()
        {
            var products = await _productRepository.NewGetAdminProducts();
            var numless = products.Where(p => p.numStock <= 10).ToList();

            List<StockReport> stockReports = new List<StockReport>();

            foreach (var product in numless)
            {
                StockReport stockReport = ModelMapperUtils.ConvertFromNewProductDto(product);
                stockReports.Add(stockReport);
            }
            return await GenerateStockCsvReport(stockReports);
        }

        public async Task<byte[]> GenerateSalePdf(int saleId)
        {
            var sale = await _saleRepository.GetSaleBySaleId(saleId);
            sale.SaleItems = await _saleRepository.GetSaleItemsBySaleId(saleId);

            if (sale == null)
            {
                return null;
            }

            var htmlContent = await HtmlContentForSalePdf(sale);

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10, Bottom = 10 }
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent
            };

            var pdf = _pdfConverter.Convert(new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            });

            return pdf;
        }

        public async Task<byte[]> GenerateDeliveryPdf(int saleId)
        {
            var sale = await _saleRepository.GetSaleBySaleId(saleId);
            if (sale == null)
            {
                return null;
            }
            var htmlContent = await GenerateHtmlContentForDelivery(sale);

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Landscape, // Adjusted to Landscape for a small rectangular shape
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10, Bottom = 10 }
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent
            };

            var pdf = _pdfConverter.Convert(new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            });

            return pdf;

        }

        public async Task<byte[]> GenerateDailyReportPdfAsync()
        {
            var today = DateTime.Today;

            var sales = await _saleRepository.GetAllSalesForReport();
            var byDate = sales.Where(s => s.OrderDate.Date == today).ToList();

            List<SaleReport> saleReports = ConvertToSaleReports(byDate);

            return await GeneratePdfReport(saleReports, "Daily Sales Report");
        }

        public async Task<byte[]> GenerateWeeklyReportPdfAsync()
        {
            var startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(6);

            var sales = await _saleRepository.GetAllSalesForReport();
            var byDate = sales.Where(s => s.OrderDate.Date >= startOfWeek && s.OrderDate.Date <= endOfWeek).ToList();

            List<SaleReport> saleReports = ConvertToSaleReports(byDate);

            return await GeneratePdfReport(saleReports, "Weekly Sales Report");
        }

        public async Task<byte[]> GenerateMonthlyReportPdfAsync()
        {
            var startOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var sales = await _saleRepository.GetAllSalesForReport();
            var byDate = sales.Where(s => s.OrderDate.Date >= startOfMonth && s.OrderDate.Date <= endOfMonth).ToList();

            List<SaleReport> saleReports = ConvertToSaleReports(byDate);

            return await GeneratePdfReport(saleReports, "Monthly Sales Report");
        }

        public async Task<byte[]> GenerateYearlyReportPdfAsync()
        {
            var startOfYear = new DateTime(DateTime.Today.Year, 1, 1);
            var endOfYear = startOfYear.AddYears(1).AddDays(-1);


            var sales = await _saleRepository.GetAllSalesForReport();
            var byDate = sales.Where(s => s.OrderDate.Date >= startOfYear && s.OrderDate.Date <= endOfYear)
            .ToList();

            List<SaleReport> saleReports = ConvertToSaleReports(byDate);

            return await GeneratePdfReport(saleReports, "Yearly Sales Report");
        }

        private List<SaleReport> ConvertToSaleReports(List<CartsDto> saleDtos)
        {
            List<SaleReport> saleReports = new List<SaleReport>();

            foreach (var cartsDto in saleDtos)
            {
                SaleReport saleReport = ModelMapperUtils.ConvertFromCartsDto(cartsDto);
                saleReports.Add(saleReport);
            }

            return saleReports;
        }

        private async Task<byte[]> GeneratePdfReport(List<SaleReport> saleReports, string reportTitle)
        {
            var htmlContent = await HtmlContentForReportPdf(saleReports, reportTitle);

            // Convert HTML to PDF
            var document = new HtmlToPdfDocument()
            {
                GlobalSettings = {
            ColorMode = ColorMode.Color,
            Orientation = Orientation.Landscape,
            PaperSize = PaperKind.A4Plus,
        },
                Objects = {
            new ObjectSettings() {
                HtmlContent = htmlContent,
            }
        }
            };

            return _pdfConverter.Convert(document);
        }

        private async Task<string> HtmlContentForReportPdf(List<SaleReport> saleReports, string reportTitle)
        {
            string reportTitleWithDateRange = "";
            switch (reportTitle)
            {
                case "Daily Sales Report":
                    DateTime dailyDate = DateTime.Today;
                    string dailyDateThai = dailyDate.ToString("d MMM yyyy", new CultureInfo("th-TH"));
                    reportTitleWithDateRange = $"รายงานประจำวันที่ {dailyDateThai}";
                    break;
                case "Weekly Sales Report":
                    DateTime startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                    DateTime endOfWeek = startOfWeek.AddDays(6);
                    string startOfWeekThai = startOfWeek.ToString("d MMM yyyy", new CultureInfo("th-TH"));
                    string endOfWeekThai = endOfWeek.ToString("d MMM yyyy", new CultureInfo("th-TH"));
                    reportTitleWithDateRange = $"รายงานประจำสัปดาห์ที่ {startOfWeekThai} - {endOfWeekThai}";
                    break;
                case "Monthly Sales Report":
                    DateTime startOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
                    string startOfMonthThai = startOfMonth.ToString("d MMM yyyy", new CultureInfo("th-TH"));
                    string endOfMonthThai = endOfMonth.ToString("d MMM yyyy", new CultureInfo("th-TH"));
                    reportTitleWithDateRange = $"รายงานประจำเดือน {startOfMonthThai} - {endOfMonthThai}";
                    break;
                case "Yearly Sales Report":
                    int currentYear = DateTime.Today.Year;
                    DateTime startOfYear = new DateTime(currentYear, 1, 1);
                    DateTime endOfYear = startOfYear.AddYears(1).AddDays(-1);
                    string startOfYearThai = startOfYear.ToString("d MMM yyyy", new CultureInfo("th-TH"));
                    string endOfYearThai = endOfYear.ToString("d MMM yyyy", new CultureInfo("th-TH"));
                    reportTitleWithDateRange = $"รายงานประจำปีวันที่ {startOfYearThai} - {endOfYearThai}";
                    break;
                default:
                    reportTitleWithDateRange = "รายงาน";
                    break;
            }

            var htmlContent = $@"
<html>
<head>
    <meta charset=""UTF-8"">
    <style>
        table {{
            width: 100%;
            border-collapse: collapse;
        }}
        th, td {{
            border: 1px solid black;
            padding: 8px;
        }}
        th {{
            background-color: #f2f2f2;
            text-align: left;
        }}
        td.price {{
            text-align: right;
        }}
        tr {{
            page-break-inside: avoid;
        }}
    </style>
</head>
<body>
    <h2>{reportTitleWithDateRange}</h2>
    <table>
        <thead>
            <tr>
                <th>รหัสการสั่งซื้อ</th>
                <th>จำนวนที่สั่ง</th>
                <th>จำนวนส่วนลด</th>
                <th>ราคารวม</th>
                <th>วันที่สั่งซื้อ</th>
            </tr>
        </thead>
        <tbody>";

            decimal totalPrice = 0;

            foreach (var sale in saleReports)
            {
                htmlContent += $@"
        <tr>
            <td>{sale.SaleCode}</td>
            <td>{sale.SaleNum}</td>
            <td>{sale.SaleDiscount}</td>
            <td class=""price"">{sale.SaleTotal?.ToString("N2")}</td>
            <td>{sale.OrderDate}</td>
        </tr>";

                totalPrice += sale.SaleTotal ?? 0;
            }

            htmlContent += @"
        </tbody>
    </table>
    <div style=""text-align: left; margin-top: 10px;"">ยอดรวมเป็น: <span id=""total"">" + totalPrice.ToString("N2") + @" บาท</span></div>
</body>
</html>";

            return htmlContent;
        }

        public async Task<byte[]> GenerateStockReportPdfAsync()
        {
            var products = await _productRepository.NewGetAdminProducts();
            var numLessProducts = products.Where(p => p.numStock <= 10).ToList();

            List<StockReport> stockReports = new List<StockReport>();

            foreach (var product in numLessProducts)
            {
                StockReport stockReport = ModelMapperUtils.ConvertFromNewProductDto(product);
                stockReports.Add(stockReport);
            }

            return await GeneratePdfReport(stockReports, "Low Stock Products Report");
        }

        private async Task<byte[]> GeneratePdfReport(List<StockReport> stockReports, string reportTitle)
        {
            var htmlContent = await HtmlContentForStockReportPdf(stockReports, reportTitle);

            // Convert HTML to PDF
            var document = new HtmlToPdfDocument()
            {
                GlobalSettings = {
            ColorMode = ColorMode.Color,
            Orientation = Orientation.Landscape,
            PaperSize = PaperKind.A4Plus,
        },
                Objects = {
            new ObjectSettings() {
                HtmlContent = htmlContent,
            }
        }
            };

            return _pdfConverter.Convert(document);
        }

        private async Task<string> HtmlContentForStockReportPdf(List<StockReport> stockReports, string reportTitle)
        {
            var htmlContent = $@"
<html>
<head>
    <meta charset=""UTF-8"">
</head>
<body>
    <h2>{reportTitle}</h2>
    <table border=""1"">
        <tr>
            <th>Product Name</th>
            <th>Product Status</th>
            <th>Product Category</th>
            <th>Product Type</th>
            <th>Stock Number</th>
            <th>Promotion Name</th>
            <th>Discount</th>
            <th>Price</th>
            <th>Condition</th>
            <th>Insert By</th>
            <th>Insert Date</th>
            <th>Update By</th>
            <th>Update Date</th>
        </tr>";

            foreach (var report in stockReports)
            {
                htmlContent += $@"
        <tr>
            <td>{report.ProductName}</td>
            <td>{report.ProductStatus}</td>
            <td>{report.ProductCatagoryName}</td>
            <td>{report.ProductTypeName}</td>
            <td>{report.numStock}</td>
            <td>{report.PromotionName}</td>
            <td>{report.Discount}</td>
            <td>{report.Price}</td>
            <td>{report.Condition}</td>
            <td>{report.InsertBy}</td>
            <td>{report.InsertDate}</td>
            <td>{report.UpdateBy}</td>
            <td>{report.UpdateDate}</td>
        </tr>";
            }

            htmlContent += @"
    </table>
</body>
</html>";

            return htmlContent;
        }

        public async Task<byte[]> GenerateReceiptPdf(int saleId)
        {
            var user = await _userRepository.GetUserById(_authService.GetUserId());
            var carts = await _saleRepository.GetCartsById(saleId);
            if (carts.UserId == user.UserId || user.RoleId == 6 || user.RoleId == 7)
            {
                if (carts == null)
                {
                    return null;
                }

                var htmlContent = await HtmlContentForReceipt(carts);

                var globalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings { Top = 10, Bottom = 10 }
                };

                var objectSettings = new ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = htmlContent
                };

                var pdf = _pdfConverter.Convert(new HtmlToPdfDocument()
                {
                    GlobalSettings = globalSettings,
                    Objects = { objectSettings }
                });

                return pdf;
            }
            return null;
        }

        private async Task<byte[]> GenerateCsvReport(List<SaleReport> sales)
        {
            return await Task.Run(() =>
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture);
                using var memoryStream = new MemoryStream();
                using var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);
                using var csvWriter = new CsvWriter(streamWriter, config);

                csvWriter.WriteRecords(sales);
                streamWriter.Flush();

                return memoryStream.ToArray();
            });
        }

        private async Task<byte[]> GenerateStockCsvReport(List<StockReport> stocks)
        {
            //await Task.Delay(100);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture);
            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);
            using var csvWriter = new CsvWriter(streamWriter, config);

            csvWriter.WriteRecords(stocks);
            streamWriter.Flush();

            return memoryStream.ToArray();
        }

        private async Task<string> HtmlContentForSalePdf(Sale sale)
        {
            //await Task.Delay(100);
            // Implement logic to format sale data into HTML content
            // You can use a library like RazorEngine for more advanced HTML templating
            // For simplicity, concatenate HTML strings here

            var htmlContent = $@"
        <html>
            <head>
            <meta charset=""UTF-8"">
                <style>
                    body {{
                        font-family: 'Arial', sans-serif;
                    }}
                    h1 {{
                        color: #333;
                    }}
                    table {{
                        width: 100%;
                        border-collapse: collapse;
                        margin-top: 20px;
                    }}
                    th, td {{
                        border: 1px solid #ddd;
                        padding: 8px;
                        text-align: left;
                    }}
                    th {{
                        background-color: #f2f2f2;
                    }}
                </style>
            </head>
            <body>
                <h1>Sale Report</h1>
                <p>Sale Id: {sale.SaleId}</p>
                <p>Sale Code: {sale.SaleCode}</p>
                <p>Order Date: {sale.OrderDate.ToShortDateString()}</p>
                
                <h2>Sale Details</h2>
                <table>
                    <tr>
                        <th>Product Name</th>
                        <th>Quantity</th>
                        <th>Unit Price</th>
                        <th>Total Price</th>
                    </tr>";

            foreach (var cartDetail in sale.SaleItems)
            {
                htmlContent += $@"
                    <tr>
                        <td>{cartDetail.Product.ProductName}</td>
                        <td>{cartDetail.Qty}</td>
                        <td>${cartDetail.Product.Price}</td>
                        <td>${cartDetail.TotalPrice}</td>
                    </tr>";
            }

            htmlContent += $@"
                </table>
                <h1> รวมเป็นจำนวน: {sale.SaleTotal} บาท </h1>
                <h1>หมายเลขพัสดุ: {sale.ParcelNumber}</h1>
            </body>
        </html>
    ";

            return htmlContent;
        }

        private async Task<string> GenerateHtmlContentForDelivery(Sale sale)
        {
            //await Task.Delay(100);
            var address = await _userRepository.GetAddressSnapshotById(sale.AddressSnapshotId);
            var user = await _userRepository.GetUserById(sale.UserId);

            var htmlContent = $@"
        <html>
        <head>
            <meta charset=""UTF-8"">
        </head>
        <body>
            <h2>ข้อมูลส่งสินค้า</h2>
            <p><strong>จาก:</strong> ธนวรรณ นิมิตวานิชกุล T.0894823768</p>
            <p><strong></strong> 63 ซ. วชิรธรรมสาธิต 57 แยก 37-5-1 ถ.สุขุมวิท 101/1
                                    บางจาก พระโขนง กทม.10260</p>
            <p><strong>ถึง :</strong> {address.FirstName} {address.LastName}</p>
            <p><strong>Address:</strong> {address.AddressName} {address.Street} {address.City} {address.State} {address.Zip} {address.Country}</p>
            <p><strong>เบอร์โทร:</strong> {address.Phone}</p>

        </body>
        </html>";

            return htmlContent;
        }

        private async Task<string> HtmlContentForReceipt(CartsDto sale)
        {
            decimal? sumSale = 0;
            int sumDiscount = 0;
            decimal? totalPrice = 0;
            decimal totalVat = 0;
            decimal totalPriceWithoutVat = 0;

            var tableRows = "";
            foreach (var item in sale.CartDetails)
            {
                sumSale += item.SumPrice;
                sumDiscount += (item.Discount * item.Quantity);
                totalPrice += item.TotalPrice;
                totalVat += (item.VatPrice ?? 0);
                totalPriceWithoutVat += (item.SumWithoutVat ?? 0);

                tableRows += $@"
            <tr>
                <td>{item.ProductName}</td>
                <td>{item.Quantity}</td>
                <td>{item.DiscountedPrice.Value.ToString("N2")}</td>
                <td>{item.TotalPrice.Value.ToString("N2")}</td>
            </tr>";
            }

            var htmlContent = $@"
    <html>
        <head>
            <meta charset=""UTF-8"">
            <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css"" rel=""stylesheet"" integrity=""sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC"" crossorigin=""anonymous"">
            <link rel=""stylesheet"" href=""https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/css/bootstrap.min.css"" integrity=""sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm"" crossorigin=""anonymous"">
            <style>
                .box-centerside {{
                    display: inline-block;
                    margin-left: 0%;
                    width: 75%;
                    padding: 15px 20px 15px 20px;
                    flex-shrink: 0;
                    border: 1px solid rgba(0, 0, 0, 0.30);
                    background: #FFF;
                }}
                .Centering{{
                    display:flex;
                    justify-content:center;
                    align-items:center;
                }}
                .Spacing {{
                    display: flex;
                    justify-content: space-between;
                    padding: 0 3px 3px 3px;
                }}
                .ReceiptTitle {{
                    font-size: 20px;
                    margin-right: 10px;
                }}
                .ReceiptText{{
                    font-size:16px;
                    margin-top: 10px;
                }}
                .ReceiptText {{
                    font-size: 16px;
                }}
                .ReceiptNumber {{
                    display: flex;
                    justify-content: center;
                    align-items: center;
                    font-size: 20px;
                    font-weight: 700;
                    color: #FFF;
                    background-color: black;
                    width: 170px;
                    height: 50px;
                }}
                .NoteBox {{
                    background-color: rgba(0, 0, 0, 0.1);
                    padding: 10px 10px 10px 20px;
                    width: 400px;
                    height: 100px;
                }}
                .Half {{
                    flex:0 0 50%;
                }}

                table {{
                    border-collapse: collapse;
                    border-top: 2px solid black;
                }}
                hr {{
                    height: 2px;
                    background-color: black;
                    }}
                th, td {{
                    padding: 8px;
                }}
            </style>
            </head>
        <body>
            <div class=""d-flex justify-content-center align-items-center"">
                <div class=""ReceiptTitle"" style=""font-weight:700;"">
                    ใบเสร็จรับเงิน(Receipt)
                </div>
                <div class=""d-flex justify-content-center align-items-center ReceiptNumber"">
                    #{sale.SaleCode}
                </div>
            </div>
            <div class=""d-flex mt-2"">
                <div class=""Half"">
                    <div class=""ReceiptTitle"">
                        ร้านค้าผู้ให้บริการ
                    </div>
                    <div class=""ReceiptText"">
                        ร้านขายสินค้า IT Maew123 จำกัด
                    </div>
                    <div class=""ReceiptText"">
                        ซอย วชิรธรรมสาธิต 57 แยก 37-5-1 ตำบลบางจาก อำเภอพระโขนง ถนนสุขุมวิท 101/1 กรุงเทพมหานคร 10260
                    </div>
                    <div class=""ReceiptText"">
                        หมายเลขประจำผู้เสียภาษีอากร 012345678910
                    </div>
                    <div class=""ReceiptText"">
                        ติดต่อ 0894823768 หรือ 0875910805
                    </div>
                </div>
                <div class=""Half"">
                    <div class=""ReceiptTitle"">
                        รายละเอียดลูกค้า
                    </div>
                    <div class=""ReceiptText"">
                        คุณ {sale.FirstName} {sale.LastName}
                    </div>
                    <div class=""ReceiptText"">
                        ติดต่อ {sale.UserTel}
                    </div>
                    <div class=""ReceiptText"">
                        วันที่ {sale.FormattedPayDate}
                    </div>
                    <div class=""ReceiptText"">
                        ชำระผ่านทาง ธนาคารไทยพาณิชย์ SCB
                    </div>
                </div>
            </div>
        <table class=""table mt-4"">
            <thead>
                <tr>
                    <th style=""width: 50%;"">รายการสินค้า</th>
                    <th style=""width: 10%;text-align:right;"">จำนวน</th>
                    <th style=""width: 20%;text-align:right;"">ราคาต่อหน่วย</th>
                    <th style=""width: 20%;text-align:right;"">จำนวนเงิน</th>
                </tr>
            </thead>
            <tbody>
                {tableRows}
            </tbody>
        </table>
            <hr />
            <div class=""d-flex mt-2"">
                <div class=""col"">
                    <div class=""NoteBox"">
                        หมายเหตุ
                    </div>
                </div>
                <div class=""col"">
                    <div class=""d-flex justify-content-between px-3 py-1"">
                        <div class=""ReceiptTotal"">
                            ทั้งหมด
                        </div>
                        <div class=""ReceiptTotal"">
                            {sumSale.Value.ToString("N2")} บาท
                        </div>
                    </div>
                    <div class=""d-flex justify-content-between px-3 py-1 mt-2"">
                        <div class=""ReceiptTotal"">
                            ส่วนลด
                        </div>
                        <div class=""ReceiptTotal"">
                           {@sumDiscount.ToString("N2")} บาท
                        </div>
                    </div>
                    <div class=""d-flex justify-content-between px-3 py-1 mt-2"" style=""background-color: rgba(0, 0, 0, 0.1);"">
                        <div class=""ReceiptTotal"">
                            ราคารวมสุทธิ
                        </div>
                        <div class=""ReceiptTotal"">
                           {totalPrice.Value.ToString("N2")} บาท
                        </div>
                    </div>
                    <div class=""d-flex justify-content-between px-3 py-1 mt-3"">
                        <div class=""ReceiptTotal"">
                            ภาษีมูลค่าเพิ่ม 7%
                        </div>
                        <div class=""ReceiptTotal"">
                            {totalVat.ToString("N2")} บาท
                        </div>
                    </div>
                    <div class=""d-flex justify-content-between px-3 py-1 mt-2"">
                        <div class=""ReceiptText"">
                            ราคาไม่รวมภาษีมูลค่าเพิ่ม
                        </div>
                        <div class=""ReceiptText"">
                            {totalPriceWithoutVat.ToString("N2")} บาท
                        </div>
                    </div>
                    <div class=""d-flex justify-content-between px-3 py-1 mt-2"" style=""background-color: rgba(0, 0, 0, 0.1);"">
                        <div class=""ReceiptTotal"">
                            ราคารวมสุทธิ
                        </div>
                        <div class=""ReceiptTotal"">
                            {ConvertToBaht.ConvertToThaiBaht(totalPrice.HasValue ? totalPrice.Value : 0)}
                        </div>
                    </div>
                </div>
            </div>
            <div class=""d-flex justify-content-center align-items-center mt-4"">
                ขอบคุณที่ใช้บริการ(Thank you)
            </div>
        </body>
    </html>";

            return htmlContent;
        }
    }
}
