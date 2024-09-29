global using System.Text;
global using Microsoft.Win32;
global using Dzone.Backend.Data;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Extensions.Options;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.IdentityModel.Tokens;
global using Dzone.Shared.Contracts.Authentication;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Dzone.Backend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();


        //3. Register Identity in Program.cs
        //==========================================Identit=====================================================
        var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(ConnectionString));

        builder.Services.AddIdentity<MyCustomAppUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;    
            options.Password.RequireLowercase = false; 
            options.Password.RequireUppercase = false; 
            options.Password.RequireNonAlphanumeric = false; 
            options.Password.RequiredLength = 6; 
            //options.Password.RequiredUniqueChars = 6; // at least one unique character

            options.User.RequireUniqueEmail = true;
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ د ج ح خ ه ع غ ف ق ث ص ض ذ 12 3 3 4 4 5 5 6 6 7 7 8 8 9 8 9 9 09  ط ك م ن ت ا ل ب ي س ش ظ ز و ة ى لا لا ر ؤ ء ئ أ آ لأ لآ" + "أ آ إ ب ت ث ج ح خ د ذ ر ز س ش ص ض ط ظ ع غ ف ق ك ل م ن  ه  و ؤ ئ ي ء";

            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = true;
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

        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();



        //=========================================Identit======================================================
        app.UseAuthentication();

        app.UseAuthorization();
        //=========================================Identit======================================================


        app.MapControllers();

        app.Run();
    }
}

















