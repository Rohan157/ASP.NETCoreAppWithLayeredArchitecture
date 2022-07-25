using Models;

namespace TestApplication.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<Users>> GetUsers();
        Task<Users> GetUser(int Id); 
        Task<Users> GetUserByDto();
        Task<Users> AddUser(Users user);
        Task<Users> UpdateUser(Users user);
        Task<Users> DeleteUser(int Id);
        Task<IEnumerable<UsersDto>> GetUsersForDto();
    }
}
