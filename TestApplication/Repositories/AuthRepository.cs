using Models;
using GlobalScopedVariables;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using TestApplication.Data;
using Utilities;

namespace TestApplication.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly IAuthUtilities _utilites;
        public AuthRepository(DataContext context, IAuthUtilities authUtilities)
        {
            _context = context;
            _utilites = authUtilities;
        }


        public async Task<string> LoginUser(LoginDto login)
        {
            var result = await _context.logins.FirstOrDefaultAsync(a => a.UserName == login.UserName);
            if (result == null)
            {
                return "No User Found";            }

            if (! (await _utilites.VerifyPasswordHash(login.Password, result.PasswordHash, result.PasswordSalt)))
            {
                return "Wrong Password";
            }

            string token = await _utilites.Token(result);

            return token;
        }

        public async Task<Login> UserRegister(LoginDto login)
        {
            await _utilites.CreatePasswordHash(login.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var LoginUser = new Login();
            LoginUser.UserName = login.UserName;
            LoginUser.PasswordHash = passwordHash;
            LoginUser.PasswordSalt = passwordSalt;
            await _context.logins.AddAsync(LoginUser);
            await _context.SaveChangesAsync();

            return LoginUser;
        }
    }
}
