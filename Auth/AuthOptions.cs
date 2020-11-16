using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AidaCarParts.Auth
{
    public class AuthOptions
    {
        public const string ISSUER = "AidaCarParts";
        public const string AUDIENCE = "AidaCarPartsClient";
        const string KEY = "mysupersecret_secretkey!123";
        public const int LIFETIME = 1000;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
