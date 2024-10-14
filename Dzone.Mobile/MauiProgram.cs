global using Dzone.Mobile.Views.AuthViews;
global using Microsoft.Extensions.Logging;
global using Microsoft.Maui.Controls.Platform.Compatibility;
global using CommunityToolkit.Maui;
global using Maui.FreakyEffects;
global using UraniumUI;
global using SkiaSharp.Views.Maui.Controls.Hosting;
global using RestSharp;
global using Dzone.Shared;
global using Dzone.Mobile.Interfaces;
global using Dzone.Shared.Contracts.Ordars;
global using Dzone.Shared.Contracts.Products;
global using Dzone.Shared.Contracts.Authentication;
global using Dzone.Mobile.Services;
global using ErrorOr;
global using Dzone.Models.Shered;
global using CommunityToolkit.Maui.Alerts;

namespace Dzone.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            //Add NuGet Packages => CommunityToolkit.Maui
            //Add NuGet Packages => UraniumUI
            //Add NuGet Packages => UraniumUI.Material
            //Add NuGet Packages => Maui.FreakyEffects
            //Add NuGet Packages => RestSharp

            //.UseSkiaSharp()
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseUraniumUI()
                .UseUraniumUIMaterial()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("BalooBhaijaan.ttf", "MyFont");
                })
                .ConfigureEffects(effects =>
                {
                    effects.InitFreakyEffects();
                });

            //Add Services to IoC
            builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);

            builder.Services.AddSingleton<IRestClient>(sp =>
            {
                var options = new RestClientOptions("https://dzone.maui.ly/api/");

                var client = new RestClient(options);

                client.AddDefaultHeader("Accept", "application/json");

                return client;
            });

            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<IProductsService, ProductsService>();

            //Add Main Views to IoC=>
            builder.Services.AddTransient<AppShell>();
            builder.Services.AddSingleton<MainPage>();

            //Add Auth Views to IoC=>
            builder.Services.AddSingleton<LoginView>();
            builder.Services.AddSingleton<RegisterView>();
            builder.Services.AddSingleton<ConfirmOtpView>();
            builder.Services.AddSingleton<ForggetPasswordView>();

            return builder.Build();
        }
    }
}