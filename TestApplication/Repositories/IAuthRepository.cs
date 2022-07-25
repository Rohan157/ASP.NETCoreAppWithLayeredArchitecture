using Models;

namespace TestApplication.Repositories
{
    public interface IAuthRepository
    {
        Task<Login> UserRegister(LoginDto login);
        Task<String> LoginUser(LoginDto login);
      
    }
}
