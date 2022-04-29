
using System.IdentityModel.Tokens.Jwt;


namespace UserService.Controllers
{
    internal class JwtTokenHelper
    {
        public string GetId(string bearerToken)
        {
            var token = bearerToken;
            var handler = new JwtSecurityTokenHandler();
            return handler.ReadJwtToken(token).Subject;
        }
    }
}
