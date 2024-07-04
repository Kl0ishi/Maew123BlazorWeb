using CurrieTechnologies.Razor.SweetAlert2;
using Maew123.Web.Services;
using Maew123.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Maew123.Web.Pages.Admin.Promotion
{
    public class PromotionBase : ComponentBase
    {
        [Inject]
        protected IPromotionService _promotionService { get; set; }

        [Inject]
        SweetAlertService Swal { get; set; }

        public PromotionDto model = new PromotionDto();
        protected DateTime? expiration;
        protected string? errorMessage;
        protected string? Errors;
        protected override async Task OnInitializedAsync()
        {
            await _promotionService.GetPromotionsAdmin();
            model.StartDate = DateOnly.FromDateTime(DateTime.Now);
            model.EndDate = DateOnly.FromDateTime(DateTime.Now).AddDays(1);

        }
        public async Task CreatePromotion()
        {
            try
            {
                model.PromotionType = "Fixed";

                // Check if the promotion name already exists, ignoring spaces
                string promotionNameWithoutSpaces = model.PromotionName.Replace(" ", "");
                var existingPromotion = _promotionService.PromotionsAdmin.FirstOrDefault(p => p.PromotionName.Replace(" ", "") == promotionNameWithoutSpaces);

                if (existingPromotion != null)
                {
                    // If the end date of the existing promotion is not expired, display an error
                    if (existingPromotion.EndDate >= DateOnly.FromDateTime(DateTime.Today))
                    {
                        await Swal.FireAsync(new SweetAlertOptions
                        {
                            Icon = "error",
                            Title = "ชื่อกี่สร้างนั้นมีอยู่แล้วและยังไม่หมดอายุ",
                            Text = $"Promotion with name '{model.PromotionName}' already exists \n and its end date is not expired.",
                            ShowConfirmButton = true
                        });
                        return; // Exit method to prevent further execution
                    }
                }

                var result = await _promotionService.CreatePromotion(model);
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
        public async Task UpdatePromotion()
        {
            try
            {
                var result = await _promotionService.UpdatePromotion(model);
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