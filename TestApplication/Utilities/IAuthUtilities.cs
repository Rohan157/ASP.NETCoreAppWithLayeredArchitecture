using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public interface IAuthUtilities
    {
        Task<String> Token(Login login);
        Task<Login> CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        Task<bool> VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);

    }
}
