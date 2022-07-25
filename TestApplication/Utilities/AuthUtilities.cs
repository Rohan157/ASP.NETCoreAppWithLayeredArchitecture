using Models;
using GlobalScopedVariables;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;

namespace Utilities
{
    public class AuthUtilities : IAuthUtilities
    {
        public AuthUtilities()
        {
        }

        public Task<Login> CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return null;
            }
        }

        public async Task<string> Token(Login login)
        {
            Global newkey = new Global();
            string mykey = newkey.Jwtkey();

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, login.UserName),
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
              mykey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;

        }

        public async Task<bool> VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                bool result = (computedHash.SequenceEqual(passwordHash));
                return result;
            }
        } 
    }
}