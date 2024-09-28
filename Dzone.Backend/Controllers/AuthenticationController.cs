
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Dzone.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        //يُستخدم لإدارة عمليات المستخدمين مثل التسجيل، تسجيل الدخول، البحث عن المستخدمين، وما إلى ذلك.
        private readonly UserManager<IdentityUser> userManager;
        //يُستخدم لإدارة الأدوار في النظام، مثل إضافة أو حذف الأدوار وإسنادها للمستخدمين.
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;

        public AuthenticationController(
            UserManager<IdentityUser> userManager,
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
            var user = new IdentityUser
            {
                UserName = model.name,
                Email = model.email,
                PhoneNumber = model.phoneNumber
            };

            //Create =>
            var result = await userManager.CreateAsync(user, model.password);

            //TO DO =>

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            ////Update =>
            //user.UserName = "LaKremed2";
            //var UpdateResult = await userManager.UpdateAsync(user);

            ////Search =>
            //var FindByIdResult = await userManager.FindByIdAsync(user.Id);

            //var FindByNameResult = await userManager.FindByNameAsync(user.UserName);

            //var FindByEmailResult = await userManager.FindByEmailAsync(user.Email);

            ////Delete =>
            //var DeleteResult = await userManager.DeleteAsync(user);

            return Ok("تم أنشاء المستخدم بنجاح, الرجاء تسجيل دخولك الأن.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginContract model)
        {
            var user = await userManager.FindByEmailAsync(model.email);

            if (user is null)
                return Unauthorized();

            var isSucssesLoginResult = await userManager.CheckPasswordAsync(user, model.password);

            if (isSucssesLoginResult)
            {
                var authClaims = new List<Claim>{
                                     new Claim("name", user?.UserName!),
                                     new Claim(ClaimTypes.Name, user?.UserName!),
                                     new Claim(ClaimTypes.NameIdentifier, user?.Id!),
                                     new Claim(ClaimTypes.MobilePhone, user?.PhoneNumber!),
                                 };



                var token = GetToken(authClaims);

                return Ok(new LoginResponce
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }

            return Unauthorized();
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
