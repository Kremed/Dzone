namespace Dzone.Backend.Data
{
    public class MyCustomAppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
    }
}
