
namespace Dzone.Backend.ServicesRepositories
{
    public class JwtTokenService(IConfiguration configuration) : ITokenService
    {
        public LoginResponce CreateTokenFromClaims(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            var tokenResponce = new LoginResponce();

            tokenResponce.token = new JwtSecurityTokenHandler().WriteToken(token);

            tokenResponce.expiration = token.ValidTo;

            return tokenResponce;
        }
    }
}
