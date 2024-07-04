using CurrieTechnologies.Razor.SweetAlert2;
using Maew123.Web.Services;
using Maew123.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System.Reflection.PortableExecutable;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Maew123.Web.Pages.Admin.Product
{
    public class ProductBase : ComponentBase
    {
        [Inject]
        NavigationManager navigationManager { get; set; } = default;
        [Inject]
        SweetAlertService Swal { get; set; }
        [Inject]
        protected ILogger<ProductCreate> Logger { get; set; }
        [Inject]
        protected IWebAssemblyHostEnvironment Environment {  get; set; }
        [Inject]
        protected ICatagoryService _catagoryService { get; set; }
        [Inject]
        protected ITypeService _typeService { get; set; }
        [Inject]
        protected IPromotionService _promotionService { get; set; }
        [Inject]
        protected IProductService _productService {  get; set; }

        protected ProductDto model = new ProductDto();
        protected StocksDto stock = new StocksDto();
        protected DateTime? expiration;
        protected string? Errors;

        protected int selectedCatagoryId {  get; set; }
        protected bool IsProductTypeEnabled { get; set; } = false;
        protected bool IsProPriceEnabled {  get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            await _catagoryService.GetCatagories();
            await _typeService.getProductTypes();
            await _promotionService.GetPromotionsAdmin();
            model.ProductStatus = "Available";
        }

        protected void LoadProductTypes(ChangeEventArgs e)
        {
            selectedCatagoryId = Convert.ToInt32(e.Value);
            model.ProductCatagoryId = selectedCatagoryId;

            IsProductTypeEnabled = !string.IsNullOrEmpty(e.Value?.ToString());
        }

        public async Task CreateProduct()
        {
            try
            {
                var result = await _productService.CreateProduct(model);
                if (result == null)
                {
                    Errors = "การดำเนินการล้มเหลว";
                    await Swal.FireAsync(new SweetAlertOptions
                    {
                        Icon = "error",
                        Title = string.Join("\n", Errors),
                        ShowConfirmButton = false,
                        Timer = 3000
                    });
                }
                else
                {
                    await Swal.FireAsync(new SweetAlertOptions
                    {
                        Title = "ดำเนินการสำเร็จ",
                        Icon = "success",
                        ShowConfirmButton = false,
                        Timer = 3000
                    });
                    //navigationManager.NavigateTo("admin/ProductIndex");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public async Task UpdateProduct()
        {
            try
            {
                if(model.ProductStatus == "เลิกจำหน่าย")
                {
                    model.Visible = false;
                    model.Featured = false;
                }
                var result = await _productService.UpdateProduct(model);
                if (result == null)
                {
                    Errors = "การดำเนินการล้มเหลว";
                    await Swal.FireAsync(new SweetAlertOptions
                    {
                        Icon = "error",
                        Title = string.Join("\n", Errors),
                        ShowConfirmButton = false,
                        Timer = 3000
                    });
                }
                else
                {
                    await Swal.FireAsync(new SweetAlertOptions
                    {
                        Title = "ดำเนินการสำเร็จ",
                        Icon = "success",
                        ShowConfirmButton = false,
                        Timer = 3000
                    });
                    //navigationManager.NavigateTo("admin/ProductIndex");
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task UpdateStock()
        {
            try
            {
                var result = await _productService.UpdateStock(stock);
                if (result == null)
                {
                    Errors = "การดำเนินการล้มเหลว";
                    await Swal.FireAsync(new SweetAlertOptions
                    {
                        Icon = "error",
                        Title = string.Join("\n", Errors),
                        ShowConfirmButton = false,
                        Timer = 3000
                    });
                }
                else
                {
                    await Swal.FireAsync(new SweetAlertOptions
                    {
                        Title = "ดำเนินการสำเร็จ",
                        Icon = "success",
                        ShowConfirmButton = false,
                        Timer = 3000
                    });
                    //navigationManager.NavigateTo("admin/ProductIndex");
                }
            }
            catch (Exception ex)
            {

                throw;
            }

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
                        displayImage= false;
                    };
                }
                catch (Exception ex)
                {
                    Logger.LogError("File: {Filename} Error: {Error}",
                        file.Name, ex.Message);
                }
            }
            isLoading = false;
        }


    }
}