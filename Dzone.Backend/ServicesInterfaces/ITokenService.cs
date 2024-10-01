namespace Dzone.Backend.ServicesInterfaces
{
    public interface ITokenService
    {
        LoginResponce CreateTokenFromClaims(List<Claim> authClaims);
    }
}
