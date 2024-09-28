namespace Dzone.Backend.Data
{
    // Identity الكلاس مسؤول عن الاتصال بقاعدة البيانات ويتعامل مع الكلاسات\الجداول الخاصة بـ،
    public partial class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
