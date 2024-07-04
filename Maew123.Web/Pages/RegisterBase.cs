using Microsoft.AspNetCore.Components;
using Maew123.Web.Services.Contracts;
using System.ComponentModel.DataAnnotations;
using Maew123.Web.Utilities;
using Blazored.LocalStorage;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Authorization;

namespace Maew123.Web.Pages
{
    public class RegisterBase : ComponentBase
    {
        [Inject]
        IAuthenticationService? AuthenticationService { get; set; }
        [Inject]
        ILocalStorageService localStorageService { get; set; }
        [Inject]
        NavigationManager navigationManager { get; set; } = default;
        [Inject]
        SweetAlertService Swal { get; set; }
        [Inject]
        AuthenticationStateProvider? authStateProvider { get; set; }

        protected RegisterRequest model = new RegisterRequest();
        protected bool ShowRegistrationErrors { get; set; }
        protected IEnumerable<string> Errors;
        protected bool isloading { get; set; } = false;

        //protected override async Task OnInitializedAsync()
        //{
        //   httpClient.DefaultRequestHeaders.Add("CustomHeader", "myValue");
        //}

        public async Task Register()
        {
            ShowRegistrationErrors = false;
            isloading = true;

            var result = await AuthenticationService.RegisterAsync(model);
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
                await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = "เหลือเพียงอีกหนึ่งขั้นตอนก็จะสำเร็จ",
                    Icon = "success",
                    ShowConfirmButton = false,
                    Timer = 3000
                });
                navigationManager.NavigateTo("Identifying");
                //ย้ายไปใส่หน้า identifying await authStateProvider.GetAuthenticationStateAsync();
            }

        }

        protected void NavigateToRoot()
        {
            navigationManager.NavigateTo("/");
        }

    }
}
