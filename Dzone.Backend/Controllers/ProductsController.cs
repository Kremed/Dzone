using Dzone.Shared.Contracts.Products;

namespace Dzone.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(DzoneDbContext context) : ControllerBase
    {
        [HttpGet("startData")]
        public async Task<IActionResult> StartData()
        {
            try
            {

                StartDataDto data = new()
                {
                    categories = await context.Categories.Where(c => c.IsActive).AsNoTracking().ToListAsync(),
                    products = await context.Products.Where(c => c.IsActive).AsNoTracking().ToListAsync(),

                };

                //return Ok(data);


                var CategoriesAndProducts = await context.Categories.Where(c => c.IsActive)
                                                  .Include(p => p.Products.Where(p => p.IsActive))
                                                    .ThenInclude(p => p.Category)
                                                  .Include(p => p.Products.Where(p => p.IsActive))
                                                    .ThenInclude(p=>p.OrderContents)
                                                  .ToListAsync();

                return Ok(CategoriesAndProducts);
            }
            catch (Exception exception)
            {
                return Problem(exception.Message);
            }
        }

        [HttpGet("getCategoryById")]
        public async Task<IActionResult> CategoryById([FromBody] string categoryId)
        {
            try
            {
                var seelectedItem = await context.Categories
                                           .FirstOrDefaultAsync(c => c.Id == categoryId &&
                                                                     c.IsActive == true);

                if (seelectedItem is not null)
                    return Ok(seelectedItem);

                return NotFound("لم يتم العثور علي التصنيف المطلوب, الرجاء أعادة المحاولة");
            }
            catch (Exception exception)
            {
                return Problem(exception.Message);
            }
        }

        [HttpGet("getProductById")]
        public async Task<IActionResult> ProductById([FromBody] string productId)
        {
            try
            {
                var seelectedItem = await context.Products
                                             .FirstOrDefaultAsync(c => c.Id == productId &&
                                                                       c.IsActive == true);

                if (seelectedItem is not null)
                    return Ok(seelectedItem);

                return NotFound("لم يتم العثور علي الصنف المطلوب, الرجاء أعادة المحاولة");
            }
            catch (Exception exception)
            {
                return Problem(exception.Message);
            }
        }


    }
}
