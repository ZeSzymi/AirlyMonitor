using AirlyInfrastructure.Contexts;
using AirlyInfrastructure.Models.Database;
using AirlyInfrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AirlyInfrastructure.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly AirlyDbContext _airlyDbContext;

        public UsersRepository(AirlyDbContext airlyDbContext)
        {
            _airlyDbContext = airlyDbContext;
        }

        public Task<List<User>> GetUsersAsync(List<string> userIds)
            => _airlyDbContext.Users.Where(user => userIds.Contains(user.Id)).ToListAsync();
    }
}
