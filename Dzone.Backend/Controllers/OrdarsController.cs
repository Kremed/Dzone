using Dzone.Shared.Contracts.Ordars;

namespace Dzone.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdarsController : ControllerBase
    {
        private readonly DzoneDbContext context;
        private readonly UserManager<MyCustomAppUser> userManager;

        public OrdarsController(DzoneDbContext context, UserManager<MyCustomAppUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [HttpPost("createOrder")]
        [Authorize(Roles = "AppUser")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderContract orderContract)
        {
            try
            {
                var userNameIdentifier = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userNameIdentifier is null)
                    return Unauthorized();

                var selectedUser = await userManager.FindByIdAsync(userNameIdentifier);
                if (selectedUser is null)
                    return Unauthorized();

                var selectedLocation = await context.Locations.FirstOrDefaultAsync(l => l.Id == orderContract.locationId);
                if (selectedLocation is null)
                    return BadRequest("الرجاء تحديد موقع الطلب, الموقع المختار غير موجود");

                if (orderContract.orderContents.Count() == 0)
                    return BadRequest("الرجاء أضافة أصناف للطلب, لايمكن ان تكون سلة الاصناف فارغة");

                //01) Set Order Values =>
                Ordar ordar = new Ordar();
                ordar.Id = Guid.NewGuid().ToString();
                ordar.StartTime = DateTime.Now;
                ordar.UserId = selectedUser.Id;
                ordar.LocationId = selectedLocation.Id;
                ordar.Status = "غير مكتملة";
                ordar.Notes = orderContract.notes;

                ordar.IsPaid = false;

                //To Do => Initi Payment Link Tlync Payment Gateway
                ordar.PaymentLink = "";

                //============================================================================================================
                //02) Check Order Contents Values =>

                List<OrderContent> orderContents = new();

                foreach (var item in orderContract.orderContents)
                {
                    OrderContent orderItem = new();

                    var prodactFromDb = await context.Products.FindAsync(item.productId);

                    if (prodactFromDb is null)
                        return BadRequest("احد اصناف السلة غير موجود, الرجاء تـأكيد البيانات وأعادة أرسال الطلب.");

                    if (item.quantity == 0)
                        return BadRequest($"لايمكن أضافة صنف بكمية صفرية, الرجاء تـاكيد كمية الصنف : {prodactFromDb.Name}");

                    orderItem.OrderId = ordar.Id;

                    orderItem.Quantity = item.quantity;
                    orderItem.Note = item.note;

                    orderItem.ProductId = prodactFromDb.Id;
                    orderItem.Price = prodactFromDb.SellPrice;
                }

                ordar.Total = orderContract.orderContents.Sum(i => i.price * i.quantity);

                //03) Save Data In DB =>
                await context.Ordars.AddAsync(ordar);

                await context.OrderContents.AddRangeAsync(orderContents);

                await context.SaveChangesAsync();

                return Ok("تمت عملية أضافة الطلب بنجاح");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("getOrdersArchive")]
        [Authorize(Roles = "AppUser")]
        public async Task<IActionResult> GetOrdersArchive()
        {
            try
            {
                var userNameIdentifier = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userNameIdentifier is null)
                    return Unauthorized();

                var ordersArchive = await context.Ordars
                                    .Where(order => order.UserId == userNameIdentifier && order.IsPaid == true)
                                    .Include(c => c.OrderContents)
                                    .Include(l => l.Location)
                                    .ToListAsync();

                //var ordersArchive = await context.Ordars
                //                  .Where(order => order.UserId == userNameIdentifier && order.IsPaid == true)
                //                  .Include(c => c.OrderContents)
                //                      .ThenInclude(oc => oc.Product)
                //                      .ThenInclude(p => p.Category)
                //                  .Include(c => c.User)
                //                  .Include(c => c.Captain)
                //                  .ToListAsync();

                return Ok(ordersArchive);
            }
            catch (Exception exception)
            {
                return Problem(exception.Message);
            }
        }

        [HttpGet("getOrderContents")]
        [Authorize(Roles = "AppUser")]
        public async Task<IActionResult> GetOrderContents([FromBody] string orderID)
        {
            try
            {
                var userNameIdentifier = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userNameIdentifier is null)
                    return Unauthorized();

                var orderContents = await context.Ordars
                                    .Where(order => order.Id == orderID)
                                    .Include(c => c.Captain)
                                    .Include(l => l.Location)
                                    .Include(c => c.OrderContents)
                                        .ThenInclude(p => p.Product)
                                            .ThenInclude(g => g.Category)
                                    .ToListAsync();

                return Ok(orderContents);
            }
            catch (Exception exception)
            {
                return Problem(exception.Message);
            }
        }
    }
}