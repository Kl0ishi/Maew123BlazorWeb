global using Maew123.Models.Models;
global using Maew123.Models.Dtos;
global using Maew123.Api.Repositories.Contracts;
global using Maew123.Api.Repositories;

using Maew123.Api.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Maew123.Api.Services;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Maew123.Api;
using Maew123.Api.Services.MailService;
using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.Extensions.FileProviders;
using System.Runtime.InteropServices;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//before IIS
//builder.Services.AddDbContext<ItshopMaew123Context>(
//    options => options.UseSqlServer(builder.Configuration.GetConnectionString("Maew123ConnectionString")));
//After IIS
builder.Services.AddDbContext<ItshopMaew123Context>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Maew123ConnectionString");

    // Specify SQL Server authentication (User ID and Password)
    options.UseSqlServer(connectionString, sqlServerOptions =>
    {
        sqlServerOptions.EnableRetryOnFailure(); // Optional: Enable transient error resiliency
    });
});


builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMailService, MailService>();

builder.Services.Configure<RecaptchaSettings>(builder.Configuration.GetSection("RecaptchaSettings"));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductCatagoryRepository, ProductCatagoryRepository>();
builder.Services.AddScoped<IProductTypeRepository, ProductTypeRepository>();
builder.Services.AddScoped<IOtpEntityRepository, OtpEntityRepository>();
builder.Services.AddScoped<IProductStockRepository, ProductStockRepository>();
builder.Services.AddScoped<IPromotionRepository, PromotionRepository>();
builder.Services.AddScoped<IStatusRepository, StatusRepository>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ProductService>();

builder.Services.AddScoped<IReportService, ReportService>();

// Determine the path to libwkhtmltox.dll based on the platform architecture
//string basePath = Path.Combine(builder.Environment.ContentRootPath);

//// Set the paths for the x64 and x86 directories
//string x64Path = Path.Combine(basePath, "x64");
//string x86Path = Path.Combine(basePath, "x86");

//// Determine the appropriate path based on the platform architecture
//string wkhtmltoxPath;
//if (Environment.Is64BitOperatingSystem)
//{
//    // If running on a 64-bit system, use the x64 directory
//    wkhtmltoxPath = Path.Combine(x64Path, "libwkhtmltox.dll");
//}
//else
//{
//    // If running on a 32-bit system, use the x86 directory
//    wkhtmltoxPath = Path.Combine(x86Path, "libwkhtmltox.dll");
//}
//try
//{
//    // Load the wkhtmltopdf binary
//    var context = new CustomAssemblyLoadContext();
//    context.LoadUnmanagedLibrary(wkhtmltoxPath);
//}
//catch (Exception ex)
//{
//    throw;
//}

var architectureFolder = (IntPtr.Size == 8) ? "64 bit" : "32 bit";
var wkHtmlToPdfPath = Path.Combine(builder.Environment.ContentRootPath, $"wkhtmltox\\v0.12.4\\{architectureFolder}\\libwkhtmltox");
CustomAssemblyLoadContext context = new CustomAssemblyLoadContext();
context.LoadUnmanagedLibrary(wkHtmlToPdfPath);

// Configure DinkToPdf
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

builder.Services.AddHttpContextAccessor();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
        .GetBytes(jwtSettings["SecretKey"])),
        ValidateIssuer = false,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = false,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();

//ทำให้ฝั่ง client ทำงานด้วยได้ ให้ผ่านเงื้อนไขความปลอดภัย
app.UseCors(policy =>
    policy.WithOrigins("http://localhost:7156", "https://localhost:7156", "https://proud-bush-0bbdc4f00-preview.eastasia.5.azurestaticapps.net")
    .AllowAnyMethod()
    .AllowAnyHeader()
    //.WithHeaders(HeaderNames.ContentType)
    );

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(app.Environment.ContentRootPath, "ZStores", "Images", "Products")),
    RequestPath = "/api/images/Products"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(app.Environment.ContentRootPath, "ZStores", "Images", "Payment")),
    RequestPath = "/api/images/Payment"
});

//ปิด httpsredirect เพื่อ IIS
//app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
