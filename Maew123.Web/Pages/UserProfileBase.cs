using Microsoft.AspNetCore.Components;
using Maew123.Models.Models;

namespace Maew123.Web.Pages
{
    public class UserProfileBase: ComponentBase
    {
        [Inject]
        NavigationManager navigationManager { get; set; } = default;

        protected UserInfoDto model = new UserInfoDto();
        protected DateTime? expiration;
        protected string? errorMessage;
    }
}
