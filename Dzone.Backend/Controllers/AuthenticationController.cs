
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;

namespace Dzone.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        //يُستخدم لإدارة عمليات المستخدمين مثل التسجيل، تسجيل الدخول، البحث عن المستخدمين، وما إلى ذلك.
        private readonly UserManager<MyCustomAppUser> userManager;
        //يُستخدم لإدارة الأدوار في النظام، مثل إضافة أو حذف الأدوار وإسنادها للمستخدمين.
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;

        public AuthenticationController(
               UserManager<MyCustomAppUser> userManager,
               RoleManager<IdentityRole> roleManager,
               IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterContract model)
        {

            var user = new MyCustomAppUser
            {
                UserName = model.name,
                Email = model.email,
                PhoneNumber = model.phoneNumber
            };

            //Create User=>
            var result = await userManager.CreateAsync(user, model.password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);



            if (model.UserType == "AppUser")
            {
                var addToRoleResult = await userManager.AddToRoleAsync(user, "AppUser");
            }
            else if (model.UserType == "Captain")
            {
                var addToRoleResult2 = await userManager.AddToRoleAsync(user, "Captain");
            }
            else
            {
                await userManager.DeleteAsync(user);
                return BadRequest("الرجاء تحديد نوع المستخدم.");
            }


            return Ok("تم أنشاء المستخدم بنجاح, الرجاء تسجيل دخولك الأن.");
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginContract model)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(model.email);

                if (user is null)
                    return Unauthorized("فشلت عملية المصادقة الرجاء التـأكد من بياناتكـ.");

                var isSucssesLoginResult = await userManager.CheckPasswordAsync(user, model.password);

                if (isSucssesLoginResult)
                {
                    var authClaims = new List<Claim>{
                                     new Claim(ClaimTypes.Name, user.UserName!),
                                     new Claim(ClaimTypes.NameIdentifier, user.Id!),
                                     new Claim(ClaimTypes.MobilePhone, user.PhoneNumber!)};

                    var userRoles = await userManager.GetRolesAsync(user);

                    foreach (var role in userRoles)
                        authClaims.Add(new Claim(ClaimTypes.Role, role));

                    var token = GetToken(authClaims);

                    var tokenResponce = new LoginResponce();

                    tokenResponce.token = new JwtSecurityTokenHandler().WriteToken(token);

                    tokenResponce.expiration = token.ValidTo;

                    return Ok(tokenResponce);
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost("createSystemRoles")]
        public async Task<IActionResult> CreateSystemRoles()
        {
            var GetAllRolesAsync = await roleManager.Roles.ToListAsync();

            if (!await roleManager.RoleExistsAsync("AppUser"))
            {
                var role = new IdentityRole { Name = "AppUser" };
                await roleManager.CreateAsync(role);
            }

            if (!await roleManager.RoleExistsAsync("Captain"))
            {
                await roleManager.CreateAsync(new IdentityRole { Name = "Captain" });
            }

            return Ok();
















            // var FindByRoleName = await roleManager.FindByNameAsync("Captain");

            //if (FindByRoleName is not null)
            //{

            //    FindByRoleName.Name = "[captain]";

            //    var UpdateRsult = await roleManager.UpdateAsync(FindByRoleName);

            //    //var DeleteRsult = await roleManager.DeleteAsync(FindByRoleName);

            //    var GetAllRolesAsync = await roleManager.Roles.ToListAsync();

            //    FindByRoleName.Name = "Captain";

            //    UpdateRsult = await roleManager.UpdateAsync(FindByRoleName);
            //}


        }

        [HttpGet("GetName")]
        [Authorize]
        public async Task<IActionResult> GetName()
        {
            return Ok("Hi From Endpoint");
        }


        [SwaggerIgnore]
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }

      
    }
}
