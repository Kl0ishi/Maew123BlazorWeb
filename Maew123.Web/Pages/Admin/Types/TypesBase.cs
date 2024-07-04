using CurrieTechnologies.Razor.SweetAlert2;
using Maew123.Web.Services;
using Maew123.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Maew123.Web.Pages.Admin.Types
{
    public class TypesBase : ComponentBase
    {
        [Inject]
        protected ITypeService _typeService { get; set; }

        [Inject]
        protected ICatagoryService _catagoryService { get; set; }

        [Inject]
        SweetAlertService Swal { get; set; }

        public ProductType model = new ProductType();
        protected DateTime? expiration;
        protected string? errorMessage;
        protected string? Errors;
        public IEnumerable<ProductType> Types;

        public async Task CreateProductType()
        {
            try
            {
                var productNameWithoutSpaces = model.ProductTypeName.Replace(" ", "");

                // Check if the product type name (without spaces) already exists
                if (_typeService.ProductTypes.Any(t => t.ProductTypeName.Replace(" ", "") == productNameWithoutSpaces))
                {
                    // Show error message
                    await Swal.FireAsync(new SweetAlertOptions
                    {
                        Icon = "error",
                        Title = "มีชื่อนี้อยู่แล้วในระบบ",
                        Text = $"Product type with name '{model.ProductTypeName}' already exists.",
                        ShowConfirmButton = true
                    });
                    return; // Exit method to prevent further execution
                }

                var result = await _typeService.CreateProductType(model);
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
        public async Task UpdateCatagory()
        {
            try
            {
                var result = await _typeService.UpdateProductType(model);
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
    }
}
