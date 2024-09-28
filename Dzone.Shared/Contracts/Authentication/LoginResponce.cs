namespace Dzone.Shared.Contracts.Authentication
{
    public class LoginResponce
    {
        public string token { get; set; } = null!;
        public DateTime expiration { get; set; }
    }
}
