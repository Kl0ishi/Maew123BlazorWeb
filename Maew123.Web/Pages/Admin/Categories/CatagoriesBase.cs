using CurrieTechnologies.Razor.SweetAlert2;
using Maew123.Web.Services;
using Maew123.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Maew123.Web.Pages.Admin.Categories
{
    public class CatagoriesBase : ComponentBase
    {
        [Inject]
        protected ICatagoryService _catagoryService { get; set; }

        [Inject]
        SweetAlertService Swal { get; set; }

        public ProductCatagory model = new ProductCatagory();
        protected DateTime? expiration;
        protected string? errorMessage;
        protected string? Errors;

        protected override async Task OnInitializedAsync()
        {
            await _catagoryService.GetCatagories();
        }
        public async Task CreateCatagory()
        {
            try
            {
                var productNameWithoutSpaces = model.ProductCatagoryName.Replace(" ", "");

                // Check if the product type name (without spaces) already exists
                if (_catagoryService.Catagories.Any(t => t.ProductCatagoryName.Replace(" ", "") == productNameWithoutSpaces))
                {
                    // Show error message
                    await Swal.FireAsync(new SweetAlertOptions
                    {
                        Icon = "error",
                        Title = "มีชื่อนี้อยู่แล้วในระบบ",
                        Text = $"Product Catagory with name '{model.ProductCatagoryName}' already exists.",
                        ShowConfirmButton = true
                    });
                    return; // Exit method to prevent further execution
                }

                model.Deleted = false;
                model.Visible = true;
                model.Url = model.ProductCatagoryName.ToLower();
                var result = await _catagoryService.CreateCatagory(model);
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
                var result = await _catagoryService.UpdateCatagory(model);
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