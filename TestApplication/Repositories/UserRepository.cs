using Models;
using Microsoft.EntityFrameworkCore;
using TestApplication.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace TestApplication.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Users> AddUser(Users user)
        {
            var result = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Users> DeleteUser(int Id)
        {
            var result = await _context.Users.Where(a => a.Id == Id).FirstOrDefaultAsync();
            if(result != null)
            {
                _context.Users.Remove(result);
                await _context.SaveChangesAsync();
            }
            return null;
        }

        public async Task<Users> GetUser(int Id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == Id);
        }
        public async Task<IEnumerable<Users>> GetUserByDto()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<IEnumerable<Users>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<IEnumerable<UsersDto>> GetUsersForDto()
        {
            return await _context.Users
                .ProjectTo<UsersDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<Users> UpdateUser(Users user)
        {
            var result = await _context.Users.FirstOrDefaultAsync(a => a.Id == user.Id);
            if(result != null)
            {
                result.Name = user.Name;
                result.Age = user.Age;
                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }

        Task<Users> IUserRepository.GetUserByDto()
        {
            throw new NotImplementedException();
        }
    }
}
