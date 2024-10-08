using Dzone.Shared.Contracts.Products;

namespace Dzone.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdarsController : ControllerBase
    {
        private readonly DzoneDbContext context;

        public OrdarsController(DzoneDbContext context)
        {
            this.context = context;
        }


        [HttpGet("getOrdersArchive"), Authorize(Roles = "AppUser")]
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


        [HttpGet("getOrderContents"), Authorize(Roles = "AppUser")]
        public async Task<IActionResult> GetOrderContents([FromBody] string orderID)
        {
            try
            {
                var userNameIdentifier = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userNameIdentifier is null)
                    return Unauthorized();

                var orderContents = await context.Ordars
                                    .Where(order => order.Id == orderID)
                                    .Include(c => c.OrderContents)
                                        .ThenInclude(p => p.Product)
                                        .ThenInclude(g => g.Category)   
                                    .Include(c => c.Captain)
                                    //.Include(l=>l.Location)
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
