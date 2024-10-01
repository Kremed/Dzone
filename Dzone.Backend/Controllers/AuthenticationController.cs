using Microsoft.AspNetCore.Identity;

namespace Dzone.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //(UserManager<MyCustomAppUser> userManager,RoleManager<IdentityRole> roleManager,ITokenService tokenService,IEmailService emailService)
    public class AuthenticationController : ControllerBase
    {
        //يُستخدم لإدارة عمليات المستخدمين مثل التسجيل، تسجيل الدخول، البحث عن المستخدمين، وما إلى ذلك.
        private readonly UserManager<MyCustomAppUser> userManager;
        //يُستخدم لإدارة الأدوار في النظام، مثل إضافة أو حذف الأدوار وإسنادها للمستخدمين.
        private readonly RoleManager<IdentityRole> roleManager;

        private readonly SignInManager<MyCustomAppUser> signInManager;
        //يُستخدم لإدارة رموز التعريف والعضوية في النظام, يمكن أضافة اي خاصة خاصة بالرموز في هذا المستودع.
        private readonly ITokenService tokenService;
        //يُستخدم لإدارة أرسال البريد الألكتروني الي المستخدمين او زوار.
        private readonly IEmailService emailService;
        private readonly IUserOtpService otpService;
        private readonly DzoneDbContext context;

        public AuthenticationController(
               UserManager<MyCustomAppUser> userManager,
               RoleManager<IdentityRole> roleManager,
               SignInManager<MyCustomAppUser> signInManager,
               DzoneDbContext context,
               IEmailService emailService,
               ITokenService tokenService,
               IUserOtpService otpService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.context = context;
            this.tokenService = tokenService;
            this.emailService = emailService;
            this.otpService = otpService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterContract model)
        {
            var user = new MyCustomAppUser
            {
                UserName = model.name,
                Email = model.email,
                PhoneNumber = model.phoneNumber,
                FirstName = model.name,
                LastName = model.name,
                Location = "اي حاجة في الحمادة"
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

            var otpCode = await otpService.CreateEmailOtpCode(user.Id);

            var isEmailSent = await emailService.SendConfirmationEmail(otp: otpCode.ToString(), email: user.Email);

            if (!isEmailSent)
            {
                await userManager.DeleteAsync(user);
                return BadRequest("لم تتم عملية أرسال بريد التحقق, الرجاء أعادة المحاولة مرة اخرئ.");
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

                //var isSucssesLoginResult = await userManager.CheckPasswordAsync(user, model.password);

                var isSucssesLoginResult = await signInManager.PasswordSignInAsync(user.UserName!, model.password, isPersistent: false, lockoutOnFailure: true);
                
                if (isSucssesLoginResult.IsNotAllowed)
                {
                    var otpCode = await otpService.CreateEmailOtpCode(user.Id);

                    var isEmailSent = await emailService.SendConfirmationEmail(otp: otpCode.ToString(), email: user.Email!);

                    return BadRequest("الرجاء تــأكيد بريدك الألكتروني, لايمكن تسجيل الدخول قبل تــأكيد امتلاكك للبريد الألكترونية");
                }
                else if (isSucssesLoginResult.IsLockedOut)
                {
                    return BadRequest("حسابك غير مفعل لانك حاولت تسجيل الدخول لاكثر من مرة بطريقة خاطئة, الرجاء الانتظار قليلا واعادة المحاولة مرة اخرئ");
                }
                else if (isSucssesLoginResult.Succeeded)
                {
                    var authClaims = new List<Claim>{
                                     new Claim(ClaimTypes.Name, user.UserName!),
                                     new Claim(ClaimTypes.NameIdentifier, user.Id!),
                                     new Claim(ClaimTypes.MobilePhone, user.PhoneNumber!)};

                    var userRoles = await userManager.GetRolesAsync(user);

                    foreach (var role in userRoles)
                        authClaims.Add(new Claim(ClaimTypes.Role, role));

                    var token = tokenService.CreateTokenFromClaims(authClaims);

                    return Ok(token);
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailContract model)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(model.email);

                if (user is null)
                    return BadRequest("فشلت عملية التحقق");

                var result = await otpService.IsValidEmailOtpCode(code: model.code, userID: user.Id);

                if (result is true)
                {
                    //userManager.CreateSecurityTokenAsync(,);
                    //userManager.ConfirmEmailAsync(,);

                    var SelectedUser = await context.AspNetUsers.FirstOrDefaultAsync(u => user.Email == user.Email);

                    SelectedUser!.EmailConfirmed = true;

                    await context.SaveChangesAsync();

                    return Ok("تمت عملية التحقق بنجاح, لقد تم أعتماد بريدك الألكتروني وتفعيله");
                }

                return BadRequest("فشلت عملية التحقق");
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


    }
}
