using CurrieTechnologies.Razor.SweetAlert2;
using Maew123.Web.Pages.Admin.Product;
using Maew123.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Maew123.Web.Pages
{
    public class PaymentBase : ComponentBase
    {
        [Inject]
        protected NavigationManager navigationManager { get; set; } = default;
        [Inject]
        protected SweetAlertService Swal { get; set; }
        [Inject]
        protected ILogger<ProductCreate> Logger { get; set; }
        [Inject]
        protected IWebAssemblyHostEnvironment Environment { get; set; }
        [Inject]
        protected IOrderService _saleService { get; set; }


        protected CartsDto model = new CartsDto();
        protected DateTime? expiration;
        protected string? Errors;

        protected decimal? sumSale = 0;
        protected int sumDiscount;
        protected decimal? totalPrice = 0;
        protected decimal totalVat;
        protected decimal totalPriceWithoutVat;

        public async Task PaySlip()
        {
            try
            {
                if (model.Base64ImageData != null)
                {
                    var isSuccess = await _saleService.Payment(model);
                    if (isSuccess == false)
                    {
                        Errors = "การดำเนินการล้มเหลว";
                        await ShowError();
                    }
                    else
                    {
                        await Swal.FireAsync(new SweetAlertOptions
                        {
                            Title = "ส่งสลิปสำเร็จ",
                            Icon = "success",
                            ShowConfirmButton = false,
                            Timer = 3000
                        });
                        navigationManager.NavigateTo("DeliveryStatus");
                    }
                }
                else
                {
                    Errors = "กรุณาแนบหลักฐานการชำระเงิน";
                    await ShowError();
                }
                //ที่ต้องใส่resetตรงนี้ เพราะมันมีการ re-render หน้าหลังtriggerปุ่ม
                sumSale = 0;
                sumDiscount = 0;
                totalPrice = 0;
                totalVat = 0;
                totalPriceWithoutVat = 0;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        protected async Task ShowError()
        {
            await Swal.FireAsync(new SweetAlertOptions
            {
                Icon = "error",
                Title = string.Join("\n", Errors),
                ShowConfirmButton = false,
                Timer = 3000
            });
        }


        protected List<IBrowserFile> loadedFiles = new();
        protected long maxFileSize = 1024 * 15;
        protected int maxAllowedFiles = 1;
        protected bool isLoading;
        protected string extensionname = "default";
        protected string base64data = ""; //you can set a defaut image
        protected string isdisplayimage;
        protected bool displayImage = false;
        //InputFile Change event
        protected async Task LoadFiles(InputFileChangeEventArgs e)
        {
            isLoading = true;
            loadedFiles.Clear();
            foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
            {
                try
                {
                    loadedFiles.Add(file);

                    //get the upload file extension.
                    extensionname = Path.GetExtension(file.Name);

                    var imagefiletypes = new List<string>() {
                    ".png",".jpg",".jpeg"
                };
                    if (imagefiletypes.Contains(extensionname))
                    {
                        //resize the image and create the thumbnails
                        var resizedFile = await file.RequestImageFileAsync(file.ContentType, 1200, 800); // resize the image file og 640x480
                        var buf = new byte[resizedFile.Size]; // allocate a buffer to fill with the file's data
                        using (var stream = resizedFile.OpenReadStream())
                        {

                            await stream.ReadAsync(buf); // copy the stream to the buffer
                            //var formFile = new FormFile(stream, 0, stream.Length, file.Name, file.Name)
                            //{
                            //    Headers = new HeaderDictionary(),
                            //};
                            //model.ImagePath = formFile;
                        }
                        base64data = "data:image/png;base64," + Convert.ToBase64String(buf); // convert to a base64 string!! 

                        //then you can send the base64 data to the server side and insert it into database.
                        model.Base64ImageData = base64data;

                        //show the thumbnails image
                        isdisplayimage = "block";
                        displayImage = true;
                    }
                    else
                    {
                        isdisplayimage = "none";
                        displayImage = false;
                    };
                }
                catch (Exception ex)
                {
                    Logger.LogError("File: {Filename} Error: {Error}",
                        file.Name, ex.Message);
                }
            }
            isLoading = false;
            //ที่ต้องใส่resetตรงนี้ เพราะมันมีการ re-render หน้าหลังupรูป
            sumSale = 0;
            sumDiscount = 0;
            totalPrice = 0;
            totalVat = 0;
            totalPriceWithoutVat = 0;
        }

        protected string ConvertBytesToReadableString(long bytes)
        {
            const long KB = 1024;
            const long MB = 1024 * 1024;

            if (bytes >= MB)
            {
                return $"{((double)bytes / MB):F2} MB";
            }
            else if (bytes >= KB)
            {
                return $"{((double)bytes / KB):F2} KB";
            }
            else
            {
                return $"{bytes} bytes";
            }
        }
    }
}
