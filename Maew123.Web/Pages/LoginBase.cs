using Microsoft.AspNetCore.Components;
using Maew123.Web.Services.Contracts;
using Blazored.LocalStorage;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;


namespace Maew123.Web.Pages
{
    public class LoginBase : ComponentBase
    {
        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }
        [Inject]
        ILocalStorageService localStorageService { get; set; }
        [Inject]
        protected NavigationManager navigationManager { get; set; }
        [Inject]
        SweetAlertService Swal { get; set; }
        [Inject]
        AuthenticationStateProvider authStateProvider { get; set; }

        protected LoginRequest model = new LoginRequest();
        protected bool ShowRegistrationErrors { get; set; }
        protected IEnumerable<string> Errors;
        protected string returnUrl = string.Empty;
        protected bool isloading { get; set; } = false;

        protected override void OnInitialized()
        {
            var uri = navigationManager.ToAbsoluteUri(navigationManager.Uri);
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("returnUrl", out var url))
            {
                returnUrl = url;
            }
        }

        protected async Task SubmitAsync()
        {
            isloading = true;
            var result = await AuthenticationService.LoginAsync(model);
            if (!result.Result)
            {
                isloading = false;
                Errors = result.Errors!;
                ShowRegistrationErrors = true;
                await Swal.FireAsync(new SweetAlertOptions
                {
                    Icon = "error",
                    Title = string.Join("\n", result.Errors),
                    ShowConfirmButton = false,
                    Timer = 3000
                });
            }
            else
            {
                isloading = false;
                if (result.Expired < DateTime.Now)
                {
                    await Swal.FireAsync(new SweetAlertOptions
                    {
                        Title = "กรุณายืนยันตัวตนให้สำเร็จก่อน",
                        Icon = "success",
                        ShowConfirmButton = false,
                        Timer = 3000
                    });
                    navigationManager.NavigateTo("Identifying");
                }
                else
                {
                    await Swal.FireAsync(new SweetAlertOptions
                    {
                        Icon = "success",
                        Title = "ยินดีต้อนรับเข้าสู่ระบบ",
                        ShowConfirmButton = false,
                        Timer = 3000
                    });
                    await authStateProvider.GetAuthenticationStateAsync();
                    navigationManager.NavigateTo(returnUrl);
                }
            }
        }

        protected void NavigateToRoot()
        {
            navigationManager.NavigateTo("/");
        }

    }
}
