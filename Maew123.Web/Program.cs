global using Maew123.Models.Dtos;
global using System.Net.Http.Json;
global using Microsoft.AspNetCore.Components.Authorization;
global using Maew123.Models.Models;
global using Maew123.Models.InputedValues;
using Maew123.Web;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using Maew123.Web.Services;
using Maew123.Web.Services.Contracts;
using CurrieTechnologies.Razor.SweetAlert2;
using Blazored.LocalStorage;
using GoogleCaptchaComponent;
using GoogleCaptchaComponent.Configuration;
using GoogleCaptchaComponent.Models;
using Blazored.SessionStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7266") }); //https://maew123api.azurewebsites.net

//builder.Services.AddGoogleCaptcha(configuration =>
//{
//    configuration.V2SiteKey = "6LcKAZ8pAAAAADv5cDEbqEfnOqU9faurLnv1MXQU";
//    configuration.V3SiteKey = "6LcKAZ8pAAAAADv5cDEbqEfnOqU9faurLnv1MXQU";
//    configuration.DefaultVersion = CaptchaConfiguration.Version.V2;
//    configuration.DefaultTheme = CaptchaConfiguration.Theme.Light;
//    configuration.DefaultLanguage = CaptchaLanguages.English;
//});

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICatagoryService, CatagoryService>();
builder.Services.AddScoped<IPromotionService, PromotionService>();
builder.Services.AddScoped<ITypeService, TypeService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IAdminService, AdminService>();

builder.Services.AddSweetAlert2();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
//builder.Services.AddAuthorizationCore(config =>
//{
//    config.AddPolicy("IsAdmin", policy => policy.RequireClaim("roles", "Admin"));
//});   

builder.Services.AddRadzenComponents();

await builder.Build().RunAsync();
