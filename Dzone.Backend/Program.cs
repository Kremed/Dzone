global using Dzone.Backend.Data;
global using Dzone.Backend.ServicesInterfaces;
global using Dzone.Backend.ServicesRepositories;
global using Dzone.Models.Shered;
global using Dzone.Shared.Contracts.Authentication;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.IdentityModel.Tokens;
global using Newtonsoft.Json;
global using Newtonsoft.Json.Serialization;
global using System.IdentityModel.Tokens.Jwt;
global using System.Net;
global using System.Net.Mail;
global using System.Security.Claims;
global using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Dzone.Backend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers()
                        .AddNewtonsoftJson(options =>
                        {
                            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        });

        //3. Register Identity in Program.cs
        //==========================================Identit=====================================================
        var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<DzoneDbContext>(options => options.UseSqlServer(ConnectionString));
        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(ConnectionString));

        builder.Services.AddIdentity<MyCustomAppUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;

            options.User.RequireUniqueEmail = true;
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ د ج ح خ ه ع غ ف ق ث ص ض ذ 12 3 3 4 4 5 5 6 6 7 7 8 8 9 8 9 9 09  ط ك م ن ت ا ل ب ي س ش ظ ز و ة ى لا لا ر ؤ ء ئ أ آ لأ لآ" + "أ آ إ ب ت ث ج ح خ د ذ ر ز س ش ص ض ط ظ ع غ ف ق ك ل م ن  ه  و ؤ ئ ي ء";

            options.SignIn.RequireConfirmedEmail = true;
            options.SignIn.RequireConfirmedPhoneNumber = false;
            //options.SignIn.RequireConfirmedAccount = true;

            options.Lockout.MaxFailedAccessAttempts = 3;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
            options.Lockout.AllowedForNewUsers = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        //JWT support =>
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
            };
        });
        //==========================================Identit=====================================================

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Dzone API Gateway",
                Version = "v1",
                Description = "DZone’s API Gateway is an architectural component that simplifies communication between clients and microservices by centralizing routing, security, and protocol management. It helps improve security by authenticating requests before they reach backend services, offloading traffic management, and enabling advanced features like rate limiting, protocol translation, caching, and load balancing. This layer ensures that clients don’t need to handle the complexities of service discovery or multiple APIs and protocols independently, leading to better performance, security, and scalability in distributed systems​\r\n.",
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT",
                Description = "Enter 'Bearer [space] and then your token'",
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                }
            });

            options.EnableAnnotations();

            //var xmlFile = $"ApiDocumentation.xml";
            //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            //options.IncludeXmlComments(xmlPath);
        });

        //Objects : instance-based injection or factory method injection=>
        builder.Services.AddTransient<MailMessage>(provider =>
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("info@maui.ly", "DZone system"),
                Subject = "Dzone OTP - Email Confirmation",
                IsBodyHtml = true
            };

            return mailMessage;
        });

        builder.Services.AddSingleton<SmtpClient>(opt =>
        {
            var client = new SmtpClient("maui.ly", 587)
            {
                Credentials = new NetworkCredential("info@maui.ly", "Mfqb48!53"),
            };

            return client;
        });

        //Services Implementation =>
        builder.Services.AddSingleton<IEmailService, EmailService>();

        builder.Services.AddSingleton<ITokenService, JwtTokenService>();

        builder.Services.AddTransient<IUserOtpService, UserOtpService>();

        var app = builder.Build();

        //if (app.Environment.IsDevelopment())
        //{
        //}
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        //=========================================Identit======================================================
        app.UseAuthentication();

        app.UseAuthorization();
        //=========================================Identit======================================================

        app.MapControllers();

        app.Run();
    }
}