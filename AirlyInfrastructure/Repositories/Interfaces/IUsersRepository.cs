using AirlyInfrastructure.Models.Database;

namespace AirlyInfrastructure.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        Task<List<User>> GetUsersAsync(List<string> userIds);
    }
}
