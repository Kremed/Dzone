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

        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
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

















//options =>
//{
//    // Password settings
//    options.Password.RequireDigit = true; // Requires at least one digit ('0'-'9')
//    options.Password.RequireLowercase = true; // Requires at least one lowercase letter ('a'-'z')
//    options.Password.RequireUppercase = true; // Requires at least one uppercase letter ('A'-'Z')
//    options.Password.RequireNonAlphanumeric = true; // Requires at least one non-alphanumeric character
//    options.Password.RequiredLength = 6; // Minimum length of 6 characters
//    options.Password.RequiredUniqueChars = 1; // At least one unique character
//}
