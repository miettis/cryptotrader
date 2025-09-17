using Binance.Net;
using Binance.Net.Clients;
using CryptoExchange.Net.Authentication;
using CryptoTrader.Data;
using CryptoTrader.Data.Features;
using CryptoTrader.Data.Services;
using CryptoTrader.Web.Services;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

BinanceRestClient.SetDefaultOptions(options =>
{
    var apiKey = builder.Configuration.GetValue<string>("Binance:ApiKey");
    var secret = builder.Configuration.GetValue<string>("Binance:Secret");
    //options.ApiCredentials = new ApiCredentials("", ""); // <- Provide you API key/secret in these fields to retrieve data related to your account
    options.ApiCredentials = new ApiCredentials(apiKey, secret); // <- Provide you API key/secret in these fields to retrieve data related to your account

});

builder.Logging.AddJsonConsole(options =>
{
    options.IncludeScopes = true;
    options.TimestampFormat = "HH:mm:ss ";
    options.JsonWriterOptions = new JsonWriterOptions
    {
        Indented = true
    };
});
builder.Logging.AddSystemdConsole(options => 
{
    options.IncludeScopes = true;
    options.TimestampFormat = "HH:mm:ss ";
});
builder.Services.AddDbContextFactory<BinanceContext>(x => x
    .UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
    .UseSnakeCaseNamingConvention());

builder.Services.AddControllersWithViews().AddJsonOptions(x => 
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddFastEndpoints();
builder.Services.AddBinance();
builder.Services.AddSingleton<ConfigService>();
builder.Services.AddSingleton<AccountInfoService>();
builder.Services.AddSingleton<OpenOrderService>();
builder.Services.AddSingleton<ExchangeInfoService>();
builder.Services.AddSingleton<OrderUpdateService>();
builder.Services.AddSingleton<TradingService>();
builder.Services.AddSingleton<PriceHourStreamService>();
builder.Services.AddSingleton<FeatureCalculationService>();
builder.Services.AddSingleton<PriceHourUpdateService>(); 
builder.Services.AddSingleton<PredictionService>();
builder.Services.AddSingleton<TradingService>();
builder.Services.AddSingleton<OrderRelationUpdateService>();
builder.Services.AddHostedService(x => x.GetRequiredService<AccountInfoService>());
builder.Services.AddHostedService(x => x.GetRequiredService<OpenOrderService>());
builder.Services.AddHostedService(x => x.GetRequiredService<ExchangeInfoService>());
builder.Services.AddHostedService(x => x.GetRequiredService<OrderUpdateService>());
builder.Services.AddHostedService(x => x.GetRequiredService<TradingService>());
builder.Services.AddHostedService(x => x.GetRequiredService<PriceHourUpdateService>());
builder.Services.AddHostedService(x => x.GetRequiredService<FeatureCalculationService>());
builder.Services.AddHostedService(x => x.GetRequiredService<PriceHourStreamService>());
builder.Services.AddHostedService(x => x.GetRequiredService<PredictionService>());
builder.Services.AddHostedService(x => x.GetRequiredService<TradingService>());
builder.Services.AddHostedService(x => x.GetRequiredService<OrderRelationUpdateService>());
//builder.Services.AddHostedService<PriceMinuteUpdateService>();
//builder.Services.AddHostedService<PriceMinuteStreamService>();
builder.Services.AddTransient<FeatureCalculation>();
builder.Services.AddTransient<PythonService>(x => new PythonService("http://predict:80"));
builder.Services.AddOpenApiDocument();
builder.Services.AddCors();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseFastEndpoints();
app.UseOpenApi();
app.UseSwaggerUi();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapFallbackToFile("/index.html");
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseCors(x => x
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin()
);
app.Run();
